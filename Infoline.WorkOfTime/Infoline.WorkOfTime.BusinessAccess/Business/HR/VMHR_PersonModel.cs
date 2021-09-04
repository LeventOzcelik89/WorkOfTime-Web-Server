using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMHR_PersonModel : VWHR_Person
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}

	public class VMKeywordAndPositions
	{
		public HR_Keywords[] keywords { get; set; }
		public HR_Position[] positions { get; set; }
	}

}
