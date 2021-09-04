using System;
using System.ComponentModel;

namespace Infoline.WorkOfTime.Tools
{


    public class Base
    {
        [DisplayInfo("Id", "Tablonun id'sinin tutulduğu alandır"), IsPrimary, DefaultInfo(DefaultInfo.EnumDefaults.newid)]
        public Guid id { get; set; }

        [DisplayInfo("Oluşturan", "Kaydı oluşturan kullanıcının idsinin tutulduğu alandır."), RelationInfo("SH_User", "id", new string[] { "firstname", "lastname" })]
        public Guid? createdby { get; set; }
        [DisplayInfo("Değiştiren", "Kaydı değiştiren kullanıcının idsinin tutulduğu alandır."), RelationInfo("SH_User", "id", new string[] { "firstname", "lastname" })]
        public Guid? changedby { get; set; }

        [DisplayInfo("Oluşturulma Tarihi", "Kaydın oluşturulma tarihinin tutulduğu alandır."), DefaultInfo(DefaultInfo.EnumDefaults.getdate)]
        public DateTime? created { get; set; }

        [DisplayInfo("Değiştirilme Tarihi", "Kaydın oluşturulma tarihinin tutulduğu alandır.")]
        public DateTime? changed { get; set; }
    }


    //[DisplayInfo("Kurallar", "İzin, görevlendirme vs. süreci bulunan işlemlerin kurallarının tutulduğu tablodur.")]
    //public class UT_Rules : Base
    //{
    //    [DisplayInfo("Kural", "Kural adının tutulduğu alandır."), LengthInfo(250)]
    //    public string name { get; set; }
    //    [DisplayInfo("Tip", "Kural tipinin tutulduğu alandır.")]
    //    public short? type { get; set; }
    //}

    //[DisplayInfo("Kullanıcı Kuralları", "İzin, görevlendirme vs. süreci bulunan işlemlerin kurallarının personellerle ilişkilendirildiği tablodur.")]
    //public class UT_RulesUser : Base
    //{
    //    [DisplayInfo("Kural Id'si", "Kuralın tutulduğu alandır."), RelationInfo("UT_Rules", "id", new string[] { "name" })]
    //    public Guid? rulesId { get; set; }
    //    [DisplayInfo("Kullanıc Id'si", "Kullanıcının tutulduğu alandır."), RelationInfo("SH_User", "id", new string[] { "firstname", "lastname" })]
    //    public Guid? userId { get; set; }
    //}


    //[DisplayInfo("Kullanıcı Kural Aşamaları", "İzin, görevlendirme vs. süreci bulunan işlemlerin kurallarının personellerle ilişkilendirildiği tablodur.")]
    //public class UT_RulesUserStage : Base
    //{
    //    [DisplayInfo("Kural Id'si", "Kuralın tutulduğu alandır."), RelationInfo("UT_Rules", "id", new string[] { "name" })]
    //    public Guid? rulesId { get; set; }
    //    [DisplayInfo("Sıra", "Kural aşama sırasının tutulduğu alandır.")]
    //    public short? order { get; set; }
    //    [DisplayInfo("Tip", "Kural aşama tipinin tutulduğu alandır.")]
    //    public short? type { get; set; }
    //    [DisplayInfo("Kullanıc Id'si", "Kullanıcının tutulduğu alandır."), RelationInfo("SH_User", "id", new string[] { "firstname", "lastname" })]
    //    public Guid? userId { get; set; }
    //    [DisplayInfo("Başlık", "Kural aşama başlığının tutulduğu alandır.")]
    //    public string title { get; set; }
    //}

    //[BusinessAccess.EnumInfo(typeof(UT_Rules), "type")]
    //public enum EnumUT_RulesType
    //{
    //    [Description("İzin")]
    //    Permit = 0,
    //    [Description("Görevlendirme")]
    //    Commission = 10
    //}


