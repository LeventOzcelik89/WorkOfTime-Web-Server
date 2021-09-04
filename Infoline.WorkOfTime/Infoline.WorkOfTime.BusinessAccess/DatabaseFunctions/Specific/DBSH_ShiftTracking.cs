using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(SH_ShiftTracking), "shiftTrackingStatus")]
    public enum EnumSH_ShiftTrackingShiftTrackingStatus
    {
        [Description("Mesaiye Başlandı")]
        MesaiBaslandi = 0,
        [Description("Mesai Bitti")]
        MesaiBitti = 1,
        [Description("Mola Verildi")]
        MolaVerildi = 2,
        [Description("Mola Bitti")]
        MolaBitti = 3,
    }


    public partial class WorkOfTimeDatabase
    {
        public SH_ShiftTracking[] GetSH_ShiftTrackingByDate(DateTime date)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_ShiftTracking>().Where(a => a.timestamp >= date && a.timestamp <= date.AddDays(1)).Execute().ToArray();
            }
        }


        public SH_ShiftTracking GetSH_ShiftTrackingByUserId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_ShiftTracking>().Where(a => a.userId == id).Execute().OrderByDescending(x => x.timestamp).FirstOrDefault();
            }
        }

        public SH_ShiftTracking GetSH_ShiftTrackingFirstByUseridBeforeDate(Guid userid, DateTime beforeDate)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_ShiftTracking>().Where(a => a.userId == userid && a.timestamp < beforeDate).OrderByDesc(a => a.timestamp).Take(1).Execute().FirstOrDefault();
            }
        }
    }
}
