using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_CompanyType : InfolineTable
    {
        public Guid? companyId { get; set;}
        public Guid? typesId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string companyId_Title { get; set;}
        public string typesId_Title { get; set;}
    }
}
