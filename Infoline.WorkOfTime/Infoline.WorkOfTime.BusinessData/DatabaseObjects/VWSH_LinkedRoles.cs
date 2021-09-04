using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWSH_LinkedRoles : InfolineTable
    {
        public Guid? RoleId { get; set;}
        public Guid? InnerRoleId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string RolName { get; set;}
        public string InnerRolName { get; set;}
    }
}
