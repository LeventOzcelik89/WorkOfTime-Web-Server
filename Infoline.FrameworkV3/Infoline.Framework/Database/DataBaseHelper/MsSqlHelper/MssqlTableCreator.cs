using System;
using System.Data.Common;
using System.Linq;

namespace Infoline.Framework.Database.Mssql
{
    public class MssqlTableCreator : ITableCreator
    {
        IQueryBuilder _builder;
        IQueryExecutor _executor;
        ITypeMapper _typeMapper;

        public MssqlTableCreator(IQueryBuilder builder, IQueryExecutor executor, ITypeMapper typeMapper)
            : base()
        {
            _builder = builder;
            _executor = executor;
            _typeMapper = typeMapper;
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
                // create table
                SetDefaultFunctions(tableInfo);
                query = _builder.ConvertToQuery("SELECT CAST(COUNT(*) as BIT) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {0}", tableInfo.TableName);
                var tableExits = _executor.ExecuteScaler<bool>(query);
                if (tableExits)
                {
                    if (!opentransaction)
                        transaction.Commit();
                    return new ResultStatus { result = true, message = "Tablo zaten var." };
                }
                
                var columns = tableInfo.Columns.Select(a =>
                                    string.Format(" [{0}] {1} {2}",
                                            a.ColumnName,
                                            _typeMapper.GetSqlType(a.Type, a.Length),
                                            a.AutoIncrement != null ?
                                                string.Format("IDENTITY({0},{1}) NOT NULL", a.AutoIncrement.Start, a.AutoIncrement.Increment) :
                                                (tableInfo.PrimaryKey.Contains(a.ColumnName) | a.NotNull ? "NOT NULL" : "NULL")));

                var pk = string.Join(",", tableInfo.PrimaryKey.Select(a => string.Format("[{0}]", a)));
                query = new Query { Command = string.Format(@"CREATE TABLE [{1}]( {0} ) ON [PRIMARY]", string.Join(",", columns), tableInfo.TableName, pk) };
                result = _executor.ExecuteNonQuery(query);
                if (!result.result)
                    throw new QueryExecuteException(QueryExecuteException.ExceptionTypes.TableCreateException, result.message);

                // create primary key
                if (tableInfo.PrimaryKey.Count > 0)
                {
                    var columnsString = string.Join(",", tableInfo.PrimaryKey.Select(a => string.Format("[{0}] ASC", a)).ToArray());
                    query = new Query { Command = string.Format("ALTER TABLE [{0}] ADD CONSTRAINT [PK_{0}] PRIMARY KEY ( {1} )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]", tableInfo.TableName, columnsString) };
                    result = _executor.ExecuteNonQuery(query);
                    if (!result.result)
                        throw new QueryExecuteException(QueryExecuteException.ExceptionTypes.TableCreateException, result.message);
                }

                // set default values
                foreach (var col in tableInfo.Columns.Where(a => a.Default != null))
                {
                    query = new Query { Command = string.Format("ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_{1}]  DEFAULT ({2}) FOR {1}", tableInfo.TableName, col.ColumnName, col.Default.Text) };
                    result = _executor.ExecuteNonQuery(query);
                    if (!result.result)
                        throw new QueryExecuteException(QueryExecuteException.ExceptionTypes.TableCreateException, result.message);
                }

                // create indexes
                foreach (var index in tableInfo.Indexes)
                {
                    index.Name = string.Format("IDX_{0}_{1}", tableInfo.TableName, string.Join("_", index.Columns));
                    var columText = string.Join(",", index.Columns);
                    query = new Query { Command = string.Format("CREATE {3} INDEX {0} ON {1} ({2})", index.Name, tableInfo.TableName, columText, index.IsUnique ? "UNIQUE" : "") };
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

        public ResultStatus Alter(TableInfo tableInfo)
        {
            DbTransaction transaction = null;
            var opentransaction = _executor.IsTransactionOpen;
            if (!opentransaction)
                transaction = _executor.BeginTransaction();

            try
            {
                Query query = null;
                ResultStatus result = null;
              
                SetDefaultFunctions(tableInfo);
                query = _builder.ConvertToQuery("SELECT CAST(COUNT(*) as BIT) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {0}", tableInfo.TableName);
                var tableExits = _executor.ExecuteScaler<bool>(query);
                if (tableExits) //Table Check
                {
                    var columns = tableInfo.Columns.Select(a =>
                                    string.Format(" [{0}] {1} {2}",
                                            a.ColumnName,
                                            _typeMapper.GetSqlType(a.Type, a.Length),
                                            a.AutoIncrement != null ?
                                                string.Format("IDENTITY({0},{1}) NOT NULL", a.AutoIncrement.Start, a.AutoIncrement.Increment) :
                                                (tableInfo.PrimaryKey.Contains(a.ColumnName) ? "NOT NULL" : "NULL")));

                    var pk = string.Join(",", tableInfo.PrimaryKey.Select(a => string.Format("[{0}]", a)));

                    var schemaColoumbs = GetSchema(tableInfo.TableName);
                   
                    var k = tableInfo.Columns.Where(t2 => !schemaColoumbs.Columns.Any(t1 => t2.ColumnName.Contains(t1.ColumnName)));
                    
                    foreach (var col in tableInfo.Columns.Where(t2 => !schemaColoumbs.Columns.Any(t1 => t2.ColumnName.Contains(t1.ColumnName))))
                    {
                        query = new Query { Command = string.Format("ALTER TABLE [dbo].[{0}] ADD {1} {2}", tableInfo.TableName, col.ColumnName, _typeMapper.GetSqlType(col.Type))};
                        result = _executor.ExecuteNonQuery(query);
                        if (!result.result)
                            throw new QueryExecuteException(QueryExecuteException.ExceptionTypes.TableCreateException, result.message);
                        if (result.result && col.Default != null)
                        {
                            query = new Query { Command = string.Format("ALTER TABLE [dbo].[{0}] ADD  CONSTRAINT [DF_{0}_{1}]  DEFAULT ({2}) FOR {1}", tableInfo.TableName, col.ColumnName, col.Default.Text) };
                            result = _executor.ExecuteNonQuery(query);
                            if (!result.result)
                                throw new QueryExecuteException(QueryExecuteException.ExceptionTypes.TableCreateException, result.message);
                        }
                    }
                    
                    if (!opentransaction)
                        transaction.Commit();
                    return new ResultStatus { result = true };
                }

                else
                {
                    if (!opentransaction)
                        transaction.Commit();
                    return new ResultStatus { result = true, message = "Böyle Bir Tablo Yok" };
                }
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

            query = _builder.ConvertToQuery("SELECT TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME, DATA_TYPE, COLUMN_DEFAULT, CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = {0}", tableName);
            var columns = _executor.ExecuteReader(query);
            if (columns == null || columns.Count() == 0) return null;
            tableInfo.Columns = columns.Select(a => new TableColumn
            {
                ColumnName = a["COLUMN_NAME"].ToString(),
                Type = _typeMapper.GetType(a["DATA_TYPE"].ToString()),
                Length = a["CHARACTER_MAXIMUM_LENGTH"] != null ? (int?)a["CHARACTER_MAXIMUM_LENGTH"] : null,
                Default = SqlFunctionsFromString(a["COLUMN_DEFAULT"])
            }).ToList();


            query = _builder.ConvertToQuery(@"select object_name(i.object_id) as table_name, ic.index_id, i.name as index_name, i.[type] as index_type, i.type_desc, i.is_primary_key, i.is_unique, c.name column_name, t.name column_type_name , key_ordinal
from sys.index_columns ic
inner join sys.indexes i on ic.object_id = i.object_id and ic.index_id = i.index_id
inner join sys.columns c on ic.object_id = c.object_id and ic.column_id = c.column_id
inner join sys.types t on c.system_type_id = t.system_type_id and c.user_type_id = t.user_type_id
where object_name(ic.object_id) = {0}
order by index_id, key_ordinal", tableName);
            var indexes = _executor.ExecuteReader(query).ToArray();
            var primaryKey = indexes.Where(a => (bool)a["is_primary_key"] == true);
            tableInfo.PrimaryKey = primaryKey.Select(a => a["column_name"]).Cast<string>().ToList();

            var groups = indexes.Where(a => (bool)a["is_primary_key"] == false)
                .GroupBy(a => new
                {
                    table_name = a["table_name"],
                    index_id = a["index_id"],
                    index_name = a["index_name"],
                    index_type = a["index_type"],
                    type_desc = a["type_desc"],
                    is_primary_key = a["is_primary_key"],
                    is_unique = a["is_unique"]
                }).ToArray();

            tableInfo.Indexes = groups.Select(a => new TableIndex
            {
                IsUnique = (bool)a.Key.is_unique,
                Columns = a.Select(b => b["column_name"]).Cast<string>().ToArray()
            }).ToList();

            return tableInfo;
        }

        private void SetDefaultFunctions(TableInfo tableInfo)
        {
            foreach (var col in tableInfo.Columns.Where(a => a.Default != null))
                if (col.Default.Text == null)
                    switch (col.Default.Function)
                    {
                        case SqlFunctions.NEWID:
                            col.Default.Text = "NEWID()";
                            break;
                        case SqlFunctions.GETDATE:
                            col.Default.Text = "GETDATE()";
                            break;
                        default:
                            break;
                    }
        }

        private ColumnDefaultValue SqlFunctionsFromString(object value)
        {
            if (value == null) return null;
            var val = (string)value;
            if (val.Trim().ToLower() == "(newid())" || val.Trim().ToLower() == "newid()")
                return SqlFunctions.NEWID;
            else if (val.Trim().ToLower() == "(getdate())" || val.Trim().ToLower() == "getdate()")
                return SqlFunctions.GETDATE;
            return value.ToString();
        }
    }
}
