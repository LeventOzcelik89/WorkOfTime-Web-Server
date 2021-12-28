using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SV_ServiceOperation), "status")]
    public enum EnumSV_ServiceOperation
    {
        [Description("Transfer Süreci Başladı"), Generic("icon", "fa fa-truck", "color", "0E5B66", "description", "Üretim Emri Verildi")]
        BeginTransfer = 0,
        [Description("Servis Süreci İptal Edildi"), Generic("icon", "fa fa-times", "color", "ED3535", "description", "Servis Süreci İptal Edildi")]
        Canceled = 1,
        [Description("Süreç Durduruldu"), Generic("icon", "fa fa-pause", "color", "1d6a75", "description", "Süreç Durduruldu")]
        Waiting = 2,
        [Description("Servis Süreci Bitti"), Generic("icon", "fa fa-check-circle", "color", "2AF913", "description", "Servis Süreci Bitti")]
        Finished = 3,
        [Description("Aşama Geçişi Yapıldı"), Generic("icon", "fa fa-retweet", "color", "ffa500", "description", "Aşama Bildirimi Yapıldı")]
        AsamaBildirimi = 4,
        [Description("Süreç Başladı"), Generic("icon", "fa fa-check", "color", "70E4EF", "description", "Süreç Başladı")]
        Started = 5,
        [Description("Form Yüklendi"), Generic("icon", "fa fa-upload", "color", "F4A698", "description", "Form Yüklendi")]
        FormYuklendi = 202,
        [Description("Harcama Bildirildi"), Generic("icon", "fa fa-cubes", "color", "73A580", "description", "Harcama Bildirimi Yapıldı")]
        HarcamaBildirildi = 100,
        [Description("Fire Bildirimi Yapıldı"), Generic("icon", "fa fa-trash", "color", "DD614A", "description", "Fire Bildirimi Yapıldı")]
        FireBildirimiYapildi = 101,
        [Description("Parça Değişikliği Yapıldı"), Generic("icon", "fa fa-cogs", "color", "E3C16F", "description", "Parça Değişikliği Yapıldı")]
        PartChanged = 102,
        [Description("Transfer Süreci Bitti"), Generic("icon", "fa fa-truck", "color", "BAAB68", "description", "Transfer Süreci Bitti")]
        TransferEnded = 103,
        [Description("Not Eklendi"), Generic("icon", "fa fa-note", "color", "946846", "description", "Not Eklendi")]
        NoteAdded = 104,
        [Description("Servis Kaydı Güncellendi"), Generic("icon", "fa fa-edit", "color", "6D213C", "description", "Servis Kaydı Güncellendi")]
        Updated = 105,
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
