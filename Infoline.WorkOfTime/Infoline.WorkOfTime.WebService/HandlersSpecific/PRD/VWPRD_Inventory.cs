using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService.HandlersSpecific
{
    [Export(typeof(ISmartHandler))]
    public partial class VWPRD_InventoryHandler : BaseSmartHandler
    {
        public VWPRD_InventoryHandler()
            : base("VWPRD_Inventory")
        {

        }

        [HandleFunction("VWPRD_Inventory/GetAll")]
        public void VWPRD_InventoryGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWPRD_Inventory(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPRD_Inventory/GetByCode")]
        public void VWPRD_InventoryGetByCode(HttpContext context)
        {
            try
            {
                var code = context.Request["code"];
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWPRD_InventoryByCode(code);

                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPRD_Inventory/GetBySerialCode")]
        public void VWPRD_InventoryGetSerialCode(HttpContext context)
        {
            try
            {
                var serial = context.Request["serial"];
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWPRD_InventoryBySerialCode(serial);

                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPRD_Inventory/GetBySerialOrCode")]
        public void VWPRD_InventoryGetSerialOrCode(HttpContext context)
        {
            try
            {
                var serial = context.Request["barcode"];
                var db = new WorkOfTimeDatabase();
                var inventory = db.GetVWPRD_InventoryBySerialOrCode(serial);
                if (inventory == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Envanter Bulunamadı" });
                    return;
                }

                var data = new VWPRD_InventoryWithActions().B_EntityDataCopyForMaterial(inventory);
                data.actions = db.GetVWPRD_InventoryActionByInventoryIdOrderByCreatedDesc(data.id);

                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}
