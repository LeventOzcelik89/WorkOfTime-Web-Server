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
        /// VWINV_CompanyPersonAvailability tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWINV_CompanyPersonAvailability dizi objesini geri döndürür.</returns>
        public VWINV_CompanyPersonAvailability[] GetVWINV_CompanyPersonAvailability(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonAvailability>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWINV_CompanyPersonAvailability tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWINV_CompanyPersonAvailability dizi objesini geri döndürür.</returns>
        public VWINV_CompanyPersonAvailability[] GetVWINV_CompanyPersonAvailability(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWINV_CompanyPersonAvailability>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWINV_CompanyPersonAvailability tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWINV_CompanyPersonAvailabilityCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWINV_CompanyPersonAvailability").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWINV_CompanyPersonAvailability tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWINV_CompanyPersonAvailability tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWINV_CompanyPersonAvailability Objesini geri döndürür.</returns>
        public VWINV_CompanyPersonAvailability GetVWINV_CompanyPersonAvailabilityById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWINV_CompanyPersonAvailability>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
