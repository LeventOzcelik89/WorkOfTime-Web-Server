using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWFTM_TaskGrid : InfolineTable
    {
        public string fixture_Title { get; set;}
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
        public short? sendMailCustomer { get; set;}
        public string sendedCustomer { get; set;}
        public string createdTime { get; set;}
        public string planStartDateTime { get; set;}
        public string dueDateTime { get; set;}
        public string createdby_Title { get; set;}
        public string type_Title { get; set;}
        public string customer_Title { get; set;}
        public string customerStorage_Title { get; set;}
        public string company_Title { get; set;}
        public string subjectTitles { get; set;}
        public string taskType_Title { get; set;}
        public string town { get; set;}
        public string city { get; set;}
        public string description { get; set;}
        public string personelDescription { get; set;}
        public string DurdurulmaDescription { get; set;}
        public string helperUserIds { get; set;}
        public string helperUserTitles { get; set;}
        public string lastOperationStatus_Title { get; set;}
        public int? lastOperationStatus { get; set;}
        public int? MemnuniyetAnketiDoldurulmaSayisi { get; set;}
        public string openAddressLocationId_Title { get; set;}
        public Guid? assignUserId { get; set;}
        public DateTime? lastOperationDate { get; set;}
        public int? SahaOperasyonGorevFormSayisi { get; set;}
        public int? DoldurulanGorevFormSayisi { get; set;}
        public int? DosyaSayisi { get; set;}
        public DateTime? GorevAtamaTarihi { get; set;}
        public DateTime? GorevUstlenilmeTarihi { get; set;}
        public DateTime? GorevBaslangicTarihi { get; set;}
        public DateTime? GorevBitisTarihi { get; set;}
        public DateTime? DurdurulmaTarihi { get; set;}
        public DateTime? DevamTarihi { get; set;}
        public string assignableUserIds { get; set;}
        public bool? isComplete { get; set;}
        public string assignUser_Title { get; set;}
        public string GecenSure { get; set;}
        public string CevapSure { get; set;}
        public string gorevBitisTarihiZamani { get; set;}
        public string groupName { get; set;}
    }
}
