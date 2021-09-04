using Infoline.Framework.Helper;
using Infoline.Helper;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.DatabaseE
{
    public class Database : IDisposable
    {
        IQueryBuilder _queryBuilder;
        SqlConnection _connection;


        public static DateTime Mindate = new DateTime(1900, 1, 1);
        public static DateTime Maxdate = new DateTime(2100, 1, 1);
        public static object DBNulltoNull(object o) { return o == DBNull.Value ? null : o; }
        public static string SqlDate(DateTime date)
        {
            return string.Format("{0:yyyy-MM-dd HH:mm:ss}", date < Mindate ? Mindate : (date > Maxdate ? Maxdate : date));
        }



        public Database(string connectionString, SqlTransaction tran = null)
        {
            _connection = new SqlConnection(connectionString);
            _queryBuilder = new SqlQueryBuilder();
            _tran = tran;
            if (_tran == null)
                _connection.Open();
            else if (_tran.Connection != null)
                _connection = _tran.Connection;
            else throw new Exception("Transaction kapatılmış.");
                
        }

        private void SetTransaction(SqlCommand cmd)
        {
            if (_tran != null && _tran.Connection != null)
                cmd.Transaction = _tran;
        }

        public void Execute(StreamReader sr)
        {
            var sb = new StringBuilder();
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.ToUpper().Trim() == "GO")
                {
                    if (sb.Length > 0)
                        ExecuteNonQuery(sb.ToString());
                    sb.Clear();
                }
                else
                    sb.AppendLine(line);
            }
            if (sb.Length > 0)
                ExecuteNonQuery(sb.ToString());
        }

        public int ExecuteNonQuery(string txt, params object[] parameters)
        {
            var query = _queryBuilder.ConvertToQuery(txt, parameters);
            using (var cmd = GetCommand(query))
            {
                SetTransaction(cmd);
                return cmd.ExecuteNonQuery();
            }
        }

        public T ExecuteScaler<T>(string txt, params object[] parameters)
        {
            var query = _queryBuilder.ConvertToQuery(txt, parameters);
            using (var cmd = GetCommand(query))
            {
                SetTransaction(cmd);
                var ret = cmd.ExecuteScalar();
                return ret == DBNull.Value ? default(T) : (T)ret;
            }
        }

        //public IEnumerable<object[]> ExecuteReader(string txt, params object[] parameters)
        //{
        //    var query = _queryBuilder.ConvertToQuery(txt, parameters);
        //    using (var cmd = GetCommand(query))
        //    {
        //        if (cmd.Connection.State != ConnectionState.Open)
        //            cmd.Connection.Open();
        //        using (var reader = cmd.ExecuteReader())
        //        {
        //            var columns = new string[reader.FieldCount].Select((a, i) => reader.GetName(i)).ToArray();
        //            while (reader.Read())
        //            {
        //                object[] ret = new object[reader.FieldCount];
        //                for (int i = 0; i < reader.FieldCount; i++)
        //                {
        //                    ret[i] = reader.GetValue(i);
        //                }
        //                yield return ret;
        //            }
        //        }
        //    }
        //}

        public IEnumerable<Dictionary<string, object>> ExecuteReader(string txt, params object[] parameters)
        {
            var query = _queryBuilder.ConvertToQuery(txt, parameters);
            using (var cmd = GetCommand(query))
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                SetTransaction(cmd);
                using (var reader = cmd.ExecuteReader())
                {
                    //var t = typeof(T);
                    //Action<object, object>[] setters = new Action<object, object>[reader.FieldCount];
                    //for (int i = 0; i < reader.FieldCount; i++)
                    //    setters[i] = PropertyAccessGenerator.SetDelegate(t, reader.GetName(i));

                    while (reader.Read())
                    {
                        Dictionary<string, object> ret = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {

                            if (reader.GetFieldType(i).Name.Equals("SqlGeography"))
                            {
                                ret[reader.GetName(i)] = SqlGeography.Deserialize(reader.GetSqlBytes(i));
                            }
                            else if (reader.GetName(i).Equals("id"))
                            {
                                ret[reader.GetName(i)] = new Guid(reader.GetValue(i).ToString());
                            }
                            else
                            {
                                ret[reader.GetName(i)] = reader.GetValue(i);
                            }

                        }
                        yield return ret;
                    }
                }
            }
        }

        public IEnumerable<T> ExecuteReader<T>(string txt, params object[] parameters) where T : new()
        {
            var query = _queryBuilder.ConvertToQuery(txt, parameters);
            using (var cmd = GetCommand(query))
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                SetTransaction(cmd);
                using (var reader = cmd.ExecuteReader())
                {
                    Action<object, object>[] setters = new Action<object, object>[reader.FieldCount];
                    try
                    {
                        var t = typeof (T);
                        for (int i = 0; i < reader.FieldCount; i++)
                            setters[i] = PropertyAccessGenerator.SetDelegate(t, reader.GetName(i));
                    }
                    catch (Exception ex)
                    {
                        
                    }

                    while (reader.Read())
                    {
                        T ret = new T();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            try
                            {
                                if (setters[i] != null && !(reader.GetValue(i) is DBNull) && reader.GetValue(i) != null)
                                {
                                    if (reader.GetFieldType(i).Name.Equals("SqlGeography"))
                                    {
                                        setters[i](ret, SqlGeography.Deserialize(reader.GetSqlBytes(i)));
                                    }
                                    else if (reader.GetName(i).Equals("id"))
                                    {
                                        setters[i](ret, new Guid(reader.GetValue(i).ToString()));
                                    }
                                    else
                                    {
                                        setters[i](ret, reader.GetValue(i));
                                    }
                                }
                            }
                            catch (Exception)
                            {
                                
                                throw;
                            }

                            //if (setters[i] != null && !(reader.GetValue(i) is DBNull) && reader.GetValue(i) != null)
                            //    setters[i](ret, (reader.GetFieldType(i).Name.Equals("SqlGeography")
                            //   ? SqlGeography.Deserialize(reader.GetSqlBytes(i))
                            //   : reader.GetValue(i)));
                        }
                        yield return ret;
                    }
                }
            }
        }


        public ResultStatus ExecuteInsert<T>(T parameter, Expression<Func<T, object>> exceprexp = null, string tableName = null)
        {
            var query = _queryBuilder.GetInsertQuery<T>(parameter, exceprexp, tableName);
            var result = ExecuteNonQuery(query);
            return result;
        }

        public ResultStatus ExecuteUpdate<T>(T parameter, Expression<Func<T, object>> idexp, Expression<Func<T, object>> exceprexp = null, bool setNull = false, string tableName = null)
        {
            var query = _queryBuilder.GetUpdateQuery<T>(parameter, idexp, exceprexp, setNull, tableName);
            var result = ExecuteNonQuery(query);
            return result;
        }

        public ResultStatus ExecuteDelete<T>(T parameter, Expression<Func<T, object>> idexp, string tableName = null)
        {
            var query = _queryBuilder.GetDeleteQuery<T>(parameter, idexp, tableName);
            var result = ExecuteNonQuery(query);
            return result;
        }


        public ResultStatus ExecuteBulkInsert<T>(List<T> parametre, Expression<Func<T, object>> exceprexp = null, string tableName = null)
        {
            if (parametre == null || parametre.Count == 0)
                return new ResultStatus { result = true };

            if (tableName == null)
                tableName = typeof(T).Name;
            var except = ExpressionHelper.GetPropertyNames<T, object>(exceprexp).ToArray();

            var dbTable = string.Empty;
            var dt = new DataTable();
            foreach (var prm in parametre)
            {
                var row = dt.NewRow();
                foreach (var p in prm.GetType().GetProperties()
                    .Where(p => p.GetValue(prm, null) != null)
                    .Where(p => !string.IsNullOrEmpty(p.GetValue(prm, null).
                        ToString().Trim())).Where(p => !except.Contains(p.Name)))
                {
                    if (!dt.Columns.Contains(p.Name))
                    {
                        dbTable = tableName;
                        var column = DatColumn(p.GetValue(prm, null).GetType().Name, p.Name);
                        dt.Columns.Add(column);
                    }
                    row[p.Name] = p.GetValue(prm, null);
                }
                dt.Rows.Add(row);
            }

            try
            {
                if (_connection.State != ConnectionState.Open)
                    _connection.Open();

                using (var copy = new SqlBulkCopy(_connection, SqlBulkCopyOptions.TableLock, _tran))
                {
                    foreach (var item in dt.Columns)
                    {
                        copy.ColumnMappings.Add(item.ToString(), item.ToString());
                    }
                    copy.DestinationTableName = dbTable;
                    copy.WriteToServer(dt);
                    return new ResultStatus() { message = "Ok..", result = true };
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

        public ResultStatus ExecuteBulkUpdate<T>(List<T> parametre, Expression<Func<T, object>> idexp, Expression<Func<T, object>> exceprexp = null, bool setNull = false, string tableName = null)
        {
            try
            {
                if (parametre == null || parametre.Count == 0)
                    return new ResultStatus { result = true };

                if (tableName == null)
                    tableName = typeof(T).Name;
                var except = ExpressionHelper.GetPropertyNames<T, object>(exceprexp).ToArray();
                var idprop = (PropertyInfo)ExpressionHelper.GetProperty<T, object>(idexp);

                using (var cmd = _connection.CreateCommand())
                {
                    SetTransaction(cmd);

                    var pSayi = 0;
                    foreach (var prm in parametre)
                    {
                        object id = null;
                        var parametrename = "";

                        var parameters = prm.GetType().GetProperties().Where(p => !except.Contains(p.Name));

                        if (!setNull)
                        {
                            parameters = parameters
                                .Where(p => p.GetValue(prm, null) != null)
                                .Where(p => !string.IsNullOrEmpty(p.GetValue(prm, null).ToString().Trim()));
                        }

                        foreach (var p in parameters)
                        {
                            pSayi++;
                            id = idprop.GetValue(prm, null);
                            parametrename += ",[" + p.Name + "]=@" + p.Name + pSayi;

                            if (p.GetValue(prm, null) != null && p.GetValue(prm, null).GetType().Name == "SqlGeography")
                                cmd.Parameters.Add(new SqlParameter { Value = p.GetValue(prm, null), ParameterName = p.Name + pSayi, UdtTypeName = "Geography" });
                            else
                                cmd.Parameters.AddWithValue("@" + p.Name + pSayi, p.GetValue(prm, null) ?? DBNull.Value);

                        }
                        var firstOrDefault = parametre.FirstOrDefault();
                        if (firstOrDefault != null)
                            cmd.CommandText += string.Format("Update {0} Set {1} where id = '{2}' ; ", firstOrDefault.GetType().Name, parametrename.Substring(1), id);
                    }

                    cmd.UpdatedRowSource = UpdateRowSource.None;
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

        public ResultStatus ExecuteBulkDelete<T>(List<T> parametre, Expression<Func<T, object>> idexp, string tableName = null)
        {
            try
            {
                if (parametre == null || parametre.Count == 0)
                    return new ResultStatus { result = true };

                if (tableName == null)
                    tableName = typeof(T).Name;

                for (int j = 0; j < parametre.Count; j++)
                {
                    var idProperties = ExpressionHelper.GetProperties<T, object>(idexp);
                    var whereStatement = "";
                    var param = new List<QueryParameter>();
                    int i = 0;
                    foreach (var idProperty in idProperties.Cast<PropertyInfo>())
                    {
                        if (string.IsNullOrEmpty(idProperty.GetValue(parametre[j], null).ToString().Trim())) continue;
                        whereStatement += " and " + idProperty.Name + "=@p" + i;
                        param.Add(new QueryParameter { Name = "@p" + i, Value = idProperty.GetValue(parametre[j], null) });
                        i++;
                    }
                    whereStatement = whereStatement.Substring(5);

                    if (param.Count == 0)
                        throw new Exception("Delete işlemi için vir kolon seçmeniz gerekmekte.");

                    var sql = string.Format("delete from {0} where " + whereStatement, parametre[j].GetType().Name);
                    var query = new Query { Command = sql, Parameters = param.ToArray() };
                    var result = ExecuteNonQuery(query);
                    if (!result.result)
                        return new ResultStatus { message = result.message, result = false };
                }
            }
            catch (Exception ex)
            {
                return new ResultStatus { message = "UnSuccessfull - " + ex, result = false };
            }
            return new ResultStatus { message = "Successfull - ", result = true };
        }


        public ITableCreate<T> CreateTable<T>(string name = null)
        {
            return new TableCreate<T>(this, name);
        }

        public ITableAlter AlterTable(string name)
        {
            return new TableAlter(this, name);
        }

        public ITableAlter AlterTable<T>()
        {
            string name = typeof(T).Name;
            return new TableAlter(this, name);
        }

        public ISelect<T> Select<T>(Expression<Func<T, object>> columns = null, string tablename = null) where T : new()
        {
            return new Select<T>(this, columns, tablename);
        }

        public ISelect<T> Select<T>(string[] columns, string tablename) where T : new()
        {
            return new Select<T>(this, columns, tablename);
        }

        public int Count<T>(Expression<Func<T, bool>> condition, string name = null) where T : new()
        {
            if(name == null)
                name = typeof(T).Name;
            return new Select<T>(this, new string[0], name).ExecuteCount(condition);
        }

        public void Dispose()
        {
            if (_tran == null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }


        bool _beginTransection = false;
        SqlTransaction _tran;
        public SqlTransaction BeginTransection()
        {
            if (_beginTransection)
                return _tran;
            _tran = _connection.BeginTransaction();
            return _tran;
        }
        public bool CommitTransection()
        {
            try
            {
                if (!_beginTransection)
                    return false;
                _tran.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool RollBackTransection()
        {
            try
            {
                if (!_beginTransection)
                    return false;
                _tran.Rollback();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        private DataColumn DatColumn(string type, string name)
        {
            if (type.Equals(DbType.Guid.ToString()))
                return new DataColumn(name, typeof(Guid));
            if (type.Equals(DbType.DateTime.ToString()))
                return new DataColumn(name, typeof(DateTime));
            if (type.Equals(DbType.Boolean.ToString()))
                return new DataColumn(name, typeof(Boolean));
            if (type.Equals(DbType.Int32.ToString()))
                return new DataColumn(name, typeof(Int32));
            if (type == "SqlGeography")
                return new DataColumn(name, typeof(SqlGeography));
            if (type == "SqlGeometry")
                return new DataColumn(name, typeof(SqlGeometry));

            return new DataColumn(name);
        }

        private ResultStatus ExecuteNonQuery(Query query)
        {
            try
            {
                using (var cmd = GetCommand(query))
                {
                    SetTransaction(cmd);
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

        private SqlCommand GetCommand(Query query)
        {
            var cmd = _connection.CreateCommand();
            if (_tran != null && _tran.Connection != null)
                cmd.Transaction = _tran;

            cmd.CommandText = query.Command;
            foreach (var parameter in query.Parameters)
            {
                if (parameter.Value == null)
                    cmd.Parameters.AddWithValue(parameter.Name, DBNull.Value);
                else if (parameter.Value.GetType().Name == "SqlGeography")
                    cmd.Parameters.Add(new SqlParameter { Value = parameter.Value, ParameterName = parameter.Name, UdtTypeName = "Geography" });
                else
                    cmd.Parameters.AddWithValue(parameter.Name, parameter.Value);
            }
            return cmd;
        }
    }



    public class Query
    {
        public string Command { get; set; }
        public QueryParameter[] Parameters { get; set; }
        public bool IsStoredProcedure { get; set; }
    }

    public class QueryParameter
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, Value);
        }
    }

    public class ResultStatus
    {
        public bool result { get; set; }
        public string message { get; set; }
        public object objects { get; set; }
    }

}
