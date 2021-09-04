using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMSH_UserRoleModel : VWSH_UserRole
    {
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}
	public class VMSH_UserRole : SH_UserRole
	{
		public Guid[] userIds { get; set; }
	}
}
