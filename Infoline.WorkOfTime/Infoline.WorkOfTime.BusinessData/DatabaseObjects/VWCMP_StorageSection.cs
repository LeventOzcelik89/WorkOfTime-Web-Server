using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWCMP_StorageSection : InfolineTable
    {
        public string productCount { get; set;}
        public Guid? pid { get; set;}
        public Guid? companyId { get; set;}
        public Guid? storageId { get; set;}
        public string code { get; set;}
        public string name { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string pid_Title { get; set;}
        public string companyId_Title { get; set;}
        public string storageId_Title { get; set;}
    }
}
