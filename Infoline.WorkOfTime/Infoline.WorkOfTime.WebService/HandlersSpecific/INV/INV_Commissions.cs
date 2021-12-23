using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Mobile;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Web.SmartHandlers;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class INV_CommissionsHandler : BaseSmartHandler
    {
        public INV_CommissionsHandler()
            : base("INV_CommissionsHandler")
        {

        }

        [HandleFunction("INV_Commissions/MBInsert")]
        public void INV_CommissionsInsert(HttpContext context)
        {
            try
            {
                var item = ParseRequest<INV_CommissionModel>(context);

                var result = new Commissions().CommissionsInsert(item, CallContext.Current.UserId);
                if (result.result == false)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = result.message });
                    return;
                }

                RenderResponse(context, new ResultStatus { result = true, message = result.message });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("INV_Commissions/Calculater")]
        public void INV_CommissionsCalculater(HttpContext context)
        {
            try
            {
                var data = ParseRequest<INV_Commissions>(context);
                var res = new Commissions().CommisCalculate(data);
                RenderResponse(context, new ResultStatus { result = true, objects = res.objects });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("INV_Commissions/MBUpdate")]
        public void INV_CommissionsUpdate(HttpContext context)
        {

            try
            {
                var db = new WorkOfTimeDatabase();
                var userId = CallContext.Current.UserId;
                var item = ParseRequest<INV_Commissions>(context);


                var res = new Commissions().CommissionsUpdate(item, userId);
                if (res.result == false)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = res.message });
                    return;
                }

                RenderResponse(context, new ResultStatus { result = true, message = res.message });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("INV_Commissions/MBMyCommissionsAbout")]
        public void INV_CommissionsGetManagerData(HttpContext context)
        {

            try
            {
                var userid = CallContext.Current.UserId;
                var res = new Commissions().MyCommissionsAbout(userid);
                if (res.result == false)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = res.message });
                    return;
                }

                RenderResponse(context, new ResultStatus { result = true, message = res.message, objects = res.objects });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }


        }

        [HandleFunction("INV_Commissions/MBMyCommissions")]
        public void GetMyData(HttpContext context)
        {
            try
            {
                var userId = CallContext.Current.UserId;
                var res = new Commissions().MBMyCommissions(userId);
                if (res.result == false)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = res.message });
                    return;
                }
                RenderResponse(context, new ResultStatus { result = true, message = res.message, objects = res.objects });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("INV_Commissions/GetMyCommissionsSummary")]
        public void INV_CommissionsGetMyCommissionsSummary(HttpContext context)
        {
            try
            {
                var data = new Commissions().GetMyCommissionsSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("INV_Commissions/GetMyAboutCommissionsSummary")]
        public void INV_CommissionsGetMyAboutCommissionsSummary(HttpContext context)
        {
            try
            {
                var data = new Commissions().GetMyAboutCommissionsSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("INV_Commissions/GetDetailCommissions")]
        public void INV_CommissionsGetDetailCommissions(HttpContext context)
        {
            try
            {
                var commissionId = context.Request["id"];
                var db = new WorkOfTimeDatabase();
                var projects = db.GetVWINV_CommissionsProjectsByCommissionsId(new Guid(commissionId));
                var persons = db.GetVWINV_CommissionsPersonsByIdAll(new Guid(commissionId));
                var information = db.GetVWINV_CommissionsInformationByComissionId(new Guid(commissionId));
                var data = new VMINV_CommissionsProjectsAndPersons
                {
                    ComissionsPersons = persons,
                    CommissionsProjects = projects,
                    ComissionsInformation = information
                };

                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
            }
        }
    }
}