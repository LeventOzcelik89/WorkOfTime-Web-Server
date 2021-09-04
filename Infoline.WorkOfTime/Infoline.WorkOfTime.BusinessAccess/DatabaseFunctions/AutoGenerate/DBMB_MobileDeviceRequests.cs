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
        /// MB_MobileDeviceRequests tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>MB_MobileDeviceRequests dizi objesini geri döndürür.</returns>
        public MB_MobileDeviceRequests[] GetMB_MobileDeviceRequests(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<MB_MobileDeviceRequests>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// MB_MobileDeviceRequests tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu MB_MobileDeviceRequests dizi objesini geri döndürür.</returns>
        public MB_MobileDeviceRequests[] GetMB_MobileDeviceRequests(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<MB_MobileDeviceRequests>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// MB_MobileDeviceRequests tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetMB_MobileDeviceRequestsCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("MB_MobileDeviceRequests").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// MB_MobileDeviceRequests tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">MB_MobileDeviceRequests tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu MB_MobileDeviceRequests Objesini geri döndürür.</returns>
        public MB_MobileDeviceRequests GetMB_MobileDeviceRequestsById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<MB_MobileDeviceRequests>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// MB_MobileDeviceRequests Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">MB_MobileDeviceRequests Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertMB_MobileDeviceRequests(MB_MobileDeviceRequests item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<MB_MobileDeviceRequests>(item);
            }
        }

        /// <summary>
        /// MB_MobileDeviceRequests Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">MB_MobileDeviceRequests Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateMB_MobileDeviceRequests(MB_MobileDeviceRequests item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<MB_MobileDeviceRequests>(item, setNull);
            }
        }

        /// <summary>
        /// MB_MobileDeviceRequests tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">MB_MobileDeviceRequests Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteMB_MobileDeviceRequests(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<MB_MobileDeviceRequests>(id);
            }
        }

        /// <summary>
        /// MB_MobileDeviceRequests tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">MB_MobileDeviceRequests Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteMB_MobileDeviceRequests(MB_MobileDeviceRequests item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<MB_MobileDeviceRequests>(item);
            }
        }

        /// <summary>
        /// MB_MobileDeviceRequests Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">MB_MobileDeviceRequests Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertMB_MobileDeviceRequests(IEnumerable<MB_MobileDeviceRequests> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<MB_MobileDeviceRequests>(item);
            }
        }

        /// <summary>
        /// MB_MobileDeviceRequests Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">MB_MobileDeviceRequests Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateMB_MobileDeviceRequests(IEnumerable<MB_MobileDeviceRequests> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<MB_MobileDeviceRequests>(item, setNull);
            }
        }

        /// <summary>
        /// MB_MobileDeviceRequests tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">MB_MobileDeviceRequests Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteMB_MobileDeviceRequests(IEnumerable<MB_MobileDeviceRequests> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<MB_MobileDeviceRequests>(item);
            }
        }

    }
}
