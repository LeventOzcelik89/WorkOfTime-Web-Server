using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;

namespace Infoline.OmixEntegrationApp.FtpEntegrations.Utils
{
    public class Finder
    {
        public static VWPRD_Inventory FindInventory(string serialNumber,string imei)
        {
            var db =  new WorkOfTimeDatabase();
            return db.GetPRD_InventoryBySerialCodeOrImei(serialNumber,imei);

        }

        public  static VWCMP_Invoice FindInvoice(string invoiceId) {
            var db = new WorkOfTimeDatabase();
            return db.GetVWCMP_InvoiceById(new Guid(invoiceId));

        }
        public static VWCMP_Company FindCompany(string name,string code) {
            var db =new WorkOfTimeDatabase();
            return db.GetVWCMP_CompanyByNameOrCode(name,code);
                    
        }

        public static VWCMP_Storage FindStorage(string name, string code)
        {
            var db = new WorkOfTimeDatabase();
            return db.GetVWCMP_StorageByNameOrCode(name, code);

        }
    }
}
