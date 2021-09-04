using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess.Mobile
{
	public class Advance
	{
		public ResultStatus<Splitted<VMPA_Advance>> MyAdvance(Guid userid)
		{
			var db = new WorkOfTimeDatabase();
			var datas = db.GetVWPA_AdvanceByCreatedBy(userid);
			var res = new List<VMPA_Advance>();
			foreach (var data in datas)
			{
				var item = new VMPA_Advance().B_EntityDataCopyForMaterial(data, true);
				res.Add(item);
			}

			var tumu = res.Where(c => c.createdby == userid).ToArray();
			var bekleyen = res.Where(c => c.direction == 0  && c.createdby == userid).ToArray();
			var onaylanan = res.Where(c => c.direction == -1  && c.createdby == userid).ToArray();
			var reddedilen = res.Where(c => c.direction == 2 && c.createdby == userid).ToArray();

			return new ResultStatus<Splitted<VMPA_Advance>>
			{
				result = true,
				objects = new Splitted<VMPA_Advance>
				{
					All = tumu,
					Approved = onaylanan,
					Waiting = bekleyen,
					Declined = reddedilen
				}
			};
		}

		public ResultStatus<Splitted<VWPA_Advance>> MySummaryTransaction(Guid userid)
		{
			var db = new WorkOfTimeDatabase();
			var datas = db.GetVWPA_AdvanceMyWaitingApproved(userid);
			var onaylanan = datas;

			return new ResultStatus<Splitted<VWPA_Advance>>
			{
				result = true,
				objects = new Splitted<VWPA_Advance>
				{
					Waiting = onaylanan
				}
			};
		}
	}

	public class VMPA_Advance : VWPA_Advance
	{
		public VWINV_CommissionsPersons[] ComissionsPersons { get; set; }
		public VWINV_CommissionsProjects[] CommissionsProjects { get; set; }
	}
}

