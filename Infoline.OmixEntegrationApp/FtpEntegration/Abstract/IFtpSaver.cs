
using Infoline.Framework.Database;

namespace Infoline.OmixEntegrationApp.FtpEntegration.Abstract
{
    public interface IFtpSaver
    {
        IFtpChecker FtpChecker();
        ResultStatus Save();
        

    }
}
