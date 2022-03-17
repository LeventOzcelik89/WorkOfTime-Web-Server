using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(PRD_Stocktaking), "priority")]
    public enum EnumPRD_StocktakingStatus
    {
        [Description("Sayım Başladı"), Generic("color", "EF5352")]
        SayimBasladi = 0,
        [Description("Stoklara İşlendi"), Generic("color", "F8AC59")]
        StoklaraIslendi = 1,
        [Description("Sayım Tamamlandı"), Generic("color", "1ab394")]
        SayimTamamlandi = 2,
    }

}


