using Infoline.OmixEntegrationApp.DistFtpEntegration.Abstract;
using Infoline.OmixEntegrationApp.DistFtpEntegration.Concrete;
using Infoline.OmixEntegrationApp.TitanEntegration.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.DistFtpEntegration
{
    public class ProcessDistFtpEntegration : IDisposable
    {
        IFtpWorker FtpWorker = new FtpWorker();
        public ProcessDistFtpEntegration()
        {
            Log.Info("ProcessDistFtpEntegration is Start");
        }
        public Task Run()
        {
            while (true)
            {
                //if (DateTime.Now.Hour == 22 && DateTime.Now.Minute == 5)
                //{
                    FtpWorker.GetToDayFile();
                //}
                Thread.Sleep(new TimeSpan(0, 1, 0));
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
