using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public PRD_TitanDeviceActivated GetPRD_TitanDeviceActivatedBySerialCode(string id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(a => a.SerialNumber == id).Execute().FirstOrDefault();
            }
            
        }
        public PRD_TitanDeviceActivated[] GetPRD_TitanDeviceActivatedByProductId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(a => a.ProductId == id).Execute().ToArray();
            }

        }
        public PRD_TitanDeviceActivated GetPRD_TitanDeviceActivatedByInventoryId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(a => a.InventoryId == id).Execute().FirstOrDefault();
            }

        }
        public int GetPRD_TitanDeviceActivatedCount(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Execute().Count();
            }
          
        }
        public int GetPRD_TitanDeviceActivatedTodayCount(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(x=>x.CreatedOfTitan>DateTime.Now.AddDays(-1)).Execute().Count();
            }

        }
        public int GetPRD_TitanDeviceActivatedSevenDaysCount(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(x=>x.CreatedOfTitan>DateTime.Now.AddDays(-7)).Execute().Count();
            }
        }
        public int GetPRD_TitanDeviceActivatedThirtyDaysCount(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_TitanDeviceActivated>().Where(x => x.CreatedOfTitan > DateTime.Now.AddDays(-30)).Execute().Count();
            }
        }

    }
}
