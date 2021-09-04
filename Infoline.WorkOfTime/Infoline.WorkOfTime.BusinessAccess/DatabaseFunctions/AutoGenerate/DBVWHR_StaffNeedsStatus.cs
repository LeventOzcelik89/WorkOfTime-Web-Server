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
        /// VWHR_StaffNeedsStatus tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWHR_StaffNeedsStatus dizi objesini geri döndürür.</returns>
        public VWHR_StaffNeedsStatus[] GetVWHR_StaffNeedsStatus(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWHR_StaffNeedsStatus>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWHR_StaffNeedsStatus tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWHR_StaffNeedsStatus dizi objesini geri döndürür.</returns>
        public VWHR_StaffNeedsStatus[] GetVWHR_StaffNeedsStatus(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWHR_StaffNeedsStatus>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWHR_StaffNeedsStatus tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWHR_StaffNeedsStatusCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWHR_StaffNeedsStatus").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWHR_StaffNeedsStatus tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWHR_StaffNeedsStatus tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWHR_StaffNeedsStatus Objesini geri döndürür.</returns>
        public VWHR_StaffNeedsStatus GetVWHR_StaffNeedsStatusById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWHR_StaffNeedsStatus>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
