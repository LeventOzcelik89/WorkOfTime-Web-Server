using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(PRD_Product), "type")]
    public enum EnumPRD_ProductType
    {
        [Description("Hizmet"), Generic("icon", "icon-users", "color", "success")]
        Hizmet = 0,
        [Description("Mamül (Ticari Mal)"), Generic("icon", "icon-basket-1", "color", "success")]
        TicariMal = 1,
        [Description("Yarı Mamül"), Generic("icon", "icon-archive-1", "color", "success")]
        YariMamul = 2,
        [Description("Ham Madde"), Generic("icon", " icon-industry", "color", "success")]
        Hammadde = 3,
        [Description("Diğer"), Generic("icon", "icon-hashtag", "color", "success")]
        Diger = 4,
    }


    [EnumInfo(typeof(PRD_Product), "stockType")]
    public enum EnumPRD_ProductStockType
    {
        [Description("Takip Yapılmıyor"), Generic("icon", "icon-eye-off", "color", "danger")]
        Stoksuz = 0,
        [Description("Normal Takip Yapılıyor"), Generic("icon", "icon-eye", "color", "primary")]
        NormalTakip = 1,
        [Description("Seri Numaralı Takip Yapılıyor"), Generic("icon", "icon-qrcode", "color", "primary")]
        SeriNoluTakip = 2,
    }


    [EnumInfo(typeof(PRD_Product), "isCriticalStock")]
    public enum EnumPRD_ProductIsCriticalStock
    {
        [Description("Kritik Stok (Hayır)"), Generic("icon", "icon-binoculars", "color", "success")]
        Hayir = 0,
        [Description("Kritik Stok (Evet)"), Generic("icon", "icon-qrcode", "color", "primary")]
        Evet = 1,
    }

    [Flags]
    [EnumInfo(typeof(PRD_Product), "status")]
    public enum EnumPRD_ProductStatus
    {
        [Description("Satış modülünde gösterilsin mi?")]
        satis = 1,
        [Description("Satın alma modülünde gösterilsin mi?")]
        satinalma = 2,
        [Description("Fire bildiriminde gösterilsin mi?")]
        firebildirimi = 4,
    }

    [EnumInfo(typeof(PRD_Product), "barcodeType")]
    public enum EnumPRD_ProductbarcodeType
    {
        [Description("Code11")]
        Code11 = 0,
        [Description("Code39")]
        Code39 = 1,
        [Description("Code39Extended")]
        Code39Extended = 2,
        [Description("Code128")]
        Code128 = 3,
        [Description("Code93")]
        Code93 = 4,
        [Description("Code93Extended")]
        Code93Extended = 5,
        [Description("Code128A")]
        Code128A = 6,
        [Description("Code128B")]
        Code128B = 7,
        [Description("Code128C")]
        Code128C = 8,
        [Description("EAN13")]
        EAN13 = 9,
        [Description("EAN8")]
        EAN8 = 10,
        [Description("UPCA")]
        UPCA = 11,
        [Description("UPCE")]
        UPCE = 12,
        [Description("MSI")]
        MSI = 13,
        [Description("POSTNET")]
        POSTNET = 14,
        [Description("MSImod10")]
        MSImod10 = 15,
        [Description("MSImod11")]
        MSImod11 = 16,
        [Description("MSImod1010")]
        MSImod1010 = 17,
        [Description("GS1128")]
        GS1128 = 18,
        [Description("MSImod1110")]
        MSImod1110 = 19
    }

    partial class WorkOfTimeDatabase
    {

        public PRD_Product[] GetPRD_ProductByIds(Guid[] ids)
        {

            using (var db = GetDB())
            {
                return db.Table<PRD_Product>().Where(a => a.id.In(ids)).Execute().ToArray();
            }
        }

        public PRD_Product[] GetPRD_ProductsByBrandIds(Guid[] brandIds)
        {

            using (var db = GetDB())
            {
                return db.Table<PRD_Product>().Where(a => a.brandId.In(brandIds)).Execute().ToArray();
            }
        }
        public PRD_Product GetPRD_ProductByCode(string code)
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_Product>().Where(a => a.code == code).Execute().FirstOrDefault();
            }
        }

        public PRD_Product GetPRD_ProductByName(string name)
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_Product>().Where(a => a.name == name).Execute().FirstOrDefault();
            }
        }
    }
}
