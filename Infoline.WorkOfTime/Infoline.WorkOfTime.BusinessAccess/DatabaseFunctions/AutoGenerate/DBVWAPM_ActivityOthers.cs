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
        /// VWAPM_ActivityOthers tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWAPM_ActivityOthers dizi objesini geri döndürür.</returns>
        public VWAPM_ActivityOthers[] GetVWAPM_ActivityOthers(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWAPM_ActivityOthers>().Execute().ToArray();
            }
        }

        /// <summary>
        /// VWAPM_ActivityOthers tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWAPM_ActivityOthers dizi objesini geri döndürür.</returns>
        public VWAPM_ActivityOthers[] GetVWAPM_ActivityOthers(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWAPM_ActivityOthers>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWAPM_ActivityOthers tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWAPM_ActivityOthersCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWAPM_ActivityOthers").Where(conditionExpression).Count();
            }
        }
    }
}
