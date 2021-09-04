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



        public VWUT_Location GetVWUT_LocationGetByPoint(string location, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteReader<VWUT_Location>("SELECT * FROM VWUT_Location WHERE   ((code!='TR' and type='" + (int)EnumUT_LocationType.Sehir + "') or (code='TR' and type='" + (int)EnumUT_LocationType.İlce + "')) and polygon.STIntersects(GEOGRAPHY::STGeomFromText('" + location + "', 4326)) = 1").FirstOrDefault();
            }
        }

    }
}
