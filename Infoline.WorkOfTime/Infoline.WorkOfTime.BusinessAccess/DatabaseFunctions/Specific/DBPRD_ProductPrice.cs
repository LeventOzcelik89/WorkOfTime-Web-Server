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


    [BusinessAccess.EnumInfo(typeof(PRD_ProductPrice), "type")]
    public enum EnumPRD_ProductPriceType
    {
        [Description("Birim Maliyet")]
        Alis = 0,
        [Description("Birim Fiyat")]
        Satis = 1,
        [Description("Teknik Servis")]
        TeknikServis= 2,
    }

    partial class WorkOfTimeDatabase
    {
        public PRD_ProductPrice GetPRD_ProductPriceByProductId(Guid productId, EnumPRD_ProductPriceType type, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductPrice>().Where(x => x.productId == productId && x.type == (int)type).OrderByDesc(a => a.created).Skip(0).Take(1).Execute().FirstOrDefault();
            }
        }

        public PRD_ProductPrice[] GetPRD_ProductPriceByProductIdAll(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductPrice>().Where(x => x.productId == id).Execute().ToArray();
            }
        }

        public PRD_ProductPrice[] GetPRD_ProductPriceByProductIds(Guid[] productIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_ProductPrice>().Where(x => x.productId.In(productIds)).Execute().ToArray();
            }
        }

    }
}
