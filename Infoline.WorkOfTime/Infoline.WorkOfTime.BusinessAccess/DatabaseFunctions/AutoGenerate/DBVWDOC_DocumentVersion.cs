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
        /// VWDOC_DocumentVersion tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWDOC_DocumentVersion dizi objesini geri döndürür.</returns>
        public VWDOC_DocumentVersion[] GetVWDOC_DocumentVersion(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWDOC_DocumentVersion>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWDOC_DocumentVersion tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWDOC_DocumentVersion dizi objesini geri döndürür.</returns>
        public VWDOC_DocumentVersion[] GetVWDOC_DocumentVersion(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWDOC_DocumentVersion>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWDOC_DocumentVersion tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWDOC_DocumentVersionCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWDOC_DocumentVersion").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWDOC_DocumentVersion tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWDOC_DocumentVersion tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWDOC_DocumentVersion Objesini geri döndürür.</returns>
        public VWDOC_DocumentVersion GetVWDOC_DocumentVersionById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWDOC_DocumentVersion>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
