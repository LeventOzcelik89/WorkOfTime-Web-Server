using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Mobile;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class INV_PermitHandler : BaseSmartHandler
    {
        public INV_PermitHandler()
            : base("INV_PermitHandler")
        {

        }

        [HandleFunction("INV_Permit/MBInsert")]
        public void MBInsert(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();

                if (CallContext.Current == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı bulunamadı." });
                    return;
                }
                var userId = CallContext.Current.UserId;
                var data = ParseRequest<INV_Permit>(context);
                if(data == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "İzin bulunamadı." });
                    return;
                }

                data.createdby = userId;
                var res = new INV_PermitModelBusiness().Insert(data);
                if (res.result == false)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = res.message });
                    return;
                }
                RenderResponse(context, new ResultStatus { result = true, message = res.message, objects = data.id });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("INV_Permit/Calculater")]
        public void INV_PermitCalculater(HttpContext context)
        {
            try
            {
                var data = ParseRequest<INV_Permit>(context);
                var res = new Permits().Calculate(data);
                RenderResponse(context, new ResultStatus { result = true, objects = res.objects });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("INV_Permit/MBMyPermitsAbout")]
        public void MBMyPermitsAbout(HttpContext context)
        {

            try
            {
                var userid = CallContext.Current.UserId;
                var res = new Permits().MyPermitsAbout(userid);
                RenderResponse(context, new ResultStatus {result = true , objects = res.objects });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }


        }

        [HandleFunction("INV_Permit/MBMyPermits")]
        public void MBMyPermits(HttpContext context)
        {
            try
            {
                var userid = CallContext.Current.UserId;

                var res = new Permits().MyPermits(userid);

                RenderResponse(context, new ResultStatus { result = true, objects = res.objects });

            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false , message = ex.Message.ToString() });
                return;
            }
        }

        [HandleFunction("INV_Permit/MBUpdate")]
        public void MBUpdate(HttpContext context)
        {
            try
            {
                var userid = CallContext.Current.UserId;

                var item = ParseRequest<INV_Permit>(context);
                var res = new Permits().PermitUpdate(item, userid);

                if(res.result == false)
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


        [HandleFunction("INV_Permit/GetMyPermitsSummary")]
        public void INV_CommissionsGetMyCommissionsSummary(HttpContext context)
        {
            try
            {
                var data = new Permits().GetMyPermitsSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
            }
          
        }

        [HandleFunction("INV_Permit/GetMyAboutPermitsSummary")]
        public void INV_CommissionsGetMyAboutPermitsSummary(HttpContext context)
        {
            try
            {
                var data = new Permits().GetMyAboutPermitsSummary(CallContext.Current.UserId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
            }
          
        }
    }
}