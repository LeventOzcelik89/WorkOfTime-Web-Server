using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CRM_PresentationInvoice : InfolineTable
    {
        public Guid? presentationId { get; set;}
        public Guid? invoiceId { get; set;}
        public int? type { get; set;}
    }
}
