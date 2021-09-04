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


    [EnumInfo(typeof(CRM_Contact), "ContactType")]
    public enum EnumCRM_ContactContactType
    {
        [Description("Yüzyüze")]
        Yuzyuze = 0,
        [Description("Telefon Aracılığı İle")]
        Telefon = 1,
        [Description("Video Konferans")]
        VideoKonferans = 2,
        [Description("Yazılı (Mail,SMS vb.)")]
        Yazili = 3,
        [Description("Yemekte Toplantı")]
        Yemek = 4,
        [Description("Diğer")]
        Diger = 5
    }

    [EnumInfo(typeof(CRM_Contact), "ContactStatus")]
    public enum EnumCRM_ContactContactStatus
    {
        [Description("Planlandı")]
        ToplantiPlanlandi = 1,
        [Description("Gerçekleşti")]
        ToplantiGerceklesti = 0,
        [Description("İptal Edildi")]
        ToplantiIptal = 11,
    }

    public partial class WorkOfTimeDatabase
    {
        public CRM_Contact[] GetCRM_ContactByPresentationIds(Guid[] id)
        {
            using (var db = GetDB())
            {
                return db.Table<CRM_Contact>().Where(a => a.PresentationId.In(id)).Execute().ToArray();

            }
        }

        public CRM_Contact[] GetCRM_ContactByPresentationId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<CRM_Contact>().Where(a => a.PresentationId == id).Execute().ToArray();
            }
        }


        public CRM_Contact GetCRM_ContactLastContact(Guid presentationId)
        {
            using (var db = GetDB())
            {
                return db.Table<CRM_Contact>().Where(a => a.PresentationId == presentationId).OrderByDesc(a => a.created).Skip(0).Take(1).Execute().FirstOrDefault();
            }
        }

    }
}


