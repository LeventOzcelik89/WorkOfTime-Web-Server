using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductPointSelling : InfolineTable
    {
        public Guid? productId { get; set;}
        public double? point { get; set;}
        public DateTime? startDate { get; set;}
        public DateTime? endDate { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
    }
}
