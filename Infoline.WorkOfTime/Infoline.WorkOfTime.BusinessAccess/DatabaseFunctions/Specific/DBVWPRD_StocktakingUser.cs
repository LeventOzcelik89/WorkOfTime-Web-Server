using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;
using System.Collections.Generic;
using static Infoline.WorkOfTime.BusinessAccess.VMFTM_TaskModel;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{

	partial class WorkOfTimeDatabase
	{
		public VWPRD_StocktakingUser[] GetVWPRD_StocktakingUserByStocktakingId(Guid stocktakingId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWPRD_StocktakingUser>().Where(a => a.stocktakingId == stocktakingId).Execute().ToArray();
			}
		}
	}
}


