using Infoline.Framework.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.DatabaseE
{
    class SqlQueryBuilder : IQueryBuilder
    {
        public Query GetInsertQuery<T>(T parametre, Expression<Func<T, object>> exceptexp, string tableName = null)
        {
            if (tableName == null)
                tableName = typeof(T).Name;

            var except = ExpressionHelper.GetPropertyNames<T, object>(exceptexp);
            string parametrename = "";
            string parametrevalue = "";
            List<QueryParameter> param = new List<QueryParameter>();
            foreach (var p in typeof(T).GetProperties().Where(p => p.GetValue(parametre, null) != null))
            {
                if (string.IsNullOrEmpty(p.GetValue(parametre, null).ToString().Trim())) continue;
                if (except.Contains(p.Name)) continue;
                parametrename += ",[" + p.Name + "]";
                parametrevalue += ",@" + p.Name;
                var val = p.GetValue(parametre);
                param.Add(new QueryParameter { Name = "@" + p.Name, Value = val });
            }

            parametrename = parametrename.Substring(1);
            parametrevalue = parametrevalue.Substring(1);
            string sql = null;
            if (parametrename.Length > 2)
                sql = string.Format("Insert Into {0} ({1}) values({2})", tableName, parametrename, parametrevalue);

            return new Query { Command = sql, Parameters = param.ToArray() };
        }

        public Query GetUpdateQuery<T>(T parametre, Expression<Func<T, object>> idexp, Expression<Func<T, object>> exceptexp = null, bool setNull = false, string tableName = null)
        {
            if (tableName == null)
                tableName = typeof(T).Name;

            //var idProperty = ExpressionHelper.GetProperty<T, object>(idexp);
            //var id = ((PropertyInfo)idProperty).GetValue(parametre);
            var except = ExpressionHelper.GetPropertyNames<T, object>(exceptexp);

            var parametrename = "";
            var param = new List<QueryParameter>();
            foreach (var p in typeof(T).GetProperties().Where(p => setNull || p.GetValue(parametre, null) != null))
            {
                if (!setNull && string.IsNullOrEmpty(p.GetValue(parametre, null).ToString().Trim())) continue;
                if (except.Contains(p.Name)) continue;
                parametrename += ",[" + p.Name + "]=@" + p.Name;
                param.Add(new QueryParameter { Name = "@" + p.Name, Value = p.GetValue(parametre, null) });
            }
            if (parametrename.Length > 1)
                parametrename = parametrename.Substring(1);

            var idProperties = ExpressionHelper.GetProperties<T, object>(idexp);
            var whereStatement = "";
            int i = 0;
            foreach (var idProperty in idProperties.Cast<PropertyInfo>())
            {
                if (string.IsNullOrEmpty(idProperty.GetValue(parametre, null).ToString().Trim())) continue;
                whereStatement += " and [" + idProperty.Name + "]=@p" + i;
                param.Add(new QueryParameter { Name = "@p" + i, Value = idProperty.GetValue(parametre, null) });
                i++;
            }
            if (whereStatement.Length > 5)
                whereStatement = whereStatement.Substring(5);

            whereStatement = whereStatement.Length > 0 ? " where " + whereStatement : "";

            string sql = null;
            if (parametrename.Length > 2)
                sql = string.Format("Update {0} Set {1} {2}", tableName, parametrename, whereStatement);
            //else
            //    throw new Exception("Update edilecek kolon belirtilmedi");
            return new Query { Command = sql, Parameters = param.ToArray() };
        }

        public Query GetDeleteQuery<T>(T parametre, Expression<Func<T, object>> idexp, string tableName = null)
        {
            if (tableName == null)
                tableName = typeof(T).Name;


            var idProperties = ExpressionHelper.GetProperties<T, object>(idexp);
            var whereStatement = "";
            var param = new List<QueryParameter>();
            int i = 0;
            foreach (var idProperty in idProperties.Cast<PropertyInfo>())
            {
                if (string.IsNullOrEmpty(idProperty.GetValue(parametre, null).ToString().Trim())) continue;
                whereStatement += " and " + idProperty.Name + "=@p" + i;
                param.Add(new QueryParameter { Name = "@p" + i, Value = idProperty.GetValue(parametre, null) });
                i++;
            }
            whereStatement = whereStatement.Substring(5);

            if (param.Count == 0)
                throw new Exception("Delete işlemi için vir kolon seçmeniz gerekmekte.");

            var sql = string.Format("delete  from {0} where " + whereStatement, parametre.GetType().Name);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }

        public Query ConvertToQuery(string txt, params object[] parameters)
        {
            string query;
            bool isStoredProcedure;
            var parameterList = new List<QueryParameter>();
            var nullCheck = new Dictionary<string, bool>();
            isStoredProcedure = txt.IndexOf(' ') == -1 ? true : false;
            if (isStoredProcedure)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    var p = new QueryParameter { Name = "@p" + i.ToString(), Value = parameters[i] ?? DBNull.Value };
                    parameterList.Add(p);
                }

                query = txt + " " + string.Join(",", parameterList.Select(a => a.Name).ToArray());
            }
            else
            {

                for (int i = 0; i < parameters.Length; i++)
                {
                    var p = new QueryParameter();
                    p.Name = "@p" + i.ToString();
                    p.Value = parameters[i] ?? DBNull.Value;
                    parameterList.Add(p);
                }
                if (parameterList.Count > 0)
                    query = string.Format(txt, parameterList.Select(a => a.Name).ToArray());
                else
                    query = txt;

            }
            return new Query { Command = query, Parameters = parameterList.ToArray(), IsStoredProcedure = isStoredProcedure };
        }
    }


    public class Condition
    {
        public QueryCondition[] Filter { get; set; }
        public QuerySort Sort { get; set; }
        public string[] Fields { get; set; }
        public int? StartIndex { get; set; }
        public int? Count { get; set; }
    }
    public class QuerySort
    {
        public string Field { get; set; }
        public string Type { get; set; }
    }
    public class QueryCondition
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
    }

    

    //public class ColumnCompareCondition : IConditon
    //{
    //    public string Key { get; set; }
    //    public object Value { get; set; }
    //    public ExpressionType CompareExpression { get; set; }

    //    static Dictionary<ExpressionType, string> _comparison;
    //    static ColumnCompareCondition()
    //    {
    //        _comparison = new Dictionary<ExpressionType, string>();
    //        _comparison.Add(ExpressionType.Equal, "=");
    //        _comparison.Add(ExpressionType.GreaterThan, ">");
    //        _comparison.Add(ExpressionType.GreaterThanOrEqual, ">=");
    //        _comparison.Add(ExpressionType.LessThan, "<");
    //        _comparison.Add(ExpressionType.LessThanOrEqual, "<=");
    //        _comparison.Add(ExpressionType.Not, "not");
    //        _comparison.Add(ExpressionType.NotEqual, "<>");
    //    }

    //    public ColumnCompareCondition(string key, object value)
    //    {
    //        Key = key;
    //        Value = value;
    //        CompareExpression = ExpressionType.Equal;
    //    }

    //    public ColumnCompareCondition(string field, object value, string operato)
    //    {
    //        Key = field;
    //        Value = value;
    //        CompareExpression = ExpressionType.Equal;

    //        if (operato == ExpressionType.Equal.ToString())
    //            CompareExpression = ExpressionType.Equal;
    //        else if (operato == ExpressionType.NotEqual.ToString())
    //            CompareExpression = ExpressionType.NotEqual;
    //        else if (operato == ExpressionType.GreaterThan.ToString())
    //            CompareExpression = ExpressionType.GreaterThan;
    //        else if (operato == ExpressionType.GreaterThanOrEqual.ToString())
    //            CompareExpression = ExpressionType.GreaterThanOrEqual;
    //        else if (operato == ExpressionType.LessThan.ToString())
    //            CompareExpression = ExpressionType.LessThan;
    //        else if (operato == ExpressionType.LessThanOrEqual.ToString())
    //            CompareExpression = ExpressionType.LessThanOrEqual;

    //    }

    //    public Tuple<string, QueryParameter> GetQueryString(int parametreCount)
    //    {
    //        var parameterName = string.Format("@p{0}", parametreCount);
    //        var parameter = new QueryParameter { Name = parameterName, Value = Value };
    //        return new Tuple<string, QueryParameter>(string.Format("[{0}] {1} {2}", Key, _comparison[CompareExpression], parameterName), parameter);
    //    }
    //}
}
