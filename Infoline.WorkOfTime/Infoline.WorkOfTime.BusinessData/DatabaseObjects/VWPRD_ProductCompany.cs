using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductCompany : InfolineTable
    {
        public Guid? productId { get; set;}
        public Guid? companyId { get; set;}
        public int? type { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
        public string companyId_Title { get; set;}
        public string type_Title { get; set;}
        public string companyId_Image { get; set;}
    }
}
