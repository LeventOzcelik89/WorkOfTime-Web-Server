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
   
    [EnumInfo(typeof(PRD_ProductUnit), "isDefault")]
    public enum EnumPRD_ProductUnitIsDefault
    {
        [Description("Hayır"),Generic("order","2")]
        Hayir = 0,
        [Description("Evet"), Generic("order", "1")]
        Evet = 1,
    }


    partial class WorkOfTimeDatabase
    {
        public PRD_ProductUnit[] GetPRD_ProductUnitByProductIds(Guid[] productIds)
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_ProductUnit>().Where(a => a.productId.In(productIds)).Execute().ToArray();
            }
        }

    }
}
