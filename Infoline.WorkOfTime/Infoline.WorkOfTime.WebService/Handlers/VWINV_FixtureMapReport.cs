using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_FixtureMapReportHandler : BaseSmartHandler
    {
        public VWINV_FixtureMapReportHandler()
            : base("VWINV_FixtureMapReport")
        {

        }

        [HandleFunction("VWINV_FixtureMapReport/GetAll")]
        public void VWINV_FixtureMapReportGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetVWINV_FixtureMapReport(cond);
            RenderResponse(context, data);
        }

    }
}
