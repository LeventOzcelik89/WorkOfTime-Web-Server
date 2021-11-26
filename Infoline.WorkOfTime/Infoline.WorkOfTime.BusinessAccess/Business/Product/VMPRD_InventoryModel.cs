using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPRD_InventoryModel : VWPRD_Inventory
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPRD_InventoryAction[] actions { get; set; }
        public VWFTM_Task[] tasks { get; set; }
        public VWPRD_InventoryTask[] inventoryTasks { get; set; }
        public FTM_TaskFormRelation[] taskFormRelations { get; set; }
        public VWSYS_TableAdditionalProperty[] tableAdditionalProperties { get; set; }
        public VMPRD_InventoryModel LoadMobile(Guid? id, Guid? userId)
        {
            if (id.HasValue)
            {
                this.db = this.db ?? new WorkOfTimeDatabase();

                this.id = id.Value;
                var thisItem = db.GetVMPRD_InventoryById(this.id);
                if (thisItem == null)
                {
                    return null;
                }

                return LoadModelReturn(thisItem, userId);
            }

            return null;
        }

        public VMPRD_InventoryModel LoadMobile(string barcode, Guid? userId)
        {
            if (barcode != null)
            {
                this.db = this.db ?? new WorkOfTimeDatabase();

                var thisItem = db.GetVWPRD_InventoryBySerialOrCode(barcode);
                if (thisItem == null)
                {
                    return null;
                }

                return LoadModelReturn(thisItem, userId);
            }

            return null;
        }

        private VMPRD_InventoryModel LoadModelReturn(VWPRD_Inventory thisItem, Guid? userId)
        {
            this.B_EntityDataCopyForMaterial(thisItem);

            var actions = db.GetVWPRD_InventoryActionByInventoryIdOrderByCreatedDesc(this.id);
            var inventoryTasks = db.GetVWPRD_InventoryTaskByInventoryIdOrderByCreatedDesc(this.id);
            var taskFormRelations = db.GetVWFTM_TaskFormRelationByInventoryId(this.id);
            var tableAdditionalProperties = db.GetVWSYS_TableAdditionalPropertyByDataIdAndDataTable(this.id, "PRD_Inventory");

            if (actions.Count() > 0)
            {
                this.actions = actions;
            }

            if (inventoryTasks.Count() > 0)
            {
                this.inventoryTasks = inventoryTasks;
            }

            if (taskFormRelations.Count() > 0)
            {
                this.taskFormRelations = taskFormRelations;
            }

            if (tableAdditionalProperties.Count() > 0)
            {
                this.tableAdditionalProperties = tableAdditionalProperties;
            }

            if (userId != null)
            {
                var tasks = db.GetVWFTM_TaskByFixtureId(this.id, new Guid(userId.ToString()));
                if (tasks.Count() > 0)
                {
                    this.tasks = tasks;
                }

            }

            return this;
        }
    }

    public class VMPRD_InventoryLog : VWPRD_Inventory
    {
        public IOT_CameraLog[] CameraLogs { get; set; }
    }

    public class DetailInventoryModel
    {
        public Guid? ProductId { get; set; }
        public Guid? StockId { get; set; }
    }

    public class QrClass
    {
        public int? height { get; set; }
        public int? width { get; set; }
        public string logo { get; set; }
        public string phone { get; set; }
        public string fax { get; set; }
        public string weburl { get; set; }
        public InventoryFilterClass[] invertorys { get; set; }
        public InventoryFilterClass invertory { get; set; }
    }

    public class InventoryFilterClass
    {
        public Guid id { get; set; }
        public string serialcode { get; set; }
        public string code { get; set; }
        public string fullName { get; set; }
        public string productId_Title { get; set; }
    }

    public class VWPRD_InventoryWithActions : VWPRD_Inventory
    {
        public VWPRD_InventoryAction[] actions { get; set; }
    }
    public class VWPRD_InventoryDetail : VWPRD_Inventory
    {
        public VWPRD_InventoryAction[] actions { get; set; }
    }
}
