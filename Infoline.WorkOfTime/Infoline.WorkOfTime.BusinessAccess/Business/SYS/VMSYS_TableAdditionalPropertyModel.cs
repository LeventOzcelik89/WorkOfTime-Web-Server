using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMSYS_TableAdditionalPropertyModel : VWSYS_TableAdditionalProperty
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}

	public class VMSYS_TableAdditionalProperty
	{
		public VWSYS_TableAdditionalProperty[] properties { get; set; }
		public Guid dataId { get; set; }
		public string dataTable { get; set; }
	}

}
