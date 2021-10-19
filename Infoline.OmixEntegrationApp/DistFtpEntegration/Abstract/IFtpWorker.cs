using Infoline.OmixEntegrationApp.DistFtpEntegration.Model;
using System.Collections.Generic;
namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Abstract
{
    public interface IFtpWorker
    {
        FtpConfiguration FtpConfiguration { get; set; }
        void SetConfiguration(FtpConfiguration ftpConfiguration);
        FtpConfiguration GetConfiguration();
        IEnumerable<SellIn> GetSellInObjectForToday();
        IEnumerable<SellThr> GetSellThrObjectForToday();
    }
}
