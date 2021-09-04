using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;


namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(PRJ_Project), "ProjectType")]
    public enum EnumPRJ_ProjectProjectType
    {
        [Description("Teknokent")]
        Teknokent = 0,
        [Description("Satış")]
        Satis = 1,
        [Description("İhale")]
        Ihale = 2,
        [Description("Şirket İçi")]
        SirketIci = 3,
        [Description("Diğer")]
        Diger = 4,
        [Description("Tübitak")]
        Tubitak = 5,
        [Description("Bakım Onarım")]
        BakimOnarim = 6,
        [Description("Danışmanlık")]
        Danismanlik = 7,
        [Description("İşletme Yönetim")]
        IsletmeYonetim = 8,
        [Description("Teknik İşletme")]
        TeknikIsletme = 9,
        [Description("Komite Kararı")]
        KomiteKarari = 10,
        [Description("İş Geliştirme")]
        Isgelistirme = 11,
        [Description("Müşteri Takip")]
        MusteriTakip = 12
    }


    [EnumInfo(typeof(PRJ_Project), "ProjectTechnicalType")]
    public enum EnumPRJ_ProjectProjectProjectTechnicalType
    {
        [Description("Yazılım")]
        Yazilim = 0,
        [Description("Donanım")]
        Donanim = 1,
        [Description("Hizmet")]
        Hizmet = 2,
        [Description("Diğer")]
        Diger = 3
    }



    [EnumInfo(typeof(PRJ_Project), "IsActive")]
    public enum EnumPRJ_ProjectIsActive
    {
        [Description("Hayır")]
        Hayir = 0,
        [Description("Evet")]
        Evet = 1
    }

    [EnumInfo(typeof(PRJ_Project), "IsConfirm")]
    public enum EnumPRJ_ProjectIsConfirm
    {
        [Description("Hayır")]
        Hayir = 0,
        [Description("Evet")]
        Evet = 1
    }


    [EnumInfo(typeof(PRJ_Project), "IsDelete")]
    public enum EnumPRJ_ProjectIsDelete
    {
        [Description("Hayır")]
        Hayir = 0,
        [Description("Evet")]
        Evet = 1
    }

    [EnumInfo(typeof(PRJ_Project), "PriceType")]
    public enum EnumPRJ_ProjectPriceType
    {
        [Description("Türk Lirası")]
        TurkLirasi = 0,
        [Description("Dolar")]
        Dolar = 1,
        [Description("Euro")]
        Euro = 2,
        [Description("Tenge")]
        Tenge = 3
    }

    partial class WorkOfTimeDatabase
    {

        public PRJ_Project[] GetPRJ_ProjectByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRJ_Project>().Where(x => x.CompanyId == companyId || x.CorporationId == companyId).Execute().ToArray();
            }
        }
    }
}
