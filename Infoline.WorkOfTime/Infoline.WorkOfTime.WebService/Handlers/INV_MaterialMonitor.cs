using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class INV_MaterialMonitorHandler : BaseSmartHandler
    {
        public INV_MaterialMonitorHandler()
            : base("INV_MaterialMonitor")
        {

        }

        [HandleFunction("INV_MaterialMonitor/GetAll")]
        public void INV_MaterialMonitorGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetINV_MaterialMonitor(cond);
            RenderResponse(context, data);
        }

        [HandleFunction("INV_MaterialMonitor/GetById")]
        public void INV_MaterialMonitorGetById(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            var data = db.GetINV_MaterialMonitorById(new Guid((string)id));
            RenderResponse(context, data);

        }

        [HandleFunction("INV_MaterialMonitor/Insert")]
        public void INV_MaterialMonitorInsert(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<INV_MaterialMonitor>(context);
            RenderResponse(context, db.InsertINV_MaterialMonitor(data));
        }

        [HandleFunction("INV_MaterialMonitor/Update")]
        public void INV_MaterialMonitorUpdate(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<INV_MaterialMonitor>(context);
            RenderResponse(context, db.UpdateINV_MaterialMonitor(data));
        }

        [HandleFunction("INV_MaterialMonitor/Delete")]
        public void INV_MaterialMonitorDelete(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            if(id != null)
            {
                RenderResponse(context, db.DeleteINV_MaterialMonitor(new Guid((string)id)));
            }
            else
            {
                var item = ParseRequest<INV_MaterialMonitor>(context);
                RenderResponse(context, db.DeleteINV_MaterialMonitor(item));
            }
        }

    }
}
