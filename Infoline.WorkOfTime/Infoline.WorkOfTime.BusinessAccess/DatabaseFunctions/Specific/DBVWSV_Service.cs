using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SV_Service), "DeliveryType")]
    public enum EnumSV_ServiceDeliveryType
    {
        [Description("Elden Teslim")]
        HandedOver = 1,
        [Description("Kargo")]
        Cargo = 2,
        [Description("Diğer")]
        Other = 3,
    }
    [EnumInfo(typeof(SV_Service), "CustomerType")]
    public enum EnumSV_ServiceCustomerType
    {
        [Description("Bayi")]
        Company = 1,
        [Description("Müşteri")]
        Customer = 2,

    }
    [EnumInfo(typeof(SV_Service), "stage")]
    public enum EnumSV_ServiceStages
    {
        [Description("Cihaz Teslim Alındı"), Generic("icon", "fa fa-bookmark", "color", "0267C1", "description", "Cihaz Teslim Alındı")]
        DeviceHanded=0,
        [Description("Tespit"), Generic("icon", "fa fa-cogs", "color", "0075C4", "description", "Cihazın Sorunlaru Tespit Ediliyor")]
        Detection = 1,
        [Description("Fiyat Onayı"), Generic("icon", "fa fa-user", "color", "EFA00B", "description", "Cihazın Problemlerinin Tamiri İçin Kullanıcı Onayına Gitti")]
        UserPermission = 2,
        [Description("Tamir Başladı"), Generic("icon", "fa fa-wrench", "color", "D65108", "description", "Tamir Süreci Başladı")]
        Fixing= 3,
        [Description("Kalite Kontrol"), Generic("icon", "fa fa-check", "color", "591F0A", "description", "Kalite Kontrol Süreci Başladı")]
        Qualitycontrol = 4,
        [Description("Müşteriye Teslim"), Generic("icon", "fa fa-truck", "color", "61E786", "description", "Kalite Kontrol Başarılı, Cihaz Müşteriye Telsim Ediliyor")]
        Delivery = 5,
    }
    [EnumInfo(typeof(SV_Service), "Actions")]
    public enum EnumSV_ServiceActions
    {
        [Description("Transfer Süreci Başlat"), Generic("icon", "fa fa-truck", "color", "2D7DD2", "description", "Transfer Süreci Başlat","attr", "data-model=true data-enum=0 id=transferstart data-href=/SV/VWSV_ServiceOperation/Transfer?serviceId={{}}&status=0 data-task=Insert data-method=GET ")]
        TransferStart = 0,
        [Description("Süreci İptal Et"), Generic("icon", "fa fa-times", "color", "ff0000", "description", "Süreci İptal Et", "attr", "data-model=true data-enum=1 id=transferstart  data-href=/SV/VWSV_ServiceOperation/Insert?serviceId={{}}&status=1 data-task=Insert data-method=GET ")]
        Cancel = 1,
        [Description("Süreci Durdur"), Generic("icon", "fa fa-pause", "color", "FFBF00", "description", "Süreci Durdur", "attr", "data-model=true data-enum=2 id=transferstart  data-href=/SV/VWSV_ServiceOperation/Insert?serviceId={{}}&status=2 data-task=Insert data-method=GET ")]
        Stop = 2,
        [Description("Süreci Yeniden Başlat"), Generic("icon", "fa fa-play", "color", "FFBF00", "description", "Süreci Yeniden Başlat", "attr", "data-model=true data-enum=3 id=transferstart  data-href=/SV/VWSV_ServiceOperation/Insert?serviceId={{}}&status=3 data-task=Insert data-method=GET ")]
        ReStart = 3,
        [Description("Transfer Sürecini Bitir"), Generic("icon", "fa fa-truck", "color", "F45D01", "description", "Transfer Sürecini Bitir", "attr", " data-enum=4 data-model=true id=transferend data-href=/SV/VWSV_ServiceOperation/Transfer?serviceId={{}}&status=4  data-task=Insert data-method=GET  ")]
        TransferEnds = 4,
        [Description("Diğer Aşamaya Geç"), Generic("icon", "fa fa-arrow-right", "color", "97CC04", "description", "{{stage}}","attr", "data-model=false data-enum=5 id=transferstart data-ask data-href=/SV/VWSV_ServiceOperation/NextStage?serviceId={{}}&status=5 data-task=Insert data-method=POST ")]
        NextStage = 5,
        [Description("Fire Bildirimi Yap"), Generic("icon", "fa fa-trash", "color", "8F2D56", "description", "Fire Bildirimi Yap","attr", "data-model=true data-enum=100 id=transferstart  data-href=/SV/VWSV_ServiceOperation/Upsert?serviceId={{}}&Type=14 data-task=Insert data-method=GET ")]
        Fire= 100,
        [Description("Harcama Bildirimi Yap"), Generic("icon", "fa fa-cubes", "color", "111D4A", "description", "Harcama Bildirimi Yap", "attr", "data-model=true data-enum=100 id=transferstart  data-href=/SV/VWSV_ServiceOperation/Upsert?serviceId={{}}&Type=100 data-task=Insert data-method=GET ")]
        Harcama = 101,
        [Description("Yeni Imei Ata"), Generic("icon", "fa fa-barcode", "color", "0496FF", "description", "Yeni Imei Ata", "attr", "data-model=true data-enum=102 id=transferstart  data-href=/SV/VWSV_ChangedDevice/Insert?serviceId={{}}&oldInventoryId=%% data-task=Insert data-method=GET ")]
        NewImei = 102,
        [Description("Cihaz Problemlerini Belirle"), Generic("icon", "fa fa-cogs", "color", "392061", "description", "Cihaz Problemlerini Belirle", "attr", "data-model=true data-enum=108 id=transferstart  data-href=/SV/VWSV_DeviceProblem/AddMultipleDeviceProblem?serviceId={{}}&productId={}&inventoryId=[] data-task=Insert data-method=GET ")]
        ChancingPart = 108,
        [Description("Kalite Kontrol Başarısız"), Generic("icon", "fa fa-exclamation-triangle", "color", "731963", "description", "Kalite Kontrol Başarısız", "attr", "data-model=true data-enum=200 id=transferstart  data-href=/SV/VWSV_ServiceOperation/QualityCheck?serviceId={{}}&status=False data-task=Insert data-method=POST")]
        QualityControllNot= 200,
        [Description("Kalite Kontrol Başarılı"), Generic("icon", "fa fa-check-circle", "color", "FF934F", "description", "Kalite Kontrol Başarılı", "attr", "data-model=true data-enum=201 id=transferstart  data-href=/SV/VWSV_ServiceOperation/QualityCheck?serviceId={{}}&status=True data-task=Insert data-method=POST")]
        QualityControl = 201,
        [Description("Süreci Tamamla"), Generic("icon", "fa fa-check", "color", "85BDBF", "description", "Süreci Tamamla", "attr", "data-model=true data-enum=300 id=transferstart  data-href=/SV/VWSV_ServiceOperation/Cargo?serviceId={{}} data-task=Insert data-method=GET ")]
        Done = 300,
        [Description("Müşteriye Mail Gönder"), Generic("icon", "fa fa-user", "color", "F45B69", "description", "Müşteriye Mail Gönder", "attr", "data-modal=true data-enum=400 id=transferstart  data-href=/SV/VWSV_ServiceOperation/Insert?serviceId={{}}&status=400 data-task=Insert data-method=GET ")]
        AskCustomer = 400,
        [Description("Teslim Çıktısı Al"), Generic("icon", "fa fa-print", "color", "84732B", "description", "Teslim Çıktısı Al", "attr", "data-modal=false data-blank=true  data-enum=500 id=transferstart  data-href=/SV/VWSV_Service/PrintEnd?id={{}} data-task=Insert data-method=GET ")]
        PrintEnd = 500,
    }
    partial class WorkOfTimeDatabase
    {
        public VWSV_Service GetVWSV_ServiceByCode(string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSV_Service>().Where(x => x.code == code).Execute().FirstOrDefault();
            }
        }
        public VWSV_Service GetVWSV_ServiceByCodeAndIdIsNotEqual(string code, Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSV_Service>().Where(x => x.code == code &&x.id==id).Execute().FirstOrDefault();
            }
        }
    }
}
