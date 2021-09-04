using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMUT_RulesUserModel : VWUT_RulesUser
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}

	public class VMUT_RulesUser : UT_RulesUser
	{
		public Guid[] userIds { get; set; }
	}

}
