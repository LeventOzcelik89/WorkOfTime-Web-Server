using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_Storage : InfolineTable
    {
        public string fullName { get; set;}
        public string pid_Title { get; set;}
        public string searchField { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string companyId_Title { get; set;}
        public string companyId_Image { get; set;}
        public string locationId_Title { get; set;}
        public string supervisorId_Title { get; set;}
        public bool? myStorage { get; set;}
        public Guid? companyId { get; set;}
        public string code { get; set;}
        public string name { get; set;}
        public IGeometry  location { get; set;}
        public string address { get; set;}
        public Guid? locationId { get; set;}
        public Guid? supervisorId { get; set;}
        public string phone { get; set;}
        public string fax { get; set;}
        public Guid? pid { get; set;}
        public string email { get; set;}
    }
}
