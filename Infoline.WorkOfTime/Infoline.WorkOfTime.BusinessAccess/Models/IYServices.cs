using Infoline.Framework.Database;
using System.Linq;
using System.Net;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class IYServices
	{
		private string _username { get; set; }
		private string _password { get; set; }
		private int _iysCode { get; set; }
		private int _brandCode { get; set; }
		private int _retailerCode { get; set; }
		private string _accessToken { get; set; }
		private string _refreshToken { get; set; }

		//IYSCODE : 671533
		//BRANDCODE : 619391
		//USERNAME : aqloam@bqbnincgkwrzadawztdn.it
		//PASSWORD : 374337
		public IYServices(string username, string password, int iysCode, int brandCode, int retailerCode)
		{
			_username = username;
			_password = password;
			_iysCode = iysCode;
			_brandCode = brandCode;
			_retailerCode = retailerCode;
		}


		public IYS_TakeTokenAndRefreshResponse TakeToken()
		{
			var response = SendRequest<IYS_TakeTokenAndRefreshResponse>("https://api.sandbox.iys.org.tr/oauth/token", Infoline.Helper.Json.Serialize(new { username = _username, password = _password }), MethodType.POST);

			_accessToken = response.objects.result.accessToken;
			_refreshToken = response.objects.result.refreshToken;
			return response.objects;
		}


		public IYS_TakeTokenAndRefreshResponse RefreshToken()
		{
			var response = SendRequest<IYS_TakeTokenAndRefreshResponse>("https://api.sandbox.iys.org.tr/oauth/refresh", Infoline.Helper.Json.Serialize(new { refreshToken = _refreshToken }), MethodType.POST);

			_accessToken = response.objects.result.accessToken;
			_refreshToken = response.objects.result.refreshToken;
			return response.objects;
		}

		public IYS_SingularPermitResponse InsertPermit(IYS_SingularPermitRequest permit)
		{
			var response = SendRequest<IYS_SingularPermitResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/consents", Infoline.Helper.Json.Serialize(new
			{
				consentDate = permit.consentDate,
				source = permit.source,
				recipientType = permit.recipientType,
				status = permit.status,
				type = permit.type,
				//retailerCode = permit.retailerCode,
				//retailerAccess = permit.retailerAccess,
				recipient = permit.recipient
			}), MethodType.POST);
			return response.objects;
		}

		public IYS_MultiplePermitResponse BulkPermit(IYS_MultiplePermitRequest[] permit)
		{
			var data = permit.Select(x => new IYS_MultiplePermitRequest
			{
				consentDate = x.consentDate,
				source = x.source,
				recipientType = x.recipientType,
				status = x.status,
				type = x.type,
				retailerCode = x.retailerCode,
				retailerAccess = x.retailerAccess,
				recipient = x.recipient,
			});

			var response = SendRequest<IYS_MultiplePermitResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/consents/request", Infoline.Helper.Json.Serialize(new { data }), MethodType.POST);

			if (response.objects.status == 401)
			{
				RefreshToken();
			}

			return response.objects;
		}

		public IYS_IndividualLeaveQueryResponse IndividualLeaveQuery(IYS_IndividualLeaveQueryRequest permit)
		{
			var response = SendRequest<IYS_IndividualLeaveQueryResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/consents/status", Infoline.Helper.Json.Serialize(new { recipient = permit.recipient, recipientType = permit.recipientType, type = permit.type }), MethodType.POST);
			return response.objects;
		}

		
		public IYS_DealerInquiryResponse DealerInquiry()
		{
			var response = SendRequest<IYS_DealerInquiryResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/retailers/" + _retailerCode + "", null, MethodType.GET);
			return response.objects;
		}

		public IYS_DeleteDealerResponse DeleteDealer()
		{
			var response = SendRequest<IYS_DeleteDealerResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/retailers/" + _retailerCode + "", null, MethodType.POST);

			if (response.objects.status == 401)
			{
				RefreshToken();
			}

			return response.objects;
		}

		public IYS_DealerListResponse DealerList()
		{
			var response = SendRequest<IYS_DealerListResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/retailers", "", MethodType.GET);

			if (response.objects.status == 401)
			{
				RefreshToken();
			}

			return response.objects;
		}

		public IYS_InsertDealerResponse InsertDealer(IYS_InsertDealerRequest dealer)
		{
			var response = SendRequest<IYS_InsertDealerResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/retailers", Infoline.Helper.Json.Serialize(new
			{
				title = dealer.title,
				mersis = dealer.mersis,
				alias = dealer.alias,
				name = dealer.name,
				tckn = dealer.tckn,
				mobile = dealer.mobile,
				city = dealer.city,
				town = dealer.town,
				status = dealer.status
			}), MethodType.POST);

			if (response.objects.status == 401)
			{
				RefreshToken();
			}

			_retailerCode = response.objects.retailerCode;

			return response.objects;
		}


		public IYS_AllDealerPermitDeleteResponse AllDealerPermitDelete(IYS_AllDealerPermitDeleteRequest dealerPermit)
		{
			var response = SendRequest<IYS_AllDealerPermitDeleteResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/consents/retailers/access/remove/all", Infoline.Helper.Json.Serialize(new
			{
				type = dealerPermit.type,
				recipientType = dealerPermit.recipientType,
				recipients = dealerPermit.recipients,
			}), MethodType.POST);

			if (response.objects.status == 401)
			{
				RefreshToken();
			}

			return response.objects;
		}

		public IYS_DeletingResellerPermissionAccessResponse DeletingResellerPermissionAccess(IYS_DeletingResellerPermissionAccessRequest dealer)
		{
			var response = SendRequest<IYS_DeletingResellerPermissionAccessResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/consents/retailers/access/remove", Infoline.Helper.Json.Serialize(new
			{
				type = dealer.type,
				recipientType = dealer.recipientType,
				retailerAccess = dealer.retailerAccess,
				recipients = dealer.recipients,
			}), MethodType.POST);

			if (response.objects.status == 401)
			{
				RefreshToken();
			}

			return response.objects;
		}

		public IYS_ResellerPermissionAccessInquiryResponse ResellerPermissionAccessInquiry(IYS_ResellerPermissionAccessInquiryRequest dealer)
		{
			var response = SendRequest<IYS_ResellerPermissionAccessInquiryResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/consent/retailers/access/list", Infoline.Helper.Json.Serialize(new
			{
				recipient = dealer.recipient,
				type = dealer.type,
				recipientType = dealer.recipientType,
			}), MethodType.POST);

			if (response.objects.status == 401)
			{
				RefreshToken();
			}

			return response.objects;
		}

		public IYS_GrantResellerPermissionAccessResponse GrantResellerPermissionAccess(IYS_GrantResellerPermissionAccessRequest dealer)
		{
			var response = SendRequest<IYS_GrantResellerPermissionAccessResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/consents/retailers/access", Infoline.Helper.Json.Serialize(new
			{
				type = dealer.type,
				recipientType = dealer.recipientType,
				retailerAccess = dealer.retailerAccess,
				recipients = dealer.recipients,
			}), MethodType.POST);

			if (response.objects.status == 401)
			{
				RefreshToken();
			}

			return response.objects;
		}

		public IYS_InsertResellerPermissionAccessResponse InsertResellerPermissionAccess(IYS_InsertResellerPermissionAccessRequest dealer)
		{
			var response = SendRequest<IYS_InsertResellerPermissionAccessResponse>("https://api.sandbox.iys.org.tr/sps/" + _iysCode + "/brands/" + _brandCode + "/consents/retailers/access", Infoline.Helper.Json.Serialize(new
			{
				type = dealer.type,
				recipient = dealer.recipient,
				recipientType = dealer.recipientType,
				retailerAccess = dealer.retailerAccess,

			}), MethodType.POST);

			if (response.objects.status == 401)
			{
				RefreshToken();
			}

			return response.objects;
		}


		private ResultStatus<T> SendRequest<T>(string uri, string rawData, MethodType type)
		{
			using (WebClient client = new WebClient())
			{
				ServicePointManager.Expect100Continue = true;
				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

				if (_accessToken != null)
				{
					client.Headers.Add("Authorization", "Bearer " + _accessToken);
				}

				if (type == MethodType.GET)
				{
					return new ResultStatus<T>
					{
						result = true,
						message = "istek başarılı",
						objects = Infoline.Helper.Json.Deserialize<T>(client.DownloadString(uri))
					};
				}
				return new ResultStatus<T>
				{
					result = true,
					message = "istek başarılı",
					objects = Infoline.Helper.Json.Deserialize<T>(client.UploadString(uri, "POST", rawData))
				};
			}
		}
	}

	public enum MethodType
	{
		POST,
		GET
	}

	public class IYS_TakeTokenAndRefreshResponse
	{
		public string message { get; set; }
		public int status { get; set; }
		public IYS_Token result { get; set; }

	}

	public class IYS_Token
	{
		public string accessToken { get; set; }
		public int expiresIn { get; set; }
		public string refreshToken { get; set; }
		public string tokenType { get; set; }
	}

	public class IYS_RefreshTokenRequest
	{
		public string refreshToken { get; set; }
	}

	public class IYS_SingularPermitRequest
	{
		public string recipient { get; set; }
		public string type { get; set; }
		public string source { get; set; }
		public string status { get; set; }
		public string consentDate { get; set; }
		public string recipientType { get; set; }
     //   public int retailerCode { get; set; }
	//	public int[] retailerAccess { get; set; }
	}

	public class IYS_SingularPermitResponse
	{
		public string transactionId { get; set; }
		public string creationDate { get; set; }
		//public int status { get; set; }
	}

	public class IYS_MultiplePermitRequest
	{
		public string recipient { get; set; }
		public string type { get; set; }
		public string source { get; set; }
		public string status { get; set; }
		public string consentDate { get; set; }
		public string recipientType { get; set; }
		public int retailerCode { get; set; }
		public int[] retailerAccess { get; set; }
	}

	public class IYS_MultiplePermitResponse
	{
		public string requestId { get; set; }
		public string message { get; set; }
		public int status { get; set; }
		public IYS_MultiplePermitSubRequest[] subRequests { get; set; }
	}

	public class IYS_MultiplePermitSubRequest
	{
		public string subrequestId { get; set; }
		public string consentDate { get; set; }
		public string recipient { get; set; }
		public string recipientType { get; set; }
		public string status { get; set; }
		public string type { get; set; }
	}

	public class IYS_IndividualLeaveQueryRequest
	{
		public string recipient { get; set; }
		public string recipientType { get; set; }
		public string type { get; set; }
	}

	public class IYS_IndividualLeaveQueryResponse
	{
		public string consentDate { get; set; }
		public string creationDate { get; set; }
		public string source { get; set; }
		public string recipient { get; set; }
		public string recipientType { get; set; }
		public string status { get; set; }
		public string message { get; set; }
		public string type { get; set; }
		public string retailerAccessCount { get; set; }
		public string retailerTitle { get; set; }
		public string retailerCode { get; set; }
	}

	public class IYS_DealerInquiryResponse
	{
		public string title { get; set; }
		public string mersis { get; set; }
		public string alias { get; set; }
		public string name { get; set; }
		public string tckn { get; set; }
		public string mobile { get; set; }
		public City city { get; set; }
		public Town town { get; set; }
		public int retailerAccessCount { get; set; }
		public bool canBeDeleted { get; set; }
		public string status { get; set; }
	}

	public class City
	{
		public string name { get; set; }
		public string code { get; set; }
	}

	public class Town
	{
		public string name { get; set; }
		public string code { get; set; }
	}

	public class IYS_DeleteDealerResponse
	{
		public string message { get; set; }
		public int status { get; set; }
	}

	public class IYS_DealerListResponse
	{
		public IYS_Pagination pagination { get; set; }
		public IYS_DealerListSearchList[] list { get; set; }
		public string message { get; set; }
		public int status { get; set; }
	}

	public class IYS_Pagination
	{
		public int offset { get; set; }
		public int pageSize { get; set; }
		public int totalCount { get; set; }
	}

	public class IYS_DealerListSearchList
	{
		public string title { get; set; }
		public string mersis { get; set; }
		public string alias { get; set; }
		public string name { get; set; }
		public string tckn { get; set; }
		public string mobile { get; set; }
		public City city { get; set; }
		public Town town { get; set; }
		public int retailerAccessCount { get; set; }
		public int retailerCode { get; set; }
		public bool canBeDeleted { get; set; }
		public string status { get; set; }
	}


	public class IYS_InsertDealerRequest
	{
		public string title { get; set; }
		public string mersis { get; set; }
		public string alias { get; set; }
		public string name { get; set; }
		public string mobile { get; set; }
		public long tckn { get; set; }
		public City city { get; set; }
		public Town town { get; set; }
		public string status { get; set; }
	}

	public class IYS_InsertDealerResponse
	{
		public int retailerCode { get; set; }
		public string message { get; set; }
		public int status { get; set; }

	}

	public class IYS_AllDealerPermitDeleteRequest
	{
		public string type { get; set; }
		public string recipientType { get; set; }
		public string[] recipients { get; set; }

	}

	public class IYS_AllDealerPermitDeleteResponse
	{
		public string requestId { get; set; }
		public string message { get; set; }
		public int status { get; set; }

	}

	public class IYS_DeletingResellerPermissionAccessRequest
	{
		public string type { get; set; }
		public string recipientType { get; set; }
		public int[] retailerAccess { get; set; }
		public string[] recipients { get; set; }
	}

	public class IYS_DeletingResellerPermissionAccessResponse
	{
		public string requestId { get; set; }
		public string message { get; set; }
		public int status { get; set; }
	}
	public class IYS_ResellerPermissionAccessInquiryRequest
	{
		public string recipient { get; set; }
		public string type { get; set; }
		public string recipientType { get; set; }

	}

	public class IYS_ResellerPermissionAccessInquiryResponse
	{
		public IYS_Pagination pagination { get; set; }
		public IYS_ResellerPermissionAccessInquiryRetailerSearchList list { get; set; }
		public string message { get; set; }
		public int status { get; set; }

	}
	public class IYS_ResellerPermissionAccessInquiryRetailerSearchList
	{
		public string title { get; set; }
		public string mersis { get; set; }
		public string alias { get; set; }
		public string name { get; set; }
		public string tckn { get; set; }
		public string mobile { get; set; }
		public City city { get; set; }
		public Town town { get; set; }
		public int retailerCode { get; set; }
	}

	public class IYS_GrantResellerPermissionAccessRequest
	{
		public string type { get; set; }
		public string recipientType { get; set; }
		public int[] retailerAccess { get; set; }
		public string[] recipients { get; set; }
	}
	public class IYS_GrantResellerPermissionAccessResponse
	{
		public string requestId { get; set; }
		public string message { get; set; }
		public int status { get; set; }
	}
	public class IYS_InsertResellerPermissionAccessRequest
	{
		public string type { get; set; }
		public string recipient { get; set; }
		public string recipientType { get; set; }
		public int[] retailerAccess { get; set; }
	}
	public class IYS_InsertResellerPermissionAccessResponse
	{
		public string transactionId { get; set; }
		public string message { get; set; }
		public int status { get; set; }
	}

}
