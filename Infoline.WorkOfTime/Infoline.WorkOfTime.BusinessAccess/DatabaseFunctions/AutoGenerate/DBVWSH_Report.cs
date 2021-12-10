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
        /// VWSH_Report tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWSH_Report dizi objesini geri döndürür.</returns>
        public VWSH_Report[] GetVWSH_Report(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_Report>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWSH_Report tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWSH_Report dizi objesini geri döndürür.</returns>
        public VWSH_Report[] GetVWSH_Report(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSH_Report>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWSH_Report tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWSH_ReportCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWSH_Report").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWSH_Report tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWSH_Report tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWSH_Report Objesini geri döndürür.</returns>
        public VWSH_Report GetVWSH_ReportById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWSH_Report>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
