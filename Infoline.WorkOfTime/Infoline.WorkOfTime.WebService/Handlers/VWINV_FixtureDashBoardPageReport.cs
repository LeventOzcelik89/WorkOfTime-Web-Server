using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_FixtureDashBoardPageReportHandler : BaseSmartHandler
    {
        public VWINV_FixtureDashBoardPageReportHandler()
            : base("VWINV_FixtureDashBoardPageReport")
        {

        }

        [HandleFunction("VWINV_FixtureDashBoardPageReport/GetAll")]
        public void VWINV_FixtureDashBoardPageReportGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetVWINV_FixtureDashBoardPageReport(cond);
            RenderResponse(context, data);
        }

    }
}
