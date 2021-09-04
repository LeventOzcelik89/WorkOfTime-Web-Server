using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Infoline.Framework.Database;
using GeoAPI.Geometries;
using NetTopologySuite.IO;
using System.Linq.Expressions;
using System.Linq;
using NetTopologySuite.Features;
using System.Data.Common;

namespace Infoline.Framework.Test
{
    [TestClass]
    public class T1_CreateTables
    {
        //[TestMethod]
        //public void T1_F1_CreateTables()
        //{
        //    var connection = ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString;

        //    var database = new Database.Database(connection, DatabaseType.Mssql);

        //    var createResult = 
        //        database.CreateTable<UT_Town>()
        //                .SetDefaultValue(a => a.id, SqlFunctions.NEWID)
        //                .SetDefaultValue(a => a.created, SqlFunctions.GETDATE)
        //                .SetDefaultValue(a => a.changed, SqlFunctions.GETDATE)
        //                .SetPrimaryKey(a => a.id)
        //                .Execute();

        //    var list = new string[] { "istanbul", "izmir" };
        //    var list2 = new int?[] { 10, 11 };

        //    var text1 = database.Table<UT_Town>()
        //                            .Where(a => a.CityName.In(list) | a.CityNumber.In(list2))
        //                            .GetQueryForTest();

        //    //var result = database.Table<UT_Town>().Delete(a => a.CityName == "Test");

        //    var text2 = database.Table("UT_Town")
        //                           .Where(((VAL)"YENISEHIR").IN((COL)"Name", (COL)"CityName"))
        //                           .ExecuteFeature();

        //    var text = new NetTopologySuite.IO.GeoJsonWriter().Write(text2);
        //    var text3 = database.Table("UT_Town").Where( ((COL)"CityName").IN(list.Select(a => new VAL(a))) ).ExecuteFeature();


            

        //    var town = new UT_Town
        //    {
        //        id = Guid.NewGuid(),
        //        BolgeNumber = 1001,
        //        CityName = "Test",
        //        CityNumber = 1001,
        //        Name = "Test",
        //        Polygon = new WKTReader().Read("POINT(10 10)"),
        //        PolygonToString = "POINT(10 10)",
        //        TownNumber = 1001,
        //    };
        //    var transaction = database.BeginTransaction();

        //    var insertResult = database.Table<UT_Town>().Insert(new[] { town });

        //    if (insertResult.result) transaction.Commit();
        //    else transaction.Rollback();
        //}

        [TestMethod]
        public void T1_F1s_CreateTables()
        {

            int? type = null;
            Guid? CityId = null;
            Guid? TownId = null;

            var ConnectionString = "Data Source=10.100.0.222;Initial Catalog=AssetManagementSystemPYS;User ID=sa;Password=c2SSsCM9Y3HZVU";
            using (var db = new InfolineDatabase(ConnectionString, DatabaseType.Mssql, null))
            {
                type = (type == null ? 0 : type);
                var result = db.Table<VWMEB_TumListe>()
                                .Where(a => (a.Tip == type) && (CityId == null | a.CityId == CityId) && (TownId == null | a.TownId == TownId))
                                .OrderByDesc(a => a.created);


                var c = result.GetQueryForTest();

                var d = result.Execute().ToArray();
            }
        }

        public class InfolineDatabase : IDisposable
        {
            string _connectionString;
            Database.Database _database;

            public InfolineDatabase(string connectionString, DatabaseType dbType, DbTransaction tran = null)
            {
                _connectionString = connectionString;
                _database = new Database.Database(connectionString, dbType, tran);
            }


            public ResultStatus ExecuteNonQuery(string txt, params object[] parameters)
            {
                return _database.ExecuteNonQuery(txt, parameters);
            }
            public T ExecuteScaler<T>(string txt, params object[] parameters)
            {
                return _database.ExecuteScaler<T>(txt, parameters);
            }
            public IEnumerable<Dictionary<string, object>> ExecuteReader(string txt, params object[] parameters)
            {
                return _database.ExecuteReader(txt, parameters);
            }
            public IEnumerable<T> ExecuteReader<T>(string txt, params object[] parameters) where T : new()
            {
                return _database.ExecuteReader<T>(txt, parameters);
            }

            public void Dispose()
            {
                _database.Dispose();
            }


            public IGetTable Table(string tableName, string schemaName = "infoline")
            {
                return _database.Table(tableName, schemaName);
            }
            public IGetTable TableFunction(string schemaName, string tableName, params object[] parameters)
            {
                return _database.TableFunction(schemaName, tableName, parameters);
            }
            public IGetTable<T> Table<T>(string schemaName = "infoline", string tableName = null) where T : new()
            {
                return _database.Table<T>(schemaName, tableName);
            }
            public IGetTable<T> TableFunction<T>(string schemaName, string tableName, params object[] parameters) where T : new()
            {
                return _database.TableFunction<T>(schemaName, tableName, parameters);
            }
            public ResultStatus CreateTable(TableInfo tableInfo)
            {
                return _database.CreateTable(tableInfo);
            }
            public ITableCreate CreateTable(string tableName, string schemaName = "infoline")
            {
                return _database.CreateTable(tableName, schemaName);
            }
            public ITableCreate<T> CreateTable<T>(string tableName = null, string schemaName = "infoline") where T : InfolineTable
            {
                return _database.CreateTable<T>(tableName, schemaName);
            }

