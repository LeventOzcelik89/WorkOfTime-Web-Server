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

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class ShiftTracking
	{
		public Guid userId { get; set; }
		public DateTime date { get; set; }
		public string[] shiftStart { get; set; }
		public string[] shiftEnd { get; set; }
		public string[] breakStart { get; set; }
		public string[] breakEnd { get; set; }
		public string totalWorking { get; set; }
	}

	partial class WorkOfTimeDatabase
    {
		
        public VWSH_ShiftTracking[] VWGetSH_ShiftTrackingByDate(DateTime date)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_ShiftTracking>().Where(a => a.timestamp >= date && a.timestamp <= date.AddDays(1)).Execute().ToArray();
            }
        }
        public VWSH_ShiftTracking[] VWGetSH_ShiftTrackingByStartAndEndDate(DateTime startDate, DateTime endDate)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_ShiftTracking>().Where(a => a.timestamp >= startDate && a.timestamp <= endDate.AddDays(1)).Execute().ToArray();
            }
        }


        public VWSH_ShiftTracking[] VWGetSH_ShiftTrackingByDateAndUserId(DateTime date, Guid userId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_ShiftTracking>().Where(x => x.timestamp >= date && x.timestamp <= date.AddDays(1) && x.userId == userId).Execute().OrderByDescending(x => x.timestamp).ToArray();
            }
        }

  



        public VWSH_ShiftTracking GetVWSH_ShiftTrackingLastRecordByUserIdAndDateAndTypeInvetory(Guid userid, DateTime date)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_ShiftTracking>().Where(a => a.userId == userid && a.timestamp > date && a.timestamp < date.AddDays(1).AddMinutes(-1)  && a.tableName == "PRD_Inventory").OrderByDesc(a => a.timestamp).Take(1).Execute().FirstOrDefault();
            }
        }
    }
}
