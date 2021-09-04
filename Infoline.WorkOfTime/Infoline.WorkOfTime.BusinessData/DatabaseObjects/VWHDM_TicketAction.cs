using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHDM_TicketAction : InfolineTable
    {
        public Guid? ticketId { get; set;}
        public short? type { get; set;}
        public string description { get; set;}
        public short? ticketStatus { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string ticketId_Title { get; set;}
        public string type_Title { get; set;}
        public string createdby_Photo { get; set;}
    }
}
