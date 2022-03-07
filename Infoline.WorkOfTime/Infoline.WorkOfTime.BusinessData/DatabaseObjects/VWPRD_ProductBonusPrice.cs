using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductBonusPrice : InfolineTable
    {
        public double? unitPrice { get; set;}
        public Guid? productBonusId { get; set;}
        public Guid? productId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
        public string code { get; set;}
        public string ruleName { get; set;}
        public string query { get; set;}
        public double? present { get; set;}
    }
}
