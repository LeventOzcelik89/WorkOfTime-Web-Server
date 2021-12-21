using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;
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
        [Description("Süreç Başladı"), Generic("icon", "fa fa-check", "color", "dd1212", "description", "Süreç Başladı")]
        Started = 5,
        [Description("Form Yüklendi"), Generic("icon", "fa fa-upload", "color", "E56464", "description", "Form Yüklendi")]
        FormYuklendi = 202,
        [Description("Harcama Bildirildi"), Generic("icon", "fa fa-cubes", "color", "4E5EF1", "description", "Harcama Bildirimi Yapıldı")]
        HarcamaBildirildi = 100,
        [Description("Fire Bildirimi Yapıldı"), Generic("icon", "fa fa-trash", "color", "dc1212", "description", "Fire Bildirimi Yapıldı")]
        FireBildirimiYapildi = 101,
        [Description("Parça Değişikliği Yapıldı"), Generic("icon", "fa fa-cogs", "color", "dd1512", "description", "Parça Değişikliği Yapıldı")]
        PartChanged = 102,
        [Description("Transfer Süreci Bitti"), Generic("icon", "fa fa-truck", "color", "dd1312", "description", "Transfer Süreci Bitti")]
        TransferEnded = 103,
        [Description("Not Eklendi"), Generic("icon", "fa fa-note", "color", "dd1312", "description", "Not Eklendi")]
        NoteAdded = 104,
    }
    partial class WorkOfTimeDatabase
    {
    }
}
