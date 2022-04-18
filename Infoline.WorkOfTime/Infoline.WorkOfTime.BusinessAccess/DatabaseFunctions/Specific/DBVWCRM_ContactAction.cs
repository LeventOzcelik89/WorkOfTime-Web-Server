using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {

        public VWCRM_ContactAction[] GetVWCRM_ContactActionByContactId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<VWCRM_ContactAction>().Where(a => a.ContactId == id).Execute().ToArray();

            }
        }


        public VWCRM_ContactAction[] GetVWCRM_ContactActionByContactIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCRM_ContactAction>().Where(a => a.ContactId.In(ids)).Execute().ToArray();
            }
        }

    }
}
