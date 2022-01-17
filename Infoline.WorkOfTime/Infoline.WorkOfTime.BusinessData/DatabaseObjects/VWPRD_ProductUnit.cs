using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductUnit : InfolineTable
    {
        public Guid? productId { get; set;}
        public Guid? unitId { get; set;}
        public double? quantity { get; set;}
        public short? isDefault { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string unitId_Title { get; set;}
        public string isDefault_Title { get; set;}
    }
}
