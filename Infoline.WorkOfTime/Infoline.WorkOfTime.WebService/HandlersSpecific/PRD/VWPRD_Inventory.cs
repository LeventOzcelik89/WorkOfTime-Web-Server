using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData.Specific;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
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

        [HandleFunction("VWPRD_InventoryDetail/GetById")]
        public void VWPRD_InventoryDetailGetById(HttpContext context)
        {
            try
            {
                var id = context.Request["id"];
                var data = new VMPRD_InventoryModel().LoadMobile(new Guid(id));

                if (data == null)
                {
                    RenderResponse(context, new ResultStatus() { result = false, message = "id Bulunamadı" });
                }

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

                var data = new VMPRD_InventoryModel().LoadMobile(serial);
                if (data == null)
                {
                    RenderResponse(context, new ResultStatus() { result = false, message = "Envanter Bulunamadı" });
                }

                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPRD_Inventory/GetPageInfo")]
        public void VWPRD_InventoryGetPageInfo(HttpContext context)
        {
            var items = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumPRD_InventoryActionType>().Where(a => Convert.ToInt32(a.Key) >= 0 && Convert.ToInt32(a.Key) < 20);
            var db = new WorkOfTimeDatabase();

            var headers = db.GetDBVWPRD_InventorySummaries(items);
            RenderResponse(context, headers);
        }


        [HandleFunction("VWPRD_Inventory/BarcodeRead")]
        public void VWPRD_InventoryBarcodeRead(HttpContext context)
        {
            try
            {
                var barcode = context.Request["barcode"];
                var db = new WorkOfTimeDatabase();
                var productData = db.GetVWPRD_ProductByCodeOrBarcode(barcode);
                var inventoryData = db.GetVWPRD_InventoryBySerialCodesOrCodes(new string[] { barcode });

                var data = new
                {
                    product = productData,
                    inventories = inventoryData.Select(a => new VMPRD_InventoryModel().LoadMobileForBarcodeRead(a)).ToArray()
                };

                if (data.product == null && data.inventories.Length == 0)
                {
                    RenderResponse(context, new ResultStatus() { result = false, message = "Envanter veya Ürün Bulunamadı" });
                }
                else
                {

                RenderResponse(context, data);
                }

            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}
