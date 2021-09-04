using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.HR.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.HR.Controllers
{
	public class VWHR_StaffNeedsController : Controller
	{
		[PageInfo("Personel Talepleri", SHRoles.IKYonetici)]
		public ActionResult Index()
		{
			return View();
		}

		[PageInfo("Personel Talep İşlemleri", SHRoles.PersonelTalebi, SHRoles.IdariPersonelYonetici, SHRoles.IKYonetici)]
		public ActionResult MyIndex()
		{
			var db = new WorkOfTimeDatabase();
			var data = new VMKeywordAndPositions
			{
				positions = db.GetHR_Position(),
				keywords = db.GetHR_Keywords()
			};
			return View(data);
		}

		[PageInfo("Personel Talebi Methodu", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWHR_StaffNeeds(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWHR_StaffNeedsCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Personel Talebi Adet Metodu", SHRoles.Personel)]
		public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			return db.GetVWHR_StaffNeedsCount(condition.Filter);
		}

		[PageInfo("Personel Talebi Veri Methodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

			var db = new WorkOfTimeDatabase();
			var data = db.GetVWHR_StaffNeeds(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Personelin Talebi Detayı", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici, SHRoles.PersonelTalebi)]
		public ActionResult Detail(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var model = db.GetVWHR_StaffNeedsById(id);
			var data = new VMStaffNeedsAndSearch().B_EntityDataCopyForMaterial(model);
			data.Totalcv = db.GetHR_StaffNeedsPersonByNeedsId(id).Count();
			var plans = db.GetHR_PlanByHrNeedsId(id);
			data.cards = new Models.Card
			{
				Olumlu = plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.Olumlu).Count(),
				Olumsuz = plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.Olumsuz).Count(),
				DahaSonra = plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.DahaSonra).Count(),
				Diger = plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.Diger).Count(),
				Gorusulmedi = plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.Gorusulmedi).Count(),
				Tekrar = plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.Tekrar).Count()
			};

			var requesters = db.GetHR_StaffNeedsRequesterByStaffNeedId(data.id);
			var requesterIds = requesters.Where(x => x.RequestId.HasValue).Select(x => x.RequestId.Value).ToArray();

			data.requesters = string.Join(", ", db.GetSH_UserByIds(requesterIds).Select(x => x.firstname + " " + x.lastname).ToArray());

			return View(data);
		}
		[PageInfo("Personellere İhtiyacına Uygun CV'lerin Filtrelemesi", SHRoles.IKYonetici)]
		public ActionResult DetailFor(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var model = db.GetVWHR_StaffNeedsById(id);
			var data = new VMStaffNeedsAndSearch().B_EntityDataCopyForMaterial(model);
			var personPositions = db.GetHR_StaffNeedsPositionByNeedsId(id);
			var personKeywords = db.GetHR_StaffNeedsKeywordsByNeedsId(id);
			data.personPosition = personPositions.Select(x => x.HrPositionId).ToArray();
			data.personKeywords = personKeywords.Select(x => x.HrKeywordsId).ToArray();
			return View(data);
		}

		[PageInfo("Personel Talebi Ekleme", SHRoles.PersonelTalebi)]
		public ActionResult Insert()
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var data = new VWHR_StaffNeeds
			{
				id = Guid.NewGuid(),
				createdby = userStatus.user.id
			};
			return View(data);
		}


		[PageInfo("Personel Talebi Ekleme", SHRoles.PersonelTalebi)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(HR_StaffNeeds item, Guid[] Position, Guid[] Keywords, Guid?[] RequestIds)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var results = new ResultStatus();
			results.result = true;
			var trans = db.BeginTransaction();
			var personelonaylayicisi = db.GetSH_UserByRoleId(SHRoles.IdariPersonelYonetici);
			item.created = DateTime.Now;
			item.createdby = userStatus.user.id;
			item.RequestDate = DateTime.Now;
			item.NeedCode = BusinessExtensions.B_GetIdCode();

			if (RequestIds != null && RequestIds.Count() > 0)
			{
				var requesterList = RequestIds.Select(a => new HR_StaffNeedsRequester
				{
					RequestId = a,
					StaffNeedId = item.id,
					createdby = userStatus.user.id
				}).ToList();

				if (requesterList.Count(x => x.RequestId == userStatus.user.id) == 0)
				{
					requesterList.Add(new HR_StaffNeedsRequester
					{
						RequestId = userStatus.user.id,
						StaffNeedId = item.id,
						createdby = userStatus.user.id
					});
				}

				results &= db.BulkInsertHR_StaffNeedsRequester(requesterList, trans);
			}
			else
			{
				var requester = new HR_StaffNeedsRequester
				{
					createdby = userStatus.user.id,
					RequestId = userStatus.user.id,
					StaffNeedId = item.id
				};

				results &= db.InsertHR_StaffNeedsRequester(requester, trans);
			}

			results &= db.InsertHR_StaffNeeds(item, trans);
			if (Position != null)
			{
				var PositionList = Position.Select(a => new HR_StaffNeedsPosition
				{
					createdby = userStatus.user.id,
					HrPositionId = a,
					HrStaffNeedsId = item.id
				});
				results &= db.BulkInsertHR_StaffNeedsPosition(PositionList, trans);
			}
			if (Keywords != null)
			{
				var KeywordsList = Keywords.Select(a => new HR_StaffNeedsKeywords
				{
					createdby = userStatus.user.id,
					HrKeywordsId = a,
					HrStaffNeedsId = item.id
				});
				results &= db.BulkInsertHR_StaffNeedsKeywords(KeywordsList, trans);
			}

			results &= db.InsertHR_StaffNeedsStatus(new HR_StaffNeedsStatus
			{
				status = (Int32)EnumHR_StaffNeedsStatusStatus.TalepGerceklestirildi,
				staffNeedId = item.id,
				description = userStatus.user.FullName + " tarafından cv talebi gerçekleştirildi."
			}, trans);

			if (results.result == false)
			{
				trans.Rollback();
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Personel talep gerçekleştirme işlemi başarısız oldu.")

				}, JsonRequestBehavior.AllowGet);
			}

			trans.Commit();
			string url = TenantConfig.Tenant.GetWebUrl();
			var tenantName = TenantConfig.Tenant.TenantName;

			foreach (var genelMudur in personelonaylayicisi)
			{
				if (item.createdby.HasValue)
				{
					var userReq = db.GetVWSH_UserById(item.createdby.Value);
					var text = "<h3>Merhaba {0},</h3>";
					text += "<p>{1} adlı personel {2} tarihinde yeni personel talebi gerçekleştirmiştir.</p>";
					text += "<p>Personel talebi detayını görmek ve onay işlemleri için <a href='{3}/HR/VWHR_StaffNeeds/Update?id={4}'> Buraya tıklayınız! </a></p>";
					text += "<p> Bilgilerinize. </p>";
					text += "<p> Saygılarımızla. </p>";
					var mesaj = string.Format(text, genelMudur.firstname + " " + genelMudur.lastname, userReq.firstname + " " + userReq.lastname, string.Format("{0:dd.MM.yyyy HH:mm}", item.created), url, item.id);
					new Email().Template("Template1", "demo.jpg", "Yeni Personel İhtiyacı", mesaj)
							.Send((Int16)EmailSendTypes.PersonelAdayMulakat, genelMudur.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Yeni Personel İhtiyacı Hakkında"), true);
				}
			}

			return Json(new ResultStatusUI
			{
				Result = true,
				FeedBack = feedback.Success("Personel talep işlemi başarı ile gerçekleşti.", false, Url.Action("MyIndex", "VWHR_StaffNeeds", new { area = "HR" }))

			}, JsonRequestBehavior.AllowGet);

		}
		[PageInfo("Personel Talebi Güncelleme", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici, SHRoles.PersonelTalebi)]
		public ActionResult Update(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var model = db.GetVWHR_StaffNeedsById(id);
			var data = new VMStaffNeedsAndSearch().B_EntityDataCopyForMaterial(model);
			var personPositions = db.GetHR_StaffNeedsPositionByNeedsId(id);
			var personKeywords = db.GetHR_StaffNeedsKeywordsByNeedsId(id);
			data.personPosition = personPositions.Select(x => x.HrPositionId).ToArray();
			data.personKeywords = personKeywords.Select(x => x.HrKeywordsId).ToArray();
			return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Personel Talebi Güncelleme", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici, SHRoles.PersonelTalebi)]
		public JsonResult Update(HR_StaffNeeds item, Guid[] Position, Guid[] Keywords, short? lastStatus)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var data = db.GetHR_StaffNeedsById(item.id);
			data.changed = DateTime.Now;
			data.changedby = userStatus.user.id;
			data.Description = item.Description;
			data.RequestApprovedId = userStatus.user.id;
			HR_Position[] positions;
			string position = "";
			var PositionData = db.GetHR_StaffNeedsPositionList(item.id).Select(x => x.HrPositionId).ToList();

			if (PositionData.Count() > 0)
			{
				positions = db.GetHR_PositionList(PositionData);
				position = string.Join(",", positions.Select(x => x.Name).ToArray());
			}

			HR_Keywords[] keywords;
			string keyword = "";
			var KeywordsData = db.GetHR_StaffNeedsKeywordsList(item.id).Select(x => x.HrKeywordsId).ToList();
			if (KeywordsData.Count() > 0)
			{
				keywords = db.GetHR_KeywordsList(KeywordsData);
				keyword = string.Join(",", keywords.Select(x => x.Name).ToArray());
			}

			string url = TenantConfig.Tenant.GetWebUrl();
			var tenantName = TenantConfig.Tenant.TenantName;
			var allBilgiler = db.GetVWHR_StaffNeedsById(item.id);

			if (lastStatus == (Int32)EnumHR_StaffNeedsStatusStatus.YoneticiOnaylandi)
			{

				var insankaynaklariAll = db.GetSH_UserByRoleId(SHRoles.IKYonetici);
				foreach (var insankaynaklari in insankaynaklariAll)
				{

					//To do:Oğuz
					var text = "<h3>Merhaba {0},</h3>";
					text += "<p>{1} adlı personelin, personel alım talebi yöneticiler tarafından onaylanmıştır.</p>";
					text += "<p>İşlem yapmak için lütfen <a href='{2}/HR/VWHR_StaffNeeds/DetailFor?id={3}'> Buraya tıklayınız! </a></p>";
					text += "<p> Bilgilerinize. </p>";
					var mesaj = string.Format(text, insankaynaklari.firstname + " " + insankaynaklari.lastname, allBilgiler.createdby_Title, url, item.id);
					new Email().Template("Template1", "demo.jpg", "Cv Talebi Hakkında", mesaj)
							.Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Cv Talebi Hakkında.."), true);
				}
			}
			else if (lastStatus == (Int32)EnumHR_StaffNeedsStatusStatus.YoneticiReddetti)
			{
				if (item.createdby.HasValue)
				{
					var userReq = db.GetSH_UserById(item.createdby.Value);
					var text = "<h3>Merhaba {0},</h3>";
					text += "<p>{1} tarihinde {2} pozisyon/ları için gerçekleştirmiş olduğunuz personel talebi yönetici tarafından reddedilmiştir.</p>";
					text += "<p> Bilgilerinize. </p>";
					var mesaj = string.Format(text, userReq.firstname + " " + userReq.lastname, string.Format("{0:dd.MM.yyyy HH:mm}", allBilgiler.RequestDate), position == "" ? "Bulunamadı" : position);
					new Email().Template("Template1", "demo.jpg", "Cv Talebi Hakkında", mesaj)
							.Send((Int16)EmailSendTypes.PersonelAdayMulakat, userReq.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Cv Talebi Hakkında.."), true);
				}
			}

			var trans = db.BeginTransaction();
			var dbresult = db.UpdateHR_StaffNeeds(data, true, trans);
			dbresult &= db.InsertHR_StaffNeedsStatus(new HR_StaffNeedsStatus
			{
				staffNeedId = data.id,
				status = lastStatus,
				description = lastStatus == (Int32)EnumHR_StaffNeedsStatusStatus.YoneticiReddetti ? userStatus.user.FullName + " tarafından talep reddedilmiştir." : userStatus.user.FullName + " tarafından talep onaylanmıştır."
			}, trans);

			if (dbresult.result)
			{
				trans.Commit();
			}
			else
			{
				trans.Rollback();
			}

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Cv talebi yanıtlama işlemi başarı ile gerçekleşti.", false, Url.Action("MyIndex", "VWHR_StaffNeeds", new { area = "HR" })) : feedback.Error("Cv talebi yanıtlama işlemi başarısız oldu.")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[PageInfo("Personel Talebi Güncelleme", SHRoles.IdariPersonelYonetici)]
		public JsonResult UpdateStatus(Guid id, short lastStatus, string description)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();
			var userStatus = (PageSecurity)Session["userStatus"];

			var data = db.GetHR_StaffNeedsById(id);
			if (data == null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Talep bulunamadı.")
				}, JsonRequestBehavior.AllowGet);
			}

			var dbresult = db.InsertHR_StaffNeedsStatus(new HR_StaffNeedsStatus
			{
				staffNeedId = data.id,
				status = lastStatus,
				description = lastStatus == (Int32)EnumHR_StaffNeedsStatusStatus.YoneticiReddetti ?
			    "<p>" + userStatus.user.FullName + " tarafından talep reddedilmiştir." + "</p><p> Açıklama : " + description + "</p>" :
			    lastStatus == (Int32)EnumHR_StaffNeedsStatusStatus.TalepIptalEdildi ?
			    "<p>" + userStatus.user.FullName + " tarafından talep iptal edilmiştir." + "</p><p> Açıklama : " + description + "</p>" :
				"<p>" + userStatus.user.FullName + " tarafından talep onaylanmıştır." + "</p><p> Açıklama : " + description + "</p>"
			});

			if (dbresult.result && lastStatus == (Int32)EnumHR_StaffNeedsStatusStatus.YoneticiOnaylandi)
			{
				var demandingUser = "Kullanıcı Bulunamadı";
				var hrPerson = db.GetSH_UserByRoleId(SHRoles.IKYonetici);
				var url = TenantConfig.Tenant.GetWebUrl();
				var tenantName = TenantConfig.Tenant.TenantName;

				if (data.created != null)
				{
					var user = db.GetVWSH_UserById(data.createdby.Value);
					demandingUser = user.FullName != null ? user.FullName : demandingUser;
				}

				foreach (var item in hrPerson)
				{
					var text = "<h3>Merhaba {0},</h3>";
					text += "<p>{1} tarafından oluşturulan personel talep işlemi {2} tarafından onaylanmıştır.</p>";
					text += "<p>Onaylanan personel talebi için işlem yapmak veya cv yönlendirmek için <a href='{3}/HR/VWHR_StaffNeeds/DetailFor?id={4}'> buraya tıklayınız! </a></p>";
					text += "<p> Bilgilerinize. </p>";
					var mesaj = string.Format(text, item.FullName, demandingUser, userStatus.user.FullName, url, data.id);
					new Email().Template("Template1", "demo.jpg", "Onaylanan Personel Talebi Hakkında", mesaj)
				.Send((Int16)EmailSendTypes.PersonelAdayMulakat, item.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Onaylanan Personel Talebi"), true);
				}
			}

			if (dbresult.result && lastStatus == (Int32)EnumHR_StaffNeedsStatusStatus.TalepIptalEdildi)
			{
				var demandingUser = "Kullanıcı Bulunamadı";
				var hrPerson = db.GetSH_UserByRoleId(SHRoles.IKYonetici);
				var url = TenantConfig.Tenant.GetWebUrl();
				var tenantName = TenantConfig.Tenant.TenantName;

				if (data.created != null)
				{
					var user = db.GetVWSH_UserById(data.createdby.Value);
					demandingUser = user.FullName != null ? user.FullName : demandingUser;
				}

				foreach (var item in hrPerson)
				{
					var text = "<h3>Merhaba {0},</h3>";
					text += "<p>{1} tarafından oluşturulan personel talep işlemi {2} tarafından iptal edilmiştir.</p>";
					text += "<p> Bilgilerinize. </p>";
					var mesaj = string.Format(text, item.FullName, demandingUser, userStatus.user.FullName, url, data.id);
					new Email().Template("Template1", "demo.jpg", "İptal Edilen Personel Talebi Hakkında", mesaj)
				.Send((Int16)EmailSendTypes.PersonelAdayMulakat, item.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İptal Edilen Personel Talebi"), true);
				}
			}

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Talep cevaplama işlemi başarılı") : feedback.Warning("Talep cevaplama işlemi başarısız oldu.")

			}, JsonRequestBehavior.AllowGet);

		}


		[HttpPost]
		[PageInfo("Personel Talebi Silme")]
		public JsonResult Delete(string[] id)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();

			var item = id.Select(a => new HR_StaffNeeds { id = new Guid(a) });

			var dbresult = db.BulkDeleteHR_StaffNeeds(item);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("CV Havuzu Filtreleme", SHRoles.IKYonetici)]
		public ContentResult Search(StaffNeedsSearch filter)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();

			if (filter.Position2 == null && filter.Keywords2 == null && filter.EEducation == null && filter.City == null && filter.Military == null && filter.Exprience == null && filter.Result == null)
			{
				return Content(Infoline.Helper.Json.Serialize(new
				{
					Result = false,
					FeedBack = feedback.Warning("En az bir seçenek girerek sorgulamayı gerçekleştiriniz.")
				}), "aplication/json");
			}

			var AllSearch = db.GetData(filter.Position2, filter.Keywords2, filter.EEducation, filter.City, filter.Military, filter.Exprience, filter.Result);


			return Content(Infoline.Helper.Json.Serialize(new
			{
				Result = true,
				FeedBack = AllSearch.Count() > 0 ? feedback.Success("Aradığınız kriterlere uygun " + AllSearch.Count() + " adet kayıt bulunmuştur.") : feedback.Warning("Kayıt bulunamamıştır."),
				Object = AllSearch
			}), "aplication/json");
		}

		[PageInfo("Personel Talebi Yönledirme Methodu", SHRoles.IKYonetici)]
		public ActionResult PersonToNeed(Guid id)
		{
			return View(id);
		}



		[PageInfo("Personel Talebi Yönledirme Sayfası", SHRoles.IKYonetici)]
		public ActionResult PersonToManager(Guid id, Guid NeedId)
		{
			var db = new WorkOfTimeDatabase();
			var dataReq = db.GetHR_StaffNeedsRequesterByStaffNeedId(NeedId);
			var data = new VMPersonToManager
			{
				staffNeedPersonId = id,
				needId = NeedId,
				requestUsers = dataReq.Where(x => x.RequestId.HasValue).Select(x => x.RequestId.Value).ToArray()
			};
			return View(data);
		}

		[PageInfo("Personel Talebi Yönledirme Methodu", SHRoles.IKYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult PersonToManager(VMPersonToManager data)
		{
			var db = new WorkOfTimeDatabase();
			var feedBack = new FeedBack();
			var userStatus = (PageSecurity)Session["userStatus"];

			var control = db.GetHR_StaffNeedsPersonByNeedAndPersonId(data.staffNeedPersonId, data.needId);

			if (control != null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedBack.Warning("Aynı personeli aynı ihtiyaç için birden fazla yönlendiremezsiniz.")
				}, JsonRequestBehavior.AllowGet);
			}


			var needstatus = db.GetHR_StaffNeedsStatusByStaffNeedId(data.needId).Where(x => x.status == (Int32)EnumHR_StaffNeedsStatusStatus.TalebiSonlandir).FirstOrDefault();
			if (needstatus != null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedBack.Warning("Sonlandırılmış talebe cv yönlendiremezsiniz.")
				}, JsonRequestBehavior.AllowGet);
			}

			var hrStaffNeedsPerson = new HR_StaffNeedsPerson
			{
				id = Guid.NewGuid(),
				created = DateTime.Now,
				createdby = userStatus.user.id,
				HrPersonId = data.staffNeedPersonId,
				status = (Int32)EnumHR_StaffNeedsPersonstatus.Bos,
				HrStaffNeedsId = data.needId
			};

			var staffNeeds = db.GetHR_StaffNeedsById(data.needId);



			var users = db.GetVWSH_UserByIds(data.requestUsers);
			var url = TenantConfig.Tenant.GetWebUrl();
			var tenantName = TenantConfig.Tenant.TenantName;

			if (users.Count() > 0)
			{
				foreach (var user in users)
				{
					var text = "<h3>Merhaba {0},</h3>";
					text += "<p>Personel talepleriniz için insan kaynakları tarafından, tarafınıza personel adayı yönlendirilmiştir.</p>";
					text += "<p>Yönlendirilen personel adayını incelemek için <a href='{1}/HR/VWHR_StaffNeedsPerson/Update?id={2}'> Buraya tıklayınız! </a></p>";
					text += "<p> Bilgilerinize. </p>";
					var mesaj = string.Format(text, user.firstname + " " + user.lastname, url, hrStaffNeedsPerson.id);
					new Email().Template("Template1", "demo.jpg", "Personel Talebi İçin Yönlendirilen Personel Adayı Hakkında", mesaj)
				.Send((Int16)EmailSendTypes.PersonelAdayMulakat, user.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Personel Talebi İçin Yönlendirilen Personel Adayı"), true);
				}
			}

			var trans = db.BeginTransaction();

			var dbres = db.InsertHR_StaffNeedsPerson(hrStaffNeedsPerson, trans);
			var needRequester = db.GetHR_StaffNeedsRequesterByStaffNeedId(data.needId);
			if (needRequester.Count() > 0)
			{
				if (data.requestUsers != null && data.requestUsers.Count() > 0)
				{
					dbres &= db.BulkDeleteHR_StaffNeedsRequester(needRequester, trans);

					var newDataRequester = data.requestUsers.Select(x => new HR_StaffNeedsRequester
					{
						StaffNeedId = data.needId,
						RequestId = x,
						createdby = userStatus.user.id

					});

					dbres &= db.BulkInsertHR_StaffNeedsRequester(newDataRequester, trans);
				}

			}



			dbres &= db.UpdateHR_StaffNeeds(staffNeeds, true, trans);
			dbres &= db.InsertHR_StaffNeedsStatus(new HR_StaffNeedsStatus
			{
				staffNeedId = data.needId,
				status = (Int32)EnumHR_StaffNeedsStatusStatus.CvYonlendirildi,
				description = "<p>" + userStatus.user.FullName + " tarafından cv yönlendirildi.</p><p>Yönlendirilen cv detayı için <a target='_blank' href='/HR/VWHR_Person/Detail?id=" + data.staffNeedPersonId + "'> Buraya tıklayınız! </a></p>"
			}, trans);

			if (!dbres.result)
			{
				trans.Rollback();
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedBack.Warning("Yönlendirme işlemi başarısız oldu.")
				},
			JsonRequestBehavior.AllowGet);
			}

			trans.Commit();
			return Json(new ResultStatusUI
			{
				Result = true,
				FeedBack = feedBack.Success("Başarı ile yönlendirme gerçekleştirdiniz.")
			},
			JsonRequestBehavior.AllowGet);
		}


		[PageInfo("CV Havuzunda Filtreleme Methodu", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici, SHRoles.PersonelTalebi)]
		public ActionResult NeedDetail(string NeedCode)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWHR_StaffNeeds().Where(c => c.NeedCode == NeedCode).FirstOrDefault();
			return RedirectToAction("Detail", "VWHR_StaffNeeds", new { area = "HR", id = data.id });

		}


		[PageInfo("Personel İhtiyaç Formu", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
		public ActionResult NeedForm(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var model = new VMHR_StaffNeedsAndReview();
			model.Logo = TenantConfig.Tenant.Logo;
			model.VWHR_StaffNeeds = db.GetVWHR_StaffNeedsById(id);
			model.Keywords = "";
			model.Positions = "";
			if (model.VWHR_StaffNeeds != null)
			{
				var keywords = db.GetHR_StaffNeedsKeywordsByNeedsId(id);
				var positions = db.GetHR_StaffNeedsPositionByNeedsId(id);
				if (keywords.Count() > 0)
				{
					var keywordsName = db.GetHR_KeywordsByIds(keywords.Select(f => f.HrKeywordsId.Value).ToArray());
					model.Keywords = string.Join(", ", keywordsName.Select(c => c.Name));
				}
				if (positions.Count() > 0)
				{
					var positionsName = db.GetHR_PositionByIds(positions.Select(v => v.HrPositionId.Value).ToArray());
					model.Positions = string.Join(", ", positionsName.Select(c => c.Name));
				}
			}
			return View(model);
		}

		[PageInfo("Talebi Sonlandırma Methodu", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici, SHRoles.PersonelTalebi)]
		[HttpPost]
		public JsonResult FinishRead(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();
			var userStatus = (PageSecurity)Session["userStatus"];
			var staffneedsStatus = db.GetHR_StaffNeedsStatusByStaffNeedId(id).Where(c => c.status == (Int32)EnumHR_StaffNeedsStatusStatus.TalepIptalEdildi || c.status == (Int32)EnumHR_StaffNeedsStatusStatus.TalebiSonlandir).FirstOrDefault();
			if (staffneedsStatus != null)
			{
				if (staffneedsStatus.status == (Int32)EnumHR_StaffNeedsStatusStatus.TalepIptalEdildi)
				{
					return Json(new ResultStatusUI
					{
						Result = false,
						FeedBack = feedback.Warning("Talep iptal edilmiştir. İptal olan talep sonlandırılamaz.")
					}, JsonRequestBehavior.AllowGet);
				}

				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Aynı talep daha önce sonlandırılmış.")
				}, JsonRequestBehavior.AllowGet);

			}

			var status = new HR_StaffNeedsStatus
			{
				staffNeedId = id,
				status = (Int32)EnumHR_StaffNeedsStatusStatus.TalebiSonlandir,
				description = userStatus.user.FullName + " tarafından talep sonlandırılmıştır."
			};

			var dbres = db.InsertHR_StaffNeedsStatus(status);
			return Json(new ResultStatusUI
			{
				Result = dbres.result,
				FeedBack = dbres.result ? feedback.Success("Talep başarı ile sonlandırıldı.") : feedback.Warning("Talep sonlandırma işlemi başarısız oldu.")
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Yeni Cv talep methodu", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici, SHRoles.PersonelTalebi)]
		[HttpPost]
		public JsonResult ReturnRequest(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();
			var userStatus = (PageSecurity)Session["userStatus"];
			var staffneedsStatus = db.GetHR_StaffNeedsStatusByStaffNeedId(id);
			var staffneedsStatusEnd = staffneedsStatus.Where(c => c.status == (Int32)EnumHR_StaffNeedsStatusStatus.TalebiSonlandir).FirstOrDefault();
			if (staffneedsStatusEnd != null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Talep daha önce sonlandırılmış.Yeni talep gerçekleştirin.")
				}, JsonRequestBehavior.AllowGet);
			}

			var staffnew = staffneedsStatus.FirstOrDefault();
			if (staffnew != null && staffnew.status == (Int32)EnumHR_StaffNeedsStatusStatus.TalepGerceklestirildi)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Yönetici onayı beklenirken yeni cv talebinde bulunamazsınız.")
				}, JsonRequestBehavior.AllowGet);
			}

			var staffneedsStatusReq = staffneedsStatus.Skip(0).Take(1).FirstOrDefault();
			if (staffneedsStatusReq != null && (staffneedsStatusReq.status == (Int32)EnumHR_StaffNeedsStatusStatus.YeniCvTalepEdildi))
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Yeni cv daha önce talep edilmiş.")
				}, JsonRequestBehavior.AllowGet);
			}

			if (staffneedsStatusReq != null && staffneedsStatusReq.status == (Int32)EnumHR_StaffNeedsStatusStatus.YoneticiReddetti)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Yönetici tarafından reddedilen talebe yeni cv talebi gerçekleştiremezsiniz.")
				}, JsonRequestBehavior.AllowGet);
			}

			var status = new HR_StaffNeedsStatus
			{
				staffNeedId = id,
				status = (Int32)EnumHR_StaffNeedsStatusStatus.YeniCvTalepEdildi,
				description = userStatus.user.FullName + " tarafından yeni cv talebi gerçekleştirilmiştir."
			};

			var dbres = db.InsertHR_StaffNeedsStatus(status);
			return Json(new ResultStatusUI
			{
				Result = dbres.result,
				FeedBack = dbres.result ? feedback.Success("Yeni cv talep etme işlemi başarı ile gerçekleşti.") : feedback.Warning("Yeni cv talep etme işlemi başarısız oldu.")
			}, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("İhtiyaç Çizelge Sayfası", SHRoles.PersonelTalebi, SHRoles.IdariPersonelYonetici, SHRoles.IKYonetici)]
		public ActionResult Timeline(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var staffNeedsStatus = db.GetHR_StaffNeedsStatusByStaffNeedId(id);
			return View(staffNeedsStatus);
		}
	}
}
