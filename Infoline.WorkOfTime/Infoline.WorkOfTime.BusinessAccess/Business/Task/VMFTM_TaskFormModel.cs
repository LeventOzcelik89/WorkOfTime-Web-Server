using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMFTM_TaskFormModel : VWFTM_TaskForm
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}

	public class VMFTM_TaskForm : VWFTM_TaskForm
	{
		public Guid? productId { get; set; }
		public Guid?[] inventoryId { get; set; }
		public Guid? companyId { get; set; }
		public Guid? companyStorageId { get; set; }

	}
}
