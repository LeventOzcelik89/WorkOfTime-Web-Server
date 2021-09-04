using GeoAPI.Geometries;
using NetTopologySuite.Features;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Database.Postgis
{
    class PostgisQueryExecutor : IQueryExecutor
    {
        NpgsqlConnection _connection;
        ITypeMapper _typeMapper;
        NpgsqlTransaction _transaction;
        public static Action<Query, ResultStatus> OnExecuionComplate { get; set; }

        public string ServerName { get { return _connection.DataSource; } }
        public string DbName { get { return _connection.Database; } }

        public bool IsTransactionOpen { get { return _transaction != null && _transaction.Connection != null; } }

        public string ConnectionString { get { return _connection.ConnectionString; } }

        public PostgisQueryExecutor(string connectionString, ITypeMapper typeMapper, DbTransaction transaction = null)
        {
            _typeMapper = typeMapper;
            _transaction = (NpgsqlTransaction)transaction;
            _connection = _transaction == null || _transaction.Connection == null ?
                                new NpgsqlConnection(connectionString) :
                                _transaction.Connection;

        }

        public void Dispose()
        {
            if (_transaction == null)
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
                var ret = cmd.ExecuteScalar();
                return ret == DBNull.Value ? default(T) : (T)Convert.ChangeType(ret, typeof(T));
            }
        }

        public FeatureCollection ExecuteFeature(Query query)
        {
            using (var cmd = GetCommand(query))
            {
                var collection = new FeatureCollection();
                var geoReader = new NetTopologySuite.IO.WKBReader();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        IGeometry geometry = null;
                        IAttributesTable table = new AttributesTable();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            object val;
                            if (!typeof(NpgsqlTypes.PostgisGeometry).IsAssignableFrom(reader.GetFieldType(i)))
                                val = _typeMapper.ConvertFromSql(reader.GetValue(i));
                            else val = !reader.IsDBNull(i) ? geoReader.Read(reader.GetFieldValue<byte[]>(i)) : null;

                            if (!(val is IGeometry))
                                table.AddAttribute(reader.GetName(i), val);
                            else geometry = (IGeometry)val;
                        }
                        collection.Add(new Feature(geometry, table));
                    }
                }
                return collection;
            }
        }

        public IEnumerable<T> ExecuteReader<T>(Query query)
        {
            using (var cmd = GetCommand(query))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    var geoReader = new NetTopologySuite.IO.WKBReader();
                    while (reader.Read())
                    {
                        if (IsAnonymous(typeof(T)))
                        {
                            var args = new List<object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                object val;
                                if (!typeof(NpgsqlTypes.PostgisGeometry).IsAssignableFrom(reader.GetFieldType(i)))
                                    val = _typeMapper.ConvertFromSql(reader.GetValue(i));
                                else val = !reader.IsDBNull(i) ? geoReader.Read(reader.GetFieldValue<byte[]>(i)) : null;
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
                                object val;
                                if (!typeof(NpgsqlTypes.PostgisGeometry).IsAssignableFrom(reader.GetFieldType(i)))
                                    val = _typeMapper.ConvertFromSql(reader.GetValue(i));
                                else val = !reader.IsDBNull(i) ? geoReader.Read(reader.GetFieldValue<byte[]>(i)) : null;
                                var info = ret.GetType().GetProperties().Where(a => a.Name.ToLower() == reader.GetName(i)).FirstOrDefault();
                                if (info != null && val != null)
                                    info.SetValue(ret, Convert.ChangeType(val, GetTypeWithoutNullable(info.PropertyType)), null);
                            }
                            yield return ret;
                        }
                    }
                }
            }
        }

        private Type GetTypeWithoutNullable(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                return Nullable.GetUnderlyingType(type);
            else
                return type;
        }

        public IEnumerable<Dictionary<string, object>> ExecuteReader(Query query)
        {
            using (var cmd = GetCommand(query))
            {
                //cmd.UnknownResultTypeList = new[] { true };
                cmd.AllResultTypesAreUnknown = true;
                using (var reader = cmd.ExecuteReader())
                {
                    var geoReader = new NetTopologySuite.IO.WKBReader();
                    while (reader.Read())
                    {
                        var ret = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (!typeof(NpgsqlTypes.PostgisGeometry).IsAssignableFrom(reader.GetFieldType(i)))
                            {
                                var name = reader.GetName(i);
                                if (name == "shape")
                                { }
                                //var val = reader.GetFieldValue<byte[]>(i);
                                var value = _typeMapper.ConvertFromSql(reader.GetValue(i));
                                ret[name] = value;
                            }
                            else
                            {
                                var name = reader.GetName(i);
                                var value = !reader.IsDBNull(i) ? geoReader.Read(reader.GetFieldValue<byte[]>(i)) : null;
                                ret[name] = value;
                            }
                        }
                        yield return ret;
                    }
                }
            }
        }
        
        public ResultStatus ExecuteNonQuery(Query query)
        {
            try
            {
                using (var cmd = GetCommand(query))
                {
                    var tr = cmd.ExecuteNonQuery();
                    return new ResultStatus
                    {
                        message = tr.ToString(),
                        result = true
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResultStatus
                {
                    message = "UnSuccessfull - " + ex.ToString(),
                    result = false
                };
            }
        }
        
        public bool IsSupportBulkInsert { get { return true; } }


        public ResultStatus ExecuteBulkInsert(string tableName, string schemaName, FeatureCollection collection,TableInfo tableInfo, string geomColName)
        {
            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                tableName = schemaName == null ? tableName : string.Format("{0}.{1}", schemaName, tableName);
                var columns = string.Join(",", collection.Features[0].Attributes.GetNames()).ToLower(CultureInfo.InvariantCulture) + "," + geomColName;
                var command = string.Format("COPY {0} ({1}) FROM STDIN (FORMAT BINARY)", tableName, columns);
                using (var writer = _connection.BeginBinaryImport(command))
                {
                    foreach (var feature in collection.Features)
                    {
                        writer.StartRow();
                        foreach (var name in feature.Attributes.GetNames())
                            if (feature.Attributes[name] != null)
                                writer.Write(feature.Attributes[name]);
                            else writer.WriteNull();
                        writer.Write(feature.Geometry.AsBinary());
                    }
                }
                return new ResultStatus { result = true };
            }
            catch (Exception ex)
            {
                return new ResultStatus { result = false, message = ex.Message };
            }
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

        private NpgsqlCommand GetCommand(Query query)
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            var cmd = _connection.CreateCommand();
            if (_transaction != null && _transaction.Connection != null)
                cmd.Transaction = _transaction;

            cmd.CommandText = query.Command;
            foreach (var parameter in query.Parameters)
                cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
            return cmd;
        }
    }
}
