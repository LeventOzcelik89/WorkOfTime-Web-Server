using Infoline.OmixEntegrationApp.FtpEntegration.Entities;
using System;
using System.Collections.Generic;

namespace Infoline.OmixEntegrationApp.FtpEntegration.Abstract
{
    public interface IFtpChecker
    {
        DateTime GetFilesUntil { get; }
        IEnumerable<DirectoryItem> GetFilesNotExist();
        DateTime GetLastFileGetted();

    }
}
