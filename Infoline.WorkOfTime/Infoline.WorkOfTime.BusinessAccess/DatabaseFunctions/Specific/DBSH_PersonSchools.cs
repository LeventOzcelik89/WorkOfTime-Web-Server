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
    [EnumInfo(typeof(SH_PersonSchools), "Level")]
    public enum EnumSH_PersonSchoolsLevel
    {
        [Description("İlk Öğretim")]
        IlkOgretim = 99,
        [Description("Orta Öğretim")]
        OrtaOgretim = 0,
        [Description("Lise")]
        Lise = 1,
        [Description("Ön Lisans")]
        OnLisans = 2,
        [Description("Lisans")]
        Lisans = 3,
        [Description("Yüksek Lisans")]
        YuksekLisans = 4,
        [Description("Doktora")]
        Doktora = 5,
        [Description("Doçent")]
        Docent = 6,
        [Description("Profesör")]
        Prof = 7,
    }

    partial class WorkOfTimeDatabase
    {
        public SH_PersonSchools GetSH_PersonSchoolsByParamsUpdate(Guid id, Guid IdUser, Guid schoolId, string branch)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_PersonSchools>().Where(a => a.id != id && a.UserId == IdUser && a.SchoolId == schoolId && a.Branch == branch).Execute().FirstOrDefault();
            }
        }

        public SH_PersonSchools GetSH_PersonSchoolsByInsert(Guid schoolId, Guid IdUser,string branch)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_PersonSchools>().Where(a => a.SchoolId == schoolId && a.Branch == branch && a.UserId == IdUser).Execute().FirstOrDefault();
            }
        }



    }


}
