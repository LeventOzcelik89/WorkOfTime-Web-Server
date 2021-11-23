using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductBounty : InfolineTable
    {
        public double? amount { get; set;}
        public Guid? companyId { get; set;}
        public Guid? personId { get; set;}
        public Guid? productId { get; set;}
        public int? year { get; set;}
        public int? month { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string companyId_Title { get; set;}
        public string productId_Title { get; set;}
        public string personId_Title { get; set;}
    }
}
