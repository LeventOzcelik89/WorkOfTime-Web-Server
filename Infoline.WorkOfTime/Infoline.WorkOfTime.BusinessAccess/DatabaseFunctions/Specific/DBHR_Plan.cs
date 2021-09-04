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
    [EnumInfo(typeof(HR_Plan), "Result")]
    public enum EnumHR_PlanResult
    {
        [Description("Olumlu"),Generic("icon", "fa-check", "color", "rgb(26, 179, 148)", "EnumDesc", "Olumlu")]
        Olumlu = 0,
        [Description("Olumsuz"), Generic("icon", "fa-close", "color", "rgb(237, 85, 101)", "EnumDesc", "Olumsuz")]
        Olumsuz = 1,
        [Description("Tekrar Mülakat"), Generic("icon", "fa-history", "color", "rgb(28, 132, 198)", "EnumDesc", "Tekrar")]
        Tekrar = 2,
        [Description("Diğer"), Generic("icon", "fa-question", "color", "rgb(121, 167, 213)", "EnumDesc", "Diger")]
        Diger = 3,
        [Description("Henüz Görüşülmedi"), Generic("icon", "fa-bell", "color", "rgb(61, 77, 93)", "EnumDesc", "Gorusulmedi")]
        Gorusulmedi = 4,
        [Description("Daha Sonra Karar Ver"), Generic("icon", "fa-spinner", "color", "rgb(248, 172, 89)", "EnumDesc", "DahaSonra")]
        DahaSonra = 5
    }
    public partial class WorkOfTimeDatabase
    {

        public HR_Plan[] GetHR_PlanByHrPersonId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_Plan>().Where(a => a.HrPersonId == id).Execute().ToArray();
            }
        }


        public HR_Plan[] GetHR_PlanByHrNeedsId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_Plan>().Where(a => a.StaffNeedsId == id).Execute().ToArray();
            }
        }
    }
    
}
