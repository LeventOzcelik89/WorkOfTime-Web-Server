using Infoline.OmixEntegrationApp.FtpEntegrations.Business;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
namespace Infoline.OmixEntegrationApp.FtpEntegrations
{

    public class ProcessFtpEntegration : IDisposable
    {

        public ProcessFtpEntegration()
        {
            Log.Info("ProcessFtpEntegration is Start");
        }
        public void Run()
        {

            var entegrationFilesModel = new FtpMobitel();
            entegrationFilesModel.ProcessFiles();

        }
        public void Dispose()
        {
        }
    }
}
