using System;
using System.Threading.Tasks;


namespace Infoline.Jobs
{
    public class JobItem
    {
        public static IJobService JobService { get { return Application.Current.GetService<IJobService>(); } }

        public Guid Id { get; internal set; }
        public Task Task { get; internal set; }
        public bool Complete { get; internal set; }
        public string StatusMessage { get; set; }
        public double Progress { get; set; }
        public string ProgressMessage { get; set; }
        public Exception Exception { get; set; }
        public int NextUpdate { get; set; }
        public bool CanRemove { get; set; }
        public string ExtraData { get; set; }
        public System.Threading.CancellationTokenSource CancellationToken { get; internal set; }
        public void Cancel()
        {
            CancellationToken.Cancel();
           // Task.Wait();
        }
    }


}
