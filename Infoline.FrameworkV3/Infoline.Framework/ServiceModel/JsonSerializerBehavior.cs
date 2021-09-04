using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Infoline.ServiceModel
{
    class JsonSerializerBehavior : IContractBehavior
    {
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, System.ServiceModel.Dispatcher.DispatchRuntime dispatchRuntime)
        {
            //dispatchRuntime.ChannelDispatcher.Opening += delegate
            //{
            //    foreach (var op in dispatchRuntime.Operations)
            //    {
            //        op.Formatter = new Formater(op.Formatter);
            //    }
            //};
        }

        void ChannelDispatcher_Opening(object sender, EventArgs e)
        {

        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {

        }
        class Formater : IDispatchMessageFormatter
        {
            IDispatchMessageFormatter sink;
            public Formater(IDispatchMessageFormatter sink)
            {
                this.sink = sink;
            }
            void IDispatchMessageFormatter.DeserializeRequest(System.ServiceModel.Channels.Message message, object[] parameters)
            {
                sink.DeserializeRequest(message, parameters);
            }

            System.ServiceModel.Channels.Message IDispatchMessageFormatter.SerializeReply(System.ServiceModel.Channels.MessageVersion messageVersion, object[] parameters, object result)
            {

                //return new JsonMessage(messageVersion);
                return new JsonMessage(sink.SerializeReply(messageVersion, parameters, result));
            }
        }
        class JsonMessage : Message
        {
            MessageProperties _props;
            public override MessageProperties Properties
            {
                get { return _props; }
            }
            MessageHeaders _header;
            public override MessageHeaders Headers
            {
                get { return _header; }
            }
            MessageVersion _ver;
            public override MessageVersion Version
            {
                get { return _ver; }
            }
            Message _sink;
            public JsonMessage(Message sink):this(sink.Version)
            {
                this._sink = sink;
                _props = sink.Properties;
                _header = sink.Headers;
                
                
            }
            public JsonMessage(MessageVersion ver)
            {
                _props = new MessageProperties();
                _header = new MessageHeaders(_ver = ver);
            }
            protected override void OnWriteBodyContents(System.Xml.XmlDictionaryWriter writer)
            {
                
            }
           
        }
    }
}
