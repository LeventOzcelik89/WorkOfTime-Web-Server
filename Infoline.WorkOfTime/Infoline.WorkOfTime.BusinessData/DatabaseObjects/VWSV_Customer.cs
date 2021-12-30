using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSV_Customer : InfolineTable
    {
        public string fullName { get; set;}
        public string name { get; set;}
        public string lastName { get; set;}
        public string phoneNumber { get; set;}
        public string email { get; set;}
        public string otherPhoneNumber { get; set;}
        public Guid? openLocationId { get; set;}
        public string Address { get; set;}
        public string code { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string openAddressLocationId_Title { get; set;}
    }
}
