using BotDetect.Web.Mvc;
using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Controllers
{
	public class AccountController : Controller
	{
		[PageInfo("Benim Sayfam", SHRoles.Personel, SHRoles.SahaGorevMusteri, SHRoles.BayiPersoneli, SHRoles.BayiGorevPersoneli, SHRoles.YukleniciPersoneli, SHRoles.YardimMasaMusteri)]
		public ActionResult Index()
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var db = new WorkOfTimeDatabase();

			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.BayiPersoneli)))
			{
				return RedirectToAction("Index", "VWCRM_Presentation", new { area = "CRM" });
			}

			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.BayiGorevPersoneli)))
			{
				return RedirectToAction("Index", "VWFTM_Task", new { area = "FTM" });
			}

			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.YardimMasaMusteri)) && userStatus.user.type == (int)EnumSH_UserType.OtherPerson)
			{
				return RedirectToAction("Help", "VWHDM_Issue", new { area = "HDM" });
			}

			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevMusteri)) && userStatus.user.type == (int)EnumSH_UserType.OtherPerson)
			{
				return RedirectToAction("Index", "Customer", new { area = "" });
			}

			if (userStatus.user.id == new Guid("DCEAA584-35B7-4637-AF55-48CCF013C9D3"))
			{
				return RedirectToAction("ScadaTasks", "VWFTM_TaskGrid", new { area = "FTM" });
			}

			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.YukleniciPersoneli)) && userStatus.user.type == (int)EnumSH_UserType.OtherPerson)
			{
				return PartialView("~/Areas/FTM/Views/VWFTM_Task/IndexYuklenici.cshtml");
			}


			var data = db.GetSH_UserLog(userStatus.user.id);
			var permitTypes = db.GetINV_PermitType();
			var viewDataPermitTypesCanHourly = permitTypes.Where(x => x.ViewStaff == true && x.CanHourly == true).Select(x => x.id).ToArray();
			var viewDataPermitTypes = permitTypes.Where(x => x.ViewStaff == true && x.CanHourly == false).Select(x => x.id).ToArray();

			var newListKeyValue = new List<KeyValue>();

			var daylyPermits = db.GetVWINV_PermitByUserIdAndPermitTypeIds(userStatus.user.id, viewDataPermitTypes)
				.Where(x => x.PermitTypeId != new Guid("F8547488-3215-1235-5126-EF2323CBBCB2") && x.PermitTypeId != new Guid("AF40ADF4-9963-4790-A9DE-1D74C32B61C1"))
				.GroupBy(x => x.PermitType_Title)
				.Select(x => new KeyValue
				{
					Key = x.Key,
					Value = x.Select(f => f.TotalDays).Sum(),
					Tooltip = "true"
				}).ToArray();

			var hourlyPermits = db.GetVWINV_PermitByUserIdAndPermitTypeIds(userStatus.user.id, viewDataPermitTypesCanHourly)
				.Where(x => x.PermitTypeId != new Guid("F8547488-3215-1235-5126-EF2323CBBCB2") && x.PermitTypeId != new Guid("AF40ADF4-9963-4790-A9DE-1D74C32B61C1"))
				.GroupBy(x => x.PermitType_Title)
				.Select(x => new KeyValue
				{
					Key = x.Key,
					Value = x.Select(f => f.TotalHours).Sum(),
					Tooltip = "false"
				}).ToArray();

			newListKeyValue.AddRange(daylyPermits);
			newListKeyValue.AddRange(hourlyPermits);
			var newData = new IndexDatas
			{
				data = data,
				permits = newListKeyValue,
				permitTypes = permitTypes
			};

			return View(newData);
		}

		[PageInfo("Benim Bilgilerim", SHRoles.Personel)]
		public new ActionResult Profile(VMSH_UserModel item)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			item.id = userStatus.user.id;
			item.Load();
			return View(item);
		}


		[PageInfo("Mobil Yönlendirme"), AllowEveryone]
		public ActionResult Redirect()
		{
			string url = TenantConfig.Tenant.GetWebUrl();
			var serviceDomain = TenantConfig.Tenant.GetWebServiceUrl();
			var userAgent = HttpContext.Request.UserAgent.ToLower();
			string intentLink = string.Format(@"wotclient://musteri/{0}#{1}", url, serviceDomain);
			if (userAgent.Contains("iphone"))
			{
				intentLink = string.Format(@"wotclient://musteri/{0}#{1}", url, serviceDomain);
			}
			else if (userAgent.Contains("android"))
			{
				intentLink = string.Format(@"intent://musteri/{0}#{1}/#Intent;scheme=wotclient;package=com.infoline.wotclient;end;", url, serviceDomain);
			}
			ViewBag.URL = intentLink;
			return View();
		}

		[PageInfo("Kullanıcı Girişi"), AllowEveryone]
		public ActionResult SignIn()
		{
			var user = new LoginModel();
			var userStatus = Session["userStatus"];
			string returnurl = Url.Action("Index", "Account", new { area = "" });
			if (userStatus != null)
			{
				if (!string.IsNullOrEmpty(Request.QueryString["returnUrl"]))
				{
					try
					{
						returnurl = new Infoline.Framework.Helper.CryptographyHelper().Decrypt(Request.QueryString["returnUrl"]);
					}
					catch { }
				}
				return Redirect(returnurl);
			}

			var signQrCore = new SignQrCore
			{
				WebServiceUrl = TenantConfig.Tenant.GetWebServiceUrl(),
				WebUrl = TenantConfig.Tenant.GetWebUrl()
			};

#if DEBUG == true
			signQrCore = new SignQrCore
			{
				WebServiceUrl = "http://10.100.0.91/wot",
				WebUrl = "http://10.100.0.72:62223"
			};
#endif

			ViewBag.ServiceUrl = Infoline.Helper.Json.Serialize(signQrCore);
			ViewBag.returnUrl = Request.QueryString["returnUrl"];
			return View(user);
		}


		[PageInfo("Kullanıcı Girişi"), AllowEveryone]
		[HttpPost, CaptchaValidation("CaptchaCode", "UserControlCaptcha", "Hatalı Giriş")]
		public JsonResult SignIn(LoginModel sh_user)
		{
			var Feedback = new FeedBack();

			var username = !String.IsNullOrEmpty(sh_user.loginname) ? sh_user.loginname.Trim() : "";
			var password = !String.IsNullOrEmpty(sh_user.password) ? sh_user.password.Trim() : "";

			if (String.IsNullOrEmpty(sh_user.loginname))
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = Feedback.Warning("Kullanıcı adı alanı boş geçilemez..")
				}, JsonRequestBehavior.AllowGet);
			}

			if (String.IsNullOrEmpty(sh_user.password))
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = Feedback.Warning("Şifre alanı boş geçilemez..")
				}, JsonRequestBehavior.AllowGet);
			}

			string redirecturl = Url.Action("Index", "Account", new { area = "" });
			if (!string.IsNullOrEmpty(Request["returnUrl"]))
			{
				try
				{
					redirecturl = new Infoline.Framework.Helper.CryptographyHelper().Decrypt(Request["returnUrl"]);
				}
				catch { }
			}
			var db = new WorkOfTimeDatabase();

			var user = db.GetVWSH_UserByLoginName(sh_user.loginname);
			if (user != null)
			{
				var shUserRoles = db.GetSH_UserRoleByUserId(user.id);

				if (TenantConfig.Tenant.TenantCode == 1179 || TenantConfig.Tenant.TenantCode == 1100 && shUserRoles.Where(x => x.roleid.Value == new Guid(SHRoles.BayiPersoneli)).FirstOrDefault() != null)
				{
					redirecturl = Url.Action("Index", "VWCRM_Presentation", new { area = "CRM" });
				}
			}


			var login = db.Login(username, password, TenantConfig.Tenant.Config.LdapUrls);

			var loginError = Convert.ToInt32(Session["LoginError"] ?? "0");

			if (login.LoginResult != Infoline.LoginResult.OK || login.ticketid == null)
			{
				Session["LoginError"] = ++loginError;

				var state = login.LoginResult.ToString();
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = Feedback.Warning(state == "InvalidUser" ? "Geçersiz kullanıcı" : (state == "InvalidPassword" ? "Geçersiz şifre" : "Kullanıcı aktif değil")),
					Object = new SignInResult
					{
						RequiredCaptcha = loginError >= 3
					}
				}, JsonRequestBehavior.AllowGet);
			}
			if (login.LoginResult == LoginResult.OK && loginError >= 3)
			{
				var res = CaptchaControl(sh_user.CapthcaCode);
				if (!res.Result)
				{
					return Json(new ResultStatusUI
					{
						FeedBack = res.FeedBack,
						Object = new SignInResult
						{
							RequiredCaptcha = loginError >= 3
						}
					}, JsonRequestBehavior.AllowGet);
				}
				Session["LoginError"] = 0;
			}


			var userStatusm = db.GetUserPageSecurityByticketid(login.ticketid);

			//Sınırsız login isteyen kullanıcılar için (Kardeş talebi)
			if (TenantConfig.Tenant.TenantCode == 1115 && userStatusm.user.id == new Guid("3AE3D3CF-EB97-4553-A2EC-F4EAD3ABBB2B"))
			{
				Session.Timeout = int.MaxValue;
			}


			Session["userStatus"] = userStatusm;
			Session.Timeout = 120;

			return Json(new ResultStatusUI
			{
				Result = true,
				FeedBack = Feedback.Custom("Giriş işlemi başarılı.", redirecturl, "Bilgilendirme", "success", 2, false)
			}, JsonRequestBehavior.AllowGet);
		}

		[AllowEveryone]
		[PageInfo("Oturum Kapat")]
		public ActionResult SignOut()
		{
			if (Session["userStatus"] != null)
			{
				var db = new WorkOfTimeDatabase();
				var userStatus = (PageSecurity)Session["userStatus"];
				db.UpdateSH_Ticket(new SH_Ticket { id = userStatus.ticketid, endtime = DateTime.Now }); Session.Remove("userStatus");
				Session.Clear();
			}
			return RedirectToAction("SignIn", "Account", new { area = String.Empty });
		}

		[PageInfo("Şifre Sıfırlama"), AllowEveryone]
		public ActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		[PageInfo("Şifre Sıfırlama"), AllowEveryone]
		public JsonResult ForgotPassword(SH_User item)
		{
			var feedback = new FeedBack();
			var email = item.email;
			var loginName = item.loginname;



			if (string.IsNullOrEmpty(email))
			{
				return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Eposta bilgisi zorunludur.") }, JsonRequestBehavior.AllowGet);
			}

			if (string.IsNullOrEmpty(loginName))
			{
				return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Kullanıcı Adı bilgisi zorunludur.") }, JsonRequestBehavior.AllowGet);
			}


			var db = new WorkOfTimeDatabase();
			var user = db.GetVWSH_UserByLoginName(loginName);
			if (user == null)
			{
				return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Kullanıcı Adı sistemden kayıtlı değil.") }, JsonRequestBehavior.AllowGet);
			}

			if (user.email.ToLower() != email.ToLower())
			{
				return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Girdiğiniz Bilgiler Uyuşmuyor.") }, JsonRequestBehavior.AllowGet);
			}

			var password = Guid.NewGuid().ToString().Substring(0, 8);
			var md5Password = db.GetMd5Hash(db.GetMd5Hash(password));
			var res = db.UpdateSH_User(new SH_User
			{
				id = user.id,
				password = md5Password,
				status = user.status
			});

			if (!res.result)
			{
				return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("HATA") }, JsonRequestBehavior.AllowGet);
			}

			if (user != null)
			{
				string url = TenantConfig.Tenant.GetWebUrl();
				var tenantName = TenantConfig.Tenant.TenantName;
				var mesajIcerigi = string.Format(@"<h3>Sayın {0}, {3} WorkOfTime Sistemine hoşgeldiniz.</h3> <p>  Geçici şifreniz başarı ile oluşturuldu.</p>
                    <p>{3} Work Of Time sistemine <u> Kimlik Numaranız (Demo kullanıcı iseniz mail adresiniz ile)</u> ve <u>Şifreniz</u> ile giriş sağlayabilirsiniz.</p>
                    <p>Lütfen <u>geçici şifreniz</u> ile giriş yaptıktan sonra profil sayfanızdan şifrenizi değiştiriniz.</p>
                    <p>Şifrenizi değiştirmek için <a href='{2}/Account/UpdatePassword/'>Buraya tıklayınız! </a></p>
                    <p><strong>Geçici Şifreniz : <strong><span style='color: #ed5565;'>{1}</span></p>", user.firstname + " " + user.lastname, password, url, tenantName);

				new Email().Template("Template1", "userMailFoto.jpg", "Geçici Şifre Bilgilendirmesi", mesajIcerigi)
				  .Send((Int16)EmailSendTypes.ZorunluMailler, user.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Geçici Şifre Bilgilendirmesi"), true);
			}

			var result = new ResultStatusUI
			{
				Result = true,
				FeedBack = feedback.Custom("Geçici şifreniz mail adresinize gönderilmiştir.", user != null ? null : Url.Action("SignIn", "Account", new { area = String.Empty, loginName = loginName }), "Bilgilendirme", "success", 5, false)
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Şifre güncelle", SHRoles.Personel, SHRoles.SahaGorevMusteri)]
		public ActionResult UpdatePassword()
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];

			var kullanici = db.GetSH_UserById(userStatus.user.id);
			if (kullanici == null)
			{
				return RedirectToAction("Index", "Account");
			}


			kullanici.password = null;

			return View(kullanici);
		}

		[PageInfo("Şifre güncelleme methodu", SHRoles.Personel, SHRoles.SahaGorevMusteri)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult UpdatePassword(string oldPassword, string password, string rePassword)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			if (password != rePassword)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Şifreler uyuşmuyor.")
				}, JsonRequestBehavior.AllowGet);
			}

			if (userStatus.user.password != db.GetMd5Hash(db.GetMd5Hash(oldPassword)))
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Eski şifrenizi kontrol edin.")
				}, JsonRequestBehavior.AllowGet);
			}

			var newuser = new SH_User().B_EntityDataCopyForMaterial(userStatus.user);
			newuser.password = db.GetMd5Hash(db.GetMd5Hash(password));
			var dbresult = db.UpdateSH_User(newuser);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result
					? feedback.Success("Şifre güncelleme işlemi başarılı")
					: feedback.Error(dbresult.message)
			};

			return Json(result, JsonRequestBehavior.AllowGet);

		}

		[AllowEveryone]
		public string Execute()
		{
			EnumInfo.Execute();
			TenantConfig.Tenant.ExecuteRoles();
			ExecuteUTRules();
			ExecuteProductUnit();
			try
			{
				var db = new WorkOfTimeDatabase();
				var userStatus = (PageSecurity)Session["userStatus"];
				var result = db.Login(userStatus.user.loginname, userStatus.user.password);
				if (result.LoginResult == LoginResult.OK)
				{
					Session["userStatus"] = db.GetUserPageSecurityByticketid(result.ticketid);
				}
			}
			catch
			{
			}
			return "işlem başarılı";
		}

		[AllowEveryone]
		public void ExecuteUTRules()
		{
			var db = new WorkOfTimeDatabase();
			var defaultRuleTransaction = db.GetUT_RulesByTypeIsDefault((Int16)EnumUT_RulesType.Transaction);
			var defaultRuleAdvance = db.GetUT_RulesByTypeIsDefault((Int16)EnumUT_RulesType.Advance);
			var dbres = new ResultStatus { result = true };
			var trans = db.BeginTransaction();

			if (defaultRuleTransaction == null)
			{
				var newRule = new UT_Rules
				{
					created = DateTime.Now,
					createdby = Guid.Empty,
					isDefault = true,
					type = (Int16)EnumUT_RulesType.Transaction,
					name = "Masraf Kuralı"
				};

				var newRuleStage = new UT_RulesUserStage
				{
					type = (Int16)EnumUT_RulesUserStage.Manager1,
					created = DateTime.Now,
					rulesId = newRule.id,
					order = 1,
					roleId = null,
					userId = null,
					title = "1. Yönetici"
				};

				dbres &= db.InsertUT_Rules(newRule, trans);
				dbres &= db.InsertUT_RulesUserStage(newRuleStage, trans);
			}

			if (defaultRuleAdvance == null)
			{
				var newRule = new UT_Rules
				{
					created = DateTime.Now,
					createdby = Guid.Empty,
					isDefault = true,
					type = (Int16)EnumUT_RulesType.Advance,
					name = "Avans Kuralı"
				};

				var newRuleStage = new UT_RulesUserStage
				{
					type = (Int16)EnumUT_RulesUserStage.Manager1,
					created = DateTime.Now,
					rulesId = newRule.id,
					order = 1,
					roleId = null,
					userId = null,
					title = "1. Yönetici"
				};

				dbres &= db.InsertUT_Rules(newRule, trans);
				dbres &= db.InsertUT_RulesUserStage(newRuleStage, trans);
			}

			if (!dbres.result)
			{
				trans.Rollback();
			}
			else
			{
				trans.Commit();
			}
		}

		[AllowEveryone]
		public void ExecuteProductUnit()
		{
			var db = new WorkOfTimeDatabase();

			var productUnit = db.GetVWPRD_ProductUnit();
			var dbres = new ResultStatus { result = true };
			var trans = db.BeginTransaction();

			if (productUnit.Count() == 0)
			{
				var products = db.GetVWPRD_Product();
				var productUnitList = new List<PRD_ProductUnit>();
				foreach (var product in products)
				{
					var productUnitData = new PRD_ProductUnit();
					if (product.unitId.HasValue)
					{
						productUnitData = new PRD_ProductUnit
						{
							id = Guid.NewGuid(),
							created = DateTime.Now,
							createdby = product.createdby,
							isDefault = 1,
							productId = product.id,
							quantity = 1,
							unitId = product.unitId.Value
						};

					}
					else
					{
						productUnitData = new PRD_ProductUnit
						{
							id = Guid.NewGuid(),
							created = DateTime.Now,
							createdby = product.createdby,
							isDefault = 1,
							productId = product.id,
							quantity = 1,
							unitId = new Guid("8F6E4C58-47AB-445C-B3C6-6DF642AF1DAC")
						};
					}

					productUnitList.Add(productUnitData);

				}

				dbres &= db.BulkInsertPRD_ProductUnit(productUnitList, trans);

				if (!dbres.result)
				{
					trans.Rollback();
				}
				else
				{
					trans.Commit();

					if (products.Count() > 0)
					{
						var transactionItems = db.GetPRD_TransactionItemByProductIds(products.Select(a => a.id).ToArray());
						var transactionItemList = new List<PRD_TransactionItem>();
						if (transactionItems.Count() > 0)
						{
							foreach (var product in products)
							{
								var transactions = transactionItems.Where(a => a.productId == product.id).ToList();
								foreach (var transaction in transactions)
								{
									transaction.alternativeUnitId = product.unitId.HasValue ? product.unitId.Value : new Guid("8F6E4C58-47AB-445C-B3C6-6DF642AF1DAC");
									transaction.unitId = product.unitId.HasValue ? product.unitId.Value : new Guid("8F6E4C58-47AB-445C-B3C6-6DF642AF1DAC");
									transaction.alternativeQuantity = 1;
									transactionItemList.Add(transaction);
								}
							}

							db.BulkUpdatePRD_TransactionItem(transactionItemList);
						}
					}
				}
			}
		}

		public ResultStatusUI CaptchaControl(string resp)
		{

			var secretKey = "6LfZ8IUbAAAAALzYo9O1EnbY__jI-x9U_sjliIw8";
			var res = BusinessExtensions.GetRemoteString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secretKey, resp));
			if (!res.result)
			{
				return new ResultStatusUI
				{
					FeedBack = new FeedBack().Warning("Lütfen güvenlik doğrulaması için sunucular cevap vermiyor.")
				};
			}

			var obj = JObject.Parse(res.objects);
			var status = (bool)obj.SelectToken("success");

			if (!status)
			{
				return new ResultStatusUI
				{
					Result = false,
					FeedBack = new FeedBack().Warning("Lütfen güvenlik doğrulamasını yapınız.")
				};
			}

			return new ResultStatusUI { Result = true };

		}

		[AllowEveryone]
		public ActionResult Fab()
		{
			return View();
		}

		[AllowEveryone]
		public ActionResult Fabric()
		{
			return View();
		}

		[AllowEveryone]
		public ActionResult Fabric2()
		{
			return View();
		}

		[AllowEveryone]
		public ContentResult GetOperators()
		{
			return Content(Helper.Json.Serialize(new object { }), "application/json");
		}

	}

	public class SignQrCore
	{
		public string WebServiceUrl { get; set; }
		public string WebUrl { get; set; }
	}

	public class IndexDatas
	{
		public LogTable[] data { get; set; }
		public List<KeyValue> permits { get; set; }
		public INV_PermitType[] permitTypes { get; set; }

	}

}