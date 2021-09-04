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

        public VWPA_Account GetVWPA_AccountByCode(string code, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPA_Account>().Where(a => a.code == code).Execute().FirstOrDefault();
            }
        }

        public VWPA_Account GetVWPA_AccountByDataId(Guid dataId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPA_Account>().Where(a => a.dataId == dataId && a.isDefault == true).Execute().FirstOrDefault();
            }
        }

        public VWPA_Account GetVWPA_AccountByDataIdDataTable(Guid dataId, string dataTable, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPA_Account>().Where(a => a.dataId == dataId && a.dataTable == dataTable && a.isDefault == true).Execute().FirstOrDefault();
            }
        }

        public VWPA_Account[] GetVWPA_AccountsByDataIdDataTable(Guid dataId, string dataTable, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPA_Account>().Where(a => a.dataId == dataId && a.dataTable == dataTable).Execute().ToArray();
            }
        }

        public VWPA_Account GetVWPA_AccountByIBAN(string iban, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPA_Account>().Where(a => a.iban == iban).Execute().FirstOrDefault();
            }
        }
        public VWPA_Account[] GetVWPA_AccountMy(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPA_Account>().Where(x=>x.myAccount == true).Execute().ToArray();
            }
        }

    }
}
