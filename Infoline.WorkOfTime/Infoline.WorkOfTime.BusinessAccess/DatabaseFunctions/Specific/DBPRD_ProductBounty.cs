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
	}
}
