
using Infoline.Framework.Database.Mssql;
using Infoline.Framework.Database.Postgis;
using NetTopologySuite.Features;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Infoline.Framework.Database
{
    public class Database : IDisposable
    {
        private DatabaseType _dbType;
        private string _connectionString;
        private IQueryExecutor _executor;
        private IQueryBuilder _builder;
        private ITypeMapper _typeMapper;
        private DbTransaction _transaction;

        public static Action<Query, ResultStatus> OnExecuionComplate
        {
            get { return MssqlQueryExecutor.OnExecuionComplate; }
            set
            {
                MssqlQueryExecutor.OnExecuionComplate = value;
                PostgisQueryExecutor.OnExecuionComplate = value;
            }
        }

        public string ServerName { get { return _executor.ServerName; } }
        public string DbName { get { return _executor.DbName; } }

        public Database(string connectionString, DatabaseType dbType = DatabaseType.Mssql, DbTransaction transaction = null)
        {
            _dbType = dbType;
            _connectionString = connectionString;
            _transaction = transaction;

            if (dbType == DatabaseType.Mssql)
            {
                _typeMapper = new MssqlTypeMapper();
                _executor = new MssqlQueryExecutor(_connectionString, _typeMapper, transaction);
                _builder = new MssqlQueryBuilder(_typeMapper);
            }
            else if (dbType == DatabaseType.Postgis)
            {
                _typeMapper = new PostgisTypeMapper();
                _executor = new PostgisQueryExecutor(_connectionString, _typeMapper, transaction);
                _builder = new PostgisQueryBuilder(_typeMapper);
            }
            else if (dbType == DatabaseType.PostgresqlWithGeodatabase)
            {
                _typeMapper = new PostgisTypeMapper();
                _executor = new PostgisQueryExecutor(_connectionString, _typeMapper, transaction);
                _builder = new PostgisQueryBuilder(_typeMapper);
            }
            else
            {
                throw new Exception("Veritabanı tipi bulunamadı");
            }
        }

        public bool IsTransactionOpen { get { return _executor.IsTransactionOpen; } }
        public DbTransaction BeginTransaction()
        {
            return _executor.BeginTransaction();
        }
        public ResultStatus Commit()
        {
            throw new NotImplementedException();
        }
        public ResultStatus RollBack()
        {
            throw new NotImplementedException();
        }


        public T ExecuteScaler<T>(string command, params object[] parameters)
        {
            var query = _builder.ConvertToQuery(command, parameters);
            return _executor.ExecuteScaler<T>(query);
        }
        public IEnumerable<T> ExecuteReader<T>(string command, params object[] parameters)
        {
            var query = _builder.ConvertToQuery(command, parameters);
            return _executor.ExecuteReader<T>(query);
        }
        public IEnumerable<Dictionary<string, object>> ExecuteReader(string command, params object[] parameters)
        {
            var query = _builder.ConvertToQuery(command, parameters);
            return _executor.ExecuteReader(query);
        }
        public FeatureCollectionExt ExecuteFeature(string command, params object[] parameters)
        {
            var query = _builder.ConvertToQuery(command, parameters);
            return (_executor as MssqlQueryExecutor).ExecuteFeature2(query);
        }
        public ResultStatus ExecuteNonQuery(string command, params object[] parameters)
        {
            var query = _builder.ConvertToQuery(command, parameters);
            return _executor.ExecuteNonQuery(query);
        }

        public IGetTable Table(string tableName, string schemaName = null)
        {
            return new QueryProcessor(tableName, schemaName, _builder, _executor, _typeMapper);
        }
        public IGetTable TableFunction(string schemaName, string tableName, params object[] parameters)
        {
            return new QueryProcessor(tableName, schemaName, parameters, _builder, _executor, _typeMapper);
        }
        public IGetTable<T> Table<T>(string schemaName = null, string tableName = null) where T : new()
        {
            if (tableName == null)
                tableName = typeof(T).Name;

            var processor = new QueryProcessor(tableName, schemaName, _builder, _executor, _typeMapper);
            return new QueryProcessor<T, T>(tableName, schemaName, processor, _builder, _executor);
        }
        //public IGetTable<T> Table<T>(string tableName=null) where T : new()
        //{
        //    if (tableName == null)
        //        tableName = typeof(T).Name;
        //    var schemaName = "infoline";
        //    var processor = new QueryProcessor(tableName, schemaName, _builder, _executor, _typeMapper);
        //    return new QueryProcessor<T, T>(tableName, schemaName, processor, _builder, _executor);
        //}

        public IGetTable<T> TableFunction<T>(string schemaName, string tableName, params object[] parameters) where T : new()
        {
            if (tableName == null)
                tableName = typeof(T).Name;

            var processor = new QueryProcessor(tableName, schemaName, parameters, _builder, _executor, _typeMapper);
            return new QueryProcessor<T, T>(tableName, schemaName, parameters, processor, _builder, _executor);
        }

        public ResultStatus CreateTable(TableInfo tableInfo)
        {
            ITableCreator creator = null;
            if (_dbType == DatabaseType.Mssql) creator = new MssqlTableCreator(_builder, _executor, _typeMapper);
            else if (_dbType == DatabaseType.Postgis) creator = new PostgisTableCreator(_builder, _executor, _typeMapper);
            else throw new Exception("Veritabanı tipi bulunamadı");
            return creator.Create(tableInfo);
        }
        public ITableCreate CreateTable(string tableName, string schemaName = null)
        {
            ITableCreator creator = null;
            if (_dbType == DatabaseType.Mssql) creator = new MssqlTableCreator(_builder, _executor, _typeMapper);
            else if (_dbType == DatabaseType.Postgis) creator = new PostgisTableCreator(_builder, _executor, _typeMapper);
            else throw new Exception("Veritabanı tipi bulunamadı");
            return new TableCreate(tableName, schemaName, creator);
        }
        public ITableCreate<T> CreateTable<T>(string tableName = null, string schemaName = null)
        {
            if (tableName == null)
                tableName = typeof(T).Name;

            ITableCreator creator = null;
            if (_dbType == DatabaseType.Mssql) creator = new MssqlTableCreator(_builder, _executor, _typeMapper);
            else if (_dbType == DatabaseType.Postgis) creator = new PostgisTableCreator(_builder, _executor, _typeMapper);
            else throw new Exception("Veritabanı tipi bulunamadı");
            return new TableCreate<T>(tableName, schemaName, creator);
        }

        public ResultStatus AlterTable(TableInfo tableInfo)
        {
            ITableCreator creator = null;
            if (_dbType == DatabaseType.Mssql) creator = new MssqlTableCreator(_builder, _executor, _typeMapper);
            else if (_dbType == DatabaseType.Postgis) creator = new PostgisTableCreator(_builder, _executor, _typeMapper);
            else throw new Exception("Veritabanı tipi bulunamadı");
            return creator.Alter(tableInfo);
        }
        public ITableCreate AlterTable(string tableName, string schemaName = null)
        {
            ITableCreator creator = null;
            if (_dbType == DatabaseType.Mssql) creator = new MssqlTableCreator(_builder, _executor, _typeMapper);
            else if (_dbType == DatabaseType.Postgis) creator = new PostgisTableCreator(_builder, _executor, _typeMapper);
            else throw new Exception("Veritabanı tipi bulunamadı");
            return new TableAlter(tableName, schemaName, creator);
        }
        public ITableCreate<T> AlterTable<T>(string tableName = null, string schemaName = null)
        {
            if (tableName == null)
                tableName = typeof(T).Name;

            ITableCreator creator = null;
            if (_dbType == DatabaseType.Mssql) creator = new MssqlTableCreator(_builder, _executor, _typeMapper);
            else if (_dbType == DatabaseType.Postgis) creator = new PostgisTableCreator(_builder, _executor, _typeMapper);
            else throw new Exception("Veritabanı tipi bulunamadı");
            return new TableAlter<T>(tableName, schemaName, creator);
        }

        public ResultStatus DropTable(string tableName, string schemaName = null)
        {
            var query = _builder.GetDropTableQuery(tableName, schemaName);
            return _executor.ExecuteNonQuery(query);
        }
        public bool TableExists(string tableName, string schemaName = null)
        {
            var query = _builder.GetTableExistsQuery(tableName, schemaName);
            var isExists = _executor.ExecuteScaler<bool>(query);
            return isExists;
        }


        public TableInfo TableInfo(string tableName, bool onlyColumns = false)
        {
            ITableCreator creator = null;
            if (_dbType == DatabaseType.Mssql) creator = new MssqlTableCreator(_builder, _executor, _typeMapper);
            else if (_dbType == DatabaseType.Postgis) creator = new PostgisTableCreator(_builder, _executor, _typeMapper);
            else throw new Exception("Veritabanı tipi bulunamadı");
            return creator.GetSchema(tableName, onlyColumns);
        }

        public void Dispose()
        {
            _executor.Dispose();
        }




        public ResultStatus CreateFunction<T1, T2, T3, TResult>(string functionName, Expression<Func<T1, T2, T3, IQuery>> function)
        {

            return null;
        }

        public T CallFunction<T>(string functionName, params object[] parameters)
        {

            return default(T);
        }

    }
}
