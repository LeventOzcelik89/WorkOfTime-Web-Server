using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMINV_CompanyDepartmentsModel : VWINV_CompanyDepartments
	{
		private WorkOfTimeDatabase db { get; set; }
		private DbTransaction trans { get; set; }
	}

    public class Organization
    {
        public Guid? id { get; set; }
        public string department { get; set; }
        public string color { get; set; }
        public string personels { get; set; }
        public Organization[] children { get; set; }
        public bool collapsed { get; set; }
    }

    public class DepartmentsDetailPageReport
    {
        public int ToplamBirimSayisi { get; set; }
        public int YanBirimSayisi { get; set; }
        public string SonEklenenBirim { get; set; }
    }
}
