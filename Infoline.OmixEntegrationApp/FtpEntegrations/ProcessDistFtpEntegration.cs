using Infoline.OmixEntegrationApp.DistFtpEntegrations.Business;
using System;
using System.Threading;
using System.Threading.Tasks;
namespace Infoline.OmixEntegrationApp.DistFtpEntegrations
{

    public class ProcessDistFtpEntegration : IDisposable
    {
        public ProcessDistFtpEntegration()
        {
            Log.Info("ProcessDistFtpEntegration is Start");
        }
        public Task Run()
        {
            while (true)
            {
                //Mobitel
                var entegrationFilesModel = new FtpMobitel();
                entegrationFilesModel.FileSave();
                Thread.Sleep(new TimeSpan(0, 1, 0));


            }
        }
        public void Dispose()
        {

        }
    }
}
