using System;
using System.Data.Common;
using System.Linq;

namespace Infoline.Framework.Database.Postgis
{
    public class PostgisTableCreator : ITableCreator
    {
        IQueryBuilder _builder;
        IQueryExecutor _executor;
        ITypeMapper _typeMapper;

        public PostgisTableCreator(IQueryBuilder builder, IQueryExecutor executor, ITypeMapper typeMapper)
            : base()
        {
            _builder = builder;
            _executor = executor;
            _typeMapper = typeMapper;
        }

        public ResultStatus Alter(TableInfo tableInfo)
        {
            throw new NotImplementedException();
        }

        public ResultStatus Create(TableInfo tableInfo)
        {
            DbTransaction transaction = null;
            var opentransaction = _executor.IsTransactionOpen;
            if (!opentransaction)
                transaction = _executor.BeginTransaction();

            try
            {
                Query query = null;
                ResultStatus result = null;
                string schema = tableInfo.SchemaName != null ? string.Format("{0}.", tableInfo.SchemaName) : "";
                // create sequences 
                foreach (var col in tableInfo.Columns.Where(a => a.AutoIncrement != null))
                {
                    var sequenceName = string.Format("seq_{0}_{1}", tableInfo.TableName, col.ColumnName);
                    col.Default = string.Format("NEXTVAL('{0}')", sequenceName);
                    query = _builder.ConvertToQuery("SELECT CAST(COUNT(*) as BIT) FROM INFORMATION_SCHEMA.SEQUENCES WHERE SEQUENCE_NAME = {0}", sequenceName);
                    var sequenceExits = _executor.ExecuteScaler<bool>(query);
                    if (!sequenceExits)
                    {
                        query = new Query { Command = string.Format("CREATE SEQUENCE {0} INCREMENT BY {2} START WITH {1}", sequenceName, col.AutoIncrement.Start, col.AutoIncrement.Increment) };
                        _executor.ExecuteNonQuery(query);
                    }
                }

                // create table

                SetDefaultFunctions(tableInfo);
                var queryString = "SELECT CAST(COUNT(*) as BIT) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {0} " + (tableInfo.SchemaName != null ? "AND TABLE_SCHEMA =  {1}" : "");
                var queryParams = new object[tableInfo.SchemaName  != null ? 2 : 1];
                queryParams[0] = tableInfo.TableName.ToLower();
                if (tableInfo.SchemaName != null)
                    queryParams[1] = tableInfo.SchemaName.ToLower();

                query = _builder.ConvertToQuery(queryString, queryParams);
                var tableExits = _executor.ExecuteScaler<bool>(query);
                if (tableExits)
                {
                    if (!opentransaction)
                        transaction.Commit();
                    return new ResultStatus { result = true, message = "Tablo zaten var." };
                }

                var columns = tableInfo.Columns.Select(a =>
                                    string.Format(" {0} {1} {2} {3}",
                                            a.ColumnName,
                                            _typeMapper.GetSqlType(a.Type),
                                            a.Default != null ? string.Format("DEFAULT {0}", a.Default.Text) : "",
                                            (a.AutoIncrement != null || tableInfo.PrimaryKey.Contains(a.ColumnName) ? "NOT NULL" : "NULL")));

                query = new Query { Command = string.Format(@"CREATE TABLE {2}{1}( {0} ) ", string.Join(",", columns), tableInfo.TableName, schema) };
                result = _executor.ExecuteNonQuery(query);
                if (!result.result)
                    throw new QueryExecuteException(QueryExecuteException.ExceptionTypes.TableCreateException, result.message);
                
                // create primary key
                if (tableInfo.PrimaryKey.Count > 0)
                {
                    var columnsString = string.Join(",", tableInfo.PrimaryKey.Select(a => string.Format("{0}", a)).ToArray());
                    query = new Query { Command = string.Format("ALTER TABLE {2}{0} ADD PRIMARY KEY ( {1} )", tableInfo.TableName, columnsString, schema) };
                    result = _executor.ExecuteNonQuery(query);
                    if (!result.result)
                        throw new QueryExecuteException(QueryExecuteException.ExceptionTypes.TableCreateException, result.message);
                }

                // create indexes
                foreach (var index in tableInfo.Indexes)
                {
                    index.Name = string.Format("idx_{0}_{1}", tableInfo.TableName.Replace(".", "_"), string.Join("_", index.Columns));
                    var columText = string.Join(",", index.Columns);
                    query = new Query { Command = string.Format("CREATE {3} INDEX {0} ON {4}{1} ({2})", index.Name, tableInfo.TableName, columText, index.IsUnique ? "UNIQUE" : "", schema) };
                    result = _executor.ExecuteNonQuery(query);
                    if (!result.result)
                        throw new QueryExecuteException(QueryExecuteException.ExceptionTypes.TableCreateException, result.message);
                }

                if (!opentransaction)
                    transaction.Commit();
                return new ResultStatus { result = true };
            }
            catch (Exception ex)
            {
                if (!opentransaction)
                    transaction.Rollback();
                return new ResultStatus { result = false, message = ex.Message };
            }
        }

