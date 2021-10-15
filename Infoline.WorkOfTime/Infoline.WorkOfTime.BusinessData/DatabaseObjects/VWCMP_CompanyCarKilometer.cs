using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_CompanyCarKilometer : InfolineTable
    {
        public DateTime? entryDate { get; set;}
        public double? kilometer { get; set;}
        public IGeometry  location { get; set;}
        public string image { get; set;}
        public Guid? companyCarId { get; set;}
        public Guid? commissionId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string carPlate { get; set;}
        public string carBrand { get; set;}
        public string carModel { get; set;}
        public Guid? responsiblePersonId { get; set;}
    }
}
