using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_CompanyType : InfolineTable
    {
        public Guid? companyId { get; set;}
        public Guid? typesId { get; set;}
    }
}
