using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class SummaryHeadersAdvance
	{
		public int AllMyAdvance { get; set; }
		public string AllMyAdvanceFilter { get; set; }
		public int WaitingMyAdvance { get; set; }
		public string WaitingMyAdvanceFilter { get; set; }
		public int ApprovedMyAdvance { get; set; }
		public string ApprovedMyAdvanceFilter { get; set; }
		public int DeclinedMyAdvance { get; set; }
		public string DeclinedMyAdvanceFilter { get; set; }
		public HeadersAdvance headerFilters { get; set; }
	}

	public class HeadersAdvance
	{
		public string title { get; set; }
		public List<HeadersAdvanceItem> Filters { get; set; }
	}

	public class HeadersAdvanceItem
	{
		public string title { get; set; }
		public int count { get; set; }
		public string filter { get; set; }
		public bool isActive { get; set; }
	}
	partial class WorkOfTimeDatabase
	{
		public VWPA_Advance[] GetVWPA_AdvanceByInvoiceId(Guid invoiceId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))

			{
				return db.Table<VWPA_Advance>().Where(a => a.invoiceId == invoiceId).Execute().ToArray();
			}
		}

		public VWPA_Advance[] GetVWPA_AdvanceByCreatedBy(Guid createdBy, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))

			{
				return db.Table<VWPA_Advance>().Where(a => a.createdby == createdBy).Execute().ToArray();
			}
		}

		public VWPA_Advance[] GetVWPA_AdvanceByIds(Guid[] ids, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))

			{
				return db.Table<VWPA_Advance>().Where(a => a.id.In(ids)).Execute().ToArray();
			}
		}

		public SummaryHeadersAdvance GetVWPA_AdvanceByUserIdCounts(Guid userId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return new SummaryHeadersAdvance
				{
					AllMyAdvance = db.Table<VWPA_Advance>().Where(a => a.createdby == userId).Count(),
					AllMyAdvanceFilter = "{'Filter':{'Operand1':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operand2':{'Operand1':'searchField','Operator':'Like','Operand2':'%#%'},'Operator':'And'}}",

					WaitingMyAdvance = db.Table<VWPA_Advance>().Where(a => a.direction == 0 && a.createdby == userId).Count(),
					WaitingMyAdvanceFilter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",

					ApprovedMyAdvance = db.Table<VWPA_Advance>().Where(a => a.direction == -1 && a.createdby == userId).Count(),
					ApprovedMyAdvanceFilter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'-1'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",

					DeclinedMyAdvance = db.Table<VWPA_Advance>().Where(a => a.direction == 2 && a.createdby == userId).Count(),
					DeclinedMyAdvanceFilter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
				};
			}
		}


		public VWPA_Advance[] GetVWPA_AdvanceMyWaitingApproved(Guid userid, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWPA_Advance>().Where(a => (a.confirmationStatus == 1 || a.confirmationStatus == null) && (a.direction == 0 && a.confirmationUserIds.Contains(userid.ToString()))).Execute().ToArray();
			}
		}

		public SummaryHeadersAdvance GetVWPA_AdvancesByUserId(Guid userId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				var headers = new SummaryHeadersAdvance();
				headers.headerFilters = new HeadersAdvance();
				headers.headerFilters.title = "Durumuna Göre";
				headers.headerFilters.Filters = new List<HeadersAdvanceItem>();
				headers.headerFilters.Filters.Add(new HeadersAdvanceItem
				{
					title = "Onay Bekleyenler",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
					count = db.Table<VWPA_Advance>().Where(a => a.direction == 0 && a.createdby == userId).Count(),
					isActive = true
				});

				headers.headerFilters.Filters.Add(new HeadersAdvanceItem
				{
					title = "Onaylananlar",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'-1'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
					count = db.Table<VWPA_Advance>().Where(a => a.direction == -1 && a.createdby == userId).Count(),
					isActive = true
				});

				headers.headerFilters.Filters.Add(new HeadersAdvanceItem
				{
					title = "Reddedilenler",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
					count = db.Table<VWPA_Advance>().Where(a => a.direction == 2 && a.createdby == userId).Count(),
					isActive = true
				});

				return headers;
			}
		}


		public SummaryHeadersAdvance GetVWPA_AdvancesApprovedByUserId(Guid userId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				var headers = new SummaryHeadersAdvance();
				headers.headerFilters = new HeadersAdvance();
				headers.headerFilters.title = "Durumuna Göre";
				headers.headerFilters.Filters = new List<HeadersAdvanceItem>();
				headers.headerFilters.Filters.Add(new HeadersAdvanceItem
				{
					title = "Onay Bekleyenler",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':'confirmationUserIds','Operator':'Like','Operand2':'" + userId + "'},'Operator':'And'}}",
					count = db.Table<VWPA_Advance>().Where(a => a.direction == 0 && a.confirmationUserIds.Contains(userId.ToString())).Count(),
					isActive = true
				});

				headers.headerFilters.Filters.Add(new HeadersAdvanceItem
				{
					title = "Onaylananlar",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'-1'},'Operand2':{'Operand1':'confirmUserIds','Operator':'Like','Operand2':'" + userId + "'},'Operator':'Or'}}",
					count = db.Table<VWPA_Advance>().Where(a => a.direction == -1 || a.confirmUserIds.Contains(userId.ToString())).Count(),
					isActive = true
				});

				headers.headerFilters.Filters.Add(new HeadersAdvanceItem
				{
					title = "Reddedilenler",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':'rejectedUserIds','Operator':'Like','Operand2':'" + userId + "'},'Operator':'And'}}",
					count = db.Table<VWPA_Advance>().Where(a => a.direction == 2 && a.rejectedUserIds.Contains(userId.ToString())).Count(),
					isActive = true
				});

				return headers;
			}
		}

	}
}
