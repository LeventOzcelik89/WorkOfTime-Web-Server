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
        public VWPRD_Product[] GetVWPRD_ProductByProductIds(Guid[] productIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPRD_Product>().Where(a => a.id.In(productIds)).Execute().ToArray();
            }
        }
        public VWPRD_Product[] GetVWPRD_ProductByIds(Guid[] ids)
        {

            using (var db = GetDB())
            {
                return db.Table<VWPRD_Product>().Where(a => a.id.In(ids)).Execute().ToArray();
            }
        }



    }
}