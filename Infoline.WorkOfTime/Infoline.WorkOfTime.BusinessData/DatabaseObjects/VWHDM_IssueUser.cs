using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHDM_IssueUser : InfolineTable
    {
        public Guid? issueId { get; set;}
        public Guid? userId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string issueId_Title { get; set;}
        public string userId_Title { get; set;}
    }
}