    //[BusinessAccess.EnumInfo(typeof(UT_RulesUserStage), "type")]
    //public enum EnumUT_RulesUserStage
    //{
    //    [Description("1. Yöetici")]
    //    Manager1 = 0,
    //    [Description("2. Yönetici")]
    //    Manager2 = 1,
    //    [Description("3. Yönetici")]
    //    Manager3 = 2,
    //    [Description("4. Yönetici")]
    //    Manager4 = 3,
    //    [Description("İnsan Kaynakları Yöneticisi")]
    //    Ik = 10
    //}

    //[DisplayInfo("Yardım Talebi Üzerinde Yazılan Mesajlar", "Yardım talebi üzerinde yapılan mesaj işlemlerinin tutulduğu tablodur.")]
    //public class HDM_TicketMessage : Base
    //{
    //    [DisplayInfo("Talebin Id'si", "Talebin tutulduğu alandır."), RelationInfo("HDM_Ticket", "id", new string[] { "title" })]
    //    public Guid? ticketId { get; set; }
    //    [DisplayInfo("Mesaj Tipi", "Mesaj tipinin tutulduğu alandır.")]
    //    public short? type { get; set; }
    //    [DisplayInfo("İçerik", "Mesaj içeriğinin tutulduğu alandır.")]
    //    public string content { get; set; }
    //    [DisplayInfo("Mail CC", "Mesaj gönderilirken cc'ye eklenecek mail adresilerinin tutulduğu alandır.")]
    //    public string cc { get; set; }
    //    [DisplayInfo("Mail BCC", "Mesaj gönderilirken bcc'ye eklenecek mail adresilerinin tutulduğu alandır.")]
    //    public string bcc { get; set; }
    //}

    //[BusinessAccess.EnumInfo(typeof(HDM_TicketMessage), "type")]
    //public enum EnumHDM_TicketMessageType
    //{
    //    [Description("Cevapla")]
    //    Reply = 0,
    //    [Description("İlet")]
    //    Forward = 1,
    //    [Description("Not")]
    //    Note = 2,
    //}

    //[DisplayInfo("Yardım Talepleri", "Yardım taleplerinin tutulduğu tablodur.")]
    //public class HDM_Ticket : Base
    //{
    //    [DisplayInfo("Talep Kodu", "Talep kodunun tutulduğu alandır."), LengthInfo(50)]
    //    public string code { get; set; }
    //    [DisplayInfo("Değerlendirme Puanı", "Talebin değerlendirme puanının tutulduğu alandır.")]
    //    public short? evaluation { get; set; }
    //    [DisplayInfo("Talebin Durumu", "Talebin durumunun tutulduğu alandır.")]
    //    public short? status { get; set; }
    //    [DisplayInfo("Öncelik", "Talebin önceliğinin tutulduğu alandır.")]
    //    public short? priority { get; set; }
    //    [DisplayInfo("Öncelik", "Talebin nerden geldiğinin tutulduğu alandır.")]
    //    public short? channel { get; set; }
    //    [DisplayInfo("Konu", "Talebin konusunun tutulduğu alandır."), RelationInfo("HDM_Issue", "id", new string[] { "title" })]
    //    public Guid? issueId { get; set; }
    //    [DisplayInfo("Sorun", "Talebin sorununun tutulduğu alandır."), RelationInfo("HDM_Suggestion", "id", new string[] { "title" })]
    //    public Guid? suggestionId { get; set; }
    //    [DisplayInfo("Başlık", "Talep başlığının tutulduğu alandır.")]
    //    public string title { get; set; }
    //    [DisplayInfo("İçerik", "Talep içeriğinin tutulduğu alandır.")]
    //    public string content { get; set; }
    //    [DisplayInfo("Talep Eden", "Talebi yapan kişinin tutulduğu alandır."), RelationInfo("SH_User", "id", new string[] { "firstname", "lastname" })]
    //    public Guid? requesterId { get; set; }
    //    [DisplayInfo("Görevli", "Görevli kişinin tutulduğu alandır."), RelationInfo("SH_User", "id", new string[] { "firstname", "lastname" })]
    //    public Guid? assignUserId { get; set; }
    //}

