using Infoline.OmixEntegrationApp.FtpEntegrations.Business;
using System;
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
            entegrationFilesModel.ExportFilesToDatabase();
            var entegrationForGenpa = new FtpGenpa();
            entegrationForGenpa.ExportFilesToDatabase();
            var entegrationForKvk = new FtpKvk();
            entegrationForKvk.ExportFilesToDatabase();

            var entegrationForPort = new FtpPort();
            entegrationForPort.ExportFilesToDatabase();


        }
        public void Dispose()
        {
        }
    }
}
