using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_FixtureActionServiceHandler : BaseSmartHandler
    {
        public VWINV_FixtureActionServiceHandler()
            : base("VWINV_FixtureActionService")
        {

        }

        [HandleFunction("VWINV_FixtureActionService/GetAll")]
        public void VWINV_FixtureActionServiceGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetVWINV_FixtureActionService(cond);
            RenderResponse(context, data);
        }

    }
}
