using Infoline.OmixEntegrationApp.FtpEntegration.Entities;
using System.Collections.Generic;

namespace Infoline.OmixEntegrationApp.FtpEntegration.Abstract
{
    public interface IFtpWorker
    {
        /// <summary>
        /// Ftp'nin bilgilerinin tutulduğu alan
        /// </summary>
        FtpConfiguration Configuration { get; }

        /// <summary>
        /// Dizinde ki tüm dosya adlarını getirir.
        /// </summary>
        /// <returns></returns>
        IEnumerable<DirectoryItem> GetAllFilesNames(FtpConfiguration configuration);
        /// <summary>
        /// Verilen parametreye göre dosyadan okunan verinin ham şekilde listeye dökülmesidir
        /// </summary>
        /// <param name="directoryItem"></param>
        /// <returns></returns>
        IEnumerable<string[]> GetRawFile(DirectoryItem directoryItem);
    }
}
