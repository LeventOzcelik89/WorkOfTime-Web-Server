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
        /// VWHDM_TicketRequester tablosundan tüm kayıtları çeken fonksiyondur
        /// </summary>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>VWHDM_TicketRequester dizi objesini geri döndürür.</returns>
        public VWHDM_TicketRequester[] GetVWHDM_TicketRequester(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWHDM_TicketRequester>().OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        /// <summary>
        /// VWHDM_TicketRequester tablosundan simpleQuery objesi filtresi sonucunda gelen kayıtları çeken fonksiyondur. Sayfa giridnde kullanılmaktadır.
        /// </summary>
        /// <param name="simpleQuery">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
         /// <returns>Filtre Sonucu VWHDM_TicketRequester dizi objesini geri döndürür.</returns>
        public VWHDM_TicketRequester[] GetVWHDM_TicketRequester(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWHDM_TicketRequester>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        /// <summary>
        /// VWHDM_TicketRequester tablosundan BEXP Objesi filtresi sonucunda gelen kayıtların toplam adedini veren fonksiyondur.
        /// </summary>
        /// <param name="conditionExpression">Filtre parametreleri olarak obje doldurularak gönderilir.</param>
        /// <returns>Filtre Sonucu Tablo adedini döndürür, sayı(int) olarak.</returns>
        public int GetVWHDM_TicketRequesterCount(BEXP conditionExpression)
        {
            using (var db = GetDB())
            {
                return db.Table("VWHDM_TicketRequester").Where(conditionExpression).Count();
            }
        }
        /// <summary>
        /// VWHDM_TicketRequester tablosundan, id si gönderilen kaydın bilgilerini döndüren fonksiyondur.
        /// </summary>
        /// <param name="id">VWHDM_TicketRequester tablo id'si verilir.</param>
        /// <param name="tran">Mevcut dışında farklı bir transection kullanılacak ise bu parametreye gönderilir.</param>
        /// <returns>Filtre Sonucu VWHDM_TicketRequester Objesini geri döndürür.</returns>
        public VWHDM_TicketRequester GetVWHDM_TicketRequesterById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWHDM_TicketRequester>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }

    }
}