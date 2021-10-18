using Infoline.Framework.Database;
using Infoline.OmixEntegrationApp.LogoService;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.OmixEntegrationApp.LogoEntegration
{
    public interface IDataMapper
    {
        ResultStatus CompanyValidator(AdClientFind param, CMP_Company company);
        ResultStatus CompanySave(AdClientFind[] param, string firmaNo);
        ResultStatus CompanyInsert(AdClientFind param, string firmaNo);
        ResultStatus CompanyUpdate(AdClientFind param, string firmaNo);
        ResultStatus ProductValidator(AdItemsFindList param, PRD_Product product);
        ResultStatus ProductSave(AdItemsFindList[] param);
        ResultStatus ProductInsert(AdItemsFindList param);
        //ResultStatus ProductUpdate(AdItemsFindList param);
        ResultStatus StorageValidator(AdShipFindList param, CMP_Storage storage);
        ResultStatus StorageSave(AdShipFindList[] param, string firmaNo);
        ResultStatus StorageInsert(AdShipFindList param, string firmaNo);
        ResultStatus StorageUpdate(AdShipFindList param, string firmaNo);
    }
}
