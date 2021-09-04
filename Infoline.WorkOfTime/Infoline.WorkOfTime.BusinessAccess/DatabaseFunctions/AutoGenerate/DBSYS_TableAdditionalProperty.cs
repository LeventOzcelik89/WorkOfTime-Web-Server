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
        /// SYS_TableAdditionalProperty tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>SYS_TableAdditionalProperty dizi objesini geri döndürür.</returns>
        public SYS_TableAdditionalProperty[] GetSYS_TableAdditionalProperty(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SYS_TableAdditionalProperty>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// SYS_TableAdditionalProperty tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu SYS_TableAdditionalProperty dizi objesini geri döndürür.</returns>
        public SYS_TableAdditionalProperty[] GetSYS_TableAdditionalProperty(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<SYS_TableAdditionalProperty>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// SYS_TableAdditionalProperty tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetSYS_TableAdditionalPropertyCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("SYS_TableAdditionalProperty").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// SYS_TableAdditionalProperty tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">SYS_TableAdditionalProperty tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu SYS_TableAdditionalProperty Objesini geri döndürür.</returns>
        public SYS_TableAdditionalProperty GetSYS_TableAdditionalPropertyById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<SYS_TableAdditionalProperty>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// SYS_TableAdditionalProperty Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_TableAdditionalProperty Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertSYS_TableAdditionalProperty(SYS_TableAdditionalProperty item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<SYS_TableAdditionalProperty>(item);
            }
        }

        /// <summary>
        /// SYS_TableAdditionalProperty Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_TableAdditionalProperty Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateSYS_TableAdditionalProperty(SYS_TableAdditionalProperty item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<SYS_TableAdditionalProperty>(item, setNull);
            }
        }

        /// <summary>
        /// SYS_TableAdditionalProperty tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">SYS_TableAdditionalProperty Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteSYS_TableAdditionalProperty(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<SYS_TableAdditionalProperty>(id);
            }
        }

        /// <summary>
        /// SYS_TableAdditionalProperty tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_TableAdditionalProperty Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteSYS_TableAdditionalProperty(SYS_TableAdditionalProperty item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<SYS_TableAdditionalProperty>(item);
            }
        }

        /// <summary>
        /// SYS_TableAdditionalProperty Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_TableAdditionalProperty Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertSYS_TableAdditionalProperty(IEnumerable<SYS_TableAdditionalProperty> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<SYS_TableAdditionalProperty>(item);
            }
        }

        /// <summary>
        /// SYS_TableAdditionalProperty Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_TableAdditionalProperty Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateSYS_TableAdditionalProperty(IEnumerable<SYS_TableAdditionalProperty> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<SYS_TableAdditionalProperty>(item, setNull);
            }
        }

        /// <summary>
        /// SYS_TableAdditionalProperty tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">SYS_TableAdditionalProperty Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteSYS_TableAdditionalProperty(IEnumerable<SYS_TableAdditionalProperty> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<SYS_TableAdditionalProperty>(item);
            }
        }

    }
}
