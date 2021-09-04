using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRJ_ProjectInvoice : InfolineTable
    {
        public Guid? invoiceId { get; set;}
        public Guid? projectId { get; set;}
    }
}
