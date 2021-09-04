using System;
using System.Web;

namespace Infoline.Web.SmartHandlers
{

    public interface ISmartHandlerService : IService
    {
        Action<HttpContext> OnProcessRequest { get; set; }

        string HandlerUrl(string handler, params object[] paramters);
    }

}