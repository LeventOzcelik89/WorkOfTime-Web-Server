using GeoAPI.Geometries;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Infoline.WorkOfTime.WebProject
{

    public class MvcApplication : System.Web.HttpApplication
    {

        protected void Application_Start()
        {
            Utilities.ConfigureSQL();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(IGeometry), new UIGeographyBinder());
            ModelBinders.Binders.Add(typeof(double), new DoubleBinderTest());
            ModelBinders.Binders.Add(typeof(double?), new DoubleBinderTest());
            ModelBinders.Binders.Add(typeof(bool), new UIBoolBinder());
            ModelBinders.Binders.Add(typeof(bool?), new UIBoolBinder());

            //var mesaj = string.Format("asdasdasdasdasdsadsada");
            //new Email().Template("Template1", "birthday.jpg", "Doğum Günü Tebrik", mesaj)
            //   .Send((Int16)EmailSendTypes.ZorunluMailler, "oguz.yavuz@infoline-tr.com", string.Format("{0} | {1}", "sa", "Doğum Günü Tebrik"), true, null, null);
            
        }

        public void AutoLogin(string loginname, string pass)
        {
            var db = new WorkOfTimeDatabase();
            var result = db.Login(loginname, pass);
            if (result.LoginResult == LoginResult.OK)
            {
                HttpContext.Current.Session["userStatus"] = db.GetUserPageSecurityByticketid(result.ticketid);
            }
        }

        protected void Application_BeginRequest()
        {
            new TenantConfig(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority);

        }


        protected void Session_OnStart(object sender, EventArgs e)
        {
#if DEBUG
            AutoLogin("developer", "devinfo2021.");
            //new Defaults().GetSummaries(Guid.Empty);
            //new Defaults().GetNotifications(new WorkOfTimeDatabase().GetVWSH_UserById(Guid.Empty));

            //new Defaults().GetConfigs();
            //new Defaults().GetDataSources(Guid.Empty);
#endif
        }
        protected void Session_End(object sender, EventArgs e)
        {
            if (Session["userStatus"] != null)
            {
                var db = new WorkOfTimeDatabase();
                var userStatus = (PageSecurity)Session["userStatus"];
                db.UpdateSH_Ticket(new SH_Ticket { id = userStatus.ticketid, endtime = DateTime.Now });
                Session.Remove("userStatus");
                Session.Clear();
            }
        }

        void Application_Error(object sender, EventArgs e)
        {

#if !DEBUG
            
            if (Session["AppError"] != null)
                {
                    return;
                }

            var message = "";
            var url = HttpContext.Current.Request.Url.AbsoluteUri;
            if (HttpContext.Current.Session["userStatus"] != null)
            {
                var userStatus = (PageSecurity)HttpContext.Current.Session["userStatus"];
                var kullanici = userStatus.user.FullName + " (" + userStatus.user.loginname + ")";
                message = "<strong>" + kullanici + " isimli kullanıcı</strong>";
            }
            message += "<p>" + DateTime.Now.ToString("dd.MM.yyyy HH:mm") + " tarihinde <a href='" + url + "'>" + url + "</a> sayfasında hata almıştır.<br/>Tekrar etmeyeceğini temenni ederek modül sorumlusunun en kısa sürede ilgili hatayı düzelmesini rica ederiz.</p>";
            message += "<br/><strong>Hataya Gelinen sayfa : " + (Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : "Bulunamadı.") + "</strong>";



            var serverError = Server.GetLastError() as HttpException;

            if (null != serverError)
            {
                int errorCode = serverError.GetHttpCode();

                switch (errorCode)
                {
                    case 404:
                        Response.Redirect("/Error/UnkownPage");
                        break;
                    case 500:

                        if (HttpContext.Current.Session != null)
                        {
                            HttpContext.Current.Session["ErrorCode"] = serverError.ErrorCode;
                            HttpContext.Current.Session["ErrorMessage"] = serverError.Message;
                            message += "<br/><br/>" + serverError.ErrorCode + "<br/><br/>" + serverError.Message;
                            try
                            {
                                new Email().Send((Int16)EmailSendTypes.ZorunluMailler,"ahmet.undemir@infoline-tr.com", "Sayfa Hata Verdi.", message, true, false, new string[] { "oguz.yavuz@infoline-tr.com","kerem.un@infoline-tr.com","cihat.kapucu@infoline-tr.com", "senol.elik@infoline-tr.com", "levent.ozcelik@infoline-tr.com", "ahmet.temiz@infoline-tr.com",  "ahmet.undemir@infoline-tr.com", "kerem.un@infoline-tr.com" }, null, null, true);
                            }
                            catch
                            {
                            }
                        }
                        Response.Redirect("/Error/InternalServer");
                        Log.Error(serverError.Message);
                        return;
                }

                Server.ClearError();
            }


            var error = Server.GetLastError();

            if (error != null)
            {

                Log.Error(error.Message);

                if (HttpContext.Current.Session != null)
                {
                    HttpContext.Current.Session["ErrorCode"] = error.Source;
                    HttpContext.Current.Session["ErrorMessage"] = error.Message;
                    HttpContext.Current.Session["ErrorTrace"] = error.StackTrace;
                    message += "<br/><br/>" + error.Source + "<br/><br/>" + error.Message + "<br/><br/>" + error.StackTrace;
                    try
                    {
                        new Email().Send((Int16)EmailSendTypes.ZorunluMailler, "ahmet.undemir@infoline-tr.com", "Sayfa Hata Verdi.", message, true, false, new string[] { "oguz.yavuz@infoline-tr.com","kerem.un@infoline-tr.com","cihat.kapucu@infoline-tr.com", "senol.elik@infoline-tr.com", "levent.ozcelik@infoline-tr.com", "ahmet.temiz@infoline-tr.com", "ahmet.undemir@infoline-tr.com", "kerem.un@infoline-tr.com" }, null, null, true);

                    }
                    catch { }

                }
                Response.Redirect("/Error/InternalServer");

            }

#endif

        }
    }
}
