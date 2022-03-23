using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_Bounty : InfolineTable
    {
        public Guid? companyId { get; set;}
        public Guid? productBonusId { get; set;}
        public double? productTotalPrice { get; set;}
        public double? additionalPrice { get; set;}
        public DateTime? paymentDate { get; set;}
        public short? status { get; set;}
    }
}
