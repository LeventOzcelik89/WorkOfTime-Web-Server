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
        /// CRM_PerformanceMultiplier tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>CRM_PerformanceMultiplier dizi objesini geri döndürür.</returns>
        public CRM_PerformanceMultiplier[] GetCRM_PerformanceMultiplier(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CRM_PerformanceMultiplier>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// CRM_PerformanceMultiplier tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu CRM_PerformanceMultiplier dizi objesini geri döndürür.</returns>
        public CRM_PerformanceMultiplier[] GetCRM_PerformanceMultiplier(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<CRM_PerformanceMultiplier>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// CRM_PerformanceMultiplier tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetCRM_PerformanceMultiplierCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("CRM_PerformanceMultiplier").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// CRM_PerformanceMultiplier tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">CRM_PerformanceMultiplier tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu CRM_PerformanceMultiplier Objesini geri döndürür.</returns>
        public CRM_PerformanceMultiplier GetCRM_PerformanceMultiplierById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CRM_PerformanceMultiplier>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// CRM_PerformanceMultiplier Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">CRM_PerformanceMultiplier Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertCRM_PerformanceMultiplier(CRM_PerformanceMultiplier item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<CRM_PerformanceMultiplier>(item);
            }
        }

        /// <summary>
        /// CRM_PerformanceMultiplier Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">CRM_PerformanceMultiplier Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateCRM_PerformanceMultiplier(CRM_PerformanceMultiplier item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<CRM_PerformanceMultiplier>(item, setNull);
            }
        }

        /// <summary>
        /// CRM_PerformanceMultiplier tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">CRM_PerformanceMultiplier Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteCRM_PerformanceMultiplier(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<CRM_PerformanceMultiplier>(id);
            }
        }

        /// <summary>
        /// CRM_PerformanceMultiplier tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">CRM_PerformanceMultiplier Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteCRM_PerformanceMultiplier(CRM_PerformanceMultiplier item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<CRM_PerformanceMultiplier>(item);
            }
        }

        /// <summary>
        /// CRM_PerformanceMultiplier Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">CRM_PerformanceMultiplier Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertCRM_PerformanceMultiplier(IEnumerable<CRM_PerformanceMultiplier> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<CRM_PerformanceMultiplier>(item);
            }
        }

        /// <summary>
        /// CRM_PerformanceMultiplier Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">CRM_PerformanceMultiplier Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateCRM_PerformanceMultiplier(IEnumerable<CRM_PerformanceMultiplier> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<CRM_PerformanceMultiplier>(item, setNull);
            }
        }

        /// <summary>
        /// CRM_PerformanceMultiplier tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">CRM_PerformanceMultiplier Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteCRM_PerformanceMultiplier(IEnumerable<CRM_PerformanceMultiplier> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<CRM_PerformanceMultiplier>(item);
            }
        }

    }
}
