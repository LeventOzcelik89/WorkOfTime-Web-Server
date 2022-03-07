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
        /// PRD_ProductBonusPrice tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>PRD_ProductBonusPrice dizi objesini geri döndürür.</returns>
        public PRD_ProductBonusPrice[] GetPRD_ProductBonusPrice(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductBonusPrice>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// PRD_ProductBonusPrice tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu PRD_ProductBonusPrice dizi objesini geri döndürür.</returns>
        public PRD_ProductBonusPrice[] GetPRD_ProductBonusPrice(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<PRD_ProductBonusPrice>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// PRD_ProductBonusPrice tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetPRD_ProductBonusPriceCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("PRD_ProductBonusPrice").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// PRD_ProductBonusPrice tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">PRD_ProductBonusPrice tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu PRD_ProductBonusPrice Objesini geri döndürür.</returns>
        public PRD_ProductBonusPrice GetPRD_ProductBonusPriceById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PRD_ProductBonusPrice>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// PRD_ProductBonusPrice Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">PRD_ProductBonusPrice Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertPRD_ProductBonusPrice(PRD_ProductBonusPrice item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<PRD_ProductBonusPrice>(item);
            }
        }

        /// <summary>
        /// PRD_ProductBonusPrice Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">PRD_ProductBonusPrice Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdatePRD_ProductBonusPrice(PRD_ProductBonusPrice item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<PRD_ProductBonusPrice>(item, setNull);
            }
        }

        /// <summary>
        /// PRD_ProductBonusPrice tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">PRD_ProductBonusPrice Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeletePRD_ProductBonusPrice(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<PRD_ProductBonusPrice>(id);
            }
        }

        /// <summary>
        /// PRD_ProductBonusPrice tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">PRD_ProductBonusPrice Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeletePRD_ProductBonusPrice(PRD_ProductBonusPrice item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<PRD_ProductBonusPrice>(item);
            }
        }

        /// <summary>
        /// PRD_ProductBonusPrice Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">PRD_ProductBonusPrice Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertPRD_ProductBonusPrice(IEnumerable<PRD_ProductBonusPrice> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<PRD_ProductBonusPrice>(item);
            }
        }

        /// <summary>
        /// PRD_ProductBonusPrice Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">PRD_ProductBonusPrice Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdatePRD_ProductBonusPrice(IEnumerable<PRD_ProductBonusPrice> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<PRD_ProductBonusPrice>(item, setNull);
            }
        }

        /// <summary>
        /// PRD_ProductBonusPrice tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">PRD_ProductBonusPrice Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeletePRD_ProductBonusPrice(IEnumerable<PRD_ProductBonusPrice> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<PRD_ProductBonusPrice>(item);
            }
        }

    }
}
