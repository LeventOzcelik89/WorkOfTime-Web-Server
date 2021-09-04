using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_PermitTypePageReportHandler : BaseSmartHandler
    {
        public VWINV_PermitTypePageReportHandler()
            : base("VWINV_PermitTypePageReport")
        {

        }

        [HandleFunction("VWINV_PermitTypePageReport/GetAll")]
        public void VWINV_PermitTypePageReportGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetVWINV_PermitTypePageReport(cond);
            RenderResponse(context, data);
        }

    }
}
