using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Business.Storage;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebService.Handler
{
	[Export(typeof(ISmartHandler))]
    public partial class TpusmanHandler : BaseSmartHandler
    {
        public TpusmanHandler() : base("TpusmanHandler")
        {

        }


        [HandleFunction("VWCMP_Storage/GetCustom")]
        public void VWCMP_StorageGetCustom(HttpContext context)
        {
            try
            {
                var request = ParseRequest<StorageRequest>(context) ?? new StorageRequest { };
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCMP_StorageMyStorage(request.keyword);
                double? distance = null;

                var response = data.Select(a => new StorageReponse
                {
                    id = a.id,
                    adress = string.IsNullOrEmpty(a.address) ? a.locationId_Title : a.address,
                    fullName = a.fullName,
                    location = a.location,
                    distance = request.location != null && a.location != null ? request.location.Distance(a.location) * 100 : distance,
                })
                .OrderBy(a => a.distance)
                .Skip(request.skip ?? 0)
                .Take(request.take ?? 100)
                .ToArray();

                RenderResponse(context, response);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}