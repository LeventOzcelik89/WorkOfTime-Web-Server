using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{


    public class Startup
    {
        public void Configuration()
        {

        }
    }




    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

        }


        protected void Session_OnStart(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest()
        {
            new TenantConfig(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }



        //void Application_Error(object sender, EventArgs e)
        //{
        //    var message = "";
        //    var url = HttpContext.Current.Request.Url.AbsoluteUri;
        //    message += "<p>" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " tarihinde <a href='" + url + "'>" + url + "</a> sayfasında hata almıştır.<br/>Tekrar etmeyeceğini temenni ederek modül sorumlusunun en kısa sürede ilgili hatayı düzelmesini rica ederiz.</p>";
        //    message += "<br/><strong>Hataya Gelinen sayfa : " + (Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : "Bulunamadı.") + "</strong>";


        //    var serverError = Server.GetLastError() as HttpException;

        //    if (null != serverError)
        //    {
        //        int errorCode = serverError.GetHttpCode();

        //        switch (errorCode)
        //        {
        //            case 404:
        //                Response.Redirect("/Error/UnkownPage");
        //                break;
        //            case 500:

        //                if (HttpContext.Current.Session != null)
        //                {
        //                    message += "<br/><br/>" + serverError.ErrorCode + "<br/><br/>" + serverError.Message;
        //                }
        //                Log.Error(serverError.Message);
        //                return;
        //        }
        //        Server.ClearError();
        //    }


        //    var error = Server.GetLastError();

        //    if (error != null)
        //    {
        //        if (HttpContext.Current.Session != null)
        //        {
        //            message += "<br/><br/>" + error.Source + "<br/><br/>" + error.Message + "<br/><br/>" + error.StackTrace;
        //            Log.Error(message);
        //        }
        //    }
        //}


    }
}
