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
using Infoline.WorkOfTime.BusinessAccess;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(HR_StaffNeedsStatus), "status")]
    public enum EnumHR_StaffNeedsStatusStatus
    {
        [Description("Talep Gerçekleştirildi"), Generic("icon", "fa-bullhorn", "color", "rgb(248, 172, 89)","desc","Talep Gerçekleştirildi")]
        TalepGerceklestirildi = 0,
        [Description("Yönetici Onayladı"), Generic("icon", "fa-check", "color", "rgb(26, 179, 148)", "desc", "Yönetici Onayladı")]
        YoneticiOnaylandi = 5,
        [Description("Yönetici Reddetti"), Generic("icon", "fa-times", "color", "rgb(237, 85, 101)", "desc", "Yönetici Reddetti")]
        YoneticiReddetti = 10,
        [Description("CV Yönlendirildi"), Generic("icon", "fa-mail-forward", "color", "rgb(28, 132, 198)", "desc", "Cv Yönlendirildi")]
        CvYonlendirildi = 15,
        [Description("Talep Gerçekleştiren Onayladı"), Generic("icon", "fa-check-circle", "color", "rgb(26, 179, 148)", "desc", "Talep Gerçekleştiren CV Onayladı")]
        TalepEdenOnayladi = 16,
        [Description("Talep Gerçekleştiren Reddetti"), Generic("icon", "fa-times-circle", "color", "rgb(237, 85, 101)", "desc", "Talep Gerçekleştiren CV Reddetti")]
        TalepEdenReddetti = 17,
        [Description("Mülakat Ayarlandı"), Generic("icon", "fa-calendar", "color", "rgb(114, 176, 230)", "desc", "Mülakat Ayarlandı")]
        MulakatAyarlandi = 20,
        [Description("Mülakat Gerçekleşti"), Generic("icon", "fa-check-circle", "color", "rgb(26, 179, 148)", "desc", "Mülakat Olumlu Geçti")]
        MulakatGerceklesti = 21,
        [Description("Yeni CV Talep Edildi"), Generic("icon", "fa-newspaper-o", "color", "rgb(202,151,131)", "desc", "Yeni CV Talep Edildi")]
        YeniCvTalepEdildi = 25,
        [Description("Talep İptal Edildi"), Generic("icon", "fa fa-ban", "color", "rgb(255 0 0)", "desc", "Talep İptal Edildi")]
        TalepIptalEdildi = 26,
        [Description("Talebi Sonlandır"), Generic("icon", "fa-check-square-o", "color", "rgb(255, 109, 0)", "desc", "Talep Sonlandırıldı")]
        TalebiSonlandir = 99
    }
    public partial class WorkOfTimeDatabase
    {
        public HR_StaffNeedsStatus[] GetHR_StaffNeedsStatusByStaffNeedId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_StaffNeedsStatus>().Where(a => a.staffNeedId == id).Execute().OrderByDescending(x => x.created).ToArray();
            }
        }
    }
}
