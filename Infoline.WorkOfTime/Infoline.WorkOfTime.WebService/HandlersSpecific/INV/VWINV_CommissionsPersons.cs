using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Mobile;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Web.SmartHandlers;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_CommissionsPersonsHandler : BaseSmartHandler
    {
        public VWINV_CommissionsPersonsHandler()
            : base("VWINV_CommissionsPersonsHandler")
        {

        }

        [HandleFunction("VWINV_CommissionsPersons/GetById")]
        public void VWINV_CommissionsPersonsGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = db.GetVWINV_CommissionsPersonsById(new Guid((string)id));
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWINV_CommissionsPersons/GetMyPageInfo")]
        public void VWINV_CommissionsPersonsGetMyPageInfo(HttpContext context)
        {
            try
            {
                var data = new VMINV_CommissionsModel().GetMyCommissionsSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWINV_CommissionsPersons/GetRequestPageInfo")]
        public void VWINV_CommissionsPersonsGetAllPageInfo(HttpContext context)
        {
            try
            {
                var data = new VMINV_CommissionsModel().GetRequestCommissionsSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}