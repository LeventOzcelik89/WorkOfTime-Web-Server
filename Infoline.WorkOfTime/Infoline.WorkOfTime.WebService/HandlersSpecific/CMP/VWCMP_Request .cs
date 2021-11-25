using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessData.Specific;
using Infoline.Web.SmartHandlers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Infoline.ProjectManagement.WebService.HandlersSpecific.CMP
{
    [Export(typeof(ISmartHandler))]
    public partial class VWCMP_RequestHandler : BaseSmartHandler
    {
        public VWCMP_RequestHandler()
            : base("VWCMP_RequestHandler")
        {

        }


        [HandleFunction("VWCMP_Request/GetById")]
        public void VWVWCMP_RequestGetById(HttpContext context)
        {
            try
            {
                var id = context.Request["id"];
                var model = new VMCMP_RequestModels { id = new Guid(id) }.Load(false);
                RenderResponse(context, model);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message });
                return;
            }
        }

        [HandleFunction("VWCMP_Request/Insert")]
        public void VWCMP_RequestInsert(HttpContext context)
        {
            try
            {
                var item = ParseRequest<VMCMP_RequestModels>(context);
                var userId = CallContext.Current.UserId;
                var db = new WorkOfTimeDatabase();
                item.Save(userId);
                RenderResponse(context, new ResultStatus() { result = true, message = "Satın Alma Talebi Başarıyla Oluşturuldu" });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}