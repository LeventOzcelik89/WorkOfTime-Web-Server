using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;


namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(PRD_ProductionOperation), "status")]
    public enum EnumPRD_ProductionOperationStatus
    {
        [Description("Üretim Emri Verildi"), Generic("icon", "fa fa-bookmark", "color", "0E5B66", "description", "Üretim Emri Verildi")]
        UretimEmriVerildi = 0,
        [Description("Üretime Başlandı"), Generic("icon", "fa fa-play", "color", "0E5B66", "description", "Üretim Başlanması Bekleniyor")]
        UretimBasladi = 1,
        [Description("Üretim Durduruldu"), Generic("icon", "fa fa-pause", "color", "f8ac59", "description", "Üretim Durduruldu")]
        UretimDurduruldu = 2,
        [Description("Üretim Bitti"), Generic("icon", "fa fa-check-circle", "color", "173270", "description", "Üretim Bitti")]
        UretimBitti = 3,
        [Description("Üretim İptal Edildi"), Generic("icon", "fa fa-times", "color", "AD1000", "description", "Üretim İptal Edildi")]
        UretimIptalEdildi = 4,
        [Description("Aşama Geçişi Yapıldı"), Generic("icon", "fa fa-retweet", "color", "4f8ea5", "description", "Aşama Bildirimi Yapıldı")]
        AsamaBildirimi = 5,
        [Description("Harcama Bildirildi"), Generic("icon", "fa fa-cubes", "color", "4E5EF1", "description", "Harcama Bildirimi Yapıldı")]
        HarcamaBildirildi = 100,
        [Description("Fire Bildirimi Yapıldı"), Generic("icon", "fa fa-trash", "color", "21b9bb", "description", "Fire Bildirimi Yapıldı")]
        FireBildirimiYapildi = 101,
        [Description("Stoğa İade Edildi"), Generic("icon", "fa fa-reply", "color", "A77B13", "description", "Stoğa İade Edildi")]
        StogaIadeEdildi = 102,
        [Description("Personel Ataması Yapıldı"), Generic("icon", "fa fa-users", "color", "173270", "description", "Personel Ataması Yapıldı")]
        PersonelAtamasiYapildi = 200,
        [Description("Personel Üretimden Alındı"), Generic("icon", "fa fa-qrcode", "color", "000000", "description", "Personel Uretimden Alındı")]
        PersonelUretimdenAlindi = 201,
        [Description("Form Yüklendi"), Generic("icon", "fa fa-upload", "color", "00ad06", "description", "Form Yüklendi")]
        FormYuklendi = 202
    }
    
    partial class WorkOfTimeDatabase
    {
        public PRD_ProductionOperation[] GetPRD_ProductionOperationByProductionId(Guid productionId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductionOperation>().Where(x => x.productionId == productionId).Execute().ToArray();
            }
        }

        public PRD_ProductionOperation GetPRD_ProductionOperationByDataId(Guid dataId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductionOperation>().Where(x => x.dataId == dataId).Execute().FirstOrDefault();
            }
        }
    }
}
