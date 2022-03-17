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
    public partial class VWPRD_Stocktaking : BaseSmartHandler
    {
        public VWPRD_Stocktaking()
            : base("VWPRD_Stocktaking")
        {

        }

        [HandleFunction("VWPRD_Stocktaking/GetAll")]
        public void VWPRD_StocktakingGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWPRD_Stocktaking(cond);
                var list = new List<VMPRD_StocktakingModel>();
                foreach (var stocktaking in data)
                {
                    var items = db.GetVWPRD_StocktakingItemByStocktakingId(stocktaking.id);
                    var item = new VMPRD_StocktakingModel { id = stocktaking.id }.Load();
                    item.items = items;
                    list.Add(item);
                }
                RenderResponse(context, list);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPRD_Stocktaking/Insert")]
        public void VWPRD_StocktakingInsert(HttpContext context)
        {
            try
            {
                var item = ParseRequest<VMPRD_StocktakingModel>(context);

                if (item == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Sayım nesnesi boş gönderilemez." });
                    return;
                }

                var res = item.Save(CallContext.Current.UserId);
                RenderResponse(context, res);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPRD_Stocktaking/BarcodeRead")]
        public void VWPRD_StocktakingBarcodeRead(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var barcode = context.Request["barcode"];
                var productData = db.GetVWPRD_ProductByCode(barcode);
                if (productData != null && productData.Length > 0)
                {
                    RenderResponse(context, productData);
                }
                else
                {
                    var inventoryData = db.GetVWPRD_InventoryBySerialOrCode(barcode);
                    if (inventoryData != null)
                    {
                        RenderResponse(context, inventoryData);
                    }
                    else
                    {
                        RenderResponse(context, new ResultStatus() { result = false, message = "Ürün Bulunamadı" });
                    }
                }

            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

    }
}