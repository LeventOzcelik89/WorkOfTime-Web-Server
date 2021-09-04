using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Composition;
using Infoline.Helper;

namespace Infoline.Jobs
{
    [Export(typeof(IService))]
    [ExportMetadata("ServiceType", typeof(IJobService))]
    class JobService : IJobService
    {
        internal static Dictionary<Guid, JobItem> items = new Dictionary<Guid, JobItem>();

        [ThreadStatic]
        static JobItem _current = null;
        public JobItem CurrentItem
        {
            get { return _current; }
        }


        public void SetStatus(string message, string progresmessage, double progress)
        {
            if (_current != null)
            {
                lock (_current)
                {
                    _current.Progress = progress;
                    _current.ProgressMessage = progresmessage;
                    _current.StatusMessage = message;
                }
            }
        }

        public void SetStatus(double progress, string progresmessage)
        {
            if (_current != null)
            {
                lock (_current)
                {
                    _current.Progress = progress;
                    _current.ProgressMessage = progresmessage;
                }
            }
        }
        public void SetStatus(double progress)
        {
            if (_current != null)
            {
                lock (_current)
                {
                    _current.Progress = progress;
                }
            }
        }
        public void SetStatus(int current, int total)
        {
            if (_current != null)
            {
                lock (_current)
                {
                    _current.Progress = total > 0 ? (double)current / (double)total : 0;
                }
            }
        }
        public JobItem CreateJob(Action task)
        {
            return CreateJob((a) => task());
        }
        public JobItem CreateJob(Action<JobItem> task)
        {
            Guid id = Guid.NewGuid();
            var item = new JobItem { Id = id, StatusMessage = "", Progress = 0, Complete = false, CancellationToken = new System.Threading.CancellationTokenSource(), NextUpdate = 2000 };

            Task ts = task != null ? Task.Factory.StartNew(() =>
            {

                try
                {
                    _current = item;
                    task(item);
                }
                catch (OperationCanceledException)
                {
                    item.StatusMessage = "İşlem İptal edildi";
                }
                catch (Exception ex)
                {
                    lock (item)
                        item.Exception = ex;
                    item.StatusMessage = "Hata oluştu";
                    //item.ProgressMessage = "Hata oluştu";
                    item.Progress = 1;
                }

                lock (item)
                    item.Complete = true;

            }, item.CancellationToken.Token) : null;
            //ts.ReportError();
            lock (item)
                item.Task = ts;
            items[id] = item;
            //  return Application.Current.GetService<ISmartHandlerService>().HandlerUrl("job", id,"status");
            return item;
        }


        public JobItem GetJob(Guid id)
        {
            JobItem j = null;
            items.TryGetValue(id, out j);
            return j;
        }

        public void CollectJobs()
        {
            lock (items)
            {
                items.Values.Where(a => a.CanRemove).ToArray().Do(a => items.Remove(a.Id));
            }

        }
    }
}
