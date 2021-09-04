using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWHDM_Suggestion : InfolineTable
    {
        public string title { get; set;}
        public string content { get; set;}
        public short? status { get; set;}
        public Guid? issueId { get; set;}
        public Guid? assignUserId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string status_Title { get; set;}
        public string issueId_Title { get; set;}
        public string assignUserId_Title { get; set;}
    }
}
