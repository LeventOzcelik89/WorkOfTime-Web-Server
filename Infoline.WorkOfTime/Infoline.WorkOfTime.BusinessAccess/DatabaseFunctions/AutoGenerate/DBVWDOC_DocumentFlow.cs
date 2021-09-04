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
        /// VWDOC_DocumentFlow tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWDOC_DocumentFlow dizi objesini geri döndürür.</returns>
        public VWDOC_DocumentFlow[] GetVWDOC_DocumentFlow(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWDOC_DocumentFlow>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWDOC_DocumentFlow tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWDOC_DocumentFlow dizi objesini geri döndürür.</returns>
        public VWDOC_DocumentFlow[] GetVWDOC_DocumentFlow(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWDOC_DocumentFlow>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWDOC_DocumentFlow tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWDOC_DocumentFlowCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWDOC_DocumentFlow").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWDOC_DocumentFlow tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWDOC_DocumentFlow tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWDOC_DocumentFlow Objesini geri döndürür.</returns>
        public VWDOC_DocumentFlow GetVWDOC_DocumentFlowById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWDOC_DocumentFlow>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
