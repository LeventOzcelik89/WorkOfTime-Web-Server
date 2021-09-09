using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Agile Boardların tutulduğu tablodur.
    /// </summary>
    public partial class SH_AgileBoards : InfolineTable
    {
        public Guid? userId { get; set;}
        public string name { get; set;}
        public string description { get; set;}
        public string json { get; set;}
        public DateTime? lastUsedDate { get; set;}
    }
}
