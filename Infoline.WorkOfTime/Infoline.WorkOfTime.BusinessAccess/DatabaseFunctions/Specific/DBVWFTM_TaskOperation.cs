using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;
using System.Collections.Generic;

namespace Infoline.WorkOfTime.BusinessAccess
{
	partial class WorkOfTimeDatabase
	{

		public VWFTM_TaskOperation[] GetFTM_TodayReportSolution(int day, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWFTM_TaskOperation>().Where(c => c.status == (int)EnumFTM_TaskOperationStatus.CozumBildirildi &&
				c.created >= DateTime.Now.Date.AddDays(day) && c.created <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)
				).Execute().ToArray();
			}
		}
		public VWFTM_TaskOperation[] GetFTM_TodayAcceptSolutionTaskOperation(int day, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWFTM_TaskOperation>().Where(c => c.status == (int)EnumFTM_TaskOperationStatus.CozumOnaylandi &&
				c.created >= DateTime.Now.Date.AddDays(day) && c.created <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)
				).Execute().ToArray();
			}
		}
		public VWFTM_TaskOperation[] GetFTM_MostAcceptSolutionPersonTaskOperation(DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWFTM_TaskOperation>().Where(c => c.status == (int)EnumFTM_TaskOperationStatus.CozumOnaylandi &&
				c.created >= DateTime.Now.Date.AddDays(-11) && c.created <= DateTime.Now.Date.AddDays(-10).AddSeconds(-1)).Execute().ToArray();
			}
		}

		public VWFTM_TaskOperation[] GetVWFTM_TaskOperationByTaskId(Guid taskId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{

				return db.Table<VWFTM_TaskOperation>().Where(a => a.taskId == taskId).Execute().OrderBy(c => c.created).ToArray();
			}
		}

		public VWFTM_TaskOperation[] GetVWFTM_TaskOperationByTaskIdsAndStatus(Guid[] taskIds, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{

				return db.Table<VWFTM_TaskOperation>().Where(a => a.taskId.In(taskIds) && a.status == 10).Execute().OrderBy(c => c.created).ToArray();
			}
		}

		public VWFTM_TaskOperation[] GetVWFTM_TaskOperationByTaskIds(Guid[] taskIds, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{

				return db.Table<VWFTM_TaskOperation>().Where(a => a.taskId.In(taskIds)).Execute().OrderBy(c => c.created).ToArray();
			}
		}

		public VWFTM_TaskOperation[] GetVWFTM_TaskOperationStatusApprovedByTaskIds(Guid[] taskIds, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{

				return db.Table<VWFTM_TaskOperation>().Where(a => a.taskId.In(taskIds) && a.status == (int)EnumFTM_TaskOperationStatus.CozumOnaylandi).Execute().OrderBy(c => c.created).ToArray();
			}
		}
		public VWFTM_TaskOperation[] GetVWFTM_TaskOperationByCreated(DateTime date, List<Guid?> userIds, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				var startdate = new DateTime();

				var enddate = new DateTime();

				if (date.Month == 1)
				{
					startdate = new DateTime(date.Year, 12, date.Day, 00, 00, 00);
					enddate = new DateTime(date.Year, date.Month + 1, date.Day, 23, 59, 59);
				}
				else if (date.Month == 12)
				{
					startdate = new DateTime(date.Year, date.Month - 1, date.Day, 00, 00, 00);
					enddate = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
				}
				else
				{
					if (date.Day == 31)
					{
						startdate = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
						enddate = new DateTime(date.Year, date.Month + 1, 25, 23, 59, 59);
					}
					else
					{
						startdate = new DateTime(date.Year, date.Month - 1, date.Day, 00, 00, 00);
						enddate = new DateTime(date.Year, date.Month + 1, date.Day, 23, 59, 59);
					}
				}

				if (userIds != null)
				{
					return db.Table<VWFTM_TaskOperation>().Where(a => a.userId.In(userIds.ToArray()) && a.created >= startdate && a.created <= enddate && a.status >= (int)EnumFTM_TaskOperationStatus.GorevBaslandi).Execute().OrderBy(c => c.created).ToArray();
				}

				return db.Table<VWFTM_TaskOperation>().Where(a => a.created >= startdate && a.created <= enddate && a.status >= (int)EnumFTM_TaskOperationStatus.GorevBaslandi).Execute().OrderBy(c => c.created).ToArray();
			}
		}

	}
}
