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
        /// VWCRM_MonthlyTarget tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWCRM_MonthlyTarget dizi objesini geri döndürür.</returns>
        public VWCRM_MonthlyTarget[] GetVWCRM_MonthlyTarget(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_MonthlyTarget>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWCRM_MonthlyTarget tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWCRM_MonthlyTarget dizi objesini geri döndürür.</returns>
        public VWCRM_MonthlyTarget[] GetVWCRM_MonthlyTarget(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWCRM_MonthlyTarget>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWCRM_MonthlyTarget tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWCRM_MonthlyTargetCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWCRM_MonthlyTarget").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWCRM_MonthlyTarget tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWCRM_MonthlyTarget tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWCRM_MonthlyTarget Objesini geri döndürür.</returns>
        public VWCRM_MonthlyTarget GetVWCRM_MonthlyTargetById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCRM_MonthlyTarget>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}