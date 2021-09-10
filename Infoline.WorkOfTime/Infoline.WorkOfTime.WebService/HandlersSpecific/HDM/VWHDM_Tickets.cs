using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{

    [Export(typeof(ISmartHandler))]
    public partial class VWHDM_TicketsHandler : BaseSmartHandler
    {
        public VWHDM_TicketsHandler()
            : base("VWHDM_Ticket")
        {

        }


        [HandleFunction("VWHDM_Tickets/GetById")]
        public void VWHDM_TicketGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = new VMHDM_TicketModel { id = new Guid((string)id) }.Load();
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWHDM_Tickets/ManagerGetAll")]
        public void VWHDM_TickestManagerGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                cond = VMHDM_TicketModel.UpdateQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity());
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWHDM_TicketSpesific(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWHDM_Tickets/PersonalGetAll")]
        public void VWHDM_TickestPersonalGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                cond = VMHDM_TicketModel.UpdateQueryPersonal(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity());
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWHDM_TicketSpesific(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWHDM_Tickets/GetPageInfo")]
        public void VWPA_TicketGetPageInfo(HttpContext context)
        {
            try
            {
                var data = new VMHDM_TicketModel().GetMyTicketSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWHDM_Tickets/GetPageMasterManagerInfo")]
        public void VWPA_TicketGetPageMasterManagerInfo(HttpContext context)
        {
            try
            {
                var data = new VMHDM_TicketModel().GetMyMasterManagerTicketSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWHDM_Tickets/GetPageManagerInfo")]
        public void VWPA_TicketGetManagerPageInfo(HttpContext context)
        {
            try
            {
                var data = new VMHDM_TicketModel().GetMyManagerTicketSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWHDM_Tickets/GetSuggestionsByText")]
        public void VWHDM_TicketGetSuggestionsByText(HttpContext context)
        {
            try
            {
                var keyword = context.Request["keyword"];
                var data = new VMHDM_SuggestionModel().GetByKeyword(keyword);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWHDM_Tickets/Insert")]
        public void VWHDM_TicketInsert(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var model = ParseRequest<VMHDM_TicketModel>(context);
                var userId = CallContext.Current.UserId;
                var user = db.GetVWSH_UserById(CallContext.Current.UserId);
                if (model.Requester != null)
                {
                    model.Requester.fullName = user.FullName;
                    model.Requester.email = user.email;
                    model.Requester.phone = user.phone;
                    model.Requester.company = user.companyCellPhone;
                }
                else
                {
                    model.Requester = new VWHDM_TicketRequester
                    {
                        fullName = user.FullName,
                        email = user.email,
                        phone = user.phone,
                        company = user.companyCellPhone,
                    };
                }
                if (!model.status.HasValue)
                {
                    var rs = model.Save(userId, model.status, null);
                    RenderResponse(context, new ResultStatus() { result = true, message = "İşlem Başarılı Bir Şekilde Gerçekleştirildi.", objects = null });
                }
                else
                {   
                    var rs = model.Load(model.status).Save(userId, model.status, null);
                    RenderResponse(context, new ResultStatus() { result = true, message = "İşlem Başarılı Bir Şekilde Gerçekleştirildi.", objects = null });
                }
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWHDM_Suggestion/SuggestionByIssueId")]
        public void VWHDM_SuggestionByIssueId(HttpContext context)
        {
            try
            {
                var issueId = context.Request["issueId"];
                var db = new WorkOfTimeDatabase();
                var data = db.GetHDM_SuggestionsByIssueId(Guid.Parse(issueId));
                foreach (var item in data)
                {
                    item.content = Regex.Replace(item.content, "<p>", "½$#");
                    item.content = Regex.Replace(item.content, "<.*?>", String.Empty);
                    item.content = Regex.Replace(item.content, "\r", String.Empty);
                    item.content = Regex.Replace(item.content, "\n", String.Empty);
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
