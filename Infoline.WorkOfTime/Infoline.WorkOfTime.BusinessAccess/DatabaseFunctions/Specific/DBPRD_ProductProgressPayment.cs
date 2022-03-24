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
        public PRD_ProductProgressPayment[] GetPRD_BountyProductExistByDataQuery(string dataQuery, Guid? productId, string startDate, string endDate)
        {
            using (var db = GetDB())
            {
                var query = $@"Select * from(
                               select companyId, productId, CAST(MONTH(date) AS VARCHAR(2)) +'-' + CAST(YEAR(date) AS VARCHAR(4)) AS dates, count(*) as quantity from PRD_ProductProgressPayment Group by companyId, productId, CAST(MONTH(date) AS VARCHAR(2)) +'-' + CAST(YEAR(date) AS VARCHAR(4))) as b" +
                            " where b.productId = '" + productId + "' and " + dataQuery + " and dates between CAST(MONTH('" + startDate + "') AS VARCHAR(2)) + '-' + CAST(YEAR('" + startDate + "') AS VARCHAR(4)) and CAST(MONTH('" + endDate + "') AS VARCHAR(2)) + '-' + CAST(YEAR('" + endDate + "') AS VARCHAR(4))";
                var queryExecute = db.ExecuteReader<PRD_ProductProgressPayment>(query.ToString()).ToArray();
                return queryExecute;
            }
        }

    }
}
