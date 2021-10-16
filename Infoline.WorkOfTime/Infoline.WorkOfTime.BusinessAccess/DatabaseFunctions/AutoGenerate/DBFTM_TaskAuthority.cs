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
        /// FTM_TaskAuthority tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>FTM_TaskAuthority dizi objesini geri döndürür.</returns>
        public FTM_TaskAuthority[] GetFTM_TaskAuthority(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<FTM_TaskAuthority>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// FTM_TaskAuthority tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu FTM_TaskAuthority dizi objesini geri döndürür.</returns>
        public FTM_TaskAuthority[] GetFTM_TaskAuthority(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<FTM_TaskAuthority>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// FTM_TaskAuthority tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetFTM_TaskAuthorityCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("FTM_TaskAuthority").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// FTM_TaskAuthority tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">FTM_TaskAuthority tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu FTM_TaskAuthority Objesini geri döndürür.</returns>
        public FTM_TaskAuthority GetFTM_TaskAuthorityById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<FTM_TaskAuthority>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// FTM_TaskAuthority Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">FTM_TaskAuthority Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertFTM_TaskAuthority(FTM_TaskAuthority item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<FTM_TaskAuthority>(item);
            }
        }

        /// <summary>
        /// FTM_TaskAuthority Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">FTM_TaskAuthority Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateFTM_TaskAuthority(FTM_TaskAuthority item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<FTM_TaskAuthority>(item, setNull);
            }
        }

        /// <summary>
        /// FTM_TaskAuthority tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">FTM_TaskAuthority Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteFTM_TaskAuthority(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<FTM_TaskAuthority>(id);
            }
        }

        /// <summary>
        /// FTM_TaskAuthority tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">FTM_TaskAuthority Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteFTM_TaskAuthority(FTM_TaskAuthority item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<FTM_TaskAuthority>(item);
            }
        }

        /// <summary>
        /// FTM_TaskAuthority Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">FTM_TaskAuthority Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertFTM_TaskAuthority(IEnumerable<FTM_TaskAuthority> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<FTM_TaskAuthority>(item);
            }
        }

        /// <summary>
        /// FTM_TaskAuthority Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">FTM_TaskAuthority Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateFTM_TaskAuthority(IEnumerable<FTM_TaskAuthority> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<FTM_TaskAuthority>(item, setNull);
            }
        }

        /// <summary>
        /// FTM_TaskAuthority tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">FTM_TaskAuthority Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteFTM_TaskAuthority(IEnumerable<FTM_TaskAuthority> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<FTM_TaskAuthority>(item);
            }
        }

    }
}
