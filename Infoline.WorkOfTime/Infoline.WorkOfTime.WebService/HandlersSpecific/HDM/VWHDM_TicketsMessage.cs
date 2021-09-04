using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.Text.RegularExpressions;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{

    [Export(typeof(ISmartHandler))]
    public partial class VWHDM_TicketsMessageHandler : BaseSmartHandler
    {
        public VWHDM_TicketsMessageHandler()
            : base("VWHDM_TicketMessage")
        {

        }

        [HandleFunction("VWHDM_TicketMessage/Insert")]
        public void VWHDM_TicketInsert(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var model = ParseRequest<VMHDM_TicketMessageModel>(context);
                var files = new SYS_Files();
                if (model.ticketId.HasValue)
                {
                     files = db.GetSYS_FilesByDataId(model.id);
                }
                var userId = CallContext.Current.UserId;
                var FilePath = files != null ? String.IsNullOrEmpty(files.FilePath) ? "" : files.FilePath : "";
                var rs = model.Save(userId, null, null, FilePath );
                RenderResponse(context, new ResultStatus() { result = true, message = "İşlem Başarılı Bir Şekilde Gerçekleştirildi.", objects = null });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

    }
}
