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
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public VWPA_AdvanceConfirmation[] GetVWPA_AdvanceConfirmationByAdvanceId(Guid advanceId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWPA_AdvanceConfirmation>().Where(a => a.advanceId == advanceId).OrderBy(x=>x.ruleOrder).Execute().ToArray();
            }
        }
    }
}
