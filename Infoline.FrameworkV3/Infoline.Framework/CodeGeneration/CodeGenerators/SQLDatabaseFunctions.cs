using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.CodeGeneration.CodeGenerators
{
    public class SQLDatabaseFunctions
    {
        public Dictionary<string, string> GenerateMultiFile(string strConn)
        {
            var result = new Dictionary<string, string>();
            using (var con = new SqlConnection(strConn))
            {
                con.Open();
                string[] objArrRestrict;
                DataTable schemaTbl;
                schemaTbl = con.GetSchema("Tables", null);
                var classname = con.Database;
                foreach (DataRow row in schemaTbl.Rows)
                {
                    StringBuilder sb = new StringBuilder();
                    bool hasname = false;
                    var tablename = (string)row["TABLE_NAME"];
                    var tabletype = (string)row["TABLE_TYPE"]; // "BASE TABLE", "VIEW"

                    if (tablename.IndexOf("ZOUT_") > -1) continue;

                    objArrRestrict = new string[] { null, null, tablename, null };
                    var tbl = con.GetSchema("Columns", objArrRestrict);
                    foreach (DataRow colrow in tbl.Rows)
                        hasname |= (string)colrow["column_name"] == "adi";

                    sb.AppendFormat("    partial class {0}Database", classname);
                    sb.AppendLine();
                    sb.AppendLine("    {");

                    sb.AppendLine("        /// <summary>");
                    sb.AppendFormat("        /// {0} tablosundan tüm kayıtları çeken fonksiyondur", tablename); sb.AppendLine();
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine("        /// <param name=\"tran\">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>");
                    sb.AppendFormat("        /// <returns>{0} dizi objesini geri döndürür.</returns>", tablename); sb.AppendLine();
                    sb.AppendFormat("        public {0}[] Get{0}(DbTransaction tran = null)", tablename); sb.AppendLine();
                    sb.AppendLine("        {");
                    sb.AppendLine("            using (var db = GetDB(tran))");
                    sb.AppendLine("            {");
                    sb.AppendFormat("                return db.Table<{0}>().OrderByDesc(a => a.created).Execute().ToArray();", tablename); sb.AppendLine();
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");
                    sb.AppendLine();

                    sb.AppendLine("        /// <summary>");
                    sb.AppendFormat("        /// {0} tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.", tablename); sb.AppendLine();
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine("        /// <param name=\"simpleQuery\">Filtre parametreleri olarak obje doldurularak gönderilir.</param>");
                    sb.AppendLine("        /// <param name=\"tran\">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>");
                    sb.AppendFormat("         /// <returns>Filtre Sonucu {0} dizi objesini geri döndürür.</returns>", tablename); sb.AppendLine();
                    sb.AppendFormat("        public {0}[] Get{0}(SimpleQuery simpleQuery, DbTransaction tran = null)\r\n", tablename);
                    sb.AppendLine("        {");
                    sb.AppendLine("            using (var db = GetDB(tran))");
                    sb.AppendLine("            {\r\n");
                    sb.AppendFormat("                return db.Table<{0}>().ExecuteSimpleQuery(simpleQuery).ToArray();\r\n", tablename);
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");

                    sb.AppendLine("        /// <summary>");
                    sb.AppendFormat("        ///  {0} tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.", tablename); sb.AppendLine();
                    sb.AppendLine("        /// </summary>");
                    sb.AppendLine("        /// <param name=\"conditionExpression\">Filtre parametreleri olarak obje doldurularak gönderilir.</param>");
                    sb.AppendFormat("        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>", tablename); sb.AppendLine();
                    sb.AppendFormat("        public int Get{0}Count(BEXP conditionExpression)\r\n", tablename);
                    sb.AppendLine("        {");
                    sb.AppendLine("            using (var db = GetDB())");
                    sb.AppendLine("            {");
                    sb.AppendFormat("                return db.Table(\"{0}\").Where(conditionExpression).Count();\r\n", tablename);
                    sb.AppendLine("            }");
                    sb.AppendLine("        }");

                    var hasIdColumn = tbl.Rows.Cast<DataRow>().Where(a => a.ItemArray.Contains("id")).Count() > 0;
                    if (tabletype == "BASE TABLE" || (tabletype == "VIEW" && hasIdColumn))
                    {
                        sb.AppendLine("        /// <summary>");
                        sb.AppendFormat("        /// {0} tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.", tablename); sb.AppendLine();
                        sb.AppendLine("        /// </summary>");
                        sb.AppendFormat("        /// <param name=\"id\">{0} tablo id'si verilir.</param>", tablename); sb.AppendLine();
                        sb.AppendLine("        /// <param name=\"tran\">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>");
                        sb.AppendFormat("        /// <returns>Filtre Sonucu {0} Objesini geri döndürür.</returns>", tablename); sb.AppendLine();
                        sb.AppendFormat("        public {0} Get{0}ById(Guid id, DbTransaction tran = null)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendLine("            using (var db = GetDB(tran))"); sb.AppendLine();
                        sb.AppendLine("            {");
                        sb.AppendFormat("                return db.Table<{0}>().Where(a => a.id == id).Execute().FirstOrDefault();", tablename); sb.AppendLine();
                        sb.AppendLine("            }");
                        sb.AppendLine("        }");
                        sb.AppendLine();
                    }

                    if (tabletype == "BASE TABLE")
                    {
                        #region BASE TABLE
                        sb.AppendLine("        /// <summary>");
                        sb.AppendFormat("        /// {0} Tablosuna Kayıt Atan Fonksiyondur.", tablename); sb.AppendLine();
                        sb.AppendLine("        /// </summary>");
                        sb.AppendFormat("        /// <param name=\"item\">{0} Objesidir. Insert Yapılacak Propertiler Eklenir</param>", tablename); sb.AppendLine();
                        sb.AppendLine("        /// <param name=\"tran\">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>");
                        sb.AppendLine("        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>");
                        sb.AppendFormat("        public ResultStatus Insert{0}({0} item, DbTransaction tran = null)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendLine("            using (var db = GetDB(tran))");
                        sb.AppendLine("            {");
                        sb.AppendFormat("                return db.ExecuteInsert<{0}>(item);", tablename); sb.AppendLine();
                        sb.AppendLine("            }");
                        sb.AppendLine("        }");
                        sb.AppendLine();

                        sb.AppendLine("        /// <summary>");
                        sb.AppendFormat("        /// {0} Tablosuna Günceleme Yapan Fonksiyondur.", tablename); sb.AppendLine();
                        sb.AppendLine("        /// </summary>");
                        sb.AppendFormat("        /// <param name=\"item\">{0} Objesidir. :Update Yapılacak Propertiler Eklenir</param>", tablename); sb.AppendLine();
                        sb.AppendLine("        /// <param name=\"setNull\">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>");
                        sb.AppendLine("        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>");
                        sb.AppendFormat("        public ResultStatus Update{0}({0} item, bool setNull = false, DbTransaction tran = null)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendLine("            using (var db = GetDB(tran))");
                        sb.AppendLine("            {");
                        sb.AppendFormat("                return db.ExecuteUpdate<{0}>(item, setNull);", tablename); sb.AppendLine();
                        sb.AppendLine("            }");
                        sb.AppendLine("        }");
                        sb.AppendLine();


                        sb.AppendLine("        /// <summary>");
                        sb.AppendFormat("        /// {0} tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.", tablename); sb.AppendLine();
                        sb.AppendLine("        /// </summary>");
                        sb.AppendFormat("        /// <param name=\"id\">{0} Tablo id'si</param>", tablename); sb.AppendLine();
                        sb.AppendLine("        /// <param name=\"tran\">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>");
                        sb.AppendLine("        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>");
                        sb.AppendFormat("        public ResultStatus Delete{0}(Guid id, DbTransaction tran = null)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendLine("            using (var db = GetDB(tran))");
                        sb.AppendLine("            {");
                        sb.AppendFormat("                return db.ExecuteDelete<{0}>(id);", tablename); sb.AppendLine();
                        sb.AppendLine("            }");
                        sb.AppendLine("        }");
                        sb.AppendLine();

                        sb.AppendLine("        /// <summary>");
                        sb.AppendFormat("        /// {0} tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.", tablename); sb.AppendLine();
                        sb.AppendLine("        /// </summary>");
                        sb.AppendFormat("        /// <param name=\"item\">{0} Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>", tablename); sb.AppendLine();
                        sb.AppendLine("        /// <param name=\"tran\">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>");
                        sb.AppendLine("        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>");
                        sb.AppendFormat("        public ResultStatus Delete{0}({0} item, DbTransaction tran = null)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendLine("            using (var db = GetDB(tran))");
                        sb.AppendLine("            {");
                        sb.AppendFormat("                return db.ExecuteDelete<{0}>(item);", tablename); sb.AppendLine();
                        sb.AppendLine("            }");
                        sb.AppendLine("        }");
                        sb.AppendLine();

                        sb.AppendLine("        /// <summary>");
                        sb.AppendFormat("        /// {0} Tablosuna Toplu insert işlemi yapan fonksiyondur.", tablename); sb.AppendLine();
                        sb.AppendLine("        /// </summary>");
                        sb.AppendFormat("        /// <param name=\"item\">{0} Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>", tablename); sb.AppendLine();
                        sb.AppendLine("        /// <param name=\"tran\">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>");
                        sb.AppendLine("        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>");
                        sb.AppendFormat("        public ResultStatus BulkInsert{0}(IEnumerable<{0}> item, DbTransaction tran = null)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendLine("            using (var db = GetDB(tran))");
                        sb.AppendLine("            {");
                        sb.AppendFormat("                return db.ExecuteBulkInsert<{0}>(item);", tablename); sb.AppendLine();
                        sb.AppendLine("            }");
                        sb.AppendLine("        }");
                        sb.AppendLine();

                        sb.AppendLine("        /// <summary>");
                        sb.AppendFormat("        /// {0} Tablosuna Toplu Update işlemi yapan fonksiyondur.", tablename); sb.AppendLine();
                        sb.AppendLine("        /// </summary>");
                        sb.AppendFormat("        /// <param name=\"item\">{0} Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>", tablename); sb.AppendLine();
                        sb.AppendLine("        /// <param name=\"tran\">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>");
                        sb.AppendLine("        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>");
                        sb.AppendFormat("        public ResultStatus BulkUpdate{0}(IEnumerable<{0}> item, bool setNull = false, DbTransaction tran = null)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendLine("            using (var db = GetDB(tran))");
                        sb.AppendLine("            {");
                        sb.AppendFormat("                return db.ExecuteBulkUpdate<{0}>(item, setNull);", tablename); sb.AppendLine();
                        sb.AppendLine("            }");
                        sb.AppendLine("        }");
                        sb.AppendLine();

                        sb.AppendLine("        /// <summary>");
                        sb.AppendFormat("        /// {0} tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.", tablename); sb.AppendLine();
                        sb.AppendLine("        /// </summary>");
                        sb.AppendFormat("        /// <param name=\"item\">{0} Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>", tablename); sb.AppendLine();
                        sb.AppendLine("        /// <param name=\"tran\">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>");
                        sb.AppendLine("        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>");
                        sb.AppendFormat("        public ResultStatus BulkDelete{0}(IEnumerable<{0}> item, DbTransaction tran = null)", tablename); sb.AppendLine();
                        sb.AppendLine("        {");
                        sb.AppendLine("            using (var db = GetDB(tran))");
                        sb.AppendLine("            {");
                        sb.AppendFormat("                return db.ExecuteBulkDelete<{0}>(item);", tablename); sb.AppendLine();
                        sb.AppendLine("            }");
                        sb.AppendLine("        }");
                        sb.AppendLine();
                        #endregion
                    }
                    sb.AppendLine("    }");

                    result.Add(tablename, sb.ToString());
                }
                result.Add(classname, GetDefaultClass(classname, strConn));
                result.Add("InfolineDatabase", GetDatabaseWrapper());
            }
            return result;
        }

        public string GetDefaultClass(string dbname, string strConn)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("   public partial class {0}Database", dbname); sb.AppendLine();
            sb.AppendLine("   {");
            sb.AppendLine("       public string ConnectionString { get; private set; }");
            sb.AppendLine("       public DatabaseType DatabaseType { get; private set; }");
            sb.AppendLine("       ");
            sb.AppendFormat("       public {0}Database()", dbname);
            sb.AppendLine();
            sb.AppendLine("       {");


            sb.AppendLine("if (System.Configuration.ConfigurationManager.ConnectionStrings[\"DatabaseConnection\"] != null)");
            sb.AppendLine("    this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[\"DatabaseConnection\"].ConnectionString;");
            sb.AppendLine();

            sb.AppendLine("#if DEBUG");
            sb.AppendFormat("           ConnectionString =  \"{0}\";", strConn);
            sb.AppendLine();
            sb.AppendLine("#endif");

            sb.AppendLine();
            sb.AppendLine("       }");
            sb.AppendLine("       ");
            sb.AppendFormat("       public {0}Database(string conn)", dbname); sb.AppendLine();
            sb.AppendLine("       {");
            sb.AppendLine("           this.ConnectionString = conn;");
            sb.AppendLine("       }");
            sb.AppendLine();
            sb.AppendLine("       public InfolineDatabase GetDB(DbTransaction tran = null)");
            sb.AppendLine("       {");
            sb.AppendLine("           return new InfolineDatabase(ConnectionString, DatabaseType, tran);");
            sb.AppendLine("       }");
            sb.AppendLine();
            sb.AppendLine("       public DbTransaction BeginTransaction()");
            sb.AppendLine("       {");
            sb.AppendLine("           return new InfolineDatabase(ConnectionString, DatabaseType).BeginTransection();");
            sb.AppendLine("       }");
            sb.AppendLine("   }");

            return sb.ToString();

        }

        public string GetDatabaseWrapper()
        {
            var str = @"

public class InfolineDatabase : IDisposable
    {
        string _connectionString;
        Database _database;

        public InfolineDatabase(string connectionString, DatabaseType dbType, DbTransaction tran = null)
        {
            _connectionString = connectionString;
            _database = new Database(connectionString, dbType, tran);
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


        public IGetTable Table(string tableName, string schemaName = ""infoline"")
        {
            return _database.Table(tableName, schemaName);
        }
        public IGetTable TableFunction(string schemaName, string tableName, params object[] parameters)
        {
            return _database.TableFunction(schemaName, tableName, parameters);
        }
        public IGetTable<T> Table<T>(string schemaName = ""infoline"", string tableName = null) where T : new()
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
        public ITableCreate CreateTable(string tableName, string schemaName = ""infoline"")
        {
            return _database.CreateTable(tableName, schemaName);
        }
        public ITableCreate<T> CreateTable<T>(string tableName = null, string schemaName = ""infoline"") where T : InfolineTable
        {
            return _database.CreateTable<T>(tableName, schemaName);
        }

        public ResultStatus AlterTable(TableInfo tableInfo)
        {
            return _database.AlterTable(tableInfo);
        }
        public ITableCreate AlterTable(string tableName, string schemaName = ""infoline"")
        {
            return _database.AlterTable(tableName, schemaName);
        }
        public ITableCreate<T> AlterTable<T>(string tableName = null, string schemaName = ""infoline"") where T : InfolineTable
        {
            return _database.AlterTable<T>(tableName, schemaName);
        }

        public TableInfo TableInfo(string tableName, bool onlyColumns = false)
        {
            return _database.TableInfo(tableName, onlyColumns); 
        }
        public ResultStatus DropTable(string tableName, string schemaName = ""infoline"")
        {
            return _database.DropTable(tableName, schemaName);
        }
        public bool TableExists(string tableName, string schemaName = ""infoline"")
        {
            return _database.TableExists(tableName, schemaName);
        }



        public ResultStatus ExecuteInsert<T>(T parameter, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>(""infoline"", tableName).Insert(parameter);
        }
        public ResultStatus ExecuteUpdate<T>(T parameter, bool setNull = false, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>(""infoline"", tableName).Update(parameter, a => a.id, a => new { a.created, a.id }, setNull);
        }
        public ResultStatus ExecuteDelete<T>(T parameter, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>(""infoline"", tableName).Delete(parameter, a => a.id);
        }
        public ResultStatus ExecuteDelete<T>(Guid id, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>(""infoline"", tableName).Delete(new T { id = id }, a => a.id);
        }
        public ResultStatus ExecuteBulkInsert<T>(IEnumerable<T> parameter, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>(""infoline"", tableName).Insert(parameter);
        }
        public ResultStatus ExecuteBulkUpdate<T>(IEnumerable<T> parametre, bool setNull = false, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>(""infoline"", tableName).Update(parametre, a => a.id, a => new { a.created, a.id }, setNull);
        }
        public ResultStatus ExecuteBulkDelete<T>(IEnumerable<T> parametre, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>(""ifoline"", tableName).Delete(parametre, a => a.id);
        }
        public ResultStatus ExecuteBulkDelete<T>(IEnumerable<Guid> parametre, string tableName = null) where T : InfolineTable, new()
        {
            return _database.Table<T>(""infoline"", tableName).Delete(parametre.Select(a => new T {  id = a }), a => a.id);
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

";
            return str;
        }

    }
}
