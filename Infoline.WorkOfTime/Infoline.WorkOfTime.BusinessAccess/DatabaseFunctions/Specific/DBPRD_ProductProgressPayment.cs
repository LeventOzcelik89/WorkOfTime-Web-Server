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
    [EnumInfo(typeof(PRD_ProductProgressPayment), "isProgressPayment")]
    public enum EnumPRD_ProductProgressPaymentIsProgressPayment
    {
        [Description("Hakediş Onay Bekleniyor")]
        approving = 0,
        [Description("Hakediş Onaylandı")]
        approved = 1,
    }
    partial class WorkOfTimeDatabase
    {
        public PRD_ProductProgressPayment[] GetPRD_ProductProgressPaymentExistByDataQuery(string dataQuery, Guid id)
        {
            using (var db = GetDB())
            {
                var query = $@"Select * from(
                               select id,companyId,productId,count(*) as quantity from PRD_ProductProgressPayment Group by id,companyId,productId 
                               ) as b
                               where b.id = '" + id + "' and " + dataQuery;
                var queryExecute = db.ExecuteReader<PRD_ProductProgressPayment>(query.ToString()).ToArray();
                return queryExecute;
            }
        }

    }
}
