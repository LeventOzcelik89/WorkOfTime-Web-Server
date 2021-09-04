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
        public CRM_PresentationProducts[] GetCRM_PresentationProductsByPresentationId(Guid presentationId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CRM_PresentationProducts>().Where(a => a.PresentationId == presentationId).Execute().ToArray();
            }
        }

    }
}
