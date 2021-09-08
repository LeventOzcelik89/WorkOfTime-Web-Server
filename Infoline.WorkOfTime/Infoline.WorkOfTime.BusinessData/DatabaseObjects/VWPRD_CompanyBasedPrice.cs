using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_CompanyBasedPrice : InfolineTable
    {
        public Guid companyId { get; set;}
        public bool isSingle { get; set;}
        public short toFilterSpecified { get; set;}
        public double? minAmount { get; set;}
        public bool? isPrice { get; set;}
        public int? installmentAmount { get; set;}
        public double? price { get; set;}
        public double? discount { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
        public string companyId_Title { get; set;}
        public Guid? productId { get; set;}
        public short? sellingType { get; set;}
    }
}