    //[BusinessAccess.EnumInfo(typeof(HDM_Ticket), "status")]
    //public enum EnumHDM_TicketStatus
    //{
    //    [Description("Açık")]
    //    Open = 0,
    //    [Description("Beklemede")]
    //    Pending = 1,
    //    [Description("Cevaplandı")]
    //    Answered = 2,
    //    [Description("Çözümlendi")]
    //    Resolved = 3,
    //    [Description("Kapatıldı")]
    //    Closed = 4,
    //    [Description("Gereksiz")]
    //    Spam = 5,
    //}

    //[BusinessAccess.EnumInfo(typeof(HDM_Ticket), "channel")]
    //public enum EnumHDM_TicketChannel
    //{
    //    [Description("Web Site")]
    //    WebSite = 0,
    //    [Description("Telefon")]
    //    Telefon = 1,
    //    [Description("Eposta")]
    //    Email = 2,
    //    [Description("Diğer")]
    //    Diger = 3,
    //}


    //[DisplayInfo("Yardım Talebinde Bulunanlar", "Yardım talebinde bulunan kişilerin tutulduğu tablodur.")]
    //public class HDM_TicketRequester : Base
    //{
    //    [DisplayInfo("Talep Eden Kişi", "Talep eden kişinin ad soyad bilgisinin tutulduğu alandır."), LengthInfo(255)]
    //    public string fullName { get; set; }
    //    [DisplayInfo("Talep Eden Kişi", "Talep eden kişinin email adresinin tutulduğu alandır."), LengthInfo(255)]
    //    public string email { get; set; }
    //    [DisplayInfo("Talep Eden Kişi", "Talep eden kişinin telefon numarasının tutulduğu alandır."), LengthInfo(50)]
    //    public string phone { get; set; }
    //    [DisplayInfo("Talep Eden Kişi", "Talep eden kişinin işletmesinin tutulduğu alandır."), LengthInfo(255)]
    //    public string company { get; set; }
    //}




    //[BusinessAccess.EnumInfo(typeof(HDM_TicketAction), "type")]
    //public enum EnumHDM_TicketActionType
    //{
    //    [Description("Ticket Oluşturuldu")]
    //    YeniTicket = 0,
    //    [Description("Personel Ataması Yapıldı")]
    //    PersonelAtandı = 1,
    //    [Description("Ticket Durumu Güncellendi")]
    //    DurumGuncelleme = 2,
    //    [Description("Hizmet Değerlendirildi")]
    //    HizmetDegerlendirme = 3,
    //    [Description("Operator Mesaj Yazdı")]
    //    OperatorMesaj = 4,
    //    [Description("Müşteri Mesaj Yazdı")]
    //    MusteriMesaj = 5,
    //}














