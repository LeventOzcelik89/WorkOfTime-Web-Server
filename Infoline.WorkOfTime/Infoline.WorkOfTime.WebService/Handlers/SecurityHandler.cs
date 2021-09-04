using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using Infoline.Web.SmartHandlers;
using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Newtonsoft.Json;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.Agent.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class SecurityHandler : BaseSmartHandler
    {
        public SecurityHandler()
            : base("security", new string[] { "cmd" })
        {

        }
        public override void ProcessRequest(HttpContext context, IDictionary<string, object> paramters)
        {

            var _start = DateTime.Now;
            var cmd = paramters["cmd"].ToString().ToLower(System.Globalization.CultureInfo.InvariantCulture);

            var func = typeof(SecurityHandler).GetMethods(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                .FirstOrDefault(a => a.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture) == cmd && a.GetParameters().Length == 1);
            if (func != null)
            {
                func.Invoke(this, new[] { context });
            }
        }
        enum LoginResult
        {
            None = 0,
            InvalidUser,
            InvalidPassword,
            Success
        }
        void Logout()
        {
            if (CallContext.IsReady)
            {
                CallContext.Current.Logout();
            }
        }
        void Login(HttpContext context)
        {
            Login loginpost = null;
            MB_MobileDevice postedDevice = null;

            if (HttpContext.Current.Request.HttpMethod != "POST")
            {
                RenderResponse(context, new ResultStatus { result = false, objects = null, message = "Method is not POST" });
                return;
            }

            try
            {
                loginpost = ParseRequest<Login>(context);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Hatalı login nesnesi." + ex.Message });
                return;
            }

            try
            {
                if (!string.IsNullOrEmpty(loginpost.device))
                {
                    postedDevice = JsonConvert.DeserializeObject<MB_MobileDevice>(new CryptographyHelper().Decrypt(loginpost?.device));
                    if (postedDevice != null)
                    {
                        var db = new WorkOfTimeDatabase();
                        var device = db.GetMB_MobileDeviceByDevice(postedDevice);
                        if (device == null)
                        {
                            db.InsertMB_MobileDevice(postedDevice);
                        }
                        else
                        {
                            postedDevice = device;
                        }
                    }
                }
            }
            catch { }

            try
            {
                var userName = new CryptographyHelper().Decrypt(loginpost.username);
                var passwordBeforeSplit = new CryptographyHelper().Decrypt(loginpost.password).Split('_');
                var password = passwordBeforeSplit[0];
                var clientTime = Convert.ToDateTime(passwordBeforeSplit[1]);

                if (clientTime.Day == DateTime.Now.Day)
                {
                    var db = new WorkOfTimeDatabase();
                    var user = db.GetVWSH_UserByLoginName(userName);
                    var resultStatus = new ResultStatus { result = false, };
                    if (user == null)
                    {
                        resultStatus.message = "Kullanıcı bulunmamaktadır, Lütfen bilgilerinizi kontrol ediniz.";
                        RenderResponse(context, resultStatus);
                        return;
                    }
                    //&& (user.type == (int)EnumSH_UserType.MyPerson && user.IsWorking == true) || user.id == Guid.Empty
                    if (user != null)
                    {
                        var result = Application.Current.SecurityService.Login(userName, password, postedDevice?.id, context.Request.UserHostAddress);
                        if (result == Infoline.LoginResult.OK)
                        {
                            resultStatus.result = true;
                            resultStatus.message = "TicketId is Lives";
                            resultStatus.objects = new ResultLogin
                            {
                                TicketId = CallContext.Current.TicketId,
                                DeviceId = postedDevice?.id
                            };
                        }
                        else if (result == Infoline.LoginResult.InvalidPassword)
                        {
                            resultStatus.message = "Şifreniz kullanıcı adınız ile eşleşmemektedir, Lütfen bilgilerinizi kontrol ediniz.";
                        }
                        else if (result == Infoline.LoginResult.InvalidUser)
                        {
                            resultStatus.message = "Kullanıcı bulunmamaktadır, Lütfen bilgilerinizi kontrol ediniz.";
                        }
                        else if (result == Infoline.LoginResult.InvalidUser)
                        {
                            resultStatus.message = "Hesabınız kapatılmıştır. İletişim üzerinden iletişime geçiniz.";
                        }
                        else if (result == Infoline.LoginResult.RequiresPasswordChage)
                        {
                            resultStatus.message = "Şifre değişikliğine ihtiyaç vardır. Daha sonra tekrar deneyiniz.";
                        }
                        else
                        {
                            resultStatus.message = result.ToString();
                        }

                    }
                    else
                    {
                        resultStatus.message = "Kullanıcı bulunmamaktadır, Lütfen bilgilerinizi kontrol ediniz.";
                    }

                    RenderResponse(context, resultStatus);
                }
                else
                {
                    context.Response.StatusCode = 401;
                    context.Response.End();
                }
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Bir sorun oluştu." + ex.Message });
            }
        }
        void LoginCustomer(HttpContext context)
        {
            Login loginpost = null;
            MB_MobileDevice postedDevice = null;

            if (HttpContext.Current.Request.HttpMethod != "POST")
            {
                RenderResponse(context, new ResultStatus { result = false, objects = null, message = "Method is not POST" });
                return;
            }

            try
            {
                loginpost = ParseRequest<Login>(context);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Hatalı login nesnesi." + ex.Message });
                return;
            }

            try
            {
                postedDevice = JsonConvert.DeserializeObject<MB_MobileDevice>(new CryptographyHelper().Decrypt(loginpost?.device));
                if (postedDevice != null)
                {
                    var db = new WorkOfTimeDatabase();
                    var device = db.GetMB_MobileDeviceByDevice(postedDevice);
                    if (device == null)
                    {
                        db.InsertMB_MobileDevice(postedDevice);
                    }
                    else
                    {
                        postedDevice = device;
                    }
                }
            }
            catch { }

            try
            {
                var userName = new CryptographyHelper().Decrypt(loginpost.username);
                var passwordBeforeSplit = new CryptographyHelper().Decrypt(loginpost.password).Split('_');
                var password = passwordBeforeSplit[0];
                var clientTime = Convert.ToDateTime(passwordBeforeSplit[1]);

                if (clientTime.Day == DateTime.Now.Day)
                {
                    var db = new WorkOfTimeDatabase();
                    var user = db.GetVWSH_UserByLoginName(userName);
                    var resultStatus = new ResultStatus { result = false, };

                    if (user == null)
                    {
                        resultStatus.message = "Kullanıcı bulunmamaktadır, Lütfen bilgilerinizi kontrol ediniz.";
                        RenderResponse(context, resultStatus);
                        return;
                    }

                    if (user != null && (user.type == (int)EnumSH_UserType.OtherPerson && user.IsWorking == true) || user.id == Guid.Empty)
                    {
                        var result = Application.Current.SecurityService.Login(userName, password, postedDevice?.id, context.Request.UserHostAddress);
                        if (result == Infoline.LoginResult.OK)
                        {
                            resultStatus.result = true;
                            resultStatus.message = "TicketId is Lives";
                            resultStatus.objects = new ResultLogin
                            {
                                TicketId = CallContext.Current.TicketId,
                                DeviceId = postedDevice?.id
                            };
                        }
                        else if (result == Infoline.LoginResult.InvalidPassword)
                        {
                            resultStatus.message = "Şifreniz kullanıcı adınız ile eşleşmemektedir, Lütfen bilgilerinizi kontrol ediniz.";
                        }
                        else if (result == Infoline.LoginResult.InvalidUser)
                        {
                            resultStatus.message = "Kullanıcı bulunmamaktadır, Lütfen bilgilerinizi kontrol ediniz.";
                        }
                        else if (result == Infoline.LoginResult.InvalidUser)
                        {
                            resultStatus.message = "Hesabınız kapatılmıştır. İletişim üzerinden iletişime geçiniz.";
                        }
                        else if (result == Infoline.LoginResult.RequiresPasswordChage)
                        {
                            resultStatus.message = "Şifre değişikliğine ihtiyaç vardır. Daha sonra tekrar deneyiniz.";
                        }
                        else
                        {
                            resultStatus.message = result.ToString();
                        }

                    }
                    else
                    {
                        resultStatus.message = "Kullanıcı bulunmamaktadır, Lütfen bilgilerinizi kontrol ediniz.";
                    }

                    RenderResponse(context, resultStatus);
                }
                else
                {
                    context.Response.StatusCode = 401;
                    context.Response.End();
                }
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Bir sorun oluştu." + ex.Message });
            }
        }
        void LoginStudent(HttpContext context)
        {
            Login loginpost = null;
            MB_MobileDevice postedDevice = null;

            if (HttpContext.Current.Request.HttpMethod != "POST")
            {
                RenderResponse(context, new ResultStatus { result = false, objects = null, message = "Method is not POST" });
                return;
            }

            try
            {
                loginpost = ParseRequest<Login>(context);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Hatalı login nesnesi." + ex.Message });
                return;
            }

            try
            {
                postedDevice = JsonConvert.DeserializeObject<MB_MobileDevice>(new CryptographyHelper().Decrypt(loginpost?.device));
                if (postedDevice != null)
                {
                    var db = new WorkOfTimeDatabase();
                    var device = db.GetMB_MobileDeviceByDevice(postedDevice);
                    if (device == null)
                    {
                        db.InsertMB_MobileDevice(postedDevice);
                    }
                    else
                    {
                        postedDevice = device;
                    }
                }
            }
            catch { }

            try
            {
                var userName = new CryptographyHelper().Decrypt(loginpost.username);
                var passwordBeforeSplit = new CryptographyHelper().Decrypt(loginpost.password).Split('_');
                var password = passwordBeforeSplit[0];
                var clientTime = Convert.ToDateTime(passwordBeforeSplit[1]);

                if (clientTime.Day == DateTime.Now.Day)
                {
                    var db = new WorkOfTimeDatabase();
                    var resultStatus = new ResultStatus { result = false, };
                    context.Response.StatusCode = 401;
                    context.Response.End();
                }
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Bir sorun oluştu." + ex.Message });
            }
        }
        void SignUp(HttpContext context)
        {
            RenderResponse(context, new ResultStatus
            {
                result = false,
                message = "Web Uygulamasından personel oluşturabilirsiniz."
            });
        }
        void ForgotPassword(HttpContext context)
        {
            //TCKimlik Numarası ve Email İle geliyorlar
            try
            {
                var _user = ParseRequest<SH_User>(context);
                if (_user.email == null || _user.loginname == null)
                {
                    RenderResponse(context, new ResultStatus
                    {
                        result = false,
                        message = "Eksik Parametre Girildi. Gerekli Parametreler : Email, TC Kimlik No "
                    });
                    return;
                }

                var db = new WorkOfTimeDatabase();
                var user = db.GetVWSH_UserByLoginName(_user.loginname);

                if (user == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Sistemde tckimlikno ile kayıtlı kullanıcı bulunamadı." });
                    return;
                }

                if (user.email.ToLower() != _user.email.ToLower())
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Girilen bilgiler uyuşmuyor." });
                    return;
                }

                var password = RandomPassword();
                var md5Password = db.GetMd5Hash(db.GetMd5Hash(password));
                var dbres = db.UpdateSH_User(new SH_User
                {
                    id = user.id,
                    password = md5Password,
                    status = user.status,
                    changed = DateTime.Now
                });

                if (!dbres.result)
                {
                    RenderResponse(context, dbres.message);
                    return;
                }
                var tenantName = TenantConfig.Tenant.TenantName;
                var mesaj = "<p>Şifreniz güncellenmiştir." + "</p>";
                mesaj += "<p>Kullanıcı Adınız : " + user.loginname + "</p>";
                mesaj += "<p>Yeni şifreniz : " + password + "</p>";

                new Email().Template("Template1", "working.jpg", "Şifre Talebi Hakkında..", mesaj)
                .Send((Int16)EmailSendTypes.ZorunluMailler, user.email, string.Format("{0} | {1}", tenantName + " | WorkOfTime", "Şifre Talebi Hakkında.."), true);

                RenderResponse(context, new ResultStatus { result = true, message = "Geçici şifreniz mail adresinize gönderilmiştir." });
                return;

            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message });
            }
        }


        private string RandomPassword()
        {
            var random = new Random();
            string randonValue = "12345647890";
            string result = "";
            for (var i = 0; i < 10; i++)
            {
                result += randonValue[random.Next(randonValue.Length)];
            }
            return result;
        }
    }

    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
        public string device { get; set; }

    }

    public class ResultLogin
    {
        public Guid TicketId { get; set; }
        public Guid? DeviceId { get; set; }
    }

}