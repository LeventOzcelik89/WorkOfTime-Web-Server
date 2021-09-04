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

    [EnumInfo(typeof(CRM_PresentationInvoice), "type")]
    public enum EnumCRM_PresentationInvoiceType
    {
        [Description("Teklif")]
        Teklif = 0,
        [Description("Sipariş")]
        Siparis = 1,
        [Description("Fatura")]
        Fatura = 2,
    }


    partial class WorkOfTimeDatabase
    {

    }
}
