using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_Task : InfolineTable
    {
        public string fixture_Title { get; set;}
        public string searchField { get; set;}
        public int SLAStatus { get; set;}
        public Guid? scopeLevelId { get; set;}
        public Guid? typeLevelId { get; set;}
        public string resolutionDate_Title { get; set;}
        public string closingDate_Title { get; set;}
        public string scopeId_levelTitle { get; set;}
        public string subjectId_LevelTitle { get; set;}
        public DateTime? penaltyStartDate { get; set;}
        public double? amercementTotal { get; set;}
        public string SLAText { get; set;}
        public short? amercement { get; set;}
        public short? resolutionTime { get; set;}
        public short? resolutionType { get; set;}
        public bool? isWorkingTime { get; set;}
        public string customer_openAddressCityId_Title { get; set;}
        public Guid? customer_openAddressCityId { get; set;}
        public string product_Title { get; set;}
        public string taskPlanId_Title { get; set;}
        public string customerStorage_Title { get; set;}
        public string company_Title { get; set;}
        public string assignableUserIds { get; set;}
        public string assignableUserTitles { get; set;}
        public string helperUserIds { get; set;}
        public string helperUserTitles { get; set;}
        public string taskSubjectType_Title { get; set;}
        public bool? isComplete { get; set;}
        public int? transactionCount { get; set;}
        public string operationDescription { get; set;}
        public int? SahaOperasyonGorevFormSayisi { get; set;}
        public int? IslemSayisi { get; set;}
        public int? DoldurulanGorevFormSayisi { get; set;}
        public int? MemnuniyetAnketiDoldurulmaSayisi { get; set;}
        public Guid? memnuniyetAnketId { get; set;}
        public int? DosyaSayisi { get; set;}
        public string workingHour { get; set;}
        public string plate { get; set;}
        public string stopSubject_Titles { get; set;}
        public Guid? projectId { get; set;}
        public string assignUserPhoto { get; set;}
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
        public string company_Code { get; set;}
        public string customer_Code { get; set;}
        public string customer_Title { get; set;}
        public Guid? customer_openAddressLocationId { get; set;}
        public Guid? customerStorage_openAddressLocationId { get; set;}
        public string customer_OpenAdress { get; set;}
        public DateTime? resolutionDate { get; set;}
        public DateTime? rejectedDate { get; set;}
        public DateTime? closingDate { get; set;}
        public string lastOperationStatus_Title { get; set;}
        public int? lastOperationStatus { get; set;}
        public IGeometry  lastOperationLocation { get; set;}
        public DateTime? lastOperationDate { get; set;}
        public string description { get; set;}
        public Guid? assignUserId { get; set;}
        public string assignUser_Title { get; set;}
        public bool? hasProblem { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string type_Title { get; set;}
        public string planLater_Title { get; set;}
        public string priority_Title { get; set;}
    }
}
