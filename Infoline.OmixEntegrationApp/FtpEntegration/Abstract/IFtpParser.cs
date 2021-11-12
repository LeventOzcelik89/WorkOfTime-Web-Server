using Infoline.OmixEntegrationApp.FtpEntegration.Entities;
using System.Collections.Generic;

namespace Infoline.OmixEntegrationApp.FtpEntegration.Abstract
{
    public interface IFtpParser
    {
        IFtpWorker FtpWorker();
        IEnumerable<FtpObjectsWithFileName<SellIn>> GetSellIns();
        IEnumerable<FtpObjectsWithFileName<SellThr>> GetSellThrs();
    }
}
