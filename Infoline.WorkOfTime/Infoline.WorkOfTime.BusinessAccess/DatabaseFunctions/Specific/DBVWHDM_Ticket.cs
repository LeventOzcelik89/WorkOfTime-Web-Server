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

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class SummaryHeadersTicket
    {
        public int AllMyTicket { get; set; }
        public string AllMyTicketFilter { get; set; }
        public int WaitingMyTicket { get; set; }
        public string WaitingMyTicketFilter { get; set; }
        public int ApprovedMyTicket { get; set; }
        public string ApprovedMyTicketFilter { get; set; }
        public int DeclinedMyTicket { get; set; }
        public string DeclinedMyTicketFilter { get; set; }
        public HeadersTicket headerFilters { get; set; }
    }

    public class HeadersTicket
    {
        public string title { get; set; }
        public List<HeadersTicketItem> Filters { get; set; }
    }

    public class HeadersTicketItem
    {
        public string title { get; set; }
        public int count { get; set; }
        public string filter { get; set; }
        public bool isActive { get; set; }
    }

    partial class WorkOfTimeDatabase
    {
        public VWHDM_Ticket[] GetVWHDM_TicketByTicketId(Guid ticketId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWHDM_Ticket>().Where(a => a.id == ticketId).Execute().ToArray();
            }
        }

        public VWHDM_Ticket[] GetDBVWHDM_TicketMyWaiting(Guid userid, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWHDM_Ticket>().Where(a => (a.status == 0 && a.requesterId == userid)).Execute().ToArray();
            }
        }

        public VWHDM_Ticket[] GetDBVWHDM_TicketMyManagerWaiting(Guid userid, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWHDM_Ticket>().Where(a => (a.status == 0 && a.assignUserId == userid)).Execute().ToArray();
            }
        }

        public VWHDM_Ticket[] GetVWHDM_TicketSpesific(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWHDM_Ticket>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }

        public SummaryHeadersTicket GetDBVWHDM_GetMyTicketSummary(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersTicket();
                headers.headerFilters = new HeadersTicket();
                headers.headerFilters.title = "Durumuna Göre";
                headers.headerFilters.Filters = new List<HeadersTicketItem>();
                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Açık",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':'requesterId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 0 && a.requesterId == userId).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Beklemede",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'1'},'Operand2':{'Operand1':'requesterId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 1 && a.requesterId == userId).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "İşlem Yapılıyor",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':'requesterId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 2 && a.requesterId == userId).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Çözüm Sağlandı",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'3'},'Operand2':{'Operand1':'requesterId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 3 && a.requesterId == userId).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Kapatıldı",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'5'},'Operand2':{'Operand1':'requesterId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 5 && a.requesterId == userId).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Vazgeçildi",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'6'},'Operand2':{'Operand1':'requesterId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 6 && a.requesterId == userId).Count(),
                    isActive = false
                });

                return headers;
            }
        }

        public SummaryHeadersTicket GetDBVWHDM_GetMyMasterManagerTicketSummary(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersTicket();
                headers.headerFilters = new HeadersTicket();
                headers.headerFilters.title = "Durumuna Göre";
                headers.headerFilters.Filters = new List<HeadersTicketItem>();
                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Açık",
                    filter = "{'Filter':{'Operand1':'status','Operator':'Equal','Operand2':0}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 0).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Beklemede",
                    filter = "{'Filter':{'Operand1':'status','Operator':'Equal','Operand2':1}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 1).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "İşlem Yapılıyor",
                    filter = "{'Filter':{'Operand1':'status','Operator':'Equal','Operand2':2}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 2).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Çözüm Sağlandı",
                    filter = "{'Filter':{'Operand1':'status','Operator':'Equal','Operand2':3}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 3).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Kapatıldı",
                    filter = "{'Filter':{'Operand1':'status','Operator':'Equal','Operand2':5}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 5).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Vazgeçildi",
                    filter = "{'Filter':{'Operand1':'status','Operator':'Equal','Operand2':6}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 6).Count(),
                    isActive = false
                });

                return headers;
            }
        }
        public SummaryHeadersTicket GetDBVWHDM_GetMyManagerTicketSummary(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var headers = new SummaryHeadersTicket();
                headers.headerFilters = new HeadersTicket();
                headers.headerFilters.title = "Durumuna Göre";
                headers.headerFilters.Filters = new List<HeadersTicketItem>();
                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Açık",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 0 && a.assignUserId == userId).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Beklemede",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'1'},'Operand2':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 1 && a.assignUserId == userId).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "İşlem Yapılıyor",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 2 && a.assignUserId == userId).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Çözüm Sağlandı",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'3'},'Operand2':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 3 && a.assignUserId == userId).Count(),
                    isActive = true
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Kapatıldı",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'5'},'Operand2':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 5 && a.assignUserId == userId).Count(),
                    isActive = false
                });

                headers.headerFilters.Filters.Add(new HeadersTicketItem
                {
                    title = "Vazgeçildi",
                    filter = "{'Filter':{'Operand1':{'Operand1':'status','Operator':'Equal','Operand2':'6'},'Operand2':{'Operand1':'assignUserId','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
                    count = db.Table<VWHDM_Ticket>().Where(a => a.status == 6 && a.assignUserId == userId).Count(),
                    isActive = false
                });

                return headers;
            }
        }
    }
}
