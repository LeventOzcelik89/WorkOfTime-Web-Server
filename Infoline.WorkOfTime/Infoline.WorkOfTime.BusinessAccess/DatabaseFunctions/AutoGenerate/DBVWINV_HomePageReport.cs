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
        /// VWINV_HomePageReport tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWINV_HomePageReport dizi objesini geri döndürür.</returns>
        public VWINV_HomePageReport[] GetVWINV_HomePageReport(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_HomePageReport>().Execute().ToArray();
            }
        }

        /// <summary>
        /// VWINV_HomePageReport tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWINV_HomePageReport dizi objesini geri döndürür.</returns>
        public VWINV_HomePageReport[] GetVWINV_HomePageReport(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWINV_HomePageReport>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWINV_HomePageReport tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWINV_HomePageReportCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWINV_HomePageReport").Where(conditionExpression).Count();
            }
        }
    }
}