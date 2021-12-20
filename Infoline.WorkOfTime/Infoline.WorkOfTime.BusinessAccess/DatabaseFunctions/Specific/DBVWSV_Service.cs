using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;
namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SV_Service), "DeliveryType")]
    public enum EnumSV_Service
    {
        [Description("Bayi")]
        CompanyPerson = 0,
        [Description("Elden Teslim")]
        HandedOver = 1,
        [Description("Kargo")]
        Cargo = 2,
        [Description("Diğer")]
        Other = 3,
    }
    partial class WorkOfTimeDatabase
    {
    }
}
