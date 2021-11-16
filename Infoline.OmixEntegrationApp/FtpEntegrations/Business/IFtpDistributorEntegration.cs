using Infoline.Framework.Database;
using Infoline.OmixEntegrationApp.FtpEntegrations.Model;
using Infoline.WorkOfTime.BusinessData;
using System;

namespace Infoline.OmixEntegrationApp.FtpEntegrations.Business
{
    interface IFtpDistributorEntegration
    {
        FtpConfiguration ftpConfiguration { get; }
        string DistributorName { get; }
        Guid DistributorId { get; }

        void SetFtpConfiguration();
        ResultStatus ProcessFiles();
        PRD_EntegrationFiles[] GetFiles(DateTime processDate);
        PRD_EntegrationAction[] GetSellThr(string fileName, Guid entegrationFilesId);
        string FileTypeName(string fileName);
    }
}
