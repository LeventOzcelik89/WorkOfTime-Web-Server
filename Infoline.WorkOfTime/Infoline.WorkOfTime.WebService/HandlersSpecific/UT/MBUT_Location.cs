using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Mobile;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebService
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
            //Log.Warning("Ömer servise geldi.");
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
                if (!string.IsNullOrEmpty(location.latitude) && !string.IsNullOrEmpty(location.longitude))
                {
                    var point = "POINT(" + location.longitude + " " + location.latitude + ")";
                    location.location = new NetTopologySuite.IO.WKTReader().Read(point);
                }
            }

            var res = db.BulkInsertUT_LocationTracking(locations);

            RenderResponse(context, new ResultStatus { result = res.result, message = res.result ? "Kaydetme işlemi başarılı" : "Kaydetme işlemi başarısız oldu." });
        }


        [HandleFunction("MBUT_LocationConfig/GetByUserId")]
        public void MBUT_LocationConfigGetByUserId(HttpContext context)
        {
            if (CallContext.Current == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı girişi yapılmamış" });
                return;
            }

            var locationList = new Locations().LConfigGetByUserId(CallContext.Current.UserId);
            if (locationList != null)
            {
                RenderResponse(context, new ResultStatus { result = true, objects = locationList });
                return;
            }

            RenderResponse(context, new ResultStatus { result = false, message = "Config bulunamadı." });
        }
    }
}