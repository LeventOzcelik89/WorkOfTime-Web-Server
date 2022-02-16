using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;

namespace Infoline.OmixEntegrationApp.FtpEntegrations.Utils
{
    public class Finder
    {
        public static VWPRD_Inventory FindInventory(string serialNumber, string imei)
        {
            var db = new WorkOfTimeDatabase();
            return db.GetPRD_InventoryBySerialCodeOrImei(serialNumber, imei);

        }
        public static VWCMP_Invoice FindInvoice(string invoiceId)
        {
            var db = new WorkOfTimeDatabase();
            return db.GetVWCMP_InvoiceById(new Guid(invoiceId));
        }
        public static Guid? FindCompany(PRD_EntegrationAction item)
        {
            var db = new WorkOfTimeDatabase();
            var getByCompany = db.GetVWCMP_CompanyByTaxNumberOrCode( item.CustomerOperatorCode, item.TaxNumber);
            var getByStorage = db.GetVWCMP_StorageByCode( item.CustomerOperatorCode);
            if (getByCompany != null)
            {
                return getByCompany.id;
            }
            if (getByStorage != null)
            {
                var findCompanyByStorage = db.GetCMP_CompanyById(getByStorage.companyId.Value);
                if (findCompanyByStorage!=null)
                {
                    return findCompanyByStorage.id;
                }  
            }
             return null;

        }
       
        public static VWCMP_Storage FindStorage(string name, string code)
        {
            var db = new WorkOfTimeDatabase();
            return db.GetVWCMP_StorageByNameOrCode(name, code);
        }
    }
}
