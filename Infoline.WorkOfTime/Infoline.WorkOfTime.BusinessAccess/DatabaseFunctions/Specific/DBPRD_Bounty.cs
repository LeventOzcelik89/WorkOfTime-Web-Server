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
    [EnumInfo(typeof(PRD_Bounty), "status")]
    public enum EnumPRD_BountyStatus
    {
        [Description("Ödeme Yapılmadı")]
        notPayment = 0,
        [Description("Ödeme Yapıldı")]
        completePayment = 1,
    }
    partial class WorkOfTimeDatabase
    {
     
    }
}
