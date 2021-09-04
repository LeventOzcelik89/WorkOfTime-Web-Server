using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class FTM_TaskScadaHandler : BaseSmartHandler
    {
        public FTM_TaskScadaHandler()
            : base("FTM_TaskScadaHandler")
        {

        }

        [HandleFunction("Scada/InsertFTM_Task")]
        public void InsertFTM_Task(HttpContext context)
        {
            try
            {
                var task = ParseRequest<VMFTM_TaskModel>(context);
                if (task == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Görev nesnesi boş gönderilemez." });
                    return;
                }

                task.id = Guid.NewGuid();
                task.customerId = new Guid("667B9E0F-9508-451E-AE8A-017F10B3FC2A");
                if (TenantConfig.Tenant.TenantCode == 1187)
                {
                    task.customerId = new Guid("A1799E80-517E-423A-A3E6-70FE9363BDD9");
                }

                var result = task.InsertAll(CallContext.Current.UserId);

                RenderResponse(context, new ResultStatus { result = true, message = result.message, objects = task.id });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }


        [HandleFunction("Scada/TaskSubjectGetAll")]
        public void FTM_TaskSubjectGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetFTM_TaskSubject(cond).Select(x => new
                {
                    id = x.id,
                    name = x.name
                }).ToArray();
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("Scada/TaskStatusControl")]
        public void FTM_TaskTaskStatusControl(HttpContext context)
        {
            try
            {
                var id = context.Request["id"];
                var db = new WorkOfTimeDatabase();
                var task = db.GetVWFTM_TaskById(new Guid(id));
                RenderResponse(context, task?.lastOperationStatus_Title);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}