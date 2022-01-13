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
        /// SYS_Notification tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>SYS_Notification dizi objesini geri döndürür.</returns>
        public SYS_Notification[] GetSYS_Notification(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_Notification>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// SYS_Notification tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu SYS_Notification dizi objesini geri döndürür.</returns>
        public SYS_Notification[] GetSYS_Notification(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<SYS_Notification>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// SYS_Notification tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetSYS_NotificationCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("SYS_Notification").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// SYS_Notification tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">SYS_Notification tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu SYS_Notification Objesini geri döndürür.</returns>
        public SYS_Notification GetSYS_NotificationById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<SYS_Notification>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// SYS_Notification Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_Notification Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertSYS_Notification(SYS_Notification item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<SYS_Notification>(item);
            }
        }

        /// <summary>
        /// SYS_Notification Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_Notification Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateSYS_Notification(SYS_Notification item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<SYS_Notification>(item, setNull);
            }
        }

        /// <summary>
        /// SYS_Notification tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">SYS_Notification Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteSYS_Notification(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<SYS_Notification>(id);
            }
        }

        /// <summary>
        /// SYS_Notification tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_Notification Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteSYS_Notification(SYS_Notification item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<SYS_Notification>(item);
            }
        }

        /// <summary>
        /// SYS_Notification Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_Notification Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertSYS_Notification(IEnumerable<SYS_Notification> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<SYS_Notification>(item);
            }
        }

        /// <summary>
        /// SYS_Notification Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_Notification Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateSYS_Notification(IEnumerable<SYS_Notification> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<SYS_Notification>(item, setNull);
            }
        }

        /// <summary>
        /// SYS_Notification tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_Notification Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteSYS_Notification(IEnumerable<SYS_Notification> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<SYS_Notification>(item);
            }
        }

    }
}
