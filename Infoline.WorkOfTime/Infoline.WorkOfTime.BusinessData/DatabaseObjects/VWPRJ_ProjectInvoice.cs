using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRJ_ProjectInvoice : InfolineTable
    {
        public Guid? invoiceId { get; set;}
        public Guid? projectId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Project_Title { get; set;}
    }
}
