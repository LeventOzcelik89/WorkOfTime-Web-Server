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
        public PRD_ProductProgressPayment[] GetPRD_ProductProgressPaymentExistByDataQuery(string dataQuery)
        {
            using (var db = GetDB())
            {
                var query = $@"Select * from(
                            select companyId,productId,count(*) as quantity from PRD_ProductProgressPayment Group by companyId,productId 
                            ) as b 
                            Where " + dataQuery;
                var queryExecute = db.ExecuteReader<PRD_ProductProgressPayment>(query.ToString()).ToArray();
                return queryExecute;
            }
        }

    }
}
