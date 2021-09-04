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
        /// PRJ_ProjectSubjectLevelItems tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>PRJ_ProjectSubjectLevelItems dizi objesini geri döndürür.</returns>
        public PRJ_ProjectSubjectLevelItems[] GetPRJ_ProjectSubjectLevelItems(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRJ_ProjectSubjectLevelItems>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// PRJ_ProjectSubjectLevelItems tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu PRJ_ProjectSubjectLevelItems dizi objesini geri döndürür.</returns>
        public PRJ_ProjectSubjectLevelItems[] GetPRJ_ProjectSubjectLevelItems(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<PRJ_ProjectSubjectLevelItems>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// PRJ_ProjectSubjectLevelItems tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetPRJ_ProjectSubjectLevelItemsCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("PRJ_ProjectSubjectLevelItems").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// PRJ_ProjectSubjectLevelItems tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">PRJ_ProjectSubjectLevelItems tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu PRJ_ProjectSubjectLevelItems Objesini geri döndürür.</returns>
        public PRJ_ProjectSubjectLevelItems GetPRJ_ProjectSubjectLevelItemsById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PRJ_ProjectSubjectLevelItems>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// PRJ_ProjectSubjectLevelItems Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">PRJ_ProjectSubjectLevelItems Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertPRJ_ProjectSubjectLevelItems(PRJ_ProjectSubjectLevelItems item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<PRJ_ProjectSubjectLevelItems>(item);
            }
        }

        /// <summary>
        /// PRJ_ProjectSubjectLevelItems Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">PRJ_ProjectSubjectLevelItems Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdatePRJ_ProjectSubjectLevelItems(PRJ_ProjectSubjectLevelItems item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<PRJ_ProjectSubjectLevelItems>(item, setNull);
            }
        }

        /// <summary>
        /// PRJ_ProjectSubjectLevelItems tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">PRJ_ProjectSubjectLevelItems Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeletePRJ_ProjectSubjectLevelItems(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<PRJ_ProjectSubjectLevelItems>(id);
            }
        }

        /// <summary>
        /// PRJ_ProjectSubjectLevelItems tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">PRJ_ProjectSubjectLevelItems Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeletePRJ_ProjectSubjectLevelItems(PRJ_ProjectSubjectLevelItems item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<PRJ_ProjectSubjectLevelItems>(item);
            }
        }

        /// <summary>
        /// PRJ_ProjectSubjectLevelItems Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">PRJ_ProjectSubjectLevelItems Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertPRJ_ProjectSubjectLevelItems(IEnumerable<PRJ_ProjectSubjectLevelItems> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<PRJ_ProjectSubjectLevelItems>(item);
            }
        }

        /// <summary>
        /// PRJ_ProjectSubjectLevelItems Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">PRJ_ProjectSubjectLevelItems Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdatePRJ_ProjectSubjectLevelItems(IEnumerable<PRJ_ProjectSubjectLevelItems> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<PRJ_ProjectSubjectLevelItems>(item, setNull);
            }
        }

        /// <summary>
        /// PRJ_ProjectSubjectLevelItems tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">PRJ_ProjectSubjectLevelItems Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeletePRJ_ProjectSubjectLevelItems(IEnumerable<PRJ_ProjectSubjectLevelItems> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<PRJ_ProjectSubjectLevelItems>(item);
            }
        }

    }
}
