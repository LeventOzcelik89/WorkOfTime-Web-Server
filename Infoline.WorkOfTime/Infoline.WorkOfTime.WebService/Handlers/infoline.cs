using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class infolineHandler : BaseSmartHandler
    {
        public infolineHandler()
            : base("infoline")
        {

        }

        [HandleFunction("infoline/GetAll")]
        public void infolineGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.Getinfoline(cond);
            RenderResponse(context, data);
        }

        [HandleFunction("infoline/GetById")]
        public void infolineGetById(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            var data = db.GetinfolineById(new Guid((string)id));
            RenderResponse(context, data);

        }

        [HandleFunction("infoline/Insert")]
        public void infolineInsert(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<infoline>(context);
            RenderResponse(context, db.Insertinfoline(data));
        }

        [HandleFunction("infoline/Update")]
        public void infolineUpdate(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<infoline>(context);
            RenderResponse(context, db.Updateinfoline(data));
        }

        [HandleFunction("infoline/Delete")]
        public void infolineDelete(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            if(id != null)
            {
                RenderResponse(context, db.Deleteinfoline(new Guid((string)id)));
            }
            else
            {
                var item = ParseRequest<infoline>(context);
                RenderResponse(context, db.Deleteinfoline(item));
            }
        }

    }
}