        public TableInfo GetSchema(string tableName, bool onlyColums = false)
        {
            Query query = null;
            var tableInfo = new TableInfo() { TableName = tableName };

            query = _builder.ConvertToQuery("SELECT TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, DATA_TYPE, COLUMN_DEFAULT FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = {0}", tableName);
            var columns = _executor.ExecuteReader(query).ToArray();
            if (columns == null || columns.Count() == 0) return null;
            tableInfo.Columns = columns.Select(a => new TableColumn { ColumnName = a["column_name"].ToString(), Type = _typeMapper.GetType(a["data_type"].ToString()), Default = SqlFunctionsFromString(a["column_default"]) }).ToList();

            query = _builder.ConvertToQuery(@"select t.relname as table_name, i.relname as index_name, ix.indisunique is_unique, ix.indisprimary is_primary_key, a.attname as column_name
from pg_class t, pg_class i, pg_index ix, pg_attribute a
where t.oid = ix.indrelid and i.oid = ix.indexrelid and a.attrelid = t.oid and a.attnum = ANY(ix.indkey) and t.relkind = 'r' and t.relname like 'test_6'
order by t.relname, i.relname", tableName.Replace(".", "_"));
            var indexes = _executor.ExecuteReader(query).ToArray();
            var primaryKey = indexes.Where(a => (string)a["is_primary_key"] == "t");
            tableInfo.PrimaryKey = primaryKey.Select(a => a["column_name"]).Cast<string>().ToList();

            var groups = indexes.Where(a => (string)a["is_primary_key"] == "f").GroupBy(a => new { table_name = a["table_name"], index_name = a["index_name"], is_unique = a["is_unique"], is_primary_key = a["is_primary_key"] }).ToArray();
            tableInfo.Indexes = groups.Select(a => new TableIndex { IsUnique = a.Key.is_unique.Equals("t"), Columns = a.Select(b => b["column_name"]).Cast<string>().ToArray() }).ToList();

            return tableInfo;
        }

        private void SetDefaultFunctions(TableInfo tableInfo)
        {
            foreach (var col in tableInfo.Columns.Where(a => a.Default != null))
                if (col.Default.Text == null)
                    switch (col.Default.Function)
                    {
                        case SqlFunctions.NEWID:
                            col.Default.Text = "uuid_generate_v4()";
                            //col.Default.Text = "UUID_IN((MD5((RANDOM())::TEXT))::CSTRING)";
                            break;
                        case SqlFunctions.GETDATE:
                            col.Default.Text = "now()";
                            break;
                        default:
                            break;
                    }
        }

        private ColumnDefaultValue SqlFunctionsFromString(object value)
        {
            if (value == null) return null;
            var val = (string)value;
            if (val.Trim().ToLower() == "uuid_generate_v4()" || val.Trim().ToLower() == "uuid_in((md5((random())::text))::cstring)")
                return SqlFunctions.NEWID;
            else if (val.Trim().ToLower() == "now()")
                return SqlFunctions.GETDATE;
            return value.ToString();
        }
    }
}
