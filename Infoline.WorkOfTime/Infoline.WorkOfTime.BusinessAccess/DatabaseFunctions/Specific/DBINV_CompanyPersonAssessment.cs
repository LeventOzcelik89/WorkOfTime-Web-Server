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
    [EnumInfo(typeof(INV_CompanyPersonAssessment), "AssessmentType")]
    public enum EnumINV_CompanyPersonAssessmentType
    {
        [Description("2 Aylık Değerlendirme")]
        IkiAy = 2,
        [Description("6 Aylık Değerlendirme")]
        AltiAy = 6,
        [Description("Yıllık Değerlendirme")]
        Yillik = 12,
    }

    [EnumInfo(typeof(INV_CompanyPersonAssessment), "ConfirmStatus")]
    public enum EnumINV_CompanyPersonConfirmStatus
    {
        [Description("İşe Uygundur.")]
        Uygun = 1,
        [Description("Başka Bir İşte Değerlendirilebilir")]
        BaskaIs = 2,
        [Description("İşe Uygun Değildir")]
        UygunDegil = 0,
    }

    [EnumInfo(typeof(INV_CompanyPersonAssessment), "ApproveStatus")]
    public enum EnumINV_CompanyPersonAssessmentApproveStatus
    {
        [Description("1.Yöneticinin Değerlendirmesi Bekleniyor.")]
        Yonetici1Degerlendirme = 0,
        [Description("1. Yönetici Değerlendirdi. (2.Yöneticinin Değerlendirmesi Bekleniyor)")]
        Yonetici2Degerlendirme = 1,
        [Description("2. Yönetici Değerlendirdi. (Genel Müdürün Değerlendirmesi Bekleniyor)")]
        GenelMudurDegerlendirme = 2,
        [Description("Genel Müdür Değerlendirdi. (İK'nın Değerlendirmesi Bekleniyor)")]
        IkDegerlendirme = 3,
        [Description("İ.K. Islak İmzayı Yükledi. (Süreç Tamamlandı.)")]
        SurecTamamlandi = 4
    }
    partial class WorkOfTimeDatabase
    {


       

    }
}
