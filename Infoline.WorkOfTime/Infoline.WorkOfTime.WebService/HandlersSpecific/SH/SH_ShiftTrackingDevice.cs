using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Web.SmartHandlers;
using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Linq;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class SH_ShiftTrackingDeviceHandler : BaseSmartHandler
    {
        public SH_ShiftTrackingDeviceHandler()
            : base("SH_ShiftTrackingDeviceHandler")
        {

        }

        [HandleFunction("SH_ShiftTrackingDevice/GetAll")]
        public void SH_ShiftTrackingDeviceGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetSH_ShiftTrackingDevice(cond).OrderByDescending((a)=> a.created);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}