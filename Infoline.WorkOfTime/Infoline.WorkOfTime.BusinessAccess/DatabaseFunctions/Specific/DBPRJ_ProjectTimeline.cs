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
    [EnumInfo(typeof(PRJ_ProjectTimeline), "Type")]
    public enum EnumPRJ_ProjectTimelineType
    {
        [Description("İş Paketi")]
        IsPaketi = 0,
        [Description("Hakediş")]
        Hakedis = 1,
        [Description("Satın Alma")]
        SatinAlma = 2,
        [Description("Bakım")]
        Bakım = 3,
        [Description("IT Talep")]
        ITTalep = 4,
        [Description("Oys Takip")]
        OysTakip = 5,
        [Description("Yapılandırma")]
        Yapilandirma = 6,
        [Description("İmza Onay")]
        ImzaOnay = 7,
        [Description("Ek Geliştirme")]
        EkGelistirme = 8,
        [Description("3C Onayı")]
        C3Onay = 9,
        [Description("Analiz")]
        Analiz = 10,
        [Description("Bilgilendirme")]
        Bilgilendirme = 11
    }

    [EnumInfo(typeof(PRJ_ProjectTimeline), "Status")]
    public enum EnumPRJ_ProjectTimelineStatus
    {
        [Description("Başlamadı"), Generic("color", "#ff492c")]
        Baslamadi = 0,
        [Description("Başlandı"), Generic("color", "#409bf9")]
        Baslandi = 1,
        [Description("Beklemede"), Generic("color", "#ffb020")]
        Beklemede = 2,
        [Description("Bitti"), Generic("color", "#11b51d")]
        Bitti = 3
    }

    partial class WorkOfTimeDatabase
    {

        public PRJ_ProjectTimeline[] GetPRJ_ProjectTimelineScheduled()
        {
            using (var db = GetDB())
            {
                return
                    db.ExecuteReader<PRJ_ProjectTimeline>("SELECT id,EndDate FROM [PRJ_ProjectTimeline]  with(nolock) where EndDate > GETDATE()").ToArray();
            }
        }


        public PRJ_ProjectTimeline[] GetPRJ_ProjectTimelineScheduledControl()
        {
            using (var db = GetDB())
            {
                return
                    db.ExecuteReader<PRJ_ProjectTimeline>("SELECT id,EndDate FROM [PRJ_ProjectTimeline]  with(nolock) where EndDate < GETDATE()").ToArray();
            }
        }


        public PRJ_ProjectTimeline[] GetPRJ_ProjectTimelineByIdProject(Guid ProjectId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRJ_ProjectTimeline>().Where(a => a.IdProject == ProjectId).Execute<PRJ_ProjectTimeline>().ToArray();
            }
        }





        public PRJ_ProjectTimelinePersons[] GetPRJ_ProjectTimelinePersonTimelineId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<PRJ_ProjectTimelinePersons>().Where(x => x.IdTimeline == id).Execute().ToArray();

            }
        }
    }
}
