using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Database
{
    public interface IGroupObject<TKey, TElement>
    {
        TKey Key { get; set; }
        int Count();
        decimal? Sum(Func<TElement, decimal?> column);
        R Max<R>(Func<TElement, R> column);
        R Min<R>(Func<TElement, R> column);

        IGeometry CollectionAggregate(Func<TElement, IGeometry> column);
        IGeometry ConvexHullAggregate(Func<TElement, IGeometry> column);
        IGeometry EnvelopeAggregate(Func<TElement, IGeometry> column);
        IGeometry UnionAggregate(Func<TElement, IGeometry> column);
    }

    public interface IQuery
    {

    }
    public interface IExecutable<TKey>
    {
        IEnumerable<TKey> Execute();
        IEnumerable<T> Execute<T>();
        T ExecuteScaler<T>();
        string GetQueryForTest();
    }
    public interface ISelect<TKey, TPrev> : IQuery, IExecutable<TKey>
    {
        IWhere<TKey, TKey> Where(Expression<Func<TKey, bool>> condition);
        IOrderBy<TKey, TKey> OrderBy(Expression<Func<TKey, object>> columns);
        IOrderBy<TKey, TKey> OrderByDesc(Expression<Func<TKey, object>> columns);
        ISkip<TKey, TKey> Skip(int? start);
        ITake<TKey, TKey> Take(int? count);
        IEnumerable<TKey> ExecuteSimpleQuery(SimpleQuery simpleQuery);
    }
    public interface IWhere<TKey, TPrev> : IExecutable<TKey>
    {
        ISelect<TResult, TKey> Select<TResult>(Expression<Func<TKey, TResult>> columns = null);
        IGroupBy<TResult, TKey> GroupBy<TResult>(Expression<Func<TKey, TResult>> columns);
        ISkip<TKey, TKey> Skip(int? start);
        ITake<TKey, TKey> Take(int? count);
        IOrderBy<TKey, TKey> OrderBy(Expression<Func<TKey, object>> columns);
        IOrderBy<TKey, TKey> OrderByDesc(Expression<Func<TKey, object>> columns);
        int Count();
    }
    public interface IGroupBy<TKey, TElement> : IExecutable<TKey>
    {
        ISelect<TResult, TKey> Select<TResult>(Expression<Func<IGroupObject<TKey, TElement>, TResult>> columns = null);
        int Count();
    }
    public interface IOrderBy<TKey, TPrev> : IExecutable<TKey>
    {
        IOrderBy<TKey, TKey> OrderBy(Expression<Func<TKey, object>> columns);
        IOrderBy<TKey, TKey> OrderByDesc(Expression<Func<TKey, object>> columns);
        ISkip<TKey, TKey> Skip(int? start);
        ITake<TKey, TKey> Take(int? count);


    }
    public interface ISkip<TKey, TPrev> : IExecutable<TKey>
    {
        ITake<TKey, TKey> Take(int? count);
    }
    public interface ITake<TKey, TPrev> : IExecutable<TKey>
    {


    }

    public interface ICallFunction<TResult>
    {

    }

    public interface IGetTable<TKey> : IExecutable<TKey>
    {
        ISelect<TResult, TKey> Select<TResult>(Expression<Func<TKey, TResult>> columns = null);
        IWhere<TKey, TKey> Where(Expression<Func<TKey, bool>> condition);
        IGroupBy<TResult, TKey> GroupBy<TResult>(Expression<Func<TKey, TResult>> columns);
        IOrderBy<TKey, TKey> OrderBy(Expression<Func<TKey, object>> columns);
        IOrderBy<TKey, TKey> OrderByDesc(Expression<Func<TKey, object>> columns);
        ISkip<TKey, TKey> Skip(int? start);
        ITake<TKey, TKey> Take(int? count);
        int Count();
        IEnumerable<TKey> ExecuteSimpleQuery(SimpleQuery simpleQuery);

        ResultStatus Insert(TKey parameter, Expression<Func<TKey, object>> exceptCols = null);
        ResultStatus Update(TKey parameter, Expression<Func<TKey, object>> idCols, Expression<Func<TKey, object>> exceptCols = null, bool setNull = false);
        ResultStatus Delete(TKey parameter, Expression<Func<TKey, object>> idCols);
        ResultStatus Delete(Expression<Func<TKey, bool>> condition);
        ResultStatus Insert(IEnumerable<TKey> parameter, Expression<Func<TKey, object>> exceptCols = null);
        ResultStatus Update(IEnumerable<TKey> parameter, Expression<Func<TKey, object>> idCols, Expression<Func<TKey, object>> exceptCols = null, bool setNull = false);
        ResultStatus UpdateManyColumn(IEnumerable<TKey> parameter, Expression<Func<TKey, object>> idCols, Expression<Func<TKey, object>> exceptCols = null, bool setNull = false);
        ResultStatus InsertManyColumn(IEnumerable<TKey> parameter, bool setNull = false);
        ResultStatus Delete(IEnumerable<TKey> parameter, Expression<Func<TKey, object>> idCols);

    }
    public interface IGetTableFunction<TKey> : IExecutable<TKey>
    {
        ISelect<TResult, TKey> Select<TResult>(Expression<Func<TKey, TResult>> columns = null);
        IWhere<TKey, TKey> Where(Expression<Func<TKey, bool>> condition);
        IGroupBy<TResult, TKey> GroupBy<TResult>(Expression<Func<TKey, TResult>> columns);
        IOrderBy<TKey, TKey> OrderBy(Expression<Func<TKey, object>> columns);
        IOrderBy<TKey, TKey> OrderByDesc(Expression<Func<TKey, object>> columns);
        ISkip<TKey, TKey> Skip(int? start);
        ITake<TKey, TKey> Take(int? count);
        int Count();
        IEnumerable<TKey> ExecuteSimpleQuery(SimpleQuery simpleQuery);

        ResultStatus Insert(TKey parameter, Expression<Func<TKey, object>> exceptCols = null);
        ResultStatus Update(TKey parameter, Expression<Func<TKey, object>> idCols, Expression<Func<TKey, object>> exceptCols = null, bool setNull = false);
        ResultStatus Delete(TKey parameter, Expression<Func<TKey, object>> idCols);
        ResultStatus Delete(Expression<Func<TKey, bool>> condition);
        ResultStatus Insert(IEnumerable<TKey> parameter, Expression<Func<TKey, object>> exceptCols = null);
        ResultStatus Update(IEnumerable<TKey> parameter, Expression<Func<TKey, object>> idCols, Expression<Func<TKey, object>> exceptCols = null, bool setNull = false);
        ResultStatus Delete(IEnumerable<TKey> parameter, Expression<Func<TKey, object>> idCols);

    }

    public class QueryProcessor<TKey, TPrev> : IExecutable<TKey>, IGetTable<TKey>, IGetTableFunction<TKey>, ISelect<TKey, TPrev>, IWhere<TKey, TPrev>, IGroupBy<TKey, TPrev>, IOrderBy<TKey, TPrev>, ISkip<TKey, TPrev>, ITake<TKey, TPrev>
    {
        private string _tableName;
        private string _schemaName;
        private QueryProcessor _queryProcessor;
        private IQueryBuilder _builder;
        private IQueryExecutor _executor;

        public QueryProcessor(string tableName, string schemaName, QueryProcessor queryProcessor, IQueryBuilder builder, IQueryExecutor executor)
        {
            _tableName = tableName;
            _schemaName = schemaName;
            _queryProcessor = queryProcessor;
            _builder = builder;
            _executor = executor;
        }

        public QueryProcessor(string functionName, string schemaName, object[] parameters, QueryProcessor queryProcessor, IQueryBuilder builder, IQueryExecutor executor)
        {
            _tableName = functionName;
            _schemaName = schemaName;
            _queryProcessor = queryProcessor;
            _builder = builder;
            _executor = executor;

        }

        public IGetTable<TKey> GetTableFunction(string schemaName, string tableName, params object[] parameters)
        {
            _tableName = tableName;
            _schemaName = schemaName;

            return this;
        }

        public IEnumerable<TKey> Execute()
        {
            return _queryProcessor.Execute<TKey>();
        }
        public IEnumerable<T> Execute<T>()
        {
            return _queryProcessor.Execute<T>();
        }
        public T ExecuteScaler<T>()
        {
            return _queryProcessor.ExecuteScaler<T>();
        }
        public string GetQueryForTest()
        {
            return _queryProcessor.GetQueryForTest();
        }

        public IGroupBy<TResult, TKey> GroupBy<TResult>(Expression<Func<TKey, TResult>> columns)
        {
            var result = Process(columns.Body);
            _queryProcessor.GroupBy(result);
            return new QueryProcessor<TResult, TKey>(_tableName, _schemaName, _queryProcessor, _builder, _executor);
        }
        public ISelect<TResult, TKey> Select<TResult>(Expression<Func<TKey, TResult>> columns = null)
        {
            var result = Process(columns.Body, true);
            _queryProcessor.Select(result.Cast<INamedItem>().ToArray());
            return new QueryProcessor<TResult, TKey>(_tableName, _schemaName, _queryProcessor, _builder, _executor);
        }
        public ISelect<TResult, TKey> Select<TResult>(Expression<Func<IGroupObject<TKey, TPrev>, TResult>> columns = null)
        {
            var result = Process(columns.Body, true);
            _queryProcessor.Select(result.Cast<INamedItem>().ToArray());
            return new QueryProcessor<TResult, TKey>(_tableName, _schemaName, _queryProcessor, _builder, _executor);
        }
        public IWhere<TKey, TKey> Where(Expression<Func<TKey, bool>> condition)
        {
            var result = ProcessItem(condition.Body);
            _queryProcessor.Where(result as BEXP);
            return new QueryProcessor<TKey, TKey>(_tableName, _schemaName, _queryProcessor, _builder, _executor);
        }
        public IOrderBy<TKey, TKey> OrderBy(Expression<Func<TKey, object>> columns)
        {
            var result = Process(columns.Body);
            _queryProcessor.OrderBy(result.Cast<IQueryValue>().Select(a => new ASC { Value = a }).Cast<IQueryOrderItem>().ToArray());
            return new QueryProcessor<TKey, TKey>(_tableName, _schemaName, _queryProcessor, _builder, _executor);
        }
        public IOrderBy<TKey, TKey> OrderByDesc(Expression<Func<TKey, object>> columns)
        {
            var result = Process(columns.Body);
            _queryProcessor.OrderBy(result.Cast<IQueryValue>().Select(a => new DESC { Value = a }).Cast<IQueryOrderItem>().ToArray());
            return new QueryProcessor<TKey, TKey>(_tableName, _schemaName, _queryProcessor, _builder, _executor);
        }




        public ISkip<TKey, TKey> Skip(int? start)
        {
            _queryProcessor.Skip(start);
            return new QueryProcessor<TKey, TKey>(_tableName, _schemaName, _queryProcessor, _builder, _executor);
        }
        public ITake<TKey, TKey> Take(int? count)
        {
            _queryProcessor.Take(count);
            return new QueryProcessor<TKey, TKey>(_tableName, _schemaName, _queryProcessor, _builder, _executor);
        }

        static Dictionary<ExpressionType, BinaryOperator> binaryOperators;
        static Dictionary<ExpressionType, TransformOperator> transformOperators;
        static Dictionary<string, QueryFunctions> staticFunctions;
        static Dictionary<string, QueryFunctions> aggregteFunctions;
        static Dictionary<string, QueryFunctions> instanceFunctions;
        static Dictionary<FunctionProperty, QueryFunctions> functionProperties;
        class FunctionProperty
        {
            public Type ObjectType { get; set; }
            public string FunctionName { get; set; }
        }
        static QueryProcessor()
        {
            binaryOperators = new Dictionary<ExpressionType, BinaryOperator>();
            binaryOperators.Add(ExpressionType.And, BinaryOperator.And);
            binaryOperators.Add(ExpressionType.AndAlso, BinaryOperator.And);
            binaryOperators.Add(ExpressionType.Or, BinaryOperator.Or);
            binaryOperators.Add(ExpressionType.OrElse, BinaryOperator.Or);
            binaryOperators.Add(ExpressionType.Equal, BinaryOperator.Equal);
            binaryOperators.Add(ExpressionType.NotEqual, BinaryOperator.NotEqual);
            binaryOperators.Add(ExpressionType.GreaterThan, BinaryOperator.GreaterThan);
            binaryOperators.Add(ExpressionType.GreaterThanOrEqual, BinaryOperator.GreaterThanOrEqual);
            binaryOperators.Add(ExpressionType.LessThan, BinaryOperator.LessThan);
            binaryOperators.Add(ExpressionType.LessThanOrEqual, BinaryOperator.LessThanOrEqual);

            transformOperators = new Dictionary<ExpressionType, TransformOperator>();
            transformOperators.Add(ExpressionType.Add, TransformOperator.Add);
            transformOperators.Add(ExpressionType.Subtract, TransformOperator.Subtract);
            transformOperators.Add(ExpressionType.Multiply, TransformOperator.Multiply);
            transformOperators.Add(ExpressionType.Divide, TransformOperator.Divide);
            transformOperators.Add(ExpressionType.Modulo, TransformOperator.Modulo);
            transformOperators.Add(ExpressionType.Power, TransformOperator.Power);

            staticFunctions = new Dictionary<string, QueryFunctions>();
            staticFunctions.Add("abs", QueryFunctions.Abs);
            staticFunctions.Add("ceiling", QueryFunctions.Ceiling);
            staticFunctions.Add("floor", QueryFunctions.Floor);
            staticFunctions.Add("sqrt", QueryFunctions.Sqrt);
            staticFunctions.Add("sign", QueryFunctions.Sign);
            staticFunctions.Add("acos", QueryFunctions.Acos);
            staticFunctions.Add("asin", QueryFunctions.Asin);
            staticFunctions.Add("atan", QueryFunctions.Atan);
            staticFunctions.Add("exp", QueryFunctions.Exp);
            staticFunctions.Add("cos", QueryFunctions.Cos);
            staticFunctions.Add("sin", QueryFunctions.Sin);
            staticFunctions.Add("tan", QueryFunctions.Tan);
            staticFunctions.Add("square", QueryFunctions.Square);
            staticFunctions.Add("radians", QueryFunctions.Radians);
            staticFunctions.Add("degrees", QueryFunctions.Degrees);
            staticFunctions.Add("STGeomFromText", QueryFunctions.STGeomFromText);
            staticFunctions.Add("STPointFromText", QueryFunctions.STPointFromText);
            staticFunctions.Add("STLineFromText", QueryFunctions.STLineFromText);
            staticFunctions.Add("STPolyFromText", QueryFunctions.STPolyFromText);
            staticFunctions.Add("STMPointFromText", QueryFunctions.STMPointFromText);
            staticFunctions.Add("STMLineFromText", QueryFunctions.STMLineFromText);
            staticFunctions.Add("STMPolyFromText", QueryFunctions.STMPolyFromText);
            staticFunctions.Add("STGeomCollFromText", QueryFunctions.STGeomCollFromText);
            staticFunctions.Add("STGeomFromWKB", QueryFunctions.STGeomFromWKB);
            staticFunctions.Add("STPointFromWKB", QueryFunctions.STPointFromWKB);
            staticFunctions.Add("STLineFromWKB", QueryFunctions.STLineFromWKB);
            staticFunctions.Add("STPolyFromWKB", QueryFunctions.STPolyFromWKB);
            staticFunctions.Add("STMPointFromWKB", QueryFunctions.STMPointFromWKB);
            staticFunctions.Add("STMLineFromWKB", QueryFunctions.STMLineFromWKB);
            staticFunctions.Add("STMPolyFromWKB", QueryFunctions.STMPolyFromWKB);
            staticFunctions.Add("STGeomCollFromWKB", QueryFunctions.STGeomCollFromWKB);
            staticFunctions.Add("GeomFromGML", QueryFunctions.GeomFromGML);

            instanceFunctions = new Dictionary<string, QueryFunctions>();
            instanceFunctions.Add("substring", QueryFunctions.Substring);
            instanceFunctions.Add("tolower", QueryFunctions.Lower);
            instanceFunctions.Add("toupper", QueryFunctions.Upper);
            instanceFunctions.Add("replace", QueryFunctions.Replace);
            instanceFunctions.Add("trimend", QueryFunctions.Rtrim);
            instanceFunctions.Add("trimstart", QueryFunctions.Ltrim);
            instanceFunctions.Add("trim", QueryFunctions.Trim);
            instanceFunctions.Add("reverse", QueryFunctions.Reverse);
            instanceFunctions.Add("asbinary", QueryFunctions.STAsBinary);
            instanceFunctions.Add("astext", QueryFunctions.STAsText);
            instanceFunctions.Add("buffer", QueryFunctions.STBuffer);
            //instanceFunctions.Add("contains", QueryFunctions.STContains);
            instanceFunctions.Add("convexhull", QueryFunctions.STConvexHull);
            instanceFunctions.Add("crosses", QueryFunctions.STCrosses);
            instanceFunctions.Add("difference", QueryFunctions.STDifference);
            instanceFunctions.Add("disjoint", QueryFunctions.STDisjoint);
            instanceFunctions.Add("distance", QueryFunctions.STDistance);
            instanceFunctions.Add("envelope", QueryFunctions.STEnvelope);
            instanceFunctions.Add("getgeometryn", QueryFunctions.STGeometryN);
            instanceFunctions.Add("intersection", QueryFunctions.STIntersection);
            instanceFunctions.Add("intersects", QueryFunctions.STIntersects);
            instanceFunctions.Add("overlaps", QueryFunctions.STOverlaps);
            instanceFunctions.Add("relate", QueryFunctions.STRelate);
            instanceFunctions.Add("symmetricdifference", QueryFunctions.STSymDifference);
            instanceFunctions.Add("touches", QueryFunctions.STTouches);
            instanceFunctions.Add("union", QueryFunctions.STUnion);
            instanceFunctions.Add("within", QueryFunctions.STWithin);

            aggregteFunctions = new Dictionary<string, QueryFunctions>();
            aggregteFunctions.Add("count", QueryFunctions.Count);
            aggregteFunctions.Add("max", QueryFunctions.Max);
            aggregteFunctions.Add("min", QueryFunctions.Min);
            aggregteFunctions.Add("unionaggregate", QueryFunctions.UnionAggregate);
            aggregteFunctions.Add("collectionaggregate", QueryFunctions.CollectionAggregate);
            aggregteFunctions.Add("convexHullaggregate", QueryFunctions.ConvexHullAggregate);
            aggregteFunctions.Add("envelopeaggregate", QueryFunctions.EnvelopeAggregate);

            functionProperties = new Dictionary<FunctionProperty, QueryFunctions>();
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(string), FunctionName = "length" }, QueryFunctions.Len);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "area" }, QueryFunctions.STArea);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "boundary" }, QueryFunctions.STBoundary);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "centroid" }, QueryFunctions.STCentroid);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "dimension" }, QueryFunctions.STDimension);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "isempty" }, QueryFunctions.STIsEmpty);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "issimple" }, QueryFunctions.STIsSimple);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "isvalid" }, QueryFunctions.STIsValid);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "length" }, QueryFunctions.STLength);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "numgeometries" }, QueryFunctions.STNumGeometries);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "numpoints" }, QueryFunctions.STNumPoints);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "pointonsurface" }, QueryFunctions.STNumPoints);
            functionProperties.Add(new FunctionProperty { ObjectType = typeof(IGeometry), FunctionName = "srid" }, QueryFunctions.STSrid);


        }


        private IQueryItem ProcessItem(Expression expression)
        {
            BinaryOperator binaryOp;
            TransformOperator transformOp;
            QueryFunctions function;
            if (binaryOperators.TryGetValue(expression.NodeType, out binaryOp))
            {
                var left = ProcessItem((expression as BinaryExpression).Left);
                var right = ProcessItem((expression as BinaryExpression).Right);

                if ((left is VAL && (left as VAL).Value == DBNull.Value))
                {
                    if (binaryOp == BinaryOperator.Equal)
                        return new BEXP { Operand1 = right, Operator = BinaryOperator.IsNull };
                    else if (binaryOp == BinaryOperator.NotEqual)
                        return new BEXP { Operand1 = right, Operator = BinaryOperator.IsNotNull };
                }

                if ((right is VAL && (right as VAL).Value == DBNull.Value))
                {
                    if (binaryOp == BinaryOperator.Equal)
                        return new BEXP { Operand1 = left, Operator = BinaryOperator.IsNull };
                    else if (binaryOp == BinaryOperator.NotEqual)
                        return new BEXP { Operand1 = left, Operator = BinaryOperator.IsNotNull };
                }

                return new BEXP { Operand1 = left, Operand2 = right, Operator = binaryOp };
            }
            else if (expression.NodeType == ExpressionType.Not)
            {
                var left = ProcessItem((expression as UnaryExpression).Operand);
                return new BEXP { Operand1 = left, Operator = BinaryOperator.Not };
            }
            else if (transformOperators.TryGetValue(expression.NodeType, out transformOp))
            {
                var left = ProcessItem((expression as BinaryExpression).Left);
                var right = ProcessItem((expression as BinaryExpression).Right);

                return new TEXP { Operand1 = left, Operand2 = right, Operator = transformOp };
            }
            else if (expression.NodeType == ExpressionType.Negate)
            {
                var left = ProcessItem((expression as BinaryExpression).Left);
                return new TEXP { Operand1 = left, Operator = TransformOperator.Negate };
            }
            else if (expression is MemberExpression)
            {
                var exp = (expression as MemberExpression);
                var propFunc = functionProperties.Where(a => a.Key.FunctionName == exp.Member.Name.ToLower() && a.Key.ObjectType.IsAssignableFrom(exp.Expression.Type)).ToArray();
                if (propFunc.Length > 0)
                {
                    var param = ProcessItem(exp.Expression);
                    return new FEXP { Function = propFunc[0].Value, Parameters = new IQueryValue[] { (IQueryValue)param } };
                }
                if (exp.Expression is ParameterExpression || (exp.Expression is MemberExpression && (exp.Expression as MemberExpression).Expression is ParameterExpression))
                {
                    var col = new COL(exp.Member.Name);
                    return col;
                }
                //if (exp.Expression is ConstantExpression)
                //{
                var val = Expression.Lambda(expression).Compile().DynamicInvoke();
                if (val == null)
                    return new VAL { Value = DBNull.Value };
                if (!val.GetType().IsArray)
                    return new VAL { Value = val };
                else return new ARR { Values = ((Array)val).Cast<object>().Select(a => new VAL { Value = a }).ToArray() };

                //}
            }
            else if (expression is UnaryExpression)
            {
                return ProcessItem((expression as UnaryExpression).Operand);
                //var val = new VAL { Value = Expression.Lambda(expression).Compile().DynamicInvoke() };
                //return val;
            }
            else if (expression is ConstantExpression)
            {
                var val = new VAL { Value = Expression.Lambda(expression).Compile().DynamicInvoke() };
                return val;
            }
            else if (expression is MethodCallExpression)
            {
                var exp = (MethodCallExpression)expression;
                if (aggregteFunctions.TryGetValue(exp.Method.Name.ToLower(CultureInfo.InvariantCulture), out function) && exp.Object is ParameterExpression)
                {
                    var arguments = new List<IQueryItem>();
                    foreach (var arg in exp.Arguments)
                        arguments.Add(ProcessItem((arg as LambdaExpression).Body));
                    if (exp.Method.Name.ToLower() == "count")
                        arguments.Add(COL.ALL);
                    return new FEXP { Function = function, Parameters = arguments.Where(a => a is IQueryValue).Cast<IQueryValue>().ToArray() };
                }
                else if (exp.Method.Name.ToLower(CultureInfo.InvariantCulture) == "asc")
                {
                    var arguments = new List<IQueryItem>();
                    foreach (var arg in (expression as MethodCallExpression).Arguments)
                        arguments.Add(ProcessItem((arg as LambdaExpression).Body));
                    return new ASC { Value = (IQueryValue)arguments[0] };
                }
                else if (exp.Method.Name.ToLower(CultureInfo.InvariantCulture) == "desc")
                {
                    var arguments = new List<IQueryItem>();
                    foreach (var arg in (expression as MethodCallExpression).Arguments)
                        arguments.Add(ProcessItem((arg as LambdaExpression).Body));
                    return new DESC { Value = (IQueryValue)arguments[0] };
                }
                else if (staticFunctions.TryGetValue(exp.Method.Name.ToLower(CultureInfo.InvariantCulture), out function))
                {
                    var arguments = new List<IQueryItem>();
                    foreach (var arg in (expression as MethodCallExpression).Arguments)
                        arguments.Add(ProcessItem(arg));
                    return new FEXP { Function = function, Parameters = arguments.Where(a => a is IQueryValue).Cast<IQueryValue>().ToArray() };
                }
                else if (instanceFunctions.TryGetValue(exp.Method.Name.ToLower(CultureInfo.InvariantCulture), out function))
                {
                    var arguments = new List<IQueryItem>();
                    arguments.Add(ProcessItem((expression as MethodCallExpression).Object));
                    foreach (var arg in (expression as MethodCallExpression).Arguments)
                        arguments.Add(ProcessItem(arg));
                    return new FEXP { Function = function, Parameters = arguments.Where(a => a is IQueryValue).Cast<IQueryValue>().ToArray() };
                }
                else if (exp.Method.Name.ToLower(CultureInfo.InvariantCulture) == "in")
                {
                    var p1 = ProcessItem((expression as MethodCallExpression).Arguments[0]);
                    var p2 = ProcessItem((expression as MethodCallExpression).Arguments[1]);
                    return new BEXP { Operand1 = p1, Operand2 = p2, Operator = BinaryOperator.In };
                }
                else if (new[] { "contains", "startswith", "endswith" }.Contains(exp.Method.Name.ToLower(CultureInfo.InvariantCulture)))
                {
                    if (exp.Method.Name.ToLower(CultureInfo.InvariantCulture) == "contains" && exp.Object.Type == typeof(IGeometry))
                    {
                        var args = new List<IQueryItem>();
                        args.Add(ProcessItem(exp.Object));
                        foreach (var arg in exp.Arguments)
                            args.Add(ProcessItem(arg));
                        return new FEXP { Function = QueryFunctions.STContains, Parameters = args.Cast<IQueryValue>().ToArray() };
                    }
                    var name = exp.Method.Name.ToLower(CultureInfo.InvariantCulture);
                    var arguments = new List<IQueryItem>();
                    arguments.Add(ProcessItem(exp.Object));
                    foreach (var arg in exp.Arguments)
                        arguments.Add(ProcessItem(arg));
                    var p = (VAL)"%";
                    var op2 = arguments[1];
                    List<IQueryValue> values = new List<IQueryValue>();
                    if (name == "contains" || name == "endswith") values.Add(p);
                    values.Add((IQueryValue)op2);
                    if (name == "contains" || name == "startswith") values.Add(p);
                    op2 = new FEXP { Function = QueryFunctions.Concat, Parameters = values.ToArray() };
                    return new BEXP { Operator = BinaryOperator.Like, Operand1 = arguments[0], Operand2 = op2 };
                }

                var val = Expression.Lambda(expression).Compile().DynamicInvoke();
                if (val == null)
                    return new VAL { Value = DBNull.Value };
                if (!val.GetType().IsArray)
                    return new VAL { Value = val };
                else return new ARR { Values = ((Array)val).Cast<object>().Select(a => new VAL { Value = a }).ToArray() };

            }
            return null;
        }
        private IQueryItem[] Process(Expression expression, bool withNames = false)
        {
            List<IQueryItem> items = new List<IQueryItem>();
            if (expression is NewExpression)
            {
                int i = 0;
                foreach (var exp in (expression as NewExpression).Arguments)
                {
                    var result = ProcessItem(exp);
                    if (withNames && !(result is INamedItem))
                        result = new NEXP { Expression = (IQueryValue)result, Name = (expression as NewExpression).Members[i].Name };
                    items.Add(result);
                    i++;
                }
            }
            else if (expression is NewArrayExpression)
            {
                foreach (var exp in (expression as NewArrayExpression).Expressions)
                {
                    var result = ProcessItem(exp);
                    items.Add(result);
                }
            }
            else
            {
                var result = ProcessItem(expression);
                return new IQueryItem[] { result };
            }
            return items.ToArray();
        }

        public ResultStatus Insert(TKey parameter, Expression<Func<TKey, object>> exceptCols)
        {
            var query = _builder.GetInsertQuery(_tableName, _schemaName, parameter, exceptCols);
            return _executor.ExecuteNonQuery(query);
        }
        public ResultStatus Update(TKey parameter, Expression<Func<TKey, object>> idCols, Expression<Func<TKey, object>> exceptCols = null, bool setNull = false)
        {
            var query = _builder.GetUpdateQuery(_tableName, _schemaName, parameter, idCols, exceptCols, setNull);
            return _executor.ExecuteNonQuery(query);
        }
        public ResultStatus Update(List<TKey> parameter, Expression<Func<TKey, object>> idCols, Expression<Func<TKey, object>> exceptCols = null, bool setNull = false)
        {
            var query = _builder.GetUpdateQueryBulkUpdate(_tableName, _schemaName, parameter.ToList(), idCols, exceptCols, setNull);
            return _executor.ExecuteNonQuery(query);
        }
        public ResultStatus Insert(List<TKey> parameter, bool setNull = false)
        {
            var query = _builder.GetInsertQueryBulkInsert(_tableName, _schemaName, parameter.ToList(), setNull);
            return _executor.ExecuteNonQuery(query);
            
        }

        public ResultStatus Delete(TKey parameter, Expression<Func<TKey, object>> idCols)
        {
            var query = _builder.GetDeleteQuery(_tableName, _schemaName, parameter, idCols);
            return _executor.ExecuteNonQuery(query);
        }
        public ResultStatus Insert(IEnumerable<TKey> parameter, Expression<Func<TKey, object>> exceptCols)
        {
            DbTransaction transaction = null;
            var result = new ResultStatus { result = true };
            var opentransaction = _executor.IsTransactionOpen;
            if (!opentransaction)
                transaction = _executor.BeginTransaction();
            var i = 0;
            foreach (var p in parameter)
            {
                result = result & Insert(p, exceptCols);
                if (result.result == false) break;
                i++;
            }
            if (!opentransaction)
            {
                if (result.result) transaction.Commit();
                else transaction.Rollback();
            }
            return result;
        }
        public ResultStatus Update(IEnumerable<TKey> parameter, Expression<Func<TKey, object>> idCols, Expression<Func<TKey, object>> exceptCols = null, bool setNull = false)
        {
            DbTransaction transaction = null;
            var result = new ResultStatus { result = true };
            var opentransaction = _executor.IsTransactionOpen;
            if (!opentransaction)
                transaction = _executor.BeginTransaction();
            var i = 0;
            foreach (var p in parameter)
            {
                result = result & Update(p, idCols, exceptCols, setNull);
                if (result.result == false) break;
                i++;
            }
            if (!opentransaction)
            {
                if (result.result) transaction.Commit();
                else transaction.Rollback();
            }
            return result;
        }
        public ResultStatus Delete(IEnumerable<TKey> parameter, Expression<Func<TKey, object>> idCols)
        {
            DbTransaction transaction = null;
            var result = new ResultStatus { result = true };
            var opentransaction = _executor.IsTransactionOpen;
            if (!opentransaction)
                transaction = _executor.BeginTransaction();
            var i = 0;
            foreach (var p in parameter)
            {
                result = result & Delete(p, idCols);
                if (result.result == false) break;
                i++;
            }
            if (!opentransaction)
            {
                if (result.result) transaction.Commit();
                else transaction.Rollback();
            }
            return result;
        }

        public int Count()
        {
            return _queryProcessor.Count();
        }

        public IEnumerable<TKey> ExecuteSimpleQuery(SimpleQuery simpleQuery)
        {
            return _queryProcessor.ExecuteSimpleQuery<TKey>(simpleQuery);
        }

        public ResultStatus Delete(Expression<Func<TKey, bool>> condition)
        {
            var cond = (BEXP)ProcessItem(condition.Body);
            return _queryProcessor.Delete(cond);
        }

        public ResultStatus UpdateManyColumn(IEnumerable<TKey> parameter, Expression<Func<TKey, object>> idCols, Expression<Func<TKey, object>> exceptCols = null, bool setNull = false)
        {
            DbTransaction transaction = null;
            var result = new ResultStatus { result = true };

            var opentransaction = _executor.IsTransactionOpen;
            if (!opentransaction)
                transaction = _executor.BeginTransaction();

            for (var i = 0;
                i < parameter.Count(); i += 1000)
            {
                result = result & Update(parameter.ToList().Skip(i).Take(1000).ToList(), idCols, exceptCols, setNull);

                if (!opentransaction)
                {
                    if (!result.result)
                    {
                        transaction.Rollback();
                        return new ResultStatus { result = false };
                    }
                }
            }

            if (!opentransaction)
            {
                transaction.Commit();
            }

            return result;
        }

        public ResultStatus InsertManyColumn(IEnumerable<TKey> parameter,  bool setNull = false)
        {
            DbTransaction transaction = null;
            var result = new ResultStatus { result = true };

            var opentransaction = _executor.IsTransactionOpen;
            if (!opentransaction)
                transaction = _executor.BeginTransaction();

            for (var i = 0;i < parameter.Count(); i += 1000)
            {
                result = result & Insert(parameter.ToList().Skip(i).Take(1000).ToList(), setNull);

                if (!opentransaction)
                {
                    if (!result.result)
                    {
                        transaction.Rollback();
                        return new ResultStatus { result = false };
                    }
                }
            }

            if (!opentransaction)
            {
                transaction.Commit();
            }
            return result;
        }
    }
}
