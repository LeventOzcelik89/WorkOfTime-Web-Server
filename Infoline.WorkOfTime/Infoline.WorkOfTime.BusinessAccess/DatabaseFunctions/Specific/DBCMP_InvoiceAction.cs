using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(CMP_InvoiceAction), "type")]
    public enum EnumCMP_InvoiceActionType
    {
        [Description("Talep Oluşturuldu"), Generic("color", "#f5f5f5")]
        YeniTalep = 0,
        [Description("Talep Onaylandı"), Generic("color", "#afffb5")]
        TalepOnay = 1,
        [Description("Talep Reddedildi"), Generic("color", "#ffb2b2")]
        TalepRed = 2,
        [Description("Talep İçin Teklif Eklendi"), Generic("color", "#fac8b5")]
        TalepTeklifEklendi = 3,
        [Description("Teklifler Onaya Sunuldu"), Generic("color", "#fbffb9")]
        TalepTekliflerOnayaSunuldu = 4,
        [Description("Teklifler Tekrar Toplanacak"), Generic("color", "#fde1ff")]
        TeklifTekrar = 5,
        [Description("Tekliflerden Biri Onaylandı"), Generic("color", "#d1e0f6")]
        TalepTeklifOnay = 6,
        [Description("Talebin Faturası Kesildi"), Generic("color", "#71ffaa")]
        TalepFatura = 7,
        [Description("Talep Süreci Tamamlandı"), Generic("color", "#c7edff")]
        TalepSurecTamamlandi = 8,

        [Description("Teklif Oluşturuldu"), Generic("color", "#f5f5f5")]
        YeniTeklif = 9,
        [Description("Teklifi Yönetici Onayladı"), Generic("color", "#afffb5")]
        TeklifYoneticiOnay = 10,
        [Description("Teklif Reddedildi"), Generic("color", "#ffb2b2")]
        TeklifRed = 11,
        [Description("Teklifi Müşteri Onayladı"), Generic("color", "#c7ffef")]
        TeklifMusteriOnay = 12,
        [Description("Teklif Siparişe Dönüştürüldü"), Generic("color", "#ffe9c7")]
        TeklifSiparis = 13,
        [Description("Teklif Faturaya Dönüştürüldü"), Generic("color", "#abfe96")]
        TeklifFatura = 14,
        [Description("Teklif İrsaliye Dönüştürüldü"), Generic("color", "#23c6c8")]
        TeklifIrsaliye = 15,
        [Description("Teklif Süreci Tamamlandı"), Generic("color", "#c7edff")]
        TeklifSurecTamamlandi = 16,

        [Description("Sipariş Oluşturuldu"), Generic("color", "#f5f5f5")]
        YeniSiparis = 16,
        [Description("Sipariş Onaylandı"), Generic("color", "#afffb5")]
        SiparisOnay = 17,
        [Description("Sipariş İçin Satın Alma Talebi"), Generic("color", "#afffb5")]
        SiparisSatinAlma = 18,
        [Description("Sipariş Reddedildi"), Generic("color", "#ffb2b2")]
        SiparisRed = 19,
        [Description("Siparişe İrsaliye Eklendi"), Generic("color", "#ffecc1")]
        Siparisİrsaliye = 20,
        [Description("Siparişin Faturası Kesildi"), Generic("color", "#abfe96")]
        SiparisFatura = 21,
        [Description("Sipariş Süreci Tamamlandı"), Generic("color", "#c7edff")]
        SiparisSurecTamamlandi = 22,

        [Description("Fatura Oluşturuldu"), Generic("color", "#f5f5f5")]
        YeniFatura = 23,
        [Description("Faturanın İrsaliyesi Düzenlendi"), Generic("color", "#ffecc1")]
        FaturaIrsaliye = 24,
        [Description("Fatura Süreci Tamamlandı"), Generic("color", "#c7edff")]
        FaturaSurecTamamlandi = 25,

        [Description("İrsaliye Oluşturuldu"), Generic("color", "#f5f5f5")]
        YeniIrsaliye = 26,
        [Description("İrsaliye Düzenlendi"), Generic("color", "#afffb5")]
        IrsaliyeDuzenlendi = 27,
        [Description("İrsaliye Reddedildi"), Generic("color", "#ffb2b2")]
        IrsaliyeRed = 28,
        [Description("İrsaliyenin Faturası Kesildi"), Generic("color", "#abfe96")]
        IrsaliyeFatura = 29,


        [Description("Not Eklendi"), Generic("color", "#c3c3c3")]
        NotEkle = 30,
        [Description("Ödeme Yapıldı"), Generic("color", "#cffc91")]
        FaturaOdeme = 31,
        [Description("Tahsilat Yapıldı"), Generic("color", "#cffc91")]
        FaturaTahsilat = 32,


        [Description("Talep İptal Edildi"), Generic("color", "rgb(232 194 194)")]
        TalepIptal = 40,
    }

    partial class WorkOfTimeDatabase
    {
        public CMP_InvoiceAction[] GetCMP_InvoiceActionByInvoiceId(Guid invoiceId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_InvoiceAction>().Where(a => a.invoiceId == invoiceId).Execute().ToArray();
            }
        }

    }
}
