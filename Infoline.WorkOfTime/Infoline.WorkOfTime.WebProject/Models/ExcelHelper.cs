using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace System.Web.Mvc
{
    public class ColumnInfoAttribute : Attribute
    {
        public string Alias { get; set; }
        public object DefaultValue { get; set; }
        public bool Required { get; set; }
        public bool IsUnique { get; set; }
        public string Description { get; set; }
        public string Info { get; set; }
        public ColumnInfoAttribute(string alias, bool required = false, object defaultValue = null, bool isUnique = false, string description = "", string info = "")
        {
            Alias = alias;
            DefaultValue = defaultValue;
            Required = required;
            IsUnique = isUnique;
            Description = description;
            Info = info;
        }
    }
    public static class ExcelHelper
    {
        public class ColumnInfo
        {
            public string Name { get; set; }
            public string Alias { get; set; }
            public string Type { get; set; }//Javascript Type
            public object DefaultValue { get; set; }
            public bool Required { get; set; }
            public bool Unique { get; set; }
            public string Description { get; set; }
            public string Info { get; set; }
        }
        public static string GetSchema(Type excelClass, string tableName = "")
        {
            return Infoline.Helper.Json.Serialize(GetColumnInfo(excelClass, tableName));
        }
        public static ColumnInfo[] GetColumnInfo(Type excelClass, string tableName = "")
        {
            var list = new List<ColumnInfo>();
            foreach (var item in excelClass.GetProperties().OrderBy(a => a.MetadataToken))
            {
                var column = new ColumnInfo();
                column.Name = item.Name;
                if (item.PropertyType.IsGenericType && item.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    column.Type = item.PropertyType.GetGenericArguments()[0].Name;
                }
                else
                {
                    column.Type = item.PropertyType.Name;
                }
                column.Alias = item.GetCustomAttribute<ColumnInfoAttribute>().Alias;
                column.DefaultValue = item.GetCustomAttribute<ColumnInfoAttribute>().DefaultValue;
                column.Required = item.GetCustomAttribute<ColumnInfoAttribute>().Required;
                column.Unique = item.GetCustomAttribute<ColumnInfoAttribute>().IsUnique;
                column.Info = item.GetCustomAttribute<ColumnInfoAttribute>().Info;
                if (column.Type == "DateTime")
                {
                    column.Description = " Örnek : (12.03.1996)";
                }
                if (column.Type == "Double")
                {
                    column.Description = " Örnek : (1234.45)";
                }
                if (column.Type == "Boolean")
                {
                    column.Description = " (1 veya 0)";
                }
                list.Add(column);
            }
            if (!string.IsNullOrEmpty(tableName) && TenantConfig.Tenant.Config.CustomProperty.ContainsKey(tableName))
            {
                var props = TenantConfig.Tenant.Config.CustomProperty[tableName];
                foreach (var prop in props)
                {
                    list.Add(new ColumnInfo
                    {
                        Alias = prop,
                        Name = prop,
                        Required = false,
                        Type = "String",
                        DefaultValue = ""
                    });
                }
            }
            return list.ToArray();
        }
    }
    public class SH_UserExcel
    {
        [ColumnInfoAttribute("İsim")]
        public string firstName { get; set; }
        [ColumnInfoAttribute("Soyisim")]
        public string lastName { get; set; }
        [ColumnInfoAttribute("Kimlik Numarası", isUnique: true)]
        public string loginname { get; set; }
        [ColumnInfoAttribute("Doğum Tarihi")]
        public DateTime? birthday { get; set; }
        [ColumnInfoAttribute("Firma Kodu", info: "Personelin çalıştığı işletmenin kodu zorunlu olmalıdır.")]
        public string companyCode { get; set; }
        [ColumnInfoAttribute("E-Posta Adresi")]
        public string email { get; set; }
        [ColumnInfoAttribute("Telefon Numarası")]
        public string phone { get; set; }
        [ColumnInfoAttribute("Şirket Numarası")]
        public string companyPhone { get; set; }
        [ColumnInfoAttribute("Yaşadığı Şehir")]
        public string city { get; set; }
        [ColumnInfoAttribute("Yaşadığı İlçe")]
        public string town { get; set; }
        [ColumnInfoAttribute("Açık Adres")]
        public string address { get; set; }
        [ColumnInfoAttribute("Sigortadaki Ünvanı")]
        public string title { get; set; }
        [ColumnInfoAttribute("Personel Kademesi")]
        public int? level { get; set; }
        [ColumnInfoAttribute("İşe Giriş Tarihi")]
        public DateTime? startDate { get; set; }
    }
    public class SH_UserOtherExcel
    {
        [ColumnInfoAttribute("İsim")]
        public string firstName { get; set; }
        [ColumnInfoAttribute("Soyisim")]
        public string lastName { get; set; }
        [ColumnInfoAttribute("Firma Kodu")]
        public string companyCode { get; set; }
        [ColumnInfoAttribute("E-Posta Adresi")]
        public string email { get; set; }
        [ColumnInfoAttribute("Telefon Numarası")]
        public string phone { get; set; }
        [ColumnInfoAttribute("Yaşadığı Şehir")]
        public string city { get; set; }
        [ColumnInfoAttribute("Yaşadığı İlçe")]
        public string town { get; set; }
    }
    public class CMP_CompanyExcel
    {
        [ColumnInfoAttribute("İşletme Kodu *", isUnique: true)]
        public string code { get; set; }
        [ColumnInfoAttribute("İşletme İsmi *")]
        public string name { get; set; }
        [ColumnInfoAttribute("Ticari Ünvan")]
        public string commercialTitle { get; set; }
        [ColumnInfoAttribute("E-Posta Adresi")]
        public string email { get; set; }
        [ColumnInfoAttribute("Telefon Numarası")]
        public string phone { get; set; }
        [ColumnInfoAttribute("Faks Numarası")]
        public string fax { get; set; }
        [ColumnInfoAttribute("Lokasyon")]
        public string location { get; set; }
        [ColumnInfoAttribute("Mersis Numarası")]
        public string mersisNo { get; set; }
        [ColumnInfoAttribute("Şehir")]
        public string city { get; set; }
        [ColumnInfoAttribute("İlçe")]
        public string town { get; set; }
        [ColumnInfoAttribute("Vergi Dairesi")]
        public string taxOffice { get; set; }
        [ColumnInfoAttribute("Vergi Numarası")]
        public string taxNumber { get; set; }
        [ColumnInfoAttribute("Fatura Adresi")]
        public string invoiceAddress { get; set; }
        [ColumnInfoAttribute("Açık Adres")]
        public string openAddress { get; set; }
    }
    public class PRD_StockTaskPlanExcel
    {
        [ColumnInfoAttribute("Abone Numarası *", isUnique: true)]
        public string code { get; set; }
        [ColumnInfoAttribute("İşletme")]
        public string company { get; set; }
        [ColumnInfoAttribute("Abone Adı Soyadı")]
        public string subscriber { get; set; }
        [ColumnInfoAttribute("Cihaz Adı")]
        public string deviceName { get; set; }
        [ColumnInfoAttribute("Marka")]
        public string brand { get; set; }
        [ColumnInfoAttribute("Model")]
        public string model { get; set; }
        [ColumnInfoAttribute("Seri No")]
        public string serialNumber { get; set; }
        [ColumnInfoAttribute("İl")]
        public string city { get; set; }
        [ColumnInfoAttribute("İlçe")]
        public string town { get; set; }
        [ColumnInfoAttribute("Adres")]
        public string address { get; set; }
    }
    public class CMP_OtherCompanyExcel
    {
        [ColumnInfoAttribute("İşletme Kodu *", isUnique: true)]
        public string code { get; set; }
        [ColumnInfoAttribute("İşletme İsmi *")]
        public string name { get; set; }
        [ColumnInfoAttribute("Ticari Ünvan")]
        public string commercialTitle { get; set; }
        [ColumnInfoAttribute("E-Posta Adresi")]
        public string email { get; set; }
        [ColumnInfoAttribute("Telefon Numarası")]
        public string phone { get; set; }
        [ColumnInfoAttribute("Faks Numarası")]
        public string fax { get; set; }
        [ColumnInfoAttribute("İl")]
        public string city { get; set; }
        [ColumnInfoAttribute("İlçe")]
        public string town { get; set; }
        [ColumnInfoAttribute("Vergi Dairesi")]
        public string taxOffice { get; set; }
        [ColumnInfoAttribute("Vergi Numarası")]
        public string taxNumber { get; set; }
        [ColumnInfoAttribute("Fatura Adresi")]
        public string invoiceAddress { get; set; }
        [ColumnInfoAttribute("Açık Adres")]
        public string openAddress { get; set; }
        [ColumnInfoAttribute("Yetkili 1 Ad-Soyad")]
        public string responsible1_Title { get; set; }
        [ColumnInfoAttribute("Yetkili 1 E-Posta")]
        public string responsible_1Email { get; set; }
        [ColumnInfoAttribute("Yetkili 1 Telefon")]
        public string responsible1_Phone { get; set; }
        [ColumnInfoAttribute("Yetkili 2 Ad-Soyad")]
        public string responsible2_Title { get; set; }
        [ColumnInfoAttribute("Yetkili 2 E-Posta")]
        public string responsible_2Email { get; set; }
        [ColumnInfoAttribute("Yetkili 2 Telefon")]
        public string responsible2_Phone { get; set; }
    }
    public class PRD_BrandExcel
    {
        [ColumnInfoAttribute("Marka Adı", true, isUnique: true)]
        public string name { get; set; }
    }
    public class PRD_ProductExcel
    {
        [ColumnInfoAttribute("Kodu", isUnique: true)]
        public string code { get; set; }
        [ColumnInfoAttribute("İsmi")]
        public string name { get; set; }
        [ColumnInfoAttribute("Açıklama")]
        public string description { get; set; }
        [ColumnInfoAttribute("Kategori")]
        public string category { get; set; }
        [ColumnInfoAttribute("Marka")]
        public string brand { get; set; }
        [ColumnInfoAttribute("Birimi")]
        public string unit { get; set; }
        [ColumnInfoAttribute("Türü")]
        public string type { get; set; }
        [ColumnInfoAttribute("Barkod")]
        public string barcode { get; set; }
        [ColumnInfoAttribute("Barkod Tipi")]
        public string barcodeType { get; set; }
        [ColumnInfoAttribute("Kritik Stok")]
        public int? criticalStock { get; set; }
        [ColumnInfoAttribute("Stok Takip Tipi")]
        public string stockType { get; set; }
        [ColumnInfoAttribute("Alış Fiyatı (Maliyet)")]
        public double? buyingPrice { get; set; }
        [ColumnInfoAttribute("Güncel Satış Fiyatı")]
        public double? sellingPrice { get; set; }
        [ColumnInfoAttribute("Para Birimi")]
        public string currency { get; set; }
    }
    public class CMP_StorageExcel
    {
        [ColumnInfoAttribute("Depo/Şube Kodu  *", isUnique: true)]
        public string code { get; set; }
        [ColumnInfoAttribute("Depo/Şube Adı *")]
        public string name { get; set; }
        [ColumnInfoAttribute("İşletme Kodu *")]
        public string companyCode { get; set; }
        [ColumnInfoAttribute("Telefon Numarası")]
        public string phone { get; set; }
        [ColumnInfoAttribute("Faks Numarası")]
        public string fax { get; set; }
        [ColumnInfoAttribute("Sorumlu Personel")]
        public string supervisor { get; set; }
        [ColumnInfoAttribute("İl")]
        public string city { get; set; }
        [ColumnInfoAttribute("İlçe")]
        public string town { get; set; }
        [ColumnInfoAttribute("Adres")]
        public string address { get; set; }
    }
    public class PA_TransactionLedgerExcel
    {
        [ColumnInfoAttribute("İşletme Kodu")]
        public string companyCode { get; set; }
        [ColumnInfoAttribute("İşletme Adı")]
        public string companyName { get; set; }
        [ColumnInfoAttribute("İşlem Tipi")]
        public string actionType { get; set; }
        [ColumnInfoAttribute("Borç")]
        public double? debt { get; set; }
        [ColumnInfoAttribute("Alacak")]
        public double? credit { get; set; }
        [ColumnInfoAttribute("Para Birimi")]
        public string currency { get; set; }
        [ColumnInfoAttribute("İşlem Tarihi")]
        public DateTime? date { get; set; }
        [ColumnInfoAttribute("Vade Tarihi")]
        public DateTime? expiryDate { get; set; }
        [ColumnInfoAttribute("Açıklama")]
        public string description { get; set; }
    }
    public class ExcelResult
    {
        public bool status { get; set; }
        public string message { get; set; }
        public int rowNumber { get; set; }
    }
}