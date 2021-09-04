using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_InvoiceAction : InfolineTable
    {
        /// <summary>
        /// İşlemin açıklamasının tutulduğu alandır.
        /// </summary>
        public string description { get; set;}
        /// <summary>
        /// İşlemin yapıldığı faturanın tutulduğu alandır.
        /// </summary>
        public Guid? invoiceId { get; set;}
        /// <summary>
        /// İşlemin tipi tutulur. (Not, Onay, Red)
        /// </summary>
        public short? type { get; set;}
        public Guid? transformInvoiceId { get; set;}
    }
}
