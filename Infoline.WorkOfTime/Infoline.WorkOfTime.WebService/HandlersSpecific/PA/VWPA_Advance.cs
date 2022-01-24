using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class VWPA_AdvanceHandler : BaseSmartHandler
    {
        public VWPA_AdvanceHandler()
            : base("VWPA_Advance")
        {

        }

        [HandleFunction("VWPA_Advance/GetAll")]
        public void VWPA_VWPA_AdvanceGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWPA_Advance(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Advance/Insert")]
        public void VWPA_AdvanceInsert(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMPA_AdvanceModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Save(userId);
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Advance/Update")]
        public void VWPA_AdvanceUpdate(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMPA_AdvanceModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Save(userId);
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Advance/Delete")]
        public void VWPA_AdvanceDelete(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMPA_AdvanceModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Delete();
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Advance/GetById")]
        public void VWPA_AdvanceGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = new VMPA_AdvanceModel { id = new Guid((string)id) }.Load();
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


        [HandleFunction("VWPA_Advance/MyAdvanceGetAll")]
        public void VWPA_AdvanceMyAdvanceGetAll(HttpContext context)
        {
            try
            {
                var direction = context.Request["direction"];
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                if (!String.IsNullOrEmpty(direction))
                {
                    cond = VMPA_AdvanceModel.MyAdvanceQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity(), int.Parse(direction));
                }
                else
                {
                    cond = VMPA_AdvanceModel.MyAdvanceQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity(), null);
                }
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWPA_Advance(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWPA_Advance/MyAdvanceApprovedGetAll")]
        public void VWPA_AdvanceMyAdvanceApprovedGetAll(HttpContext context)
        {
            try
            {
                var direction = context.Request["direction"];
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                if (!String.IsNullOrEmpty(direction))
                {
                    cond = VMPA_AdvanceModel.MyAdvanceApprovedQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity(), int.Parse(direction));
                }
                else
                {
                    cond = VMPA_AdvanceModel.MyAdvanceApprovedQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity(), null);
                }
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWPA_Advance(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWPA_Advance/GetPageInfo")]
        public void VWPA_AdvanceGetPageInfo(HttpContext context)
        {
            try
            {
                var data = new VMPA_AdvanceModel().GetAdvanceSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Advance/GetAllPageInfo")]
        public void VWPA_AdvanceGetPageInfoList(HttpContext context)
        {
            try
            {
                var data = new VMPA_AdvanceModel().GetListAdvanceSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWPA_Advance/GetApprovedPageInfo")]
        public void VWPA_AdvanceGetApprovedPageInfoList(HttpContext context)
        {
            try
            {
                var data = new VMPA_AdvanceModel().GetListMyApprovedAdvanceSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWPA_Advance/DemandAgain")]
        public void VWPA_AdvanceDemandAgain(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = new VMPA_AdvanceModel { id = new Guid((string)id) }.LoadMobile();
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


        [HandleFunction("VWPA_Advance/GetRequestPageInfo")]
        public void VWPA_AdvanceGetRequestPageInfo(HttpContext context)
        {
            try
            {
                var data = new VMPA_AdvanceModel().GetRequestAdvanceSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}
