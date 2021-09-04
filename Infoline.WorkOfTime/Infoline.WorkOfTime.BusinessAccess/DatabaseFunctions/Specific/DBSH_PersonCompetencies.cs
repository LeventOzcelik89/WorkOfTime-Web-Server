using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SH_PersonCompetencies), "CompetenciesLevel")]
    public enum EnumSH_PersonCompetenciesLevel
    {
        [Description("Başlangıç")]
        Baslangic = 0,
        [Description("Temel")]
        Temel = 1,
        [Description("Orta")]
        Orta = 2,
        [Description("İyi")]
        Iyi = 3,
        [Description("İleri")]
        Ileri = 4,
    }
}
