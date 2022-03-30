using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFVR_Favorites : InfolineTable
    {
        public Guid? userId { get; set;}
        public string Area { get; set;}
        public string Controller { get; set;}
        public string Method { get; set;}
        public int? Count { get; set;}
        public bool? Status { get; set;}
        public string Action { get; set;}
        public string Description { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userId_Title { get; set;}
    }
}
