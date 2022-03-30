using System;
using System.Linq;
using Infoline.Framework.Database;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public VWPRD_Product[] GetVWPRD_ProductByProductIds(Guid[] productIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPRD_Product>().Where(a => a.id.In(productIds)).Execute().ToArray();
            }
        }
        public VWPRD_Product[] GetVWPRD_ProductByIds(Guid[] ids)
        {

            using (var db = GetDB())
            {
                return db.Table<VWPRD_Product>().Where(a => a.id.In(ids)).Execute().ToArray();
            }
        }
        public VWPRD_Product[] GetVWPRD_ProductByType(EnumPRD_ProductType type)
        {

            using (var db = GetDB())
            {
                return db.Table<VWPRD_Product>().Where(a => a.type == (short)type).Execute().ToArray();
            }
        }

        public VWPRD_Product GetVWPRD_ProductByCode(string code)
        {

            using (var db = GetDB())
            {
                return db.Table<VWPRD_Product>().Where(a => a.code == code).Execute().FirstOrDefault();
            }
        }



    }
}
