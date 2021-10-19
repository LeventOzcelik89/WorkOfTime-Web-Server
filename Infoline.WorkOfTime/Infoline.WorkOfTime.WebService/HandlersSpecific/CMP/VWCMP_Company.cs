using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWCMP_CompanyHandlers : BaseSmartHandler
    {
        public VWCMP_CompanyHandlers()
            : base("VWCMP_Company")
        {

        }


        [HandleFunction("VWCMP_Company/GetAll")]
        public void VWCMP_CompanyGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCMP_Company(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
        [HandleFunction("VWCMP_Company/GetById")]
        public void VWCMP_CompanyGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = new VMCMP_CompanyModel().LoadCompanyDetail(new Guid((string)id));
                RenderResponse(context, new ResultStatus() { result = true, objects = data });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


    }
}