    //[DisplayInfo("Kısmi Zamanlı Görevlendirmeler", "Kısmi Zamanlı Görevlendirilmelerin Tutulduğu Tablodur.")]
    //public class SH_PartialAssigment : Base
    //{
    //    [DisplayInfo("Personel", "Kısmi zamanlı çalışan personelin tutulduğu alandır."), RelationInfo("SH_User", "id", new string[] { "firstname", "lastname" })]
    //    public Guid? userId { get; set; }
    //    [DisplayInfo("Görev Başlangıç Tarihi", "Görev başlangıç tarihinin tutulduğu alandır.")]
    //    public DateTime? startDate { get; set; }
    //    [DisplayInfo("Görev Bitiş Tarihi", "Görev bitiş tarihinin tutulduğu alandır.")]
    //    public DateTime? endDate { get; set; }
    //    [DisplayInfo("Girdiği Ders Saati", "Girmiş olduğu ders saatinin  tutulduğu alandır.")]
    //    public double? courseHours { get; set; }
    //    [DisplayInfo("Bölüm Adı", "Derse girdiği bölümün tutulduğu alandır.")]
    //    public string schoolDepartment { get; set; }
    //    [DisplayInfo("Ders Adı", "Girmiş olduğu dersin tutulduğu alandır."), LengthInfo(250)]
    //    public string lesson { get; set; }
    //    [DisplayInfo("Saatlik Ücreti", "Girmiş olduğu dersin saatlik ücretinin tutulduğu alandır."), LengthInfo(250)]
    //    public double? hourlyWage { get; set; }
    //    [DisplayInfo("Para birimi", "Para biriminin tutulduğu alandır."), RelationInfo("UT_Currency", "id", new string[] { "name", "symbol" })]
    //    public Guid? qurrencyId { get; set; }
    //}




    //[DisplayInfo("Ürün Fiyatları", "Ürün Fiyatlarının Tutulduğu Tablodur.")]
    //public class PA_Expense : Base
    //{
    //    [DisplayInfo("Ürün", "Ürün bilgisinin tutulduğu alandır."), RelationInfo("PRD_Product", "id", new string[] { "code", "name" })]
    //    public Guid? transactionId { get; set; }

    //    [DisplayInfo("Tip", "Fiyat tipinin tutulduğu alandır."), EnumInfo(typeof(EnumPRD_ProductPriceType))]
    //    public short? type { get; set; }
    //    [DisplayInfo("Tutar", "Tutar bilgisinin tutulduğu alandır.")]
    //    public double? price { get; set; }
    //    [DisplayInfo("Para birimi", "Para biriminin tutulduğu alandır."), RelationInfo("UT_Currency", "id", new string[] { "name", "symbol" })]
    //    public Guid? qurrencyId { get; set; }
    //}







    //[BusinessAccess.EnumInfo(typeof(PRD_Transaction), "type")]
    //public enum EnumPRD_TransactionType
    //{
    //    //Giriş İşlemi İşlemleri
    //    [Description("Gelen İrsaliye"), Generic("icon", "icon-plus")]
    //    GelenIrsaliye = 0,
    //    [Description("Zimmet İade Fişi"), Generic("icon", "icon-user-delete")]
    //    ZimmetAlma = 1,
    //    [Description("Kiralama Fişi"), Generic("icon", "icon-tag-2")]
    //    Kiralama = 2,
    //    [Description("Açılış Fişi"), Generic("icon", "icon-tag-2")]
    //    AcılısFisi = 3,
    //    [Description("Üretim Fişi"), Generic("icon", "icon-loop-alt")]
    //    UretimFisi = 4,
    //    [Description("Sayım Fazlası"), Generic("icon", "icon-tag-2")]
    //    SayımFazlası = 5,

    //    //Çıkış İşlemleri
    //    [Description("Giden İrsaliye"), Generic("icon", "icon-minus")]
    //    GidenIrsaliye = 10,
    //    [Description("Zimmet Verme Fişi"), Generic("icon", "icon-user-add-1")]
    //    ZimmetVerme = 11,
    //    [Description("Kiraya Verme Fişi"), Generic("icon", "icon-tag-2")]
    //    KiralayaVerme = 12,
    //    [Description("Sarf Fişi"), Generic("icon", "icon-basket")]
    //    SarfFisi = 13,
    //    [Description("Fire Fişi"), Generic("icon", "icon-basket")]
    //    FireFisi = 14,
    //    [Description("Sayım Eksiği"), Generic("icon", "icon-tag-2")]
    //    SayımEksigi = 15,
    //}


