using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess.Mobile
{
    public class Transaction
    {
        public ResultStatus<Splitted<VMPA_Transaction>> MyTransaction(Guid userid)
        {
            var db = new WorkOfTimeDatabase();
            var datas = db.GetVWPA_TransactionByCreatedBy(userid);
            var res = new List<VMPA_Transaction>();
            foreach (var data in datas)
            {
                var item = new VMPA_Transaction().B_EntityDataCopyForMaterial(data, true);
                res.Add(item);
            }

            var tumu = res.Where(c => (c.type == (Int32)EnumPA_TransactionType.Masraf) && c.createdby == userid).ToArray();
            var bekleyen = res.Where(c => (c.direction == 0 && c.type == (Int32)EnumPA_TransactionType.Masraf) && c.createdby == userid).ToArray();
            var onaylanan = res.Where(c => (c.direction == -1 && c.type == (Int32)EnumPA_TransactionType.Masraf) && c.createdby == userid).ToArray();
            var reddedilen = res.Where(c => (c.direction == 2 && c.type == (Int32)EnumPA_TransactionType.Masraf) && c.createdby == userid).ToArray();

            return new ResultStatus<Splitted<VMPA_Transaction>>
            {
                result = true,
                objects = new Splitted<VMPA_Transaction>
                {
                    All = tumu,
                    Approved = onaylanan,
                    Waiting = bekleyen,
                    Declined = reddedilen
                }
            };
        }


		public ResultStatus<Splitted<VWPA_Transaction>> MySummaryTransaction(Guid userid)
		{
			var db = new WorkOfTimeDatabase();
			var datas = db.GetVWPA_TransactionMyWaitingApproved(userid);
            var onaylanan = datas;

            return new ResultStatus<Splitted<VWPA_Transaction>>
			{
				result = true,
				objects = new Splitted<VWPA_Transaction>
				{
					Waiting = onaylanan
				}
			};
		}
	}

    public class VMPA_Transaction : VWPA_Transaction
    {
        public VWINV_CommissionsPersons[] ComissionsPersons { get; set; }
        public VWINV_CommissionsProjects[] CommissionsProjects { get; set; }
    }
}

