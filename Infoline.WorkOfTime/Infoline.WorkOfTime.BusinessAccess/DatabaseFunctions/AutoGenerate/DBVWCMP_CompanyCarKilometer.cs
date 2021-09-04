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
        /// VWCMP_CompanyCarKilometer tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWCMP_CompanyCarKilometer dizi objesini geri döndürür.</returns>
        public VWCMP_CompanyCarKilometer[] GetVWCMP_CompanyCarKilometer(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_CompanyCarKilometer>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWCMP_CompanyCarKilometer tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWCMP_CompanyCarKilometer dizi objesini geri döndürür.</returns>
        public VWCMP_CompanyCarKilometer[] GetVWCMP_CompanyCarKilometer(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWCMP_CompanyCarKilometer>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWCMP_CompanyCarKilometer tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWCMP_CompanyCarKilometerCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWCMP_CompanyCarKilometer").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWCMP_CompanyCarKilometer tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWCMP_CompanyCarKilometer tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWCMP_CompanyCarKilometer Objesini geri döndürür.</returns>
        public VWCMP_CompanyCarKilometer GetVWCMP_CompanyCarKilometerById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_CompanyCarKilometer>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
