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
        /// VWCRM_ContactRelationTables tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWCRM_ContactRelationTables dizi objesini geri döndürür.</returns>
        public VWCRM_ContactRelationTables[] GetVWCRM_ContactRelationTables(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_ContactRelationTables>().Execute().ToArray();
            }
        }

        /// <summary>
        /// VWCRM_ContactRelationTables tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWCRM_ContactRelationTables dizi objesini geri döndürür.</returns>
        public VWCRM_ContactRelationTables[] GetVWCRM_ContactRelationTables(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWCRM_ContactRelationTables>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWCRM_ContactRelationTables tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWCRM_ContactRelationTablesCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWCRM_ContactRelationTables").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWCRM_ContactRelationTables tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWCRM_ContactRelationTables tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWCRM_ContactRelationTables Objesini geri döndürür.</returns>
        public VWCRM_ContactRelationTables GetVWCRM_ContactRelationTablesById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCRM_ContactRelationTables>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
