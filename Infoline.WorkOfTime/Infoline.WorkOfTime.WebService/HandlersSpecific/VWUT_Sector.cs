using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;using Infoline.WorkOfTime.BusinessData;using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService.HandlersSpecific
{
    [Export(typeof(ISmartHandler))]
    public partial class VWUT_SectorHandler : BaseSmartHandler
    {
        public VWUT_SectorHandler()
            : base("VWUT_Sector")
        {

        }
       
        [HandleFunction("VWUT_Sector/GetAll")]
        public void VWUT_SectorGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                cond = VWUT_SectorModel.UpdateQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity());
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWUT_Sector(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

    }
}
