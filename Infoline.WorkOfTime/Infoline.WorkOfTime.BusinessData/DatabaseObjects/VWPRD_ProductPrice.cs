using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductPrice : InfolineTable
    {
        public Guid? productId { get; set;}
        public double? price { get; set;}
        public short? type { get; set;}
        public Guid? currencyId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
        public string type_Title { get; set;}
        public string currencyId_Title { get; set;}
    }
}
