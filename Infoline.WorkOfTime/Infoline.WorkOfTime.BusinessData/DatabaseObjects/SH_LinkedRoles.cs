using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    /// <summary>
    /// Bağlantılı rollerin tutulduğu tablodur.
    /// </summary>
    public partial class SH_LinkedRoles : InfolineTable
    {
        /// <summary>
        /// asıl rolün id bilgisinin tutulduğu alandır. buradaki role başka rol eklenir.
        /// </summary>
        public Guid? RoleId { get; set;}
        /// <summary>
        /// asıl role başka rol ilişkilendirilrken ilişkilendirilecek rol id bilgisi burada tutulur.
        /// </summary>
        public Guid? InnerRoleId { get; set;}
    }
}
