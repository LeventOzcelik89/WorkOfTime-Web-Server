using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMHR_StaffNeedsModel : VWHR_StaffNeeds
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}
	public class VMPersonToManager
	{
		public Guid staffNeedPersonId { get; set; }
		public Guid needId { get; set; }
		public Guid[] requestUsers { get; set; }
	}
}
