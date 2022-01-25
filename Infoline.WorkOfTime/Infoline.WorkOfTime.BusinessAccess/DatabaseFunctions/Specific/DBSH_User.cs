using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(SH_User), "type")]
    public enum EnumSH_UserType
    {
        [Description("Sahip Olduğum Şirket Personeli")]
        MyPerson = 0,
        [Description("Diğer İşletme Personeli")]
        OtherPerson = 1,
        [Description("Bayi Personeli")]
        CompanyPerson= 2,
    }
    [EnumInfo(typeof(SH_User), "status")]
    public enum EnumSH_UserStatus
    {
        [Description("Evet")]
        Onaylanmis = 1,
        [Description("Hayır")]
        OnayBekliyor = 0
    }

    public enum EnumSH_UserCertificateStatus
    {
        [Description("Süreli")]
        Sureli = 1,
        [Description("Süresiz")]
        Suresiz = 2
    }
    public partial class WorkOfTimeDatabase
    {
        public VWSH_User[] GetSH_UserByRoleId(string roleid)
        {
            using (var db = GetDB())
            {
                var id = new Guid(roleid);
                var userIds = db.Table<SH_UserRole>().Where(a => a.roleid == id && a.userid != null).OrderByDesc(a => a.created).Execute().ToArray();
                return db.Table<VWSH_User>().Where(s => s.id.In(userIds.Select(a => a.userid.Value).ToArray())).Execute().ToArray();
            }
        }


        public Guid[] GetUserByRoleId(string roleid)
        {
            using (var db = GetDB())
            {
                var id = new Guid(roleid);
                var userIds = db.Table<SH_UserRole>().Where(a => a.roleid == id && a.userid != null).Execute().GroupBy(a => a.userid).Where(a => a.Key.HasValue).Select(a => a.Key.Value).ToList();
                if (userIds.Count() == 0)
                {
                    userIds.Add(Guid.NewGuid());
                }
                return userIds.ToArray();
            }
        }


        public SH_User[] GetSH_UserMyPerson(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_User>().Where(a => a.type == (int)EnumSH_UserType.MyPerson).Execute().ToArray();
            }
        }

        public SH_User[] GetSH_UserOtherPerson(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_User>().Where(a => a.type == (int)EnumSH_UserType.OtherPerson).Execute().ToArray();
            }
        }

        public SH_User[] GetSH_UserByIds(Guid[] ids)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(x => x.id.In(ids)).Execute().ToArray();
            }
        }

        public SH_User[] GetSH_UserByEmail(string email)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(x => x.email == email).Execute().ToArray();
            }
        }

        public SH_User[] GetSH_UserByEmailWithNotEqualUserId(string email, Guid userId)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(x => x.email == email && x.id != userId).Execute().ToArray();
            }
        }
    }
}
