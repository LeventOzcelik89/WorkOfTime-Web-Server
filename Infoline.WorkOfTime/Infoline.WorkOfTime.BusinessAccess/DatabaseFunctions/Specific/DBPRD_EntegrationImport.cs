using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{
	partial class WorkOfTimeDatabase
	{
		
		public VWPRD_EntegrationImport[] GetVWPRD_EntegrationImportByPeriodAndCompanyCode(int month, int year ,string companyCode, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWPRD_EntegrationImport>().Where(a => a.month == month && a.year == year&&a.customerCode==companyCode).Execute().ToArray();
			}
		}
		public VWPRD_EntegrationImport GetVWPRD_EntegrationImportBySerialCode(string serialCode, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<VWPRD_EntegrationImport>().Where(a => a.imei ==serialCode).Execute().FirstOrDefault();
			}
		}
	}
}
