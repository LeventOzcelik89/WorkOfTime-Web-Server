using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_CompanyCarKilometer : InfolineTable
    {
        public DateTime? entryDate { get; set;}
        public double? kilometer { get; set;}
        public IGeometry  location { get; set;}
        public string image { get; set;}
        public Guid? companyCarId { get; set;}
    }
}
