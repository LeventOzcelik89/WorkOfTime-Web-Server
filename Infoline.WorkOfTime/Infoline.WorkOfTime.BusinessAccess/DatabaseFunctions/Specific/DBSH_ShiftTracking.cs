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

    [EnumInfo(typeof(SH_ShiftTracking), "passType")]
    public enum EnumSH_ShiftTrackingPassType
    {
        [Description("Mobil Cihaz")]
        MobilCihaz = 0,
        [Description("Pdks Parmak İzi")]
        PdksParmakİzi = 1,
        [Description("Pdks Şifre")]
        PdksSifre = 2,
        [Description("Pdks Kart")]
        PdksKart = 3,
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

        public SH_ShiftTracking[] GetSH_ShiftTrackingByDeviceIdAndUserDeviceId(Guid deviceId, string userDeviceId)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_ShiftTracking>().Where(a => a.shiftTrackingDeviceId == deviceId && a.deviceUserId == userDeviceId).Execute().OrderByDescending(x => x.timestamp).ToArray();
            }
        }

        public SH_ShiftTracking GetSH_ShiftTrackingFirstByUseridBeforeDate(Guid userid, DateTime beforeDate)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_ShiftTracking>().Where(a => a.userId == userid && a.timestamp < beforeDate).OrderByDesc(a => a.timestamp).Take(1).Execute().FirstOrDefault();
            }
        }

        public SH_ShiftTracking GetSH_ShiftTrackingLastRecordByUserId(Guid userId)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_ShiftTracking>().Where(a => a.userId == userId).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public SH_ShiftTracking GetSH_ShiftTrackingLastRecordByDeviceIdAndUserDeviceId(Guid deviceId, string UserDeviceId)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_ShiftTracking>().Where(a =>a.shiftTrackingDeviceId == deviceId  && a.deviceUserId == UserDeviceId).OrderByDesc(a => a.timestamp).Take(1).Execute().FirstOrDefault();
            }
        }

        public SH_ShiftTracking GetSH_ShiftTrackingLastRecordByDeviceId(Guid deviceId)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_ShiftTracking>().Where(a => a.shiftTrackingDeviceId == deviceId).OrderByDesc(a => a.created).Take(1).Execute().FirstOrDefault();
            }
        }
    }
}
