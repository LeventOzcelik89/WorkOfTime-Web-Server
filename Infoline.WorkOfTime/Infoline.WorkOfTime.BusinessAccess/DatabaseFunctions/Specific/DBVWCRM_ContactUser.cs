using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {

        public VWCRM_ContactUser[] GetVWCRM_ContactUserByContactId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<VWCRM_ContactUser>().Where(a => a.id == id).Execute().ToArray();

            }
        }

        public VWCRM_ContactUser[] GetVWCRM_ContactUsersById(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCRM_ContactUser>().Where(a => a.id == id).Execute().ToArray();
            }
        }


        public VWCRM_ContactUser[] GetVWCRM_ContactSalesUsersByIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_ContactUser>().Where(x => x.id.In(ids)).Execute().ToArray();
                //return db.ExecuteReader<VWCRM_ContactUser>("SELECT * FROM VWCRM_ContactUser WHERE id IN ('" + String.Join("','", ids) + "')").ToArray();
            }
        }

        public VWCRM_ContactUser[] GetVWCRM_ContactUserByContactIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                if (ids.Count() > 2000)
                {
                    return db.ExecuteReader<VWCRM_ContactUser>("SELECT * From VWCRM_ContactUser with(nolock) where id in(" + string.Format("'{0}'", string.Join("','", ids)) + ")").ToArray();
                }
                return db.Table<VWCRM_ContactUser>().Where(a => a.id.In(ids)).Execute().ToArray();
            }
        }


        public VWCRM_ContactUser[] GetVWCRM_ContactUserByMostMeetingPerformingPerson()
        {
            using (var db = GetDB())
            {
                return db.Table<VWCRM_ContactUser>().Where(a => a.ContactStatus == (int)EnumCRM_ContactContactStatus.ToplantiGerceklesti && a.ContactEndDate >= DateTime.Now.Date &&
                                                    a.ContactEndDate <= DateTime.Now.Date).Execute().ToArray();

            }
        }

    }
}
