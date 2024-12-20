﻿using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;


namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public PRD_ProductionUser[] GetPRD_ProductionUsersByProductionId(Guid productionId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductionUser>().Where(x => x.productionId == productionId).Execute().ToArray();
            }
        }
        public PRD_ProductionUser GetPRD_ProductionUserByProductionIdAndUserId(Guid productionId,Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductionUser>().Where(x => x.productionId == productionId&&x.userId==userId).Execute().FirstOrDefault();
            }
        }
        public VWPRD_ProductionUser[] GetVWPRD_ProductionUsersByProductionId(Guid productionId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_ProductionUser>().Where(x => x.productionId == productionId).Execute().ToArray();
            }
        }
    }
}
