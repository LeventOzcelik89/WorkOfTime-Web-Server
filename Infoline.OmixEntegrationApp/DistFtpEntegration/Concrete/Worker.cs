using Infoline.OmixEntegrationApp.DistFtpEntegration.Business;
using System.Configuration;
using System.Threading;
namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Concrete
{
    public class Worker
    {
        FtpWorkerForGenpa Genpa = new FtpWorkerForGenpa();
        FtpWorkerForKvk Kvk = new FtpWorkerForKvk();
        FtpWorkerForMobiltel Mobitel = new FtpWorkerForMobiltel();
        PRD_EntegrationFilesModel EntegrationFilesModel = new PRD_EntegrationFilesModel();
        public Worker()
        {
            var genpaUserName = ConfigurationManager.AppSettings["GenpaUserName"].ToString();
            var genpaPassword = ConfigurationManager.AppSettings["GenpaPassword"].ToString();
            Genpa.SetConfiguration(new Model.FtpConfiguration { Password = genpaPassword, UserName = genpaUserName });
            var kvkUserName = ConfigurationManager.AppSettings["KvkUserName"].ToString();
            var kvkPassword = ConfigurationManager.AppSettings["KvkPassword"].ToString();
            var kvkUrl = ConfigurationManager.AppSettings["KvkHost"].ToString() ?? "";
            Kvk.SetConfiguration(new Model.FtpConfiguration { UserName = kvkUserName, Password = kvkPassword, Url = kvkUrl });
        }
        private void GetKvkObjects()
        {
            var sellIn = Kvk.GetSellInObjectForToday();
            var sellThr = Kvk.GetSellThrObjectForToday();
        }
        private void GetGenpaObjects()
        {
            var sellIn = Genpa.GetSellInObjectForToday();
            var sellThr = Genpa.GetSellThrObjectForToday();
        }
        private void GetMobitelObjects()
        {
            var sellIn =Mobitel.GetSellInObjectForToday();
            var sellThr = Mobitel.GetSellThrObjectForToday();
        }
        public void GetObjects()
        {

            //GetMobitelObjects();
            EntegrationFilesModel.Save();
            //new Thread(() =>
            //{
            //    GetKvkObjects();
            //}).Start();
            //new Thread(() =>
            //{
            //    GetGenpaObjects();
            //}).Start();
        }
    }
}
