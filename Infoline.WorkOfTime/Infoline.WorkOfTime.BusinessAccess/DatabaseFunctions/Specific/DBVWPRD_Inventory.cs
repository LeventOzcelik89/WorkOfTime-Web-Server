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

        public SummaryHeadersAdvance GetDBVWPRD_InventorySummaries(IEnumerable<Item> items, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersAdvance();
                headers.headerFilters = new HeadersAdvance();
                headers.headerFilters.title = "Durumuna Göre";
                headers.headerFilters.Filters = new List<HeadersAdvanceItem>();
                foreach (var item in items)
                {
                    headers.headerFilters.Filters.Add(new HeadersAdvanceItem
                    {
                        title = item.EnumKey.ToString(),
                        filter = "{'Filter':{'Operand1':'lastActionType','Operator':'Equal','Operand2':'" + item.Key.ToString() + "'},'Operator':'And'}",
                        count = db.Table<VWPRD_Inventory>().Where(a => a.lastActionType == short.Parse(item.Key)).Count(),
                        isActive = true
                    }); ;
                }

                //headers.headerFilters.Filters.Add(new HeadersTicketItem
                //{
                //    title = "Vazgeçildi",
                //    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'6'},'Operand2':{'Operand1':'requesterId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                //    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 6 && a.requesterId == userId).Count(),
                //    isActive = false
                //});

                return headers;
            }
        }

        public VWPRD_Inventory[] GetVWPRD_InventoryBySerialCodesOrCodes(string[] codes, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_Inventory>().Where(x => x.serialcode.In(codes) || x.code.In(codes)).Execute().ToArray();
            }
        }

    }
}
