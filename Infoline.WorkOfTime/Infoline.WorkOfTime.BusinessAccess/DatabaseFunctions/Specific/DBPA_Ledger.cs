using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(PA_Ledger), "type")]
    public enum EnumPA_LedgerType
    {
        [Description("Ödeme"), Generic("icon", "icon-upload-4")]
        Odeme = 0,
        [Description("Tahsilat"), Generic("icon", "icon-download-5")]
        Tahsilat = 1,
        [Description("Çalışana Ödeme"), Generic("icon", "icon-user-add")]
        CalisanaOdeme = 2,
        [Description("Para Girişi"), Generic("icon", "icon-login")]
        ParaGiris = 3,
        [Description("Para Çıkışı"), Generic("icon", "icon-logout-1")]
        ParaCikis = 4,
        [Description("Açılış Bakiyesi"), Generic("icon", "icon-bell")]
        AcilisBakiye = 5,
        [Description("Bakiye Sabitleme"), Generic("icon", "icon-pin-1")]
        BakiyeSabitleme = 6,
        [Description("Hesaplarım Arası Transfer"), Generic("icon", "icon-arrows-cw")]
        HesabaTransfer = 7,
        [Description("Personele Ödeme"), Generic("icon", "icon-user-add-1")]
        PersoneleOdeme = 8,
    }

    partial class WorkOfTimeDatabase
    {
        public PA_Ledger[] GetPA_LedgerByAccountId(Guid accountId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Ledger>().Where(a => a.accountId == accountId).Execute().ToArray();
            }
        }

        public PA_Ledger[] GetPA_LedgerByTransactionId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PA_Ledger>().Where(a => a.transactionId == id).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public PA_Ledger[] GetPA_LedgerByAdvanceId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PA_Ledger>().Where(a => a.advanceId == id).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public PA_Ledger[] GetPA_LedgerByAccountIds(Guid[] accountIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Ledger>().Where(a => a.accountId.In(accountIds)).Execute().ToArray();
            }
        }


        public PA_Ledger[] GetPA_LedgerByRelatedAccountIds(Guid[] accountIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Ledger>().Where(a => a.accountRealtedId.In(accountIds)).Execute().ToArray();
            }
        }

        public PA_Ledger[] GetPA_LedgerByTransactionIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Ledger>().Where(a => a.transactionId.In(ids)).Execute().ToArray();
            }
        }
    }
}
