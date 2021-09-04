using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMPRJ_ProjectScopeLevelModel : VWPRJ_ProjectScopeLevel
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}
	public class VWPRJ_ProjectScopeLevelData : VWPRJ_ProjectScopeLevel
	{
		public Guid[] locationIds { get; set; } = new Guid[] { };
	}
}
