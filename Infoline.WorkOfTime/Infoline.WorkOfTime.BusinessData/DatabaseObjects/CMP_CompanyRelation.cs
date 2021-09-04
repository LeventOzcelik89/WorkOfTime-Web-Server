using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_CompanyRelation : InfolineTable
    {
        public Guid? companyId { get; set;}
        public Guid? customerId { get; set;}
    }
}
