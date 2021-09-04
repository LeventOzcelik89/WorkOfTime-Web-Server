using Infoline.Framework.Database;
using Infoline.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Routing;

namespace Infoline.Web.SmartHandlers
{
    [Export(typeof(IService))]
    [ExportMetadata("ServiceType", typeof(ISmartHandlerService))]
    class SmartHandlerService : ISmartHandlerService, IPartImportsSatisfiedNotification, IRouteHandler, IHttpHandler
    {

        public Action<HttpContext> OnProcessRequest { get; set; }

        [ImportMany]
        IEnumerable<ISmartHandler> handlers = null;
        public SmartHandlerService()
        {

        }
        internal ISmartHandler GetHandlerByName(string name)
        {
            return handlers.FirstOrDefault(a => a.Name == name);
        }
        Dictionary<RouteBase, RouteHandler> routes = new Dictionary<RouteBase, RouteHandler>();
        class RouteHandler
        {
            public ISmartHandler Handler { get; set; }
            public MethodInfo Method { get; set; }
        }
        public void OnImportsSatisfied()
        {

            var hnds = handlers.OrderByDescending(a => a.GetType().FullName.Split('.').Length).ThenBy(a => a.GetType().Name.Length).ThenBy(a => a.GetType().Name);

            foreach (var item in hnds)
            {
                string rname = "smart_" + item.Name;
                lock (RouteTable.Routes)
                {
                    AddRoute(new RouteHandler { Handler = item }, rname, string.Format("{0}{1}", item.Name, item.Parameters.FormatJoin("/{{{0}}}")));

                    foreach (var h in item.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .Where(a => a.GetParameters().Length == 1)
                        .Select(a => new { mi = a, at = a.GetCustomAttributes(true).OfType<HandleFunctionAttribute>().FirstOrDefault() })
                        .Where(a => a.at != null))
                    {
                        AddRoute(new RouteHandler { Handler = item, Method = h.mi }, rname + h.mi.Name, h.at.Route);
                    }
                }
            }
        }

        private void AddRoute(RouteHandler item, string rname, string url)
        {
            if (RouteTable.Routes.Count(a => ((System.Web.Routing.Route)a).Url == url) == 0)
            {
                Route rt = RouteTable.Routes[rname] as Route;
                //remove old and add new
                if (rt != null)
                    RouteTable.Routes.Remove(rt);
                rt = new Route(url, this);
                RouteTable.Routes.Add(rname, rt);
                routes.Add(rt, item);
            }
        }

        public string HandlerUrl(string handler, params object[] parameters)
        {

            var rt = handlers.FirstOrDefault(a => a.Name == handler);
            if (rt == null)
                throw new ArgumentException("Invalid Handler.", handler);

            if (parameters.Length != rt.Parameters.Length)
                throw new ArgumentException("Parameter mismatch.", handler);

            var dic = new RouteValueDictionary();
            for (int i = 0; i < rt.Parameters.Length; i++)
                dic.Add(rt.Parameters[i], parameters[i]);

            VirtualPathData data = RouteTable.Routes.GetVirtualPath(HttpContext.Current.Request.RequestContext, "smart_" + handler, dic);
            if (data != null)
                return data.VirtualPath;
            return null;
        }

        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return this;
        }
        public bool IsReusable { get { return false; } }
        public void ProcessRequest(HttpContext context)
        {

            if (
                (HttpContext.Current.Request.Url.LocalPath.StartsWith("/SAIS/") || HttpContext.Current.Request.Url.LocalPath.StartsWith("/SEOS/")) && 
                HttpContext.Current.Request.HttpMethod != "POST")
              {
                BaseSmartHandler.RenderResponse(context, new ResultStatus { result = false, objects = null, message = "Method POST değil !.." });
                return;
            }

            OnProcessRequest?.Invoke(context);

            RouteHandler h = null;
            try
            {
                var d = RouteTable.Routes.GetRouteData(new HttpContextWrapper(context));
                var st = DateTime.Now;
                TimeSpan dif;
                if (routes.TryGetValue(d.Route, out h))
                {
                    dif = (DateTime.Now - st);
                    if (h.Method == null)
                        h.Handler.ProcessRequest(context, d.Values);
                    else
                        h.Method.Invoke(h.Handler, new object[] { context });
                }
                dif = (DateTime.Now - st);
            }
            catch (Exception ex)
            {

                var resultStatus = new ResultStatus()
                {
                    result = false,
                    message = ex.InnerException != null ? ex.InnerException.Message : ""
                };
                BaseSmartHandler.RenderResponse(context, resultStatus);
            }
        }

        public ISmartHandler Get(string name)
        {
            var d = routes.Where(a => a.Value.Handler.Name == name).FirstOrDefault();
            return d.Value.Handler;
        }
    }
    public class SmartHandlerManager
    {
        public static ISmartHandlerService Service
        {
            get
            {
                return Application.Current.GetService<ISmartHandlerService>();
            }
        }
    }
    public class HandleFunctionAttribute : Attribute
    {
        public string Route { get; set; }
        public HandleFunctionAttribute(string route)
        {
            Route = route;
        }
    }

}