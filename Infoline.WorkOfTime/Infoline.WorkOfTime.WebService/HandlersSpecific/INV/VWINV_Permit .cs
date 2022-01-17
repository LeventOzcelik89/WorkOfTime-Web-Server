using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Mobile;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_PermitHandler : BaseSmartHandler
    {
        public VWINV_PermitHandler()
            : base("VWINV_PermitHandler")
        {

        }

        [HandleFunction("VWINV_Permit/GetById")]
        public void VWINV_PermitGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = db.GetVWINV_PermitById(new Guid((string)id));
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWINV_Permit/GetMyPageInfo")]
        public void VWINV_PermitGetMyPageInfo(HttpContext context)
        {
            try
            {
                var data = new VMINV_PermitModel().GetMyPermitSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWINV_Permit/GetRequestPageInfo")]
        public void VWINV_PermitGetAllPageInfo(HttpContext context)
        {
            try
            {
                var data = new VMINV_PermitModel().GetRequestPermitSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}