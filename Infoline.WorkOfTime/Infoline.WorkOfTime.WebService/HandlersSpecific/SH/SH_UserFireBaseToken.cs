using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Web.SmartHandlers;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class SH_UserFireBaseTokenHandler : BaseSmartHandler
    {
        public SH_UserFireBaseTokenHandler()
            : base("SH_UserFireBaseTokenHandler")
        {

        }

        [HandleFunction("SH_UserFireBaseToken/MBInsert")]
        public void SH_UserFireBaseTokenMBInsert(HttpContext context)
        {
            var db = new WorkOfTimeDatabase();
            var userId = CallContext.Current.UserId;

            if(userId == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Kullanıcı bulunamadı." });
                return;
            }

            var token = context.Request["token"];

            if (string.IsNullOrEmpty(token))
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Token bulunamadı." });
                return;
            }

            var userFireBaseToken = new SH_UserFireBaseToken
            {
                userId = userId,
                token = token
            };

            var dbresult = db.InsertSH_UserFireBaseToken(userFireBaseToken);
            RenderResponse(context, new ResultStatus { result = dbresult.result, message = dbresult.result ? "İşlem Başarılı" : "İşlem Başarısız Oldu." });
        }
    }
}