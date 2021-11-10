using System;
using System.Configuration;
using System.Linq;
using Infoline.OmixEntegrationApp.DistFtpEntegration.Model;
using Infoline.OmixEntegrationApp.DistFtpEntegration.Utils;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Business
{
    public class PRD_EntegrationActionModel
    {
        private WorkOfTimeDatabase db = new WorkOfTimeDatabase();

        public PRD_EntegrationActionModel()
        {
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
            db = tenant.GetDatabase();
        }

        public void Insert(SellThr sellThr)
        {
            db = db ?? new WorkOfTimeDatabase();
            var findProduct = Finder.FindProduct(sellThr.SeriNo, sellThr.Imei);
            var findCompany = Finder.FindCompany(sellThr.CustomerName, sellThr.CustomerCode?? sellThr.CustomerOperatorCode);
        }


    }
}
