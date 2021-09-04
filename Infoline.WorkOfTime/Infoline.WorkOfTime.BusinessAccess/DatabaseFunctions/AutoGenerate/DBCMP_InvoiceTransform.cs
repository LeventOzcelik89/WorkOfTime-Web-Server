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
        /// CMP_InvoiceTransform tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>CMP_InvoiceTransform dizi objesini geri döndürür.</returns>
        public CMP_InvoiceTransform[] GetCMP_InvoiceTransform(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_InvoiceTransform>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// CMP_InvoiceTransform tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu CMP_InvoiceTransform dizi objesini geri döndürür.</returns>
        public CMP_InvoiceTransform[] GetCMP_InvoiceTransform(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<CMP_InvoiceTransform>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// CMP_InvoiceTransform tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetCMP_InvoiceTransformCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("CMP_InvoiceTransform").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// CMP_InvoiceTransform tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">CMP_InvoiceTransform tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu CMP_InvoiceTransform Objesini geri döndürür.</returns>
        public CMP_InvoiceTransform GetCMP_InvoiceTransformById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_InvoiceTransform>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// CMP_InvoiceTransform Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_InvoiceTransform Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertCMP_InvoiceTransform(CMP_InvoiceTransform item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<CMP_InvoiceTransform>(item);
            }
        }

        /// <summary>
        /// CMP_InvoiceTransform Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_InvoiceTransform Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateCMP_InvoiceTransform(CMP_InvoiceTransform item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<CMP_InvoiceTransform>(item, setNull);
            }
        }

        /// <summary>
        /// CMP_InvoiceTransform tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">CMP_InvoiceTransform Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteCMP_InvoiceTransform(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<CMP_InvoiceTransform>(id);
            }
        }

        /// <summary>
        /// CMP_InvoiceTransform tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_InvoiceTransform Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteCMP_InvoiceTransform(CMP_InvoiceTransform item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<CMP_InvoiceTransform>(item);
            }
        }

        /// <summary>
        /// CMP_InvoiceTransform Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_InvoiceTransform Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertCMP_InvoiceTransform(IEnumerable<CMP_InvoiceTransform> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<CMP_InvoiceTransform>(item);
            }
        }

        /// <summary>
        /// CMP_InvoiceTransform Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_InvoiceTransform Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateCMP_InvoiceTransform(IEnumerable<CMP_InvoiceTransform> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<CMP_InvoiceTransform>(item, setNull);
            }
        }

        /// <summary>
        /// CMP_InvoiceTransform tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">CMP_InvoiceTransform Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteCMP_InvoiceTransform(IEnumerable<CMP_InvoiceTransform> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<CMP_InvoiceTransform>(item);
            }
        }

    }
}
