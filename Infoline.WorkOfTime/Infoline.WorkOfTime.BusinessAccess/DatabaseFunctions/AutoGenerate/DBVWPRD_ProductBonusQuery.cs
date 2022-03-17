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
        /// VWPRD_ProductBonusQuery tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWPRD_ProductBonusQuery dizi objesini geri döndürür.</returns>
        public VWPRD_ProductBonusQuery[] GetVWPRD_ProductBonusQuery(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_ProductBonusQuery>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWPRD_ProductBonusQuery tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWPRD_ProductBonusQuery dizi objesini geri döndürür.</returns>
        public VWPRD_ProductBonusQuery[] GetVWPRD_ProductBonusQuery(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWPRD_ProductBonusQuery>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWPRD_ProductBonusQuery tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWPRD_ProductBonusQueryCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWPRD_ProductBonusQuery").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWPRD_ProductBonusQuery tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWPRD_ProductBonusQuery tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWPRD_ProductBonusQuery Objesini geri döndürür.</returns>
        public VWPRD_ProductBonusQuery GetVWPRD_ProductBonusQueryById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPRD_ProductBonusQuery>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
