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
        /// VWSV_DeviceProblem tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWSV_DeviceProblem dizi objesini geri döndürür.</returns>
        public VWSV_DeviceProblem[] GetVWSV_DeviceProblem(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSV_DeviceProblem>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWSV_DeviceProblem tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWSV_DeviceProblem dizi objesini geri döndürür.</returns>
        public VWSV_DeviceProblem[] GetVWSV_DeviceProblem(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSV_DeviceProblem>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWSV_DeviceProblem tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWSV_DeviceProblemCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWSV_DeviceProblem").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWSV_DeviceProblem tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWSV_DeviceProblem tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWSV_DeviceProblem Objesini geri döndürür.</returns>
        public VWSV_DeviceProblem GetVWSV_DeviceProblemById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWSV_DeviceProblem>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
