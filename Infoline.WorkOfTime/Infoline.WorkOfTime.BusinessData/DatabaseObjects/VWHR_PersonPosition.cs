using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHR_PersonPosition : InfolineTable
    {
        public Guid? HrPersonId { get; set;}
        public Guid? HrPositionId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string HrPersonId_Title { get; set;}
        public string HrPositionId_Title { get; set;}
    }
}
