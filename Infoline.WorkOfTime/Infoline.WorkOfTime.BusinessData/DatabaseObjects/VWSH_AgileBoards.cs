using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_AgileBoards : InfolineTable
    {
        public Guid? userId { get; set;}
        public string name { get; set;}
        public string description { get; set;}
        public string json { get; set;}
        public DateTime? lastUsedDate { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userId_Title { get; set;}
    }
}
