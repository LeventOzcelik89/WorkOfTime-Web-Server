using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_PresentationAction : InfolineTable
    {
        public string description { get; set;}
        public Guid? presentationId { get; set;}
        public short? type { get; set;}
        public string color { get; set;}
        public Guid? contactId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string type_Title { get; set;}
    }
}
