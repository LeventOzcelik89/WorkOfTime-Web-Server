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
        [Description("Cihaz Teslim Alındı"), Generic("icon", "fa fa-bookmark",  "description", "Cihaz Teslim Alındı")]
        DeviceHanded=0,
        [Description("Tespit"), Generic("icon", "fa fa-cogs", "description", "Cihazın Sornuları Tespit Ediliyor")]
        Detection = 1,
        [Description("Kullanıcı Onayı"), Generic("icon", "fa fa-user", "description", "Cihazın Problemlerinin Tamiri İçin Kullanıcı Onayına Gitti")]
        UserPermission = 2,
        [Description("Tamir Başladı"), Generic("icon", "fa fa-wrench", "description", "Tamir Süreci Başladı")]
        Fixing= 3,
        [Description("Müşteriye Teslim"), Generic("icon", "fa fa-truck", "description", "Kalite Kontrol Başarılı, Cihaz Müşteriye Telsim Ediliyor")]
        Delivery = 4,
    }
    [EnumInfo(typeof(SV_Service), "Actions")]
    public enum EnumSV_ServiceActions
    {
        [Description("Transfer Süreci Başlat"), Generic("icon", "fa fa-truck", "color", "00ff00", "description", "Transfer Süreci Başlat","attr", "data-model=true id=transferstart data-href=/SV/VWSV_Service/Transfer? data-task=Insert data-method=GET ")]
        TransferStart = 0,
        [Description("Süreci İptal Et"), Generic("icon", "fa fa-cogs", "color", "ff0000", "description", "Süreci İptal Et")]
        Cancel = 1,
        [Description("Süreci Durdur"), Generic("icon", "fa fa-times", "color", "FFBF00", "description", "Süreci Durdur")]
        Stop = 2,
        [Description("Transfer Sürecini Bitir"), Generic("icon", "fa fa-wrench", "color", "DAF7A6", "description", "Transfer Sürecini Bitir")]
        TransferEnds = 4,
        [Description("Diğer Aşamaya Geç"), Generic("icon", "fa fa-wrench", "color", "DAF7A6", "description", "Diğer Aşamaya Geç")]
        NextStage = 5,
        [Description("Fire Bildirimi Yap"), Generic("icon", "fa fa-trash", "color", "dc1212", "description", "Fire Bildirimi Yap")]
        Fire= 100,
        [Description("Harcama Bildirimi Yap"), Generic("icon", "fa fa-cubes", "color", "dc1212", "description", "Harcama Bildirimi Yap")]
        Harcama = 101,
        [Description("Yeni Imei Ata"), Generic("icon", "fa fa-serialcode", "color", "dc1212", "description", "Yeni Imei Ata")]
        NewImei = 102,
        [Description("Kalite Kontrol Başarısız"), Generic("icon", "fa fa-serialcode", "color", "dc1212", "description", "Kalite Kontrol Başarısız")]
        QualityControllNot= 200,
        [Description("Süreci Tamamla"), Generic("icon", "fa fa-check", "color", "dc1212", "description", "Süreci Tamamla")]
        Done = 300,
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
