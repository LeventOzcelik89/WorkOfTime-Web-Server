using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.Composition;
using System.Web.Routing;
using System.Threading.Tasks;
using Infoline.Helper;

namespace Infoline.Web.SmartHandlers
{
    [Export(typeof(ISmartHandler))]
    class ReflectorHandler : ISmartHandler
    {
        public void ProcessRequest(HttpContext context, IDictionary<string, object> paramters)
        {
            Guid id = new Guid((string)paramters["reflectid"]);
            ReflectedItem item;
            lock (Reflector.items)
            {
                if (Reflector.items.TryGetValue(id, out item))
                    Reflector.items.Remove(id);
            }
            if (item != null)
            {
                if (item.Task != null && !item.Task.IsCompleted)
                    item.Task.Wait();
                item.Handler(context, item);
            }
        }

        public string Name
        {
            get { return "reflect"; }
        }

        public string[] Parameters
        {
            get { return new string[] { "reflectid" }; }
        }
    }

}