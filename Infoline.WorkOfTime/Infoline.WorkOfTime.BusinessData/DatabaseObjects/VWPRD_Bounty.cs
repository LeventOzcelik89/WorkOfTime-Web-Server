using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_Bounty : InfolineTable
    {
        public Guid? companyId { get; set;}
        public Guid? productBonusId { get; set;}
        public double? productTotalPrice { get; set;}
        public double? additionalPrice { get; set;}
        public DateTime? paymentDate { get; set;}
        public short? status { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productBonusId_Title { get; set;}
        public string companyId_Title { get; set;}
    }
}
