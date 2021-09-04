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

namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {

        public HR_PlanPerson[] GetHR_PlanPersonByPlanId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_PlanPerson>().Where(a => a.HrPlanId == id).Execute().ToArray();
            }
        }



    }
    
}
