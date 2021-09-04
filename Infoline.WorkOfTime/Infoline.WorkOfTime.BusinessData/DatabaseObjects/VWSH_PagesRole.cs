using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_PagesRole : InfolineTable
    {
        public Guid? roleid { get; set;}
        public string action { get; set;}
    }
}
