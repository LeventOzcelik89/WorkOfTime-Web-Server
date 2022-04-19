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
        /// CMP_CompanyManaging tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>CMP_CompanyManaging dizi objesini geri döndürür.</returns>
        public CMP_CompanyManaging[] GetCMP_CompanyManaging(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_CompanyManaging>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// CMP_CompanyManaging tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu CMP_CompanyManaging dizi objesini geri döndürür.</returns>
        public CMP_CompanyManaging[] GetCMP_CompanyManaging(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<CMP_CompanyManaging>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// CMP_CompanyManaging tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetCMP_CompanyManagingCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("CMP_CompanyManaging").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// CMP_CompanyManaging tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">CMP_CompanyManaging tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu CMP_CompanyManaging Objesini geri döndürür.</returns>
        public CMP_CompanyManaging GetCMP_CompanyManagingById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_CompanyManaging>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// CMP_CompanyManaging Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_CompanyManaging Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertCMP_CompanyManaging(CMP_CompanyManaging item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<CMP_CompanyManaging>(item);
            }
        }

        /// <summary>
        /// CMP_CompanyManaging Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_CompanyManaging Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateCMP_CompanyManaging(CMP_CompanyManaging item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<CMP_CompanyManaging>(item, setNull);
            }
        }

        /// <summary>
        /// CMP_CompanyManaging tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">CMP_CompanyManaging Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteCMP_CompanyManaging(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<CMP_CompanyManaging>(id);
            }
        }

        /// <summary>
        /// CMP_CompanyManaging tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_CompanyManaging Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteCMP_CompanyManaging(CMP_CompanyManaging item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<CMP_CompanyManaging>(item);
            }
        }

        /// <summary>
        /// CMP_CompanyManaging Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_CompanyManaging Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertCMP_CompanyManaging(IEnumerable<CMP_CompanyManaging> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<CMP_CompanyManaging>(item);
            }
        }

        /// <summary>
        /// CMP_CompanyManaging Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_CompanyManaging Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateCMP_CompanyManaging(IEnumerable<CMP_CompanyManaging> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<CMP_CompanyManaging>(item, setNull);
            }
        }

        /// <summary>
        /// CMP_CompanyManaging tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_CompanyManaging Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteCMP_CompanyManaging(IEnumerable<CMP_CompanyManaging> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<CMP_CompanyManaging>(item);
            }
        }

    }
}
