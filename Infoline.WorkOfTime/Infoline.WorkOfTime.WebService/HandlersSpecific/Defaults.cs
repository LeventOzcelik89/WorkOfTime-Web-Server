using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess.Mobile;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Web.SmartHandlers;
using Newtonsoft.Json;
using System;
using System.ComponentModel.Composition;
using System.Web;
using Infoline.WorkOfTime.BusinessAccess;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class DefaultsHandler : BaseSmartHandler
    {
        public DefaultsHandler()
            : base("DefaultsHandler")
        {

        }

        [HandleFunction("Defaults/Summaries")]
        public void DefaultsSummaries(HttpContext context)
        {
            try
            {
                var userId = CallContext.Current.UserId;
                var obj = new Defaults().GetSummaries(userId);
                RenderResponse(context, obj);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message });
            }
        }

        [HandleFunction("Defaults/DataSources")]
        public void DefaultsDataSources(HttpContext context)
        {
            try
            {
                var userId = CallContext.Current.UserId;
                var obj = new Defaults().GetDataSources(userId);
                RenderResponse(context, obj);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message });
            }
        }



        [HandleFunction("Default/GetUserInfo")]
        public void GetUserInfo(HttpContext context)
        {
            try
            {
                var userId = CallContext.Current.UserId;
                var db = new WorkOfTimeDatabase();
                var result = db.GetUserPageSecurityByUserid(userId, CallContext.Current.TicketId);
                RenderResponse(context, result);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message });
            }
        }


        [HandleFunction("Defaults/Config")]
        public void DefaultsConfig(HttpContext context)
        {
            try
            {
                var obj = new Defaults().GetConfigs();
                RenderResponse(context, obj);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message });
            }
        }

        [HandleFunction("Defaults/GetTenantInfo")]
        public void DefaultsGetTenantInfo(HttpContext context)
        {
            try
            {
                var data = new ConfigTenantUrl();
                var tenantCode = context.Request["code"];
                if (string.IsNullOrEmpty(tenantCode))
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Müşteri kodu boş olamaz" });
                    return;
                }
                var remoteCon = ConfigurationManager.AppSettings["RemoteConnection"];
                var connection = new Infoline.Framework.Helper.CryptographyHelper().Decrypt(remoteCon);
                using (var db = new InfolineDatabase(connection, DatabaseType.Mssql))
                {
                    var _tenants = db.ExecuteReader<TEN_Tenant>("select * from TEN_Tenant with(nolock)").Where(x => x.TenantCode == int.Parse(tenantCode)).OrderBy(a => a.TenantCode).FirstOrDefault();
                    if (_tenants == null)
                    {
                        RenderResponse(context, new ResultStatus { result = false, message = "Aradığınız müşteri kodunda bir firma bulunmamaktadır. " });
                        return;
                    }
                    if (_tenants != null)
                    {
                        data.WebURL = _tenants.GetWebUrl();
                        data.WebServiceURL = _tenants.GetWebServiceUrl();
                    }
                }

                RenderResponse(context, new ResultStatus { result = true, objects = data });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message });
            }
        }


        [HandleFunction("Defaults/AToken")]
        public void DefaultsAToken(HttpContext context)
        {

            try
            {
                var loginData = new Models.Login
                {
                    username = "20003789606",
                    password = "123456",
                    device = new MB_MobileDevice
                    {
                        UniqID = "WEB SERVICE HELPER",
                        AppID = "DEVELOPER",
                        AppVersionCode = "1",
                        AppVersionName = "DEVELOPER",
                        Device = "DEVELOPER",
                        Model = "DEVELOPER",
                        OSVersion = "DEVELOPER",
                        Brand = "DEVELOPER",
                        Board = "DEVELOPER",
                        Product = "DEVELOPER",
                        Serial = "DEVELOPER",
                        SDK = "DEVELOPER",
                        ScreenScale = "DEVELOPER",
                        DeviceType = 0
                    }
                };

                var data = Newtonsoft.Json.JsonConvert.SerializeObject(loginData);
                var result = Application.Current.SecurityService.Login(loginData.username, loginData.password, loginData.device.id, "HOST");

                if (result == LoginResult.OK)
                {
                    RenderResponse(context, new CryptographyHelper().Encrypt(JsonConvert.SerializeObject(new Models.AToken
                    {
                        DeviceId = loginData.device.id,
                        TicketId = CallContext.Current.TicketId
                    })));
                }

            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message });
            }

        }
    }
}
