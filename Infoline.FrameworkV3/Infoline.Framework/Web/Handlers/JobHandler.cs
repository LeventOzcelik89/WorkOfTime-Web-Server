using System;
using System.ComponentModel.Composition;
using System.Web;
using Infoline.Helper;
using Infoline.Jobs;
using System.Collections.Generic;

namespace Infoline.Web.SmartHandlers
{
    [Export(typeof(ISmartHandler))]
    class JobHandler : ISmartHandler
    {

        public static string CancelURL(Guid id)
        {
            return Application.Current.GetService<ISmartHandlerService>().HandlerUrl("job", id, "cancel");
        }
        public static string StatusURL(Guid id)
        {
            return Application.Current.GetService<ISmartHandlerService>().HandlerUrl("job", id, "status");
        }
        public void ProcessRequest(HttpContext context, IDictionary<string, object> paramters)
        {
            Guid id = new Guid((string)paramters["id"]);
            var service = Application.Current.GetService<IJobService>();

            var item = service.GetJob(id);
            if (item != null)
            {

                string cmd = paramters["cmd"] as string;
                if (cmd == "cancel" && !item.CancellationToken.IsCancellationRequested)
                {
                    item.Cancel();
                }
                lock (item)
                    context.Response.Write(Json.Serialize(new
                    {
                        IsCompleted = item.Complete,
                        Status = item.StatusMessage,
                        Progress = item.Progress,
                        ProgressMessage = item.ProgressMessage,
                        Error = item.Exception != null ?
                        item.Exception.Message : item.Exception.ToString(),
                        NextUpdate = item.NextUpdate,
                        Script = item.ExtraData
                    }));
                if (item.Complete)
                    service.CollectJobs();
            }
        }

        public string Name
        {
            get { return "job"; }
        }

        public string[] Parameters
        {
            get { return new string[] { "id", "cmd" }; }
        }
    }

}