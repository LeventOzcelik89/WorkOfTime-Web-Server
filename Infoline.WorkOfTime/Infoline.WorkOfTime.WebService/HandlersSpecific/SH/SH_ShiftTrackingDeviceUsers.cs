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

        [HandleFunction("SH_ShiftTrackingDeviceUsers/GetByDeviceId")]
        public void SH_ShiftTrackingDeviceUsersByDeviceId(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();

                var deviceId = context.Request["deviceId"];
                var data = db.GetSH_ShiftTrackingDeviceUsersByDeviceId(Guid.Parse(deviceId));
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
                var newUserList = ParseRequest<List<SH_ShiftTrackingDeviceUsers>>(context);
                if(newUserList == null || newUserList.Count() == 0)
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.NoContent;
                    RenderResponse(context, new ResultStatus { result = false, message = "No Object" });
                    return;

                }
                var db = new WorkOfTimeDatabase();
                var dbResult = db.BulkInsertSH_ShiftTrackingDeviceUsers(newUserList);
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