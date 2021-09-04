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
    [EnumInfo(typeof(HDM_TicketAction), "type")]
    public enum EnumHDM_TicketActionType
    {
        [Description("Talep Oluşturuldu")]
        YeniTalep = 0,
        [Description("Talep Düzenlendi")]
        TalepDuzenleme = 1,
        [Description("Personel Ataması Yapıldı")]
        PersonelAtama = 2,
        [Description("Mesaj Yazıldı")]
        Mesaj = 3,
        [Description("Başkasına Mail Olarak İletildi")]
        BaskasinaMail = 4,
        [Description("Not Yazıldı")]
        Not = 5,
        [Description("Başka Personel Atandı")]
        BaskaPersonelAta = 6,
        [Description("İşe Başlandı")]
        IsBaslangic = 7,
        [Description("Çözüm Sağlandı")]
        CozumSaglandi = 8,
        [Description("Beklemeye Alındı")]
        Beklemede = 9,
        [Description("İş Kapatıldı")]
        Kapatildi = 10,
        [Description("İş Gereksize Atıldı")]
        Vazgec = 11,
        [Description("Çözüm Onaylandı")]
        Onay = 12,
        [Description("Çözüm Red")]
        Red = 13,
        [Description("Çözüm Değerlendirildi")]
        Degerlendirme = 14
    }


    [EnumInfo(typeof(HDM_TicketAction), "ticketStatus")]
    public enum EnumHDM_TicketActionTicketStatus
    {
        [Description("Açık")]
        Open = 0,
        [Description("Beklemede")]
        Pending = 1,
        [Description("İşlem Yapılıyor")]
        InProgress = 2,
        [Description("Çözüm Sağlandı")]
        Done = 3,
        [Description("Kapatıldı")]
        Closed = 5,
        [Description("Vazgeçildi")]
        Cancelled = 6,
    }
    partial class WorkOfTimeDatabase
    {
        public HDM_TicketAction[] GetHDM_TicketActionByTicketId(Guid ticketId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<HDM_TicketAction>().Where(a => a.ticketId == ticketId).Execute().ToArray();
            }
        }
    }
}
