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
        /// VWDOC_DocumentRelation tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWDOC_DocumentRelation dizi objesini geri döndürür.</returns>
        public VWDOC_DocumentRelation[] GetVWDOC_DocumentRelation(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWDOC_DocumentRelation>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWDOC_DocumentRelation tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWDOC_DocumentRelation dizi objesini geri döndürür.</returns>
        public VWDOC_DocumentRelation[] GetVWDOC_DocumentRelation(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWDOC_DocumentRelation>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWDOC_DocumentRelation tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWDOC_DocumentRelationCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWDOC_DocumentRelation").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWDOC_DocumentRelation tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWDOC_DocumentRelation tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWDOC_DocumentRelation Objesini geri döndürür.</returns>
        public VWDOC_DocumentRelation GetVWDOC_DocumentRelationById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWDOC_DocumentRelation>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
