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

        public VWHR_PlanPerson GetVWHR_PlanPersonByPlanIdAndResultNotGorusulmedi(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<VWHR_PlanPerson>().Where(c => c.HrPlanId == id && c.Result != (Int32)EnumHR_PlanResult.Gorusulmedi).Execute().FirstOrDefault();
            }
        }

        public VWHR_PlanPerson[] GetVWHR_PlanPersonByCalendar(DateTime start, DateTime end)
        {
            using (var db = GetDB())
            {
                return db.Table<VWHR_PlanPerson>().Where(c => c.PlanDate >= start && c.PlanDate <= end).Execute().ToArray();
            }
        }


    }

}
