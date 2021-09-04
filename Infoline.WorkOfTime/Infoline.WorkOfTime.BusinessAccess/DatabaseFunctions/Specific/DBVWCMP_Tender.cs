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
    [EnumInfo(typeof(CMP_Invoice), "tenderStatus")]
    public enum EnumCMP_TenderStatus
    {
        [Description("Yönetici Onayı Bekleniyor"), Generic("icon", "fa fa-spinner", "color", "rgb(248, 172, 89)")]
        CevapBekleniyor = 0,
        [Description("Yonetici Onayladı"), Generic("icon", "fa fa-hand-o-right", "color", "rgb(248, 172, 89)")]
        YoneticiOnay = 1,
        [Description("Teklifi Müşteri Onayladı"), Generic("icon", "fa fa-check", "color", "rgb(26, 179, 148)")]
        MusteriOnay = 2,
        [Description("Teklif Reddedildi"), Generic("icon", "fa fa-times-circle", "color", "rgb(237, 85, 101)")]
        Red = 3,
        [Description("Teklifin Siparişi Girildi"), Generic("icon", "fa fa-shopping-cart", "color", "rgb(28, 132, 198)")]
        TeklifSiparis = 4,
        [Description("Teklifin Faturası Kesildi"), Generic("icon", "fa fa-check-square-o", "color", "rgb(26, 179, 148)")]
        TeklifFatura = 5,
        [Description("Teklifin İrsaliyesi Kesildi"), Generic("icon", "fa fa-send", "color", "rgb(35, 198, 200)")]
        TeklifIrsaliye = 6,
    }

    partial class WorkOfTimeDatabase
    {
        public VWCMP_Tender[] GetVWCMP_TenderByIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Tender>().Where(a => a.id.In(ids)).Execute().ToArray();
            }
        }

        public VWCMP_Tender[] GetVWCMP_TenderByPresentationIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Tender>().Where(a => a.presentationId.In(ids)).Execute().ToArray();
            }
        }

        public VWCMP_Tender GetVWCMP_TenderByPresentationIdLast(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Tender>().Where(a => a.presentationId == id).OrderByDesc(c => c.created).Execute().FirstOrDefault();
            }
        }

    }
}
