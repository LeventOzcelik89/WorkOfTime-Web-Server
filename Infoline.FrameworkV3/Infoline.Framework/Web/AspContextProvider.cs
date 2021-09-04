using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Infoline.Web
{
    class AspContextProvider : ICallContextProvider
    {
        class WFCContextExtension : IExtension<OperationContext>
        {
            public void Attach(OperationContext owner)
            {
            }

            public void Detach(OperationContext owner)
            {
            }
            public CallContext CallContext { get; set; }
        }

        public AspContextProvider()
        {
        }
        [ThreadStatic]
        CallContext cc;
        public CallContext Context
        {
            get
            {
                var octx = OperationContext.Current;
                if (octx != null)
                {
                    var cp = octx.Extensions.Find<WFCContextExtension>();
                    return cp == null ? null : cp.CallContext;
                }
                var ctx = System.Web.HttpContext.Current;
                if (ctx != null)
                {
                    var dic = System.Web.HttpContext.Current.Items;
                    return dic.Contains("CallContext") ? dic["CallContext"] as CallContext : null;
                }

                return cc;

            }
            set
            {
                var octx = OperationContext.Current;
                if (octx != null)
                {
                    var cp = octx.Extensions.Find<WFCContextExtension>();
                    if (cp == null)
                        octx.Extensions.Add(new WFCContextExtension { CallContext = value });
                    else
                        cp.CallContext = value;

                    return;
                }
                var ctx = System.Web.HttpContext.Current;
                if (ctx != null)
                {
                    System.Web.HttpContext.Current.Items["CallContext"] = value;
                }
                else
                    cc = value;
            }
        }


        public bool IsReady
        {
            get
            {

                return Context != null;

            }
        }


    }
}
