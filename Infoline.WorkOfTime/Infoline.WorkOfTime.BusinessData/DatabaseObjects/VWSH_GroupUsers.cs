using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_GroupUsers : InfolineTable
    {
        public Guid? userId { get; set;}
        public Guid? groupId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userId_Title { get; set;}
        public string groupId_Title { get; set;}
    }
}
