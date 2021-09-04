using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SH_Role), "roletype")]
    public enum EnumSH_RolesRoleType
    {
        [Description("Sistem Tanımlı")]
        systemdefine = 0,
        [Description("Kullanıcı Tanımlı")]
        userdefine = 1,

    }


    public partial class WorkOfTimeDatabase
    {
        public SH_Role GetSH_RoleByName(string name)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_Role>().Where(a => a.rolname == name).Execute().FirstOrDefault();
            }
        }
    }
}
