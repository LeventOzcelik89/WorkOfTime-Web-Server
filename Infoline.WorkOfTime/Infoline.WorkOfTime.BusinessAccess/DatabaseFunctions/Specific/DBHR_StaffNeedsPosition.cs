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
        public HR_StaffNeedsPosition[] GetHR_StaffNeedsPositionByNeedsId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_StaffNeedsPosition>().Where(a => a.HrStaffNeedsId == id).Execute().ToArray();
            }
        }

        public HR_StaffNeedsPosition[] GetHR_StaffNeedsPositionList(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_StaffNeedsPosition>().Where(a => a.HrStaffNeedsId == id).Execute().ToArray();
            }
        }
    }
}
