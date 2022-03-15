using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskOld : InfolineTable
    {
        public string fixture_Title { get; set;}
        public string searchField { get; set;}
        public string product_Title { get; set;}
        public string customer_Title { get; set;}
        public string customerStorage_Title { get; set;}
        public string company_Title { get; set;}
        public Guid? assignUserId { get; set;}
        public string assignableUserIds { get; set;}
        public string assignableUserTitles { get; set;}
        public string helperUserTitles { get; set;}
        public string taskSubjectType_Title { get; set;}
        public bool? isComplete { get; set;}
        public int? lastOperationStatus { get; set;}
        public IGeometry  lastOperationLocation { get; set;}
        public DateTime? lastOperationDate { get; set;}
        public string description { get; set;}
        public string operationDescription { get; set;}
        public int? SahaOperasyonGorevFormSayisi { get; set;}
        public int? IslemSayisi { get; set;}
        public int? DoldurulanGorevFormSayisi { get; set;}
        public int? DosyaSayisi { get; set;}
        public string workingHour { get; set;}
        public string plate { get; set;}
        public string stopSubject_Titles { get; set;}
        public string lastOperationStatus_Title { get; set;}
        public string assignUser_Title { get; set;}
        public string code { get; set;}
        public short? type { get; set;}
        public Guid? companyId { get; set;}
        public bool? hasVerifyCode { get; set;}
        public Guid? fixtureId { get; set;}
        public IGeometry  location { get; set;}
        public short? priority { get; set;}
        public Guid? customerId { get; set;}
        public DateTime? dueDate { get; set;}
        public Guid? customerStorageId { get; set;}
        public DateTime? planStartDate { get; set;}
        public DateTime? notificationDate { get; set;}
        public Guid? companyCarId { get; set;}
        public Guid? taskPlanId { get; set;}
        public Guid? taskTemplateId { get; set;}
        public short? planLater { get; set;}
        public bool? sendMailCustomer { get; set;}
        public string sendedCustomer { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string type_Title { get; set;}
        public string priority_Title { get; set;}
    }
}
