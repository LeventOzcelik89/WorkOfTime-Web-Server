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
        public FVR_Favorites GetFVR_FavoritesControl(Guid userId, string Area, string Controller, string Method)
        {
            using (var db = GetDB())
            {
                var str = "Select * from FVR_Favorites where userId =" + "'{0}'";
                if (Area != null)
                {
                    str += " and Area =" + "'{1}'";
                }

                if (Controller != null)
                {
                    str += " and Controller =" + "'{2}'";
                }

                if (Method != null)
                {
                    str += " and Method =" + "'{3}'";
                }
                

                var sql = string.Format(str, userId, Area, Controller, Method);

                return db.ExecuteReader<FVR_Favorites>(sql).FirstOrDefault();
            }
        }

        public FVR_Favorites[] GetFVR_FavoritesByUserId(Guid userId)
        {
            using (var db = GetDB())
            {
                return db.Table<FVR_Favorites>().Where(a => a.userId == userId).Execute().ToArray();
            }
        }
    }
}
