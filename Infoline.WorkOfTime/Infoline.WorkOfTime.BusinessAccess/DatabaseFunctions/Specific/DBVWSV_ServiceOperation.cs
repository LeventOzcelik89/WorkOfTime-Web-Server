using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SV_ServiceOperation), "status")]
    public enum EnumSV_ServiceOperation
    {
        [Description("Transfer Süreci Başladı"), Generic("icon", "fa fa-truck", "color", "2D7DD2", "description", "Transfer Süreci Başladı")]
        BeginTransfer = 0,
        [Description("Servis Süreci İptal Edildi"), Generic("icon", "fa fa-times", "color", "ff0000", "description", "Servis Süreci İptal Edildi")]
        Canceled = 1,
        [Description("Süreç Durduruldu"), Generic("icon", "fa fa-pause", "color", "FFBF00", "description", "Süreç Durduruldu")]
        Waiting = 2,
        [Description("Süreç Yeniden Başladı"), Generic("icon", "fa fa-stop", "color", "FFBF00", "description", "Süreç Yeniden Başladı")]
        Restart = 3,
        [Description("Transfer Süreci Bitti"), Generic("icon", "fa fa-truck", "color", "F45D01", "description", "Transfer Süreci Bitti")]
        TransferEnded = 4,
        [Description("Aşama Geçişi Yapıldı"), Generic("icon", "fa fa-arrow-right", "color", "97CC04", "description", "Aşama Bildirimi Yapıldı")]
        AsamaBildirimi = 5,
        [Description("Fire Bildirimi Yapıldı"), Generic("icon", "fa fa-trash", "color", "DD614A", "description", "Fire Bildirimi Yapıldı")]
        FireBildirimiYapildi = 100,
        [Description("Harcama Bildirildi"), Generic("icon", "fa fa-cubes", "color", "73A580", "description", "Harcama Bildirimi Yapıldı")]
        HarcamaBildirildi = 101,
        [Description("Yeni Imei Atandı"), Generic("icon", "fa fa-barcode", "color", "392061", "description", "Yeni Imei Atandı")]
        NewImei = 102,
        [Description("Servis Kaydı Güncellendi"), Generic("icon", "fa fa-edit", "color", "6D213C", "description", "Servis Kaydı Güncellendi")]
        Updated = 105,
        [Description("Servis Süreci Bitti"), Generic("icon", "fa fa-check-circle", "color", "2AF913", "description", "Servis Süreci Bitti")]
        Done = 106,
        [Description("Süreç Başladı"), Generic("icon", "fa fa-check", "color", "70E4EF", "description", "Süreç Başladı")]
        Started = 107,
        [Description("Parça Değişikliği Yapıldı"), Generic("icon", "fa fa-cogs", "color", "E3C16F", "description", "Parça Değişikliği Yapıldı")]
        PartChanged = 108,
        [Description("Değiştirilecek Parçalar Belirlendi"), Generic("icon", "fa fa-cogs", "color", "E3C16F", "description", "Parça Değişikliği Yapıldı")]
        PartDefinied = 108,
        [Description("Kalite Kontrol Başarısız"), Generic("icon", "fa fa-serialcode", "color", "dc1212", "description", "Kalite Kontrol Başarısız")]
        QualityControllNot = 200,
        [Description("Müşteri Onayına Sunuldu"), Generic("icon", "fa fa-user", "color", "B1B1F2", "description", "Kalite Kontrol Başarısız")]
        AskCustomer = 400,
    }
  
    partial class WorkOfTimeDatabase
    {
        public VWSV_ServiceOperation[] GetVWSV_ServiceOperationsByIdServiceId(Guid serviceId) {

            using (var db = GetDB()) {


                return db.Table<VWSV_ServiceOperation>().Where(x => x.serviceId == serviceId).Execute().ToArray();
            
            }
        
        
        }


    }
}
