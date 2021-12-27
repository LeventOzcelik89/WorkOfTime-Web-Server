using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessData.Specific;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebService.HandlersSpecific
{

    [Export(typeof(ISmartHandler))]
    public partial class CRM_OpponentCompanyNewHandler : BaseSmartHandler
    {
        public CRM_OpponentCompanyNewHandler()
            : base("CRM_OpponentCompany")
        {

        }

        [HandleFunction("CRM_OpponentCompany/Insert")]
        public void CRM_OpponentCompanyInsert(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var data = ParseRequest<CRM_OpponentCompany>(context);
                data.created = DateTime.Now;
                data.createdby = CallContext.Current.UserId;

                var dataById = db.GetCRM_OpponentCompanyByCompanyNameCount(data.CompanyName);
                if (dataById > 0)
                {
                    RenderResponse(context, new ResultStatus() { result = false, message = "Aynı İsimden Birden Fazla Kayıt Olamaz" });
                }
                else
                {
                    var dbresult = db.InsertCRM_OpponentCompany(data);
                    RenderResponse(context, dbresult);
                }
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}