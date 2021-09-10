using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;


namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(PRD_ProductionProduct), "type")]
    public enum EnumPRD_ProductionProductsType
    {
        [Description("Reçete Ürünü")]
        RecetedenGelen = 0,
        [Description("Sonradan Eklenen")]
        SonradanEklenen = 1,
    }
    
    partial class WorkOfTimeDatabase
    {
        public PRD_ProductionProduct[] GetPRD_ProductionProductByProductionId(Guid productionId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductionProduct>().Where(x => x.productionId == productionId).Execute().ToArray();
            }
        }
    }
}
