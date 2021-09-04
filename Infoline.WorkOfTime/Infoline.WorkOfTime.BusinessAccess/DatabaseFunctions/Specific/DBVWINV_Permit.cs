using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel;
using System.Collections.Generic;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.BusinessAccess
{
	partial class WorkOfTimeDatabase
	{

		public VWINV_Permit[] GetVWINV_PermitPending(Guid IdUser, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWINV_Permit>().Where(a =>
				(a.Manager1Approval == IdUser && a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.TalepEdildi)) ||
				(a.Manager2Approval == IdUser && a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.Yonetici1Onay)) ||
				(a.IkApproval == IdUser && a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.Yonetici2Onay))).Execute().ToArray();
			}
		}


		public VWINV_Permit[] GetVWINV_PermitSinePending(Guid IdUser, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWINV_Permit>().Where(a => a.IdUser == IdUser && a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol).Execute().ToArray();
			}
		}

		public VWINV_Permit[] GetVWINV_PermitPersonToday(int day, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWINV_Permit>().Where(a => a.StartDate != null && a.EndDate != null
				&& (a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi || a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol)
				).Execute().Where(x => x.StartDate.Value.Date <= DateTime.Now.Date.AddDays(day) && x.EndDate.Value.Date >= DateTime.Now.Date).ToArray();

			}
		}

		public VWINV_Permit[] GetVWINV_PermitByCalendar(DateTime start, DateTime end)
		{
			using (var db = GetDB())
			{
				return db.Table<VWINV_Permit>().Where(x => (x.ApproveStatus == (int)EnumINV_PermitApproveStatus.GecmisIzin ||
														   x.ApproveStatus == (int)EnumINV_PermitApproveStatus.IkKontrol ||
														   x.ApproveStatus == (int)EnumINV_PermitApproveStatus.IslakImzaYuklendi) && x.StartDate >= start && x.EndDate <= end).Execute().ToArray();
			}
		}

		public VWINV_Permit[] GetVWINV_PermitByDateRange(DateTime start, DateTime end)
		{
			using (var db = GetDB())
			{
				return db.Table<VWINV_Permit>().Where(x => x.StartDate >= start && x.EndDate <= end).Execute().ToArray();
			}
		}

		public VWINV_Permit[] GetVWINV_PermitByApproveStatusHealtyPermit(DateTime start, DateTime end)
		{
			using (var db = GetDB())
			{
				return db.Table<VWINV_Permit>().Where(x => x.ApproveStatus == (int)EnumINV_PermitApproveStatus.SaglikRaporu && x.StartDate >= start && x.EndDate <= end).Execute().ToArray();
			}
		}

		public VWINV_Permit[] GetVWINV_PermitRequestOfferByToday(int day, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWINV_Permit>().Where(a => a.created >= DateTime.Now.Date.AddDays(day) && a.created <= DateTime.Now.Date.AddDays(1).AddSeconds(-1)).Execute().ToArray();
			}
		}

		public VWINV_Permit[] GetVWINV_PermitUserAvailableStatus(Guid userid, DbTransaction tran = null)
		{
			var today = DateTime.Now.AddDays(-1);
			var month = today.AddDays(30);

			using (var db = GetDB(tran))
			{
				return db.Table<VWINV_Permit>().Where(a => a.IdUser == userid && a.StartDate >= today && a.EndDate <= month).Execute().ToArray();
			}
		}

		public VWINV_Permit[] GetVWINV_PermitStandByByIdUser(Guid userId)
		{
			using (var db = GetDB())
			{
				return db.Table<VWINV_Permit>().Where(a => a.IdUser == userId && (a.ApproveStatus == (int)EnumINV_PermitApproveStatus.TalepEdildi || a.ApproveStatus == (int)EnumINV_PermitApproveStatus.Yonetici2Onay || a.ApproveStatus == (int)EnumINV_PermitApproveStatus.Yonetici1Onay)).Execute().ToArray();
			}
		}



		public VWINV_Permit[] GetVWINV_PermitByIdUser(Guid userId)
		{
			using (var db = GetDB())
			{
				return db.Table<VWINV_Permit>().Where(a => a.IdUser == userId).Execute().ToArray();
			}
		}

		public VWINV_Permit[] GetVWINV_PermitByDateAndUserIds(int year, List<Guid?> userIds)
		{
			using (var db = GetDB())
			{
				var startDate = new DateTime(year, 1, 1, 0, 0, 0);
				var endDate = new DateTime(year, 12, 29, 0, 0, 0);

				if (userIds != null && userIds.Count(x => x.HasValue) > 0)
				{
					return db.Table<VWINV_Permit>().Where(a => a.IdUser.In(userIds.ToArray()) && (a.StartDate >= startDate && a.EndDate <= endDate)).Execute().ToArray();
				}

				return db.Table<VWINV_Permit>().Where(a => a.StartDate >= startDate && a.EndDate <= endDate).Execute().ToArray();
			}
		}


		public SummaryHeadersPermit GetVWINV_PermitByIdUserCounts(Guid userId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return new SummaryHeadersPermit
				{
					waiting = db.Table<VWINV_Permit>().Where((c => (c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.TalepEdildi ||
																   c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Onay ||
																   c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay ||
																   c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol) && c.IdUser == userId)).Count(),
					waitingFilter = "{'Filter':{'Operand1': {'Operand1' : {'Operand1' : {'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'0'},'Operand2' : {'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'1'},'Operator' : 'Or'},'Operand2' : {'Operand1' : {'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'3'},'Operand2' : {'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'5'},'Operator' : 'Or'},'Operator' : 'Or'},'Operand2' : { 'Operand1':'IdUser','Operator':'Equal','Operand2':'" + userId + "'} , 'Operator' : 'And'}}",


					approved = db.Table<VWINV_Permit>().Where((c => (c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi ||
																	c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.GecmisIzin ||
																	c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.SaglikRaporu ||
																	c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.AvansIzin) && c.IdUser == userId)).Count(),
					approvedFilter = "{'Filter':{'Operand1': {'Operand1' : {'Operand1' : {'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'7'},'Operand2' : {'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'8'},'Operator' : 'Or'},'Operand2' : {'Operand1' : {'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'9'},'Operand2' : {'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'10'},'Operator' : 'Or'},'Operator' : 'Or'},'Operand2' : { 'Operand1':'IdUser','Operator':'Equal','Operand2':'" + userId + "'} , 'Operator' : 'And'}}",

					rejected = db.Table<VWINV_Permit>().Where((c => (c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrolRed ||
																	 c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Red ||
																	 c.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Red) && c.IdUser == userId)).Count(),

					rejectedFilter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'2'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'6'},'Operator':'Or'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'4'},'Operator':'Or'},'Operand2':{'Operand1':'IdUser','Operator':'Equal','Operand2':'" + userId + "'},'Operator':'And'}}",

					searchField = "{'Filter':{'Operand1':{'Operand1':'IdUser','Operator':'Equal','Operand2':'" + userId + "'},'Operand2':{'Operand1':'searchField','Operator':'Like','Operand2':'%#%'},'Operator':'And'}}"

				};
			}
		}

		public SummaryHeadersPermit GetVWINV_PermitAboutByIdUserCounts(Guid userid, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return new SummaryHeadersPermit
				{
					waiting = db.Table<VWINV_Permit>().Where(a =>
				 (a.Manager1Approval == userid && a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.TalepEdildi) ||
				 (a.Manager2Approval == userid && a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Onay) ||
				 (a.IkApproval == userid && a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay)).Count(),

					waitingFilter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':{'Operand1':'Manager1Approval','Operator':'Equal','Operand2':'" + userid + "'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'0'},'Operator':'And'},'Operand2':{'Operand1':{'Operand1':'Manager2Approval','Operator':'Equal','Operand2':'" + userid + "'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'1'},'Operator':'And'},'Operator':'Or'},'Operand2':{'Operand1':{'Operand1':'IkApproval','Operator':'Equal','Operand2':'" + userid + "'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'3'},'Operator':'And'},'Operator':'Or'}}",


					approved = db.Table<VWINV_Permit>().Where(a =>
				 (a.Manager1Approval == userid &&
				 (a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.Yonetici1Onay) ||
				 a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay ||
				 a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi ||
				 a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol)) ||
				 (a.Manager2Approval == userid &&
				 (a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.Yonetici2Onay) ||
				 a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi ||
				 a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol)) ||
				 (a.IkApproval == userid &&
				 (a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol ||
				 a.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi))).Count(),

					approvedFilter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':{'Operand1':{'Operand1':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'1'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'3'},'Operator':'Or'},'Operand2':{'Operand1':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'5'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'7'},'Operator':'Or'},'Operator':'Or'},'Operand2':{'Operand1':'Manager1Approval','Operator':'Equal','Operand2':'" + userid + "'},'Operator':'And'},'Operand2':{'Operand1':{'Operand1':{'Operand1':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'3'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'7'},'Operator':'Or'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'5'},'Operator':'Or'},'Operand2':{'Operand1':'Manager2Approval','Operator':'Equal','Operand2':'" + userid + "'},'Operator':'And'},'Operator':'Or'},'Operand2':{'Operand1':{'Operand1':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'5'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'7'},'Operator':'Or'},'Operand2':{'Operand1':'IkApproval','Operator':'Equal','Operand2':'" + userid + "'},'Operator':'And'},'Operator':'Or'}}",

					rejected = db.Table<VWINV_Permit>().Where(a =>
							 (a.Manager1Approval == userid && a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.Yonetici1Red)) ||
							 (a.Manager2Approval == userid && a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.Yonetici2Red)) ||
							 (a.IkApproval == userid && a.ApproveStatus == ((Int32)EnumINV_PermitApproveStatus.IkKontrolRed))).Count(),

					rejectedFilter = "{'Filter':{'Operand1':{'Operand1':{'Operand1':{'Operand1':'Manager1Approval','Operator':'Equal','Operand2':'" + userid + "'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'2'},'Operator':'And'},'Operand2':{'Operand1':{'Operand1':'Manager2Approval','Operator':'Equal','Operand2':'" + userid + "'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'4'},'Operator':'And'},'Operator':'Or'},'Operand2':{'Operand1':{'Operand1':'IkApproval','Operator':'Equal','Operand2':'" + userid + "'},'Operand2':{'Operand1':'ApproveStatus','Operator':'Equal','Operand2':'6'},'Operator':'And'},'Operator':'Or'}}",

					searchField = "{'Filter':{'Operand1':{'Operand1':{'Operand1':'Manager1Approval','Operator':'Equal','Operand2':'" + userid + "'},'Operand2':{'Operand1':{'Operand1':'Manager2Approval','Operator':'Equal','Operand2':'" + userid + "'},'Operand2':{'Operand1':'IkApproval','Operator':'Equal','Operand2':'" + userid + "'},'Operator':'Or'},'Operator':'Or'},'Operand2':{'Operand1':'searchField','Operator':'Like','Operand2':'%#%'},'Operator':'And'}}",


				};
			}
		}

	}
}