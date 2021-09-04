using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_UserEmailAccountShare : InfolineTable
    {
        public Guid? userIdFrom { get; set;}
        public Guid? userIdTo { get; set;}
        public bool? share { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userIdTo_Title { get; set;}
        public string userIdFrom_Title { get; set;}
        public string userIdFromEmail { get; set;}
    }
}
