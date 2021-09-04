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
        /// VWCRM_MonthlyTargetPerson tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWCRM_MonthlyTargetPerson dizi objesini geri döndürür.</returns>
        public VWCRM_MonthlyTargetPerson[] GetVWCRM_MonthlyTargetPerson(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_MonthlyTargetPerson>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWCRM_MonthlyTargetPerson tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWCRM_MonthlyTargetPerson dizi objesini geri döndürür.</returns>
        public VWCRM_MonthlyTargetPerson[] GetVWCRM_MonthlyTargetPerson(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWCRM_MonthlyTargetPerson>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWCRM_MonthlyTargetPerson tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWCRM_MonthlyTargetPersonCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWCRM_MonthlyTargetPerson").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWCRM_MonthlyTargetPerson tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWCRM_MonthlyTargetPerson tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWCRM_MonthlyTargetPerson Objesini geri döndürür.</returns>
        public VWCRM_MonthlyTargetPerson GetVWCRM_MonthlyTargetPersonById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCRM_MonthlyTargetPerson>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
