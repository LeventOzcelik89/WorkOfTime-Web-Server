using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(INV_Commissions), "Information")]
    public enum EnumINV_CommissionsInformation
    {
        [Description("Denetim")]
        Denetim = 0,
        [Description("Eğitim")]
        Egitim = 1,
        [Description("Satış")]
        Satis = 2,
        [Description("Organizasyon")]
        Organizasyon = 3,
        [Description("Teknik Destek")]
        TeknikDestek = 4,
        [Description("Satın Alma")]
        SatinAlma = 5,
        [Description("Davet")]
        Davet = 6,
        [Description("Diğer")]
        Diger = 7,
        [Description("Proje Çalışması")]
        ProjeCalismasi = 8

    }

    [EnumInfo(typeof(INV_Commissions), "TravelInformation")]
    public enum EnumINV_CommissionsTravelInformation
    {
        [Description("Şirket Aracı")]
        Arac = 0,
        [Description("Otobüs")]
        Otobus = 1,
        [Description("Uçak")]
        Ucak = 2,
        [Description("Diğer")]
        Diger = 3,
        [Description("Kiralık Araç + Uçak")]
        UcakAraba = 4,
        [Description("Shuttle")]
        Shuttle = 5,
        [Description("Shuttle + Uçak")]
        ShuttleUcak = 6,
        [Description("Kiralık Araç")]
        KiralikArac = 7,


    }

    [EnumInfo(typeof(INV_Commissions), "RequestForAccommodation")]
    public enum EnumINV_CommissionsRequestForAccommodation
    {
        [Description("Yok")]
        Yok = 0,
        [Description("Var")]
        Var = 1,
    }

    [EnumInfo(typeof(INV_Commissions), "DemandForAdvance")]
    public enum EnumINV_CommissionsDemandForAdvance
    {
        [Description("Yok")]
        Yok = 0,
        [Description("Var")]
        Var = 1,
    }

    [EnumInfo(typeof(INV_Commissions), "ApproveStatus")]
    public enum EnumINV_CommissionsApproveStatus
    {
        [Description("Yönetici onayı bekleniyor")]
        Bekliyor = 0,
        [Description("Islak imzalı dosya bekleniyor")]
        Onaylandi = 1,
        [Description("Yönetici reddetti")]
        Reddedildi = 2,
        [Description("Islak imzalı dosya yüklendi")]
        IslakImzaYuklendi = 3
    }

    public partial class WorkOfTimeDatabase
    {
        public VWINV_CommissionsInformation GetVWINV_CommissionsInformationByComissionId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWINV_CommissionsInformation>().Where(a => a.commissionsId == id).Execute().FirstOrDefault();
            }
        }
    }
}
