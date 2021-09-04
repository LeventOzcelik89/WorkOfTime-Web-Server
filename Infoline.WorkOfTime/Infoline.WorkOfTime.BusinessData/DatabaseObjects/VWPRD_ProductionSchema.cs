using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPRD_ProductionSchema : InfolineTable
    {
        public string code { get; set;}
        public string name { get; set;}
        public Guid? productId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string productId_Title { get; set;}
    }
}
