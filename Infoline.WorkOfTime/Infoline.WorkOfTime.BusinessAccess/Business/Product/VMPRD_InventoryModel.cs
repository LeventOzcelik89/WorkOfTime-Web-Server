using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMPRD_InventoryModel : VWPRD_Inventory
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}

	public class VMPRD_InventoryLog : VWPRD_Inventory
	{
		public IOT_CameraLog[] CameraLogs { get; set; }
	}

	public class DetailInventoryModel
	{
		public Guid? ProductId { get; set; }
		public Guid? StockId { get; set; }
	}

	public class QrClass
	{
		public int? height { get; set; }
		public int? width { get; set; }
		public string logo { get; set; }
		public string phone { get; set; }
		public string fax { get; set; }
		public string weburl { get; set; }
		public InventoryFilterClass[] invertorys { get; set; }
		public InventoryFilterClass invertory { get; set; }
	}

	public class InventoryFilterClass
	{
		public Guid id { get; set; }
		public string serialcode { get; set; }
		public string code { get; set; }
		public string fullName { get; set; }
		public string productId_Title { get; set; }
	}
}
