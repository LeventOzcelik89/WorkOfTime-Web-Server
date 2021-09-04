using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class SH_FilesRole : InfolineTable
    {
        public Guid? roleid { get; set;}
        public string fileGroup { get; set;}
        public string dataTable { get; set;}
        public bool? insert { get; set;}
        public bool? delete { get; set;}
        public bool? preview { get; set;}
    }
}
