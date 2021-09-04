using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public partial class WorkOfTimeDatabase
    {
        public INV_CommissionsProjects[] GetINV_CommissionsProjectCommissionId(Guid Id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CommissionsProjects>().Where(a => a.IdCommissions == Id).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }

    }
}
