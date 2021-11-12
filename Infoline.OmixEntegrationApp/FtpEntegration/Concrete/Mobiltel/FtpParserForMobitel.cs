

using Infoline.OmixEntegrationApp.FtpEntegration.Abstract;
using Infoline.OmixEntegrationApp.FtpEntegration.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.OmixEntegrationApp.FtpEntegration.Concrete.Mobiltel
{
    public class FtpParserForMobitel : IFtpParser
    {
        private IEnumerable<DirectoryItem> Files { get => FtpWorker().GetAllFilesNames(FtpWorker().Configuration).SelectMany(x=>x.Items); }
        public IFtpWorker FtpWorker() {
            return new FtpWorkerForMobiltel();
        }


        public IEnumerable<FtpObjectsWithFileName<SellIn>> GetSellIns()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<FtpObjectsWithFileName<SellThr>> GetSellThrs()
        {
            throw new System.NotImplementedException();
        }
    }
}
