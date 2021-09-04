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

    [EnumInfo(typeof(CMP_Invoice), "orderStatus")]
    public enum EnumCMP_OrderStatus
    {
        [Description("Siparişin Onaylanması Bekleniyor"), Generic("icon", "fa fa-spinner", "color", "rgb(248, 172, 89)")]
        CevapBekleniyor = 0,
        [Description("Sipariş Onaylandı"), Generic("icon", "fa fa-check", "color", "rgb(26, 179, 148)")]
        Onay = 1,
        [Description("Sipariş Süreci Tamamlandı"), Generic("icon", "fa fa-check-square", "color", "rgb(26, 179, 148)")]
        SurecTamamlandi = 2,
        [Description("Sipariş Reddedildi"), Generic("icon", "fa fa-times-circle", "color", "rgb(237, 85, 101)")]
        Red = 3,
        [Description("Siparişin İrsaliyesi Kesildi"), Generic("icon", "fa fa-file", "color", "rgb(35, 198, 200)")]
        IrsaliyeKesildi = 4
    }

    public class SummaryHeadersOrder
    {
        public int AllOrder { get; set; }
        public string AllOrderFilter { get; set; }
        public int WaitingOrder { get; set; }
        public string WaitingOrderFilter { get; set; }
        public int ApprovedOrder { get; set; }
        public string ApprovedOrderFilter { get; set; }
        public int InvoicedOrder { get; set; }
        public string InvoicedOrderFilter { get; set; }
        public int DeclinedOrder { get; set; }
        public string DeclinedOrderFilter { get; set; }

        public string TodayFilter { get; set; }
        public string YesterDayFilter { get; set; }
        public string ThisWeekFilter { get; set; }
        public string LastWeekFilter { get; set; }
        public string ThisMonthFilter { get; set; }
        public string LastMonthFilter { get; set; }


    }

    partial class WorkOfTimeDatabase
    {

        public SummaryHeadersOrder GetVWCMP_OrderByCounts(DbTransaction tran = null)
        {
            var now = DateTime.Now;
            var startOfWeek = now.AddDays(((int)(now.DayOfWeek) * -1) + 1).Date;
            var endOfWeek = startOfWeek.AddDays(7).Date;
            var startOfMonth = new DateTime(now.Year, now.Month, 1).Date;
            var endOfMonth = startOfMonth.AddMonths(1).Date;
            var startOfLastMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-1).Date;


            using (var db = GetDB(tran))
            {
                return new SummaryHeadersOrder
                {
                    AllOrder = db.Table<VWCMP_Order>().Where(a => a.direction == (int)EnumCMP_InvoiceDirectionType.Satis).Count(),
                    AllOrderFilter = "{'Filter':{'Operand1':'direction','Operator':'Equal','Operand2':'1'}",

                    WaitingOrder = db.Table<VWCMP_Order>().Where(a => a.status == (int)EnumCMP_OrderStatus.CevapBekleniyor && a.direction == (int)EnumCMP_InvoiceDirectionType.Satis).Count(),
                    WaitingOrderFilter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",

                    ApprovedOrder = db.Table<VWCMP_Order>().Where(a => a.status == (int)EnumCMP_OrderStatus.Onay && a.direction == (int)EnumCMP_InvoiceDirectionType.Satis).Count(),
                    ApprovedOrderFilter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'1'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",

                    InvoicedOrder = db.Table<VWCMP_Order>().Where(a => a.status == (int)EnumCMP_OrderStatus.SurecTamamlandi && a.direction == (int)EnumCMP_InvoiceDirectionType.Satis).Count(),
                    InvoicedOrderFilter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",

                    DeclinedOrder = db.Table<VWCMP_Order>().Where(a => a.status == (int)EnumCMP_OrderStatus.Red && a.direction == (int)EnumCMP_InvoiceDirectionType.Satis).Count(),
                    DeclinedOrderFilter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'3'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'}}",


                    TodayFilter = "{'Filter':{'Operand1':{'Operand1':'issueDate','Operator':'GreaterThan','Operand2':'" + now.ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':{'Operand1':'issueDate','Operator':'LessThan','Operand2':'" + now.AddDays(1).ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'},'Operator':'And'}}",
                    YesterDayFilter = "{'Filter':{'Operand1':{'Operand1':'issueDate','Operator':'GreaterThan','Operand2':'" + now.AddDays(-1).ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':{'Operand1':'issueDate','Operator':'LessThan','Operand2':'" + (VAL)now.ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'},'Operator':'And'}}",
                    ThisWeekFilter = "{'Filter':{'Operand1':{'Operand1':'issueDate','Operator':'GreaterThan','Operand2':'" + startOfWeek.ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':{'Operand1':'issueDate','Operator':'LessThan','Operand2':'" + endOfWeek.ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'},'Operator':'And'}}",
                    LastWeekFilter = "{'Filter':{'Operand1':{'Operand1':'issueDate','Operator':'GreaterThan','Operand2':'" + startOfWeek.AddDays(-7).ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':{'Operand1':'issueDate','Operator':'LessThan','Operand2':'" + startOfWeek.ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'},'Operator':'And'}}",
                    ThisMonthFilter = "{'Filter':{'Operand1':{'Operand1':'issueDate','Operator':'GreaterThan','Operand2':'" + startOfMonth.ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':{'Operand1':'issueDate','Operator':'LessThan','Operand2':'" + endOfMonth.ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'},'Operator':'And'}}",
                    LastMonthFilter = "{'Filter':{'Operand1':{'Operand1':'issueDate','Operator':'GreaterThan','Operand2':'" + startOfLastMonth.ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':{'Operand1':'issueDate','Operator':'LessThan','Operand2':'" + startOfMonth.ToString("yyyy-MM-dd") + "'},'Operand2':{'Operand1':'direction','Operator':'Equal','Operand2':'1'},'Operator':'And'},'Operator':'And'}}",

                };
            }
        }

    }
}
