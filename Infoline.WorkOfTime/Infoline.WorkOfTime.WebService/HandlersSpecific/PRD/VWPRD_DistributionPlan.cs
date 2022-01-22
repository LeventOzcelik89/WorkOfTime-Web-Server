using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData.Specific;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebService.HandlersSpecific
{
    [Export(typeof(ISmartHandler))]
    public partial class VWPRD_DistributionPlanHandler : BaseSmartHandler
    {
        public VWPRD_DistributionPlanHandler()
            : base("VWPRD_DistributionPlan")
        {

        }

      

        [HandleFunction("VWPRD_DistributionPlan/DetailById")]
        public void VWPRD_DistributionPlanGetById(HttpContext context)
        {
            try
            {
                var queryString = context.Request["distributionId"];
                var db = new WorkOfTimeDatabase();
                var distributionId = Guid.NewGuid();
                if (!String.IsNullOrEmpty(queryString))
				{
                    distributionId = new Guid(queryString);
				}

                var data = new VMPRD_DistributionPlanModel().LoadDisributionPlanDetail(distributionId);
                if (data == null)
                {
                    RenderResponse(context, new ResultStatus() { result = false, message = "Envanter Bulunamadı" });
                }

                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}
