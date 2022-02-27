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
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(UT_Template), "status")]
    public enum EnumUT_TemplateStatus
    {
        [Description("Pasif"),Generic("icon","fa fa-circle","color","#3d4d5d")]
        passive = 0,
        [Description("Aktif"),Generic("icon","fa fa-circle","color","#1ab394")]
        active = 1,
    }

    [EnumInfo(typeof(UT_Template), "type")]
    public enum EnumUT_TemplateType
    {
        [Description("Kaza Olay"),Generic("icon","fa fa-tasks")]
        accident = 0,
        [Description("DÖF"),Generic("icon","fa fa-th-list")]
        dof = 1,
    }

    partial class WorkOfTimeDatabase
    {

        public VWUT_Template GetVWUT_TemplateByExistTitle(string title, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWUT_Template>().Where(a => a.title == title).Execute().FirstOrDefault();
            }
        }

    }
}
