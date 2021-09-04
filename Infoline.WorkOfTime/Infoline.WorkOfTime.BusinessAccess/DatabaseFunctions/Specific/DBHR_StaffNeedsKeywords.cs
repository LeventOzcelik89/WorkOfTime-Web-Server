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
        public HR_StaffNeedsKeywords[] GetHR_StaffNeedsKeywordsByNeedsId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_StaffNeedsKeywords>().Where(a => a.HrStaffNeedsId == id).Execute().ToArray();
            }
        }

        public HR_StaffNeedsKeywords[] GetHR_StaffNeedsKeywordsList(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_StaffNeedsKeywords>().Where(a => a.HrStaffNeedsId == id).Execute().ToArray();
            }
        }
    }
}
