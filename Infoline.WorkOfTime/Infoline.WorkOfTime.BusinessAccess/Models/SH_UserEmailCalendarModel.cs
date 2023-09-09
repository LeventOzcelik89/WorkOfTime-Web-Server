using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessData;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess.Models
{
    public class EmailCalendar
    {
        public Guid id { get; set; }
        public Guid createdby { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public int? type { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string katilimcilar { get; set; }
        public string participantInfoMessage { get; set; }
        public string location { get; set; }
        public string opsiyonelKatilimcilar { get; set; }
        public string color { get; set; }
    }

    public class SH_UserEmailCalendarModel
    {
        public EmailCalendar[] GetCalendarEvents(Guid userId, DateTime start, DateTime end, string email)
        {
            var db = new WorkOfTimeDatabase();
            var userEmail = "";
            var userEmailPassword = "";
            if (email == null || email == "")
            {
                var userEmailAccountData = db.GetSH_UserEmailAccountByUserIdİsDefaultTrue(userId);
                if (userEmailAccountData != null)
                {
                    userEmail = userEmailAccountData.email;
                    userEmailPassword = PasswordDecrypt(userEmailAccountData.password);
                }
            }
            else
            {
                var userEmailAccountByEmailData = db.GetSH_UserEmailAccountByEmail(email);
                if (userEmailAccountByEmailData != null)
                {
                    userEmail = userEmailAccountByEmailData.email;
                    userEmailPassword = PasswordDecrypt(userEmailAccountByEmailData.password);
                }
            }
            var list = CalendarDatas(userEmail, userEmailPassword, start, end);

            if (list.value == null)
            {
                return null;
            }

            var data = list.value.Select(a => new EmailCalendar
            {
                id = Guid.NewGuid(),
                start = DateTimeProses(100, (a.start.dateTime.Hour == 0 && a.start.dateTime.Minute == 0 ? a.start.dateTime : a.start.dateTime.AddHours(3)), start),
                end = DateTimeProses(100, (a.end.dateTime.Hour == 0 && a.end.dateTime.Minute == 0 ? a.end.dateTime : a.end.dateTime.AddHours(3)), end),
                type = 100,
                title = a.subject,
                description = a.bodyPreview,
                location = a.location.displayName,
                participantInfoMessage = "Katılımcı Yanıtları : " + a.attendees.Where(x => x.status.response == "accepted").Count() + " kişi kabul etti, " + a.attendees.Where(x => x.status.response == "none").Count() + " kişiden dönüş bekleniyor, " + a.attendees.Where(x => x.status.response == "declined").Count() + " kişi reddetti.",
                katilimcilar = string.Join(", ", a.attendees.Where(b => b.type == "required").Select(x => (x.emailAddress.name != "" ? x.emailAddress.name : x.emailAddress.address))),
                opsiyonelKatilimcilar = string.Join(", ", a.attendees.Where(b => b.type == "optional").Select(x => (x.emailAddress.name != "" ? x.emailAddress.name : x.emailAddress.address))),
                color = GetTypeColor(100),
                createdby = new Guid("99999999-1234-5678-0000-999999999999")
            }).OrderBy(a => a.start).ToArray();

            return data.ToArray();
        }

        private DateTime DateTimeProses(int? type, DateTime? dt, DateTime CalendarDate)
        {
            if (!dt.HasValue)
                return DateTime.Now;

            if (GetallDay(type))
            {
                return dt.Value;
            }

            return dt.Value;
        }

        public string GetTypeColor(int? type)
        {
            if (type.HasValue == false)
            {
                return "#A4243B";
            }
            var rs = CalendarHeaDictionary.Where(
                x => (EnumINV_CompanyPersonCalendarType)x.Key == (EnumINV_CompanyPersonCalendarType)type.Value)
                .FirstOrDefault();
            if (rs.Key != null)
            {
                return rs.Value;
            }
            return "#A4243B";
        }

        public string PasswordDecrypt(string text)
        {
            CryptographyHelper cryptography = new CryptographyHelper();

            return cryptography.Decrypt(text);
        }

        private bool GetallDay(int? t) => t.HasValue && new[] { 100, 102, 103, 104 }.Contains(t.Value);

        public Dictionary<Enum, string> CalendarHeaDictionary = new Dictionary<Enum, string>()
        {
            {EnumINV_CompanyPersonCalendarType.Resmi, "#A4243B"},
            {EnumINV_CompanyPersonCalendarType.Genel,"#D8973C" },
            {EnumINV_CompanyPersonCalendarType.Saglik,"#33cc33" },
            {EnumINV_CompanyPersonCalendarType.IsGirisCikis,"#00BEFF" },
            {EnumINV_CompanyPersonCalendarType.DepartmanGirisCikis,"#ED6A5E" },
            {EnumINV_CompanyPersonCalendarType.DogumGun,"#048A81" },
            {EnumINV_CompanyPersonCalendarType.Proje,"#FF0000" },
            {EnumINV_CompanyPersonCalendarType.Gorevlendirme,"#ff9966" },
            {EnumINV_CompanyPersonCalendarType.Hatirlatma,"#3F51B5" },
            {EnumINV_CompanyPersonCalendarType.Not,"#7db113" },
            {EnumINV_CompanyPersonCalendarType.Toplanti,"#263949" },
            {EnumINV_CompanyPersonCalendarType.Duyuru,"#ff8800" },
            {EnumINV_CompanyPersonCalendarType.Tebrik,"#ff6a9d" },
            {EnumINV_CompanyPersonCalendarType.Sunum,"#23465c" },
            {EnumINV_CompanyPersonCalendarType.SatisToplantisi,"#ffd700" },
            {EnumINV_CompanyPersonCalendarType.Etkinlik,"#5e4474" },
        };


        public ValueData CalendarDatas(string username, string password, DateTime start, DateTime end)
        {
            var tokenData = Token(username, password);
            var startTime = start.Year + "-" + start.Month + "-" + start.Day;
            var endTime = end.Year + "-" + end.Month + "-" + end.Day;
            var clientString = "https://graph.microsoft.com/beta/me/calendarview?startDateTime=" + startTime + "&endDateTime=" + endTime + "&%24$orderby=start/dateTime%20DESC'&%24top=1000";
            var client = new RestClient(clientString);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + tokenData.access_token + "");
            request.AddParameter("text/plain", "", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var data = JsonConvert.DeserializeObject<ValueData>(response.Content);

            if (data == null)
            {
                return null;
            }

            return data;
        }


        private AccessToken Token(string userName, string password)
        {
            var client = new RestClient("https://login.microsoftonline.com/0967f155-3fa6-4fbc-95d2-7dfd19fdf815/oauth2/v2.0/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("SdkVersion", "postman-graph/v1.0");
            request.AddHeader("Cookie", "buid=0.AAAAf1VENWW7P0eOxKrHEXAyoAAAAAAAAAAAAAAAAAAAAABzAAA.AQABAAEAAABeStGSRwwnTq2vHplZ9KL46JaIviaHyy6BbQxAP1mbdrhHUaH02Mab9olvqxRdSgVPQ_ywFBtmRG6BSN6N_gwJO6zkbfZTOz4HUGoCLz8I9acaODBD-eDGN7s6N60MpaIgAA; x-ms-gateway-slice=prod; stsservicecookie=ests; fpc=Aqw9sMX0MedLkCnt4xK4iemDf9bWAgAAADp0mNcOAAAAEkoxtwEAAABOdZjXDgAAAKJTtxMCAAAAy3eY1w4AAAA");
            request.AddParameter("grant_type", "password");
            request.AddParameter("client_id", "b682ff9e-b160-45ab-82f8-c9eb264ea76d");
            request.AddParameter("client_secret", "6l.232-4eFpB5NtM.jv1h17ndZWQH6X.e_");
            request.AddParameter("scope", "https://graph.microsoft.com/.default");
            request.AddParameter("userName", userName);
            request.AddParameter("password", password);
            IRestResponse response = client.Execute(request);
            return JsonConvert.DeserializeObject<AccessToken>(response.Content);

        }


        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity pageSecurity)
        {
            BEXP filter = null;

            filter |= new BEXP
            {
                Operand1 = (COL)"userid",
                Operator = BinaryOperator.Equal,
                Operand2 = (VAL)pageSecurity.user.id
            };

            query.Filter &= filter;

            return query;
        }


    }

    public class body
    {
        public string contentType { get; set; }
        public string content { get; set; }
    }

    public class start
    {
        public DateTime dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class end
    {
        public DateTime dateTime { get; set; }
        public string timeZone { get; set; }
    }

    public class Address
    {
    }

    public class Coordinates
    {
    }

    public class location
    {
        public string displayName { get; set; }
        public string locationType { get; set; }
        public string uniqueIdType { get; set; }
        public Address address { get; set; }
        public Coordinates coordinates { get; set; }
    }

    public class status
    {
        public string response { get; set; }
        public DateTime time { get; set; }
    }

    public class emailAddress
    {
        public string name { get; set; }
        public string address { get; set; }
    }

    public class attendee
    {
        public string type { get; set; }
        public status status { get; set; }
        public emailAddress emailAddress { get; set; }
    }

    public class organizer
    {
        public emailAddress emailAddress { get; set; }
    }

    public class value
    {
        public string OdataEtag { get; set; }
        public string id { get; set; }
        public string subject { get; set; }
        public string bodyPreview { get; set; }
        public body body { get; set; }
        public start start { get; set; }
        public end end { get; set; }
        public location location { get; set; }
        public List<attendee> attendees { get; set; }
        public organizer organizer { get; set; }
    }

    public class ValueData
    {
        public List<value> value { get; set; }
    }

    public class AccessToken
    {
        public string token_type { get; set; }
        public string scope { get; set; }
        public int expires_in { get; set; }
        public int ext_expires_in { get; set; }
        public string access_token { get; set; }
    }
}
