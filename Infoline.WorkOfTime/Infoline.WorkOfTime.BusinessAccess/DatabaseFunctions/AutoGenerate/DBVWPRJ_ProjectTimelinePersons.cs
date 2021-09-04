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
        /// VWPRJ_ProjectTimelinePersons tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWPRJ_ProjectTimelinePersons dizi objesini geri döndürür.</returns>
        public VWPRJ_ProjectTimelinePersons[] GetVWPRJ_ProjectTimelinePersons(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRJ_ProjectTimelinePersons>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWPRJ_ProjectTimelinePersons tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWPRJ_ProjectTimelinePersons dizi objesini geri döndürür.</returns>
        public VWPRJ_ProjectTimelinePersons[] GetVWPRJ_ProjectTimelinePersons(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWPRJ_ProjectTimelinePersons>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWPRJ_ProjectTimelinePersons tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWPRJ_ProjectTimelinePersonsCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWPRJ_ProjectTimelinePersons").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWPRJ_ProjectTimelinePersons tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWPRJ_ProjectTimelinePersons tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWPRJ_ProjectTimelinePersons Objesini geri döndürür.</returns>
        public VWPRJ_ProjectTimelinePersons GetVWPRJ_ProjectTimelinePersonsById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPRJ_ProjectTimelinePersons>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
