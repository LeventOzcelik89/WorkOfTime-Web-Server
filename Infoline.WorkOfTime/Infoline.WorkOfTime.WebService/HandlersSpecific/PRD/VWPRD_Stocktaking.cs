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

        [HandleFunction("VWPRD_Stocktaking/GetById")]
        public void VWPRD_StocktakingGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var item = new VMPRD_StocktakingModel { id = Guid.Parse(id) }.Load();

                if (item == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Sayım kaydı bulunamadı." });
                    return;
                }

                RenderResponse(context, item);
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
                string[] barcodes = context.Request["barcode"].Split(',');

                if(barcodes.Length == 0)
                {
                    RenderResponse(context, new ResultStatus() { result = false, message = "Lütfen Kod Giriniz" });
                }


                var productData = db.GetVWPRD_ProductByCode(barcodes[0]);
                if (productData != null)
                {
                    RenderResponse(context, new { stocktakingItemType = 0, data = productData });
                }
                else
                {
                    var inventoryData = db.GetVWPRD_InventoryBySerialCodesOrCodes(barcodes);
                    if (inventoryData.Length > 0)
                    {
                        RenderResponse(context, new { stocktakingItemType = 1, data = inventoryData });
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

        [HandleFunction("VWPRD_Stocktaking/GetPageInfo")]
        public void VWPRD_StocktakingGetPageInfo(HttpContext context)
        {
            try
            {
                var data = new VMPRD_StocktakingModel().GetPageInfo(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

    }
}