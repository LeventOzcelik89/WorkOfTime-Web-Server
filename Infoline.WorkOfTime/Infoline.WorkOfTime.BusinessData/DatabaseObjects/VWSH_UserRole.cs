using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_UserRole : InfolineTable
    {
        public Guid? userid { get; set;}
        public Guid? roleid { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string User_Title { get; set;}
        public string Role_Title { get; set;}
    }
}
