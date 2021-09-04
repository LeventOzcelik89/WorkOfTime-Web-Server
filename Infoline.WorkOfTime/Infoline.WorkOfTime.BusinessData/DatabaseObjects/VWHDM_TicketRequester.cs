using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHDM_TicketRequester : InfolineTable
    {
        public string fullName { get; set;}
        public string email { get; set;}
        public string phone { get; set;}
        public string company { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
    }
}
