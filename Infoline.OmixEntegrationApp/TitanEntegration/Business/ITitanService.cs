using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infoline.OmixEntegrationApp.TitanEntegration.Business
{
    public interface ITitanService
    {
        string Host { get; }
        void SaveAll();
        ResultStatus GetAll();
        ResultStatus Sender<T>(string uri, string query = null);
    }
}
