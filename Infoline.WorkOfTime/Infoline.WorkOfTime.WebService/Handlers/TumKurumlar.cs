using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.ProjectManagement.BusinessAccess;using Infoline.ProjectManagement.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class TumKurumlarHandler : BaseSmartHandler
    {
        public TumKurumlarHandler()
            : base("TumKurumlar")
        {

        }

        [HandleFunction("TumKurumlar/GetAll")]
        public void TumKurumlarGetAll(HttpContext context)
        {
            var c = ParseRequest<Condition>(context);
            var cond = c != null ? CondtionToQuery.Convert(c): new SimpleQuery();
            var db = new ProjectManagementDatabase();
            var data = db.GetTumKurumlar(cond);
            RenderResponse(context, data);
        }

        [HandleFunction("TumKurumlar/GetById")]
        public void TumKurumlarGetById(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            var data = db.GetTumKurumlarById(new Guid((string)id));
            RenderResponse(context, data);

        }

        [HandleFunction("TumKurumlar/Insert")]
        public void TumKurumlarInsert(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<TumKurumlar>(context);
            RenderResponse(context, db.InsertTumKurumlar(data));
        }

        [HandleFunction("TumKurumlar/Update")]
        public void TumKurumlarUpdate(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var data = ParseRequest<TumKurumlar>(context);
            RenderResponse(context, db.UpdateTumKurumlar(data));
        }

        [HandleFunction("TumKurumlar/Delete")]
        public void TumKurumlarDelete(HttpContext context)
        {
            var db = new ProjectManagementDatabase();
            var id = context.Request["id"];
            if(id != null)
            {
                RenderResponse(context, db.DeleteTumKurumlar(new Guid((string)id)));
            }
            else
            {
                var item = ParseRequest<TumKurumlar>(context);
                RenderResponse(context, db.DeleteTumKurumlar(item));
            }
        }

    }
}
