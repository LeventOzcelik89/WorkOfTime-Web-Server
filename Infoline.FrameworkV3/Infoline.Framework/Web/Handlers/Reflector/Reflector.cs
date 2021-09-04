using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.Web.Routing;
using System.Threading.Tasks;
using Infoline.Helper;
using Infoline.Web.SmartHandlers;

namespace Infoline.Web
{
    public static class Reflector
    {
        internal static Dictionary<Guid, ReflectedItem> items = new Dictionary<Guid, ReflectedItem>();

        public static string Reflect(Action<HttpContext, ReflectedItem> handler, Func<object> task)
        {
            Guid id = Guid.NewGuid();
            Task<object> ts = task != null ? Task.Factory.StartNew<object>(task) : null;
            if (ts != null)
                //ts.ReportError();
            items[id] = new ReflectedItem { Id = id, Task = ts, Handler = handler };
            return Application.Current.GetService<ISmartHandlerService>().HandlerUrl("reflect", id);
        }

        public static string Reflect(Action<HttpContext, ReflectedItem> handler)
        {
            return Reflect(handler, null);
        }
        public static string Reflect(Func<object> task)
        {
            return Reflect((c, i) => { c.Response.Write(i.Task.Result); }, task);
        }
        public static string Reflect(string result)
        {
            return Reflect(() => result);
        }
    }
    public class ReflectedItem
    {
        public Guid Id { get; set; }
        public Task<object> Task { get; set; }
        internal Action<HttpContext, ReflectedItem> Handler;
    }
}