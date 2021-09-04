using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {
        public Guid? GetTableToId(string TableName)
        {
            using (var db = GetDB())
            {
                return db.ExecuteScaler<Guid?>("SELECT TOP 1 id FROM " + TableName + "");
            }
        }

        public SH_Pages[] GetSH_PagesByIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<SH_Pages>().Where(s => s.id.In(ids)).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public ResultStatus DeleteSH_RolesAll(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteNonQuery("Delete from dbo.SH_Role");
            }
        }

        public ResultStatus DeleteSH_UserRoleAll(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteNonQuery("Delete from dbo.SH_UserRole");
            }
        }

        public ResultStatus DeleteSH_PagesAll(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteNonQuery("Delete from dbo.SH_Pages");
            }
        }


        public ResultStatus DeleteSH_PagesRoleAll(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteNonQuery("Delete from dbo.SH_PagesRole");
            }
        }
    }
}
