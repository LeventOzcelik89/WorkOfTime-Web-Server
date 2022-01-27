using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess.Mobile
{
    public class Locations
    {
        public List<VMUT_LocationConfig> LConfigGetByUserId(Guid userId)
        {
            var db = new WorkOfTimeDatabase();
            var list = new List<VMUT_LocationConfig>();
            var data = db.GetUT_LocationConfigUserByUserId(userId).Where(x => x.locationConfigId.HasValue).Select(x => x.locationConfigId.Value).ToArray();
            
            if (data.Count() > 0)
            {
                var locationConfigs = db.GetUT_LocationConfigByIds(data);
                foreach (var locationConfig in locationConfigs)
                {
                    var res = new VMUT_LocationConfig();
                    var datas = res.B_EntityDataCopyForMaterial(locationConfig);
                    var tracking = db.GetUT_LocationConfigUserAndConfigId(userId, locationConfig.id);

                    if (tracking != null)
                    {
                        datas.isTrackingActive = tracking.isTrackingActive;
                    }

                    list.Add(datas);
                }

                return list;
            }

            return null;
        }
    }
}

