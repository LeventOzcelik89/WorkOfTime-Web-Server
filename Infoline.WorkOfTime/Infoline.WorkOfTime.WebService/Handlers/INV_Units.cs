using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class INV_UnitsHandler : BaseSmartHandler
    {
        public INV_UnitsHandler()
            : base("INV_Units")
        {

        }

        [HandleFunction("INV_Units/GetAll")]
        public void INV_UnitsGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetINV_Units(cond);
            RenderResponse(context, data);
        }

        [HandleFunction("INV_Units/GetById")]
        public void INV_UnitsGetById(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            var data = db.GetINV_UnitsById(new Guid((string)id));
            RenderResponse(context, data);

        }

        [HandleFunction("INV_Units/Insert")]
        public void INV_UnitsInsert(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<INV_Units>(context);
            RenderResponse(context, db.InsertINV_Units(data));
        }

        [HandleFunction("INV_Units/Update")]
        public void INV_UnitsUpdate(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<INV_Units>(context);
            RenderResponse(context, db.UpdateINV_Units(data));
        }

        [HandleFunction("INV_Units/Delete")]
        public void INV_UnitsDelete(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            if(id != null)
            {
                RenderResponse(context, db.DeleteINV_Units(new Guid((string)id)));
            }
            else
            {
                var item = ParseRequest<INV_Units>(context);
                RenderResponse(context, db.DeleteINV_Units(item));
            }
        }

    }
}
