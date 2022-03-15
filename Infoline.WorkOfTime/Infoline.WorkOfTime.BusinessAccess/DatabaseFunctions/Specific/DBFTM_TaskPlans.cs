using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using System.ComponentModel;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(FTM_TaskPlan), "dayFrequency")]
    public enum EnumFTM_TaskPlanDayFrequency
    {
        [Description("Gün")]
        Day = 0,
        [Description("Pazar")]
        Pazar = 3,
        [Description("Pazartesi")]
        Pazartesi = 4,
        [Description("Salı")]
        Sali = 5,
        [Description("Çarşamba")]
        Carsamba = 6,
        [Description("Perşembe")]
        Persembe = 7,
        [Description("Cuma")]
        Cuma = 8,
        [Description("Cumartesi")]
        Cumartesi = 9
    }

    [EnumInfo(typeof(FTM_TaskPlan), "monthFrequency")]
    public enum EnumFTM_TaskPlanMonthFrequency
    {
        [Description("İlk")]
        Ilk = 0,
        [Description("İkinci")]
        Ikinci = 1,
        [Description("Üçüncü")]
        Ucuncu = 2,
        [Description("Dördüncü")]
        Dorduncu = 3,
        [Description("Son")]
        Son = 4,
    }

    [EnumInfo(typeof(FTM_TaskPlan), "frequency")]
    public enum EnumFTM_TaskPlansFrequency
    {
        [Description("Günlük")]
        Gunluk = 1,
        [Description("Haftalık")]
        Haftalik = 2,
        [Description("Aylık")]
        Aylik = 3,
        [Description("Tekrarlanan")]
        Tekrarlanan = 4
    }

    [EnumInfo(typeof(FTM_TaskPlan), "taskCreationTime")]
    public enum EnumFTM_TaskPlansTaskCreationTime
    {
        [Description("Hemen")]
        Hemen = 1,
        [Description("1 Ay Önce")]
        AyOnce = 2,
        [Description("1 Hafta Önce")]
        HaftaOnce = 3,
        [Description("1 Gün Önce")]
        GunOnce = 4,
        [Description("Görev Gününde")]
        Gununde = 5,
    }

    partial class WorkOfTimeDatabase
    {

    }
}
