using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(APM_Activity), "generalType")]
    public enum EnumAPM_ActivityGeneralType
    {
        [Description("Şirket İçi")]
        Sirket = 0,
        [Description("Satış")]
        Satis = 1
    }

    [EnumInfo(typeof(APM_Activity), "type")]
    public enum EnumAPM_ActivityType
    {
        [Description("Toplantı"), Generic("icon", "icon-group-circled","color","rgb(0,0,0)")]
        Toplanti = 1,
        [Description("Sunum"), Generic("icon", "icon-bookmark-2", "color", "rgb(0,0,0)")]
        Sunum = 2,
        [Description("Etkinlik"), Generic("icon", "icon-megaphone-1", "color", "rgb(0,0,0)")]
        Etkinlik = 3,
        [Description("Duyuru"), Generic("icon", "icon-volume-up", "color", "rgb(0,0,0)")]
        Duyuru = 4,
        [Description("Not"), Generic("icon", "icon-book", "color", "rgb(0,0,0)")]
        Not = 5,
        [Description("Hatırlatma"), Generic("icon", "icon-bell-2", "color", "rgb(0,0,0)")]
        Hatırlatma = 6,
        [Description("Diğer"), Generic("icon", "icon-hashtag", "color", "rgb(0,0,0)")]
        Diger = 10,
    }

    [EnumInfo(typeof(APM_Activity), "communicationType")]
    public enum EnumAPM_ActivityCommunicationType
    {
        [Description("Telefon"), Generic("icon", "icon-volume-control-phone")]
        Cagri = 1,
        [Description("Yüz Yüze"), Generic("icon", "icon-map")]
        Yuzyuze = 2,
        [Description("Video Konferans"), Generic("icon", "icon-videocam-1")]
        Video = 3,
        [Description("Diğer"), Generic("icon", "icon-hashtag")]
        Diger = 99,
    }

    [EnumInfo(typeof(APM_Activity), "notification")]
    public enum EnumAPM_ActivityNotification
    {
        [Description("İstemiyorum")]
        istemiyorum = 0,
        [Description("E-Posta")]
        eposta = 1,
        [Description("Mobil Uygulaması")]
        mobil = 2,
        [Description("Web Uygulaması")]
        web = 3,
    }

    partial class WorkOfTimeDatabase
    {

    }
}
