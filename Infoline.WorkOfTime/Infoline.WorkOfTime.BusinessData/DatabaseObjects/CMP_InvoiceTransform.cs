using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_InvoiceTransform : InfolineTable
    {
        public Guid? invoiceIdFrom { get; set;}
        public Guid? invoiceIdTo { get; set;}
    }
}
