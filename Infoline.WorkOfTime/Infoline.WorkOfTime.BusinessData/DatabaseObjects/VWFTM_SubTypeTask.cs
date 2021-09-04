using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_SubTypeTask : InfolineTable
    {
        public Guid? taskId { get; set;}
        public Guid? subTypeId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string subTypeId_Title { get; set;}
    }
}
