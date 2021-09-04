using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;
using Infoline.Framework.Database;
using System.Linq;
using System.Data.Common;
using System;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(PRD_StockTaskPlan), "status")]
    public enum EnumPRD_StockTaskPlanStatus
    {
        [Description("Görev Açılmadı")]
        Acilmadi = 0,
        [Description("Görev Açıldı")]
        Acildi = 1,
        [Description("Görev Bitti")]
        Bitti = 2
    }

   
    partial class WorkOfTimeDatabase
    {
        public VWPRD_StockTaskPlan[] GetVWPRD_StockTaskPlanByStorageCodes(string[] codes, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWPRD_StockTaskPlan>().Where(x => x.storageCode.In(codes)).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public VWPRD_StockTaskPlan[] GetVWPRD_StockTaskPlanByIds(Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPRD_StockTaskPlan>().Where(a => a.id.In(ids)).Execute().ToArray();
            }
        }

        public VWPRD_StockTaskPlan GetVWPRD_StockTaskPlanByTaskId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPRD_StockTaskPlan>().Where(a => a.taskId == id).Execute().FirstOrDefault();
            }
        }
    }
}
