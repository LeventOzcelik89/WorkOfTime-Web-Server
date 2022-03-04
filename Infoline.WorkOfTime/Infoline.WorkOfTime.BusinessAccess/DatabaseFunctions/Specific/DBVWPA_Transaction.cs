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
	public class SummaryHeadersTransaction
	{
		public int AllMyTransaction { get; set; }
		public string AllMyTransactionFilter { get; set; }
		public int WaitingMyTransaction { get; set; }
		public string WaitingMyTransactionFilter { get; set; }
		public int ApprovedMyTransaction { get; set; }
		public string ApprovedMyTransactionFilter { get; set; }
		public int DeclinedMyTransaction { get; set; }
		public string DeclinedMyTransactionFilter { get; set; }
		public HeadersTransaction headerFilters { get; set; }
	}

	public class HeadersTransaction
	{
		public string title { get; set; }
		public List<HeadersTransactionItem> Filters { get; set; }
	}

	public class HeadersTransactionItem
	{
		public string title { get; set; }
		public int count { get; set; }
		public string filter { get; set; }
		public bool isActive { get; set; }
	}

	partial class WorkOfTimeDatabase
	{
		public VWPA_Transaction[] GetVWPA_TransactionByInvoiceId(Guid invoiceId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))

			{
				return db.Table<VWPA_Transaction>().Where(a => a.invoiceId == invoiceId).Execute().ToArray();
			}
		}

		public VWPA_Transaction[] GetVWPA_TransactionByIds(Guid[] ids, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))

			{
				return db.Table<VWPA_Transaction>().Where(a => a.id.In(ids)).Execute().ToArray();
			}
		}

		public VWPA_Transaction[] GetVWPA_TransactionBydataIds(Guid[] dataIds, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))

			{
				return db.Table<VWPA_Transaction>().Where(a => a.dataId.In(dataIds)).Execute().ToArray();
			}
		}

		public VWPA_Transaction[] GetVWPA_TransactionByDataId(Guid dataId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))

			{
				return db.Table<VWPA_Transaction>().Where(a => a.dataId == dataId).Execute().ToArray();
			}
		}
		public VWPA_Transaction[] GetVWPA_TransactionByConfirmationUserIds(Guid userId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))

			{
				return db.Table<VWPA_Transaction>().Where(a => a.confirmationUserIds.Contains(userId.ToString())).Execute().ToArray();
			}
		}


		public VWPA_Transaction[] GetVWPA_TransactionByCreatedBy(Guid createdBy, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))

			{
				return db.Table<VWPA_Transaction>().Where(a => a.createdby == createdBy).Execute().ToArray();
			}
		}

		public VWPA_Transaction[] GetVWPA_TransactionMyWaitingApproved(Guid userid, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWPA_Transaction>().Where(a => (a.confirmationStatus == 1 || a.confirmationStatus == null) && ((a.direction == 0 && a.type == 8) && a.confirmationUserIds.Contains(userid.ToString()))).Execute().ToArray();
			}
		}

		public SummaryHeadersTransaction GetVWPA_TransactionByUserIdCounts(Guid userId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return new SummaryHeadersTransaction
				{
					AllMyTransaction = db.Table<VWPA_Transaction>().Where(a => (a.type == (int)EnumPA_TransactionType.Masraf) && a.createdby == userId).Count(),
					AllMyTransactionFilter = "{'Filter':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",

					WaitingMyTransaction = db.Table<VWPA_Transaction>().Where(a => (a.direction == 0 && a.type == (int)EnumPA_TransactionType.Masraf) && a.createdby == userId).Count(),
					WaitingMyTransactionFilter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'},'Operator':'And'}}",

					ApprovedMyTransaction = db.Table<VWPA_Transaction>().Where(a => (a.direction == -1 && a.type == (int)EnumPA_TransactionType.Masraf) && a.createdby == userId).Count(),
					ApprovedMyTransactionFilter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'-1'},'Operand2':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'},'Operator':'And'}}",

					DeclinedMyTransaction = db.Table<VWPA_Transaction>().Where(a => (a.direction == 2 && a.type == (int)EnumPA_TransactionType.Masraf) && a.createdby == userId).Count(),
					DeclinedMyTransactionFilter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'},'Operator':'And'}}",
				};
			}
		}

		public SummaryHeadersTransaction GetVWPA_TransactionRequestsCountFilter(Guid userId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return new SummaryHeadersTransaction
				{
					AllMyTransaction = db.Table<VWPA_Transaction>().Where(a => (a.type == (int)EnumPA_TransactionType.Masraf) && a.confirmationUserIds.Contains(userId.ToString())).Count(),
					AllMyTransactionFilter = "{'Filter':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",

					WaitingMyTransaction = db.Table<VWPA_Transaction>().Where(a => (a.confirmationStatus == 1 || a.confirmationStatus == null) && ((a.direction == 0 && a.type == 8) && a.createdby == userId)).Count(),
					WaitingMyTransactionFilter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':{'Operand1':'confirmationStatus','Operator':'Equal','Operand2':1},'Operand2':{'Operand1':'confirmationStatus','Operator':'IsNull','Operand2':null},'Operator':'Or'},'Operand2':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':'type','Operator':'Equal','Operand2':8},'Operator':'And'},'Operator':'And'},'Operand2':{'Operand1':'createdby', 'Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",
					ApprovedMyTransaction = db.Table<VWPA_Transaction>().Where(a => (a.confirmUserIds.Contains(userId.ToString()) || ((a.confirmationStatus == (int)EnumPA_TransactionConfirmationStatus.Onay || a.confirmationStatus == null) && (a.direction == 1 && a.type == 8) && a.createdby == userId))).Count(),
					ApprovedMyTransactionFilter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'-1'},'Operand2':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'},'Operator':'And'}}",

					DeclinedMyTransaction = db.Table<VWPA_Transaction>().Where(a => a.rejectedUserIds.Contains(userId.ToString()) || ((a.confirmationStatus == 1 || a.confirmationStatus == null) && ((a.direction == 2 && a.type == 8) && a.createdby == userId))).Count(),
					DeclinedMyTransactionFilter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'},'Operator':'And'}}",
				};
			}
		}


		public SummaryHeadersTransaction GetVWPA_TransactionsByUserId(Guid userId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				var headers = new SummaryHeadersTransaction();
				headers.headerFilters = new HeadersTransaction();
				headers.headerFilters.title = "Durumuna Göre";
				headers.headerFilters.Filters = new List<HeadersTransactionItem>();
				headers.headerFilters.Filters.Add(new HeadersTransactionItem
				{
					title = "Onay Bekleyenler",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'},'Operator':'And'}}",
					count = db.Table<VWPA_Transaction>().Where(a => (a.direction == 0 && a.type == (int)EnumPA_TransactionType.Masraf) && a.createdby == userId).Count(),
					isActive = true
				});

				headers.headerFilters.Filters.Add(new HeadersTransactionItem
				{
					title = "Onaylananlar",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'-1'},'Operand2':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'},'Operator':'And'}}",
					count = db.Table<VWPA_Transaction>().Where(a => (a.direction == -1 && a.type == (int)EnumPA_TransactionType.Masraf) && a.createdby == userId).Count(),
					isActive = true
				});

				headers.headerFilters.Filters.Add(new HeadersTransactionItem
				{
					title = "Düzenleme Bekleyenler",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'3'},'Operand2':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'},'Operator':'And'}}",
					count = db.Table<VWPA_Transaction>().Where(a => (a.direction == 3 && a.type == (int)EnumPA_TransactionType.Masraf) && a.createdby == userId).Count(),
					isActive = false
				});

				headers.headerFilters.Filters.Add(new HeadersTransactionItem
				{
					title = "Reddedilenler",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'createdby','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'},'Operator':'And'}}",
					count = db.Table<VWPA_Transaction>().Where(a => (a.direction == 2 && a.type == (int)EnumPA_TransactionType.Masraf) && a.createdby == userId).Count(),
					isActive = true
				});

				return headers;
			}
		}


		public SummaryHeadersTransaction GetVWPA_TransactionsMyApprovedByUserId(Guid userId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				var headers = new SummaryHeadersTransaction();
				headers.headerFilters = new HeadersTransaction();
				headers.headerFilters.title = "Durumuna Göre";
				headers.headerFilters.Filters = new List<HeadersTransactionItem>();
				headers.headerFilters.Filters.Add(new HeadersTransactionItem
				{
					title = "Onay Bekleyenler",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'0'},'Operand2':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':{'Operand1':'confirmationUserIds','Operator':'Like','Operand2':'%"+userId.ToString()+"%'},'Operand2':{'Operand1':{'Operand1':'confirmationStatus','Operator':'Equal','Operand2':1},'Operator':'Or','Operand2':{'Operand1':'confirmationStatus','Operator':'IsNull','Operand2':null}},'Operator':'And'},'Operator':'And'},'Operator':'And'}}",
					count = db.Table<VWPA_Transaction>().Where(a => (a.confirmationStatus == 1 || a.confirmationStatus == null) && ((a.direction == 0 && a.type == 8) && a.confirmationUserIds.Contains(userId.ToString()))).Count(),
					isActive = true
				});

				headers.headerFilters.Filters.Add(new HeadersTransactionItem
				{
					title = "Onaylananlar",
					filter = "{'Filter':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'confirmUserIds','Operator':'Like','Operand2':'%" + userId + "%'},'Operator':'And'}}",
					count = db.Table<VWPA_Transaction>().Where(a => (a.confirmUserIds.Contains(userId.ToString()) || ((a.confirmationStatus == (int)EnumPA_TransactionConfirmationStatus.Onay || a.confirmationStatus == null) && (a.direction == 1 && a.type == 8) && a.confirmUserIds.Contains(userId.ToString())))).Count(),
					isActive = true
				});

				headers.headerFilters.Filters.Add(new HeadersTransactionItem
				{
					title = "Reddedilenler",
					filter = "{'Filter':{'Operand1':{'Operand1':'direction','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':{'Operand1':'type','Operator':'Equal','Operand2':'8'},'Operand2':{'Operand1':'rejectedUserIds','Operator':'Like','Operand2':'%" + userId + "%'},'Operator':'And'},'Operator':'And'}}",
					count = db.Table<VWPA_Transaction>().Where(a => a.rejectedUserIds.Contains(userId.ToString()) || ((a.confirmationStatus == 1 || a.confirmationStatus == null) && ((a.direction == 2 && a.type == 8) && a.confirmationUserIds.Contains(userId.ToString())))).Count(),
					isActive = true
				});

				return headers;
			}
		}
	}
}
