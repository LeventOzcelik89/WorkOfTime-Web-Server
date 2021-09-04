using GeoAPI.Geometries;
using NetTopologySuite.Features;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text.RegularExpressions;

namespace Infoline.Framework.Database.Mssql
{

    public class FeatureCollectionExt : FeatureCollection
    {
        public TableInfo TableInfo { get; set; }


        public FeatureCollectionExt()
        {

        }

        public FeatureCollectionExt(FeatureCollection fc, TableInfo ti)
        {
            foreach (var f in fc.Features)
            {
                this.Add(f);
            }

            TableInfo = ti;
        }

    }

    class MssqlQueryExecutor : IQueryExecutor
    {
        private SqlConnection _connection;
        private ITypeMapper _typeMapper;
        private SqlTransaction _transaction;

        public static Action<Query, ResultStatus> OnExecuionComplate { get; set; }

        public string ServerName { get { return _connection.DataSource; } }
        public string DbName { get { return _connection.Database; } }


        public bool IsTransactionOpen { get { return _transaction != null && _transaction.Connection != null; } }

        public string ConnectionString { get { return _connection.ConnectionString; } }

        public MssqlQueryExecutor(string connectionString, ITypeMapper typeMapper, DbTransaction transaction = null)
        {
            _typeMapper = typeMapper;
            _transaction = (SqlTransaction)transaction;
            _connection = _transaction == null || _transaction.Connection == null ?
                                new SqlConnection(connectionString) :
                                _transaction.Connection;
        }

        public void Dispose()
        {
            if (_connection != null && (_transaction == null || _transaction.Connection == null))
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }

        public T ExecuteScaler<T>(Query query)
        {
            using (var cmd = GetCommand(query))
            {
                var now = DateTime.Now;
                var geoReader = new NetTopologySuite.IO.WKBReader();
                var ret = cmd.ExecuteScalar();

                System.Diagnostics.Debug.WriteLine((DateTime.Now - now) + " " + WriteLog(query.Command, query.Parameters));
                return ret == null || ret == DBNull.Value ? default(T) : ret.GetType().Name == "SqlGeography" || ret.GetType().Name == "SqlGeometry" ? (T)geoReader.Read((ret.GetType().GetMethod("STAsBinary").Invoke(ret, null) as System.Data.SqlTypes.SqlBytes).Value) : (T)ret;

            }
        }

