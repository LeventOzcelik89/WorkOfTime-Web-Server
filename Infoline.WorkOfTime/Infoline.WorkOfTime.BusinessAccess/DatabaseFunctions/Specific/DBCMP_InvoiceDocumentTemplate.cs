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
    [EnumInfo(typeof(CMP_InvoiceDocumentTemplate), "templateVisibleAllUser")]
    public enum EnumCMP_InvoiceDocumentTemplateVisible
    {
        [Description("Evet")]
        Evet = 1,
        [Description("Hayır")]
        Hayir = 0,
        
    }
}
