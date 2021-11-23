using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductBounty : InfolineTable
    {
        public double? price { get; set;}
        public Guid? companyId { get; set;}
        public Guid? personId { get; set;}
        public Guid? productId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productionCompanyId_Title { get; set;}
        public string productId_Title { get; set;}
        public string personId_Title { get; set;}
    }
}
