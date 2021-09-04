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
        /// INV_CommisionConfirmation tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>INV_CommisionConfirmation dizi objesini geri döndürür.</returns>
        public INV_CommisionConfirmation[] GetINV_CommisionConfirmation(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CommisionConfirmation>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// INV_CommisionConfirmation tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu INV_CommisionConfirmation dizi objesini geri döndürür.</returns>
        public INV_CommisionConfirmation[] GetINV_CommisionConfirmation(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<INV_CommisionConfirmation>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// INV_CommisionConfirmation tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetINV_CommisionConfirmationCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("INV_CommisionConfirmation").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// INV_CommisionConfirmation tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">INV_CommisionConfirmation tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu INV_CommisionConfirmation Objesini geri döndürür.</returns>
        public INV_CommisionConfirmation GetINV_CommisionConfirmationById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<INV_CommisionConfirmation>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// INV_CommisionConfirmation Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">INV_CommisionConfirmation Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertINV_CommisionConfirmation(INV_CommisionConfirmation item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<INV_CommisionConfirmation>(item);
            }
        }

        /// <summary>
        /// INV_CommisionConfirmation Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">INV_CommisionConfirmation Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateINV_CommisionConfirmation(INV_CommisionConfirmation item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<INV_CommisionConfirmation>(item, setNull);
            }
        }

        /// <summary>
        /// INV_CommisionConfirmation tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">INV_CommisionConfirmation Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteINV_CommisionConfirmation(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<INV_CommisionConfirmation>(id);
            }
        }

        /// <summary>
        /// INV_CommisionConfirmation tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">INV_CommisionConfirmation Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteINV_CommisionConfirmation(INV_CommisionConfirmation item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<INV_CommisionConfirmation>(item);
            }
        }

        /// <summary>
        /// INV_CommisionConfirmation Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">INV_CommisionConfirmation Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertINV_CommisionConfirmation(IEnumerable<INV_CommisionConfirmation> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<INV_CommisionConfirmation>(item);
            }
        }

        /// <summary>
        /// INV_CommisionConfirmation Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">INV_CommisionConfirmation Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateINV_CommisionConfirmation(IEnumerable<INV_CommisionConfirmation> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<INV_CommisionConfirmation>(item, setNull);
            }
        }

        /// <summary>
        /// INV_CommisionConfirmation tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">INV_CommisionConfirmation Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteINV_CommisionConfirmation(IEnumerable<INV_CommisionConfirmation> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<INV_CommisionConfirmation>(item);
            }
        }

    }
}
