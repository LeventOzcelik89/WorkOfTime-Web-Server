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
        /// VWINV_CompanyPersonAssessment tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWINV_CompanyPersonAssessment dizi objesini geri döndürür.</returns>
        public VWINV_CompanyPersonAssessment[] GetVWINV_CompanyPersonAssessment(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonAssessment>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWINV_CompanyPersonAssessment tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWINV_CompanyPersonAssessment dizi objesini geri döndürür.</returns>
        public VWINV_CompanyPersonAssessment[] GetVWINV_CompanyPersonAssessment(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWINV_CompanyPersonAssessment>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWINV_CompanyPersonAssessment tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWINV_CompanyPersonAssessmentCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWINV_CompanyPersonAssessment").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWINV_CompanyPersonAssessment tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWINV_CompanyPersonAssessment tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWINV_CompanyPersonAssessment Objesini geri döndürür.</returns>
        public VWINV_CompanyPersonAssessment GetVWINV_CompanyPersonAssessmentById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWINV_CompanyPersonAssessment>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}