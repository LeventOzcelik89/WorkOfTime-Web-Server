using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;
namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SV_CustomerUser), "customerType")]
    public enum EnumSV_CustomerUser
    {
        [Description("Bayi Personeli")]
        CompanyPerson = 0,
        [Description("Personel")]
        Person = 1,
        [Description("Diğer")]
        Other = 2,
    }
    partial class WorkOfTimeDatabase
    {
    }
}
