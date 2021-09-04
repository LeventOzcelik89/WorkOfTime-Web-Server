using GeoAPI.Geometries;
using Infoline.Framework.Database.DataReader;
using NetTopologySuite.Features;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;

namespace Infoline.Framework.Database
{
    public interface IExecutable
    {
        IEnumerable<Dictionary<string, object>> Execute();
        IEnumerable<T> Execute<T>();
        FeatureCollection ExecuteFeature();
        T ExecuteScaler<T>();
        string GetQueryForTest();
    }
    public interface ISelect : IExecutable, IQuery
    {
        ISelect Select(params INamedItem[] columns);
        IWhere Where(BEXP logical_expression);
        IWhere Where(string text, params object[] parameters);
        IGroupBy GroupBy(params IQueryItem[] group_columns);
        IOrderBy OrderBy(params IQueryOrderItem[] columns);
        ISkip Skip(int? start);
        ITake Take(int? count);
    }
    public interface IWhere : IExecutable
    {
        ISelect Select(params INamedItem[] columns);
        int Count();
        IWhere Where(BEXP logical_expression);
        IWhere Where(string text, params object[] parameters);
        IGroupBy GroupBy(params IQueryItem[] group_columns);
        IOrderBy OrderBy(params IQueryOrderItem[] columns);
        ITake Take(int? count);
    }
    public interface IGroupBy : IExecutable
    {
        ISelect Select(params INamedItem[] columns);
        IWhere Where(BEXP logical_expression);
        IWhere Where(string text, params object[] parameters);
        IGroupBy GroupBy(params IQueryItem[] group_columns);
        IOrderBy OrderBy(params IQueryOrderItem[] columns);
        ITake Take(int? count);
    }
    public interface IOrderBy : IExecutable
    {
        ISkip Skip(int? start);
        ITake Take(int? count);
    }
    public interface ISkip : IExecutable
    {
        ITake Take(int? count);
    }
    public interface ITake : IExecutable
    {

    }
    public interface IGetTable : IExecutable
    {
        ISelect Select(params INamedItem[] columns);
        int Count();
        IWhere Where(BEXP logical_expression);
        IWhere Where(string text, params object[] parameters);
        IGroupBy GroupBy(params IQueryItem[] group_columns);
        IOrderBy OrderBy(params IQueryOrderItem[] columns);
        ISkip Skip(int? start);
        ITake Take(int? count);
        IEnumerable<T> ExecuteSimpleQuery<T>(SimpleQuery simpleQuery);

        ResultStatus Insert(Dictionary<string, object> parameter, string[] exceptCols = null);
        ResultStatus Update(Dictionary<string, object> parameter, string[] idCols, string[] exceptCols = null, bool setNull = false);
        ResultStatus Delete(Dictionary<string, object> parameter, string[] idCols);
        ResultStatus Delete(BEXP condition);

        ResultStatus Insert(IEnumerable<Dictionary<string, object>> parameter, string[] exceptCols = null);
        ResultStatus Update(IEnumerable<Dictionary<string, object>> parameter, string[] idCols, string[] exceptCols = null, bool setNull = false);
        ResultStatus Delete(IEnumerable<Dictionary<string, object>> parameter, string[] idCols);

        ResultStatus Insert(IFeature feature, string geometryColumnName);
        ResultStatus Update(IFeature feature, string geometryColumnName, string[] idCols, bool setNull = false);
        ResultStatus Insert(FeatureCollection collection, string geometryColumnName);
        ResultStatus BulkInsert(FeatureCollection collection, TableInfo tableInfo, string gEOMETRY_COLUMN_NAME);
    }
    public interface IGetTableFunction : IExecutable
    {
        ISelect Select(params INamedItem[] columns);
        int Count();
        IWhere Where(BEXP logical_expression);
        IWhere Where(string text, params object[] parameters);
        IGroupBy GroupBy(params IQueryItem[] group_columns);
        IOrderBy OrderBy(params IQueryOrderItem[] columns);
        ISkip Skip(int? start);
        ITake Take(int? count);
        IEnumerable<T> ExecuteSimpleQuery<T>(SimpleQuery simpleQuery);

        ResultStatus Insert(Dictionary<string, object> parameter, string[] exceptCols = null);
        ResultStatus Update(Dictionary<string, object> parameter, string[] idCols, string[] exceptCols = null, bool setNull = false);
        ResultStatus Delete(Dictionary<string, object> parameter, string[] idCols);

        ResultStatus Insert(IEnumerable<Dictionary<string, object>> parameter, string[] exceptCols = null);
        ResultStatus Update(IEnumerable<Dictionary<string, object>> parameter, string[] idCols, string[] exceptCols = null, bool setNull = false);
        ResultStatus Delete(IEnumerable<Dictionary<string, object>> parameter, string[] idCols);

        ResultStatus Insert(IFeature feature, string geometryColumnName);
        ResultStatus Insert(FeatureCollection collection, string geometryColumnName);
        ResultStatus BulkInsert(FeatureCollection collection, TableInfo tableInfo, string gEOMETRY_COLUMN_NAME);
    }


    public class QueryProcessor : IExecutable, IGetTableFunction, IGetTable, ISelect, IWhere, IGroupBy, IOrderBy, ISkip, ITake
    {
        private bool _isFunction = false;
        private string _tableName;
        private string _schemaName;
        private object[] _functionParameters;
        private List<QueryStatement> _statements;
        private List<object> _parameters;
        private IQueryBuilder _builder;
        private IQueryExecutor _executor;
        private ITypeMapper _typeMapper;


        public QueryProcessor(string tableName, string schemaName, IQueryBuilder builder, IQueryExecutor executor, ITypeMapper typeMapper)
        {
            _isFunction = false;
            _tableName = tableName;
            _schemaName = schemaName;
            _statements = new List<QueryStatement>();
            _parameters = new List<object>();
            _builder = builder;
            _executor = executor;
            _typeMapper = typeMapper;
        }
        public QueryProcessor(string functionName, string schemaName, object[] parameters, IQueryBuilder builder, IQueryExecutor executor, ITypeMapper typeMapper)
        {
            _isFunction = true;
            _tableName = functionName;
            _schemaName = schemaName;
            _functionParameters = parameters;
            _statements = new List<QueryStatement>();
            _parameters = new List<object>();
            _builder = builder;
            _executor = executor;
            _typeMapper = typeMapper;
        }
        public ISelect Select(params INamedItem[] columns)
        {
            if (columns == null)
                return this;
            _statements.Add(QueryStatement.Select);
            _parameters.Add(columns);
            return this;
        }
        public int Count()
        {
            _statements.Add(QueryStatement.Select);
            _parameters.Add(new INamedItem[] { new NEXP() { Name = "Count", Expression = COL.ALL.COUNT() } });
            return ExecuteScaler<int>();
        }
        public IWhere Where(BEXP logical_expression)
        {
            if (logical_expression == null)
                return this;
            _statements.Add(QueryStatement.Where);
            _parameters.Add(logical_expression);
            return this;
        }
        public IWhere Where(string text, params object[] parameters)
        {
            if (!string.IsNullOrEmpty(text))
            {
                _statements.Add(QueryStatement.Where);
                _parameters.Add(new { Text = text, Parameters = parameters });
            }
            return this;
        }
        public IGroupBy GroupBy(params IQueryItem[] group_columns)
        {
            _statements.Add(QueryStatement.GroupBy);
            _parameters.Add(group_columns);
            return this;
        }
        public IOrderBy OrderBy(params IQueryOrderItem[] columns)
        {
            if (columns == null)
                return this;
            if (_statements.Contains(QueryStatement.OrderBy))
            {
                var index = _statements.IndexOf(QueryStatement.OrderBy);
                _parameters[index] = ((IQueryOrderItem[])_parameters[index]).Union(columns).ToArray();
                return this;
            }
            _statements.Add(QueryStatement.OrderBy); ;
            _parameters.Add(columns);
            return this;
        }
        public ISkip Skip(int? start)
        {
            if (start.HasValue)
            {
                _statements.Add(QueryStatement.Skip);
                _parameters.Add(start.Value);
            }
            return this;
        }
        public ITake Take(int? count)
        {
            if (count.HasValue)
            {
                _statements.Add(QueryStatement.Take);
                _parameters.Add(count.Value);
            }
            return this;
        }

