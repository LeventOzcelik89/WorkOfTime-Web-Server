using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskSubject : InfolineTable
    {
        public Guid? pid { get; set;}
        public string name { get; set;}
        public int? generation { get; set;}
    }
}
