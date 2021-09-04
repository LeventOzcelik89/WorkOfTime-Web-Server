using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class MBUT_LocationHandler : BaseSmartHandler
    {
        public MBUT_LocationHandler()
            : base("MBUT_LocationHandler")
        {

        }

        [HandleFunction("MBUT_LocationTracking/Insert")]
        public void MBUT_LocationTrackingInsert(HttpContext context)
        {
            Log.Warning("Ömer servise geldi.");
            var db = new WorkOfTimeDatabase();
            var locations = ParseRequest<VMUT_LocationTrackings[]>(context);
            var id = context.Request["userId"];
            if (locations == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Obje dönüştürülemedi" });
                return;
            }

            if (string.IsNullOrEmpty(id))
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı idsi bulunamadı." });
                return;
            }

            foreach (var location in locations)
            {
                location.userId = new Guid(id);
                if(!string.IsNullOrEmpty(location.latitude) && !string.IsNullOrEmpty(location.longitude))
                {
                    var point = "POINT(" + location.longitude + " " + location.latitude + ")";
                    location.location = new NetTopologySuite.IO.WKTReader().Read(point);
                }
            }

            var res = db.BulkInsertUT_LocationTracking(locations);

            RenderResponse(context, new ResultStatus { result = res.result , message = res.result ? "Kaydetme işlemi başarılı" : "Kaydetme işlemi başarısız oldu." });
        }


        [HandleFunction("MBUT_LocationConfig/GetByUserId")]
        public void MBUT_LocationConfigGetByUserId(HttpContext context)
        {
            var db = new WorkOfTimeDatabase();

            if(CallContext.Current == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı girişi yapılmamış" });
                return;

            }
            var list = new List<VMUT_LocationConfig>();
            var data = db.GetUT_LocationConfigUserByUserIdGetConfigIds(CallContext.Current.UserId).Where(x => x.locationConfigId.HasValue).Select(x => x.locationConfigId.Value).ToArray();
            if(data.Count() > 0)
            {

                var locationConfigs = db.GetUT_LocationConfigByIds(data);
                foreach (var locationConfig in locationConfigs)
                {
                    var res = new VMUT_LocationConfig();
                    var datas = res.B_EntityDataCopyForMaterial(locationConfig);
                    var tracking = db.GetUT_LocationConfigUserAndConfigId(CallContext.Current.UserId, locationConfig.id);

                    if(tracking != null)
                    {
                        datas.isTrackingActive = tracking.isTrackingActive;
                    }

                    list.Add(datas);
                }

                RenderResponse(context, new ResultStatus { result = true, objects = list });
                return;
            }

            RenderResponse(context, new ResultStatus { result = false, message = "Config bulunamadı." });
        }
    }
}