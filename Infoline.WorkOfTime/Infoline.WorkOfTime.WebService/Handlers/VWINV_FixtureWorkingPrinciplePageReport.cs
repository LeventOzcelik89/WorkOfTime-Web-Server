using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_FixtureWorkingPrinciplePageReportHandler : BaseSmartHandler
    {
        public VWINV_FixtureWorkingPrinciplePageReportHandler()
            : base("VWINV_FixtureWorkingPrinciplePageReport")
        {

        }

        [HandleFunction("VWINV_FixtureWorkingPrinciplePageReport/GetAll")]
        public void VWINV_FixtureWorkingPrinciplePageReportGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetVWINV_FixtureWorkingPrinciplePageReport(cond);
            RenderResponse(context, data);
        }

    }
}
