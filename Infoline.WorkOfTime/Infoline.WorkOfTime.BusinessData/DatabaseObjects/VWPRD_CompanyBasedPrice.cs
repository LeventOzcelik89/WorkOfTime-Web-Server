using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_CompanyBasedPrice : InfolineTable
    {
        public Guid? companyId { get; set;}
        public Guid? productId { get; set;}
        public Guid? categoryId { get; set;}
        public short? conditionType { get; set;}
        public short? sellingType { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
        public string companyId_Title { get; set;}
        public string categoryId_Title { get; set;}
    }
}
