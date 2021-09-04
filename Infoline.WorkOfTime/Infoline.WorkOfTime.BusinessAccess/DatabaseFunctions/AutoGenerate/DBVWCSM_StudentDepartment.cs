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
        /// VWCSM_StudentDepartment tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWCSM_StudentDepartment dizi objesini geri döndürür.</returns>
        public VWCSM_StudentDepartment[] GetVWCSM_StudentDepartment(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCSM_StudentDepartment>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWCSM_StudentDepartment tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWCSM_StudentDepartment dizi objesini geri döndürür.</returns>
        public VWCSM_StudentDepartment[] GetVWCSM_StudentDepartment(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWCSM_StudentDepartment>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWCSM_StudentDepartment tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWCSM_StudentDepartmentCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWCSM_StudentDepartment").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWCSM_StudentDepartment tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWCSM_StudentDepartment tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWCSM_StudentDepartment Objesini geri döndürür.</returns>
        public VWCSM_StudentDepartment GetVWCSM_StudentDepartmentById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCSM_StudentDepartment>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
