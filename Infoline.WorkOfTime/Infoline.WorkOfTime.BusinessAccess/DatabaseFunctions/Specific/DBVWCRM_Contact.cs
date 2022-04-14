using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {


        ///TODO:ŞAHİN ELİK
        public VWCRM_Contact[] GetVWCRM_ContactByCalendar(DateTime start, DateTime end)
        {
            using (var db = GetDB())
            {
                return db.Table<VWCRM_Contact>().Where(c => c.ContactStartDate >= start && c.ContactStartDate <= end && c.PresentationId != null).Execute().ToArray();
            }
        }

        public VWCRM_Contact[] GetVWCRM_ContactByPresentationIds(Guid[] id)
        {
            using (var db = GetDB())
            {
                return db.Table<VWCRM_Contact>().Where(a => a.PresentationId.In(id)).Execute().ToArray();

            }
        }


        public VWCRM_Contact[] GetVWCRM_ContactTodayHappeningMeeting(int day, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_Contact>().Where(x => x.ContactStatus == (int)EnumCRM_ContactContactStatus.ToplantiGerceklesti &&
                x.ContactEndDate >= DateTime.Now.Date.AddDays(day) && x.ContactEndDate <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)).Execute().ToArray();
            }
        }



        public VWCRM_Contact[] GetVWCRM_ContactTodayCanceldMeeting(int day, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_Contact>().Where(x => x.ContactStatus == (int)EnumCRM_ContactContactStatus.ToplantiIptal &&
                x.created >= DateTime.Now.Date.AddDays(day) && x.created <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)).Execute().ToArray();
            }
        }

        public VWCRM_Contact[] GetVWCRM_ContactByCustomerCompanyIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_Contact>().Where(x => x.CustomerCompanyId.In(ids)).Execute().ToArray();
            }
        }

        public VWCRM_Contact[] GetVWCRM_ContactForStartDateAndEndDate(DateTime ContactStartDate, DateTime ContactEndDate,  DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_Contact>().Where(x => x.ContactStartDate >= ContactStartDate && x.ContactEndDate < ContactEndDate).Execute().ToArray();
            }
        }

        public VWCRM_Contact[] GetVWCRM_ContactByCreatedByIds(Guid[] ids, DateTime ContactStartDate, DateTime ContactEndDate, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCRM_Contact>().Where(a => a.createdby.In(ids) && a.ContactStartDate >= ContactStartDate && a.ContactEndDate < ContactEndDate).Execute().ToArray();
            }
        }




    }
}
