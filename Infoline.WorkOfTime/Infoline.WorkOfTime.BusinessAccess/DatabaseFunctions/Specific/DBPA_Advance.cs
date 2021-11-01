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
    [EnumInfo(typeof(PA_Advance), "direction")]
    public enum EnumPA_AdvanceDirection
    {
        [Description("Giriş")]
        Giris = 1,
        [Description("Çıkış")]
        Cikis = -1,
        [Description("Talep")]
        Talep = 0,
    }

    [EnumInfo(typeof(PA_Advance), "status")]
    public enum EnumPA_AdvanceStatus
    {
        [Description("Ödenecek")]
        Odenecek = 0,
        [Description("Ödendi")]
        Odendi = 1,
        [Description("Çalışan Ödedi")]
        CalisanOdedi = 2,
    }

    [EnumInfo(typeof(PA_Advance), "type")]
    public enum EnumPA_AdvanceType
    {
        [Description("İş Avansı"), Generic("icon", "fa fa-briefcase")]
        IsAvansi = 0,
        [Description("Maaş Avansı"), Generic("icon", "icon-money")]
        MaasAvansi = 1,
       
    }

    [EnumInfo(typeof(PA_AdvanceConfirmation), "status")]
    public enum EnumPA_AdvanceConfirmationStatus
    {
        [Description("Red")]
        Red = 0,
        [Description("Onay")]
        Onay = 1,
        [Description("Yeniden Talep")]
        YenidenTalep = 3
    }

    partial class WorkOfTimeDatabase
    {
        public PA_Advance[] GetPA_AdvanceByInvoiceId(Guid invoiceId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Advance>().Where(a => a.invoiceId == invoiceId).Execute().ToArray();
            }
        }

        public PA_Advance[] GetPA_AdvanceByAccountId(Guid accountId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Advance>().Where(a => a.accountId == accountId).Execute().ToArray();
            }
        }

        public PA_Advance[] GetPA_AdvanceByAccountIds(Guid[] accountIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Advance>().Where(a => a.accountId.In(accountIds)).Execute().ToArray();
            }
        }
    }
}
