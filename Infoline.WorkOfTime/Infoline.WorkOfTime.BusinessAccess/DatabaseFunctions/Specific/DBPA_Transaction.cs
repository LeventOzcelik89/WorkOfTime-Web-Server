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
    [EnumInfo(typeof(PA_Transaction), "direction")]
    public enum EnumPA_TransactionDirection
    {
        [Description("Giriş")]
        Giris = 1,
        [Description("Çıkış")]
        Cikis = -1,
        [Description("Talep")]
        Talep = 0
    }

    [EnumInfo(typeof(PA_Transaction), "status")]
    public enum EnumPA_TransactionStatus
    {
        [Description("Ödenecek")]
        Odenecek = 0,
        [Description("Ödendi")]
        Odendi = 1,
        [Description("Çalışan Ödedi")]
        CalisanOdedi = 2
    }

    [EnumInfo(typeof(PA_Transaction), "type")]
    public enum EnumPA_TransactionType
    {
        [Description("Maaş/Prim"), Generic("icon", "icon-money-2")]
        MaasPrim = 0,
        [Description("Çek"), Generic("icon", "icon-pencil-alt")]
        Cek = 1,
        [Description("Sözleşme"), Generic("icon", "icon-edit-1")]
        Sozlesme = 2,
        [Description("Vergi/SGK Prim"), Generic("icon", "icon-cc")]
        Vergi = 3,
        [Description("Fiş/Fatura"), Generic("icon", "icon-doc-text-1")]
        FisFatura = 4,
        [Description("Satış Faturası"), Generic("icon", "icon-doc-text-1")]
        SatisFatura = 5,
        [Description("Alış Faturası"), Generic("icon", "icon-newspaper-1")]
        AlisFatura = 6,
        [Description("Banka Gideri"), Generic("icon", "icon-credit-card-alt")]
        BankaGideri = 7,
        [Description("Personel Masraf"), Generic("icon", "icon-money")]
        Masraf = 8,

        [Description("Alacak Dekontu"), Generic("icon", "icon-money")]
        AlacakDekont = 9,
        [Description("Borç Dekontu"), Generic("icon", "icon-money")]
        BorcDekont = 10,
        [Description("Satış İade Faturası"), Generic("icon", "icon-money")]
        SatisİadeFatura = 11,
        [Description("Senet"), Generic("icon", "icon-money")]
        Senet = 12
    }


    public enum EnumPA_TransactionLedgerExcel
    {
        [Description("Çek Giriş")]
        CekGiris = 10,
        [Description("Çek Çıkış")]
        CekCikis = 11,
        [Description("Alış Faturası")]
        AlisFatura = 12,
        [Description("Satış Faturası")]
        SatisFatura = 13,
        [Description("Alacak Dekontu")]
        AlacakDekont = 14,
        [Description("Borç Dekontu")]
        BorcDekont = 15,
        [Description("Satış İade Faturası")]
        SatisİadeFatura = 16,
        [Description("Alış İade Faturası")]
        AlisİadeFatura = 17,
        [Description("Senet Giriş")]
        SenetGiris = 18,
        [Description("Senet Çıkış")]
        SenetCikis = 19,
        [Description("Tahsil")]
        Tahsil = 20,
        [Description("Ödeme")]
        Odeme = 21
    }

    [EnumInfo(typeof(PA_TransactionConfirmation), "status")]
    public enum EnumPA_TransactionConfirmationStatus
    {
        [Description("Red")]
        Red = 0,
        [Description("Onay")]
        Onay = 1,
        [Description("Yeniden Talep Et")]
        YenidenTalep = 3
    }

    partial class WorkOfTimeDatabase
    {
        public PA_Transaction[] GetPA_TransactionByInvoiceId(Guid invoiceId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Transaction>().Where(a => a.invoiceId == invoiceId).Execute().ToArray();
            }
        }

        public PA_Transaction[] GetPA_TransactionByAccountId(Guid accountId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Transaction>().Where(a => a.accountId == accountId).Execute().ToArray();
            }
        }
        public PA_Transaction[] GetPA_TransactionByTaskId(Guid[] taskId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Transaction>().Where(a => a.dataTable == "FTM_Task" && a.dataId.In(taskId)).Execute().ToArray();
            }
        }
        public PA_Transaction[] GetPA_TransactionByAccountIds(Guid[] accountIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PA_Transaction>().Where(a => a.accountId.In(accountIds)).Execute().ToArray();
            }
        }
    }
}
