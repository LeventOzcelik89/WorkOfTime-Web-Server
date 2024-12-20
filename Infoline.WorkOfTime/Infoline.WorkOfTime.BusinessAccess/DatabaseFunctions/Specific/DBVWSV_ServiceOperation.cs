﻿using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SV_ServiceOperation), "status")]
    public enum EnumSV_ServiceOperation
    {
        [Description("Süreç Başladı"), Generic("icon", "fa  fa-mail-forward", "color", "70E4EF", "description", "Süreç Başladı","order","1")]
        Started = 107,
        [Description("Servis Süreci Bitti"), Generic("icon", "fa fa-check-circle", "color", "2AF913", "description", "Servis Süreci Bitti", "order", "2")]
        Done = 300,
        [Description("Süreç Durduruldu"), Generic("icon", "fa fa-pause", "color", "FFBF00", "description", "Süreç Durduruldu", "order", "3")]
        Waiting = 2,
        [Description("Süreç Yeniden Başladı"), Generic("icon", "fa fa-stop", "color", "CA2E55", "description", "Süreç Yeniden Başladı", "order", "4")]
        Restart = 3,
        [Description("Aşama Geçişi Yapıldı"), Generic("icon", "fa fa-arrow-right", "color", "97CC04", "description", "Aşama Bildirimi Yapıldı", "order", "5")]
        AsamaBildirimi = 5,
        [Description("Transfer Süreci Başladı"), Generic("icon", "fa fa-truck", "color", "2D7DD2", "description", "Transfer Süreci Başladı", "order", "6")]
        BeginTransfer = 0,
        [Description("Transfer Süreci Bitti"), Generic("icon", "fa fa-truck", "color", "F45D01", "description", "Transfer Süreci Bitti", "order", "7")]
        TransferEnded = 4,
        [Description("Fire Bildirimi Yapıldı"), Generic("icon", "fa fa-trash", "color", "DD614A", "description", "Fire Bildirimi Yapıldı", "order", "8")]
        FireBildirimiYapildi = 100,
        [Description("Harcama Bildirildi"), Generic("icon", "fa fa-cubes", "color", "73A580", "description", "Harcama Bildirimi Yapıldı", "order", "9")]
        HarcamaBildirildi = 101,
        [Description("Yeni Imei Atandı"), Generic("icon", "fa fa-barcode", "color", "392061", "description", "Yeni Imei Atandı", "order", "10")]
        NewImei = 102,
        [Description("Teknik Servis Bulgusu Eklendi"), Generic("icon", "fa fa-cogs", "color", "392061", "description", "Teknik Servis Bulgusu Eklendi", "order", "11")]
        PartDefinied = 109,
        [Description("Teknik Servis Bulgusu Silindi"), Generic("icon", "fa fa-cogs", "color", "DD0426", "description", "Teknik Servis Bulgusu Silindi", "order", "12")]
        PartDeleted = 110,
        [Description("Parça Değişikliği Yapıldı"), Generic("icon", "fa fa-tasks", "color", "3083DC", "description", "Parça Değişikliği Yapıldı", "order", "13")]
        PartChanged = 108,
        [Description("Servis Kaydı Güncellendi"), Generic("icon", "fa fa-edit", "color", "6D213C", "description", "Servis Kaydı Güncellendi", "order", "14")]
        Updated = 105,
        [Description("Kalite Kontrol Başarısız"), Generic("icon", "fa fa-exclamation-triangle", "color", "731963", "description", "Kalite Kontrol Başarısız", "order", "15")]
        QualityControlNot = 200,
        [Description("Kalite Kontrol Başarılı"), Generic("icon", "fa fa-exclamation-triangle", "color", "7DDE92", "description", "Kalite Kontrol Başarılı", "order", "16")]
        QualityControl = 201,
        [Description("Fiyat Onayına Sunuldu"), Generic("icon", "fa fa-user", "color", "B1B1F2", "description", "Müşteri Onayına Sunuldu", "order", "17")]
        AskCustomer = 400,
        [Description("Hizmet Ücreti Eklendi"), Generic("icon", "fa fa-money", "color", "B1B1F2", "description", "Hizmet Eklendi", "order", "18")]
        ServicePriceAdded = 500,
        [Description("Servis Süreci İptal Edildi"), Generic("icon", "fa fa-times", "color", "ff0000", "description", "Servis Süreci İptal Edildi", "order","19")]
        Canceled = 1,
        [Description("Fiyat Onayı Alındı"), Generic("icon", "fa fa-check ", "color", "04C8B4", "description", "Fiyat Onayı Alındı","order","20")]
        CostAccepted = 501,
        [Description("Fiyat Onayı Reddedildi"), Generic("icon", "fa  fa-exclamation-triangle", "color", "E51010", "description", "Fiyat Onayı Reddedildi", "order", "20")]
        CostDenied = 502,


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
