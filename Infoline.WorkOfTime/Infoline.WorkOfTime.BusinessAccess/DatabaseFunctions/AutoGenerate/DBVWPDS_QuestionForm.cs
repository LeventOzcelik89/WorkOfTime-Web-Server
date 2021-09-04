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
        /// VWPDS_QuestionForm tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWPDS_QuestionForm dizi objesini geri döndürür.</returns>
        public VWPDS_QuestionForm[] GetVWPDS_QuestionForm(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPDS_QuestionForm>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWPDS_QuestionForm tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWPDS_QuestionForm dizi objesini geri döndürür.</returns>
        public VWPDS_QuestionForm[] GetVWPDS_QuestionForm(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWPDS_QuestionForm>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWPDS_QuestionForm tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWPDS_QuestionFormCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWPDS_QuestionForm").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWPDS_QuestionForm tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWPDS_QuestionForm tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWPDS_QuestionForm Objesini geri döndürür.</returns>
        public VWPDS_QuestionForm GetVWPDS_QuestionFormById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPDS_QuestionForm>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}
