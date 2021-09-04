using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        /// <summary>
        /// VWAPM_ActivityRelationTables tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWAPM_ActivityRelationTables dizi objesini geri döndürür.</returns>
        public VWAPM_ActivityRelationTables[] GetVWAPM_ActivityRelationTables(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWAPM_ActivityRelationTables>().Execute().ToArray();
            }
        }

        /// <summary>
        /// VWAPM_ActivityRelationTables tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWAPM_ActivityRelationTables dizi objesini geri döndürür.</returns>
        public VWAPM_ActivityRelationTables[] GetVWAPM_ActivityRelationTables(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWAPM_ActivityRelationTables>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWAPM_ActivityRelationTables tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWAPM_ActivityRelationTablesCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWAPM_ActivityRelationTables").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWAPM_ActivityRelationTables tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWAPM_ActivityRelationTables tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWAPM_ActivityRelationTables Objesini geri döndürür.</returns>
        public VWAPM_ActivityRelationTables GetVWAPM_ActivityRelationTablesById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWAPM_ActivityRelationTables>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
