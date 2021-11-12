using Infoline.Framework.Database;
using Infoline.OmixEntegrationApp.DistFtpEntegration.Concrete;
using Infoline.OmixEntegrationApp.DistFtpEntegration.Utils;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;

namespace Infoline.OmixEntegrationApp.DistFtpEntegration.Business
{
    public class PRD_EntegrationFilesModel
    {

        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        private FtpWorkerForMobiltel FtpWorkerForMobiltel = new FtpWorkerForMobiltel();
        public PRD_EntegrationFilesModel()
        {
                var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
                var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
                db = tenant.GetDatabase();
            
        }

        public ResultStatus Save()
        {
            var result = new ResultStatus() { result = true };
            trans = trans ?? db.BeginTransaction();
            FtpWorkerForMobiltel.GetFileNames(FtpWorkerForMobiltel.FtpConfiguration);
            var fileNames = FtpWorkerForMobiltel.FptUrl;
            var filesForMobitel = new List<PRD_EntegrationFiles>();
            
            foreach (var file in fileNames.Where(x=>x.FileName.Contains("SELLIN")||x.FileName.Contains("SELLTHR")))
            {
                filesForMobitel.Add(new PRD_EntegrationFiles
                {
                    created = DateTime.Now,
                    createdby = Guid.Empty,
                    CreateDateInFtp = file.FileCreatedDate,
                    DistributorName = "Mobiltel",
                    FileName = file.FileName,
                    FileNameDate = Tools.GetDateFromFileName(file.FileName, "yyyyMMddss"),
                    ProcessTime = DateTime.Now,
                });
            }





            return result;

        }








    }
}
