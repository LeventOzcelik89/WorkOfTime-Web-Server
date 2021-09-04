using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;
using System.Collections.Generic;
namespace Infoline.WorkOfTime.BusinessAccess
{

	partial class WorkOfTimeDatabase
	{


        public FTM_TaskFormRelation[] GetVWFTM_TaskFormRelationByInventoryId(Guid? inventoryId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_TaskFormRelation>().Where(a => a.inventoryId == inventoryId).Select(a => new FTM_TaskFormRelation { formId = a.formId }).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public FTM_TaskFormRelation[] GetVWFTM_TaskFormRelationByStorageId(Guid? storageId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_TaskFormRelation>().Where(a => a.companyStorageId == storageId).Select(a => new FTM_TaskFormRelation { formId = a.formId }).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public FTM_TaskFormRelation[] GetVWFTM_TaskFormRelationByCompanyId(Guid? companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWFTM_TaskFormRelation>().Where(a => a.companyId == companyId).Select(a => new FTM_TaskFormRelation { formId = a.formId }).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
    }
}
