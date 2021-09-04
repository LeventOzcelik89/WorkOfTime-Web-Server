using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService.HandlersSpecific
{
    [Export(typeof(ISmartHandler))]
    public partial class VWCMP_CompanyHandler : BaseSmartHandler
    {
        public VWCMP_CompanyHandler()
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

        [HandleFunction("CMP_Company/GetSpecific")]
        public void CMP_CompanyGetSpecific(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var data = db.GetCMP_CompanyByNameId();
                RenderResponse(context, new ResultStatus { result = true, objects = data });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWCMP_Company/Insert")]
        public void VWCMP_CompanyInsert(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCMP_CompanyModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Save(userId);
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

    }
}
