using Infoline.OmixEntegrationApp.LogoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.LogoEntegration
{
    public class ProcessLogoEntegration : IDisposable
    {
        public ProcessLogoEntegration()
        {
            Log.Info("ProcessLogoEntegration is Start");
        }

        public void Run()
        {
            while (true)
            {
                DataMapper _dataMapper = new DataMapper();
                var sc = new Service1SoapClient();
                var getCariList = sc.GetCariList(new ClientFindParam { FirmaNo = "043" });
                _dataMapper.CompanySave(getCariList);

                var getMalzemeList = sc.GetMalzemeList(new AdItemsFindParam { FirmaNo = "043" });
                _dataMapper.ProductSave(getMalzemeList);

                var getSevkAdresList = sc.GetSevkAdresList(new AdShipFindParam { FirmaNo = "043" });
                _dataMapper.StorageSave(getSevkAdresList);

                Log.Info("işlem tamamlandı");
                Thread.Sleep(new TimeSpan(2, 0, 0));
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
