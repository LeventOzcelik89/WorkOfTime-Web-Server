using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMPRD_StockSummaryModel : VWPRD_StockSummary
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}
    public class TempModel
    {
        public List<Guid?> ProductIds { get; set; }
    }

    public class SummaryActionModel
    {
        public Guid? ProductId { get; set; }
        public Guid? StockId { get; set; }
        public string StockTable { get; set; }
    }

}
