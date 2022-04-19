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
        /// VWPRD_TitanDeviceActivatedSummary tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWPRD_TitanDeviceActivatedSummary dizi objesini geri döndürür.</returns>
        public VWPRD_TitanDeviceActivatedSummary[] GetVWPRD_TitanDeviceActivatedSummary(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_TitanDeviceActivatedSummary>().Execute().ToArray();
            }
        }

        /// <summary>
        /// VWPRD_TitanDeviceActivatedSummary tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWPRD_TitanDeviceActivatedSummary dizi objesini geri döndürür.</returns>
        public VWPRD_TitanDeviceActivatedSummary[] GetVWPRD_TitanDeviceActivatedSummary(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWPRD_TitanDeviceActivatedSummary>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWPRD_TitanDeviceActivatedSummary tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWPRD_TitanDeviceActivatedSummaryCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWPRD_TitanDeviceActivatedSummary").Where(conditionExpression).Count();
            }
        }
    }
}
