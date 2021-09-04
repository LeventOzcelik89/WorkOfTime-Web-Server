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
        /// VWAPM_ActivityRelation tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWAPM_ActivityRelation dizi objesini geri döndürür.</returns>
        public VWAPM_ActivityRelation[] GetVWAPM_ActivityRelation(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWAPM_ActivityRelation>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWAPM_ActivityRelation tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWAPM_ActivityRelation dizi objesini geri döndürür.</returns>
        public VWAPM_ActivityRelation[] GetVWAPM_ActivityRelation(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWAPM_ActivityRelation>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWAPM_ActivityRelation tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWAPM_ActivityRelationCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWAPM_ActivityRelation").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWAPM_ActivityRelation tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWAPM_ActivityRelation tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWAPM_ActivityRelation Objesini geri döndürür.</returns>
        public VWAPM_ActivityRelation GetVWAPM_ActivityRelationById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWAPM_ActivityRelation>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
