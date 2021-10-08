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
        ResultStatus CompanySave(AdClientFind[] param);
        ResultStatus CompanyInsert(AdClientFind param);
        ResultStatus CompanyUpdate(AdClientFind param);
        ResultStatus ProductValidator(AdShipFindList param, PRD_Product product);
        ResultStatus ProductSave(AdItemsFindList[] param);
        ResultStatus ProductInsert(AdItemsFindList param);
        ResultStatus ProductUpdate(AdItemsFindList param);
        ResultStatus StorageValidator(AdShipFindList param, CMP_Storage storage);
        ResultStatus StorageSave(AdShipFindList[] param);
        ResultStatus StorageInsert(AdShipFindList param);
        ResultStatus StorageUpdate(AdShipFindList param);
    }
}
