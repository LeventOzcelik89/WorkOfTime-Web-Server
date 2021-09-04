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
        public VWCRM_PresentationProducts[] GetVWCRM_PresentationProductsByPresentationId(Guid presentationId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCRM_PresentationProducts>().Where(a => a.PresentationId == presentationId).OrderByDesc(a => a.Product_Title).Execute().ToArray();
            }
        }
        public VWCRM_PresentationProducts[] GetVWCRM_PresentationProductsByPresentationIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                if (ids.Count() > 2000)
                {
                    return db.ExecuteReader<VWCRM_PresentationProducts>("SELECT * From VWCRM_PresentationProducts with(nolock) where PresentationId in(" + string.Format("'{0}'", string.Join("','", ids)) + ") Order by Product_Title desc").ToArray();
                }
                return db.Table<VWCRM_PresentationProducts>().Where(a => a.PresentationId.In(ids)).OrderByDesc(a => a.Product_Title).Execute().ToArray();
            }
        }
    }
}