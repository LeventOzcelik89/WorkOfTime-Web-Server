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
	[EnumInfo(typeof(PRD_StocktakingItem), "status")]
	public enum EnumPRD_StocktakingItemStatus
	{
		[Description("Sayım Başladı"), Generic("color", "EF5352")]
		SayimBasladi = 0,
		[Description("Sayım Tamamlandı"), Generic("color", "1ab394")]
		SayimTamamlandi = 1,
		[Description("Stoklara İşlendi"), Generic("color", "F8AC59")]
		StoklaraIslendi = 2,
	}

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


