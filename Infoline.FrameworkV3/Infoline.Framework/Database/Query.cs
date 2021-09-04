using System.Linq;
using System.Linq.Expressions;
using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Infoline.Framework.Database
{

    public class Query
    {
        public string Command { get; set; }
        public QueryParameter[] Parameters { get; set; }
        public bool IsStoredProcedure { get; set; }

        public Query()
        {
            Parameters = new QueryParameter[0];
        }
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

    [Serializable]
    [DataContract]
    public class SimpleQuery
    {
        [DataMember]
        public INamedItem[] Fields { get; set; }
        [DataMember]
        public BEXP Filter { get; set; }
        [DataMember]
        public IQueryOrderItem[] Sort { get; set; }
        [DataMember]
        public int? Skip { get; set; }
        [DataMember]
        public int? Take { get; set; }
    }

    [Serializable]
    public class NamedItem : INamedItem
    {
        public string Name { get; set; }
    }

    #region Using in Web Services


    public interface Condition
    {
        QuerySort Sort { get; set; }
        string[] Fields { get; set; }
        int? StartIndex { get; set; }
        int? Count { get; set; }
    }

    public class ConditionNew : Condition
    {
        public BEXP Filter { get; set; }
        public QuerySort Sort { get; set; }
        public string[] Fields { get; set; }
        public int? StartIndex { get; set; }
        public int? Count { get; set; }
    }


    public class ConditionEx : Condition
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



    public class CondtionToQuery
    {
        public static SimpleQuery Convert(Condition condition)
        {
            SimpleQuery query = new SimpleQuery();
            if (condition != null)
            {

                if (condition is ConditionEx)
                {
                    query.Filter = ParseFilter(((ConditionEx)condition).Filter);
                }

                if (condition is ConditionNew)
                {
                    query.Filter = ((ConditionNew)condition).Filter;
                }


                query.Fields = condition.Fields == null ? null : condition.Fields.Select(a => new COL(a)).ToArray();
                if (condition.Sort != null)
                {
                    query.Sort = new IQueryOrderItem[] {
                    condition.Sort.Type.ToLower() == "asc" ?
                        (IQueryOrderItem)new ASC() { Value = new COL(condition.Sort.Field ?? "created") } :
                        (IQueryOrderItem)new DESC() { Value = new COL(condition.Sort.Field ??  "created") }
                    };
                }
                else
                {
                    query.Sort = new IQueryOrderItem[] {
                        (IQueryOrderItem)new DESC() { Value = new COL("created") }
                    };
                }
                query.Skip = condition.StartIndex ?? 0;
                query.Take = condition.Count ?? 20;
            }
            return query;
        }

        private static BEXP ParseFilter(QueryCondition[] condition)
        {
            BEXP exp = null;
            if (condition == null) return null;
            for (var i = 0; i < condition.Length; i++)
            {
                var cond = condition[i];
                BEXP exp2 = ParseQueryCondition(cond);
                if (i == 0) exp = exp2;
                else exp = new BEXP { Operand1 = exp, Operand2 = exp2, Operator = BinaryOperator.And };
            }
            return exp;
        }

        private static BEXP ParseQueryCondition(QueryCondition cond)
        {
            BEXP exp2;
            if (cond.Operator == ExpressionType.Equal.ToString() || cond.Operator == "=")
                exp2 = new BEXP { Operand1 = new COL(cond.Field), Operand2 = new VAL(cond.Value), Operator = BinaryOperator.Equal };
            else if (cond.Operator == ExpressionType.GreaterThan.ToString() || cond.Operator == ">")
                exp2 = new BEXP { Operand1 = new COL(cond.Field), Operand2 = new VAL(cond.Value), Operator = BinaryOperator.GreaterThan };
            else if (cond.Operator == ExpressionType.GreaterThanOrEqual.ToString() || cond.Operator == ">=")
                exp2 = new BEXP { Operand1 = new COL(cond.Field), Operand2 = new VAL(cond.Value), Operator = BinaryOperator.GreaterThanOrEqual };
            else if (cond.Operator == ExpressionType.LessThan.ToString() || cond.Operator == "<")
                exp2 = new BEXP { Operand1 = new COL(cond.Field), Operand2 = new VAL(cond.Value), Operator = BinaryOperator.LessThan };
            else if (cond.Operator == ExpressionType.LessThanOrEqual.ToString() || cond.Operator == "<=")
                exp2 = new BEXP { Operand1 = new COL(cond.Field), Operand2 = new VAL(cond.Value), Operator = BinaryOperator.LessThanOrEqual };
            else if (cond.Operator == ExpressionType.Not.ToString() || cond.Operator == "!=")
                exp2 = new BEXP { Operand1 = new COL(cond.Field), Operator = BinaryOperator.Not };
            else if (cond.Operator == ExpressionType.NotEqual.ToString() || cond.Operator == "<>")
                exp2 = new BEXP { Operand1 = new COL(cond.Field), Operand2 = new VAL(cond.Value), Operator = BinaryOperator.NotEqual };
            else if (cond.Operator == "contains" || cond.Operator == "like")
                exp2 = new BEXP { Operand1 = new COL(cond.Field), Operand2 = new VAL("%" + cond.Value + "%"), Operator = BinaryOperator.Like };
            else if (cond.Operator == "In")
                exp2 = new BEXP { Operand1 = new COL(cond.Field), Operand2 = new ARR() { Values = ((Array)cond.Value).Cast<object>().Select(a => new VAL(a)).ToArray() }, Operator = BinaryOperator.In };
            else
                throw new Exception("Hatalı operatör.");
            return exp2;
        }

        //private static BEXP ParseQueryConditionLogic(QueryConditionLogic condition)
        //{
        //    return new BEXP
        //    {
        //        Operand1 = ParseFilter(condition.operand1),
        //        Operand2 = ParseFilter(condition.operand2),
        //        Operator = condition.logic
        //    };
        //}

        //private static BEXP ParseFilter(IQueryCondition condition)
        //{
        //    BEXP exp = null;
        //    if (condition == null) return null;

        //    if (condition is QueryCondition)
        //    {
        //        exp = ParseQueryCondition((QueryCondition)condition);
        //    }
        //    else
        //    {
        //        exp = ParseQueryConditionLogic((QueryConditionLogic)condition);

        //    }
        //    return exp;
        //}
    }

    #endregion



}
