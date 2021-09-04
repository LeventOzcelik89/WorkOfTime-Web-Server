using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(HR_Person), "ArrivalType")]
    public enum EnumHR_PersonArrivalType
    {
        [Description("Kariyer Sitesi")]
        Web = 0,
        [Description("Mail")]
        Mail = 1,
        [Description("Referans Aracılığı İle")]
        Referans = 2
    }

    [EnumInfo(typeof(HR_Person), "Education")]
    public enum EnumHR_PersonEducation
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

    [EnumInfo(typeof(HR_Person), "MilitaryStatus")]
    public enum EnumHR_PersonMilitaryStatus
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

    [EnumInfo(typeof(HR_Person), "Language")]
    public enum EnumHR_PersonLanguage
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

    [EnumInfo(typeof(HR_Person), "LanguageLevel")]
    public enum EnumHR_PersonLanguageLevel
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

    [EnumInfo(typeof(HR_Person), "DriverLicense")]
    public enum EnumHR_PersonDriverLicense
    {
        [Description("A")]
        A = 0,
        [Description("B")]
        B = 1,
        [Description("C")]
        C = 2,
        [Description("D")]
        D = 3,
        [Description("F")]
        F = 4,
        [Description("G")]
        G = 5,
        [Description("B-A")]
        BA = 6
    }
    [EnumInfo(typeof(HR_Person), "Gender")]
    public enum EnumHR_PersonGender
    {
        [Description("Kadın")]
        Kadin = 0,
        [Description("Erkek")]
        Erkek = 1

    }
    [EnumInfo(typeof(HR_Person), "MaritalStatus")]
    public enum EnumHR_PersonMaritalStatus
    {
        [Description("Bekar")]
        Bekar = 0,
        [Description("Evli")]
        Evli = 1
    }

    public partial class WorkOfTimeDatabase
    {

    }
}
