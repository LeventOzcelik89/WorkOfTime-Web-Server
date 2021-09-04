using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class STN_StationMonitorTypeHandler : BaseSmartHandler
    {
        public STN_StationMonitorTypeHandler()
            : base("STN_StationMonitorType")
        {

        }

        [HandleFunction("STN_StationMonitorType/GetAll")]
        public void STN_StationMonitorTypeGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetSTN_StationMonitorType(cond);
            RenderResponse(context, data);
        }

        [HandleFunction("STN_StationMonitorType/GetById")]
        public void STN_StationMonitorTypeGetById(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            var data = db.GetSTN_StationMonitorTypeById(new Guid((string)id));
            RenderResponse(context, data);

        }

        [HandleFunction("STN_StationMonitorType/Insert")]
        public void STN_StationMonitorTypeInsert(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<STN_StationMonitorType>(context);
            RenderResponse(context, db.InsertSTN_StationMonitorType(data));
        }

        [HandleFunction("STN_StationMonitorType/Update")]
        public void STN_StationMonitorTypeUpdate(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<STN_StationMonitorType>(context);
            RenderResponse(context, db.UpdateSTN_StationMonitorType(data));
        }

        [HandleFunction("STN_StationMonitorType/Delete")]
        public void STN_StationMonitorTypeDelete(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            if(id != null)
            {
                RenderResponse(context, db.DeleteSTN_StationMonitorType(new Guid((string)id)));
            }
            else
            {
                var item = ParseRequest<STN_StationMonitorType>(context);
                RenderResponse(context, db.DeleteSTN_StationMonitorType(item));
            }
        }

    }
}
