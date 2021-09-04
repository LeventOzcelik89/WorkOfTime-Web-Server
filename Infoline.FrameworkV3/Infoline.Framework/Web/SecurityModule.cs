using System;
using System.Web;
using System.Linq;
using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.Helper;

namespace Infoline.Web
{
    public class SecurityAuthenticationModule : IHttpModule
    {
        public SecurityAuthenticationModule()
        {

        }

        #region IHttpModule Members

        HttpApplication _context;
        static public SecurityWebConfig Config { get; private set; }
        #endregion

        #region Properties
        string LoginPage
        {
            get
            {

                string lp = Config.LoginPage;
                return ApplicationPath + (lp = !lp.StartsWith("/") ? "/" + lp : lp);
            }
        }
        string ApplicationPath
        {
            get
            {
                string s = _context.Request.ApplicationPath;
                return s == "/" ? "" : s;
            }
        }

        string CookieName
        {
            get
            {
                return Config.CookieName;
            }
        }
        string PublicPath
        {
            get
            {
                return ApplicationPath + "/Public/";
            }
        }
        #endregion

        TimeSpan Time
        {
            get
            {
                var timer = _context.Context.Items["timer"] as System.Diagnostics.Stopwatch;
                if (timer == null)
                {
                    _context.Context.Items["timer"] = timer = new System.Diagnostics.Stopwatch();
                    timer.Start();
                }
                return timer.Elapsed;

            }
        }

        static SecurityAuthenticationModule()
        {
            Config = SecurityWebConfig.GetConfig;
            Application.Current.SecurityService.TicketLife = Config.TicketLife;
            Application.Current.ContextProvider = new AspContextProvider();
        }

        public void Init(HttpApplication context)
        {
            _context = context;
            _context.AuthorizeRequest += new EventHandler(context_AuthenticateRequest);
            _context.BeginRequest += new EventHandler(_context_BeginRequest);
            _context.EndRequest += new EventHandler(_context_EndRequest);
            _context.PreSendRequestHeaders += new EventHandler(_context_PreSendRequestHeaders);
            _context.Error += new EventHandler(_context_Error);

        }

        void _context_Error(object sender, EventArgs e)
        {
            var ex = _context.Server.GetLastError();
            if (ex != null && ex.InnerException != null)
            {
                ApplicationLog.LogError(ex.InnerException);
            }
        }

        public void Dispose()
        {
            if (_context != null)
            {
                try
                {
                    _context.AuthorizeRequest -= new EventHandler(context_AuthenticateRequest);
                }
                catch
                {

                }
            }
        }

        bool IsPublicPath(string path)
        {
            return path.ToLower().StartsWith(PublicPath.ToLower()) || path.ToLower() == LoginPage.ToLower();
        }

        private void context_AuthenticateRequest(object sender, EventArgs e)
        {
            AuthenticateRequest();
        }

        public class AToken
        {
            public Guid? TicketId { get; set; }
            public Guid? DeviceId { get; set; }
        }

        void AuthenticateRequest()
        {
            Guid? ticketId = null;
            if (_context.Request.Cookies[CookieName] != null)
            {
                ticketId = Guid.Parse(_context.Request.Cookies[CookieName].Value);

            }
            else if (_context.Request.Headers["AToken"] != null)
            {
                try
                {
                    var tokenEncryptd = _context.Request.Headers["AToken"];
                    var tokenDecrypted = new CryptographyHelper().Decrypt(tokenEncryptd);
                    var token = Json.Deserialize<AToken>(tokenDecrypted);
                    ticketId = token.TicketId;
                }
                catch
                {
                    var token = Json.Deserialize<AToken>(_context.Request.Headers["AToken"]);
                    ticketId = token.TicketId;
                }
            }

            if (ticketId.HasValue)
            {
                var cc = Application.Current.SecurityService.LoadTicket(ticketId.Value, _context.Request.Path);
                if (cc != null)
                {
                    cc.Activate();
                    _context.Context.User = System.Threading.Thread.CurrentPrincipal;
                }
            }

            string path = _context.Request.Path;
            if (_context.Request.ApplicationPath != "/")
            {
                path = path.Remove(0, _context.Request.ApplicationPath.Length);
            }

            _context.Context.SkipAuthorization = true;
            if (Config.IsSecurePath(path) && !CallContext.IsReady)
            {
                _context.Response.StatusCode = 401;
                _context.Response.End();
            }
        }

        void _context_BeginRequest(object sender, EventArgs e)
        {


        }

        void AddCookie(string ticket)
        {
            try
            {
                var cc = _context.Request.Cookies[CookieName];
                if ((cc == null || cc.Value != ticket) && !_context.Response.Cookies.AllKeys.Contains(CookieName))
                {
                    HttpCookie c = new HttpCookie(CookieName, ticket);
                    c.Expires = DateTime.Now.AddMonths(12);
                    _context.Response.Cookies.Add(c);
                }
            }
            catch (Exception ex)
            {
                ApplicationLog.LogMessage(LogLevel.Error, string.Format("Cookie Err: {0} {1}", _context.Request.Url, ex));

            }
        }

        void _context_PreSendRequestHeaders(object sender, EventArgs e)
        {
            if (CallContext.IsReady && CallContext.Current != null)
                AddCookie(CallContext.Current.TicketId.ToString());
        }

        private void _context_EndRequest(object sender, EventArgs e)
        {
            CallContext cc = CallContext.IsReady ? CallContext.Current : null;
            if (cc != null)
            {
                AddCookie(cc.TicketId.ToString());
                cc.Deactivate();
            }
        }
    }
}


