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
        /// VWSRV_ServiceMaintenance tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWSRV_ServiceMaintenance dizi objesini geri döndürür.</returns>
        public VWSRV_ServiceMaintenance[] GetVWSRV_ServiceMaintenance(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSRV_ServiceMaintenance>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWSRV_ServiceMaintenance tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWSRV_ServiceMaintenance dizi objesini geri döndürür.</returns>
        public VWSRV_ServiceMaintenance[] GetVWSRV_ServiceMaintenance(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSRV_ServiceMaintenance>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWSRV_ServiceMaintenance tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWSRV_ServiceMaintenanceCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWSRV_ServiceMaintenance").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWSRV_ServiceMaintenance tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWSRV_ServiceMaintenance tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWSRV_ServiceMaintenance Objesini geri döndürür.</returns>
        public VWSRV_ServiceMaintenance GetVWSRV_ServiceMaintenanceById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWSRV_ServiceMaintenance>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
