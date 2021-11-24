using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {

        public VWPRD_InventoryAction[] GetVWPRD_InventoryActionByInventoryIdOrderByCreatedDesc(Guid inventoryId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_InventoryAction>().Where(x => x.inventoryId == inventoryId).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }
    }
}
