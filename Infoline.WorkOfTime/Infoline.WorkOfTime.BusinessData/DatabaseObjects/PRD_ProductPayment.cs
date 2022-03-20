using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class PRD_ProductPayment : InfolineTable
    {
        public Guid? companyId { get; set;}
        public double? totalPrice { get; set;}
        public int? count { get; set;}
        public DateTime? date { get; set;}
        public Guid? productProgressPaymentId { get; set;}
        public short? hasThePayment { get; set;}
    }
}
