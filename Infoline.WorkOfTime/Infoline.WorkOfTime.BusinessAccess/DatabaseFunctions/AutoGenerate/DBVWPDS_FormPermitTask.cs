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
        /// VWPDS_FormPermitTask tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWPDS_FormPermitTask dizi objesini geri döndürür.</returns>
        public VWPDS_FormPermitTask[] GetVWPDS_FormPermitTask(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPDS_FormPermitTask>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWPDS_FormPermitTask tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWPDS_FormPermitTask dizi objesini geri döndürür.</returns>
        public VWPDS_FormPermitTask[] GetVWPDS_FormPermitTask(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWPDS_FormPermitTask>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWPDS_FormPermitTask tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWPDS_FormPermitTaskCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWPDS_FormPermitTask").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWPDS_FormPermitTask tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWPDS_FormPermitTask tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWPDS_FormPermitTask Objesini geri döndürür.</returns>
        public VWPDS_FormPermitTask GetVWPDS_FormPermitTaskById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPDS_FormPermitTask>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
