using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskForm : InfolineTable
    {
        public string name { get; set;}
        public string code { get; set;}
        public short? type { get; set;}
        public string json { get; set;}
        public short? isActive { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string type_Title { get; set;}
        public string isActive_Title { get; set;}
        public int? usedCount { get; set;}
        public string productId_Title { get; set;}
        public string companyId_Title { get; set;}
        public string companyStorageId_Title { get; set;}
        public string inventories_Title { get; set;}
    }
}
