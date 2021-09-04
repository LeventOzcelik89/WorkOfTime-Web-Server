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

        public HR_PersonPosition[] GetHR_PersonPositionByHrPersonId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_PersonPosition>().Where(a => a.HrPersonId == id).Execute().ToArray();
            }
        }
    }
}
