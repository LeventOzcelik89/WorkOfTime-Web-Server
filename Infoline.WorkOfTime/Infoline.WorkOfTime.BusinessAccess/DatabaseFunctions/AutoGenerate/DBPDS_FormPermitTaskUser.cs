﻿using System;
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
        /// PDS_FormPermitTaskUser tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>PDS_FormPermitTaskUser dizi objesini geri döndürür.</returns>
        public PDS_FormPermitTaskUser[] GetPDS_FormPermitTaskUser(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PDS_FormPermitTaskUser>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// PDS_FormPermitTaskUser tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu PDS_FormPermitTaskUser dizi objesini geri döndürür.</returns>
        public PDS_FormPermitTaskUser[] GetPDS_FormPermitTaskUser(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<PDS_FormPermitTaskUser>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// PDS_FormPermitTaskUser tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetPDS_FormPermitTaskUserCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("PDS_FormPermitTaskUser").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// PDS_FormPermitTaskUser tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">PDS_FormPermitTaskUser tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu PDS_FormPermitTaskUser Objesini geri döndürür.</returns>
        public PDS_FormPermitTaskUser GetPDS_FormPermitTaskUserById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PDS_FormPermitTaskUser>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

        /// <summary>
        /// PDS_FormPermitTaskUser Tablosuna Kayıt Atan Fonksiyondur.
        /// </summary>
        /// <param name="item">PDS_FormPermitTaskUser Objesidir. Insert Yapılacak Propertiler Eklenir</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Insert İşlemin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus InsertPDS_FormPermitTaskUser(PDS_FormPermitTaskUser item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteInsert<PDS_FormPermitTaskUser>(item);
            }
        }

        /// <summary>
        /// PDS_FormPermitTaskUser Tablosuna Günceleme Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">PDS_FormPermitTaskUser Objesidir. :Update Yapılacak Propertiler Eklenir</param>
        /// <param name="setNull">Boş bırakılan propertiler null olarak mı setlensin bilgisi setlenir</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus UpdatePDS_FormPermitTaskUser(PDS_FormPermitTaskUser item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteUpdate<PDS_FormPermitTaskUser>(item, setNull);
            }
        }

        /// <summary>
        /// PDS_FormPermitTaskUser tablosundan, Verilen kayıt id'si ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="id">PDS_FormPermitTaskUser Tablo id'si</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeletePDS_FormPermitTaskUser(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<PDS_FormPermitTaskUser>(id);
            }
        }

        /// <summary>
        /// PDS_FormPermitTaskUser tablosundan, Verilen objeler ile Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">PDS_FormPermitTaskUser Objesidir. Silme İşlemi Yapılacak Propertiler Eklenir, Eklenenkere Göre Filtrelenerek Silme işlemi yapılır.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Silme İşleminin Durumunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus DeletePDS_FormPermitTaskUser(PDS_FormPermitTaskUser item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteDelete<PDS_FormPermitTaskUser>(item);
            }
        }

        /// <summary>
        /// PDS_FormPermitTaskUser Tablosuna Toplu insert işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">PDS_FormPermitTaskUser Dizisi Gönderilerek insert işlemi yapılacak dizi objesi gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. insert işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkInsertPDS_FormPermitTaskUser(IEnumerable<PDS_FormPermitTaskUser> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkInsert<PDS_FormPermitTaskUser>(item);
            }
        }

        /// <summary>
        /// PDS_FormPermitTaskUser Tablosuna Toplu Update işlemi yapan fonksiyondur.
        /// </summary>
        /// <param name="item">PDS_FormPermitTaskUser Dizisi Gönderilerek Update İşlemi Yapılacak Dizi Objesi Gönderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Update İşlemlerinin Sonucunu ve Başarılı Mesajını Geri Döndürür.</returns>
        public ResultStatus BulkUpdatePDS_FormPermitTaskUser(IEnumerable<PDS_FormPermitTaskUser> item, bool setNull = false, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkUpdate<PDS_FormPermitTaskUser>(item, setNull);
            }
        }

        /// <summary>
        /// PDS_FormPermitTaskUser tablosundan, Verilen Object List ile Toplu Silme İşlemi Yapan Fonksiyondur.
        /// </summary>
        /// <param name="item">PDS_FormPermitTaskUser Dizisi Gönderilerek Delete işlemi yapılacak dizi objesi göderilir.</param>
        /// <param name="tran">Mevcut Dışında Farklı Bir Transection Kullanılacak ise Bu Parametreye Gönderilir.</param>
        /// <returns>ResultStatus Objesi Geri Döndürür. Delete işlemlerinin sonucunu ve başarılı mesajını geri döndürür.</returns>
        public ResultStatus BulkDeletePDS_FormPermitTaskUser(IEnumerable<PDS_FormPermitTaskUser> item, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteBulkDelete<PDS_FormPermitTaskUser>(item);
            }
        }

    }
}