using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWFTM_TaskHandler : BaseSmartHandler
    {
        public VWFTM_TaskHandler()
            : base("VWFTM_TaskHandler")
        {

        }

        [HandleFunction("VWFTM_Task/GetPageInfo")]
        public void VWPA_TransactionGetPageInfo(HttpContext context)
        {
            try
            {
                var data = new VMFTM_TaskServiceModel().GetTaskSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}