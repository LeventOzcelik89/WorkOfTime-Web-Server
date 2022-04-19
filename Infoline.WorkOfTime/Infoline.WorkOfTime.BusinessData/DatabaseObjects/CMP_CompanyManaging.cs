using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_CompanyManaging : InfolineTable
    {
        public Guid? userId { get; set;}
        public Guid? companyId { get; set;}
    }
}
