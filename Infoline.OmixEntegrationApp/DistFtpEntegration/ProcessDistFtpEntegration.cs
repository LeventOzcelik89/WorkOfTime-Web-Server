using Infoline.OmixEntegrationApp.DistFtpEntegration.Abstract;
using Infoline.OmixEntegrationApp.DistFtpEntegration.Concrete;
using Infoline.OmixEntegrationApp.TitanEntegration.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                FtpWorker.GetToDayFile();
                Task.Delay(new TimeSpan(0, 0, 10)).Wait();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
