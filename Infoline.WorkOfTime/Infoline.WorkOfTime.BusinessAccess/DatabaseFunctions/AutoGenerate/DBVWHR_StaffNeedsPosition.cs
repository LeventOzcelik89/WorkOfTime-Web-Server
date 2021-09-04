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
        /// VWHR_StaffNeedsPosition tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWHR_StaffNeedsPosition dizi objesini geri döndürür.</returns>
        public VWHR_StaffNeedsPosition[] GetVWHR_StaffNeedsPosition(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWHR_StaffNeedsPosition>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWHR_StaffNeedsPosition tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWHR_StaffNeedsPosition dizi objesini geri döndürür.</returns>
        public VWHR_StaffNeedsPosition[] GetVWHR_StaffNeedsPosition(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWHR_StaffNeedsPosition>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWHR_StaffNeedsPosition tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWHR_StaffNeedsPositionCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWHR_StaffNeedsPosition").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWHR_StaffNeedsPosition tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWHR_StaffNeedsPosition tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWHR_StaffNeedsPosition Objesini geri döndürür.</returns>
        public VWHR_StaffNeedsPosition GetVWHR_StaffNeedsPositionById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWHR_StaffNeedsPosition>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
