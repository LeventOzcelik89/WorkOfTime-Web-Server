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
        /// VWMB_MobileDevice tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWMB_MobileDevice dizi objesini geri döndürür.</returns>
        public VWMB_MobileDevice[] GetVWMB_MobileDevice(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWMB_MobileDevice>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWMB_MobileDevice tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWMB_MobileDevice dizi objesini geri döndürür.</returns>
        public VWMB_MobileDevice[] GetVWMB_MobileDevice(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWMB_MobileDevice>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWMB_MobileDevice tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWMB_MobileDeviceCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWMB_MobileDevice").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWMB_MobileDevice tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWMB_MobileDevice tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWMB_MobileDevice Objesini geri döndürür.</returns>
        public VWMB_MobileDevice GetVWMB_MobileDeviceById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWMB_MobileDevice>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
