using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHDM_TicketMessage : InfolineTable
    {
        public Guid? ticketId { get; set;}
        public short? type { get; set;}
        public string content { get; set;}
        public string cc { get; set;}
        public string bcc { get; set;}
        public string mailto { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string ticketId_Title { get; set;}
        public string type_Title { get; set;}
        public string createdby_Photo { get; set;}
    }
}
