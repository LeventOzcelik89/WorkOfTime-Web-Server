using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml;

namespace Infoline.ServiceModel
{
    [Export(typeof(IService))]
    [ExportMetadata("ServiceType", typeof(IServiceHost))]
    class ServiceHostService : IServiceHost
    {
        public ServiceHostService()
        {
            Application.ApplicationStarted += new Action<Application>(Application_ApplicationStarted);
        }

        void Application_ApplicationStarted(Application app)
        {

            foreach (var servicetype in app.Services)
            {
                var service = app.GetService(servicetype);
                if ( service.GetType().GetCustomAttributes(typeof(RegisterServiceAttribute), true) != null)
                {
                    var att = servicetype.GetCustomAttributes(typeof(ServicePathAttribute), true)
                        .FirstOrDefault() as ServicePathAttribute;
                    if (att != null)
                    {
                        this.RegisterService(servicetype, service, att.Path);
                    }
                }
            }
        }
        public void RegisterService(Type servicetype, object service, string relativeurl)
        {
            Web.SecurityAuthenticationModule.Config.AddPublicPath(relativeurl);


            System.Web.Routing.RouteTable.Routes.Add(
                new ServiceRoute(relativeurl, new CustomServiceHostFactory(service), servicetype));
        }

        public void UnregisterService(string relativeurl)
        {
            var rt = System.Web.Routing.RouteTable.Routes.OfType<ServiceRoute>().FirstOrDefault(a => a.Url == relativeurl);
            if (rt != null)
            {
                System.Web.Routing.RouteTable.Routes.Remove(rt);
                Web.SecurityAuthenticationModule.Config.RemovePublicPath(relativeurl);
            }
        }

        public string BaseUrl
        {
            get { throw new NotImplementedException(); }
        }

        class CustomServiceHostFactory : WebServiceHostFactory
        {
            object singleton;
            public CustomServiceHostFactory(object singleton)
            {
                this.singleton = singleton;
            }
            protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
            {
                var host = new ServiceHost(this.singleton, baseAddresses);

                var sb = host.Description.Behaviors.OfType<ServiceBehaviorAttribute>().FirstOrDefault();
                sb.InstanceContextMode = InstanceContextMode.Single;
                sb.IncludeExceptionDetailInFaults = true;

                var ac = host.Description.Behaviors.OfType<AspNetCompatibilityRequirementsAttribute>().FirstOrDefault(); if (ac == null)
                    host.Description.Behaviors.Add(
                            ac = new AspNetCompatibilityRequirementsAttribute { RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed });
                ac.RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed;
                var cd = ContractDescription.GetContract(serviceType);
                host.AddServiceEndpoint(serviceType, new JsonWebHttpBinding(), baseAddresses.FirstOrDefault())
                    .Behaviors.Add(new WebHttpBehavior { DefaultOutgoingResponseFormat = WebMessageFormat.Json, DefaultBodyStyle = WebMessageBodyStyle.Wrapped });
                foreach (var ep in host.Description.Endpoints)
                {

                    ep.Contract.Behaviors.Add(new CallContextBehavior());
                    ep.Contract.Behaviors.Add(new JsonSerializerBehavior());
                }
                return host;

            }



        }
    }


    class DataEndPointBehavior : IEndpointBehavior
    {



        class DataInvoker : IOperationInvoker
        {
            IOperationInvoker sink;
            public DataInvoker(IOperationInvoker sink)
            {
                this.sink = sink;
            }

            public object[] AllocateInputs()
            {
                return new object[1];
            }

            public object Invoke(object instance, object[] inputs, out object[] outputs)
            {

                return sink.Invoke(instance, inputs, out outputs);
            }

            public IAsyncResult InvokeBegin(object instance, object[] inputs, AsyncCallback callback, object state)
            {
                throw new NotImplementedException();
            }

            public object InvokeEnd(object instance, out object[] outputs, IAsyncResult result)
            {
                throw new NotImplementedException();
            }

            public bool IsSynchronous
            {
                get { return true; }
            }


        }

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {

        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            var udo = endpointDispatcher.DispatchRuntime.UnhandledDispatchOperation;
            udo.DeserializeRequest = false;
            udo.SerializeReply = false;
            udo.Invoker = new DataInvoker(udo.Invoker);
        }

        public void Validate(ServiceEndpoint endpoint)
        {

        }


    }

