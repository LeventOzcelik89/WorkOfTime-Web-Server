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
    public class SummaryHeadersTender
    {
        public int AllMyTender { get; set; }
        public string AllMyTenderFilter { get; set; }
        public int WaitingMyTender { get; set; }
        public string WaitingMyTenderFilter { get; set; }
        public int ApprovedMyTender { get; set; }
        public string ApprovedMyTenderFilter { get; set; }
        public int DeclinedMyTender { get; set; }
        public string DeclinedMyTenderFilter { get; set; }
        public HeadersTender headerFilters { get; set; }
    }
    public class HeadersTender
    {
        public string title { get; set; }
        public List<HeadersTenderItem> Filters { get; set; }
    }
    public class HeadersTenderItem
    {
        public string title { get; set; }
        public int count { get; set; }
        public string filter { get; set; }
        public bool isActive { get; set; }
    }

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

        public SummaryHeadersTender GetDBVWCMP_GetMyTenderSummary(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersTender();
                headers.headerFilters = new HeadersTender();
                headers.headerFilters.title = "Teklif Sürecine Göre";
                headers.headerFilters.Filters = new List<HeadersTenderItem>();
                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Tüm Teklifler",
                    filter = "{'Filter':{'Operand1':'direction','Operator':'Equal','Operand2':1}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.direction == 1).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Yönetici Onayı Beklenenler",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 1 && a.direction == 1).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Müşteri Onayı Beklenenler",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'1'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 1 && a.direction == 1).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Müşterinin Onayladığı Teklifler",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 2 && a.direction == 1).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Siparişi Girilen Teklifler",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'4'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 4 && a.direction == 1).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Faturası Kesilen Teklifler",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'5'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 5 && a.direction == 1).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Reddedilen Teklifler",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'3'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 3 && a.direction == 1).Count(),
                    isActive = false
                });

                return headers;
            }
        }

        public SummaryHeadersTender GetDBVWCMP_GetMyTenderAccountingSummary(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersTender();
                headers.headerFilters = new HeadersTender();
                headers.headerFilters.title = "Teklif Sürecine Göre";
                headers.headerFilters.Filters = new List<HeadersTenderItem>();
                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "İşlem Yapılacaklar",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'1'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.direction == 1 && a.status == 1).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Faturası Alınanlar",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'5'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 5 && a.direction == 1).Count(),
                    isActive = true
                });
                return headers;
            }
        }

        public SummaryHeadersTender GetDBVWCMP_GetMyTenderApproveSummary(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersTender();
                headers.headerFilters = new HeadersTender();
                headers.headerFilters.title = "Teklif Sürecine Göre";
                headers.headerFilters.Filters = new List<HeadersTenderItem>();
                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Tüm Tekliflerim",
                    filter = "{'Filter':{'Operand1':{'Operand1':'createdby','Operator': 'Equal','Operand2':'" + userId + "' },'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.direction == 1 && a.createdby == userId).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Yönetici Onayı Beklenenler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'createdby','Operand2':'" + userId + "','Operator':'Equal'},'Operand2':{'Operand1':'status','Operand2':'0','Operator':'Equal'},'Operator':'And'},'Operand2':{'Operand1':'direction','Operand2':'1','Operator':'Equal'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 0 && a.direction == 1 && a.changedby == userId).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Müşteri Onayı Beklenenler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'createdby','Operand2':'" + userId + "','Operator':'Equal'},'Operand2':{'Operand1':'status','Operand2':'1','Operator':'Equal'},'Operator':'And'},'Operand2':{'Operand1':'direction','Operand2':'1','Operator':'Equal'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 1 && a.direction == 1).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Müşterinin Onayladıkları",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'createdby','Operand2':'" + userId + "','Operator':'Equal'},'Operand2':{'Operand1':'status','Operand2':'2','Operator':'Equal'},'Operator':'And'},'Operand2':{'Operand1':'direction','Operand2':'1','Operator':'Equal'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 2 && a.direction == 1 && a.createdby == userId).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Siparişi Girilen Teklifler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'createdby','Operand2':'" + userId + "','Operator':'Equal'},'Operand2':{'Operand1':'status','Operand2':'4','Operator':'Equal'},'Operator':'And'},'Operand2':{'Operand1':'direction','Operand2':'1','Operator':'Equal'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 4 && a.direction == 1 && a.createdby == userId).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Faturası Kesilen Teklifler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'createdby','Operand2':'" + userId + "','Operator':'Equal'},'Operand2':{'Operand1':'status','Operand2':'5','Operator':'Equal'},'Operator':'And'},'Operand2':{'Operand1':'direction','Operand2':'1','Operator':'Equal'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 5 && a.direction == 1 && a.createdby == userId).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTenderItem
                {
                    title = "Reddedilen Teklifler",
                    filter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'createdby','Operand2':'" + userId + "','Operator':'Equal'},'Operand2':{'Operand1':'status','Operand2':'3','Operator':'Equal'},'Operator':'And'},'Operand2':{'Operand1':'direction','Operand2':'1','Operator':'Equal'},'Operator':'And'}}",
                    count = db.Table<VWCMP_Tender>().Where(a => a.status == 3 && a.direction == 1 && a.createdby == userId).Count(),
                    isActive = false
                });

                return headers;
            }
        }

        public VWCMP_Tender GetVWCMP_TenderByPid(Guid pid, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWCMP_Tender>().Where(a => a.pid == pid).Execute().FirstOrDefault();
            }
        }

    }
}
