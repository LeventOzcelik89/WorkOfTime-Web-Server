using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(CSM_Activity), "type")]
    public enum EnumCSM_ActivityType
    {
        [Description("Fuar")]
        Fuar = 0,
        [Description("Çağrı")]
        Cagri = 1,
        [Description("Yüz Yüze")]
        Yuzyuze = 2,
        [Description("Diğer")]
        Diger = 3,
    }


    partial class WorkOfTimeDatabase
    {
        public CSM_Activity[] GetCSM_ActivityByStudentId(Guid studentId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CSM_Activity>().Where(a => a.studentId == studentId).Execute().ToArray();
            }
        }

        public CSM_Activity[] GetCSM_ActivityByStageId(Guid stageId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CSM_Activity>().Where(a => a.stageId == stageId).Execute().ToArray();
            }
        }

        public CSM_Activity GetCSM_ActivityLastActivity(Guid studentId)
        {
            using (var db = GetDB())
            {
                return db.Table<CSM_Activity>().Where(a => a.studentId == studentId).OrderByDesc(a => a.created).Skip(0).Take(1).Execute().FirstOrDefault();
            }
        }
    }
}