    class JsonWebHttpBinding : WebHttpBinding
    {
        public override BindingElementCollection CreateBindingElements()
        {

            var ret = base.CreateBindingElements();
            ret[0] = new JsonMessageEncodingBindingElement();
            return ret;
        }
       
    }
    class JsonMessageEncodingBindingElement : MessageEncodingBindingElement
    {
        public JsonMessageEncodingBindingElement(JsonMessageEncodingBindingElement clone):base(clone)
        {

        }

        public override BindingElement Clone()
        {
            return new JsonMessageEncodingBindingElement(this);
        }
        public JsonMessageEncodingBindingElement()
        {

        }

        public override MessageVersion MessageVersion
        {
            get
            {
                return MessageVersion.None;
            }
            set
            {
            }
        }
        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new JsonMessageEncoderFactory();
        }
        public override T GetProperty<T>(BindingContext context)
        {
            return base.GetProperty<T>(context);
        }
        class JsonMessageEncoderFactory : MessageEncoderFactory
        {
            private MessageEncoder encoder;
            private MessageVersion version;
            private string mediaType;
            private string charSet;

            public JsonMessageEncoderFactory()
            {
                this.version = MessageVersion.None;

                this.mediaType = "text/json";
                this.charSet = "UTF-8";
                this.encoder = new JsonMessageEncoder(this);

            }
            internal JsonMessageEncoderFactory(string mediaType, string charSet,
                MessageVersion version)
            {
                this.version = version;
                this.mediaType = mediaType;
                this.charSet = charSet;
                this.encoder = new JsonMessageEncoder(this);
            }

            public override MessageEncoder Encoder
            {
                get
                {
                    return this.encoder;
                }
            }

            public override MessageVersion MessageVersion
            {
                get
                {
                    return this.version;
                }
            }

            internal string MediaType
            {
                get
                {
                    return this.mediaType;
                }
            }

            internal string CharSet
            {
                get
                {
                    return this.charSet;
                }
            }
        }
        class JsonMessageEncoder : MessageEncoder
        {
            private JsonMessageEncoderFactory factory;
            private XmlWriterSettings writerSettings;
            private string contentType;

            public JsonMessageEncoder(JsonMessageEncoderFactory factory)
            {
                this.factory = factory;

                this.writerSettings = new XmlWriterSettings();
                this.writerSettings.Encoding = Encoding.GetEncoding(factory.CharSet);
                this.contentType = string.Format("{0}; charset={1}",
                    this.factory.MediaType, this.writerSettings.Encoding.HeaderName);
            }

            public override string ContentType
            {
                get
                {
                    return this.contentType;
                }
            }

            public override string MediaType
            {
                get
                {
                    return factory.MediaType;
                }
            }

            public override MessageVersion MessageVersion
            {
                get
                {
                    return this.factory.MessageVersion;
                }
            }

            public override Message ReadMessage(ArraySegment<byte> buffer, BufferManager bufferManager, string contentType)
            {
                byte[] msgContents = new byte[buffer.Count];
                Array.Copy(buffer.Array, buffer.Offset, msgContents, 0, msgContents.Length);
                bufferManager.ReturnBuffer(buffer.Array);

                MemoryStream stream = new MemoryStream(msgContents);
                return ReadMessage(stream, int.MaxValue);
            }

            public override Message ReadMessage(Stream stream, int maxSizeOfHeaders, string contentType)
            {
                XmlReader reader = XmlReader.Create(stream);
                return Message.CreateMessage(reader, maxSizeOfHeaders, this.MessageVersion);
            }

            public override ArraySegment<byte> WriteMessage(Message message, int maxMessageSize, BufferManager bufferManager, int messageOffset)
            {
                MemoryStream stream = new MemoryStream();
                XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
                message.WriteMessage(writer);
                writer.Close();

                byte[] messageBytes = stream.GetBuffer();
                int messageLength = (int)stream.Position;
                stream.Close();

                int totalLength = messageLength + messageOffset;
                byte[] totalBytes = bufferManager.TakeBuffer(totalLength);
                Array.Copy(messageBytes, 0, totalBytes, messageOffset, messageLength);

                ArraySegment<byte> byteArray = new ArraySegment<byte>(totalBytes, messageOffset, messageLength);
                return byteArray;
            }

            public override void WriteMessage(Message message, Stream stream)
            {
                XmlWriter writer = XmlWriter.Create(stream, this.writerSettings);
                message.WriteMessage(writer);
                writer.Close();
            }
        }
    }
}
