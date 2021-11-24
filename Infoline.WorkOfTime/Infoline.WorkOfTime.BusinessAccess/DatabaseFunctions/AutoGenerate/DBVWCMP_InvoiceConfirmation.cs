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
        /// VWCMP_InvoiceConfirmation tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWCMP_InvoiceConfirmation dizi objesini geri döndürür.</returns>
        public VWCMP_InvoiceConfirmation[] GetVWCMP_InvoiceConfirmation(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_InvoiceConfirmation>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWCMP_InvoiceConfirmation tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWCMP_InvoiceConfirmation dizi objesini geri döndürür.</returns>
        public VWCMP_InvoiceConfirmation[] GetVWCMP_InvoiceConfirmation(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWCMP_InvoiceConfirmation>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWCMP_InvoiceConfirmation tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWCMP_InvoiceConfirmationCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWCMP_InvoiceConfirmation").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWCMP_InvoiceConfirmation tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWCMP_InvoiceConfirmation tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWCMP_InvoiceConfirmation Objesini geri döndürür.</returns>
        public VWCMP_InvoiceConfirmation GetVWCMP_InvoiceConfirmationById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_InvoiceConfirmation>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
