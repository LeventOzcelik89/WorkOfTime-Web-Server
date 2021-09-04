using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_FixtureWorkingPrincipleHandler : BaseSmartHandler
    {
        public VWINV_FixtureWorkingPrincipleHandler()
            : base("VWINV_FixtureWorkingPrinciple")
        {

        }

        [HandleFunction("VWINV_FixtureWorkingPrinciple/GetAll")]
        public void VWINV_FixtureWorkingPrincipleGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetVWINV_FixtureWorkingPrinciple(cond);
            RenderResponse(context, data);
        }

    }
}
