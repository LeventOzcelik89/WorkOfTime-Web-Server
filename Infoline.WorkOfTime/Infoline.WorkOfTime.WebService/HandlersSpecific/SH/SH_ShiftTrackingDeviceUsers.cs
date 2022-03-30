using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Web.SmartHandlers;
using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class SH_ShiftTrackingDeviceUsersHandler : BaseSmartHandler
    {
        public SH_ShiftTrackingDeviceUsersHandler()
            : base("SH_ShiftTrackingDeviceUsersHandler")
        {

        }

        [HandleFunction("SH_ShiftTrackingDeviceUsers/GetByDeviceIdAndDeviceUserId")]
        public void SH_ShiftTrackingDeviceUsersByDeviceIdAndDeviceUserId(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                
                var deviceId = context.Request["deviceId"];
                var deviceUserId = context.Request["deviceUserId"];
                var data = db.GetSH_ShiftTrackingDeviceUsersByDeviceIdAndDeviceUserId(Guid.Parse(deviceId), deviceUserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("SH_ShiftTrackingDeviceUsers/Insert")]
        public void InsertSH_ShiftTrackingDeviceUsers(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["token"];
                if(token != "anq5tyqhj1yfbkmre0efr2kw")
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    RenderResponse(context, new ResultStatus { result = false, message = "Yanlış Token" });
                    return;

                }
                var newUser = ParseRequest<SH_ShiftTrackingDeviceUsers>(context);
                var db = new WorkOfTimeDatabase();
                var dbResult = db.InsertSH_ShiftTrackingDeviceUsers(newUser);
                RenderResponse(context, new ResultStatus { result = dbResult.result, message = dbResult.message });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }
    }
}