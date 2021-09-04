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
using GeoAPI.Geometries;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {
        public HR_StaffNeedsRequester[] GetHR_StaffNeedsRequesterByStaffNeedId(Guid id,DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<HR_StaffNeedsRequester>().Where(a => a.StaffNeedId == id).Execute().ToArray();
            }
        }

    }
}