            public ResultStatus AlterTable(TableInfo tableInfo)
            {
                return _database.AlterTable(tableInfo);
            }
            public ITableCreate AlterTable(string tableName, string schemaName = "infoline")
            {
                return _database.AlterTable(tableName, schemaName);
            }
            public ITableCreate<T> AlterTable<T>(string tableName = null, string schemaName = "infoline") where T : InfolineTable
            {
                return _database.AlterTable<T>(tableName, schemaName);
            }

            public TableInfo TableInfo(string tableName, bool onlyColumns = false)
            {
                return _database.TableInfo(tableName, onlyColumns);
            }
            public ResultStatus DropTable(string tableName, string schemaName = "infoline")
            {
                return _database.DropTable(tableName, schemaName);
            }
            public bool TableExists(string tableName, string schemaName = "infoline")
            {
                return _database.TableExists(tableName, schemaName);
            }



            public ResultStatus ExecuteInsert<T>(T parameter, string tableName = null) where T : InfolineTable, new()
            {
                return _database.Table<T>("infoline", tableName).Insert(parameter);
            }
            public ResultStatus ExecuteUpdate<T>(T parameter, bool setNull = false, string tableName = null) where T : InfolineTable, new()
            {
                return _database.Table<T>("infoline", tableName).Update(parameter, a => a.id, a => new { a.created, a.id }, setNull);
            }
            public ResultStatus ExecuteDelete<T>(T parameter, string tableName = null) where T : InfolineTable, new()
            {
                return _database.Table<T>("infoline", tableName).Delete(parameter, a => a.id);
            }
            public ResultStatus ExecuteDelete<T>(Guid id, string tableName = null) where T : InfolineTable, new()
            {
                return _database.Table<T>("infoline", tableName).Delete(new T { id = id }, a => a.id);
            }
            public ResultStatus ExecuteBulkInsert<T>(IEnumerable<T> parameter, string tableName = null) where T : InfolineTable, new()
            {
                return _database.Table<T>("infoline", tableName).Insert(parameter);
            }
            public ResultStatus ExecuteBulkUpdate<T>(IEnumerable<T> parametre, bool setNull = false, string tableName = null) where T : InfolineTable, new()
            {
                return _database.Table<T>("infoline", tableName).Update(parametre, a => a.id, a => new { a.created, a.id }, setNull);
            }
            public ResultStatus ExecuteBulkDelete<T>(IEnumerable<T> parametre, string tableName = null) where T : InfolineTable, new()
            {
                return _database.Table<T>("ifoline", tableName).Delete(parametre, a => a.id);
            }
            public ResultStatus ExecuteBulkDelete<T>(IEnumerable<Guid> parametre, string tableName = null) where T : InfolineTable, new()
            {
                return _database.Table<T>("infoline", tableName).Delete(parametre.Select(a => new T { id = a }), a => a.id);
            }

            public bool IsTransactionOpen { get { return _database.IsTransactionOpen; } }
            public DbTransaction BeginTransection()
            {
                return _database.BeginTransaction();
            }
            public ResultStatus CommitTransection()
            {
                return _database.Commit();
            }
            public ResultStatus RollBackTransection()
            {
                return _database.RollBack();
            }


            public ResultStatus CreateFunction<T1, T2, T3, TResult>(string name, Func<T1, T2, T3, IQuery> function)
            {
                throw new NotImplementedException();
            }


        }
        public partial class UT_Town
        {
            public UT_Town()
            {
                id = Guid.NewGuid();
            }
            public Guid id { get; set; }
            public DateTime? created { get; set; }
            public DateTime? changed { get; set; }
            public Guid? changedby { get; set; }
            public Guid? createdby { get; set; }
            public int TownNumber { get; set; }
            public int? BolgeNumber { get; set; }
            public int? CityNumber { get; set; }
            public string CityName { get; set; }
            public string Name { get; set; }
            public IGeometry Polygon { get; set; }
            public string PolygonToString { get; set; }
        }
        public class InfolineTable
        {
            public InfolineTable()
            {
                id = Guid.NewGuid();
            }
            public Guid id { get; set; }
            public DateTime? created { get; set; }
            public DateTime? changed { get; set; }
            public Guid? changedby { get; set; }
            public Guid? createdby { get; set; }
        }
        public partial class VWMEB_TumListe : InfolineTable
        {
            public IGeometry Location { get; set; }
            public string Name { get; set; }
            public string Link { get; set; }
            public Guid? CityId { get; set; }
            public Guid? TownId { get; set; }
            public int? Tablet { get; set; }
            public int? Ogretmen { get; set; }
            public int? Ogrenci { get; set; }
            public int? Tahta { get; set; }
            public int? Sunucu { get; set; }
            public int? Network { get; set; }
            public int? SIM { get; set; }
            public int? Fiber { get; set; }
            public int? Tip { get; set; }
        }
    }
}
