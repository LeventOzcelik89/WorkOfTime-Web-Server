using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using NetTopologySuite.Features;

namespace Infoline.Framework.Database.Postgis
{
    class PostgisQueryBuilder : IQueryBuilder
    {
        private List<QueryParameter> _parameters;
        ITypeMapper _typeMapper;

        public PostgisQueryBuilder(ITypeMapper typeMapper)
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
        public Query GetFetchQuery(bool isFunction, string tableName, string schemaName, object[] functionParmeters, List<QueryStatement> statements, List<object> parameters)
        {
            return GetQuery(isFunction, tableName, schemaName, functionParmeters, statements, parameters, false);
        }
        public string GetFetchQueryForTest(bool isFunction, string tableName, string schemaName, object[] functionParameters, List<QueryStatement> statements, List<object> parameters)
        {
            var query = GetQuery(isFunction, tableName, schemaName, functionParameters, statements, parameters, true);
            var result = query.Command;
            foreach (var par in query.Parameters)
                result = result.Replace(par.Name, _typeMapper.FormatSqlByType(par.Value));
            return result;
        }
        private Query GetQuery(bool isFunction, string tableName, string schemaName, object[] functionParameters, List<QueryStatement> statements, List<object> parameters, bool withSpace)
        {
            _parameters.Clear();
            var queries = new List<SubQuery>();
            var subQuery = new SubQuery() { IsFunction = isFunction, TableName = tableName, FunctionParameters = functionParameters, SchemaName = schemaName, Level = 0 };
            for (int i = 0; i < statements.Count; i++)
            {
                var statementType = statements[i];
                if (statementType == QueryStatement.Select)
                {
                    if (!string.IsNullOrEmpty(subQuery.Select))
                    {
                        queries.Add(subQuery);
                        subQuery = new SubQuery() { IsFunction = isFunction, FunctionParameters = functionParameters, PreviousQuery = subQuery, Level = subQuery.Level + 1 };
                    }
                    subQuery.Select = ProcessSelect(parameters[i] as INamedItem[]);
                }
                else if (statementType == QueryStatement.Where)
                {
                    if (!string.IsNullOrEmpty(subQuery.Where))
                    {
                        queries.Add(subQuery);
                        subQuery = new SubQuery() { IsFunction = isFunction, FunctionParameters = functionParameters, PreviousQuery = subQuery, Level = subQuery.Level + 1 };
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
                        subQuery = new SubQuery() { IsFunction = isFunction, FunctionParameters = functionParameters, PreviousQuery = subQuery, Level = subQuery.Level + 1 };
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
            var op = exp.Operator;
            var operand1 = ProcessQueryItem(exp.Operand1);
            var operand2 = ProcessQueryItem(exp.Operand2);
            switch (op)
            {
                case BinaryOperator.And: return string.Format("({0} and {1})", operand1, operand2);
                case BinaryOperator.Or: return string.Format("({0} or {1})", operand1, operand2);
                case BinaryOperator.Not: return string.Format("(not ({0}))", operand1);
                case BinaryOperator.Equal: return string.Format("({0} = {1})", operand1, operand2);
                case BinaryOperator.NotEqual: return string.Format("({0} != {1})", operand1, operand2);
                case BinaryOperator.LessThan: return string.Format("({0} < {1})", operand1, operand2);
                case BinaryOperator.GreaterThan: return string.Format("({0} > {1})", operand1, operand2);
                case BinaryOperator.LessThanOrEqual: return string.Format("({0} <= {1})", operand1, operand2);
                case BinaryOperator.GreaterThanOrEqual: return string.Format("({0} >= {1})", operand1, operand2);
                case BinaryOperator.Like: return string.Format("({0} like {1})", operand1, operand2);
                case BinaryOperator.NotLike: return string.Format("({0} not like {1})", operand1, operand2);
                case BinaryOperator.IsNull: return string.Format("({0} is null)", operand1);
                case BinaryOperator.IsNotNull: return string.Format("({0} not is null)", operand1);
                case BinaryOperator.In: return string.Format("({0} IN {1})", operand1, operand2);
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
                case QueryFunctions.Ascii: return string.Format("ascii({0})", parameters[0]);
                case QueryFunctions.Char: return string.Format("chr({0})", parameters[0]);
                case QueryFunctions.CharIndex:
                    if (parameters.Length == 2) return string.Format("position({0} in {1})", parameters[0], parameters[1]);
                    //else if (parameters.Length == 3) return string.Format("CHARINDEX({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Concat: return string.Format("{0}", string.Join(" || ", parameters));
                case QueryFunctions.Difference: return string.Format("DIFFERENCE({0}, {1})", parameters[0], parameters[1]); ;
                case QueryFunctions.Format:
                    if (parameters.Length == 2) return string.Format("FORMAT({0}, {1})", parameters[0], parameters[1]);
                    else if (parameters.Length == 3) return string.Format("FORMAT({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Left: return string.Format("left({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Len: return string.Format("length({0})", parameters[0]);
                case QueryFunctions.Lower: return string.Format("lower({0})", parameters[0]);
                case QueryFunctions.Ltrim: return string.Format("ltrim({0})", parameters[0]);
                case QueryFunctions.Nchar: return string.Format("NCHAR({0})", parameters[0]);
                case QueryFunctions.Patindex: return string.Format("PATINDEX({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Quotename:
                    if (parameters.Length == 1) return string.Format("QUOTENAME({0})", parameters[0]);
                    else if (parameters.Length == 2) return string.Format("QUOTENAME({0}, {1})", parameters[0], parameters[1]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Replace: return string.Format("replace({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                //case QueryFunctions.Replicate: return string.Format("REPLICATE({0}, {1})", parameters[0], parameters[1]); ;
                case QueryFunctions.Reverse: return string.Format("reverse({0})", parameters[0]);
                case QueryFunctions.Right: return string.Format("right({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Rtrim: return string.Format("rtrim({0})", parameters[0]);
                //case QueryFunctions.Soundex: return string.Format("SOUNDEX({0})", parameters[0]);
                //case QueryFunctions.Space: return string.Format("SPACE({0})", parameters[0]);
                //case QueryFunctions.Str:
                //    if (parameters.Length == 1) return string.Format("STR({0})", parameters[0]);
                //    else if (parameters.Length == 2) return string.Format("STR({0}, {1})", parameters[0], parameters[1]);
                //    else if (parameters.Length == 3) return string.Format("SRT({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                //    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.String_Escape: return string.Format("STRING_ESCAPE({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.String_Split: return string.Format("STRING_SPLIT({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Stuff: return string.Format("STUFF({0}, {1}, {2}, {3})", parameters[0], parameters[1], parameters[2], parameters[3]);
                case QueryFunctions.Substring: return string.Format("substring({0} from {1} for {2})", parameters[0], parameters[1], parameters[2]);
                case QueryFunctions.Unicode: return string.Format("UNICODE({0})", parameters[0]);
                case QueryFunctions.Upper: return string.Format("upper({0})", parameters[0]);

                case QueryFunctions.Abs: return string.Format("abs({0})", parameters[0]);
                case QueryFunctions.Acos: return string.Format("acos({0})", parameters[0]);
                case QueryFunctions.Asin: return string.Format("asin({0})", parameters[0]);
                case QueryFunctions.Atan: return string.Format("atan({0})", parameters[0]);
                case QueryFunctions.Atn2: return string.Format("atan2({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Ceiling: return string.Format("ceiling({0})", parameters[0]);
                case QueryFunctions.Cos: return string.Format("cos({0})", parameters[0]);
                case QueryFunctions.Cot: return string.Format("cos({0})/sin({0})", parameters[0]);
                case QueryFunctions.Degrees: return string.Format("degrees({0})", parameters[0]);
                case QueryFunctions.Exp: return string.Format("exp({0})", parameters[0]);
                case QueryFunctions.Floor: return string.Format("floor({0})", parameters[0]);
                case QueryFunctions.Log:
                    if (parameters.Length == 1) return string.Format("ln({0})", parameters[0]);
                    else if (parameters.Length == 2) return string.Format("log({1}, {0})", parameters[0], parameters[1]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.PI: return "pi()";
                case QueryFunctions.Power: return string.Format("power({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.Radians: return string.Format("radians({0})", parameters[0]);
                case QueryFunctions.Rand:
                    if (parameters.Length == 0) return " random()";
                    //else if (parameters.Length == 1) return string.Format("RAND({0})", parameters[0]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Round:
                    if (parameters.Length == 2) return string.Format("round({0}, {1})", parameters[0], parameters[1]);
                    //else if (parameters.Length == 3) return string.Format("ROUND({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                    else if (parameters.Length == 1) return string.Format("round({0})", parameters[0]);
                    else throw new QueryBuildException(QueryBuildException.ExceptionTypes.ParameterCountIsWrong);
                case QueryFunctions.Sign: return string.Format("sign({0})", parameters[0]);
                case QueryFunctions.Sin: return string.Format("sin({0})", parameters[0]);
                case QueryFunctions.Sqrt: return string.Format("sqrt({0})", parameters[0]);
                case QueryFunctions.Square: return string.Format("{0} * {0}", parameters[0]);
                case QueryFunctions.Tan: return string.Format("tan({0})", parameters[0]);

                case QueryFunctions.GetDate: return "now()";

                case QueryFunctions.Avg: return string.Format("avg({0})", parameters[0]);
                case QueryFunctions.Max: return string.Format("max({0})", parameters[0]);
                case QueryFunctions.Min: return string.Format("min({0})", parameters[0]);
                case QueryFunctions.Sum: return string.Format("sum({0})", parameters[0]);
                case QueryFunctions.Stdev: return string.Format("stddev({0})", parameters[0]);
                case QueryFunctions.Stdevp: return string.Format("stddev_pop({0})", parameters[0]);
                case QueryFunctions.Var: return string.Format("variance({0})", parameters[0]);
                case QueryFunctions.Varp: return string.Format("var_pop({0})", parameters[0]);
                case QueryFunctions.Count: return string.Format("count({0})", parameters[0]);
                case QueryFunctions.Count_Big: return string.Format("count({0})", parameters[0]);
                //case QueryFunctions.Grouping: return string.Format("GROUPING({0})", parameters[0]);
                //case QueryFunctions.Grouping_Id: return string.Format("GROUPING_ID({0})", parameters[0]);
                //case QueryFunctions.Checksum_Agg: return string.Format("CHECKSUM_AGG({0})", parameters[0]);

                case QueryFunctions.STArea: return string.Format("st_area({0}, 't')", parameters[0]);
                case QueryFunctions.STAsBinary: return string.Format("st_asbinary({0})", parameters[0]);
                case QueryFunctions.STAsText: return string.Format("st_astext({0})", parameters[0]);
                case QueryFunctions.STBoundary: return string.Format("st_boundary({0})", parameters[0]);
                case QueryFunctions.STBuffer: return string.Format("st_buffer({0}, {1})", parameters[0], parameters[1]);
                //case QueryFunctions.STCentroid: return string.Format("{0}.STCentroid()", parameters[0]);
                case QueryFunctions.STContains: return string.Format("st_contains({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STConvexHull: return string.Format("st_convexhull({0})", parameters[0]);
                case QueryFunctions.STCrosses: return string.Format("st_crosses({0}, {1})", parameters[0], parameters[1]);
                //case QueryFunctions.STCurveN: return string.Format("{0}.STCurveN({1})", parameters[0], parameters[1]);
                case QueryFunctions.STCurveToLine: return string.Format("st_curvetoline({0})", parameters[0]);
                case QueryFunctions.STDifference: return string.Format("st_difference({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STDimension: return string.Format("st_dimension({0})", parameters[0]);
                case QueryFunctions.STDisjoint: return string.Format("st_disjoint({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STDistance: return string.Format("st_distance({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STEndpoint: return string.Format("st_endpoint({0})", parameters[0]);
                case QueryFunctions.STEnvelope: return string.Format("st_envelope({0})", parameters[0]);
                case QueryFunctions.STEquals: return string.Format("st_equals({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STExteriorRing: return string.Format("st_exteriorring({0})", parameters[0]);
                case QueryFunctions.STGeometryN: return string.Format("st_geometryn({0}, {1}+1)", parameters[0], parameters[1]);
                case QueryFunctions.STGeometryType: return string.Format("st_geometrytype({0})", parameters[0]);
                case QueryFunctions.STInteriorRingN: return string.Format("st_interiorringn({0}, {1}+1)", parameters[0], parameters[1]);
                case QueryFunctions.STIntersection: return string.Format("st_intersection({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STIntersects: return string.Format("st_intersects({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STIsClosed: return string.Format("st_isclosed({0})", parameters[0]);
                case QueryFunctions.STIsEmpty: return string.Format("st_isempty({0})", parameters[0]);
                case QueryFunctions.STIsRing: return string.Format("st_isring({0})", parameters[0]);
                case QueryFunctions.STIsSimple: return string.Format("st_issimple({0})", parameters[0]);
                case QueryFunctions.STIsValid: return string.Format("st_isvalid({0})", parameters[0]);
                case QueryFunctions.STLength: return string.Format("st_length({0})", parameters[0]);
                //case QueryFunctions.STNumCurves: return string.Format("{0}.STNumCurves()", parameters[0]);
                case QueryFunctions.STNumGeometries: return string.Format("st_numgeometries({0})", parameters[0]);
                case QueryFunctions.STNumInteriorRing: return string.Format("st_numinteriorring({0})", parameters[0]);
                case QueryFunctions.STNumPoints: return string.Format("st_numpoints({0})", parameters[0]);
                case QueryFunctions.STOverlaps: return string.Format("st_overlaps({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STPointN: return string.Format("st_pointn({0}, {1}+1)", parameters[0], parameters[1]);
                //case QueryFunctions.STPointOnSurface: return string.Format("{0}.STPointOnSurface({1})", parameters[0], parameters[1]);
                case QueryFunctions.STRelate: return string.Format("st_relate({0}, {1}, {2})", parameters[0], parameters[1], parameters[2]);
                case QueryFunctions.STSrid: return string.Format("st_srid({0})", parameters[0]);
                case QueryFunctions.STStartPoint: return string.Format("st_startpoint({0})", parameters[0]);
                case QueryFunctions.STSymDifference: return string.Format("st_symdifference({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STTouches: return string.Format("st_touches({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STUnion: return string.Format("st_union({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STWithin: return string.Format("st_within({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STX: return string.Format("st_x({0})", parameters[0]);
                case QueryFunctions.STY: return string.Format("st_y({0})", parameters[0]);
                case QueryFunctions.STGeomFromText: return string.Format("st_geomfromtext({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STPointFromText: return string.Format("st_pointfromtext({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STLineFromText: return string.Format("st_linefromtext({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STPolyFromText: return string.Format("st_polyfromtext({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMPointFromText: return string.Format("st_mpointfromtext({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMLineFromText: return string.Format("st_mlinefromtext({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMPolyFromText: return string.Format("st_mpolyfromtext({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STGeomCollFromText: return string.Format("st_geomcollfromtext({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STGeomFromWKB: return string.Format("st_geomfromwkb({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STPointFromWKB: return string.Format("st_pointfromwkb({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STLineFromWKB: return string.Format("st_linefromwkb({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STPolyFromWKB: return string.Format("st_polyfromwkb({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMPointFromWKB: return string.Format("st_mpointfromwkb({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMLineFromWKB: return string.Format("st_mlinefromwkb({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STMPolyFromWKB: return string.Format("st_mpolyfromwkb({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.STGeomCollFromWKB: return string.Format("st_geomcollfromwkb({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.GeomFromGML: return string.Format("st_geomfromgml({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.AsBinaryZM: return string.Format("st_asbinary({0})", parameters[0]);
                case QueryFunctions.AsGml: return string.Format("st_asgml({0})", parameters[0]);
                case QueryFunctions.AsTextZM: return string.Format("st_astext({0})", parameters[0]);
                //case QueryFunctions.BufferWithCurves: return string.Format("{0}.BufferWithCurves({1})", parameters[0], parameters[1]);
                //case QueryFunctions.BufferWithTolerance: return string.Format("{0}.BufferWithTolerance({1}, {2}, {3})", parameters[0], parameters[1], parameters[2], parameters[3]);
                //case QueryFunctions.CurveToLineWithTolerance: return string.Format("{0}.CurveToLineWithTolerance({1}, {2})", parameters[0], parameters[1], parameters[2]);
                //case QueryFunctions.InstanceOf: return string.Format("{0}.InstanceOf({1})", parameters[0], parameters[1]);
                //case QueryFunctions.Filter: return string.Format("{0}.Filter({1})", parameters[0], parameters[1]);
                //case QueryFunctions.HasM: return string.Format("{0}.HasM()", parameters[0]);
                //case QueryFunctions.HasZ: return string.Format("{0}.HasZ()", parameters[0]);
                //case QueryFunctions.IsNull: return string.Format("{0}.IsNull()", parameters[0]);
                case QueryFunctions.IsValidDetailed: return string.Format("st_isvaliddetail({0})", parameters[0]);
                case QueryFunctions.M: return string.Format("st_m({0})", parameters[0]);
                case QueryFunctions.MakeValid: return string.Format("st_makevalid({0})", parameters[0]);
                //case QueryFunctions.MinDbCompatibilityLevel: return string.Format("{0}.MinDbCompatibilityLevel()", parameters[0]);
                //case QueryFunctions.Reduce: return string.Format("{0}.Reduce({1})", parameters[0], parameters[1]);
                case QueryFunctions.ShortestLineTo: return string.Format("st_shortestline({0}, {1})", parameters[0], parameters[1]);
                case QueryFunctions.ToString: return string.Format("st_astext({0})", parameters[0]);
                case QueryFunctions.Z: return string.Format("st_z({0})", parameters[0]);
                //case QueryFunctions.EnvelopeAngle: return string.Format("{0}.EnvelopeAngle()", parameters[0]);
                //case QueryFunctions.EnvelopeCenter: return string.Format("{0}.EnvelopeCenter()", parameters[0]);
                //case QueryFunctions.Lat: return string.Format("{0}.Lat", parameters[0]);
                //case QueryFunctions.Long: return string.Format("{0}.Long", parameters[0]);
                //case QueryFunctions.NumRing: return string.Format("{0}.NumRing()", parameters[0]);
                //case QueryFunctions.ReorientObject: return string.Format("{0}.ReorientObject({1})", parameters[0], parameters[1]);
                //case QueryFunctions.RingN: return string.Format("{0}.RingN({1})", parameters[0], parameters[1]);
                case QueryFunctions.CollectionAggregate: return string.Format("st_collect({0})", parameters[0]);
                //case QueryFunctions.ConvexHullAggregate: return string.Format("geography::ConvexHullAggregate({0})", parameters[0]);
                //case QueryFunctions.EnvelopeAggregate: return string.Format("geography::EnvelopeAggregate({0})", parameters[0]);
                case QueryFunctions.UnionAggregate: return string.Format("st_union({0})", parameters[0]);

                default: throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorNotFound);
            }
            throw new QueryBuildException(QueryBuildException.ExceptionTypes.OperatorUnsuported);
        }
        private string ProcessNEXP(NEXP exp)
        {
            var name = exp.Name;
            var expression = ProcessQueryItem(exp.Expression);
            return string.Format("{0} as {1}", expression, name);
        }
        private string ProcessCOL(COL col)
        {
            if (col.Name == "*") return col.Name;
            return string.Format("{0}", col.Name);
        }
        private string ProcessVAL(VAL val)
        {
            var parameterName = string.Format("@p{0}", _parameters.Count);
            _parameters.Add(new QueryParameter { Name = parameterName, Value = val.Value });
            return parameterName;
        }
        private string ProcessARR(ARR arr)
        {
            var values = arr.Values.Select(a => ProcessQueryItem(a)).ToArray();
            return string.Format("({0})", string.Join(",", values));
        }
        private string ProcessOrderItem(IQueryOrderItem item)
        {
            var value = ProcessQueryItem(item.Value);
            var result = string.Format("{0} {1}", value, item.Type == QueryOrderType.ASC ? "asc" : "desc");
            return result;
        }


        class SubQuery
        {
            public bool IsFunction { get; set; }
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
                    table = string.Format("({0}) as tview{1}", PreviousQuery.BuildQuery(), Level);
                else
                    table = string.Format("{0}{1} ", SchemaName != null ? string.Format("{0}.", SchemaName) : "", TableName);
                return Combine(table);
            }
            public string BuildQueryWithSpace()
            {
                string table;
                if (PreviousQuery != null)
                    table = string.Format("\r\n\t(\r\n{0}\r\n\t) as tview{1}\r\n",
                        string.Join("\r\n", PreviousQuery.BuildQueryWithSpace().Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(a => "\t" + a).ToArray()), Level);
                else
                    table = string.Format("{0}{1} \r\n", SchemaName != null ? string.Format("{0}.", SchemaName) : "", TableName);
                return CombineWithSpace(table);
            }

            private string Combine(string table)
            {
                var selectStatement = GetSelect();
                var whereStatement = !string.IsNullOrEmpty(Where) ? "where " + Where : "";
                var groupByStatement = !string.IsNullOrEmpty(GroupBy) ? "group by " + GroupBy : "";
                var orderByStatement = !string.IsNullOrEmpty(OrderBy) ? "order by " + OrderBy : "";
                var limitStatement = Take != null ? string.Format("limit {0} ", Take) : "";
                var offsetStatement = Skip != null ? string.Format("offset {0} ", Skip) : "";

                var result = string.Format("select {0} from {1} {2} {3} {4} {5} {6}",
                    selectStatement, table,
                    whereStatement, groupByStatement, orderByStatement, limitStatement, offsetStatement);
                return result;
            }
            private string CombineWithSpace(string table)
            {
                var selectStatement = GetSelect() + "\r\n";
                var whereStatement = !string.IsNullOrEmpty(Where) ? "where " + Where + "\r\n" : "";
                var groupByStatement = !string.IsNullOrEmpty(GroupBy) ? "group by " + GroupBy + "\r\n" : "";
                var orderByStatement = !string.IsNullOrEmpty(OrderBy) ? "order by " + OrderBy + "\r\n" : "";
                var limitStatement = Take != null ? string.Format("limit {0} ", Take) : "";
                var offsetStatement = Skip != null ? string.Format("offset {0} ", Skip) : "";

                var result = string.Format("select {0}from {1}{2}{3}{4}{5}{6}",
                    selectStatement, table,
                    whereStatement, groupByStatement, orderByStatement, limitStatement, offsetStatement);
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

            tableName = schemaName == null ? tableName : string.Format("{0}.{1}", schemaName, tableName);
            var parameterName = "";
            var parameterValue = "";
            var param = new List<QueryParameter>();
            foreach (KeyValuePair<string, object> p in parameter)
            {
                if (exceptCols.Contains(p.Key)) continue;
                parameterName += "," + p.Key;
                parameterValue += ",@" + p.Key;
                param.Add(new QueryParameter { Name = "@" + p.Key, Value = _typeMapper.ConvertToSql(p.Value) });
            }
            parameterName = parameterName.Substring(1);
            parameterValue = parameterValue.Substring(1);

            string sql = null;
            if (parameterName.Length > 2)
                sql = string.Format("insert into {0} ({1}) values ({2})", tableName, parameterName, parameterValue);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }
        public Query GetUpdateQuery(string tableName, string schemaName, Dictionary<string, object> parameter, string[] idCols, string[] exceptCols = null, bool setNull = false)
        {
            tableName = schemaName == null ? tableName : string.Format("{0}.{1}", schemaName, tableName);
            var parametrename = "";
            var param = new List<QueryParameter>();
            foreach (KeyValuePair<string, object> p in parameter)
            {
                if (exceptCols.Contains(p.Key)) continue;
                parametrename += "," + p.Key + "=@" + p.Key;
                param.Add(new QueryParameter { Name = "@" + p.Key, Value = _typeMapper.ConvertToSql(p.Value) });
            }
            if (parametrename.Length > 1)
                parametrename = parametrename.Substring(1);

            var whereStatement = "";
            for (var i = 0; i < idCols.Length; i++)
            {
                var id = idCols[i];
                whereStatement += " AND " + id + "=@p" + i;
                param.Add(new QueryParameter { Name = "@p" + i, Value = _typeMapper.ConvertToSql(parameter[id]) });
            }
            if (whereStatement.Length > 5)
                whereStatement = whereStatement.Substring(5);
            whereStatement = whereStatement.Length > 0 ? " WHERE " + whereStatement : "";

            string sql = null;
            if (parametrename.Length > 2)
                sql = string.Format("update {0} set {1} {2}", tableName, parametrename, whereStatement);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }
        public Query GetDeleteQuery(string tableName, string schemaName, Dictionary<string, object> parameter, string[] idCols)
        {
            tableName = schemaName == null ? tableName : string.Format("{0}.{1}", schemaName, tableName);
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

            var sql = string.Format("delete from {0} where {1}", tableName, whereStatement);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }

        public Query GetInsertQuery<T>(string tableName, string schemaName, T parameter, Expression<Func<T, object>> exceptCols)
        {
            tableName = schemaName == null ? tableName : string.Format("{0}.{1}", schemaName, tableName);
            IEnumerable<string> except;
            if (exceptCols != null)
                except = ExpressionHelper.GetPropertyNames<T, object>(exceptCols);
            else except = new string[0];

            var parameterName = "";
            var parameterValue = "";
            var param = new List<QueryParameter>();
            foreach (var p in typeof(T).GetProperties().Where(p => p.GetValue(parameter, null) != null))
            {
                if (except.Contains(p.Name)) continue;
                parameterName += "," + p.Name;
                parameterValue += ",@" + p.Name;
                param.Add(new QueryParameter { Name = "@" + p.Name, Value = _typeMapper.ConvertToSql(p.GetValue(parameter)) });
            }
            parameterName = parameterName.Substring(1);
            parameterValue = parameterValue.Substring(1);

            string sql = null;
            if (parameterName.Length > 2)
                sql = string.Format("insert into {0} ({1}) VALUES ({2})", tableName, parameterName, parameterValue);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }
        public Query GetUpdateQuery<T>(string tableName, string schemaName, T parameter, Expression<Func<T, object>> idCols, Expression<Func<T, object>> exceptCols = null, bool setNull = false)
        {
            tableName = schemaName == null ? tableName : string.Format("{0}.{1}", schemaName, tableName);
            IEnumerable<string> except;
            if (exceptCols != null)
                except = ExpressionHelper.GetPropertyNames<T, object>(exceptCols);
            else except = new string[0];

            var parametrename = "";
            var param = new List<QueryParameter>();
            foreach (var p in typeof(T).GetProperties())
            {
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
                whereStatement += " and " + id.Name + "=@p" + k;
                param.Add(new QueryParameter { Name = "@p" + k, Value = _typeMapper.ConvertToSql(id.GetValue(parameter, null)) });
                k++;
            }
            if (whereStatement.Length > 5)
                whereStatement = whereStatement.Substring(5);
            whereStatement = whereStatement.Length > 0 ? " WHERE " + whereStatement : "";

            string sql = null;
            if (parametrename.Length > 2)
                sql = string.Format("update {0} set {1} {2}", tableName, parametrename, whereStatement);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }
        public Query GetDeleteQuery<T>(string tableName, string schemaName, T parameter, Expression<Func<T, object>> idCols)
        {
            tableName = schemaName == null ? tableName : string.Format("{0}.{1}", schemaName, tableName);
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

            var sql = string.Format("delete from {0} where {1}", tableName, whereStatement);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }

        public Query GetDeleteQuery(string tableName, string schemaName, BEXP condition)
        {
            tableName = schemaName == null ? tableName : string.Format("{0}.{1}", schemaName, tableName);
            if (condition == null) throw new Exception("Delete işleminde koşul null bırakılamaz.");
            _parameters.Clear();
            var whereStatement = ProcessWhere(condition as BEXP);
            var sql = string.Format("delete from {0} where {1}", tableName, whereStatement);
            return new Query { Command = sql, Parameters = _parameters.ToArray() };
        }

        public Query GetInsertQuery(string tableName, string schemaName, IFeature feature, string geometryColumnName)
        {
            tableName = schemaName == null ? tableName : string.Format("{0}.{1}", schemaName, tableName);
            //if (exceptCols == null)
            //    exceptCols = new string[0];

            var parameterName = "";
            var parameterValue = "";
            var param = new List<QueryParameter>();
            foreach (var name in feature.Attributes.GetNames())
            {
                //if (exceptCols.Contains(p.Key)) continue;
                parameterName += "," + name;
                parameterValue += ",@" + name;
                param.Add(new QueryParameter { Name = "@" + name, Value = _typeMapper.ConvertToSql(feature.Attributes[name]) });
            }
            parameterName += "," + geometryColumnName + "";
            parameterValue += ",@" + geometryColumnName;
            param.Add(new QueryParameter { Name = "@" + geometryColumnName, Value = _typeMapper.ConvertToSql(feature.Geometry) });

            parameterName = parameterName.Substring(1);
            parameterValue = parameterValue.Substring(1);

            string sql = null;
            if (parameterName.Length > 2)
                sql = string.Format("insert into {0} ({1}) values ({2})", tableName, parameterName, parameterValue);
            return new Query { Command = sql, Parameters = param.ToArray() };
        }

        public Query GetDropTableQuery(string tableName, string schemaName)
        {
            tableName = schemaName == null ? tableName : string.Format("{0}.{1}", schemaName, tableName);
            return new Query { Command = string.Format("drop table {0}", tableName) };
        }

        public Query GetTableExistsQuery(string tableName, string schemaName)
        {
            var query = ConvertToQuery("select cast(count(*) > 0 as boolean) from information_schema.tables where table_name = {0} and (table_schema = {1} or {1} is null)", tableName.ToLower(), schemaName != null ? schemaName.ToLower() : schemaName);
            return query;
        }

        public Query GetUpdateQuery(string tableName, string schemaName, IFeature feature, string geometryColumnName, string[] idCols, bool setNull = false)
        {
            // TODO: Postgis. IFeature update. Kontrol et. [FM].
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

        public Query GetUpdateQueryBulkUpdate<T>(string tableName, string schemaName, List<T> parameter, Expression<Func<T, object>> idCols, Expression<Func<T, object>> exceptCols = null, bool setNull = false)
        {
            throw new NotImplementedException();
        }
        public Query GetInsertQueryBulkInsert<T>(string tableName, string schemaName, List<T> parameter, bool setNull = false)
        {
            throw new NotImplementedException();
        }
    }
}
