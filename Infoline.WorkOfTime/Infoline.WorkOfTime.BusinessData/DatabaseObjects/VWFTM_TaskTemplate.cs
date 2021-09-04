using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskTemplate : InfolineTable
    {
        public string code { get; set;}
        public string name { get; set;}
        public short? type { get; set;}
        public Guid? companyId { get; set;}
        public IGeometry  location { get; set;}
        public short? priority { get; set;}
        public Guid? customerId { get; set;}
        public Guid? customerStorageId { get; set;}
        public Guid? companyCarId { get; set;}
        public int? estimatedTaskMinute { get; set;}
        public string description { get; set;}
        public bool? hasVerifyCode { get; set;}
        public bool? sendMail { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string type_Title { get; set;}
        public string priority_Title { get; set;}
        public string customer_Title { get; set;}
        public string customerStorage_Title { get; set;}
        public string company_Title { get; set;}
        public string assignableUserIds { get; set;}
        public string assignableUserTitles { get; set;}
        public string helperUserTitles { get; set;}
        public string taskSubjectType_Title { get; set;}
        public string plate { get; set;}
        public int? createdTask_Count { get; set;}
        public int? usedTaskPlan_Count { get; set;}
        public int? inventory_Count { get; set;}
        public string inventory_Title { get; set;}
    }
}
