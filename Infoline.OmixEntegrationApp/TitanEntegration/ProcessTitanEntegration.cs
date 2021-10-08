using Infoline.OmixEntegrationApp.TitanEntegration.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.TitanEntegration
{
    public class ProcessTitanEntegration : IDisposable
    {
        TitanService TitanService = new TitanService();
        public ProcessTitanEntegration()
        {
            Log.Info("ProcessTitanEntegration is Start");
        }

        public void Run()
        {
            TitanService.SaveAll();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
