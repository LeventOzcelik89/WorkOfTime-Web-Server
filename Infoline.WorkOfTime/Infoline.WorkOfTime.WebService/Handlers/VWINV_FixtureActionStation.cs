using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_FixtureActionStationHandler : BaseSmartHandler
    {
        public VWINV_FixtureActionStationHandler()
            : base("VWINV_FixtureActionStation")
        {

        }

        [HandleFunction("VWINV_FixtureActionStation/GetAll")]
        public void VWINV_FixtureActionStationGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetVWINV_FixtureActionStation(cond);
            RenderResponse(context, data);
        }

    }
}
