using NetTopologySuite.Features;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Infoline.Framework.Database
{
    public interface IQueryBuilder
    {
        Query ConvertToQuery(string txt, params object[] parameters);

        Query GetFetchQuery(bool isFunction, string tableName, string schemaName, object[] functionParameters, List<QueryStatement> statements, List<object> parameters);

        string GetFetchQueryForTest(bool isFunction, string tableName, string schemaName, object[] functionParameters, List<QueryStatement> statements, List<object> parameters);



        Query GetInsertQuery(string tableName, string schemaName, Dictionary<string, object> parameter, string[] exceptCols);

        Query GetUpdateQuery(string tableName, string schemaName, Dictionary<string, object> parameter, string[] idCols, string[] exceptCols = null, bool setNull = false);

        Query GetDeleteQuery(string tableName, string schemaName, Dictionary<string, object> parameter, string[] idCols);

        Query GetDeleteQuery(string tableName, string schemaName, BEXP condition);



        Query GetInsertQuery<T>(string tableName, string schemaName, T parameter, Expression<Func<T, object>> exceptCols);

        Query GetUpdateQuery<T>(string tableName, string schemaName, T parameter, Expression<Func<T, object>> idCols, Expression<Func<T, object>> exceptCols = null, bool setNull = false);

        Query GetUpdateQueryBulkUpdate<T>(string tableName, string schemaName, List<T> parameter, Expression<Func<T, object>> idCols, Expression<Func<T, object>> exceptCols = null, bool setNull = false);

        Query GetInsertQueryBulkInsert<T>(string tableName, string schemaName, List<T> parameter, bool setNull = false);
        Query GetDeleteQuery<T>(string tableName, string schemaName, T parameter, Expression<Func<T, object>> idCols);

        Query GetInsertQuery(string tableName, string schemaName, IFeature feature, string geometryColumnName);

        Query GetUpdateQuery(string tableName, string schemaName, IFeature feature, string geometryColumnName, string[] idCols, bool setNull = false);

        Query GetDropTableQuery(string tableName, string schemaName);

        Query GetTableExistsQuery(string tableName, string schemaName);
    }


}
