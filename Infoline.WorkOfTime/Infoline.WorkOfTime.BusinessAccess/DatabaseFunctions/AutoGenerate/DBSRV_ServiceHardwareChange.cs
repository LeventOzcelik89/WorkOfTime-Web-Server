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
        /// SRV_ServiceHardwareChange tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>SRV_ServiceHardwareChange dizi objesini geri döndürür.</returns>
        public SRV_ServiceHardwareChange[] GetSRV_ServiceHardwareChange(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SRV_ServiceHardwareChange>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// SRV_ServiceHardwareChange tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu SRV_ServiceHardwareChange dizi objesini geri döndürür.</returns>
        public SRV_ServiceHardwareChange[] GetSRV_ServiceHardwareChange(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<SRV_ServiceHardwareChange>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// SRV_ServiceHardwareChange tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetSRV_ServiceHardwareChangeCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("SRV_ServiceHardwareChange").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// SRV_ServiceHardwareChange tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">SRV_ServiceHardwareChange tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu SRV_ServiceHardwareChange Objesini geri döndürür.</returns>
        public SRV_ServiceHardwareChange GetSRV_ServiceHardwareChangeById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<SRV_ServiceHardwareChange>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// SRV_ServiceHardwareChange Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">SRV_ServiceHardwareChange Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertSRV_ServiceHardwareChange(SRV_ServiceHardwareChange item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<SRV_ServiceHardwareChange>(item);
            }
        }

        /// <summary>
        /// SRV_ServiceHardwareChange Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SRV_ServiceHardwareChange Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateSRV_ServiceHardwareChange(SRV_ServiceHardwareChange item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<SRV_ServiceHardwareChange>(item, setNull);
            }
        }

        /// <summary>
        /// SRV_ServiceHardwareChange tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">SRV_ServiceHardwareChange Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteSRV_ServiceHardwareChange(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<SRV_ServiceHardwareChange>(id);
            }
        }

        /// <summary>
        /// SRV_ServiceHardwareChange tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SRV_ServiceHardwareChange Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteSRV_ServiceHardwareChange(SRV_ServiceHardwareChange item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<SRV_ServiceHardwareChange>(item);
            }
        }

        /// <summary>
        /// SRV_ServiceHardwareChange Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">SRV_ServiceHardwareChange Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertSRV_ServiceHardwareChange(IEnumerable<SRV_ServiceHardwareChange> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<SRV_ServiceHardwareChange>(item);
            }
        }

        /// <summary>
        /// SRV_ServiceHardwareChange Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">SRV_ServiceHardwareChange Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateSRV_ServiceHardwareChange(IEnumerable<SRV_ServiceHardwareChange> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<SRV_ServiceHardwareChange>(item, setNull);
            }
        }

        /// <summary>
        /// SRV_ServiceHardwareChange tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SRV_ServiceHardwareChange Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteSRV_ServiceHardwareChange(IEnumerable<SRV_ServiceHardwareChange> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<SRV_ServiceHardwareChange>(item);
            }
        }

    }
}
