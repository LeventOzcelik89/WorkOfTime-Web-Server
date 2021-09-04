using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{
    [EnumInfo(typeof(INV_PermitType), "IsPaidPermit")]
    public enum EnumINV_PermitTypeIsPaidPermit
    {
        [Description("Hayır")]
        Hayir = 0,
        [Description("Evet")]
        Evet = 1
    }
    [EnumInfo(typeof(INV_PermitType), "CanHourly")]
    public enum EnumINV_PermitTypeCanHourly
    {
        [Description("Günlük")]
        Gunluk = 0,
        [Description("Saatlik")]
        Saat = 1
    }
    [EnumInfo(typeof(INV_PermitType), "BeDocumented")]
    public enum EnumINV_PermitTypeBeDocumented
    {
        [Description("Hayır")]
        Hayir = 0,
        [Description("Evet")]
        Evet = 1
    }
    public partial class WorkOfTimeDatabase
    {

        public INV_PermitType[] INV_PermitTypeNameIdCreated()
        {
            using (var db = GetDB())
            {
                return db.Table<INV_PermitType>()
                    .Select(x => new
                    {
                        id = x.id,
                        created = x.created,
                        Name = x.Name,
                        RequestStaff = x.RequestStaff
                    }).OrderBy(x => x.created).Execute<INV_PermitType>().ToArray();
            }
        }
    }
}
