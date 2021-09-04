using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_Schools : InfolineTable
    {
        public string SchoolName { get; set;}
        public int? Type { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Type_Title { get; set;}
    }
}
