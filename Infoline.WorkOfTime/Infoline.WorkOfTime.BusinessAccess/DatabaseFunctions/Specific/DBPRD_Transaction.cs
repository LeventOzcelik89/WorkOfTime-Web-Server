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

    [BusinessAccess.EnumInfo(typeof(PRD_Transaction), "status")]
    public enum EnumPRD_TransactionStatus
    {
        [Description("Onay Bekleniyor"), Generic("icon", "icon-spinner", "color", "warning")]
        beklemede = 0,
        [Description("Stoklara İşlendi"), Generic("icon", "icon-ok-circle-1", "color", "primary")]
        islendi = 1,
    }

    [BusinessAccess.EnumInfo(typeof(PRD_Transaction), "type")]
    public enum EnumPRD_TransactionType
    {
        //Giriş İşlemi İşlemleri
        [Description("Gelen İrsaliye"), Generic("icon", "icon-level-up-1")]
        GelenIrsaliye = 0,
        [Description("Zimmet İade Fişi"), Generic("icon", "icon-user-add-1")]
        ZimmetAlma = 1,
        [Description("Kiralama Fişi"), Generic("icon", "icon-loop-alt")]
        Kiralama = 2,
        [Description("Açılış Fişi"), Generic("icon", "icon-upload")]
        AcilisFisi = 3,
        [Description("Üretim Fişi"), Generic("icon", "icon-wpforms")]
        UretimFisi = 4,
        [Description("Sayım Fazlası"), Generic("icon", "icon-plus-squared")]
        SayimFazlasi = 5,

        //Çıkış İşlemleri
        [Description("Giden İrsaliye"), Generic("icon", "icon-level-down-1")]
        GidenIrsaliye = 10,
        [Description("Zimmet Verme Fişi"), Generic("icon", "icon-user-delete")]
        ZimmetVerme = 11,
        [Description("Kiraya Verme Fişi"), Generic("icon", "icon-loop-1")]
        KiralayaVerme = 12,
        [Description("Sarf Fişi"), Generic("icon", "icon-download")]
        SarfFisi = 13,
        [Description("Fire Fişi"), Generic("icon", "icon-paste")]
        FireFisi = 14,
        [Description("Sayım Eksiği"), Generic("icon", "icon-minus-squared")]
        SayimEksigi = 15,


        //Giriş Çıkış İşlemleri
        [Description("Transfer Fişi"), Generic("icon", "icon-updown-circle")]
        Transfer = 99,
        [Description("Harcama Bildirimi"), Generic("icon", "ii-sermayeuretim")]
        HarcamaBildirimi = 100,
        [Description("Uretim Bildirimi"), Generic("icon", "ii-isletme-envanteri")]
        UretimBildirimi = 101,
        [Description("Cihaz Değişimi"), Generic("icon", "ii-isletme-envanteri")]
        CihazDegisimi = 102,

    }

    partial class WorkOfTimeDatabase
    {
        public PRD_Transaction[] GetPRD_TransactionByInvoiceId(Guid invoiceId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_Transaction>().Where(a => a.invoiceId == invoiceId).Execute().ToArray();
            }
        }

        public PRD_Transaction[] GetPRD_TransactionByDataId(Guid dataId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_Transaction>().Where(a => a.outputCompanyId == dataId || a.inputCompanyId == dataId).Execute().ToArray();
            }
        }


    }
}
