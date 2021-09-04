using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_FixtureWarrantyHandler : BaseSmartHandler
    {
        public VWINV_FixtureWarrantyHandler()
            : base("VWINV_FixtureWarranty")
        {

        }

        [HandleFunction("VWINV_FixtureWarranty/GetAll")]
        public void VWINV_FixtureWarrantyGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetVWINV_FixtureWarranty(cond);
            RenderResponse(context, data);
        }

    }
}
