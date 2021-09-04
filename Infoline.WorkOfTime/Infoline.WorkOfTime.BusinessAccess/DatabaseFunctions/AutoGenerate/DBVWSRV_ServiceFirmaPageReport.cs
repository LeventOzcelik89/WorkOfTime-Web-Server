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
        /// VWSRV_ServiceFirmaPageReport tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWSRV_ServiceFirmaPageReport dizi objesini geri döndürür.</returns>
        public VWSRV_ServiceFirmaPageReport[] GetVWSRV_ServiceFirmaPageReport(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSRV_ServiceFirmaPageReport>().Execute().ToArray();
            }
        }

        /// <summary>
        /// VWSRV_ServiceFirmaPageReport tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWSRV_ServiceFirmaPageReport dizi objesini geri döndürür.</returns>
        public VWSRV_ServiceFirmaPageReport[] GetVWSRV_ServiceFirmaPageReport(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSRV_ServiceFirmaPageReport>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWSRV_ServiceFirmaPageReport tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWSRV_ServiceFirmaPageReportCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWSRV_ServiceFirmaPageReport").Where(conditionExpression).Count();
            }
        }
    }
}
