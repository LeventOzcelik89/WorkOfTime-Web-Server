using GeoAPI.Geometries;
using Npgsql;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Linq;
using NetTopologySuite.Features;

namespace Infoline.Framework.Database
{
    public interface IQueryExecutor : IDisposable
    {

        string ConnectionString { get; }
        bool IsTransactionOpen { get; }
        bool IsSupportBulkInsert { get; }

        string ServerName { get; }
        string DbName { get; }


        T ExecuteScaler<T>(Query query);

        FeatureCollection ExecuteFeature(Query query);

        IEnumerable<T> ExecuteReader<T>(Query query);

        IEnumerable<Dictionary<string, object>> ExecuteReader(Query query);


        ResultStatus ExecuteNonQuery(Query query);

        ResultStatus ExecuteBulkInsert(string tableName, string schemaName, FeatureCollection parametre, TableInfo tableInfo,string geomColName);

        DbTransaction BeginTransaction();
        
        void Close();
    }
}
