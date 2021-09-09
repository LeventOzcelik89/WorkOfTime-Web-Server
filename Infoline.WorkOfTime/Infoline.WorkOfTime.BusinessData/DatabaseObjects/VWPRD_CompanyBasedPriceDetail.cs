using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_CompanyBasedPriceDetail : InfolineTable
    {
        public Guid companyBasedPriceId { get; set;}
        public double? minCondition { get; set;}
        public short? type { get; set;}
        public double? discount { get; set;}
        public int? monthCount { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string CompanyId_Title { get; set;}
        public Guid? CompanyId { get; set;}
        public string productId_Title { get; set;}
        public Guid? productId { get; set;}
        public string categoryId_Title { get; set;}
        public Guid? categoryId { get; set;}
    }
}
