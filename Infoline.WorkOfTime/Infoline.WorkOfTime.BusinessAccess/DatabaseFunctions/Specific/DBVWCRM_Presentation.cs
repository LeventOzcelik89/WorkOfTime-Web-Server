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

        public VWCRM_Presentation[] GetVWCRM_PresentationBySalesPersonIds(Guid[] salesPersonIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_Presentation>().Where(a => a.SalesPersonId.In(salesPersonIds)).Execute().ToArray();
            }
        }


        public VWCRM_Presentation[] GetVWCRM_PresentationInsertByToday(int day, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_Presentation>().Where(a => a.created >= DateTime.Now.Date.AddDays(day) && a.created<= DateTime.Now.Date.AddDays(1).AddSeconds(-1)).Execute().ToArray();
            }
        }

        public VWCRM_Presentation[] GetVWCRM_PresentationByCreated(Guid createdby)
        {
            using (var db = GetDB())
            {
                return db.Table<VWCRM_Presentation>().Where(a => a.createdby == createdby).Execute().ToArray();
            }
        }

        public VWCRM_Presentation[] GetVWCRM_PresentationPositiveResultByToday(int day, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_Presentation>().Where(a => a.Stage_Title== "Olumlu" && a.created >= DateTime.Now.Date.AddDays(day) && a.created <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)).Execute().ToArray();
            }
        }

        public VWCRM_Presentation[] GetVWCRM_PresentationByPresentationStageId(string[] id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCRM_Presentation>().Where(a => a.PresentationStageId.In(id)).Execute().ToArray();
            }
        }

        public VWCRM_Presentation[] GetVWCRM_PresentationByPresentationActionId(Guid presentationId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCRM_Presentation>().Where(a => a.id == presentationId).Execute().ToArray();
            }
        }

    }
}