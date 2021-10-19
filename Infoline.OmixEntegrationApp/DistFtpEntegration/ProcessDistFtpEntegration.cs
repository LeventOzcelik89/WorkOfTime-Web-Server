using Infoline.OmixEntegrationApp.DistFtpEntegration.Concrete;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace Infoline.OmixEntegrationApp.DistFtpEntegration
{
    
    public class ProcessDistFtpEntegration : IDisposable
    {
        Worker Worker = new Worker();
        public ProcessDistFtpEntegration()
        {
            Log.Info("ProcessDistFtpEntegration is Start");
        }
        public Task Run()
        {
            while (true)
            {
                if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 5)
                {
                    Worker.GetObjects();
                }
                Worker.GetObjects();
                Thread.Sleep(new TimeSpan(0, 1, 0));
            }
        }
        public void Dispose()
        {
            
        }
    }
}
