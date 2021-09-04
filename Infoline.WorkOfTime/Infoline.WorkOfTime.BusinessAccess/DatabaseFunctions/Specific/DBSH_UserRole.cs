using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {
        public SH_UserRole[] GetSH_UserRoleByRoleId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_UserRole>().Where(x => x.roleid == id).Execute().ToArray();
            }
        }
        public SH_UserRole[] GetSH_UserRoleByRoleId(Guid roleid)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_UserRole>().Where(a => a.roleid == roleid).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
        public SH_UserRole[] GetSH_UserRoleByUserId(Guid userId)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_UserRole>().Where(a => a.userid == userId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
        public SH_UserRole[] GetSH_UserRoleByUserIds(Guid[] userIds)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_UserRole>().Where(a => a.userid.In(userIds)).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
        public SH_UserRole[] GetSH_UserRoleByUserIdRoleId(Guid userid, Guid roleid)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_UserRole>().Where(a => a.roleid == roleid && a.userid == userid).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
        public SH_UserRole[] GetSH_UserRoleByRoleIds(string[] roleIds)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_UserRole>().Where(a => a.roleid.In(roleIds)).Execute().ToArray();
            }
        }
        public Guid[] GetSH_UserByRoleId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_UserRole>().Where(x => x.roleid == id && x.userid != null).Select(a => new SH_UserRole { userid = a.userid }).Execute().Select(a => a.userid.Value).ToArray();
            }
        }
        public List<Guid?> GetSH_UserByRoleIdList(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_UserRole>().Where(x => x.roleid == id && x.userid != null).Select(a => new SH_UserRole { userid = a.userid }).Execute().Select(a => a.userid).ToList();
            }
        }
    }
}
