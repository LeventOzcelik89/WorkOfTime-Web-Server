using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NetTopologySuite.Features;

namespace Infoline.Framework.Database.Mssql
{
    class MssqlQueryBuilder : IQueryBuilder
    {
        private List<QueryParameter> _parameters;
        ITypeMapper _typeMapper;

        public MssqlQueryBuilder(ITypeMapper typeMapper)
        {
            _parameters = new List<QueryParameter>();
            _typeMapper = typeMapper;
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
        public Query GetFetchQuery(bool isFunction, string tableName, string schemaName, object[] functionParameters, List<QueryStatement> statements, List<object> parameters)
        {
            return GetQuery(isFunction, tableName, schemaName, functionParameters, statements, parameters, false);
        }
        public string GetFetchQueryForTest(bool isFunction, string tableName, string schemaName, object[] functionParameters, List<QueryStatement> statements, List<object> parameters)
        {
            var query = GetQuery(isFunction, tableName, schemaName, functionParameters, statements, parameters, true);
            var param = string.Join("\r\n", query.Parameters.Select(a => string.Format("DECLARE {0} {2} = {1}", a.Name, _typeMapper.FormatSqlByType(a.Value), _typeMapper.GetSqlType(a.Value.GetType()))));
            return string.Format("{0}\r\n\r\n{1}", param, query.Command);
        }

        private Query GetQuery(bool isFunction, string tableName, string schemaName, object[] functionParameter, List<QueryStatement> statements, List<object> parameters, bool withSpace)
        {
            _parameters.Clear();
            if (functionParameter != null)
                _parameters.AddRange(functionParameter.Select((a, i) => new QueryParameter { Name = string.Format("p{0}", i), Value = a != null ? a : DBNull.Value }));

            var queries = new List<SubQuery>();
            var subQuery = new SubQuery() { IsFunction = isFunction, TableName = tableName, SchemaName = schemaName, FunctionParameters = functionParameter, Level = 0 };
            for (int i = 0; i < statements.Count; i++)
            {
                var statementType = statements[i];
                if (statementType == QueryStatement.Select)
                {
                    if (!string.IsNullOrEmpty(subQuery.Select))
                    {
                        queries.Add(subQuery);
                        subQuery = new SubQuery() { IsFunction = isFunction, PreviousQuery = subQuery, FunctionParameters = functionParameter, Level = subQuery.Level + 1 };
                    }
                    subQuery.Select = ProcessSelect(parameters[i] as INamedItem[]);
                }
                else if (statementType == QueryStatement.Where)
                {
                    if (!string.IsNullOrEmpty(subQuery.Where))
                    {
                        queries.Add(subQuery);
                        subQuery = new SubQuery() { IsFunction = isFunction, PreviousQuery = subQuery, FunctionParameters = functionParameter, Level = subQuery.Level + 1 };
                    }
                    if (parameters[i] is BEXP)
                        subQuery.Where = ProcessWhere(parameters[i] as BEXP);
                    else if (parameters[i].GetType().GetProperty("Text") != null)
                    {
                        var text = (string)parameters[i].GetType().GetProperty("Text").GetValue(parameters[i]);
                        var prms = (object[])parameters[i].GetType().GetProperty("Parameters").GetValue(parameters[i]);
                        for (var k = 0; k < prms.Length; k++)
                        {
                            var name = string.Format("@p{0}", _parameters.Count);
                            _parameters.Add(new QueryParameter { Name = name, Value = prms[i] });
                            text = text.Replace(string.Format("{{{0}}}", i), name);
                        }
                        subQuery.Where = text;
                    }
                }
                else if (statementType == QueryStatement.GroupBy)
                {
                    if (!string.IsNullOrEmpty(subQuery.GroupBy))
                    {
                        queries.Add(subQuery);
                        subQuery = new SubQuery() { IsFunction = isFunction, PreviousQuery = subQuery, FunctionParameters = functionParameter, Level = subQuery.Level + 1 };
                    }
                    subQuery.GroupBy = ProcessGroupBy(parameters[i] as IQueryItem[]);
                }
                else if (statementType == QueryStatement.OrderBy)
                {
                    subQuery.OrderBy = ProcessOrderBy(parameters[i] as IQueryOrderItem[]);
                }
                else if (statementType == QueryStatement.Skip)
                {
                    subQuery.Skip = parameters[i] != null ? (int?)(parameters[i]) : null;
                }
                else if (statementType == QueryStatement.Take)
                {
                    subQuery.Take = parameters[i] != null ? (int?)(parameters[i]) : null;
                }
            }

            var query = !withSpace ? subQuery.BuildQuery() : subQuery.BuildQueryWithSpace();
            return new Query { Command = query, Parameters = _parameters.ToArray() };
        }

        private string ProcessSelect(INamedItem[] items)
        {
            var columns = items.Select(a => ProcessQueryItem(a)).ToArray();
            var statement = string.Join(", ", columns);
            return statement;
        }
        private string ProcessWhere(BEXP exp)
        {
            var statement = ProcessBEXP(exp);
            return statement;
        }
        private string ProcessOrderBy(IQueryOrderItem[] items)
        {
            var columns = items.Select(a => ProcessQueryItem(a)).ToArray();
            var statement = string.Join(", ", columns);
            return statement;
        }
        private string ProcessGroupBy(IQueryItem[] items)
        {
            var columns = items.Select(a => ProcessQueryItem(a)).ToArray();
            var statement = string.Join(", ", columns);
            return statement;
        }


        private string ProcessQueryItem(IQueryItem item)
        {
            if (item == null) return "";
            else if (item is BEXP) return ProcessBEXP(item as BEXP);
            else if (item is TEXP) return ProcessTEXP(item as TEXP);
            else if (item is FEXP) return ProcessFEXP(item as FEXP);
            else if (item is NEXP) return ProcessNEXP(item as NEXP);
            else if (item is COL) return ProcessCOL(item as COL);
            else if (item is VAL) return ProcessVAL(item as VAL);
            else if (item is ARR) return ProcessARR(item as ARR);
            else if (item is IQueryOrderItem) return ProcessOrderItem(item as IQueryOrderItem);
            return "";

        }
        private string ProcessBEXP(BEXP exp)
        {
            if (exp == null) return "";
            var op = exp.Operator;
            var operand1 = ProcessQueryItem(exp.Operand1);
            var operand2 = ProcessQueryItem(exp.Operand2);


            if (exp.Operator == BinaryOperator.Equal)
            {
                if (exp.Operand1 is VAL && ((exp.Operand1 as VAL).Value == null || (exp.Operand1 as VAL).Value == DBNull.Value))
                {
                    return string.Format("({0} IS NULL)", operand2);
                }
                else if (exp.Operand2 is VAL && ((exp.Operand2 as VAL).Value == null || (exp.Operand2 as VAL).Value == DBNull.Value))
                {
                    return string.Format("({0} IS NULL)", operand1);
                }
            }
            else if (exp.Operator == BinaryOperator.NotEqual)
            {
                if (exp.Operand1 is VAL && ((exp.Operand1 as VAL).Value == null || (exp.Operand1 as VAL).Value == DBNull.Value))
                {
                    return string.Format("({0} IS NOT NULL)", operand2);
                }
                else if (exp.Operand2 is VAL && ((exp.Operand2 as VAL).Value == null || (exp.Operand2 as VAL).Value == DBNull.Value))
                {
                    return string.Format("({0} IS NOT NULL)", operand1);
                }
            }

            switch (op)
            {
                case BinaryOperator.And: return string.Format("({0} AND {1})", operand1, operand2);
                case BinaryOperator.Or: return string.Format("({0} OR {1})", operand1, operand2);
                case BinaryOperator.Not: return string.Format("(not ({0}))", operand1);
                case BinaryOperator.Equal: return string.Format("({0} = {1})", operand1, operand2);
                case BinaryOperator.NotEqual: return string.Format("({0} != {1})", operand1, operand2);
                case BinaryOperator.LessThan: return string.Format("({0} < {1})", operand1, operand2);
                case BinaryOperator.GreaterThan: return string.Format("({0} > {1})", operand1, operand2);
                case BinaryOperator.LessThanOrEqual: return string.Format("({0} <= {1})", operand1, operand2);
                case BinaryOperator.GreaterThanOrEqual: return string.Format("({0} >= {1})", operand1, operand2);
                case BinaryOperator.Like: return string.Format("({0} LIKE {1})", operand1, operand2);
                case BinaryOperator.NotLike: return string.Format("({0} NOT LIKE {1})", operand1, operand2);
                case BinaryOperator.In:
                    return (!string.IsNullOrEmpty(operand2) && operand2 != "()") ? string.Format("({0} IN {1})", operand1, operand2) : "1 = 0";
                case BinaryOperator.IsNull: return string.Format("({0} IS NULL)", operand1);
                case BinaryOperator.IsNotNull: return string.Format("({0} IS NOT NULL)", operand1);

                default: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorNotFound);
            }
        }
        private string ProcessTEXP(TEXP exp)
        {
            var op = exp.Operator;
            var operand1 = ProcessQueryItem(exp.Operand1);
            var operand2 = ProcessQueryItem(exp.Operand2);
            switch (op)
            {
                case TransformOperator.Add: return string.Format("({0} + {1})", operand1, operand2);
                case TransformOperator.Divide: return string.Format("({0} / {1})", operand1, operand2);
                case TransformOperator.Modulo: return string.Format("({0} % {1})", operand1, operand2);
                case TransformOperator.Multiply: return string.Format("({0} * {1})", operand1, operand2);
                case TransformOperator.Negate: return string.Format("(-1 * {0})", operand1);
                case TransformOperator.Power: return string.Format("({0} ^ {1})", operand1, operand2);
                case TransformOperator.Subtract: return string.Format("({0} - {1})", operand1, operand2);
                case TransformOperator.Lambda: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorUnsuported);
                case TransformOperator.Conditional: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorUnsuported);
                case TransformOperator.ExclusiveOr: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorUnsuported);
                case TransformOperator.OnesComplement: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorUnsuported);
                default: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorNotFound);
            }
        }
        private string ProcessFEXP(FEXP exp)
        {
            //if (exp == null) return "";
            var function = exp.Function;
            var parameters = exp.Parameters != null ? exp.Parameters.Select(a => ProcessQueryItem(a)).ToArray() : new string[0];
            switch (function)
            {
                case QueryFunctions.Ascii: return string.Format("ASCII({0})", parameters[0]);
                case QueryFunctions.Char: return string.Format("CHAR({0})", parameters[0]);
                case QueryFunctions.CharIndex:
                    if (parameters.Length == 2) return string.Format("CHARINDEX({0}, {1}) - 1", parameters[0], parameters[1]);
                    else if (parameters.Length == 3) return string.Format("CHARINDEX({0}, {1}, {2}) - 1", parameters[0], parameters[1], parameters[2]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Concat: return string.Format("CONCAT({0})", string.Join(",", parameters));
                case QueryFunctions.Difference: return string.Format("DIFFERENCE({0}, {1})", parameters[0], parameters[1]); ;
                case QueryFunctions.Format:
                    if (parameters.Length == 2) return string.Format("FORMAT({0}, {1})", parameters[0], parameters[1]);
                    else if (parameters.Length == 3) return string.Format("FORMAT({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Left: return string.Format("LEFT({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Len: return string.Format("LEN({0})", parameters[0]);
                case QueryFunctions.Lower: return string.Format("LOWER({0})", parameters[0]);
                case QueryFunctions.Ltrim: return string.Format("LTRIM({0})", parameters[0]);
                case QueryFunctions.Nchar: return string.Format("NCHAR({0})", parameters[0]);
                case QueryFunctions.Patindex: return string.Format("PATINDEX({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Quotename:
                    if (parameters.Length == 1) return string.Format("QUOTENAME({0})", parameters[0]);
                    else if (parameters.Length == 2) return string.Format("QUOTENAME({0}, {1})", parameters[0], parameters[1]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Replace: return string.Format("REPLACE({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                case QueryFunctions.Replicate: return string.Format("REPLACE({0}, {1})", parameters[0], parameters[1]); ;
                case QueryFunctions.Reverse: return string.Format("REVERSE({0})", parameters[0]);
                case QueryFunctions.Right: return string.Format("RIGHT({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Rtrim: return string.Format("RTRIM({0})", parameters[0]);
                case QueryFunctions.Trim: return string.Format("LTRIM(RTRIM({0}))", parameters[0]);
                case QueryFunctions.Soundex: return string.Format("SOUNDEX({0})", parameters[0]);
                case QueryFunctions.Space: return string.Format("SPACE({0})", parameters[0]);
                case QueryFunctions.Str:
                    if (parameters.Length == 1) return string.Format("STR({0})", parameters[0]);
                    else if (parameters.Length == 2) return string.Format("STR({0}, {1})", parameters[0], parameters[1]);
                    else if (parameters.Length == 3) return string.Format("SRT({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.String_Escape: return string.Format("STRING_ESCAPE({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.String_Split: return string.Format("STRING_SPLIT({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Stuff: return string.Format("STUFF({0}, {1}, {2}, {3})", parameters[0], parameters[1], parameters[2], parameters[3]);
                case QueryFunctions.Substring:
                    if (parameters.Length == 2) return string.Format("SUBSTRING({0}, {1}+1, LEN({0})-{1}+1)", parameters[0], parameters[1]);
                    else if (parameters.Length == 3) return string.Format("SUBSTRING({0}, {1}+1, {2})", parameters[0], parameters[1], parameters[2]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Unicode: return string.Format("UNICODE({0})", parameters[0]);
                case QueryFunctions.Upper: return string.Format("UPPER({0})", parameters[0]);

                case QueryFunctions.Abs: return string.Format("ABS({0})", parameters[0]);
                case QueryFunctions.Acos: return string.Format("ACOS({0})", parameters[0]);
                case QueryFunctions.Asin: return string.Format("ASIN({0})", parameters[0]);
                case QueryFunctions.Atan: return string.Format("ATAN({0})", parameters[0]);
                case QueryFunctions.Atn2: return string.Format("ATN2({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Ceiling: return string.Format("CEILING({0})", parameters[0]);
                case QueryFunctions.Cos: return string.Format("COS({0})", parameters[0]);
                case QueryFunctions.Cot: return string.Format("COT({0})", parameters[0]);
                case QueryFunctions.Degrees: return string.Format("DEGREES({0})", parameters[0]);
                case QueryFunctions.Exp: return string.Format("EXP({0})", parameters[0]);
                case QueryFunctions.Floor: return string.Format("FLOOR({0})", parameters[0]);
                case QueryFunctions.Log:
                    if (parameters.Length == 1) return string.Format("LOG({0})", parameters[0]);
                    else if (parameters.Length == 2) return string.Format("LOG({0}, {1})", parameters[0], parameters[1]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.PI: return "PI()";
                case QueryFunctions.Power: return string.Format("POWER({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Radians: return string.Format("RADIANS({0})", parameters[0]);
                case QueryFunctions.Rand:
                    if (parameters.Length == 0) return "RAND()";
                    else if (parameters.Length == 1) return string.Format("RAND({0})", parameters[0]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Round:
                    if (parameters.Length == 2) return string.Format("ROUND({0}, {1})", parameters[0], parameters[1]);
                    else if (parameters.Length == 3) return string.Format("ROUND({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                    else if (parameters.Length == 1) return string.Format("ROUND({0}, {1})", parameters[0], 0);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Sign: return string.Format("SIGN({0})", parameters[0]);
                case QueryFunctions.Sin: return string.Format("SIN({0})", parameters[0]);
                case QueryFunctions.Sqrt: return string.Format("SQRT({0})", parameters[0]);
                case QueryFunctions.Square: return string.Format("SQUARE({0})", parameters[0]);
                case QueryFunctions.Tan: return string.Format("TAN({0})", parameters[0]);

                case QueryFunctions.GetDate: return "GETDATE()";
                case QueryFunctions.Datepart: return string.Format("DATEPART({0}, {1})", parameters[1].ToString(), parameters[0]);


                case QueryFunctions.Avg: return string.Format("AVG({0})", parameters[0]);
                case QueryFunctions.Max: return string.Format("MAX({0})", parameters[0]);
                case QueryFunctions.Min: return string.Format("MIN({0})", parameters[0]);
                case QueryFunctions.Sum: return string.Format("SUM({0})", parameters[0]);
                case QueryFunctions.Stdev: return string.Format("STDEV({0})", parameters[0]);
                case QueryFunctions.Stdevp: return string.Format("STDEVP({0})", parameters[0]);
                case QueryFunctions.Var: return string.Format("VAR({0})", parameters[0]);
                case QueryFunctions.Varp: return string.Format("VARP({0})", parameters[0]);
                case QueryFunctions.Count: return string.Format("COUNT({0})", parameters[0]);
                case QueryFunctions.Count_Big: return string.Format("COUNT_BIG({0})", parameters[0]);
                case QueryFunctions.Grouping: return string.Format("GROUPING({0})", parameters[0]);
                case QueryFunctions.Grouping_Id: return string.Format("GROUPING_ID({0})", parameters[0]);
                case QueryFunctions.Checksum_Agg: return string.Format("CHECKSUM_AGG({0})", parameters[0]);

                case QueryFunctions.STArea: return string.Format("{0}.STArea()", parameters[0]);
                case QueryFunctions.STAsBinary: return string.Format("{0}.STAsBinary()", parameters[0]);
                case QueryFunctions.STAsText: return string.Format("{0}.STAsText()", parameters[0]);
                case QueryFunctions.STBoundary: return string.Format("{0}.STBoundary()", parameters[0]);
                case QueryFunctions.STBuffer: return string.Format("{0}.STBuffer({1})", parameters[0], parameters[1]);
                case QueryFunctions.STCentroid: return string.Format("{0}.STCentroid()", parameters[0]);
                case QueryFunctions.STContains: return string.Format("{0}.STContains({1})", parameters[0], parameters[1]);
                case QueryFunctions.STConvexHull: return string.Format("{0}.STConvexHull()", parameters[0]);
                case QueryFunctions.STCrosses: return string.Format("{0}.STCrosses({1})", parameters[0], parameters[1]);
                case QueryFunctions.STCurveN: return string.Format("{0}.STCurveN({1})", parameters[0], parameters[1]);
                case QueryFunctions.STCurveToLine: return string.Format("{0}.STCurveToLine()", parameters[0]);
                case QueryFunctions.STDifference: return string.Format("{0}.STDifference({1})", parameters[0], parameters[1]);
                case QueryFunctions.STDimension: return string.Format("{0}.STDimension()", parameters[0]);
                case QueryFunctions.STDisjoint: return string.Format("{0}.STDisjoint({1})", parameters[0], parameters[1]);
                case QueryFunctions.STDistance: return string.Format("{0}.STDistance({1})", parameters[0], parameters[1]);
                case QueryFunctions.STEndpoint: return string.Format("{0}.STEndpoint()", parameters[0]);
                case QueryFunctions.STEnvelope: return string.Format("{0}.STEnvelope()", parameters[0]);
                case QueryFunctions.STEquals: return string.Format("{0}.STEquals({1})", parameters[0], parameters[1]);
                case QueryFunctions.STExteriorRing: return string.Format("{0}.STExteriorRing()", parameters[0]);
                case QueryFunctions.STGeometryN: return string.Format("{0}.STGeometryN({1})", parameters[0], parameters[1]);
                case QueryFunctions.STGeometryType: return string.Format("{0}.STGeometryType()", parameters[0]);
                case QueryFunctions.STInteriorRingN: return string.Format("{0}.STInteriorRingN({1})", parameters[0], parameters[1]);
                case QueryFunctions.STIntersection: return string.Format("{0}.STIntersection({1})", parameters[0], parameters[1]);
                case QueryFunctions.STIntersects: return string.Format("{0}.STIntersects({1})", parameters[0], parameters[1]);
                case QueryFunctions.STIsClosed: return string.Format("{0}.STIsClosed()", parameters[0]);
                case QueryFunctions.STIsEmpty: return string.Format("{0}.STIsEmpty()", parameters[0]);
                case QueryFunctions.STIsRing: return string.Format("{0}.STIsRing()", parameters[0]);
                case QueryFunctions.STIsSimple: return string.Format("{0}.STIsSimple()", parameters[0]);
                case QueryFunctions.STIsValid: return string.Format("{0}.STIsValid()", parameters[0]);
                case QueryFunctions.STLength: return string.Format("{0}.STLength()", parameters[0]);
                case QueryFunctions.STNumCurves: return string.Format("{0}.STNumCurves()", parameters[0]);
                case QueryFunctions.STNumGeometries: return string.Format("{0}.STNumGeometries()", parameters[0]);
                case QueryFunctions.STNumInteriorRing: return string.Format("{0}.STNumInteriorRing()", parameters[0]);
                case QueryFunctions.STNumPoints: return string.Format("{0}.STNumPoints()", parameters[0]);
                case QueryFunctions.STOverlaps: return string.Format("{0}.STOverlaps({1})", parameters[0], parameters[1]);
                case QueryFunctions.STPointN: return string.Format("{0}.STPointN({1})", parameters[0], parameters[1]);
                case QueryFunctions.STPointOnSurface: return string.Format("{0}.STPointOnSurface({1})", parameters[0], parameters[1]);
                case QueryFunctions.STRelate: return string.Format("{0}.STRelate({1}, {2})", parameters[0], parameters[1], parameters[2]);
                case QueryFunctions.STSrid: return string.Format("{0}.STSrid", parameters[0]);
                case QueryFunctions.STStartPoint: return string.Format("{0}.STStartPoint()", parameters[0]);
                case QueryFunctions.STSymDifference: return string.Format("{0}.STSymDifference({1})", parameters[0], parameters[1]);
                case QueryFunctions.STTouches: return string.Format("{0}.STTouches({1})", parameters[0], parameters[1]);
                case QueryFunctions.STUnion: return string.Format("{0}.STUnion({1})", parameters[0], parameters[1]);
                case QueryFunctions.STWithin: return string.Format("{0}.STWithin({1})", parameters[0], parameters[1]);
                case QueryFunctions.STX: return string.Format("{0}.STX", parameters[0]);
                case QueryFunctions.STY: return string.Format("{0}.STY", parameters[0]);
                case QueryFunctions.STGeomFromText: return string.Format("geography::STGeomFromText({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STPointFromText: return string.Format("geography::STPointFromText({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STLineFromText: return string.Format("geography::STLineFromText({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STPolyFromText: return string.Format("geography::STPolyFromText({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMPointFromText: return string.Format("geography::STMPointFromText({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMLineFromText: return string.Format("geography::STMLineFromText({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMPolyFromText: return string.Format("geography::STMPolyFromText({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STGeomCollFromText: return string.Format("geography::STGeomCollFromText({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STGeomFromWKB: return string.Format("geography::STGeomFromWKB({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STPointFromWKB: return string.Format("geography::STPointFromWKB({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STLineFromWKB: return string.Format("geography::STLineFromWKB({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STPolyFromWKB: return string.Format("geography::STPolyFromWKB({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMPointFromWKB: return string.Format("geography::STMPointFromWKB({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMLineFromWKB: return string.Format("geography::STMLineFromWKB({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMPolyFromWKB: return string.Format("geography::STMPolyFromWKB({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STGeomCollFromWKB: return string.Format("geography::STGeomCollFromWKB({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.GeomFromGML: return string.Format("geography::GeomFromGML({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.AsBinaryZM: return string.Format("{0}.AsBinaryZM()", parameters[0]);
                case QueryFunctions.AsGml: return string.Format("{0}.AsGml()", parameters[0]);
                case QueryFunctions.AsTextZM: return string.Format("{0}.AsTextZM()", parameters[0]);
                case QueryFunctions.BufferWithCurves: return string.Format("{0}.BufferWithCurves({1})", parameters[0], parameters[1]);
                case QueryFunctions.BufferWithTolerance: return string.Format("{0}.BufferWithTolerance({1}, {2}, {3})", parameters[0], parameters[1], parameters[2], parameters[3]);
                case QueryFunctions.CurveToLineWithTolerance: return string.Format("{0}.CurveToLineWithTolerance({1}, {2})", parameters[0], parameters[1], parameters[2]);
                case QueryFunctions.InstanceOf: return string.Format("{0}.InstanceOf({1})", parameters[0], parameters[1]);
                case QueryFunctions.Filter: return string.Format("{0}.Filter({1})", parameters[0], parameters[1]);
                case QueryFunctions.HasM: return string.Format("{0}.HasM()", parameters[0]);
                case QueryFunctions.HasZ: return string.Format("{0}.HasZ()", parameters[0]);
                case QueryFunctions.IsNull: return string.Format("{0}.IsNull()", parameters[0]);
                case QueryFunctions.IsValidDetailed: return string.Format("{0}.IsValidDetailed()", parameters[0]);
                case QueryFunctions.M: return string.Format("{0}.M", parameters[0]);
                case QueryFunctions.MakeValid: return string.Format("{0}.MakeValid()", parameters[0]);
                case QueryFunctions.MinDbCompatibilityLevel: return string.Format("{0}.MinDbCompatibilityLevel()", parameters[0]);
                case QueryFunctions.Reduce: return string.Format("{0}.Reduce({1})", parameters[0], parameters[1]);
                case QueryFunctions.ShortestLineTo: return string.Format("{0}.ShortestLineTo({1})", parameters[0], parameters[1]);
                case QueryFunctions.ToString: return string.Format("{0}.ToString()", parameters[0]);
                case QueryFunctions.Z: return string.Format("{0}.Z", parameters[0]);
                case QueryFunctions.EnvelopeAngle: return string.Format("{0}.EnvelopeAngle()", parameters[0]);
                case QueryFunctions.EnvelopeCenter: return string.Format("{0}.EnvelopeCenter()", parameters[0]);
                case QueryFunctions.Lat: return string.Format("{0}.Lat", parameters[0]);
                case QueryFunctions.Long: return string.Format("{0}.Long", parameters[0]);
                case QueryFunctions.NumRing: return string.Format("{0}.NumRing()", parameters[0]);
                case QueryFunctions.ReorientObject: return string.Format("{0}.ReorientObject({1})", parameters[0], parameters[1]);
                case QueryFunctions.RingN: return string.Format("{0}.RingN({1})", parameters[0], parameters[1]);
                case QueryFunctions.CollectionAggregate: return string.Format("geography::CollectionAggregate({0})", parameters[0]);
                case QueryFunctions.ConvexHullAggregate: return string.Format("geography::ConvexHullAggregate({0})", parameters[0]);
                case QueryFunctions.EnvelopeAggregate: return string.Format("geography::EnvelopeAggregate({0})", parameters[0]);
                case QueryFunctions.UnionAggregate: return string.Format("geography::UnionAggregate({0})", parameters[0]);

                default: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorNotFound);
            }
            throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorUnsuported);
        }
        private string ProcessNEXP(NEXP exp)
        {
            var name = exp.Name;
            var expression = ProcessQueryItem(exp.Expression);
            return string.Format("{0} AS [{1}]", expression, name);
        }
        private string ProcessCOL(COL col)
        {
            if (col.Name == "*") return col.Name;
            if (col.Name.ToLower().Contains("case when"))
                return string.Format("{0}", col.Name);
            return string.Format("[{0}]", col.Name);
        }
        private string ProcessVAL(VAL val)
        {
            var parameterName = string.Format("@p{0}", _parameters.Count);
            _parameters.Add(new QueryParameter { Name = parameterName, Value = _typeMapper.ConvertToSql(val.Value) });
            return parameterName;
        }
        private string ProcessARR(ARR arr)
        {
            //var values = arr.Values.Select(a => ProcessQueryItem(a)).ToArray();
            //return string.Format("({0})", string.Join(",", values));
            //TODO: Şahin Güncelleme Sorun olursa şahini bilgilendir
            var values = arr.Values.Select(a => "'" + string.Join("", a.ToString().Split('\'')) + "'").ToArray();
            return string.Format("({0})", string.Join(",", values));
        }
        private string ProcessOrderItem(IQueryOrderItem item)
        {
            var value = ProcessQueryItem(item.Value);
            var result = string.Format("{0} {1}", value, item.Type == QueryOrderType.ASC ? "ASC" : "DESC");
            return result;
        }


        class SubQuery
        {
            public bool IsFunction { get; set; } // Table Valued Function
            public string TableName { get; set; }
            public string SchemaName { get; set; }
            public object[] FunctionParameters { get; set; }
            public SubQuery PreviousQuery { get; set; }
            public int Level { get; set; }
            public string Where { get; set; }
            public string GroupBy { get; set; }
            public string Select { get; set; }
            public string OrderBy { get; set; }
            public int? Skip { get; set; }
            public int? Take { get; set; }

            public string BuildQuery()
            {
                string table;
                if (PreviousQuery != null)
                    table = string.Format("({0}) AS TVIEW{1}", PreviousQuery.BuildQuery(), Level);
                else
                {
                    if (!IsFunction)
                        table = string.Format("[{0}] WITH ( NOLOCK )", TableName);
                    else
                    {
                        var parameters = string.Join(",", FunctionParameters.Select((a, i) => string.Format("@p{0}", i)));
                        table = string.Format("[{0}]({1}) ", TableName, parameters);
                    }
                }

                return Combine(table);
            }
            public string BuildQueryWithSpace()
            {
                string table;
                if (PreviousQuery != null)
                    table = string.Format("\r\n\t(\r\n{0}\r\n\t) AS TVIEW{1}\r\n",
                        string.Join("\r\n", PreviousQuery.BuildQueryWithSpace().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(a => "\t" + a).ToArray()), Level);
                else
                    table = string.Format("[{0}] WITH ( NOLOCK ) \r\n", TableName);
                return CombineWithSpace(table);
            }

            private string Combine(string table)
            {
                var topStatement = Take != null & Skip == null ? "TOP " + Take : "";
                var selectStatement = GetSelect();
                var whereStatement = !string.IsNullOrEmpty(Where) ? "WHERE " + Where : "";
                var groupByStatement = !string.IsNullOrEmpty(GroupBy) ? "GROUP BY " + GroupBy : "";
                var orderByStatement = !string.IsNullOrEmpty(OrderBy) ? "ORDER BY " + OrderBy : "";
                var offsetStatement = Skip != null ? string.Format("OFFSET {0} ROWS ", Skip) : "";
                offsetStatement += Take != null & Skip != null ? string.Format("FETCH NEXT {0} ROWS ONLY", Take) : "";

                var result = string.Format("SELECT {0} {1} FROM {2} {3} {4} {5} {6}",
                    topStatement, selectStatement, table,
                    whereStatement, groupByStatement, orderByStatement, offsetStatement);
                return result;
            }
            private string CombineWithSpace(string table)
            {
                var topStatement = Take != null & Skip == null ? "TOP " + Take : "";
                var selectStatement = GetSelect() + "\r\n";
                var whereStatement = !string.IsNullOrEmpty(Where) ? "WHERE " + Where + "\r\n" : "";
                var groupByStatement = !string.IsNullOrEmpty(GroupBy) ? "GROUP BY " + GroupBy + "\r\n" : "";
                var offsetStatement = Skip != null ? string.Format(" OFFSET {0} ROWS ", Skip) : "";
                offsetStatement += Take != null & Skip != null ? string.Format("FETCH NEXT {0} ROWS ONLY", Take) : "";
                offsetStatement += !string.IsNullOrEmpty(offsetStatement) ? "\r\n" : "";
                var orderByStatement = !string.IsNullOrEmpty(OrderBy) ? "ORDER BY " + OrderBy : "";
                orderByStatement += !string.IsNullOrEmpty(orderByStatement) && string.IsNullOrEmpty(offsetStatement) ? "\r\n" : "";


                var result = string.Format("SELECT {0} {1}FROM {2}{3}{4}{5}{6}",
                    topStatement, selectStatement, table,
                    whereStatement, groupByStatement, orderByStatement, offsetStatement);
                return result;
            }
            private string GetSelect()
            {
                if (!string.IsNullOrEmpty(Select))
                    return Select;

                if (GroupBy != null)
                {
                    return GroupBy;
                }
                else
                    return "*";
            }
        }


        public Query GetInsertQuery(string tableName, string schemaName, Dictionary<string, object> parameter, string[] exceptCols)
        {
            if (exceptCols == null)
                exceptCols = new string[0];

            var parameterName = "";
            var parameterValue = "";
            var param = new List<QueryParameter>();
            foreach (KeyValuePair<string, object> p in parameter)
            {
                if (exceptCols.Contains(p.Key)) continue;
                parameterName += ",[" + p.Key + "]";
                parameterValue += ",@" + p.Key;
                param.Add(new QueryParameter { Name = "@" + p.Key, Value = _typeMapper.ConvertToSql(p.Value) });
            }
            parameterName = parameterName.Substring(1);
            parameterValue = parameterValue.Substring(1);

            string sql = null;
            if (parameterName.Length > 2)
                sql = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2})", tableName, parameterName, parameterValue);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }
        public Query GetUpdateQuery(string tableName, string schemaName, Dictionary<string, object> parameter, string[] idCols, string[] exceptCols = null, bool setNull = false)
        {
            if (exceptCols == null)
                exceptCols = new string[0];

            var parametrename = "";
            var param = new List<QueryParameter>();
            foreach (KeyValuePair<string, object> p in parameter)
            {
                var value = _typeMapper.ConvertToSql(p.Value);
                if (!setNull && value == null) continue;
                if (exceptCols.Contains(p.Key)) continue;
                parametrename += ",[" + p.Key + "]=@" + p.Key;
                param.Add(new QueryParameter { Name = "@" + p.Key, Value = value });
            }
            if (parametrename.Length > 1)
                parametrename = parametrename.Substring(1);

            var whereStatement = "";
            for (var i = 0; i < idCols.Length; i++)
            {
                var id = idCols[i];
                whereStatement += " AND [" + id + "]=@p" + i;
                param.Add(new QueryParameter { Name = "@p" + i, Value = _typeMapper.ConvertToSql(parameter[id]) });
            }
            if (whereStatement.Length > 5)
                whereStatement = whereStatement.Substring(5);
            whereStatement = whereStatement.Length > 0 ? " WHERE " + whereStatement : "";

            string sql = null;
            if (parametrename.Length > 2)
                sql = string.Format("UPDATE [{0}] SET {1} {2}", tableName, parametrename, whereStatement);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }
        public Query GetDeleteQuery(string tableName, string schemaName, Dictionary<string, object> parameter, string[] idCols)
        {
            var whereStatement = "";
            var param = new List<QueryParameter>();
            for (var i = 0; i < idCols.Length; i++)
            {
                var id = idCols[i];
                whereStatement += " AND " + id + "=@p" + i;
                param.Add(new QueryParameter { Name = "@p" + i, Value = _typeMapper.ConvertToSql(parameter[id]) });
            }
            whereStatement = whereStatement.Substring(5);

            if (param.Count == 0)
                throw new Exception("Delete işlemi için bir kolon seçmeniz gerekmekte.");

            var sql = string.Format("DELETE FROM [{0}] WHERE {1}", tableName, whereStatement);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }
        public Query GetDeleteQuery(string tableName, string schemaName, BEXP condition)
        {
            if (condition == null) throw new Exception("Delete işleminde koşul null bırakılamaz.");
            _parameters.Clear();
            var whereStatement = ProcessWhere(condition as BEXP);
            var sql = string.Format("DELETE FROM [{0}] WHERE {1}", tableName, whereStatement);
            return new Query { Command = sql, Parameters = _parameters.ToArray() };
        }

        public Query GetInsertQuery<T>(string tableName, string schemaName, T parameter, Expression<Func<T, object>> exceptCols)
        {
            IEnumerable<string> except;
            if (exceptCols != null)
                except = ExpressionHelper.GetPropertyNames<T, object>(exceptCols);
            else except = new string[0];

            if (parameter == null)
            {
                return new Query { Command = "-- NULL" };
            }

            var parameterName = "";
            var parameterValue = "";
            var param = new List<QueryParameter>();
            foreach (var p in typeof(T).GetProperties().Where(p => p.GetValue(parameter, null) != null))
            {
                if (except.Contains(p.Name)) continue;
                parameterName += ",[" + p.Name + "]";
                parameterValue += ",@" + p.Name;
                param.Add(new QueryParameter { Name = "@" + p.Name, Value = _typeMapper.ConvertToSql(p.GetValue(parameter)) });
            }
            parameterName = parameterName.Substring(1);
            parameterValue = parameterValue.Substring(1);

            string sql = null;
            if (parameterName.Length > 2)
                sql = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2})", tableName, parameterName, parameterValue);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }
        public Query GetUpdateQuery<T>(string tableName, string schemaName, T parameter, Expression<Func<T, object>> idCols, Expression<Func<T, object>> exceptCols = null, bool setNull = false)
        {

            if (parameter == null)
            {
                return new Query { Command = "-- NULL" };
            }

            IEnumerable<string> except;
            if (exceptCols != null)
                except = ExpressionHelper.GetPropertyNames<T, object>(exceptCols);
            else except = new string[0];

            var parametrename = "";
            var param = new List<QueryParameter>();
            foreach (var p in typeof(T).GetProperties())
            {
                if (!setNull && (p.GetValue(parameter, null) == null || string.IsNullOrEmpty(p.GetValue(parameter, null).ToString().Trim()))) continue;
                if (except.Contains(p.Name)) continue;
                parametrename += ",[" + p.Name + "]=@" + p.Name;
                param.Add(new QueryParameter { Name = "@" + p.Name, Value = _typeMapper.ConvertToSql(p.GetValue(parameter)) });
            }
            if (parametrename.Length > 1)
                parametrename = parametrename.Substring(1);

            IEnumerable<PropertyInfo> ids;
            if (exceptCols != null)
                ids = ExpressionHelper.GetProperties<T, object>(idCols);
            else ids = new PropertyInfo[0];


            var whereStatement = "";
            var k = 0;
            foreach (var id in ids)
            {
                whereStatement += " AND [" + id.Name + "]=@p" + k;
                param.Add(new QueryParameter { Name = "@p" + k, Value = _typeMapper.ConvertToSql(id.GetValue(parameter, null)) });
                k++;
            }
            if (whereStatement.Length > 5)
                whereStatement = whereStatement.Substring(5);
            whereStatement = whereStatement.Length > 0 ? " WHERE " + whereStatement : "";

            string sql = null;
            if (parametrename.Length > 2)
                sql = string.Format("UPDATE [{0}] SET {1} {2}", tableName, parametrename, whereStatement);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }

        public Query GetUpdateQueryBulkUpdate<T>(string tableName, string schemaName, List<T> parameter, Expression<Func<T, object>> idCols, Expression<Func<T, object>> exceptCols = null, bool setNull = false)
        {
            IEnumerable<string> except;
            if (exceptCols != null)
                except = ExpressionHelper.GetPropertyNames<T, object>(exceptCols);
            else except = new string[0];

            string sql = "";
            foreach (var parameterItem in parameter)
            {
                var parametrename = "";
                var param = new List<QueryParameter>();
                foreach (var p in typeof(T).GetProperties())
                {
                    if (!setNull && (p.GetValue(parameterItem, null) == null || string.IsNullOrEmpty(p.GetValue(parameterItem, null).ToString().Trim()))) continue;
                    if (except.Contains(p.Name)) continue;

                    parametrename = ConvertParameterStringValueToSqlValueForUpdate(parameterItem, parametrename, p);

                    //param.Add(new QueryParameter { Name = "@" + p.Name, Value = _typeMapper.ConvertToSql(p.GetValue(parameter)) });
                }
                if (parametrename.Length > 1)
                    parametrename = parametrename.Substring(1);

                IEnumerable<PropertyInfo> ids;
                if (exceptCols != null)
                    ids = ExpressionHelper.GetProperties<T, object>(idCols);
                else ids = new PropertyInfo[0];


                var whereStatement = "";
                var k = 0;
                foreach (var id in ids)
                {
                    whereStatement += " AND [" + id.Name + "]='" + _typeMapper.ConvertToSql(id.GetValue(parameterItem, null)) + "'";
                    k++;
                }
                if (whereStatement.Length > 5)
                    whereStatement = whereStatement.Substring(5);
                whereStatement = whereStatement.Length > 0 ? " WHERE " + whereStatement : "";


                if (parametrename.Length > 2)
                {
                    sql += string.Format("UPDATE [{0}] SET {1} {2} \n\r", tableName, parametrename, whereStatement);
                }
            }
            return new Query { Command = sql, Parameters = null };
        }
        public Query GetInsertQueryBulkInsert<T>(string tableName, string schemaName, List<T> parameter, bool setNull = false)
        {
            string sql = "INSERT INTO ";
            sql += tableName;
            sql += " (";
            var prop = typeof(T).GetProperties().OrderBy(a => a.Name);
            foreach (var p in prop)
            {
                sql += "[" + p.Name + "],";
            }
            sql = sql.Substring(0, sql.Length - 1);
            sql += ") VALUES";

            foreach (var parameterItem in parameter)
            {
                var parametrename = "";
                var param = new List<QueryParameter>();
                foreach (var p in prop)
                {
                    parametrename += ConvertParameterStringValueToSqlValueForInsert(parameterItem, p);
                }
                if (parametrename.Length > 1)
                    parametrename = parametrename.Substring(1);
                if (parametrename.Length > 2)
                {
                    sql += string.Format("( {0} ),", parametrename);
                }
            }
            sql = sql.Substring(0, sql.Length - 1);

            return new Query { Command = sql, Parameters = null };
        }

        private string ConvertParameterStringValueToSqlValueForInsert<T>(T parameterItem, PropertyInfo p)
        {
            var parametrename = "";
            string _name = p.Name;

            var _type = p.PropertyType.GenericTypeArguments.FirstOrDefault();
            if (p.GetValue(parameterItem, null) == null)
            {
                parametrename = ",null";
            }

            else if (_type == typeof(DateTime))
            {
                parametrename = ",'" + Convert.ToDateTime(_typeMapper.ConvertToSql(p.GetValue(parameterItem))).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            }
            else if (_type == typeof(double) || _type == typeof(float))
            {
                parametrename = "," + Convert.ToString(_typeMapper.ConvertToSql(p.GetValue(parameterItem))).Replace(",", ".");
            }
            else
            {
                parametrename = ",'" + _typeMapper.ConvertToSql(p.GetValue(parameterItem)) + "'";
            }

            return parametrename;
        }

        private string ConvertParameterStringValueToSqlValueForUpdate<T>(T parameterItem, string parametrename, PropertyInfo p)
        {
            if (p.PropertyType.GenericTypeArguments.FirstOrDefault() == typeof(DateTime))
                parametrename += string.Format(",[" + p.Name + "]='{0}'", Convert.ToDateTime(_typeMapper.ConvertToSql(p.GetValue(parameterItem))).ToString("yyyy-MM-dd HH:mm:ss.fff"));
            else if (p.PropertyType.GenericTypeArguments.FirstOrDefault() == typeof(double) || p.PropertyType.GenericTypeArguments.FirstOrDefault() == typeof(float))
                parametrename += ",[" + p.Name + "]=" + Convert.ToString(_typeMapper.ConvertToSql(p.GetValue(parameterItem))).Replace(",", ".");
            else
                parametrename += ",[" + p.Name + "]='" + _typeMapper.ConvertToSql(p.GetValue(parameterItem)) + "'";
            return parametrename;
        }

        public Query GetDeleteQuery<T>(string tableName, string schemaName, T parameter, Expression<Func<T, object>> idCols)
        {
            var idProperties = ExpressionHelper.GetProperties<T, object>(idCols);

            var whereStatement = "";
            var param = new List<QueryParameter>();
            var k = 0;
            foreach (var id in idProperties)
            {
                whereStatement += " AND " + id.Name + "=@p" + k;
                param.Add(new QueryParameter { Name = "@p" + k, Value = _typeMapper.ConvertToSql(id.GetValue(parameter, null)) });
                k++;
            }
            whereStatement = whereStatement.Substring(5);

            if (param.Count == 0)
                throw new Exception("Delete işlemi için bir kolon seçmeniz gerekmekte.");

            var sql = string.Format("DELETE FROM [{0}] WHERE {1}", tableName, whereStatement);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }

        public Query GetInsertQuery(string tableName, string schemaName, IFeature feature, string geometryColumnName)
        {
            //if (exceptCols == null)
            //    exceptCols = new string[0];

            var parameterName = "";
            var parameterValue = "";
            var param = new List<QueryParameter>();
            foreach (var name in feature.Attributes.GetNames())
            {
                //if (exceptCols.Contains(p.Key)) continue;
                parameterName += ",[" + name + "]";
                parameterValue += ",@" + name;
                param.Add(new QueryParameter { Name = "@" + name, Value = _typeMapper.ConvertToSql(feature.Attributes[name]) });
            }
            parameterName += ",[" + geometryColumnName + "]";
            parameterValue += ",@" + geometryColumnName;
            param.Add(new QueryParameter { Name = "@" + geometryColumnName, Value = _typeMapper.ConvertToSql(feature.Geometry) });

            parameterName = parameterName.Substring(1);
            parameterValue = parameterValue.Substring(1);

            string sql = null;
            if (parameterName.Length > 2)
                sql = string.Format("INSERT INTO [{0}] ({1}) VALUES ({2})", tableName, parameterName, parameterValue);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }

        public Query GetDropTableQuery(string tableName, string schemaName)
        {
            return new Query { Command = string.Format("DROP TABLE {0}", tableName) };
        }

        public Query GetTableExistsQuery(string tableName, string schemaName)
        {
            schemaName = null;
            var query = ConvertToQuery("SELECT CAST(COUNT(*) as BIT) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {0} and (TABLE_SCHEMA = {1} or {1} is null)", tableName, schemaName);
            return query;
        }

        public Query GetUpdateQuery(string tableName, string schemaName, IFeature feature, string geometryColumnName, string[] idCols, bool setNull = false)
        {

            var parametrename = "";
            var param = new List<QueryParameter>();
            foreach (var name in feature.Attributes.GetNames())
            {
                var value = _typeMapper.ConvertToSql(feature.Attributes[name]);
                if (!setNull && value == null) continue;

                parametrename += ",[" + name + "]=@" + value;
                param.Add(new QueryParameter { Name = "@" + name, Value = value });
            }
            var geometry = _typeMapper.ConvertToSql(feature.Geometry);
            // TODO: IFeatureUpdate. Kontrol et[FM]
            if (setNull || geometry != null)
            {
                parametrename += ",[" + geometryColumnName + "]=@" + geometryColumnName;
                param.Add(new QueryParameter { Name = "@" + geometryColumnName, Value = geometry });
            }

            if (parametrename.Length > 1)
                parametrename = parametrename.Substring(1);

            var whereStatement = "";
            for (var i = 0; i < idCols.Length; i++)
            {
                var id = idCols[i];
                whereStatement += " AND [" + id + "]=@p" + i;
                param.Add(new QueryParameter { Name = "@p" + i, Value = _typeMapper.ConvertToSql(feature.Attributes[id]) });
            }
            if (whereStatement.Length > 5)
                whereStatement = whereStatement.Substring(5);
            whereStatement = whereStatement.Length > 0 ? " WHERE " + whereStatement : "";

            string sql = null;
            if (parametrename.Length > 2)
                sql = string.Format("UPDATE [{0}] SET {1} {2}", tableName, parametrename, whereStatement);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }
    }
}
