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

        public Task Run()
        {
            while (true)
            {
                TitanService.CompensateFromTitanServices();
                Task.Delay(new TimeSpan(1, 0, 0)).Wait();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
