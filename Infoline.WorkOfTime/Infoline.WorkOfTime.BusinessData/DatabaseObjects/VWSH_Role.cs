using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_Role : InfolineTable
    {
        public string rolname { get; set;}
        public string roledescription { get; set;}
        public short? roletype { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string roleType_Title { get; set;}
        public string Users_Title { get; set;}
    }
}