    //[BusinessAccess.EnumInfo(typeof(PRD_TransactionInventory), "type")]
    //public enum EnumPRD_TransactionInventoryType
    //{
    //    [Description("Stok Düzeltme"), Generic("icon", "icon-archive-1")]
    //    StokDuzeltme = 0,
    //    [Description("Satın Alındı"), Generic("icon", "icon-basket")]
    //    SatinAlindi = 1,
    //    [Description("Kiralandı"), Generic("icon", "icon-stopwatch-1")]
    //    Kiralandi = 2,
    //    [Description("İade Alındı"), Generic("icon", "icon-forward-2")]
    //    IadeAlindi = 3,
    //    [Description("Üretildi"), Generic("icon", "icon-industrial-building")]
    //    Uretildi = 4,
    //    [Description("Zimmet Verildi"), Generic("icon", "icon-user-add-1")]
    //    ZimmetVerildi = 5,
    //    [Description("Zimmet Alındı"), Generic("icon", "icon-user-delete")]
    //    ZimmetAlindi = 6,
    //    [Description("Üründen Söküldü"), Generic("icon", "icon-flow-merge")]
    //    Sokuldu = 7,
    //    [Description("Ürüne Takıldı"), Generic("icon", "icon-list-add")]
    //    Takildi = 8,
    //    [Description("Birleştirildi"), Generic("icon", "icon-flow-merge")]
    //    Birlestirildi = 9,



    //    //Konum Hareketleri
    //    [Description("Depoda"), Generic("icon", "icon-home-4")]
    //    Depoda = 20,
    //    [Description("Personelde"), Generic("icon", "icon-user-2")]
    //    Personelde = 21,
    //    [Description("Ürüne Takılı"), Generic("icon", "icon-archive")]
    //    UrundeTakili = 22,
    //    [Description("Satıldı"), Generic("icon", "icon-money-1")]
    //    Satildi = 23,
    //    [Description("Kiraya Verildi"), Generic("icon", "icon-loop-outline")]
    //    KirayaVerildi = 24,
    //    [Description("İade Edildi"), Generic("icon", "icon-back-in-time")]
    //    IadeEdildi = 25,
    //    [Description("Üretimde Kullanıldı"), Generic("icon", "icon-industrial-building")]
    //    UretimdeKullanildi = 26,
    //    [Description("Parçalandı"), Generic("icon", "icon-flow-merge")]
    //    Parcalandi = 27,
    //    [Description("Hurdaya Ayrıldı"), Generic("icon", "icon-trash-1")]
    //    HurdayaAyrildi = 28,
    //    [Description("Kayboldu"), Generic("icon", "icon-warning-1")]
    //    Kayboldu = 29,
    //    [Description("Harcandı/Tükendi"), Generic("icon", "icon-trash-4")]
    //    Harcandi = 30,



    //    [Description("Bakım Envanteri"), Generic("icon", "icon-cogs")]
    //    BakimEnvanteri = 101,
    //}

    //[DisplayInfo("Stok Envanter İşlemi", "Stok Envanter İşlemlerinin Tutulduğu Tablodur.")]
    //public class PRD_Transaction : Base
    //{
    //    [DisplayInfo("İşlem Tarihi", "Kodunun tutulduğu alandır."), LengthInfo(50)]
    //    public DateTime date { get; set; }
    //    [DisplayInfo("İşlem Kodu", "Kodunun tutulduğu alandır."), LengthInfo(50)]
    //    public string code { get; set; }
    //    [DisplayInfo("İşlem Durumu", "Durumunun tutulduğu alandır."), EnumInfo(typeof(EnumPRD_TransactionStatus))]
    //    public bool? status { get; set; }
    //    [DisplayInfo("İşlem Tipi", "Tipinin tutulduğu alandır."), EnumInfo(typeof(EnumPRD_TransactionType))]
    //    public short? type { get; set; }
    //    [DisplayInfo("İşlem Açıklaması", "Açıklamanın tutulduğu alandır.")]
    //    public string description { get; set; }
    //    [DisplayInfo("Fatura", "Fatura  idsinin tutulduğu alandır."), RelationInfo("CMP_Invoice", "id", new string[] { "rowNumber", "serialNumber" })]
    //    public Guid? invoiceId { get; set; }
    //    [DisplayInfo("Sipariş", "Sipariş idsinin tutulduğu alandır."), RelationInfo("CMP_Invoice", "id", new string[] { "rowNumber", "serialNumber" })]
    //    public Guid? orderId { get; set; }


