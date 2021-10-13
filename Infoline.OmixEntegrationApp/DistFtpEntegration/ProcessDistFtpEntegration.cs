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
                if (DateTime.Now.Hour==22&& DateTime.Now.Minute==5)
                {
                    FtpWorker.GetToDayFile();
                }
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
