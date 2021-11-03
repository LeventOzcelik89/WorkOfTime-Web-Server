using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class CMP_CompanyFileSelector : InfolineTable
    {
        /// <summary>
        /// Sys files tablosundaki file group alanının tutulduğu yerdir
        /// </summary>
        public string fileGroupName { get; set;}
        /// <summary>
        /// Sys files tablosundaki data table alanının tutulduğu yerdir
        /// </summary>
        public string fileGroupModule { get; set;}
        public Guid customerId { get; set;}
    }
}
