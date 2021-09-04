using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_MaterialMonitorHandler : BaseSmartHandler
    {
        public VWINV_MaterialMonitorHandler()
            : base("VWINV_MaterialMonitor")
        {

        }

        [HandleFunction("VWINV_MaterialMonitor/GetAll")]
        public void VWINV_MaterialMonitorGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetVWINV_MaterialMonitor(cond);
            RenderResponse(context, data);
        }

    }
}
