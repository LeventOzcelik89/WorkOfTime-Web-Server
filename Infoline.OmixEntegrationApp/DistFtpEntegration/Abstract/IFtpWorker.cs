using Infoline.OmixEntegrationApp.DistFtpEntegration.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Abstract
{
    public interface IFtpWorker
    {

        FtpConfiguration ftpConfiguration { get; set; }
        void SetConfiguration(FtpConfiguration ftpConfiguration);
        FtpConfiguration GetConfiguration();
        IEnumerable<SellIn> GetSellInObjectForToday();
        IEnumerable<SellThr> GetSellThrObjectForToday();

    }

    public class FtpConfiguration
    {
        string url { get; set; }
        string userName { get; set; }
        string password { get; set; }
    }
}