    //    [DisplayInfo("Çıkış", "Çıkış yapılacak yerin detay bilgilerinin tutulduğu alandır.")]
    //    public Guid? outputId { get; set; }
    //    [DisplayInfo("Çıkış", "Çıkış yapılacak yerin tablosunun tutulduğu alandır.")]
    //    public string ouputTable { get; set; }
    //    [DisplayInfo("Çıkış Detay Bilgisi", "Çıkış yapılacak yerin detay bilgilerinin tutulduğu alandır.")]
    //    public string outputDetail { get; set; }
    //    [DisplayInfo("Giriş", "Giriş yapılacak yerin idsinin tutulduğu alandır.")]
    //    public Guid? inputId { get; set; }
    //    [DisplayInfo("Giriş", "Giriş yapılacak yerin tablosunun tutulduğu alandır.")]
    //    public string inputTable { get; set; }
    //    [DisplayInfo("Giriş Detay Bilgisi", "Giriş yapılacak yerin detay bilgilerinin tutulduğu alandır.")]
    //    public string inputDetail { get; set; }
    //}


    //[DisplayInfo("Stok Hareketleri", "Stok Hareketlerinin Tutulduğu Tablodur.")]
    //public class PRD_TransactionProduct : Base
    //{
    //    [DisplayInfo("İşlem", "İşlem idsinin tutulduğu alandır."), RelationInfo("PRD_Transaction", "id", new string[] { "code" })]
    //    public Guid? transactionId { get; set; }
    //    [DisplayInfo("Ürün", "İşlem ürünün tutulduğu alandır."), RelationInfo("PRD_Product", "id", new string[] { "code", "name" })]
    //    public Guid? productId { get; set; }
    //    [DisplayInfo("Miktar", "İşlem miktarının tutulduğu alandır.")]
    //    public double quantity { get; set; }
    //    [DisplayInfo("Birim Tutar", "İşlem birim tutarının tutulduğu alandır.")]
    //    public double unitPrice { get; set; }
    //}

    //[DisplayInfo("Envanter Hareketleri", "Envanter Hareketlerinin Tutulduğu Tablodur.")]
    //public class PRD_TransactionInventory : Base
    //{
    //    [DisplayInfo("İşlem", "İşlem idsinin tutulduğu alandır."), RelationInfo("PRD_Transaction", "id", new string[] { "code" })]
    //    public Guid? transactionId { get; set; }

    //    [DisplayInfo("İşlem", "İşlem idsinin tutulduğu alandır."), RelationInfo("PRD_TransactionProduct", "id", new string[] {  })]
    //    public Guid? transactionProductId { get; set; }

    //    [DisplayInfo("Envanter", "Envanter idsinin tutulduğu alandır."), RelationInfo("PRD_Inventory", "id", new string[] { "code", "serialcode" })]
    //    public Guid? inventoryId { get; set; }

    //    [DisplayInfo("Hareket Tipi", "Hareket Tipinin tutulduğu alandır."), EnumInfo(typeof(EnumPRD_TransactionInventoryType))]
    //    public short? type { get; set; }

    //    [DisplayInfo("Kayıt", "Kayıt yapılacak yerin idsinin tutulduğu alandır.")]
    //    public Guid? dataId { get; set; }
    //    [DisplayInfo("Kayıt Tablo", "Kayıt yapılacak yerin tablosunun tutulduğu alandır.")]
    //    public string dataTable { get; set; }

    //}

}