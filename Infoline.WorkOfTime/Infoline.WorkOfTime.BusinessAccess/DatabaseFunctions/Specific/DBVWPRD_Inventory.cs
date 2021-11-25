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

        public VWPRD_Inventory GetVWPRD_InventoryByCode(string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_Inventory>().Where(x => x.code == code).Execute().FirstOrDefault();
            }
        }
        public VWPRD_Inventory GetVMPRD_InventoryById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPRD_Inventory>().Where(a => a.id == id).Execute().FirstOrDefault();
            }
        }


        public VWPRD_Inventory[] GetVWPRD_InventoryByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_Inventory>().Where(a => a.lastActionDataCompanyId == companyId).Execute().ToArray();
            }
        }

        public VWPRD_Inventory[] GetVWPRD_InventoryByLastActionDataId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_Inventory>().Where(a => a.lastActionDataId == id).Execute().ToArray();
            }
        }

        public VWPRD_Inventory[] GetVWPRD_InventoryByIds(Guid[] inventoryIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_Inventory>().Where(a => a.id.In(inventoryIds)).Execute().ToArray();
            }
        }


        public string[] GetVWPRD_InventoryByProductIdLastActionId(Guid productId, Guid? lastActionDataCompanyId, Guid? lastActionId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_Inventory>().Where(a => (a.lastActionDataId == lastActionId || a.lastActionDataCompanyId == lastActionDataCompanyId) && a.productId == productId && a.firstActionType != (int)EnumPRD_InventoryActionType.BakimEnvanteri).Select(a => new VWPRD_Inventory { serialcode = a.serialcode }).Execute().Select(a => a.serialcode).ToArray();
            }
        }

        public VWPRD_Inventory[] GetVWPRD_InventoryBySerialCodesAndIds(Guid[] productids, string[] serialCodes, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_Inventory>().Where(x => x.productId.In(productids) && x.serialcode.In(serialCodes)).Execute().ToArray();
            }
        }
        public VWPRD_Inventory[] GetVWPRD_InventoryBySerialCodesAndId(Guid productid, string[] serialCodes, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_Inventory>().Where(x => x.productId == productid && x.serialcode.In(serialCodes)).Execute().ToArray();
            }
        }

        public VWPRD_Inventory GetVWPRD_InventoryBySerialCode(string serialCode, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_Inventory>().Where(x => x.serialcode.Contains(serialCode)).Execute().FirstOrDefault();
            }
        }

        public VWPRD_Inventory GetVWPRD_InventoryBySerialOrCode(string barcode, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_Inventory>().Where(x => x.serialcode == barcode || x.code == barcode).Execute().FirstOrDefault();
            }
        }
    }
}
