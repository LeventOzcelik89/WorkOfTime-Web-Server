using Infoline.Framework.Helper;
using Infoline.Helper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Infoline.Framework.DatabaseE
{
    public interface IExecuteReader<T> where T : new()
    {
        IEnumerable<T> Execute();
        IWhere<T> Where(IConditon condition);
        IOrderBy<T> OrderDescBy(Expression<Func<T, object>> columns);
        IOrderBy<T> OrderBy(string field, string type);
        IOrderBy<T> OrderBy(List<OrderByColumn> sorts);
        ISkip<T> Skip(int? index);
        ITake<T> Take(int? index);
    }

    public interface ISelect<T> : IExecuteReader<T> where T : new()
    {
        IWhere<T> Where(Expression<Func<T, bool>> condition);
        IOrderBy<T> OrderAscBy(Expression<Func<T, object>> columns);
        IOrderBy<T> OrderDescBy(Expression<Func<T, object>> columns);
    }

    public interface IWhere<T> : IExecuteReader<T> where T : new()
    {
        IWhere<T> Where(Expression<Func<T, bool>> condition);
        IWhere<T> Where(IConditon condition);
        IOrderBy<T> OrderAscBy(Expression<Func<T, object>> columns);
        IOrderBy<T> OrderDescBy(Expression<Func<T, object>> columns);
    }

    public interface IOrderBy<T> : IExecuteReader<T> where T : new()
    {
        IOrderBy<T> OrderAscBy(Expression<Func<T, object>> columns);
        IOrderBy<T> OrderDescBy(Expression<Func<T, object>> columns);
    }

    public interface ISkip<T> : IExecuteReader<T> where T : new()
    {

    }
    public interface ITake<T> : IExecuteReader<T> where T : new()
    {

    }

    public interface ICount<T> where T : new()
    {
        int ExecuteCount(Expression<Func<T, bool>> condition = null);
    }

    public class Select<T> : ISelect<T>, IWhere<T>, IOrderBy<T>, ISkip<T>, ITake<T>, ICount<T> where T : new()
    {
        string _tableName;
        string[] _selectColumns;
        List<OrderByColumn> _orderColumns;
        Database _database;
        Dictionary<ExpressionType, string> _connective;
        Dictionary<ExpressionType, string> _comparison;
        List<QueryParameter> _parameters;
        string _whereStatement = null;
        int? skip;
        int? take;


        public Select(Database database, Expression<Func<T, object>> columns = null, string tablename = null)
        {
            _database = database;
            _tableName = tablename;
            if (_tableName == null)
                _tableName = typeof(T).Name;

            _selectColumns = ExpressionHelper.GetPropertyNames<T, object>(columns).ToArray();

            _orderColumns = new List<OrderByColumn>();
            _parameters = new List<QueryParameter>();

            _connective = new Dictionary<ExpressionType, string>();
            _connective.Add(ExpressionType.And, "and");
            _connective.Add(ExpressionType.AndAlso, "and");
            _connective.Add(ExpressionType.Or, "or");
            _connective.Add(ExpressionType.OrElse, "or");

            _comparison = new Dictionary<ExpressionType, string>();
            _comparison.Add(ExpressionType.Equal, "=");
            _comparison.Add(ExpressionType.GreaterThan, ">");
            _comparison.Add(ExpressionType.GreaterThanOrEqual, ">=");
            _comparison.Add(ExpressionType.LessThan, "<");
            _comparison.Add(ExpressionType.LessThanOrEqual, "<=");
            _comparison.Add(ExpressionType.Not, "not");
            _comparison.Add(ExpressionType.NotEqual, "<>");
        }

        public Select(Database database, string[] columns = null, string tablename = null)
        {
            _database = database;
            _tableName = tablename;
            if (_tableName == null)
                _tableName = typeof(T).Name;

            _selectColumns = columns ?? new string[0];

            _orderColumns = new List<OrderByColumn>();
            _parameters = new List<QueryParameter>();

            _connective = new Dictionary<ExpressionType, string>();
            _connective.Add(ExpressionType.And, "and");
            _connective.Add(ExpressionType.AndAlso, "and");
            _connective.Add(ExpressionType.Or, "or");
            _connective.Add(ExpressionType.OrElse, "or");

            _comparison = new Dictionary<ExpressionType, string>();
            _comparison.Add(ExpressionType.Equal, "=");
            _comparison.Add(ExpressionType.GreaterThan, ">");
            _comparison.Add(ExpressionType.GreaterThanOrEqual, ">=");
            _comparison.Add(ExpressionType.LessThan, "<");
            _comparison.Add(ExpressionType.LessThanOrEqual, "<=");
            _comparison.Add(ExpressionType.Not, "not");
            _comparison.Add(ExpressionType.NotEqual, "<>");
        }


        public IWhere<T> Where(Expression<Func<T, bool>> condition)
        {
            if (condition == null)
                return this;
            //var binaryexpression = (condition.Body as BinaryExpression);
            //var methodexpression = (condition.Body as MethodCallExpression);
            var processWhereResult = ProcessWhere(condition.Body);
            if (_whereStatement == null)
                _whereStatement = processWhereResult;
            else  if(!string.IsNullOrEmpty(_whereStatement))
                _whereStatement += " AND " + processWhereResult;
            return this;
        }

        public IOrderBy<T> OrderAscBy(Expression<Func<T, object>> columns)
        {
            _orderColumns.AddRange(ExpressionHelper.GetPropertyNames<T, object>(columns).Select(a => new OrderByColumn { Name = a, IsAsc = true }));
            return this;
        }

        public IOrderBy<T> OrderDescBy(Expression<Func<T, object>> columns)
        {
            _orderColumns.AddRange(ExpressionHelper.GetPropertyNames<T, object>(columns).Select(a => new OrderByColumn { Name = a, IsAsc = false }));
            return this;
        }

        public IOrderBy<T> OrderBy(string filed, string type)
        {
            _orderColumns.Add(new OrderByColumn { Name = filed, IsAsc = type.ToLower() == "asc" });
            return this;
        }

        public IOrderBy<T> OrderBy(List<OrderByColumn> orderColumns)
        {
            if (orderColumns == null)
                return this;

            _orderColumns.AddRange(orderColumns);
            return this;
        }

        public IEnumerable<T> Execute()
        {
            var topStatement = "";
            var offsetStatement = "";
            var orderStatement = "";
            if (take != null && skip != null)
                offsetStatement = string.Format(" OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", skip, take);
            else if (take == null && skip != null)
                offsetStatement = string.Format(" OFFSET {0} ROWS ", skip);
            else if (take != null && skip == null)
                topStatement = string.Format("top {0}", take);

            orderStatement = _orderColumns.Count > 0 ? " order by " + string.Join(",", _orderColumns.Select(a => string.Format("{0} {1}", a.Name, a.IsAsc ? "asc" : "desc"))) : "";
            if (orderStatement == "" && offsetStatement != "")
                orderStatement = " order by id";

            var query = string.Format("select {2} {0} from {1} with (nolock)", _selectColumns.Length == 0 ? "*" : string.Join(",", _selectColumns), _tableName, topStatement);
            query += (_whereStatement != null && _whereStatement.Length > 0) ? " where " + _whereStatement : "";
            query += orderStatement;
            query += offsetStatement;

            _selectColumns = null;
            _orderColumns.Clear();
            _whereStatement = null;
            skip = null;
            take = null;
            var isDict = typeof(IDictionary).IsAssignableFrom(typeof(T));
            if (!isDict)
                return _database.ExecuteReader<T>(query, _parameters.Select(a => a.Value).ToArray());
            else return _database.ExecuteReader(query, _parameters.Select(a => a.Value).ToArray()).Cast<T>();
        }
        
        private string ProcessWhere(Expression exp)
        {
            if ((exp as BinaryExpression) != null)
            {
                var ex = (exp as BinaryExpression);
                var nodeType = exp.NodeType;
                if (_connective.ContainsKey(nodeType))
                {
                    var left = ProcessOperand(ex.Left);
                    var right = ProcessOperand(ex.Right);
                    if (nodeType == ExpressionType.And || nodeType == ExpressionType.AndAlso)
                        return string.Format("({0} and {1})", left, right);
                    else if (nodeType == ExpressionType.Or || nodeType == ExpressionType.OrElse)
                        return string.Format("({0} or {1})", left, right);
                }

                if (_comparison.ContainsKey(nodeType))
                {
                    var prop = (ex.Left as MemberExpression);
                    var prop2 = (ex.Right as MemberExpression);
                    var c = ex.Right as ConstantExpression;
                    string name = null;
                    object val = null;


                    if ((ex.Left as MemberExpression) != null && typeof(System.Linq.Expressions.ParameterExpression).IsAssignableFrom((ex.Left as MemberExpression).Expression.GetType()) &&
                        (ex.Right as MemberExpression) != null && typeof(System.Linq.Expressions.ParameterExpression).IsAssignableFrom((ex.Right as MemberExpression).Expression.GetType()))
                    {
                        name = prop.Member.Name;
                        var name2 = prop2.Member.Name;
                        return string.Format("{0} {1} {2}", name, _comparison[nodeType], name2);
                    }
                    else if ((ex.Left as MemberExpression) != null && typeof(System.Linq.Expressions.ParameterExpression).IsAssignableFrom((ex.Left as MemberExpression).Expression.GetType()))
                    {
                        name = prop.Member.Name;
                        Expression subEx = ex.Right;
                        if (subEx is UnaryExpression)
                            subEx = (subEx as UnaryExpression).Operand;
                        val = Expression.Lambda(subEx).Compile().DynamicInvoke();
                    }
                    else if ((ex.Right as MemberExpression) != null && typeof(System.Linq.Expressions.ParameterExpression).IsAssignableFrom((ex.Right as MemberExpression).Expression.GetType()))
                    {
                        name = prop2.Member.Name;
                        Expression subEx = ex.Left;
                        if (subEx is UnaryExpression)
                            subEx = (subEx as UnaryExpression).Operand;
                        val = Expression.Lambda(subEx).Compile().DynamicInvoke();
                    }

                    if (val == null)
                    {
                        if (nodeType == ExpressionType.Equal)
                            return string.Format("{0} is null", name, _comparison[nodeType]);
                        else if (nodeType == ExpressionType.NotEqual)
                            return string.Format("{0} is not null", name, _comparison[nodeType]);
                    }
                    var parameterName = string.Format("@p{0}", _parameters.Count);
                    var parameter = new QueryParameter { Name = parameterName, Value = val };
                    _parameters.Add(parameter);
                    return string.Format("{0} {1} {2}", name, _comparison[nodeType], parameterName);
                }
            }
            else
            {
                return ProcessOperand(exp);
            }
            return "";
        }

        private string ProcessOperand(Expression exp)
        {
            string left = "";
            if (exp as BinaryExpression == null)
            {
                if (exp.NodeType == ExpressionType.Not)
                    left = string.Format("not({0})", ProcessWhere((exp as UnaryExpression).Operand));
                else if (exp as MemberExpression != null)
                    if (((exp as MemberExpression).Expression as MemberExpression) != null)
                        left = string.Format("{0} = 1", ((exp as MemberExpression).Expression as MemberExpression).Member.Name);
                    else left = string.Format("{0} = 1", (exp as MemberExpression).Member.Name);
                else if (exp is MethodCallExpression)
                {
                    var mcExp = exp as MethodCallExpression;
                    var p1 = ParseExp(mcExp.Object);
                    var p2 = ParseExp(mcExp.Arguments[0]);
                    if (mcExp.Method.Name.ToLower() == "contains")
                        left = string.Format("[{0}] like {1}", p1, p2);
                }
            }
            else
                left = ProcessWhere(exp as BinaryExpression);
            return left;
        }

        public string ParseExp(Expression exp)
        {
            if(exp is ConstantExpression)
            {
                var val = Expression.Lambda(exp).Compile().DynamicInvoke();
                var parameterName = string.Format("@p{0}", _parameters.Count);
                var parameter = new QueryParameter { Name = parameterName, Value = val };
                _parameters.Add(parameter);
                return parameterName;
            }
            else if (typeof(System.Linq.Expressions.ParameterExpression).IsAssignableFrom((exp as MemberExpression).Expression.GetType()))
            {
                return string.Format("{0}", (exp as MemberExpression).Member.Name);
            }
            else if (exp is MemberExpression)
            {
                var val = Expression.Lambda(exp).Compile().DynamicInvoke();
                var parameterName = string.Format("@p{0}", _parameters.Count);
                var parameter = new QueryParameter { Name = parameterName, Value = val };
                _parameters.Add(parameter);
                return parameterName;
            }
            return "";
        }

        
        public IWhere<T> Where(IConditon condition)
        {
            var cond = condition.GetQueryString(_parameters.Count);
            if (string.IsNullOrEmpty(_whereStatement))
                _whereStatement = cond.Item1;
            else _whereStatement += " and " + cond.Item1;

            if (cond.Item2 != null)
                _parameters.Add(cond.Item2);

            return this;
        }


        public ISkip<T> Skip(int? index)
        {
            skip = index;
            return this;
        }

        public ITake<T> Take(int? index)
        {
            take = index;
            return this;
        }

        public int ExecuteCount(Expression<Func<T, bool>> condition = null)
        {
            if (condition == null)
                _whereStatement = "";
            else  _whereStatement = ProcessWhere(condition.Body);

            var query = string.Format("SELECT COUNT(*) FROM {0} WITH (NOLOCK)", _tableName);
            if (!string.IsNullOrEmpty(_whereStatement))
                query += " WHERE " + _whereStatement;

            return _database.ExecuteScaler<int>(query, _parameters.Select(a => a.Value).ToArray());
        }
    }

    public class OrderByColumn
    {
        public string Name { get; set; }
        public bool IsAsc { get; set; }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name, IsAsc);
        }
    }


    public interface IConditon
    {
        Tuple<string, QueryParameter> GetQueryString(int parametreCount);
    }
    public class ColumnCompareCondition : IConditon
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public ExpressionType CompareExpression { get; set; }

        static Dictionary<ExpressionType, string> _comparison;
        static ColumnCompareCondition()
        {
            _comparison = new Dictionary<ExpressionType, string>();
            _comparison.Add(ExpressionType.Equal, "=");
            _comparison.Add(ExpressionType.GreaterThan, ">");
            _comparison.Add(ExpressionType.GreaterThanOrEqual, ">=");
            _comparison.Add(ExpressionType.LessThan, "<");
            _comparison.Add(ExpressionType.LessThanOrEqual, "<=");
            _comparison.Add(ExpressionType.Not, "not");
            _comparison.Add(ExpressionType.NotEqual, "<>");
        }

        public ColumnCompareCondition(string key, object value)
        {
            Key = key;
            Value = value;
            CompareExpression = ExpressionType.Equal;
        }

        public ColumnCompareCondition(string field, object value, string operato)
        {
            Key = field;
            Value = value;
            CompareExpression = ExpressionType.Equal;

            if (operato == ExpressionType.Equal.ToString())
                CompareExpression = ExpressionType.Equal;
            else if (operato == ExpressionType.NotEqual.ToString())
                CompareExpression = ExpressionType.NotEqual;
            else if (operato == ExpressionType.GreaterThan.ToString())
                CompareExpression = ExpressionType.GreaterThan;
            else if (operato == ExpressionType.GreaterThanOrEqual.ToString())
                CompareExpression = ExpressionType.GreaterThanOrEqual;
            else if (operato == ExpressionType.LessThan.ToString())
                CompareExpression = ExpressionType.LessThan;
            else if (operato == ExpressionType.LessThanOrEqual.ToString())
                CompareExpression = ExpressionType.LessThanOrEqual;

        }

        public Tuple<string, QueryParameter> GetQueryString(int parametreCount)
        {
            var parameterName = string.Format("@p{0}", parametreCount);
            var parameter = new QueryParameter { Name = parameterName, Value = Value };
            return new Tuple<string, QueryParameter>(string.Format("[{0}] {1} {2}", Key, _comparison[CompareExpression], parameterName), parameter);
        }
    }
}
