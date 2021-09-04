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
        /// VWFTM_TaskNew tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWFTM_TaskNew dizi objesini geri döndürür.</returns>
        public VWFTM_TaskNew[] GetVWFTM_TaskNew(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_TaskNew>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWFTM_TaskNew tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWFTM_TaskNew dizi objesini geri döndürür.</returns>
        public VWFTM_TaskNew[] GetVWFTM_TaskNew(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWFTM_TaskNew>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWFTM_TaskNew tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWFTM_TaskNewCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWFTM_TaskNew").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWFTM_TaskNew tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWFTM_TaskNew tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWFTM_TaskNew Objesini geri döndürür.</returns>
        public VWFTM_TaskNew GetVWFTM_TaskNewById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWFTM_TaskNew>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
