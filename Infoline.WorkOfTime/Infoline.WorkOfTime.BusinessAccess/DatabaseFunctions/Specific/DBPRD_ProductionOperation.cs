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
        [Description("Üretim Emri Verildi"), Generic("icon", "fa fa-bookmark", "color", "0E5B66", "description", "Personel Atamasının Yapılması Bekleniyor")]
        UretimEmriVerildi = 0,
        [Description("Üretime Başlandı"), Generic("icon", "fa fa-building", "color", "0E5B66", "description", "Personel Atamasının Yapılması Bekleniyor")]
        UretimBasladi = 1,
        [Description("Üretim Durduruldu"), Generic("icon", "fa fa-cogs", "color", "0E5B66", "description", "Personel Atamasının Yapılması Bekleniyor")]
        UretimDurduruldu = 2,
        [Description("Üretim Bitti"), Generic("icon", "fa fa-users", "color", "173270", "description", "Doğrulama Kodunun Gönderilmesi Bekleniyor")]
        UretimBitti = 3,
        [Description("Üretim İptal Edildi"), Generic("icon", "fa fa-barcode", "color", "23c6c8", "description", "Görevin Üstlenilmesi Bekleniyor")]
        UretimIptalEdildi = 4,
        [Description("Yeni Malzeme Eklendi"), Generic("icon", "fa fa-user-secret", "color", "4E5EF1", "description", "Görev Üzerindeki İşlemlerin Yapılması Bekleniyor")]
        YeniMalzemeEklendi = 100,
        [Description("Fire Bildirimi Yapıldı"), Generic("icon", "fa fa-play", "color", "4E5EF1", "description", "Görev Üzerindeki İşlemlerin Yapılması Bekleniyor")]
        FireBildirimiYapildi = 101,
        [Description("Stoğa İade Edildi"), Generic("icon", "fa fa-retweet", "color", "A77B13", "description", "Görev Üzerindeki İşlemlerin Yapılması Bekleniyor")]
        StogaIadeEdildi = 102,
        [Description("Personel Ataması Yapıldı"), Generic("icon", "fa fa-qrcode", "color", "000000", "description", "Görev Üzerindeki İşlemlerin Yapılması Bekleniyor")]
        PersonelAtamasiYapildi = 200,
        [Description("Personel Üretimden Alındı"), Generic("icon", "fa fa-qrcode", "color", "000000", "description", "Görev Üzerindeki İşlemlerin Yapılması Bekleniyor")]
        PersonelUretimdenAlindi = 201,
        [Description("Form Yüklendi"), Generic("icon", "fa fa-file-text", "color", "A77B13", "description", "Çözümün Bildirilmesi Bekleniyor")]
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
    }
}
