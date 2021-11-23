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
		public PRD_ProductBounty[] GetPRD_ProductBountyByPersonIds(Guid[] personIds, Guid productId, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<PRD_ProductBounty>().Where(a => a.personId.In(personIds) && a.productId == productId).Execute().ToArray();
			}
		}

		public PRD_ProductBounty[] GetPRD_ProductBountyByPeriodAndPersonId(int month, int year, Guid productId, Guid[] personIds, DbTransaction tran = null)
		{
			using (var db = GetDB(tran))
			{
				return db.Table<PRD_ProductBounty>().Where(a => a.month == month && a.year == year && a.productId == productId && a.personId.In(personIds)).Execute().ToArray();
			}
		}
	}
}
