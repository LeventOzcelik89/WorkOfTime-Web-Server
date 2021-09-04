using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(HR_StaffNeeds), "ArrivalType")]
    public enum EnumHR_StaffNeedsArrivalType
    {
        [Description("Kariyer Sitesi")]
        Web = 0,
        [Description("Mail")]
        Mail = 1,
        [Description("Referans Aracılığı ile")]
        Referans = 2
    }

    [EnumInfo(typeof(HR_StaffNeeds), "Education")]
    public enum EnumHR_StaffNeedsEducation
    {
        [Description("Lise")]
        Lise = 0,
        [Description("Ön Lisans")]
        OnLisans = 1,
        [Description("Lisans")]
        Lisans = 2,
        [Description("Yüksek Lisans")]
        YuksekLisans = 3,
        [Description("Doktora")]
        Doktora = 4,
    }

    [EnumInfo(typeof(HR_StaffNeeds), "MilitaryStatus")]
    public enum EnumHR_StaffNeedsMilitaryStatus
    {
        [Description("Yapıldı")]
        Yapildi = 0,
        [Description("Tecilli")]
        Tecilli = 1,
        [Description("Muaf")]
        Muaf = 2,
        [Description("Yapılmadı")]
        Yapilmadi = 3
    }

    [EnumInfo(typeof(HR_StaffNeeds), "Language")]
    public enum EnumHR_StaffNeedsLanguage
    {
        [Description("İngilizce")]
        En = 0,
        [Description("Almanca")]
        Gr = 1,
        [Description("İtalyanca")]
        Itl = 2,
        [Description("İspanyolca")]
        Spa = 3,
        [Description("Fransızca")]
        Fr = 4,
        [Description("Çince")]
        Cince = 5,
        [Description("Rusça")]
        Ru = 6
    }

    [EnumInfo(typeof(HR_StaffNeeds), "LanguageLevel")]
    public enum EnumHR_StaffNeedsLanguageLevel
    {
        [Description("Kötü")]
        Kotu = 0,
        [Description("Az")]
        Az = 1,
        [Description("Orta")]
        Orta = 2,
        [Description("İyi")]
        Iyi = 3,
        [Description("Çok İyi")]
        Cokiyi = 4
    }

    [EnumInfo(typeof(HR_StaffNeeds), "DriverLicense")]
    public enum EnumHR_StaffNeedsDriverLicense
    {
        [Description("Evet")]
        Evet = 0,
        [Description("Hayır")]
        Hayir = 1,
        [Description("Önemli Değil")]
        OnemliDegil = 2
    }

    [EnumInfo(typeof(HR_StaffNeeds), "ReasonForStaffDemand")]
    public enum EnumHR_StaffNeedsReasonForStaffDemand
    {
        [Description("İşten Ayrılan Personel Yerine")]
        IstenAyrilanPersonelYerine = 0,
        [Description("Organizasyon İhtiyacı")]
        OrganizasyonIhtiyaci = 1
    }

    [EnumInfo(typeof(HR_StaffNeeds), "EmploymentStatus")]
    public enum EnumHR_StaffNeedsEmploymentStatus
    {
        [Description("Belirsiz Süreli")]
        BelirsizSureli = 0,
        [Description("Belirli Süreli")]
        BelirliSureli = 1,
        [Description("İhale Kapsamında")]
        IhaleKapsaminda = 2
    }

    [EnumInfo(typeof(HR_StaffNeeds), "Gender")]
    public enum EnumHR_StaffNeedsGender
    {
        [Description("Erkek")]
        Erkek = 0,
        [Description("Kadın")]
        Kadin = 1,
        [Description("Önemli Değil")]
        OnemliDegil = 2
    }

    [EnumInfo(typeof(HR_StaffNeeds), "MaritalStatus")]
    public enum EnumHR_StaffNeedsMaritalStatus
    {
        [Description("Evli")]
        Evli = 0,
        [Description("Bekar")]
        Bekar = 1,
        [Description("Önemli Değil")]
        OnemliDegil = 2
    }

    [EnumInfo(typeof(HR_StaffNeeds), "Travelability")]
    public enum EnumHR_StaffNeedsTravelability
    {
        [Description("Evet")]
        Evet = 0,
        [Description("Hayır")]
        Hayir = 1
    }
    [EnumInfo(typeof(HR_StaffNeeds), "priority")]
    public enum EnumHR_StaffNeedsPriority
    {
        [Description("Düşük (Acil Değil)")]
        Dusuk = 0,
        [Description("Orta (Yakın Tarihte)")]
        Orta = 1,
        [Description("Yüksek (Acil)")]
        Yuksek = 2
    }
}


