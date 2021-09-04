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
        /// DOC_DocumentRevisionRequest tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>DOC_DocumentRevisionRequest dizi objesini geri döndürür.</returns>
        public DOC_DocumentRevisionRequest[] GetDOC_DocumentRevisionRequest(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<DOC_DocumentRevisionRequest>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// DOC_DocumentRevisionRequest tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu DOC_DocumentRevisionRequest dizi objesini geri döndürür.</returns>
        public DOC_DocumentRevisionRequest[] GetDOC_DocumentRevisionRequest(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<DOC_DocumentRevisionRequest>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// DOC_DocumentRevisionRequest tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetDOC_DocumentRevisionRequestCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("DOC_DocumentRevisionRequest").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// DOC_DocumentRevisionRequest tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">DOC_DocumentRevisionRequest tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu DOC_DocumentRevisionRequest Objesini geri döndürür.</returns>
        public DOC_DocumentRevisionRequest GetDOC_DocumentRevisionRequestById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<DOC_DocumentRevisionRequest>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// DOC_DocumentRevisionRequest Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">DOC_DocumentRevisionRequest Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertDOC_DocumentRevisionRequest(DOC_DocumentRevisionRequest item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<DOC_DocumentRevisionRequest>(item);
            }
        }

        /// <summary>
        /// DOC_DocumentRevisionRequest Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">DOC_DocumentRevisionRequest Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdateDOC_DocumentRevisionRequest(DOC_DocumentRevisionRequest item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<DOC_DocumentRevisionRequest>(item, setNull);
            }
        }

        /// <summary>
        /// DOC_DocumentRevisionRequest tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">DOC_DocumentRevisionRequest Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteDOC_DocumentRevisionRequest(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<DOC_DocumentRevisionRequest>(id);
            }
        }

        /// <summary>
        /// DOC_DocumentRevisionRequest tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">DOC_DocumentRevisionRequest Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeleteDOC_DocumentRevisionRequest(DOC_DocumentRevisionRequest item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<DOC_DocumentRevisionRequest>(item);
            }
        }

        /// <summary>
        /// DOC_DocumentRevisionRequest Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">DOC_DocumentRevisionRequest Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertDOC_DocumentRevisionRequest(IEnumerable<DOC_DocumentRevisionRequest> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<DOC_DocumentRevisionRequest>(item);
            }
        }

        /// <summary>
        /// DOC_DocumentRevisionRequest Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">DOC_DocumentRevisionRequest Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdateDOC_DocumentRevisionRequest(IEnumerable<DOC_DocumentRevisionRequest> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<DOC_DocumentRevisionRequest>(item, setNull);
            }
        }

        /// <summary>
        /// DOC_DocumentRevisionRequest tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">DOC_DocumentRevisionRequest Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeleteDOC_DocumentRevisionRequest(IEnumerable<DOC_DocumentRevisionRequest> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<DOC_DocumentRevisionRequest>(item);
            }
        }

    }
}
