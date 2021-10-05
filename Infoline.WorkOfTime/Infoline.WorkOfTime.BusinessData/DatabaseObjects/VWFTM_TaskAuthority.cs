using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskAuthority : InfolineTable
    {
        public Guid? projectId { get; set;}
        public Guid? customerId { get; set;}
        public Guid? userId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userId_Title { get; set;}
        public string projectId_Title { get; set;}
        public string customerId_Title { get; set;}
    }
}
