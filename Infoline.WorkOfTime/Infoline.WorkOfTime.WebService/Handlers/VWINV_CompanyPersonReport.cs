using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_CompanyPersonReportHandler : BaseSmartHandler
    {
        public VWINV_CompanyPersonReportHandler()
            : base("VWINV_CompanyPersonReport")
        {

        }

        [HandleFunction("VWINV_CompanyPersonReport/GetAll")]
        public void VWINV_CompanyPersonReportGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetVWINV_CompanyPersonReport(cond);
            RenderResponse(context, data);
        }

    }
}
