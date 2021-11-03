using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_CompanyFileSelector : InfolineTable
    {
        public string fileGroupName { get; set;}
        public string fileGroupModule { get; set;}
        public Guid customerId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string CustomerId_Title { get; set;}
    }
}
