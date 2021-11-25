using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;


namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(FTM_TaskOperation), "status")]
    public enum EnumFTM_TaskOperationStatus
    {
        [Description("Görev Oluşturuldu"), Generic("icon", "fa fa-bookmark", "color", "0E5B66", "description", "Personel Atamasının Yapılması Bekleniyor")]
        GorevOlusturuldu = 0,
        [Description("Görev Oluşturuldu (Müşteri)"), Generic("icon", "fa fa-building", "color", "0E5B66", "description", "Personel Atamasının Yapılması Bekleniyor")]
        GorevOlusturulduMusteri = 1,
        [Description("Görev Oluşturuldu (Sistem)"), Generic("icon", "fa fa-cogs", "color", "0E5B66", "description", "Personel Atamasının Yapılması Bekleniyor")]
        GorevOlusturulduSistem = 2,
        [Description("Personel Ataması Yapıldı"), Generic("icon", "fa fa-users", "color", "173270", "description", "Doğrulama Kodunun Gönderilmesi Bekleniyor")]
        PersonelAtamaYapildi = 10,
        [Description("Doğrulama Kodu Gönderildi"), Generic("icon", "fa fa-barcode", "color", "23c6c8", "description", "Görevin Üstlenilmesi Bekleniyor")]
        DogrulamaKoduGonderildi = 11,
        [Description("Görev Üstlenildi"), Generic("icon", "fa fa-user-secret", "color", "4E5EF1", "description", "Görev Üzerindeki İşlemlerin Yapılması Bekleniyor")]
        GorevUstlenildi = 12,

        [Description("Göreve Başlandı"), Generic("icon", "fa fa-play", "color", "4E5EF1", "description", "Görev Üzerindeki İşlemlerin Yapılması Bekleniyor")]
        GorevBaslandi = 13,
        [Description("Ürün Değişimi"), Generic("icon", "fa fa-retweet", "color", "A77B13", "description", "Görev Üzerindeki İşlemlerin Yapılması Bekleniyor")]
        UrunDegisimi = 19,
        [Description("İşlem Yapılıyor"), Generic("icon", "fa fa-qrcode", "color", "000000", "description", "Görev Üzerindeki İşlemlerin Yapılması Bekleniyor")]
        GorevIslemYapiliyorAnaUrun = 20,
        [Description("İşlem Yapılıyor"), Generic("icon", "fa fa-qrcode", "color", "000000", "description", "Görev Üzerindeki İşlemlerin Yapılması Bekleniyor")]
        GorevIslemYapiliyorAltUrun = 21,
        [Description("Görev Formu dolduruldu"), Generic("icon", "fa fa-file-text", "color", "A77B13", "description", "Çözümün Bildirilmesi Bekleniyor")]
        GorevFormuDoldurulduAnaUrun = 22,
        [Description("Görev Formu dolduruldu"), Generic("icon", "fa fa-file-text", "color", "A77B13", "description", "Çözümün Bildirilmesi Bekleniyor")]
        GorevFormuDoldurulduAltUrun = 23,
        [Description("Göreve Dosya (Resim) Yüklendi"), Generic("icon", "fa fa-image", "color", "4f8ea5", "description", "Görev Formunun Doldurulması Bekleniyor")]
        GorevDosyaIslemiAnaUrun = 24,
        [Description("Göreve Dosya (Resim) Yüklendi"), Generic("icon", "fa fa-image", "color", "4f8ea5", "description", "Görev Formunun Doldurulması Bekleniyor")]
        GorevDosyaIslemiAltUrun = 25,

        [Description("Görev Durduruldu"), Generic("icon", "fa fa-pause", "color", "f8ac59", "description", "Görev Yeniden Başlatılması Bekleniyor")]
        GorevDurduruldu = 26,
        [Description("Görev Yeniden Başlatıldı"), Generic("icon", "fa fa-step-forward", "color", "21b9bb", "description", "Görev Üzerindeki İşlemlerin Yapılması Bekleniyor")]
        GorevYenidenBaslatildi = 27,

        [Description("Çözüm Bildirildi"), Generic("icon", "fa fa-bell", "color", "f8ac59", "description", "Çözümün Onaylanması Bekleniyor")]
        CozumBildirildi = 30,
        [Description("Çözüm Onaylandı"), Generic("icon", "fa fa-check-circle", "color", "359700", "description", "Çözüm Onaylandı, Görev Tamamlandı")]
        CozumOnaylandi = 31,
        [Description("Çözüm Reddedildi"), Generic("icon", "fa fa-close", "color", "AD1000", "description", "Görevin Tekrar Kontrol Edilmesi Bekleniyor")]
        CozumReddedildi = 32,
        [Description("Islak İmzalı Form Yüklendi"), Generic("icon", "fa fa-upload", "color", "00ad06", "description", "Islak İmzalı Form Sisteme Yüklendi")]
        IslakImzaliFormYuklendi = 40,
        [Description("Memnuniyet Anketi Dolduruldu"), Generic("icon", "fa fa-th-list", "color", "00ad06", "description", "Memnuniyet Anketi Sisteme Yüklendi")]
        MemnuniyetAnketiYuklendi = 41,
        [Description("Stoktan Malzeme Kullanımı"), Generic("icon", "fa fa-wrench", "color", "e2c47f", "description", "Stoktan Malzeme Kullanımı Yapıldı")]
        StoktanMalzemeKullanimi = 42,
        [Description("Satın Alma Talebi Yapıldı"), Generic("icon", "fa fa-bookmark", "color", "e2c47f", "description", "Stoktan Malzeme Kullanımı Yapıldı")]
        SatinAlmaTalebiYapildi = 43,
        [Description("Satın Alma Talebi Onaylandı"), Generic("icon", "fa fa-check-circle", "color", "e2c47f", "description", "Stoktan Malzeme Kullanımı Yapıldı")]
        SatinAlmaTalebiOnaylandi = 44,
        [Description("Satın Alma Talebi Reddedildi"), Generic("icon", "fa fa-close", "color", "e2c47f", "description", "Stoktan Malzeme Kullanımı Yapıldı")]
        SatinAlmaTalebiReddedildi = 45,


        // NOT : Icon değiştirildiğinde Görev detayından svg nin de değişmesi gerekmektedir.
    }

    [EnumInfo(typeof(FTM_TaskOperation), "subject")]
    public enum EnumFTM_TaskPauseSubject
    {
        [Description("Parça Bekleniyor")]
        ParcaBekleniyor = 0,
        [Description("Uzman İhtiyacı")]
        UzmanIhtiyaci = 1,
        [Description("Müşteri İsteği İle")]
        MusteriIstegiIle = 2
    }
    partial class WorkOfTimeDatabase
    {
        public FTM_TaskOperation[] GetFTM_TaskOperationByTaskId(Guid taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<FTM_TaskOperation>().Where(a => a.taskId == taskId).Execute().ToArray();
            }
        }
    }
}
