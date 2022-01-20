using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class Notification
    {
        private WorkOfTimeDatabase db { get; set; }
        private string NotificationUrl { get; set; } = "https://fcm.googleapis.com/fcm/send";
        private string NotificationAuthorizationKey { get; set; } = "AAAAEX9p954:APA91bE5nA9SE1NllSA7x9VrfErrZRtavNeZcSH_sE0wOE-lZZ6zJ1s1NBnv7mQQfl0VO01lLJ8Ix1KK4HSTV2nfYKmbQ1hB5XEJTQf42nyxFKcxIKFsF_YMxHSZNNHsLUxLrv77MXaZ";


        public void NotificationSend(Guid userId, Guid? sendedUser, string titleText, string bodyText, string key_1Data = "", string key_2Data = "", string returnUrl = "", Guid? returnUrlId = null, bool? hasConfirmation = false)
        {
            new Thread(new ThreadStart(delegate
            {
                try
                {
                    this.db = this.db ?? new WorkOfTimeDatabase();
                    var userToken = db.GetSH_UserFireBaseTokenByUserId(userId);
                    if (userToken != null)
                    {
                        var pushData = new
                        {
                            to = userToken.token,
                            collapse_key = "type_a",
                            notification = new
                            {
                                body = bodyText,
                                title = titleText,
                                icon = "push_icon",
                                color = "red",
                                sound = "default"
                            },

                            data = new
                            {
                                key_1 = TenantConfig.Tenant.GetWebUrl() + key_1Data,
                                key_2 = key_2Data
                            }
                        };
                        new VMSYS_NotificationModel
                        {
                            created = DateTime.Now,
                            createdby = sendedUser,
                            userId = userId,
                            message = bodyText,
                            title = titleText,
                            url = key_1Data,
                            paramaters = key_2Data
                        }.Save(sendedUser);
                        using (var clientm = new WebClient())
                        {
                            clientm.Headers.Add("Authorization", "key=" + this.NotificationAuthorizationKey);
                            clientm.Headers.Add("Content-Type", "application/json");
                            clientm.Encoding = System.Text.Encoding.UTF8;
                            var res = clientm.UploadString(this.NotificationUrl, Infoline.Helper.Json.Serialize(pushData));
                            var result = JsonConvert.DeserializeObject(res);
                        }
                    }

                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());

                }
            }

            )).Start();
        }
    }
}
