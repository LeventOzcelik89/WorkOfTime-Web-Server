using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_PresentationInvoice : InfolineTable
    {
        public Guid? presentationId { get; set;}
        public Guid? invoiceId { get; set;}
        public int? type { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Presentation_Title { get; set;}
        public string type_Title { get; set;}
    }
}
