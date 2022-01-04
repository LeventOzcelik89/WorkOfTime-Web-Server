using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;
namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(SV_DeviceProblem), "type")]
    public enum EnumSV_DeviceProblemType {

        [Description("Müşteri Beyanı"), Generic("icon", "fa fa-user", "color", "2D7DD2", "description", "Müşteri Beyanı")]
        Customer,
        [Description("Teknik Servis"), Generic("icon", "fa fa-wranch", "color", "2D7DD2", "description", "Teknik Servis")]
        Service
    }
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
