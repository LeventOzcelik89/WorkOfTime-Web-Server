using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class GeneralHandler : BaseSmartHandler
    {
        public GeneralHandler()
            : base("General")
        {

        }

        [HandleFunction("General/GetUsers")]
        public void GetUsers(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var data = db.GetSH_User();
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
        [HandleFunction("General/CreateLoginPassword")]
        public void CreateLoginPassword(HttpContext context)
        {
            try
            {
                var user = context.Request["username"];
                var password = context.Request["password"];

                if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
                {
                    RenderResponse(context, "Lütfen kullanıcı adı ve şifrenizi düzgün yazınız...");
                    return;
                }

                var userValue = new CryptographyHelper().Encrypt(user);
                var dateTimeDay = DateTime.Now;
                var passwordValue = new CryptographyHelper().Encrypt(password + "_" + DateTime.Now.ToString()); 
                RenderResponse(context, new { username = userValue, password = passwordValue });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}