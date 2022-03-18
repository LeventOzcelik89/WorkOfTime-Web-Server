using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;
using System.Collections.Generic;
using static Infoline.WorkOfTime.BusinessAccess.VMFTM_TaskModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
	partial class WorkOfTimeDatabase
	{
		public VWPRD_StocktakingItem[] GetVWPRD_StocktakingItemByStocktakingId(Guid stocktakingId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWPRD_StocktakingItem>().Where(a => a.stocktakingId == stocktakingId).Execute().ToArray();
			}
		}
	}
}