        public IEnumerable<Dictionary<string, object>> Execute()
        {
            var query = _builder.GetFetchQuery(_isFunction, _tableName, _schemaName, _functionParameters, _statements, _parameters);
            return _executor.ExecuteReader(query);
        }
        public IEnumerable<T> Execute<T>()
        {
            var query = _builder.GetFetchQuery(_isFunction, _tableName, _schemaName, _functionParameters, _statements, _parameters);
            return _executor.ExecuteReader<T>(query);
        }
        public T ExecuteScaler<T>()
        {
            var query = _builder.GetFetchQuery(_isFunction, _tableName, _schemaName, _functionParameters, _statements, _parameters);
            return _executor.ExecuteScaler<T>(query);
        }
        public string GetQueryForTest()
        {
            return _builder.GetFetchQueryForTest(_isFunction, _tableName, _schemaName, _functionParameters, _statements, _parameters);
        }


        public ResultStatus Insert(Dictionary<string, object> parameter, string[] exceptCols)
        {
            var query = _builder.GetInsertQuery(_tableName, _schemaName, parameter, exceptCols);
            return _executor.ExecuteNonQuery(query);
        }
        public ResultStatus Update(Dictionary<string, object> parameter, string[] idCols, string[] exceptCols = null, bool setNull = false)
        {
            var query = _builder.GetUpdateQuery(_tableName, _schemaName, parameter, idCols, exceptCols, setNull);
            return _executor.ExecuteNonQuery(query);
        }
        public ResultStatus Delete(Dictionary<string, object> parameter, string[] idCols)
        {
            var query = _builder.GetDeleteQuery(_tableName, _schemaName, parameter, idCols);
            return _executor.ExecuteNonQuery(query);
        }
        public ResultStatus Delete(BEXP condition)
        {
            var query = _builder.GetDeleteQuery(_tableName, _schemaName, condition);
            return _executor.ExecuteNonQuery(query);
        }
        public ResultStatus Insert(IEnumerable<Dictionary<string, object>> parameter, string[] exceptCols = null)
        {
            DbTransaction transaction = null;
            var result = true;
            var opentransaction = _executor.IsTransactionOpen;
            if (!opentransaction)
                transaction = _executor.BeginTransaction();
            var i = 0;
            foreach (var p in parameter)
            {
                result = result & Insert(p, exceptCols).result;
                if (result == false) break;
                i++;
            }
            if (!opentransaction)
            {
                if (result) transaction.Commit();
                else transaction.Rollback();
            }
            return new ResultStatus { result = result, message = i.ToString() };
        }
        public ResultStatus Update(IEnumerable<Dictionary<string, object>> parameter, string[] idCols, string[] exceptCols = null, bool setNull = false)
        {
            DbTransaction transaction = null;
            var result = true;
            var opentransaction = _executor.IsTransactionOpen;
            if (!opentransaction)
                transaction = _executor.BeginTransaction();
            var i = 0;
            foreach (var p in parameter)
            {
                result = result & Update(p, idCols, exceptCols, setNull).result;
                if (result == false) break;
                i++;
            }
            if (!opentransaction)
            {
                if (result) transaction.Commit();
                else transaction.Rollback();
            }
            return new ResultStatus { result = result, message = i.ToString() };
        }
        public ResultStatus Delete(IEnumerable<Dictionary<string, object>> parameter, string[] idCols)
        {
            DbTransaction transaction = null;
            var result = true;
            var opentransaction = _executor.IsTransactionOpen;
            if (!opentransaction)
                transaction = _executor.BeginTransaction();
            var i = 0;
            foreach (var p in parameter)
            {
                result = result & Delete(p, idCols).result;
                if (result == false) break;
                i++;
            }
            if (!opentransaction)
            {
                if (result) transaction.Commit();
                else transaction.Rollback();
            }
            return new ResultStatus { result = result, message = i.ToString() };
        }

        public IEnumerable<T> ExecuteSimpleQuery<T>(SimpleQuery simpleQuery)
        {
            if (simpleQuery.Skip.HasValue && simpleQuery.Take.HasValue)
                return Where(simpleQuery.Filter)
                            .Select(simpleQuery.Fields)
                            .OrderBy(simpleQuery.Sort)
                            .Skip(simpleQuery.Skip.Value)
                            .Take(simpleQuery.Take.Value).Execute<T>();
            else if (simpleQuery.Skip.HasValue)
                return Where(simpleQuery.Filter)
                            .Select(simpleQuery.Fields)
                            .OrderBy(simpleQuery.Sort)
                            .Skip(simpleQuery.Skip.Value).Execute<T>();
            else if (simpleQuery.Take.HasValue)
                return Where(simpleQuery.Filter)
                            .Select(simpleQuery.Fields)
                            .OrderBy(simpleQuery.Sort)
                            .Take(simpleQuery.Take.Value).Execute<T>();
            else
                return Where(simpleQuery.Filter)
                            .Select(simpleQuery.Fields)
                            .OrderBy(simpleQuery.Sort).Execute<T>();
        }

        public FeatureCollection ExecuteFeature()
        {
            var query = _builder.GetFetchQuery(_isFunction, _tableName, _schemaName, _functionParameters, _statements, _parameters);
            return _executor.ExecuteFeature(query);
        }

        public ResultStatus Insert(IFeature feature, string geometryColumnName)
        {
            var query = _builder.GetInsertQuery(_tableName, _schemaName, feature, geometryColumnName);
            return _executor.ExecuteNonQuery(query);
        }

        public ResultStatus Insert(FeatureCollection collection, string geometryColumnName)
        {
            if (_executor.IsSupportBulkInsert)
                return _executor.ExecuteBulkInsert(_tableName, _schemaName, collection, null, geometryColumnName);
            else
            {
                DbTransaction transaction = null;
                var result = true;
                var opentransaction = _executor.IsTransactionOpen;
                if (!opentransaction)
                    transaction = _executor.BeginTransaction();
                var i = 0;
                foreach (var feature in collection.Features)
                {
                    result = result & Insert(feature, geometryColumnName).result;
                    if (result == false) break;
                    i++;
                }
                if (!opentransaction)
                {
                    if (result) transaction.Commit();
                    else transaction.Rollback();
                }
                return new ResultStatus { result = result, message = i.ToString() };
            }

            //using (SqlConnection connection = new SqlConnection(_executor.ConnectionString))
            //{
            //    var reader = new FeatureCollectionDataReader(collection, geometryColumnName);
            //    SqlBulkCopy bulkCopy = new SqlBulkCopy(connection);
            //    if (connection.State != ConnectionState.Open)
            //        connection.Open();
            //
            //    var first = collection.Features.FirstOrDefault();
            //    foreach (var columnName in first.Attributes.GetNames())
            //        bulkCopy.ColumnMappings.Add(columnName, columnName);
            //    bulkCopy.ColumnMappings.Add(geometryColumnName, geometryColumnName);
            //    bulkCopy.BulkCopyTimeout = 200;
            //    bulkCopy.DestinationTableName = _tableName;
            //    bulkCopy.WriteToServer(reader);
            //
            //    connection.Close();
            //    return new ResultStatus { result = true };
            //}
        }

        public ResultStatus BulkInsert(FeatureCollection collection, TableInfo tableInfo, string geometryColumnName)
        {
            return _executor.ExecuteBulkInsert(_tableName, _schemaName, collection, tableInfo, geometryColumnName);
        }

        public ResultStatus Update(IFeature feature, string geometryColumnName, string[] idCols, bool setNull = false)
        {
            var query = _builder.GetUpdateQuery(_tableName, _schemaName, feature, geometryColumnName, idCols, setNull);
            return _executor.ExecuteNonQuery(query);
        }


