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
        /// VWPRD_InventoryTask tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWPRD_InventoryTask dizi objesini geri döndürür.</returns>
        public VWPRD_InventoryTask[] GetVWPRD_InventoryTask(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_InventoryTask>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWPRD_InventoryTask tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWPRD_InventoryTask dizi objesini geri döndürür.</returns>
        public VWPRD_InventoryTask[] GetVWPRD_InventoryTask(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWPRD_InventoryTask>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWPRD_InventoryTask tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWPRD_InventoryTaskCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWPRD_InventoryTask").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWPRD_InventoryTask tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWPRD_InventoryTask tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWPRD_InventoryTask Objesini geri döndürür.</returns>
        public VWPRD_InventoryTask GetVWPRD_InventoryTaskById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPRD_InventoryTask>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}