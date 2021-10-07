using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.LogoEntegration
{
    public class ProcessLogoEntegration: IDisposable
    {
        public ProcessLogoEntegration()
        {
            Log.Info("ProcessLogoEntegration is Start");
        }

        public void Run()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
