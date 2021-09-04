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
        /// VWMB_MobileDeviceRequest tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWMB_MobileDeviceRequest dizi objesini geri döndürür.</returns>
        public VWMB_MobileDeviceRequest[] GetVWMB_MobileDeviceRequest(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWMB_MobileDeviceRequest>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWMB_MobileDeviceRequest tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWMB_MobileDeviceRequest dizi objesini geri döndürür.</returns>
        public VWMB_MobileDeviceRequest[] GetVWMB_MobileDeviceRequest(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWMB_MobileDeviceRequest>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWMB_MobileDeviceRequest tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWMB_MobileDeviceRequestCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWMB_MobileDeviceRequest").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWMB_MobileDeviceRequest tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWMB_MobileDeviceRequest tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWMB_MobileDeviceRequest Objesini geri döndürür.</returns>
        public VWMB_MobileDeviceRequest GetVWMB_MobileDeviceRequestById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWMB_MobileDeviceRequest>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
