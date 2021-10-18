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
        /// VWSH_ShiftTrackingDeviceUsers tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWSH_ShiftTrackingDeviceUsers dizi objesini geri döndürür.</returns>
        public VWSH_ShiftTrackingDeviceUsers[] GetVWSH_ShiftTrackingDeviceUsers(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_ShiftTrackingDeviceUsers>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWSH_ShiftTrackingDeviceUsers tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWSH_ShiftTrackingDeviceUsers dizi objesini geri döndürür.</returns>
        public VWSH_ShiftTrackingDeviceUsers[] GetVWSH_ShiftTrackingDeviceUsers(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSH_ShiftTrackingDeviceUsers>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWSH_ShiftTrackingDeviceUsers tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWSH_ShiftTrackingDeviceUsersCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWSH_ShiftTrackingDeviceUsers").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWSH_ShiftTrackingDeviceUsers tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWSH_ShiftTrackingDeviceUsers tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWSH_ShiftTrackingDeviceUsers Objesini geri döndürür.</returns>
        public VWSH_ShiftTrackingDeviceUsers GetVWSH_ShiftTrackingDeviceUsersById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWSH_ShiftTrackingDeviceUsers>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
