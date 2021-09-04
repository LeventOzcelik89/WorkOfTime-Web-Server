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
    [EnumInfo(typeof(VWAPM_ActivityOthers), "type")]
    public enum EnumVWAPM_ActivityOthers
    {
        [Description("Doğum Günleri"), Generic("icon", "icon-user", "color","")]
        DogumGunu = 0,
        [Description("İzinler"), Generic("icon", "icon-volume-control-phone", "color", "")]
        Izın = 1,
        [Description("Görevlendirmeler"), Generic("icon", "icon-group-circled", "color", "")]
        Gorevlendirme = 2,
        [Description("Proje Zaman Çizelgesi"), Generic("icon", "icon-megaphone-1", "color", "")]
        ProjeGant = 3,
        [Description("Projeler"), Generic("icon", "icon-users-2", "color", "")]
        Proje = 4,
        [Description("İşe Giriş"), Generic("icon", "icon-videocam-1", "color", "")]
        IsGiris = 5,
        [Description("İşten Ayrılış"), Generic("icon", "icon-bookmark-2", "color", "")]
        IsCikis = 6,
        [Description("Resmi İzinler"), Generic("icon", "icon-bookmark-2", "color", "")]
        Sunum = 7
    }
}
