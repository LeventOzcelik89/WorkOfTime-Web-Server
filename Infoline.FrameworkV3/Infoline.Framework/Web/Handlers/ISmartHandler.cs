using System;
using System.Web;
using System.Collections.Generic;

namespace Infoline.Web.SmartHandlers
{
    public interface ISmartHandler
    {
        void ProcessRequest(HttpContext context, IDictionary<string, object> paramters);
        string Name { get; }
        string[] Parameters { get; }
    }
}
