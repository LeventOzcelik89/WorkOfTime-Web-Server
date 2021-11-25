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

        [HandleFunction("VWPRD_Inventory/GetPageInfo")]
        public void VWPRD_InventoryGetPageInfo(HttpContext context)
        {
            var items = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumPRD_InventoryActionType>().Where(a => Convert.ToInt32(a.Key) >= 0 && Convert.ToInt32(a.Key) < 20);

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
                    isActive = true
                });
            }

            RenderResponse(context, headers);

            //try
            //{
            //    var items = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumPRD_InventoryActionType>().Where(a => Convert.ToInt32(a.Key) >= 0 && Convert.ToInt32(a.Key) < 20);
            //    var model = new PageModel { SearchProperty = "searchField" };


            //    model.Filters.Add(new PageFilter
            //    {
            //        Title = "Son Durumuna Göre",
            //    });

            //    foreach (var item in items)
            //    {
            //        model.Filters[0].Items.Add(new PageFilterItem
            //        {
            //            Title = item.EnumKey.ToString(),
            //            Filter = new BEXP { Operand1 = (COL)"lastActionType", Operator = BinaryOperator.Equal, Operand2 = (VAL)item.Key.ToString() }.GetSerializeObject(),
            //        });
            //    }

            //    RenderResponse(context, model);
            //}
            //catch (Exception ex)
            //{
            //    RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            //}
        }

    }
}
