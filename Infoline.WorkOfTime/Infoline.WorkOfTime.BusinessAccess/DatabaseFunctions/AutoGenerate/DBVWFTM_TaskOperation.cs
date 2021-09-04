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
        /// VWFTM_TaskOperation tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWFTM_TaskOperation dizi objesini geri döndürür.</returns>
        public VWFTM_TaskOperation[] GetVWFTM_TaskOperation(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_TaskOperation>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWFTM_TaskOperation tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWFTM_TaskOperation dizi objesini geri döndürür.</returns>
        public VWFTM_TaskOperation[] GetVWFTM_TaskOperation(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWFTM_TaskOperation>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWFTM_TaskOperation tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWFTM_TaskOperationCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWFTM_TaskOperation").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWFTM_TaskOperation tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWFTM_TaskOperation tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWFTM_TaskOperation Objesini geri döndürür.</returns>
        public VWFTM_TaskOperation GetVWFTM_TaskOperationById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWFTM_TaskOperation>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
