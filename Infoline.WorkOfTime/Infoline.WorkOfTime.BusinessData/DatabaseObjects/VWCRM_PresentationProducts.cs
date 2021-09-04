using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCRM_PresentationProducts : InfolineTable
    {
        public Guid? PresentationId { get; set;}
        public Guid? ProductId { get; set;}
        public int? Amount { get; set;}
        public bool? IsNew { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Product_Title { get; set;}
        public string Presentation_Title { get; set;}
        public string unit_Title { get; set;}
        public double? sellingPrice { get; set;}
        public double? CurrentPoint { get; set;}
    }
}
