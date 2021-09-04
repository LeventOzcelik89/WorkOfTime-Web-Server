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

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class CalendarModel
	{
		public Guid id { get; set; }
		public Guid createdby { get; set; }
		public DateTime? start { get; set; }
		public DateTime? end { get; set; }
		public int? type { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string katilimcilar { get; set; }
		public string color { get; set; }
		public List<CalendarModel> Load(Guid userId, DateTime start, DateTime end, int? type, string email = "")
		{
			var db = new WorkOfTimeDatabase();
			var list = new List<CalendarModel>();
			var personIds = db.GetVWINV_CompanyPersonDepartments().Where(a => (a.IdUser == userId || a.Manager1 == userId || a.Manager2 == userId) && a.OrganizationType == (Int16)EnumINV_CompanyDepartmentsType.Organization).Select(a => a.IdUser).Distinct();
			if (userId != Guid.Empty)
			{
				personIds = personIds.Where(x => x.Value != Guid.Empty).Distinct();
			}
			var blockUserCalendar = db.GetSYS_BlockCalendarByUserId(userId);
			var _calendarnew = db.VWINV_CompanyPersonCalendarDistinct(userId, start, end);

			var enumDatas = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumINV_CompanyPersonCalendarType>().ToArray();
			if (type.HasValue)
			{
				enumDatas = enumDatas.Where(x => x.Key == type.ToString()).ToArray();
			}

			foreach (var enumData in enumDatas)
			{
				if (blockUserCalendar.Count(x => x.type == Convert.ToInt32(enumData.Key)) == 0)
				{
					switch ((EnumINV_CompanyPersonCalendarType)Convert.ToInt32(enumData.Key))
					{
						case EnumINV_CompanyPersonCalendarType.Resmi:
							var officials = db.GetVWINV_PermitOfficialByCalendar(start, end);
							list.AddRange(officials.Select(c => new CalendarModel
							{
								id = c.id,
								start = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.Resmi, c.StartDate, start),
								description = "-",
								end = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.Resmi, c.EndDate, start),
								type = (int)EnumINV_CompanyPersonCalendarType.Resmi,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Resmi),
								katilimcilar = null,
								title = (c.Type_Title != null ? c.Type_Title : "")
							}));
							break;
						case EnumINV_CompanyPersonCalendarType.Genel:
							var datePermits = db.GetVWINV_PermitByDateRange(start, end);
							var permits = datePermits.Where(a => personIds.Contains(a.IdUser) && a.ApproveStatus == (int)EnumINV_PermitApproveStatus.IslakImzaYuklendi ||
																								 a.ApproveStatus == (int)EnumINV_PermitApproveStatus.IkKontrol ||
																								 a.ApproveStatus == (int)EnumINV_PermitApproveStatus.AvansIzin ||
																								 a.ApproveStatus == (int)EnumINV_PermitApproveStatus.GecmisIzin ||
																								 a.ApproveStatus == (int)EnumINV_PermitApproveStatus.SaglikRaporu).ToArray();

							list.AddRange(permits.Select(c => new CalendarModel
							{
								id = c.id,
								start = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.Genel, c.StartDate, start),
								description = "-",
								end = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.Genel, c.EndDate, start),
								type = (int)EnumINV_CompanyPersonCalendarType.Genel,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Genel),
								katilimcilar = null,
								title = (c.PermitType_Title != null ? c.PermitType_Title : "") + " : " + (c.Person_Title != null ? c.Person_Title : "")
							}));

							break;
						case EnumINV_CompanyPersonCalendarType.IsGirisCikis:
							break;
						case EnumINV_CompanyPersonCalendarType.DepartmanGirisCikis:
							break;
						case EnumINV_CompanyPersonCalendarType.DogumGun:
							var birthdays = db.GetSH_UserByCalendar(start, end).Where(c => c.birthday != null).ToArray();
							list.AddRange(birthdays.Select(c => new CalendarModel
							{
								id = c.id,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.DogumGun),
								start = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.DogumGun, c.birthday, DateTime.Now),
								end = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.DogumGun, c.birthday, DateTime.Now),
								title = "Doğum Günü : " + (c.FullName != null ? c.FullName : ""),
								description = (c.Company_Title != null ? c.Company_Title : "") + " şirketindeki " + (c.FullName != null ? c.FullName : "") + " doğum günü",
								type = (int)EnumINV_CompanyPersonCalendarType.DogumGun,
								katilimcilar = null,
							}));
							break;
						case EnumINV_CompanyPersonCalendarType.Proje:
							var projects = db.GetVWPRJ_ProjectByCalendar(start, end);
							list.AddRange(projects.Select(c => new CalendarModel
							{
								id = c.id,
								start = c.ProjectStartDate,
								end = new DateTime(c.ProjectStartDate.Value.Year, c.ProjectStartDate.Value.Month, c.ProjectStartDate.Value.Day, 23, 59, 00),
								description = (c.Company_Title != null ? c.Company_Title : "") + " şirketinde " + (c.ProjectName != null ? c.ProjectName : "") + " projesi başladı",
								title = "Proje Başlangıcı : " + (c.ProjectName != null ? c.ProjectName : ""),
								type = (int)EnumINV_CompanyPersonCalendarType.Proje,
								katilimcilar = null,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Proje)

							}));
							break;
						case EnumINV_CompanyPersonCalendarType.Gorevlendirme:
							var commissions = db.GetVWINV_CommissionsPersonsByCalendar(start, end).Where(a => personIds.Contains(a.IdUser) && a.ApproveStatus != (int)EnumINV_CommissionsApproveStatus.Bekliyor && a.ApproveStatus != (int)EnumINV_CommissionsApproveStatus.Reddedildi).ToArray();
							list.AddRange(commissions.Select(c => new CalendarModel
							{
								id = c.id,
								start = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.Gorevlendirme, c.StartDate, start),
								description = (c.Information_Title != null ? c.Information_Title : "") + " amaçlı görevlendirme için " + (c.TravelInformation_Title != null ? c.TravelInformation_Title : "") + " ile " + (c.Manager1Approval_Title != null ? c.Manager1Approval_Title : "") + " onayı dahilinde gidilecektir.",
								end = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.Genel, c.EndDate, start),
								type = (int)EnumINV_CompanyPersonCalendarType.Gorevlendirme,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Gorevlendirme),
								katilimcilar = null,
								title = "Görevlendirme : " + (c.Person_Title != null ? c.Person_Title : "")
							}));

							break;
						case EnumINV_CompanyPersonCalendarType.Saglik:
							//var  = db.GetVWINV_PermitByApproveStatusHealtyPermit(start, end);
							break;
						case EnumINV_CompanyPersonCalendarType.Hatirlatma:
							var remindings = _calendarnew.Where(v => v.Type == Convert.ToInt32(enumData.Key)).ToArray();
							list.AddRange(remindings.Select(c => new CalendarModel
							{
								id = c.id,
								description = c.Description,
								katilimcilar = c.Katilimcilar,
								start = c.Start,
								end = c.End,
								title = c.Title,
								type = c.Type,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Hatirlatma)
							}));
							break;
						case EnumINV_CompanyPersonCalendarType.Not:
							var note = _calendarnew.Where(v => v.Type == Convert.ToInt32(enumData.Key)).ToArray();
							list.AddRange(note.Select(c => new CalendarModel
							{
								id = c.id,
								description = c.Description,
								katilimcilar = c.Katilimcilar,
								start = c.Start,
								end = c.End,
								title = c.Title,
								type = c.Type,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Not)
							}));
							break;
						case EnumINV_CompanyPersonCalendarType.Toplanti:
							break;
						case EnumINV_CompanyPersonCalendarType.Duyuru:
							var announcements = _calendarnew.Where(v => v.Type == Convert.ToInt32(enumData.Key)).ToArray();
							list.AddRange(announcements.Select(c => new CalendarModel
							{
								id = c.id,
								description = c.Description,
								katilimcilar = c.Katilimcilar,
								start = c.Start,
								end = c.End,
								title = c.Title,
								type = c.Type,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Duyuru)
							}));
							break;
						case EnumINV_CompanyPersonCalendarType.Tebrik:
							var greetings = _calendarnew.Where(v => v.Type == Convert.ToInt32(enumData.Key)).ToArray();
							list.AddRange(greetings.Select(c => new CalendarModel
							{
								id = c.id,
								description = c.Description,
								katilimcilar = c.Katilimcilar,
								start = c.Start,
								end = c.End,
								title = c.Title,
								type = c.Type,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Tebrik)
							}));
							break;
						case EnumINV_CompanyPersonCalendarType.Sunum:
							break;
						case EnumINV_CompanyPersonCalendarType.Etkinlik:
							var activity = _calendarnew.Where(v => v.Type == Convert.ToInt32(enumData.Key)).ToArray();
							list.AddRange(activity.Select(c => new CalendarModel
							{
								id = c.id,
								description = c.Description,
								katilimcilar = c.Katilimcilar,
								start = c.Start,
								end = c.End,
								title = c.Title,
								type = c.Type,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Etkinlik)
							}));
							break;
						case EnumINV_CompanyPersonCalendarType.IsGorusmesi:
							var plans = db.GetVWHR_PlanPersonByCalendar(start, end);
							list.AddRange(plans.Select(c => new CalendarModel
							{
								id = c.id,
								start = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.IsGorusmesi, c.PlanDate, start),
								end = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.IsGorusmesi, c.PlanDate, start),
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.IsGorusmesi),
								description = c.Description + " / " + (c.Result != (int)EnumHR_PlanResult.Gorusulmedi ? (c.Result_Title != null ? "Sonuç :" + c.Result_Title : "") : ""),
								katilimcilar = (c.UserId_Title != null ? c.UserId_Title : "") + ", " + (c.HRPerson_Title != null ? c.HRPerson_Title : ""),
								title = "İş Mülakatı : " + (c.UserId_Title != null ? c.UserId_Title : "") + " (" + (c.HRPerson_Title != null ? c.HRPerson_Title : "") + ")",
								type = (int)EnumINV_CompanyPersonCalendarType.IsGorusmesi
							}));
							break;
						case EnumINV_CompanyPersonCalendarType.Gant:
							var projectTimeline = db.GetVWPRJ_ProjectTimelineByCalendar(start, end);
							list.AddRange(projectTimeline.Select(c => new CalendarModel
							{
								id = c.id,
								start = c.StartDate,
								end = c.StartDate,
								description = c.Description,
								title = c.Name + " (" + c.Project_Title + ")",
								type = (int)EnumINV_CompanyPersonCalendarType.Gant,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Gant)
							}));
							break;
						case EnumINV_CompanyPersonCalendarType.SatisToplantisi:
							var newList = new List<DataContactAndUser>();
							var _contactType8 = db.GetVWCRM_ContactByCalendar(start, end);
							var _contactType8User = db.GetVWCRM_ContactUserByContactIds(_contactType8.Select(c => c.id).ToArray()).Where(x => personIds.Contains(x.UserId));
							if (_contactType8User.Count() != 0)
							{
								foreach (var item in _contactType8)
								{
									var users = string.Join(",", _contactType8User.Where(c => c.id == item.id).Select(c => c.User_Title));
									if (!string.IsNullOrEmpty(users))
									{
										newList.Add(new DataContactAndUser() { Contact = item, ContactUser = users });
									}
								}
							}
							list.AddRange(newList.Select(c => new CalendarModel
							{
								id = c.Contact.id,
								start = c.Contact.ContactStartDate,
								end = c.Contact.ContactEndDate,
								katilimcilar = c.ContactUser,
								title = "Potansiyel Toplantısı : " + (c.Contact.CustomerCompanyId_Title != null ? c.Contact.CustomerCompanyId_Title : ""),
								description = c.Contact.Description,
								type = (int)EnumINV_CompanyPersonCalendarType.SatisToplantisi,
								color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.SatisToplantisi)
							}));

							break;
						case EnumINV_CompanyPersonCalendarType.NewProjectPerson:
							break;
						//case EnumINV_CompanyPersonCalendarType.Outlook:
						//    var userEmail = "";
						//    var userEmailPassword = "";
						//    if (email == null || email == "")
						//    {
						//        var userEmailAccountData = db.GetSH_UserEmailAccountByUserIdİsDefaultTrue(userId);
						//        if (userEmailAccountData != null)
						//        {
						//            userEmail = userEmailAccountData.email;
						//            userEmailPassword = PasswordDecrypt(userEmailAccountData.password);
						//        }
						//    }
						//    else
						//    {
						//        var userEmailAccountByEmailData = db.GetSH_UserEmailAccountByEmail(email);
						//        if (userEmailAccountByEmailData != null)
						//        {
						//            userEmail = userEmailAccountByEmailData.email;
						//            userEmailPassword = PasswordDecrypt(userEmailAccountByEmailData.password);
						//        }
						//    }

						//    var lists = CalendarDatas(userEmail, userEmailPassword, start, end);
						//    if (lists.value != null)
						//    {
						//        var data = lists.value.Select(a => new CalendarModel
						//        {
						//            id = Guid.NewGuid(),
						//            description = a.bodyPreview,
						//            katilimcilar = string.Join(", ", a.attendees.Where(b => b.type == "required").Select(x => (x.emailAddress.name != "" ? x.emailAddress.name : x.emailAddress.address))),
						//            start = DateTimeProses(100, (a.start.dateTime.Hour == 0 && a.start.dateTime.Minute == 0 ? a.start.dateTime : a.start.dateTime.AddHours(3)), start),
						//            end = DateTimeProses(100, (a.end.dateTime.Hour == 0 && a.end.dateTime.Minute == 0 ? a.end.dateTime : a.end.dateTime.AddHours(3)), end),
						//            title = a.subject,
						//            color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Gorevlendirme),
						//            type = 50
						//        }).ToList();
						//        list.AddRange(data);
						//    }
						//    break;
						default:
							break;
					}
				}
			}
			return list;
		}


		public List<CalendarModel> CertificateLoad(Guid userId, DateTime start, DateTime end, int? type, string email = "")
		{
			var db = new WorkOfTimeDatabase();
			var list = new List<CalendarModel>();
			var personCertificates = new List<VWSH_PersonCertificate>();

			var myPersons = db.GetVWSH_UserMyPerson();
			if (myPersons.Count() > 0)
			{
				personCertificates = db.GetVWSH_PersonCertificateByUserIds(myPersons.Select(x => x.id).ToArray()).ToList();
			}


			list.AddRange(personCertificates.Where(x => x.ExpirationDate.HasValue).Select(c => new CalendarModel
			{
				id = c.id,
				start = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.Sertifika, c.ExpirationDate, start),
				description = c.CertificateType_Title + " sertifikasının süresi " + c.ExpirationDate.Value.ToString("dd/MM/yyyy") + " Tarihinde " + (c.ExpirationDate.Value < DateTime.Now ? "  dolmuştur" : " sona ermektedir."),
				end = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.Sertifika, c.ExpirationDate, start),
				type = (int)EnumINV_CompanyPersonCalendarType.Resmi,
				color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Sertifika),
				title = c.User_Title
			}));


			var dolanlar = personCertificates.Where(x => x.ExpirationDate.HasValue && x.ExpirationDate < DateTime.Now).ToArray();

			list.AddRange(dolanlar.Select(c => new CalendarModel {
				id = c.id,
				start = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.Sertifika, null, start),
				description = c.CertificateType_Title + " sertifikasının süresi " + c.ExpirationDate.Value.ToString("dd/MM/yyyy") + " tarihinde dolmuştur.",
				end = DateTimeProses((int)EnumINV_CompanyPersonCalendarType.Sertifika, null, start),
				type = (int)EnumINV_CompanyPersonCalendarType.Resmi,
				color = GetTypeColor((int)EnumINV_CompanyPersonCalendarType.Sertifika),
				title = c.User_Title
			}));


			return list;
		}


		public ValueData CalendarDatas(string userName, string password, DateTime start, DateTime end)
		{
			var tokenData = Token(userName, password);
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

		private AccessToken Token(string username, string password)
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
			request.AddParameter("userName", username);
			request.AddParameter("password", password);
			IRestResponse response = client.Execute(request);
			return JsonConvert.DeserializeObject<AccessToken>(response.Content);
		}

		public DateTime DateTimeProses(int? type, DateTime? dt, DateTime CalendarDate)
		{
			if (!dt.HasValue)
				return DateTime.Now;

			if (type == (Int32)EnumINV_CompanyPersonCalendarType.DogumGun)
				return dt.Value.AddYears(CalendarDate.Year - dt.Value.Year);

			if (GetallDay(type))
			{
				return dt.Value;
			}

			return dt.Value;
		}

		private bool GetallDay(int? t) => t.HasValue && new[] { 100, 102, 103, 104 }.Contains(t.Value);

		public string GetTypeColor(int? type)
		{
			if (type.HasValue == false)
			{
				return "#A4243B";
			}
			var rs = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumINV_CompanyPersonCalendarType>().Where(
				x => Convert.ToInt32(x.Key) == (Int16)(EnumINV_CompanyPersonCalendarType)type.Value)
				.FirstOrDefault();
			if (rs.Key != null)
			{
				return "#" + rs.Generic["color"];
			}
			return "#A4243B";
		}

		public string PasswordDecrypt(string text)
		{
			CryptographyHelper cryptography = new CryptographyHelper();

			return cryptography.Decrypt(text);
		}
	}


	public class DataContactAndUser
	{
		public VWCRM_Contact Contact { get; set; }
		public string ContactUser { get; set; }
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

	public class MyCalendar
	{
		public Guid id { get; set; }
		public Guid createdby { get; set; }
		public DateTime? start { get; set; }
		public DateTime? end { get; set; }
		public int? type { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string katilimcilar { get; set; }
		public string color { get; set; }
	}
}
