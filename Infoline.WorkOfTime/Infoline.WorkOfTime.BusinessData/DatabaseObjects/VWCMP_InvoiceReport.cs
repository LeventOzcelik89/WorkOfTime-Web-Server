using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_InvoiceReport : InfolineTable
    {
        public string SerialNumber_Title { get; set;}
        public string fullName { get; set;}
        public string searchField { get; set;}
        public double? totalAmountAsTL { get; set;}
        public string Project_Title { get; set;}
        public double? totalTax { get; set;}
        public double? totalSubAmount { get; set;}
        public double? totalAmount { get; set;}
        public double? totalTaxAsTL { get; set;}
        public double? totalSubAmountAsTL { get; set;}
        public Guid? projectId { get; set;}
        public string coderequest { get; set;}
        public short? direction { get; set;}
        public short? status { get; set;}
        public short? type { get; set;}
        public string description { get; set;}
        public string serialNumber { get; set;}
        public string rowNumber { get; set;}
        public Guid? supplierId { get; set;}
        public Guid? customerId { get; set;}
        public Guid? currencyId { get; set;}
        public DateTime? issueDate { get; set;}
        public DateTime? expiryDate { get; set;}
        public short? stopaj { get; set;}
        public double? discount { get; set;}
        public short? discountType { get; set;}
        public double? discountPercent { get; set;}
        public double? tevkifat { get; set;}
        public DateTime? sendingDate { get; set;}
        public short? paymentType { get; set;}
        public short? installmentCount { get; set;}
        public double? rateExchange { get; set;}
        public string customerTaxNumber { get; set;}
        public string customerTaxOffice { get; set;}
        public string customerAdress { get; set;}
        public string customerTitle { get; set;}
        public Guid? taskId { get; set;}
        public Guid? pid { get; set;}
        public string supplierTaxNumber { get; set;}
        public string supplierTaxOffice { get; set;}
        public string supplierAdress { get; set;}
        public string supplierTitle { get; set;}
        public string type_Title { get; set;}
        public string discountType_Title { get; set;}
        public string status_Title { get; set;}
        public string paymentType_Title { get; set;}
        public string direction_Title { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Supplier_Title { get; set;}
        public string Customer_Title { get; set;}
        public string Currency_Title { get; set;}
        public string Currency_Symbol { get; set;}
        public string Currency_Code { get; set;}
    }
}
