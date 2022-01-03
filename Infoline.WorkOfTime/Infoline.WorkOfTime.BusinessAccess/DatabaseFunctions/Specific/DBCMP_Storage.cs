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

    [EnumInfo(typeof(CMP_Storage), "locationType")]
    public enum EnumCMP_StorageLocationType
    {
        [Description("Depo"), Generic("order", "1")]
        Depo = 0,
        [Description("Alan"), Generic("order", "2")]
        Alan = 1
    }
    partial class WorkOfTimeDatabase
    {
        public CMP_Storage[] GetCMP_StroageByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_Storage>().Where(a => a.companyId == companyId).Execute().ToArray();
            }
        }
        public CMP_Storage GetCMP_StorageByCompanyIdFirst(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_Storage>().Where(a => a.companyId == companyId).OrderBy(x => x.created).Execute().FirstOrDefault();
            }
        }
        public CMP_Storage[] GetCMP_StorageByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_Storage>().Where(a => a.companyId == companyId).OrderBy(x => x.created).Execute().ToArray();
            }
        }

        public CMP_Storage GetCMP_StorageByCode(string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_Storage>().Where(a => a.code == code).OrderBy(x => x.created).Execute().FirstOrDefault();
            }
        }

        public CMP_Storage[] GetCMP_StorageByCodes(string[] codes, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_Storage>().Where(x => x.code.In(codes)).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public CMP_Storage[] GetCMP_StorageByIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_Storage>().Where(a => a.id.In(ids)).OrderBy(x => x.created).Execute().ToArray();
            }
        }
        public CMP_Storage[] GetCMP_StorageFromPidById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<CMP_Storage>().Where(a => a.pid == id).OrderBy(x => x.created).Execute().ToArray();
            }
        }


    }
}
