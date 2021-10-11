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
        /// SH_ShiftTrackingDeviceUsers tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>SH_ShiftTrackingDeviceUsers dizi objesini geri döndürür.</returns>
        public SH_ShiftTrackingDeviceUsers[] GetSH_ShiftTrackingDeviceUsers(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_ShiftTrackingDeviceUsers>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu SH_ShiftTrackingDeviceUsers dizi objesini geri döndürür.</returns>
        public SH_ShiftTrackingDeviceUsers[] GetSH_ShiftTrackingDeviceUsers(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<SH_ShiftTrackingDeviceUsers>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// SH_ShiftTrackingDeviceUsers tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetSH_ShiftTrackingDeviceUsersCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("SH_ShiftTrackingDeviceUsers").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// SH_ShiftTrackingDeviceUsers tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">SH_ShiftTrackingDeviceUsers tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu SH_ShiftTrackingDeviceUsers Objesini geri döndürür.</returns>
        public SH_ShiftTrackingDeviceUsers GetSH_ShiftTrackingDeviceUsersById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<SH_ShiftTrackingDeviceUsers>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">SH_ShiftTrackingDeviceUsers Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertSH_ShiftTrackingDeviceUsers(SH_ShiftTrackingDeviceUsers item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<SH_ShiftTrackingDeviceUsers>(item);
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SH_ShiftTrackingDeviceUsers Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateSH_ShiftTrackingDeviceUsers(SH_ShiftTrackingDeviceUsers item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<SH_ShiftTrackingDeviceUsers>(item, setNull);
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">SH_ShiftTrackingDeviceUsers Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteSH_ShiftTrackingDeviceUsers(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<SH_ShiftTrackingDeviceUsers>(id);
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SH_ShiftTrackingDeviceUsers Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteSH_ShiftTrackingDeviceUsers(SH_ShiftTrackingDeviceUsers item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<SH_ShiftTrackingDeviceUsers>(item);
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">SH_ShiftTrackingDeviceUsers Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertSH_ShiftTrackingDeviceUsers(IEnumerable<SH_ShiftTrackingDeviceUsers> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<SH_ShiftTrackingDeviceUsers>(item);
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">SH_ShiftTrackingDeviceUsers Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateSH_ShiftTrackingDeviceUsers(IEnumerable<SH_ShiftTrackingDeviceUsers> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<SH_ShiftTrackingDeviceUsers>(item, setNull);
            }
        }

        /// <summary>
        /// SH_ShiftTrackingDeviceUsers tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SH_ShiftTrackingDeviceUsers Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteSH_ShiftTrackingDeviceUsers(IEnumerable<SH_ShiftTrackingDeviceUsers> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<SH_ShiftTrackingDeviceUsers>(item);
            }
        }

    }
}
