using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Net;

namespace Infoline.ServiceModel
{
    public class CallContextBehavior : Attribute, IContractBehavior, ICallContextInitializer, IClientMessageInspector, IDispatchMessageInspector
    {
        public string CookieName { get; set; }

        public CallContextBehavior()
        {
            CookieName = "Mira";
        }
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(this);
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.MessageInspectors.Add(this);
            foreach (var o in dispatchRuntime.Operations)
            {
                o.CallContextInitializers.Add(this);
            }
        }


        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }




        const string CookieHeader = "Cookie";

        string GetCookie(WebHeaderCollection headers, string name)
        {
            var cookies = headers[CookieHeader];
            if (!string.IsNullOrEmpty(cookies))
            {
                if (!string.IsNullOrEmpty(cookies))
                {
                    return cookies.Split(';').Select(a =>
                    {
                        var nv = a.Split('=');
                        return new { name = nv[0].Trim(), value = nv[1].Trim() };
                    }).Where(a => a.name == name).Select(a => a.value).FirstOrDefault();
                }
            }
            return null;
        }
        void SetCookie(WebHeaderCollection headers, string name, string value)
        {
            string cookies = headers[CookieHeader];

            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(cookies))
            {
                foreach (var c in cookies.Split(';').Select(a =>
                {
                    var nv = a.Split('=');
                    return new { name = nv[0].Trim(), value = nv[1].Trim() };
                }).Where(a => a.name != name))
                {
                    sb.AppendFormat("{0}={1};", c.name, c.value);
                }
            }

            sb.AppendFormat("{0}={1};", name, value);
            headers[CookieHeader] = sb.ToString();
        }

        #region ICallContextInitializer

        void CallBeginRequest(dynamic instance)
        {
            try
            {
                instance.BeginRequest();
            }
            catch { }
        }
        object ICallContextInitializer.BeforeInvoke(InstanceContext instanceContext, IClientChannel channel, Message message)
        {

            message.Properties.Add("ticketid", "dd");

            CallBeginRequest(instanceContext.GetServiceInstance());

            var cookie = GetCookie(WebOperationContext.Current.IncomingRequest.Headers, CookieName);

            if (!string.IsNullOrEmpty(cookie))
            {
                var c = Application.Current.SecurityService.LoadTicket(new Guid(cookie));
                if (c != null)
                {
                    c.Activate();
                    return c.TicketId;
                }
            }
            return Guid.Empty;
        }
        void ICallContextInitializer.AfterInvoke(object correlationState)
        {
            if (CallContext.IsReady)
            {
                SetCookie(WebOperationContext.Current.OutgoingResponse.Headers, CookieName, CallContext.Current.TicketId.ToString());
                var id = (Guid)correlationState;
                if (id != CallContext.Current.TicketId)
                {
                    WebOperationContext.Current.OutgoingResponse.Headers["CallContext"] = Infoline.Helper.Json.Serialize(CallContext.Current);
                    //New Context
                }
                CallContext.Current.Deactivate();
            }
        }
        object IDispatchMessageInspector.AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {

            return null;
        }

        void IDispatchMessageInspector.BeforeSendReply(ref Message reply, object correlationState)
        {

        }
        #endregion
        #region IClientMessageInspector
        void IClientMessageInspector.AfterReceiveReply(ref Message reply, object correlationState)
        {
            if (!CallContext.IsReady)
            {

                var httpRes = (HttpResponseMessageProperty)reply.Properties[HttpResponseMessageProperty.Name];
                var ctx = httpRes.Headers["CallContext"];
                if (!string.IsNullOrEmpty(ctx))
                {
                    var cc = Infoline.Helper.Json.Deserialize<CallContext>(ctx);
                    cc.Activate();
                }
            }
        }

        object IClientMessageInspector.BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            if (CallContext.IsReady)
            {
                HttpRequestMessageProperty httpReq = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];

                SetCookie(httpReq.Headers, CookieName, CallContext.Current.TicketId.ToString());
            }
            return null;
        }
        #endregion






    }
}