        public FeatureCollection ExecuteFeature(Query query)
        {
            var collection = new FeatureCollection();
            var envelope = new Envelope();

            using (var cmd = GetCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IGeometry geometry = null;
                        IAttributesTable table = new AttributesTable();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            try
                            {
                                var val = _typeMapper.ConvertFromSql(reader.GetValue(i));
                                var name = reader.GetName(i);
                                if (name == "__id")
                                    name = "id";

                                if (!(val is IGeometry))
                                    table.AddAttribute(name, val);
                                else
                                {
                                    geometry = (IGeometry)val;
                                    envelope = envelope.ExpandedBy(geometry.EnvelopeInternal);
                                }

                            }
                            catch
                            {

                            }
                        }
                        collection.Add(new Feature(geometry, table));
                    }
                }
            }
            collection.BoundingBox = envelope;
            return collection;
        }
        public FeatureCollectionExt ExecuteFeature2(Query query)
        {
            var collection = new FeatureCollectionExt();
            var envelope = new Envelope();
            collection.TableInfo = new TableInfo();

            using (var cmd = GetCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    var stbl = reader.GetSchemaTable().Rows.Cast<DataRow>().OrderBy(a => a["ColumnOrdinal"]).ToArray();

                    for (var i = 0; i < stbl.Length; i++)
                    {
                        var type = stbl[i]["DataType"] as Type;
                        var name = stbl[i]["ColumnName"].ToString();
                        var typename = stbl[i]["DataTypeName"].ToString();
                        var len = (int)stbl[i]["ColumnSize"];
                        if (typename.EndsWith("geography") || typename.EndsWith("geometry"))
                            continue;

                        collection.TableInfo.Columns.Add(new TableColumn { ColumnName = name, Type = type, Length = type.Name.ToLower() == "string" ? len : 10 });
                    }


                    while (reader.Read())
                    {
                        IGeometry geometry = null;
                        IAttributesTable table = new AttributesTable();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            try
                            {
                                var val = _typeMapper.ConvertFromSql(reader.GetValue(i));
                                var name = reader.GetName(i);
                                if (name == "__id")
                                    name = "id";

                                if (!(val is IGeometry))
                                    table.AddAttribute(name, val);
                                else
                                {
                                    geometry = (IGeometry)val;
                                    envelope = envelope.ExpandedBy(geometry.EnvelopeInternal);
                                }

                            }
                            catch
                            {

                            }
                        }
                        collection.Add(new Feature(geometry, table));
                    }
                }
            }
            collection.BoundingBox = envelope;
            return collection;
        }

        public IEnumerable<T> ExecuteReader<T>(Query query)
        {
            using (var cmd = GetCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    var now = DateTime.Now;
                    while (reader.Read())
                    {
                        if (IsAnonymous(typeof(T)))
                        {
                            var args = new List<object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var val = _typeMapper.ConvertFromSql(reader.GetValue(i));
                                args.Add(val);
                            }
                            T ret = (T)Activator.CreateInstance(typeof(T), args.ToArray());
                            yield return ret;
                        }
                        else
                        {
                            T ret = (T)Activator.CreateInstance(typeof(T));
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                try
                                {
                                    var val = _typeMapper.ConvertFromSql(reader.GetValue(i));
                                    var info = ret.GetType().GetProperty(reader.GetName(i));
                                    if (info != null)
                                        info.SetValue(ret, val, null);
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine(ex.Message);
                                }
                            }
                            yield return ret;
                        }
                    }
                    System.Diagnostics.Debug.WriteLine((DateTime.Now - now) + " " + WriteLog(query.Command, query.Parameters));
                }
            }
            OnExecuionComplate?.Invoke(query, null);
        }

        public IEnumerable<Dictionary<string, object>> ExecuteReader(Query query)
        {
            using (var cmd = GetCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    var now = DateTime.Now;
                    var geoReader = new NetTopologySuite.IO.WKBReader();
                    while (reader.Read())
                    {
                        Dictionary<string, object> ret = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var name = reader.GetName(i);
                            var value = _typeMapper.ConvertFromSql(reader.GetValue(i));
                            ret[name] = value;
                        }
                        yield return ret;
                    }
                    System.Diagnostics.Debug.WriteLine((DateTime.Now - now) + " " + WriteLog(query.Command, query.Parameters));
                }
            }
            OnExecuionComplate?.Invoke(query, null);
        }

        public ResultStatus ExecuteNonQuery(Query query)
        {
            try
            {
                using (var cmd = GetCommand(query))
                {
                    cmd.CommandTimeout = 10000000;
                    var now = DateTime.Now;
                    var tr = cmd.ExecuteNonQuery();
                    var status = new ResultStatus
                    {
                        message = tr.ToString(),
                        result = true
                    };
                    OnExecuionComplate?.Invoke(query, status);
                    System.Diagnostics.Debug.WriteLine((DateTime.Now - now) + " " + WriteLog(query.Command, query.Parameters));
                    return status;
                }
            }
            catch (Exception ex)
            {
                return new ResultStatus
                {

                    message = String.Format(query.Command, query.Parameters) + System.Environment.NewLine + "UnSuccessfull - " + ex.ToString(),
                    result = false
                };
            }
        }

        public bool IsSupportBulkInsert { get { return true; } }

        public ResultStatus ExecuteBulkInsert(string tableName, string schemaName, FeatureCollection parametre, TableInfo tableInfo, string geomColName)
        {
            var partCount = 1000;
            List<IFeature[]> featureParts = new List<IFeature[]>();
            for (var i = 0; i < (int)Math.Ceiling((double)parametre.Features.Count / partCount); i++)
                featureParts.Add(parametre.Features.Skip(i * partCount).Take(partCount).ToArray());

            foreach (var features in featureParts)
            {
                var dt = new DataTable();
                foreach (var feature in features)
                {
                    var row = dt.NewRow();
                    foreach (var name in feature.Attributes.GetNames().Union(new[] { geomColName }))
                    {
                        if (tableInfo != null &&
                            name != "__id" &&
                            name != "shape" &&
                            !tableInfo.Columns.Any(a => a.ColumnName == name))
                            continue;
                        object value;
                        if (name != geomColName) value = feature.Attributes[name];
                        else value = Microsoft.SqlServer.Types.SqlGeography.STGeomFromWKB(new System.Data.SqlTypes.SqlBytes(feature.Geometry.AsBinary()), 4326);

                        if (!dt.Columns.Contains(name))
                        {
                            DataColumn column;
                            if (value == null)
                                column = new DataColumn(name);
                            else
                            {
                                var type = value.GetType();
                                if (typeof(Guid).IsAssignableFrom(type))
                                    column = new DataColumn(name, typeof(Guid));
                                else if (typeof(DateTime).IsAssignableFrom(type))
                                    column = new DataColumn(name, typeof(DateTime));
                                else if (typeof(Boolean).IsAssignableFrom(type))
                                    column = new DataColumn(name, typeof(Boolean));
                                else if (typeof(Int32).IsAssignableFrom(type))
                                    column = new DataColumn(name, typeof(Int32));
                                else if (typeof(Microsoft.SqlServer.Types.SqlGeography).IsAssignableFrom(type))
                                    column = new DataColumn(name, typeof(Microsoft.SqlServer.Types.SqlGeography));
                                else column = new DataColumn(name);
                            }
                            //var column = DatColumn(value.GetType(), pname);
                            dt.Columns.Add(column);
                        }
                        row[name] = value;
                    }
                    dt.Rows.Add(row);
                }

                try
                {
                    using (var copy = new SqlBulkCopy(_connection.ConnectionString))
                    {
                        foreach (DataColumn item in dt.Columns)
                            copy.ColumnMappings.Add(item.ColumnName, item.ColumnName);

                        copy.BulkCopyTimeout = 1000000000;
                        copy.DestinationTableName = tableName;
                        copy.WriteToServer(dt);
                    }
                }
                catch (Exception ex)
                {
                    return new ResultStatus
                    {
                        message = "UnSuccessfull - " + ex,
                        result = false
                    };
                }
            }
            return new ResultStatus() { message = "Ok..", result = true };
        }

        public DbTransaction BeginTransaction()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        public void Close()
        {
            if (_connection.State != ConnectionState.Closed)
                _connection.Close();
        }

        private static bool IsAnonymous(Type type)
        {
            if (type.IsGenericType)
            {
                var d = type.GetGenericTypeDefinition();
                if (d.IsClass && d.IsSealed && d.Attributes.HasFlag(TypeAttributes.NotPublic))
                {
                    var attributes = d.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false);
                    if (attributes != null && attributes.Length > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        private string WriteLog(string command, QueryParameter[] parameters)
        {
            try
            {
                foreach (var item in parameters)
                {
                    var regexm = new Regex(Regex.Escape(item.Name));
                    command = regexm.Replace(command, (item.Value == null ? " IS NULL " : string.Format("'{0}'", item.Value)), 1);
                }
            }
            catch{}
           
            return command;
        }



        private SqlCommand GetCommand(Query query)
        {

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            var cmd = _connection.CreateCommand();
            if (_transaction != null && _transaction.Connection != null)
            {
                cmd.Transaction = _transaction;
            }

            cmd.CommandTimeout = 10000000;
            cmd.CommandText = query.Command;

            if (query.Parameters != null)
            {
                foreach (var parameter in query.Parameters)
                {
                    cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
                }
            }

            return cmd;

        }

    }
}
