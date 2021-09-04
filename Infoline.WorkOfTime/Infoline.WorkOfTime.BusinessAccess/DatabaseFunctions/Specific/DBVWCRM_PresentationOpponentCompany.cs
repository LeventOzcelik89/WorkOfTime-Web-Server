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
        public VWCRM_PresentationOpponentCompany[] GetVWCRM_PresentationOpponentCompanyByPresentationIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                if(ids.Count() > 2000)
                {
                    return db.ExecuteReader<VWCRM_PresentationOpponentCompany>("SELECT * From VWCRM_PresentationOpponentCompany with(nolock) where PresentationId in(" + string.Format("'{0}'", string.Join("','", ids)) + ") Order by OpponentCompany_Title desc").ToArray();
                }
                return db.Table<VWCRM_PresentationOpponentCompany>().Where(a => a.PresentationId.In(ids)).OrderByDesc(a => a.OpponentCompany_Title).Execute().ToArray();
            }
        }

    }

}
