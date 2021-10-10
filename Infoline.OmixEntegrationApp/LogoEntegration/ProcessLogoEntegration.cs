using Infoline.OmixEntegrationApp.LogoService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.LogoEntegration
{
    public class ProcessLogoEntegration: IDisposable
    {
        private readonly IDataMapper _dataMapper;
        public ProcessLogoEntegration()
        {
            Log.Info("ProcessLogoEntegration is Start");
        }
        public ProcessLogoEntegration(IDataMapper dataMapper)
        {
            _dataMapper = dataMapper;
        }

        public void Run()
        {
            var sc = new Service1SoapClient();
            var getCariList = sc.GetCariList(new ClientFindParam { FirmaNo = "043" });
            _dataMapper.CompanySave(getCariList);

            var getMalzemeList = sc.GetMalzemeList(new AdItemsFindParam{ FirmaNo ="043"});
            _dataMapper.ProductSave(getMalzemeList);

            var getSevkAdresList = sc.GetSevkAdresList(new AdShipFindParam { FirmaNo = "043" });
            _dataMapper.StorageSave(getSevkAdresList);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
