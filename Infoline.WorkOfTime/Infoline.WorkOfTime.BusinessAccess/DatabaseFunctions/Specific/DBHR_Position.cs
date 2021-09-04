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
        public HR_Position[] GetHR_PositionList(List<Guid?> id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_Position>().Where(a => a.id.In(id.ToArray())).Execute().ToArray();
            }
        }


        public HR_Position[] GetHR_PositionByIds(Guid[] id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_Position>().Where(a => a.id.In(id)).Execute().ToArray();
            }
        }
    }
}
