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

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {

        public VWCMP_Storage[] GetVWCMP_StorageMyStorage(string keywords, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Storage>().Where(a => a.myStorage == true && (a.fullName.Contains(keywords) || a.address.Contains(keywords) || a.locationId_Title.Contains(keywords))).Execute().ToArray();
            }
        }


        public VWCMP_Storage GetVWCMP_StorageByCode(string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_Storage>().Where(a => a.code == code).Execute().FirstOrDefault();
            }
        }

        public VWCMP_Storage GetVWCMP_StorageByName(string name, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_Storage>().Where(a => a.name == name).Execute().FirstOrDefault();
            }
        }

        public VWCMP_Storage[] GetVWCMP_StorageOwner(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_Storage>().Where(a => a.myStorage == true).Execute().ToArray();
            }
        }

        public VWCMP_Storage[] GetVWCMP_StorageByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_Storage>().Where(a => a.companyId == companyId).Execute().ToArray();
            }
        }

        public Guid[] GetVWCMP_StorageBySupervisorIdOfCompanyIds(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCMP_Storage>().Where(a => a.supervisorId == userId).Execute().GroupBy(x=>x.companyId.Value).Select(x=>x.Key).ToArray();
            }
        }


        public VWCMP_Storage GetVWCMP_StorageByNameOrCode(string name, string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Storage>().Where(a => a.name == name || a.code == code).Execute().FirstOrDefault();
            }
        }
        public VWCMP_Storage[] GetVW_CMP_StorageSelected(DbTransaction tran = null)
        {
            using (var db = GetDB())
            {
                return db.Table<VWCMP_Storage>()
                    .Select(x => new
                    {
                        id = x.id,
                        code = x.code,
                        phone = x.phone,
                        address = x.address,
                        location = x.location,
                        companyId = x.companyId,
                        name = x.name,
                        companyId_Title = x.companyId_Title,
                        myStorage = x.myStorage
                    }).OrderBy(x => x.code).Execute<VWCMP_Storage>().ToArray();
            }
        }
    }
}
