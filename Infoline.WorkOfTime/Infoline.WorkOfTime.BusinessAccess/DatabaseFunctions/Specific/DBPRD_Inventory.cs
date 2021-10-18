using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [BusinessAccess.EnumInfo(typeof(PRD_Inventory), "type")]
    public enum EnumPRD_InventoryType
    {
        //Konum Hareketleri
        [Description("Söküm Envanteri")]
        Sokum = 0,
        [Description("Diğer")]
        Diger = 1,
    }

    partial class WorkOfTimeDatabase
    {

        public int GetPRD_InventoryByProductIdCount(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_Inventory>().Where(x => x.productId == id).Count();
            }
        }

        public PRD_Inventory GetPRD_InventoryByCode(string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_Inventory>().Where(x => x.code == code).Execute().FirstOrDefault();
            }
        }
        public PRD_Inventory[] GetPRD_InventoryByIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_Inventory>().Where(x => x.id.In(ids)).Execute().ToArray();
            }
        }

        public PRD_Inventory[] GetPRD_InventoriesByProductId(Guid productId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_Inventory>().Where(x => x.productId == productId).Execute().ToArray();
            }
        }

        /// <summary>
        /// Bu Method Omix 1194 Tenant, Envanter ve IMEI numarası Eşleştirilmesi için kullanılacaktır. Başka İşlem İçin Kullanmayınız...
        /// </summary>
        /// <param name="serialCode"></param>
        /// <param name="imei1"></param>
        /// <param name="imei2"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public VWPRD_Inventory GetPRD_InventoryBySerialCodeOrImei(string serialCode, string imei1, string imei2, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var inventory = new VWPRD_Inventory();
                var inventoryList = db.Table<VWPRD_Inventory>().Where(x => x.serialcode == serialCode || x.serialcode == imei1 || x.serialcode == imei2).Execute().ToArray();
                if (inventoryList != null && inventoryList.Count() > 0)
                {
                    inventory = inventoryList.Where(a => a.productId_Title.Contains("MP")).FirstOrDefault();
                    if (inventory == null)
                        inventory = inventoryList.FirstOrDefault();
                }
                else
                {
                    inventory = inventoryList.FirstOrDefault();
                }
                return inventory;
            }
        }

        public bool GetPRD_InventoryExistBySerialCode(Guid productId, Guid inventoryId, string serialCode, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_Inventory>().Where(x => x.productId == productId && x.id != inventoryId && x.serialcode == serialCode).Count() > 0;
            }
        }

    }
}
