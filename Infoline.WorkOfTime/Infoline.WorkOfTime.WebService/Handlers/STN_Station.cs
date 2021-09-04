using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class STN_StationHandler : BaseSmartHandler
    {
        public STN_StationHandler()
            : base("STN_Station")
        {

        }

        [HandleFunction("STN_Station/GetAll")]
        public void STN_StationGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetSTN_Station(cond);
            RenderResponse(context, data);
        }

        [HandleFunction("STN_Station/GetById")]
        public void STN_StationGetById(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            var data = db.GetSTN_StationById(new Guid((string)id));
            RenderResponse(context, data);

        }

        [HandleFunction("STN_Station/Insert")]
        public void STN_StationInsert(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<STN_Station>(context);
            RenderResponse(context, db.InsertSTN_Station(data));
        }

        [HandleFunction("STN_Station/Update")]
        public void STN_StationUpdate(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<STN_Station>(context);
            RenderResponse(context, db.UpdateSTN_Station(data));
        }

        [HandleFunction("STN_Station/Delete")]
        public void STN_StationDelete(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            if(id != null)
            {
                RenderResponse(context, db.DeleteSTN_Station(new Guid((string)id)));
            }
            else
            {
                var item = ParseRequest<STN_Station>(context);
                RenderResponse(context, db.DeleteSTN_Station(item));
            }
        }

    }
}
