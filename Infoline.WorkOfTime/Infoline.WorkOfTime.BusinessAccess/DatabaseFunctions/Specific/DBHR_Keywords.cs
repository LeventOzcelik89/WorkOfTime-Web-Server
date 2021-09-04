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
        public HR_Keywords[] GetHR_KeywordsList(List<Guid?> id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_Keywords>().Where(a => a.id.In(id.ToArray())).Execute().ToArray();
            }
        }

        public HR_Keywords[] GetHR_KeywordsByIds(Guid[] id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_Keywords>().Where(a => a.id.In(id)).Execute().ToArray();
            }
        }
    }
}
