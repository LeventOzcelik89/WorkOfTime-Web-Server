using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_CompanyManaging : InfolineTable
    {
        public Guid? userId { get; set;}
        public Guid? companyId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string userId_Title { get; set;}
        public string companyId_Title { get; set;}
    }
}
