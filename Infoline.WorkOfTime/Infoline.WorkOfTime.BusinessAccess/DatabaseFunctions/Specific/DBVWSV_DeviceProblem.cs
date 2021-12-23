using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public VWSV_DeviceProblem[] GetVWSV_DeviceProblemsByServiceId(Guid serviceId, DbTransaction transaction = null)
        {
            using (var db = GetDB(transaction))
            {
                return db.Table<VWSV_DeviceProblem>().Where(x => x.serviceId == serviceId).Execute().ToArray();
            }
        }
        
    }
}