        //private void WriteToDatabase()
        //{
        //    string connString = "";
        //    // connect to SQL
        //    using (SqlConnection connection = new SqlConnection(connString))
        //    {
        //        // make sure to enable triggers
        //        // more on triggers in next post
        //        SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
        //        // set the destination table name
        //        bulkCopy.DestinationTableName = this.tableName;
        //        connection.Open();
        //        // write the data in the “dataTable”
        //        bulkCopy.WriteToServer(dataTable);
        //        connection.Close();
        //    }
        //    // reset
        //    this.dataTable.Clear();
        //    this.recordCount = 0;
        //}

    }
    public enum QueryStatement
    {
        Select,
        Where,
        GroupBy,
        OrderBy,
        Skip,
        Take,
    }


    public interface IQueryItem
    {

    }
    public interface INamedItem : IQueryItem
    {
        string Name { get; set; }
    }
    public interface IQueryValue : IQueryItem
    {
        NEXP this[string val] { get; }
    }
    public interface IExpression
    {

    }
    public interface IQueryOrderItem : IQueryItem
    {
        IQueryValue Value { get; set; }
        QueryOrderType Type { get; }
    }


    public class COL : IQueryItem, IQueryValue, INamedItem
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public static COL ALL { get { return (COL)"*"; } }

        public COL(string name)
        {
            Name = name;
        }
        public COL(string name, object val)
        {
            Name = name;
            Value = val;
        }

        public static implicit operator COL(string name)
        {
            return new COL(name);
        }

