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
        public VWCSM_Student[] GetVWCSM_StudentByIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWCSM_Student>().Where(a => a.id.In(ids)).Execute().ToArray();
            }
        }

    }
}
