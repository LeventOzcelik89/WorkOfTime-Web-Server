using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    [EnumInfo(typeof(HR_StaffNeedsPerson), "status")]
    public enum EnumHR_StaffNeedsPersonstatus
    {
        [Description("İşlem Yapılmadı")]
        Bos = 0,
        [Description("Onaylandı")]
        Onay = 1,
        [Description("Reddedildi")]
        Red = 2
    }

    public partial class WorkOfTimeDatabase
    {
        public HR_StaffNeedsPerson GetHR_StaffNeedsPersonByNeedAndPersonId(Guid id , Guid NeedId)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_StaffNeedsPerson>().Where(a => a.HrPersonId == id && a.HrStaffNeedsId == NeedId).Execute().FirstOrDefault();
            }
        }


        public HR_StaffNeedsPerson[] GetHR_StaffNeedsPersonByNeedsId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_StaffNeedsPerson>().Where(a => a.HrStaffNeedsId == id).Execute().ToArray();
            }
        }
    }
}

