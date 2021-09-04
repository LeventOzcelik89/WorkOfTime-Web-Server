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
    [EnumInfo(typeof(HDM_Ticket), "status")]
    public enum EnumHDM_TicketStatus
    {
        [Description("Açık"), Generic("icon", "icon-arrows-cw-3", "color", "rgb(248, 172, 89)", "description", "Talep oluşturuldu")]
        Open = 0,
        [Description("Beklemede"), Generic("icon", "icon-clock", "color", "rgb(124, 47, 246)", "description", "Talep beklemeye alındı")]
        Pending = 1,
        [Description("İşlem Yapılıyor"), Generic("icon", "icon-spinner", "color", "rgb(45, 177, 207)", "description", "Talep için çözüm sağlanıyor")]
        InProgress = 2,
        [Description("Çözüm Sağlandı"), Generic("icon", "icon-ok-circle", "color", "rgb(99, 123, 59)", "description", "Talep için çözüm sağlandı")]
        Done = 3,
        [Description("Kapatıldı"), Generic("icon", "icon-check", "color", "rgb(26, 179, 148)", "description", "Talep çözümlenerek kapatıldı")]
        Closed = 5,
        [Description("Vazgeçildi"), Generic("icon", "icon-cancel-circled-1", "color", "rgb(237, 85, 101)", "description", "Talep gereksiz olarak işaretlendi")]
        Cancelled = 6,
    }

    [EnumInfo(typeof(HDM_Ticket), "channel")]
    public enum EnumHDM_TicketChannel
    {
        [Description("Web Site")]
        WebSite = 0,
        [Description("Telefon")]
        Telefon = 1,
        [Description("Eposta")]
        Email = 2,
        [Description("Diğer")]
        Diger = 3,
    }

    [EnumInfo(typeof(HDM_Ticket), "priority")]
    public enum EnumHDM_TicketPriority
    {
        [Description("Yüksek"), Generic("color", "EF5352")]
        Yuksek = 0,
        [Description("Orta"), Generic("color", "F8AC59")]
        Orta = 1,
        [Description("Düşük"), Generic("color", "1ab394")]
        Dusuk = 2,
    }
    partial class WorkOfTimeDatabase
    {
        public HDM_Ticket[] GetHDM_TicketBySuggestionId(Guid suggestionId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<HDM_Ticket>().Where(a => a.suggestionId == suggestionId).Execute().ToArray();
            }
        }
        public HDM_Ticket[] GetHDM_TicketByIssueIds(Guid[] issueIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<HDM_Ticket>().Where(a => a.issueId.In(issueIds)).Execute().ToArray();
            }
        }

        public HDM_Ticket[] GetHDM_TicketBySuggestionIds(Guid[] suggestionIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<HDM_Ticket>().Where(a => a.suggestionId.In(suggestionIds)).Execute().ToArray();
            }
        }
    }
}
