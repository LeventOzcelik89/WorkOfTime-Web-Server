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
        /// VWPRD_SellOutReportData tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWPRD_SellOutReportData dizi objesini geri döndürür.</returns>
        public VWPRD_SellOutReportData[] GetVWPRD_SellOutReportData(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_SellOutReportData>().Execute().ToArray();
            }
        }

        /// <summary>
        /// VWPRD_SellOutReportData tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWPRD_SellOutReportData dizi objesini geri döndürür.</returns>
        public VWPRD_SellOutReportData[] GetVWPRD_SellOutReportData(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWPRD_SellOutReportData>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWPRD_SellOutReportData tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWPRD_SellOutReportDataCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWPRD_SellOutReportData").Where(conditionExpression).Count();
            }
        }
    }
}
