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
        public VWPRD_ProductUnit[] GetVWPRD_ProductUnitByProductId(Guid productId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPRD_ProductUnit>().Where(a => a.productId == productId).Execute().ToArray();
            }
        }
    }
}