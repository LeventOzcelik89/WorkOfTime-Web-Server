using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskFormRelation : InfolineTable
    {
        public Guid? inventoryId { get; set;}
        public Guid? productId { get; set;}
        public Guid? formId { get; set;}
        public Guid? companyId { get; set;}
        public Guid? companyStorageId { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string inventoryId_Title { get; set;}
        public string productId_Title { get; set;}
        public string company_Title { get; set;}
        public string companyStorage_Title { get; set;}
        public string formId_Title { get; set;}
        public short? formId_Type { get; set;}
    }
}
