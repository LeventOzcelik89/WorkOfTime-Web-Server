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
            var getByCompany = db.GetVWCMP_CompanyByNameOrCode(item.CustomerOperatorName, item.CustomerOperatorCode, item.TaxNumber);
            var getByStorage = db.GetVWCMP_StorageByNameOrCode(item.CustomerOperatorName, item.CustomerOperatorCode);
            if (getByCompany != null)
            {
                return getByCompany.id;
            }
            if (getByStorage != null)
            {
                return getByStorage.id;
            }
            return AddStorage(item);
        }
        public static Guid AddStorage(PRD_EntegrationAction item)
        {
            var db = new WorkOfTimeDatabase();
            var id = Guid.NewGuid();
            db.InsertCMP_Company(new CMP_Company
            {
                id = id,
                code = item.CustomerOperatorCode,
                created = DateTime.Now,
                createdby = Guid.Empty,
                name = item.CustomerOperatorName,
                description="Otomatik Oluşturulmuştur",
                taxNumber=item.TaxNumber,
                pid=item.DistributorId
            }) ;
            return id;
        }
        public static VWCMP_Storage FindStorage(string name, string code)
        {
            var db = new WorkOfTimeDatabase();
            return db.GetVWCMP_StorageByNameOrCode(name, code);
        }
    }
}
