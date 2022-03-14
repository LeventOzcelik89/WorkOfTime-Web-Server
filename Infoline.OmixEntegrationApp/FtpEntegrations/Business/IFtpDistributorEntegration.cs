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
        ResultStatus ExportFilesToDatabase();
        PRD_EntegrationFiles[] GetFilesInFtp();
        PRD_EntegrationAction[] GetSellInFilesInFtp(string fileName, Guid entegrationFilesId);
        string FileTypeName(string fileName);
    }
}
