using Infoline.OmixEntegrationApp.LogoService;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.LogoEntegration
{
    public class ProcessLogoEntegration : IDisposable
    {
        public string FirmaNo { get => ConfigurationManager.AppSettings["FirmaHost"].ToString(); }

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
                foreach (var firmaNo in FirmaNo.Split(','))
                {
                    var getCariList = sc.GetCariList(new ClientFindParam { FirmaNo = firmaNo });
                    var getProductList = sc.GetMalzemeList(new AdItemsFindParam { FirmaNo = firmaNo });
                    _dataMapper.CompanySave(getCariList, firmaNo);
                    _dataMapper.ProductSave(getProductList);
                    if (getCariList.Count() > 0)
                    {
                        foreach (var cariKodu in getCariList)
                        {
                            var getStorageList = sc.GetSevkAdresList(new AdShipFindParam { FirmaNo = firmaNo, CariKodu = cariKodu.CariKodu });
                            _dataMapper.StorageSave(getStorageList);
                        }
                    }
                }

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
