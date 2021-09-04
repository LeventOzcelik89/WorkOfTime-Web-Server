using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.ProjectManagement.WebService.Handlers
{
    [Export(typeof(ISmartHandler))]
    public partial class FBU_Handler : BaseSmartHandler
    {
        public FBU_Handler() : base("FBU_Handler")
        {

        }

        [HandleFunction("fbu/getogrenciders")]
        public void FBU_GetOgrenciDers(HttpContext context)
        {
            try
            {
                var email = context.Request["email"];
                var model = new FBU_OISModel(email).GetOgrenciDers();
                RenderResponse(context, new ResultStatus { result = true, objects = model });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }

        }

        [HandleFunction("fbu/getogrencidersprogrami")]
        public void FBU_GetOgrenciDersProgrami(HttpContext context)
        {
            try
            {
                var email = context.Request["email"];
                var model = new FBU_OISModel(email).GetOgrenciDersProgrami();
                RenderResponse(context, new ResultStatus { result = true, objects = model });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }

        }

        [HandleFunction("fbu/getakademiktakvim")]
        public void FBU_GetAkademikTakvim(HttpContext context)
        {
            try
            {
                var email = context.Request["email"];
                var model = new FBU_OISModel(email).GetAkademikTakvim();
                RenderResponse(context, new ResultStatus { result = true, objects = model });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }

        }

        [HandleFunction("fbu/getogrencisinavprogrami")]
        public void FBU_GetOgrenciSinavProgrami(HttpContext context)
        {
            try
            {
                var email = context.Request["email"];
                var model = new FBU_OISModel(email).GetOgrenciSinavProgrami();
                RenderResponse(context, new ResultStatus { result = true, objects = model });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }

        }

        [HandleFunction("fbu/getogrencisinavsonuc")]
        public void FBU_GetOgrenciSinavSonuc(HttpContext context)
        {
            try
            {
                var email = context.Request["email"];
                var model = new FBU_OISModel(email).GetOgrenciSinavSonuc();
                RenderResponse(context, new ResultStatus { result = true, objects = model });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }

        }

        [HandleFunction("fbu/getogrenciduyuru")]
        public void FBU_GetOgrenciDuyuru(HttpContext context)
        {
            try
            {
                var email = context.Request["email"];
                var model = new FBU_OISModel(email).GetOgrenciDuyuru();
                RenderResponse(context, new ResultStatus { result = true, objects = model });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }


        [HandleFunction("fbu/getogrencibilgi")]
        public void FBU_GetOgrenciBilgi(HttpContext context)
        {
            try
            {
                var email = context.Request["email"];
                var model = new FBU_OISModel(email).GetOgrenciBilgi();
                RenderResponse(context, model);
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }


        [HandleFunction("fbu/sendtranskriptrequest")]
        public void FBU_SendTranskriptRequest(HttpContext context)
        {
            try
            {
                var email = context.Request["email"];
                var model = new FBU_OISModel(email).SendTranskriptRequest();
                RenderResponse(context, model);
                return;
            }

            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }

        }


        [HandleFunction("fbu/sendwebcandidateform")]
        public void FBU_SendWebCandidateForm(HttpContext context)
        {
            try
            {
                var studentData = ParseRequest<CSM_StudentFBUWeb>(context);
                var rsm = new VMCSM_StudentModel().SendForm(studentData);
                RenderResponse(context, new ResultStatus { result = rsm.result, message = rsm.result ? "Form kaydetme işlemi başarı ile gerçekleşti." : "Form kayıt işlemi hatalı oldu." + rsm.message });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }
    }
}