        public static BEXP operator >(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThan };
        }
        public static BEXP operator <(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThan };
        }
        public static BEXP operator <=(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThanOrEqual };
        }
        public static BEXP operator >=(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThanOrEqual };
        }
        public static BEXP operator ==(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Equal };
        }
        public static BEXP operator !=(COL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.NotEqual };
        }

        public static BEXP operator >(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThan };
        }
        public static BEXP operator <(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThan };
        }
        public static BEXP operator <=(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThanOrEqual };
        }
        public static BEXP operator >=(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThanOrEqual };
        }
        public static BEXP operator ==(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Equal };
        }
        public static BEXP operator !=(COL op1, VAL op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.NotEqual };
        }



        public static TEXP operator +(COL op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Add };
        }
        public static TEXP operator -(COL op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Subtract };
        }
        public static TEXP operator *(COL op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Multiply };
        }
        public static TEXP operator /(COL op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Divide };
        }
        public static TEXP operator %(COL op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Modulo };
        }


        public NEXP this[string val]
        {
            get { return new NEXP { Name = val, Expression = this }; }
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }  // Column
    public class VAL : IQueryItem, IQueryValue
    {
        private object _value;

        public object Value
        {
            get { return _value ?? DBNull.Value; }
            set { _value = value; }
        }

        public VAL()
        {

        }
        public VAL(object value)
        {
            _value = value;
        }

        public static explicit operator VAL(string value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(int value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(float value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(double value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(bool value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(decimal value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(char value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(Guid value)
        {
            return new VAL(value);
        }
        public static implicit operator VAL(DateTime value)
        {
            return new VAL(value);
        }

        public static BEXP operator >(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThan };
        }
        public static BEXP operator <(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThan };
        }
        public static BEXP operator <=(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThanOrEqual };
        }
        public static BEXP operator >=(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThanOrEqual };
        }
        public static BEXP operator ==(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Equal };
        }
        public static BEXP operator !=(VAL op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.NotEqual };
        }


        public static FEXP PI()
        {
            return new FEXP { Function = QueryFunctions.PI };
        }
        public static FEXP RAND()
        {
            return new FEXP { Function = QueryFunctions.Rand };
        }
        public static FEXP GETDATE()
        {
            return new FEXP { Function = QueryFunctions.GetDate };
        }


        public NEXP this[string val]
        {
            get { return new NEXP { Name = val, Expression = this }; }
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }  // Value
    public class ARR : IQueryItem
    {
        public IQueryValue[] Values { get; set; }

    }
    [Serializable]
    public class BEXP : IQueryItem, IExpression
    {
        public IQueryItem Operand1 { get; set; }

        public IQueryItem Operand2 { get; set; }

        public BinaryOperator Operator { get; set; }

        public static BEXP operator &(BEXP op1, BEXP op2)
        {
            if (op1 != null && op2 != null) return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.And };
            if (op1 != null) return op1;
            if (op2 != null) return op2;
            return null;
        }
        public static BEXP operator |(BEXP op1, BEXP op2)
        {
            if (op1 != null && op2 != null) return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Or };
            if (op1 != null) return op1;
            if (op2 != null) return op2;
            return null;
        }
        public static BEXP operator !(BEXP op1)
        {
            return new BEXP { Operand1 = op1, Operator = BinaryOperator.Not };
        }

        public override string ToString()
        {
            switch (Operator)
            {
                case BinaryOperator.And: return string.Format("{0} and {1}", Operand1, Operand2);
                case BinaryOperator.Or: return string.Format("{0} or {1}", Operand1, Operand2);
                case BinaryOperator.Not: return string.Format("not {0}", Operand1);
                case BinaryOperator.Equal: return string.Format("{0} = {1}", Operand1, Operand2);
                case BinaryOperator.GreaterThan: return string.Format("{0} > {1}", Operand1, Operand2);
                case BinaryOperator.GreaterThanOrEqual: return string.Format("{0} >= {1}", Operand1, Operand2);
                case BinaryOperator.LessThan: return string.Format("{0} < {1}", Operand1, Operand2);
                case BinaryOperator.LessThanOrEqual: return string.Format("{0} <= {1}", Operand1, Operand2);
                case BinaryOperator.NotEqual: return string.Format("{0} != {1}", Operand1, Operand2);
                case BinaryOperator.Like: return string.Format("{0} like {1}", Operand1, Operand2);
                case BinaryOperator.NotLike: return string.Format("{0} not like {1}", Operand1, Operand2);
                case BinaryOperator.In: return string.Format("{0} in {1}", Operand1, Operand2);
                default:
                    break;
            }
            return "";
        }


        public object GetSerializeObject()
        {
            return new
            {
                Operator = Operator.ToString(),
                Operand1 = (Operand1 is BEXP ? ((BEXP)Operand1).GetSerializeObject() : ((COL)Operand1).Name),
                Operand2 = (Operand2 is BEXP ? ((BEXP)Operand2).GetSerializeObject() : ((VAL)Operand2).Value)
            };
        }


    } // Binary Expression
    public class TEXP : IQueryItem, IQueryValue, IExpression
    {
        public IQueryItem Operand1 { get; set; }
        public IQueryItem Operand2 { get; set; }
        public TransformOperator Operator { get; set; }

        public static BEXP operator >(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThan };
        }
        public static BEXP operator <(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThan };
        }
        public static BEXP operator <=(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThanOrEqual };
        }
        public static BEXP operator >=(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThanOrEqual };
        }
        public static BEXP operator ==(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Equal };
        }
        public static BEXP operator !=(TEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.NotEqual };
        }


        public static TEXP operator +(TEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Add };
        }
        public static TEXP operator -(TEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Subtract };
        }
        public static TEXP operator *(TEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Multiply };
        }
        public static TEXP operator /(TEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Divide };
        }
        public static TEXP operator %(TEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Modulo };
        }


        public NEXP this[string val]
        {
            get { return new NEXP { Name = val, Expression = this }; }
        }

        public override string ToString()
        {
            switch (Operator)
            {
                case TransformOperator.Add: return string.Format("{0} + {1}", Operand1, Operand2);
                case TransformOperator.Divide: return string.Format("{0} / {1}", Operand1, Operand2);
                case TransformOperator.Modulo: return string.Format("{0} % {1}", Operand1, Operand2);
                case TransformOperator.Multiply: return string.Format("{0} * {1}", Operand1, Operand2);
                case TransformOperator.Negate: return string.Format("-1 * {0}", Operand1);
                case TransformOperator.Power: return string.Format("{0} ^ {1}", Operand1, Operand2);
                case TransformOperator.Subtract: return string.Format("{0} - {1}", Operand1, Operand2);
                case TransformOperator.Lambda:
                    break;
                case TransformOperator.Conditional:
                    break;
                case TransformOperator.OnesComplement:
                    break;
                case TransformOperator.ExclusiveOr:
                    break;
                default:
                    break;
            }
            return string.Format("Operator: {0}, Operand1: {1}, Operand2: {2}", Operator, Operand1, Operand2);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        //public static NEXP operator >>(TEXP op1, string op2)
        //{
        //    return new NEXP { Expression = op1, Name = op2 };
        //}

    } // Transform Expression
    public class FEXP : IQueryItem, IQueryValue, IExpression
    {
        public QueryFunctions Function { get; set; }
        public IQueryValue[] Parameters { get; set; }

        public NEXP this[string val]
        {
            get { return new NEXP { Name = val, Expression = this }; }
        }


        public static BEXP operator >(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThan };
        }
        public static BEXP operator <(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThan };
        }
        public static BEXP operator <=(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.LessThanOrEqual };
        }
        public static BEXP operator >=(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.GreaterThanOrEqual };
        }
        public static BEXP operator ==(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Equal };
        }
        public static BEXP operator !=(FEXP op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.NotEqual };
        }


        public static TEXP operator +(FEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Add };
        }
        public static TEXP operator -(FEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Subtract };
        }
        public static TEXP operator *(FEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Multiply };
        }
        public static TEXP operator /(FEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Divide };
        }
        public static TEXP operator %(FEXP op1, IQueryValue op2)
        {
            return new TEXP { Operand1 = op1, Operand2 = op2, Operator = TransformOperator.Modulo };
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    } // Function Expression
    public class NEXP : IQueryItem, IQueryValue, INamedItem
    {
        public string Name { get; set; }
        public IQueryValue Expression { get; set; }

        public NEXP this[string val]
        {
            get { return new NEXP { Name = val, Expression = this }; }
        }

    } // Named Expression


    public class Sort : IQueryOrderItem
    {
        public IQueryValue Value { get; set; }
        public QueryOrderType Type { get; set; }
    }

    public class ASC : IQueryOrderItem
    {
        public IQueryValue Value { get; set; }
        public QueryOrderType Type { get { return QueryOrderType.ASC; } }

        public static explicit operator ASC(COL value)
        {
            return new ASC { Value = value };
        }
        public static explicit operator ASC(VAL value)
        {
            return new ASC { Value = value };
        }
        public static explicit operator ASC(TEXP value)
        {
            return new ASC { Value = value };
        }
        public static explicit operator ASC(NEXP value)
        {
            return new ASC { Value = value };
        }
    }
    public class DESC : IQueryOrderItem
    {
        public IQueryValue Value { get; set; }
        public QueryOrderType Type { get { return QueryOrderType.DESC; } }

        public static explicit operator DESC(COL value)
        {
            return new DESC { Value = value };
        }
        public static explicit operator DESC(VAL value)
        {
            return new DESC { Value = value };
        }
        public static explicit operator DESC(TEXP value)
        {
            return new DESC { Value = value };
        }
        public static explicit operator DESC(NEXP value)
        {
            return new DESC { Value = value };
        }
    }
    public enum QueryOrderType
    {
        ASC,
        DESC,
    }

    public enum BinaryOperator
    {
        And,
        Or,
        Not,

        Equal,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        NotEqual,
        Like,
        NotLike,

        In,
        IsNull,
        IsNotNull,
    }
    public enum TransformOperator
    {
        Add,
        Divide,
        Modulo,
        Multiply,
        Negate,
        Power,
        Subtract,
        Lambda,
        Conditional,
        OnesComplement,
        ExclusiveOr,
    }

    public enum Datepart
    {
        YEAR,
        MONTH,
        DAY,
        HOUR,
    }

    public enum QueryFunctions
    {

        // String Functions
        Ascii,
        Char,
        CharIndex,
        Concat,
        Difference,
        Format,
        Left,
        Len,
        Lower,
        Ltrim,
        Nchar,
        Patindex,
        Quotename,
        Replace,
        Replicate,
        Reverse,
        Right,
        Rtrim,
        Trim,
        Soundex,
        Space,
        Str,
        String_Escape,
        String_Split,
        Stuff,
        Substring,
        Unicode,
        Upper,

        // Math Functions
        Abs,
        Acos,
        Asin,
        Atan,
        Atn2,
        Ceiling,
        Cos,
        Cot,
        Degrees,
        Exp,
        Floor,
        Log,
        PI,
        Power,
        Radians,
        Rand,
        Round,
        Sign,
        Sin,
        Sqrt,
        Square,
        Tan,

        // Datetime Functions
        GetDate,
        Datepart,

        // Geometry
        STArea,
        STAsBinary,
        STAsText,
        STBoundary,    // Geometry
        STBuffer,
        STCentroid,    // Geometry
        STContains,    // Geometry
        STConvexHull,  // Geometry
        STCrosses,     // Geometry
        STCurveN,
        STCurveToLine,
        STDifference,
        STDimension,
        STDisjoint,
        STDistance,
        STEndpoint,
        STEnvelope,   // Geometry
        STEquals,
        STExteriorRing,   // Geometry
        STGeometryN,
        STGeometryType,
        STInteriorRingN,  // Geometry
        STIntersection,
        STIntersects,
        STIsClosed,
        STIsEmpty,
        STIsRing,     // Geometry
        STIsSimple,   // Geometry
        STIsValid,
        STLength,
        STNumCurves,
        STNumGeometries,
        STNumInteriorRing,  // Geometry
        STNumPoints,
        STOverlaps,         // Geometry
        STPointN,
        STPointOnSurface,   // Geometry
        STRelate,           // Geometry
        STSrid,
        STStartPoint,
        STSymDifference,
        STTouches,          // Geometry
        STUnion,
        STWithin,           // Geometry
        STX,                // Geometry
        STY,                // Geometry
        STGeomFromText,     // Static, Geography
        STPointFromText,    // Static, Geography
        STLineFromText,     // Static, Geography
        STPolyFromText,     // Static, Geography
        STMPointFromText,   // Static, Geography
        STMLineFromText,    // Static, Geography
        STMPolyFromText,    // Static, Geography
        STGeomCollFromText, // Static, Geography
        STGeomFromWKB,      // Static, Geography
        STPointFromWKB,     // Static, Geography
        STLineFromWKB,      // Static, Geography
        STPolyFromWKB,      // Static, Geography
        STMPointFromWKB,    // Static, Geography
        STMLineFromWKB,     // Static, Geography
        STMPolyFromWKB,     // Static, Geography
        STGeomCollFromWKB,  // Static, Geography
        GeomFromGML,        // Static, Geography
        AsBinaryZM,
        AsGml,
        AsTextZM,
        BufferWithCurves,
        BufferWithTolerance,
        CurveToLineWithTolerance,
        InstanceOf,
        Filter,
        HasM,
        HasZ,
        IsNull,
        IsValidDetailed,
        M,
        MakeValid,
        MinDbCompatibilityLevel,
        Reduce,
        ShortestLineTo,
        ToString,
        Z,
        EnvelopeAngle,
        EnvelopeCenter,
        Lat,
        Long,
        NumRing,
        ReorientObject,
        RingN,


        // Aggregate Functions
        Avg,
        Checksum_Agg,
        Count,
        Count_Big,
        Grouping,
        Grouping_Id,
        Max,
        Min,
        Sum,
        Stdev,
        Stdevp,
        Var,
        Varp,
        CollectionAggregate, // Geography, Static
        ConvexHullAggregate, // Geography, Static
        EnvelopeAggregate,   // Geography, Static
        UnionAggregate,      // Geography, Static
    }
    public static class QueryFunctionExp
    {
        public static BEXP LIKE(this IQueryValue op1, IQueryValue op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = op2, Operator = BinaryOperator.Like };
        }
        public static BEXP IN(this IQueryValue op1, params IQueryValue[] op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = new ARR { Values = op2.ToArray() }, Operator = BinaryOperator.In };
        }
        public static BEXP IN(this IQueryValue op1, IEnumerable<IQueryValue> op2)
        {
            return new BEXP { Operand1 = op1, Operand2 = new ARR { Values = op2.ToArray() }, Operator = BinaryOperator.In };
        }
        //public static BEXP IN(this IQueryValue op1, IExecutable query)
        //{
        //    return new BEXP { Operator = BinaryOperator.In, Operand1 = op1, Operand2 = query };
        //}
        public static TEXP LOOKUP(this IQueryValue op, string returnValue, string destinationTable, string destinationColumn)
        {
            throw new NotImplementedException();
        }
        public static BEXP ISNULL(this IQueryValue op1)
        {
            return new BEXP { Operator = BinaryOperator.IsNull, Operand1 = op1 };
        }
        public static BEXP ISNOTNULL(this IQueryValue op1)
        {
            return new BEXP { Operator = BinaryOperator.IsNotNull, Operand1 = op1 };
        }


        /// <summary>
        /// String veya char ifadenin ilk karakterinin ASCII kodunu verir.
        /// </summary>
        /// <param name="character_expression">String, char alabilir. Diğer türler atanırsa string ifade  gibi işlen görür.</param>
        /// <returns>int</returns>
        public static FEXP ASCII(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Ascii, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// Verilen sayının ASCII karakter karşılığını verir. 255 ten sonra null döndürür.
        /// </summary>
        /// <param name="integer_expression">Sayısal değer. Noktalı sayı gelirse virgülden sonrası alınır.</param>
        /// <returns></returns>
        public static FEXP CHAR(this IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Char, Parameters = new[] { integer_expression } };
        }

        /// <summary>
        /// Verilen ifadenin diğer ifade içindeki konumunu döndürür. MSSQL de stringin karakterlerini indexlemeye 1 den başlar.
        /// </summary>
        /// <param name="expressionToFind">Aradığımız ifade.</param>
        /// <param name="expressionToSearch">İfadeyi içinde aradığımız metin.</param>
        /// <returns></returns>
        public static FEXP CHARINDEX(this IQueryValue expressionToFind, IQueryValue expressionToSearch)
        {
            return new FEXP { Function = QueryFunctions.CharIndex, Parameters = new[] { expressionToFind, expressionToSearch } };
        }

        /// <summary>
        /// Verilen ifadenin diğer ifade içindeki konumunu döndürür. MSSQL de stringin karakterlerini indexlemeye 1 den başlar.
        /// </summary>
        /// <param name="expressionToFind">Aradığımız ifade.</param>
        /// <param name="expressionToSearch">İfadeyi içinde aradığımız metin.</param>
        /// <param name="start_location">Aramaya başlanacak konum.</param>
        /// <returns></returns>
        public static FEXP CHARINDEX(this IQueryValue expressionToFind, IQueryValue expressionToSearch, IQueryValue start_location)
        {
            return new FEXP { Function = QueryFunctions.CharIndex, Parameters = new[] { expressionToFind, expressionToSearch, start_location } };
        }

        /// <summary>
        /// Verilen string ifadeleri birleştirir.
        /// </summary>
        /// <param name="string_value1">String değer alır.</param>
        /// <param name="string_value2">String değer alır.</param>
        /// <param name="string_valueN">String değer listesi alır.</param>
        /// <returns></returns>
        public static FEXP CONCAT(this IQueryValue string_value1, IQueryValue string_value2, params IQueryValue[] string_valueN)
        {
            return new FEXP { Function = QueryFunctions.Concat, Parameters = new[] { string_value1, string_value2 }.Union(string_valueN).ToArray() };
        }

        /// <summary>
        /// Verilen iki karakterin SOUNDEX değerleri arasındaki farkı sayısal olarak verir.
        /// </summary>
        /// <param name="character_expression1">String ifade.</param>
        /// <param name="character_expression2">String ifade.</param>
        /// <returns>integer</returns>
        public static FEXP DIFFERENCE(this IQueryValue character_expression1, IQueryValue character_expression2)
        {
            return new FEXP { Function = QueryFunctions.Difference, Parameters = new[] { character_expression1, character_expression2 } };
        }

        /// <summary>
        /// Sayısal değerler ve tarihler için formatlama işlemi yapar.
        /// </summary>
        /// <param name="value">Formatlanacak değer.</param>
        /// <param name="format">Format.</param>
        /// <returns></returns>
        public static FEXP FORMAT(this IQueryValue value, IQueryValue format)
        {
            return new FEXP { Function = QueryFunctions.Format, Parameters = new[] { value, format } };
        }

        /// <summary>
        /// Sayısal değerler ve tarihler için formatlama işlemi yapar.
        /// </summary>
        /// <param name="value">Formatlanacak değer.</param>
        /// <param name="format">Format.</param>
        /// <param name="culture">Kültür seçimi.</param>
        /// <returns></returns>
        public static FEXP FORMAT(this IQueryValue value, IQueryValue format, IQueryValue culture)
        {
            return new FEXP { Function = QueryFunctions.Format, Parameters = new[] { value, format, culture } };
        }

        /// <summary>
        /// Soldan ikinci parametre ile belirtilen sayıda karakter döndürür.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <param name="integer_expression">Karakter sayısı.</param>
        /// <returns></returns>
        public static FEXP LEFT(this IQueryValue character_expression, IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Left, Parameters = new[] { character_expression, integer_expression } };
        }

        /// <summary>
        /// String ifadenin uzunluğunu döndürür.
        /// </summary>
        /// <param name="string_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP LEN(this IQueryValue string_expression)
        {
            return new FEXP { Function = QueryFunctions.Len, Parameters = new[] { string_expression } };
        }

        /// <summary>
        /// Verilen string ifadeyi küçük harfe çevirir.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP LOWER(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Lower, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// Soldaki boşlukları siler.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP LTRIM(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Ltrim, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// baştaki ve sondaki boşlukları siler.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP TRIM(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Trim, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// Verilen sayının Unicode karakter karşılığını döndürür.
        /// </summary>
        /// <param name="integer_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP NCHAR(this IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Nchar, Parameters = new[] { integer_expression } };
        }

        /// <summary>
        /// Verilen paternin ifade başlangıç indisini döndürür döndürür. Mssql de karakter indisleri 1 den başlar.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static FEXP PATINDEX(this IQueryValue pattern, IQueryValue expression)
        {
            return new FEXP { Function = QueryFunctions.Patindex, Parameters = new[] { pattern, expression } };
        }

        /// <summary>
        /// Returns a Unicode string with the delimiters added to make the input string a valid SQL Server delimited identifier.
        /// </summary>
        /// <param name="character_string">String ifade</param>
        /// <returns></returns>
        public static FEXP QUOTENAME(this IQueryValue character_string)
        {
            return new FEXP { Function = QueryFunctions.Quotename, Parameters = new[] { character_string } };
        }

        /// <summary>
        /// Returns a Unicode string with the delimiters added to make the input string a valid SQL Server delimited identifier.
        /// </summary>
        /// <param name="character_string">String ifade.</param>
        /// <param name="quote_character">Uzunluğu 1 olan string ifade.</param>
        /// <returns></returns>
        public static FEXP QUOTENAME(this IQueryValue character_string, IQueryValue quote_character)
        {
            return new FEXP { Function = QueryFunctions.Quotename, Parameters = new[] { character_string, quote_character } };
        }

        /// <summary>
        /// Verilen ifadeyi yeni ifade ile değiştirir.
        /// </summary>
        /// <param name="string_expression">String ifade.</param>
        /// <param name="string_pattern">Değiştirilecek ifade.</param>
        /// <param name="string_replacement">Eski ifade.</param>
        /// <returns></returns>
        public static FEXP REPLACE(this IQueryValue string_expression, IQueryValue string_pattern, IQueryValue string_replacement)
        {
            return new FEXP { Function = QueryFunctions.Replace, Parameters = new[] { string_expression, string_pattern, string_replacement } };
        }

        /// <summary>
        /// Verilen ifadeyi verilen sayıda tekrarlar.
        /// </summary>
        /// <param name="string_expression">ifade.</param>
        /// <param name="integer_expression">Tekrar sayısı.</param>
        /// <returns></returns>
        public static FEXP REPLICATE(this IQueryValue string_expression, IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Replicate, Parameters = new[] { string_expression, integer_expression } };
        }

        /// <summary>
        /// Verilen string ifadeyi ters çevirir.
        /// </summary>
        /// <param name="string_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP REVERSE(this IQueryValue string_expression)
        {
            return new FEXP { Function = QueryFunctions.Reverse, Parameters = new[] { string_expression } };
        }

        /// <summary>
        /// Sağdan ikinci parametre ile belirtilen sayıda karakter döndürür.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <param name="integer_expression">Karakter sayısı.</param>
        /// <returns></returns>
        public static FEXP RIGHT(this IQueryValue character_expression, IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Right, Parameters = new[] { character_expression, integer_expression } };
        }

        /// <summary>
        /// Sağdaki boşlukları siler.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP RTRIM(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Rtrim, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// Stringlerin benzerliğini değerlendirmek için dört karakterli (SOUNDEX) kod döndürür.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP SOUNDEX(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Soundex, Parameters = new[] { character_expression } };
        }

        /// <summary>
        /// Verilen sayıda boşluk üretir.
        /// </summary>
        /// <param name="integer_expression">Sayısal ifade.</param>
        /// <returns></returns>
        public static FEXP SPACE(this IQueryValue integer_expression)
        {
            return new FEXP { Function = QueryFunctions.Space, Parameters = new[] { integer_expression } };
        }

        /// <summary>
        /// Verilen sayısal ifadeyi stringe çevirir.
        /// </summary>
        /// <param name="float_expression">Sayısal ifade.</param>
        /// <returns></returns>
        public static FEXP STR(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Str, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Verilen sayısal ifadeyi stringe çevirir.
        /// </summary>
        /// <param name="float_expression">Sayısal ifade.</param>
        /// <param name="length">Toplam uzunluk. Varsayılan değer 10 dur. Eğer sayısal ifadenin uzunluğu toplam uzunluktan küçük ise sol tarafa boşluk ekler.</param>
        /// <returns></returns>
        public static FEXP STR(this IQueryValue float_expression, IQueryValue length)
        {
            return new FEXP { Function = QueryFunctions.Str, Parameters = new[] { float_expression, length } };
        }

        /// <summary>
        /// Verilen sayısal ifadeyi stringe çevirir.
        /// </summary>
        /// <param name="float_expression">Sayısal ifade.</param>
        /// <param name="length">Toplam uzunluk. Varsayılan değer 10 dur. Eğer sayısal ifadenin uzunluğu toplam uzunluktan küçük ise sol tarafa boşluk ekler.</param>
        /// <returns></returns>
        /// <param name="decima">Virgülden sonraki karakter sayısı.</param>
        /// <returns></returns>
        public static FEXP STR(this IQueryValue float_expression, IQueryValue length, IQueryValue decima)
        {
            return new FEXP { Function = QueryFunctions.Str, Parameters = new[] { float_expression, length, decima } };
        }

        /// <summary>
        /// MSSQL2016 da çalışıyor.
        /// Escapes special characters in texts and returns text with escaped characters. STRING_ESCAPE is a deterministic function.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static FEXP STRING_ESCAPE(this IQueryValue text, IQueryValue type)
        {
            return new FEXP { Function = QueryFunctions.String_Escape, Parameters = new[] { text, type } };
        }

        /// <summary>
        /// MSSQL2016 da çalışıyor.
        /// String ifadeyi verilen karaktere göre böler.
        /// Splits the character expression using specified separator.
        /// </summary>
        /// <param name="string_expression"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static FEXP STRING_SPLIT(this IQueryValue string_expression, IQueryValue separator)
        {
            return new FEXP { Function = QueryFunctions.String_Split, Parameters = new[] { string_expression, separator } };
        }

        /// <summary>
        /// Başlangıç ve uzunluk değeri ile belirtilen yere yeni string eklenir.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <param name="start">Başlangıç.</param>
        /// <param name="length">Uzunluk.</param>
        /// <param name="replaceWith_expression">Yeni ifade.</param>
        /// <returns></returns>
        public static FEXP STUFF(this IQueryValue character_expression, IQueryValue start, IQueryValue length, IQueryValue replaceWith_expression)
        {
            return new FEXP { Function = QueryFunctions.Stuff, Parameters = new[] { character_expression, start, length, replaceWith_expression } };
        }

        /// <summary>
        /// String ifade içinde başlangıç ve uzunluk değerleri ile ifade edilen konumdaki ifadeyi döndürür.
        /// </summary>
        /// <param name="expression">String ifade.</param>
        /// <param name="start">Başlangıç.</param>
        /// <returns></returns>
        public static FEXP SUBSTRING(this IQueryValue expression, IQueryValue start)
        {
            return new FEXP { Function = QueryFunctions.Substring, Parameters = new[] { expression, start } };
        }

        /// <summary>
        /// String ifade içinde başlangıç ve uzunluk değerleri ile ifade edilen konumdaki ifadeyi döndürür.
        /// </summary>
        /// <param name="expression">String ifade.</param>
        /// <param name="start">Başlangıç.</param>
        /// <param name="length">Uzunluk.</param>
        /// <returns></returns>
        public static FEXP SUBSTRING(this IQueryValue expression, IQueryValue start, IQueryValue length)
        {
            return new FEXP { Function = QueryFunctions.Substring, Parameters = new[] { expression, start, length } };
        }

        /// <summary>
        /// String veya char ifadenin ilk karakterinin Unicode kodunu verir.
        /// </summary>
        /// <param name="ncharacter_expression">String, char alabilir. Diğer türler atanırsa string ifade  gibi işlen görür.</param>
        /// <returns></returns>
        public static FEXP UNICODE(this IQueryValue ncharacter_expression)
        {
            return new FEXP { Function = QueryFunctions.Unicode, Parameters = new[] { ncharacter_expression } };
        }

        /// <summary>
        /// Verilen string ifadeyi büyük harfe çevirir.
        /// </summary>
        /// <param name="character_expression">String ifade.</param>
        /// <returns></returns>
        public static FEXP UPPER(this IQueryValue character_expression)
        {
            return new FEXP { Function = QueryFunctions.Upper, Parameters = new[] { character_expression } };
        }




        /// <summary>
        /// Akosinüs işlemi. Radyan değer döndürür.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP ACOS(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Acos, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Asinüs işlemi. Radyan değer döndürür.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP ASIN(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Asin, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Atanjant işlemi. Radyan değer döndürür.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP ATAN(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Atan, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Atanjant2 işlemi. Radyan değer döndürür.
        /// </summary>
        /// <param name="float_expression1">Sayısal değer.</param>
        /// <param name="float_expression2">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP ATN2(this IQueryValue float_expression1, IQueryValue float_expression2)
        {
            return new FEXP { Function = QueryFunctions.Atn2, Parameters = new[] { float_expression1, float_expression2 } };
        }

        /// <summary>
        /// Kosinüs işlemi. Radyan değer alır.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP COS(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Cos, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Kotanjant işlemi. Radyan değer alır.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP COT(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Cot, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Sinüs fonksiyonu. Radyan değer alır.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP SIN(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Sin, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Tanjant işlemi. Radyan değer alır.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP TAN(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Tan, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Radyan olarak verilen değer dereceye çevirir.
        /// </summary>
        /// <param name="numeric_expression">Radyan değer.</param>
        /// <returns></returns>
        public static FEXP DEGREES(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Degrees, Parameters = new[] { numeric_expression } };
        }

        /// <summary>
        /// Derece olarak verilen değeri radyana çevirir.
        /// </summary>
        /// <param name="numeric_expression">Derece değer.</param>
        /// <returns></returns>
        public static FEXP RADIANS(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Radians, Parameters = new[] { numeric_expression } };
        }


        /// <summary>
        /// Verilen sayısal değerin mutlak değerini alır.
        /// </summary>
        /// <param name="numeric_expression">Sayısal değer</param>
        /// <returns></returns>
        public static FEXP ABS(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Abs, Parameters = new[] { numeric_expression } };
        }

        /// <summary>
        /// Yukarı yuvarlama.
        /// </summary>
        /// <param name="numeric_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP CEILING(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Ceiling, Parameters = new[] { numeric_expression } };
        }

        /// <summary>
        /// Aşağı yuvarlama.
        /// </summary>
        /// <param name="numeric_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP FLOOR(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Floor, Parameters = new[] { numeric_expression } };
        }

        /// <summary>
        /// Yuvarlama işlemi
        /// </summary>
        /// <param name="numeric_expression">Yuvarlancak sayı.</param>
        /// <returns></returns>
        public static FEXP ROUND(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Round, Parameters = new[] { numeric_expression } };
        }

        /// <summary>
        /// Yuvarlama işlemi
        /// </summary>
        /// <param name="numeric_expression">Yuvarlancak sayı.</param>
        /// <param name="length">Virgülden sonraki basamak sayısı.</param>
        /// <returns></returns>
        public static FEXP ROUND(this IQueryValue numeric_expression, IQueryValue length)
        {
            return new FEXP { Function = QueryFunctions.Round, Parameters = new[] { numeric_expression, length } };
        }

        /// <summary>
        /// Yuvarlama işlemi.
        /// </summary>
        /// <param name="numeric_expression">Yuvarlancak sayı.</param>
        /// <param name="length">Virgülden sonraki basamak sayısı.</param>
        /// <param name="truncate">0 - ise yuvarlar, diğer sayılarda keser.</param>
        /// <returns></returns>
        public static FEXP ROUND(this IQueryValue numeric_expression, IQueryValue length, IQueryValue truncate)
        {
            return new FEXP { Function = QueryFunctions.Round, Parameters = new[] { numeric_expression, length, truncate } };
        }

        /// <summary>
        /// İşaret fonksiyonu.
        /// </summary>
        /// <param name="numeric_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP SIGN(this IQueryValue numeric_expression)
        {
            return new FEXP { Function = QueryFunctions.Sign, Parameters = new[] { numeric_expression } };
        }


        /// <summary>
        /// e^x işlemi
        /// </summary>
        /// <param name="float_expression">Üs. Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP EXP(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Exp, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Doğal logaritma işlemi.
        /// </summary>
        /// <param name="float_expression">Logaritması alınacak sayı.</param>
        /// <returns></returns>
        public static FEXP LOG(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Log, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Logaritma işlemi
        /// </summary>
        /// <param name="float_expression">Logaritması alınacak sayı.</param>
        /// <param name="base1">Taban.</param>
        /// <returns></returns>
        public static FEXP LOG(this IQueryValue float_expression, IQueryValue base1)
        {
            return new FEXP { Function = QueryFunctions.Log, Parameters = new[] { float_expression, base1 } };
        }

        /// <summary>
        /// Üs alma işlemi.
        /// </summary>
        /// <param name="float_expression">Üs alınacak sayı.</param>
        /// <param name="y">Üs.</param>
        /// <returns></returns>
        public static FEXP POWER(this IQueryValue float_expression, IQueryValue y)
        {
            return new FEXP { Function = QueryFunctions.Power, Parameters = new[] { float_expression, y } };
        }

        /// <summary>
        /// Karekök işlemi.
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP SQRT(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Sqrt, Parameters = new[] { float_expression } };
        }

        /// <summary>
        /// Kare işlemi
        /// </summary>
        /// <param name="float_expression">Sayısal değer.</param>
        /// <returns></returns>
        public static FEXP SQUARE(this IQueryValue float_expression)
        {
            return new FEXP { Function = QueryFunctions.Square, Parameters = new[] { float_expression } };
        }




        public static FEXP STArea(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STArea, Parameters = new[] { col } }; }
        public static FEXP STAsBinary(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STAsBinary, Parameters = new[] { col } }; }
        public static FEXP STAsText(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STAsText, Parameters = new[] { col } }; }
        public static FEXP STBoundary(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STBoundary, Parameters = new[] { col } }; }
        public static FEXP STBuffer(this IQueryValue col, IQueryValue distance) { return new FEXP { Function = QueryFunctions.STBuffer, Parameters = new[] { col, distance } }; }
        public static FEXP STCentroid(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STCentroid, Parameters = new[] { col } }; }
        public static FEXP STContains(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STContains, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STConvexHull(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STConvexHull, Parameters = new[] { col } }; }
        public static FEXP STCrosses(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STCrosses, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STCurveN(this IQueryValue col, IQueryValue curve_index) { return new FEXP { Function = QueryFunctions.STCurveN, Parameters = new[] { col, curve_index } }; }
        public static FEXP STCurveToLine(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STCurveToLine, Parameters = new[] { col } }; }
        public static FEXP STDifference(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STDifference, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STDimension(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STDimension, Parameters = new[] { col } }; }
        public static FEXP STDisjoint(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STDisjoint, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STDistance(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STDistance, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STEndpoint(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STEndpoint, Parameters = new[] { col } }; }
        public static FEXP STEnvelope(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STEnvelope, Parameters = new[] { col } }; }
        public static FEXP STEquals(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STEquals, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STExteriorRing(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STExteriorRing, Parameters = new[] { col } }; }
        public static FEXP STGeometryN(this IQueryValue col, IQueryValue expression) { return new FEXP { Function = QueryFunctions.STGeometryN, Parameters = new[] { col, expression } }; }
        public static FEXP STGeometryType(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STGeometryType, Parameters = new[] { col } }; }
        public static FEXP STInteriorRingN(this IQueryValue col, IQueryValue expression) { return new FEXP { Function = QueryFunctions.STInteriorRingN, Parameters = new[] { col, expression } }; }
        public static FEXP STIntersection(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STIntersection, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STIntersects(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STIntersects, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STIsClosed(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STIsClosed, Parameters = new[] { col } }; }
        public static FEXP STIsEmpty(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STIsEmpty, Parameters = new[] { col } }; }
        public static FEXP STIsRing(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STIsRing, Parameters = new[] { col } }; }
        public static FEXP STIsSimple(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STIsSimple, Parameters = new[] { col } }; }
        public static FEXP STIsValid(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STIsValid, Parameters = new[] { col } }; }
        public static FEXP STLength(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STLength, Parameters = new[] { col } }; }
        public static FEXP STNumCurves(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STNumCurves, Parameters = new[] { col } }; }
        public static FEXP STNumGeometries(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STNumGeometries, Parameters = new[] { col } }; }
        public static FEXP STNumInteriorRing(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STNumInteriorRing, Parameters = new[] { col } }; }
        public static FEXP STNumPoints(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STNumPoints, Parameters = new[] { col } }; }
        public static FEXP STOverlaps(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STOverlaps, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STPointN(this IQueryValue col, IQueryValue expression) { return new FEXP { Function = QueryFunctions.STPointN, Parameters = new[] { col, expression } }; }
        public static FEXP STPointOnSurface(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STPointOnSurface, Parameters = new[] { col } }; }
        public static FEXP STRelate(this IQueryValue col, IQueryValue other_geometry, IQueryValue intersection_pattern_matrix) { return new FEXP { Function = QueryFunctions.STRelate, Parameters = new[] { col, other_geometry, intersection_pattern_matrix } }; }
        public static FEXP STSrid(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STSrid, Parameters = new[] { col } }; }
        public static FEXP STStartPoint(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STStartPoint, Parameters = new[] { col } }; }
        public static FEXP STSymDifference(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STSymDifference, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STTouches(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STTouches, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STUnion(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STUnion, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STWithin(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.STWithin, Parameters = new[] { col, other_geometry } }; }
        public static FEXP STX(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STX, Parameters = new[] { col } }; }
        public static FEXP STY(this IQueryValue col) { return new FEXP { Function = QueryFunctions.STY, Parameters = new[] { col } }; }

        public static FEXP CollectionAggregate(this IQueryValue col) { return new FEXP { Function = QueryFunctions.CollectionAggregate, Parameters = new[] { col } }; }
        public static FEXP ConvexHullAggregate(this IQueryValue col) { return new FEXP { Function = QueryFunctions.ConvexHullAggregate, Parameters = new[] { col } }; }
        public static FEXP EnvelopeAggregate(this IQueryValue col) { return new FEXP { Function = QueryFunctions.EnvelopeAggregate, Parameters = new[] { col } }; }
        public static FEXP UnionAggregate(this IQueryValue col) { return new FEXP { Function = QueryFunctions.UnionAggregate, Parameters = new[] { col } }; }

        public static FEXP STGeomFromText(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STGeomFromText, Parameters = new[] { col, SRID } }; }
        public static FEXP STPointFromText(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STPointFromText, Parameters = new[] { col, SRID } }; }
        public static FEXP STLineFromText(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STLineFromText, Parameters = new[] { col, SRID } }; }
        public static FEXP STPolyFromText(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STPolyFromText, Parameters = new[] { col, SRID } }; }
        public static FEXP STMPointFromText(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STMPointFromText, Parameters = new[] { col, SRID } }; }
        public static FEXP STMLineFromText(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STMLineFromText, Parameters = new[] { col, SRID } }; }
        public static FEXP STMPolyFromText(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STMPolyFromText, Parameters = new[] { col, SRID } }; }
        public static FEXP STGeomCollFromText(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STGeomCollFromText, Parameters = new[] { col, SRID } }; }
        public static FEXP STGeomFromWKB(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STGeomFromWKB, Parameters = new[] { col, SRID } }; }
        public static FEXP STPointFromWKB(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STPointFromWKB, Parameters = new[] { col, SRID } }; }
        public static FEXP STLineFromWKB(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STLineFromWKB, Parameters = new[] { col, SRID } }; }
        public static FEXP STPolyFromWKB(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STPolyFromWKB, Parameters = new[] { col, SRID } }; }
        public static FEXP STMPointFromWKB(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STMPointFromWKB, Parameters = new[] { col, SRID } }; }
        public static FEXP STMLineFromWKB(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STMLineFromWKB, Parameters = new[] { col, SRID } }; }
        public static FEXP STMPolyFromWKB(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STMPolyFromWKB, Parameters = new[] { col, SRID } }; }
        public static FEXP STGeomCollFromWKB(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.STGeomCollFromWKB, Parameters = new[] { col, SRID } }; }
        public static FEXP GeomFromGML(this IQueryValue col, IQueryValue SRID) { return new FEXP { Function = QueryFunctions.GeomFromGML, Parameters = new[] { col, SRID } }; }


        public static FEXP AsBinaryZM(this IQueryValue col) { return new FEXP { Function = QueryFunctions.AsBinaryZM, Parameters = new[] { col } }; }
        public static FEXP AsGml(this IQueryValue col) { return new FEXP { Function = QueryFunctions.AsGml, Parameters = new[] { col } }; }
        public static FEXP AsTextZM(this IQueryValue col) { return new FEXP { Function = QueryFunctions.AsTextZM, Parameters = new[] { col } }; }
        public static FEXP BufferWithCurves(this IQueryValue col, IQueryValue distance) { return new FEXP { Function = QueryFunctions.BufferWithCurves, Parameters = new[] { col, distance } }; }
        public static FEXP BufferWithTolerance(this IQueryValue col, IQueryValue tolerance, IQueryValue relative) { return new FEXP { Function = QueryFunctions.BufferWithTolerance, Parameters = new[] { col, tolerance, relative } }; }
        public static FEXP CurveToLineWithTolerance(this IQueryValue col, IQueryValue tolerance, IQueryValue relative) { return new FEXP { Function = QueryFunctions.CurveToLineWithTolerance, Parameters = new[] { col, tolerance, relative } }; }
        public static FEXP InstanceOf(this IQueryValue col, IQueryValue geometry_type) { return new FEXP { Function = QueryFunctions.InstanceOf, Parameters = new[] { col, geometry_type } }; }
        public static FEXP Filter(this IQueryValue col, IQueryValue other_geometry) { return new FEXP { Function = QueryFunctions.Filter, Parameters = new[] { col, other_geometry } }; }
        public static FEXP HasM(this IQueryValue col) { return new FEXP { Function = QueryFunctions.HasM, Parameters = new[] { col } }; }
        public static FEXP HasZ(this IQueryValue col) { return new FEXP { Function = QueryFunctions.HasZ, Parameters = new[] { col } }; }
        public static FEXP IsGeometryNull(this IQueryValue col) { return new FEXP { Function = QueryFunctions.IsNull, Parameters = new[] { col } }; }
        public static FEXP IsValidDetailed(this IQueryValue col) { return new FEXP { Function = QueryFunctions.IsValidDetailed, Parameters = new[] { col } }; }
        public static FEXP M(this IQueryValue col) { return new FEXP { Function = QueryFunctions.M, Parameters = new[] { col } }; }
        public static FEXP MakeValid(this IQueryValue col) { return new FEXP { Function = QueryFunctions.MakeValid, Parameters = new[] { col } }; }
        public static FEXP MinDbCompatibilityLevel(this IQueryValue col) { return new FEXP { Function = QueryFunctions.MinDbCompatibilityLevel, Parameters = new[] { col } }; }
        public static FEXP Reduce(this IQueryValue col, IQueryValue tolerance) { return new FEXP { Function = QueryFunctions.Reduce, Parameters = new[] { col, tolerance } }; }
        public static FEXP ShortestLineTo(this IQueryValue col, IQueryValue geography_other) { return new FEXP { Function = QueryFunctions.ShortestLineTo, Parameters = new[] { col, geography_other } }; }
        public static FEXP ToString(this IQueryValue col) { return new FEXP { Function = QueryFunctions.ToString, Parameters = new[] { col } }; }
        public static FEXP Z(this IQueryValue col) { return new FEXP { Function = QueryFunctions.Z, Parameters = new[] { col } }; }
        public static FEXP EnvelopeAngle(this IQueryValue col) { return new FEXP { Function = QueryFunctions.EnvelopeAngle, Parameters = new[] { col } }; }
        public static FEXP EnvelopeCenter(this IQueryValue col) { return new FEXP { Function = QueryFunctions.EnvelopeCenter, Parameters = new[] { col } }; }
        public static FEXP Lat(this IQueryValue col) { return new FEXP { Function = QueryFunctions.Lat, Parameters = new[] { col } }; }
        public static FEXP Long(this IQueryValue col) { return new FEXP { Function = QueryFunctions.Long, Parameters = new[] { col } }; }
        public static FEXP NumRing(this IQueryValue col) { return new FEXP { Function = QueryFunctions.NumRing, Parameters = new[] { col } }; }
        public static FEXP ReorientObject(this IQueryValue col) { return new FEXP { Function = QueryFunctions.ReorientObject, Parameters = new[] { col } }; }
        public static FEXP RingN(this IQueryValue col, IQueryValue expression) { return new FEXP { Function = QueryFunctions.RingN, Parameters = new[] { col, expression } }; }



        public static FEXP AVG(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Avg, Parameters = new[] { expression } }; }
        public static FEXP CHECKSUM_AGG(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Checksum_Agg, Parameters = new[] { expression } }; }
        public static FEXP COUNT(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Count, Parameters = new[] { expression } }; }
        public static FEXP COUNT_BIG(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Count_Big, Parameters = new[] { expression } }; }
        public static FEXP GROUPING(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Grouping, Parameters = new[] { expression } }; }
        public static FEXP GROUPING_ID(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Grouping_Id, Parameters = new[] { expression } }; }
        public static FEXP MAX(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Max, Parameters = new[] { expression } }; }
        public static FEXP MIN(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Min, Parameters = new[] { expression } }; }
        public static FEXP SUM(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Sum, Parameters = new[] { expression } }; }
        public static FEXP STDEV(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Stdev, Parameters = new[] { expression } }; }
        public static FEXP STDEVP(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Stdevp, Parameters = new[] { expression } }; }
        public static FEXP VAR(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Var, Parameters = new[] { expression } }; }
        public static FEXP VARP(this IQueryValue expression) { return new FEXP { Function = QueryFunctions.Varp, Parameters = new[] { expression } }; }

    }



}
