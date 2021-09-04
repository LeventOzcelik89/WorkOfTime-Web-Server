using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMUT_LocationTrackingModel : VWUT_LocationTracking
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}
    public class VMUT_LocationTrackings : UT_LocationTracking
    {
        public string latitude { get; set; }
        public string longitude { get; set; }
    }

    public class VMUT_LocationConfig : UT_LocationConfig
    {
        public bool? isTrackingActive { get; set; }
    }

}
