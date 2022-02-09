using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework;


namespace Infoline.WorkOfTime.BusinessAccess
{
	[EnumInfo(typeof(INV_Permit), "ApproveStatus")]
	public enum EnumINV_PermitApproveStatus
	{
		[Description("1.Yönetici Onayı Bekleniyor")]
		TalepEdildi = 0,
		[Description("2.Yönetici Onayı Bekleniyor")]
		Yonetici1Onay = 1,
		[Description("1. Yönetici Reddetti")]
		Yonetici1Red = 2,
		[Description("İ.K. Onayı Bekleniyor")]
		Yonetici2Onay = 3,
		[Description("2.Yönetici Reddetti")]
		Yonetici2Red = 4,
		[Description("Islak İmzalı Dosya Bekleniyor")]
		IkKontrol = 5,
		[Description("İ.K. Reddetti")]
		IkKontrolRed = 6,
		[Description("Islak imzalı Dosya Yüklendi")]
		IslakImzaYuklendi = 7,
		[Description("Geçmiş İzin (İ.K. Ekledi)")]
		GecmisIzin = 8,
		[Description("Sağlık Raporu İzni (İ.K. Ekledi)")]
		SaglikRaporu = 9,
		[Description("Avans İzin (İ.K. Ekledi)")]
		AvansIzin = 10,
	}


	public class SummaryHeadersPermit
	{
		public int waiting { get; set; }
		public string waitingFilter { get; set; }
		public int approved { get; set; }
		public string approvedFilter { get; set; }
		public int rejected { get; set; }
		public string rejectedFilter { get; set; }
		public string searchField { get; set; }
	}

	public partial class WorkOfTimeDatabase
	{

		public INV_Permit GetINV_PermitApprovedPermit(Guid IdUser, DateTime startDate, DateTime endDate)
		{
			using (var db = GetDB())
			{
				return db.Table<INV_Permit>().Where(x => x.IdUser == IdUser && (x.ApproveStatus == (int)EnumINV_PermitApproveStatus.GecmisIzin || x.ApproveStatus == (int)EnumINV_PermitApproveStatus.IkKontrol || x.ApproveStatus == (int)EnumINV_PermitApproveStatus.IslakImzaYuklendi || x.ApproveStatus == (int)EnumINV_PermitApproveStatus.SaglikRaporu || x.ApproveStatus == (int)EnumINV_PermitApproveStatus.TalepEdildi || x.ApproveStatus == (int)EnumINV_PermitApproveStatus.Yonetici1Onay || x.ApproveStatus == (int)EnumINV_PermitApproveStatus.Yonetici2Onay) &&
					((x.StartDate <= startDate && x.EndDate >= endDate) ||
				(x.StartDate < startDate && x.EndDate > startDate && x.EndDate < endDate) ||
				(x.StartDate > startDate && x.StartDate < endDate && x.EndDate > endDate) ||
				(startDate <= x.StartDate && endDate >= x.EndDate)
				)).OrderBy(x => x.created).Execute().FirstOrDefault();
			}
		}
		public INV_Permit[] GetINV_PermitApprovedByStatus(Guid IdUser)
		{
			using (var db = GetDB())
			{
				return db.Table<INV_Permit>().Where(x => x.Manager1Approval == IdUser &&x.Manager1ApprovalDate==null|| x.Manager2Approval == IdUser&&x.Manager2ApprovalDate == null || x.IkApproval == IdUser&&x.IkApprovalDate == null).Execute().ToArray();
			}
		}

		public INV_Permit GetINV_PermitByControlDate(Guid IdUser, DateTime? start, DateTime? end)
		{
			using (var db = GetDB())
			{
				return db.Table<INV_Permit>().Where(a =>
				a.IdUser == IdUser &&
				(a.ApproveStatus != (Int32)EnumINV_PermitApproveStatus.Yonetici1Red && a.ApproveStatus != (Int32)EnumINV_PermitApproveStatus.Yonetici2Red && a.ApproveStatus != (Int32)EnumINV_PermitApproveStatus.IkKontrolRed) && ((
				(a.StartDate <= start && a.EndDate >= end) ||
				(a.StartDate < start && a.EndDate > start && a.EndDate < end) ||
				(a.StartDate > start && a.StartDate < end && a.EndDate > end) ||
				(start <= a.StartDate && end >= a.EndDate)
				))).OrderByDesc(x => x.created).Execute().FirstOrDefault();
			}
		}

		public VWINV_Permit[] GetVWINV_PermitByIdUsers(Guid[] userIds)
		{
			using (var db = GetDB())
			{
				return db.Table<VWINV_Permit>().Where(a => a.IdUser.In(userIds)).Execute().ToArray();
			}
		}

		public VWINV_Permit[] GetVWINV_PermitByUserIdAndPermitTypeIds(Guid userId, Guid[] permitTypeIds)
		{
			using (var db = GetDB())
			{
				return db.Table<VWINV_Permit>().Where(a => a.IdUser == userId && a.PermitTypeId.In(permitTypeIds) && (a.ApproveStatus == (int)EnumINV_PermitApproveStatus.IslakImzaYuklendi || a.ApproveStatus == (int)EnumINV_PermitApproveStatus.IkKontrol || a.ApproveStatus == (int)EnumINV_PermitApproveStatus.GecmisIzin)).Execute().ToArray();
			}
		}



	}
}
