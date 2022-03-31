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
    public partial class VWPRD_StocktakingItem : BaseSmartHandler
    {
        public VWPRD_StocktakingItem()
            : base("VWPRD_StocktakingItem")
        {

        }

        [HandleFunction("VWPRD_StocktakingItem/Insert")]
        public void VWPRD_StocktakingItemInsert(HttpContext context)
        {
            try
            {
                var item = ParseRequest<VMPRD_StocktakingItemModel>(context);

                if (item == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Sayım kalemi nesnesi boş gönderilemez." });
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

        [HandleFunction("VWPRD_StocktakingItem/Delete")]
        public void VWPRD_StocktakingItemDelete(HttpContext context)
        {
            try
            {
                var id = context.Request["id"];
                var res = new VMPRD_StocktakingItemModel { id = Guid.Parse(id) }.Delete();

                RenderResponse(context, res);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

    }
}