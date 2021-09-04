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
        /// VWAPM_ActivityAction tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWAPM_ActivityAction dizi objesini geri döndürür.</returns>
        public VWAPM_ActivityAction[] GetVWAPM_ActivityAction(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWAPM_ActivityAction>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWAPM_ActivityAction tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWAPM_ActivityAction dizi objesini geri döndürür.</returns>
        public VWAPM_ActivityAction[] GetVWAPM_ActivityAction(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWAPM_ActivityAction>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWAPM_ActivityAction tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWAPM_ActivityActionCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWAPM_ActivityAction").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWAPM_ActivityAction tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWAPM_ActivityAction tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWAPM_ActivityAction Objesini geri döndürür.</returns>
        public VWAPM_ActivityAction GetVWAPM_ActivityActionById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWAPM_ActivityAction>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
