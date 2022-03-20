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
    [EnumInfo(typeof(PRD_ProductPayment), "hasThePayment")]
    public enum EnumPRD_PRD_ProductPaymentHasThePayment
    {
        [Description("Hakediş Ödenmedi")]
        notPaid = 0,
        [Description("Hakediş Ödendi")]
        paid = 1,
    }
    partial class WorkOfTimeDatabase
    {

    }
}
