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

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class VWPA_TransactionHandler : BaseSmartHandler
    {
        public VWPA_TransactionHandler()
            : base("VWPA_Transaction")
        {

        }

        [HandleFunction("VWPA_Transaction/GetAll")]
        public void VWPA_TransactionGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWPA_Transaction(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Transaction/MyTransactionGetAll")]
        public void VWPA_TransactionMyTransactionGetAll(HttpContext context)
        {
            try
            {
                var direction = context.Request["direction"];
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                if (!String.IsNullOrEmpty(direction))
                {
                    cond = VMPA_TransactionModel.MyTransactionQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity(), int.Parse(direction));
                }
                else
                {
                    cond = VMPA_TransactionModel.MyTransactionQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity(), null);
                }
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWPA_Transaction(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Transaction/MyTransactionApprovedGetAll")]
        public void VWPA_TransactionMyTransactionApprovedGetAll(HttpContext context)
        {
            try
            {
                var direction = context.Request["direction"];
                var userId = CallContext.Current.UserId;
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                if (!String.IsNullOrEmpty(direction))
                {
                    cond = VMPA_TransactionModel.MyTransactionApprovedQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity(), int.Parse(direction));
                }
                else if (cond.Filter == null)
                {
                    cond = VMPA_TransactionModel.MyTransactionApprovedQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity(), null);
                }
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWPA_Transaction(cond);
                data = data.Where(x => 
                (x.confirmationUserIds != null && x.confirmationUserIds.Contains(userId.ToString().ToUpper())) ||(x.confirmUserIds != null && x.confirmUserIds.Contains(userId.ToString().ToUpper())) || 
                (x.rejectedUserIds != null && x.rejectedUserIds.Contains(userId.ToString().ToUpper()))).ToArray();
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWPA_Transaction/Insert")]
        public void VWPA_TransactionInsert(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMPA_TransactionModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Save(userId);
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Transaction/Update")]
        public void VWPA_TransactionUpdate(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMPA_TransactionModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Save(userId);
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Transaction/Delete")]
        public void VWPA_TransactionDelete(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMPA_TransactionModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Delete();
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Transaction/GetById")]
        public void VWPA_TransactionGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = new VMPA_TransactionModel { id = new Guid((string)id) }.Load();
                RenderResponse(context, new ResultStatus
                {
                    result = true,
                    objects = data,
                });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWPA_Transaction/GetPageInfo")]
        public void VWPA_TransactionGetPageInfo(HttpContext context)
        {
            try
            {
                var data = new VMPA_TransactionModel().GetTransactionSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWPA_Transaction/GetAllPageInfo")]
        public void VWPA_TransactionGetPageInfoList(HttpContext context)
        {
            try
            {
                var data = new VMPA_TransactionModel().GetListTransactionSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Transaction/GetMyApprovedPageInfo")]
        public void VWPA_TransactionGetMyApprovedPageInfoList(HttpContext context)
        {
            try
            {
                var data = new VMPA_TransactionModel().GetMyApprovedTransactionSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Transaction/DemandAgain")]
        public void VWPA_TransactionDemandAgain(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = new VMPA_TransactionModel { id = new Guid((string)id) }.LoadMobile();
                RenderResponse(context, new ResultStatus
                {
                    result = true,
                    objects = data,
                });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

    }
}
