using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.INV.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static Infoline.WorkOfTime.WebProject.Areas.INV.Models.INV_PermitModel;
namespace Infoline.WorkOfTime.WebProject.Areas.INV.Controllers
{
	public class VWINV_PermitController : Controller
	{

		[PageInfo("Tüm İzinler", SHRoles.IKYonetici)]
		public ActionResult Index()
		{
			var db = new WorkOfTimeDatabase();
			var result = db.GetINV_PermitType();
			return View(result);
		}

		[PageInfo("İzin Taleplerim", SHRoles.Personel)]
		public ActionResult MyIndex()
		{
			var db = new WorkOfTimeDatabase();
			var result = db.GetINV_PermitType();
			return View(result);
		}

		[PageInfo("İzin Talepleri (Onay)", SHRoles.Personel)]
		public ActionResult MyAboutIndex()
		{
			var db = new WorkOfTimeDatabase();
			var result = db.GetINV_PermitType();
			return View(result);
		}

		[PageInfo("İzinler DataSource", SHRoles.Personel)]
		public JsonResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWINV_Permit(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWINV_PermitCount(condition.Filter);
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("İzinler Adet Methodu", SHRoles.Personel)]
		public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var count = db.GetVWINV_PermitCount(condition.Filter);
			return count;
		}

		[PageInfo("İzin Detayı", SHRoles.Personel)]
		public ActionResult Detail(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWINV_PermitById(id);

			if (data == null)
			{
				return RedirectToAction("MyAboutIndex");
			}

			var model = new VWINV_PermitForm
			{
				User = db.GetVWSH_UserById(data.IdUser.Value),
				Calc = INV_PermitCalculator.Calculate(data),
			}.EntityDataCopyForMaterial(data, true);

			return View(model);
		}

		[PageInfo("Yeni izin talebi", SHRoles.Personel)]
		public ActionResult Insert()
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			if (userStatus.user.type != (int)EnumSH_UserType.MyPerson || userStatus.user.IsWorking == false)
			{
				new FeedBack().Warning("Şuan aktif olarak herangi bir şirkette çalışmıyor gözüküyorsunuz.Bunun bir hata olduğunu düşünüyorsanız lütfen insan kaynkları yöneticiniz ile görüşünüz.", true);
				return RedirectToAction("Index", "Account", new { area = string.Empty });
			}

			var feedback = new FeedBack();
			var now = DateTime.Now;
			var start = INV_PermitCalculator.CommencementDate(new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));
			var endTime = TenantConfig.Tenant.Config.WorkingTimes[start.DayOfWeek].allowTimes.OrderBy(a => a.End).Select(a => a.End).LastOrDefault();
			var end = new DateTime(start.Year, start.Month, start.Day, endTime.Hours, endTime.Minutes, 0);
			var insankaynaklari = db.GetSH_UserByRoleId(SHRoles.IKYonetici).OrderBy(a => a.CompanyId == userStatus.user.CompanyId).FirstOrDefault();


			var model = new VWINV_PermitForm
			{
				id = Guid.NewGuid(),
				PermitCode = BusinessExtensions.B_GetIdCode(),
				IdUser = userStatus.user.id,
				created = DateTime.Now,
				createdby = userStatus.user.id,
				StartDate = start,
				EndDate = end,
				createdby_Title = userStatus.user.firstname + " " + userStatus.user.lastname,
				Person_Title = userStatus.user.firstname + " " + userStatus.user.lastname,
				PermitTypeId = INV_PermitCalculator.yillikIzin,
				AccessPhone = userStatus.user.cellphone,
				ArriveAdress = userStatus.user.address,
				IkApproval = insankaynaklari != null ? insankaynaklari.id : (Guid?)null,
				IkApproval_Title = insankaynaklari != null ? insankaynaklari.FullName : null,
				Manager1Approval = userStatus.user.Manager1,
				Manager1Approval_Title = userStatus.user.Manager1_Title,
				Manager2Approval = userStatus.user.Manager2,
				Manager2Approval_Title = userStatus.user.Manager2_Title,
			};

			return View(model);
		}

		[PageInfo("Yeni izin talebi", SHRoles.Personel)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(INV_Permit item)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();
			var userStatus = (PageSecurity)Session["userStatus"];
			item.created = DateTime.Now;
			item.createdby = userStatus.user.id;
			if (!item.IdUser.HasValue)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Herhangi bir firmada çalışmıyorsunuz.")
				}, JsonRequestBehavior.AllowGet);
			}
			var personel = db.GetVWSH_UserById(item.IdUser.Value);
			var IKYonetici = db.GetSH_UserByRoleId(SHRoles.IKYonetici).OrderBy(a => a.CompanyId == personel.CompanyId).FirstOrDefault();
			var isIK = IKYonetici.id == item.createdby ? true : false;
			var result = new ResultStatus();

			result = new INV_PermitModelBusiness().Insert(item);

			if (result.result)
			{
				new FileUploadSave(Request).SaveAs();
			}
			return Json(new ResultStatusUI
			{
				Result = result.result,
				FeedBack = result.result ? feedback.Success(result.message, false, Url.Action((isIK ? "Index" : "MyIndex"), "VWINV_Permit")) : feedback.Warning(result.message)
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("İzin Talebi Onay/Red", SHRoles.Personel)]
		public ActionResult Update(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWINV_PermitById(id);
			if (data == null)
			{
				return RedirectToAction("MyAboutIndex");
			}
			var userStatus = (PageSecurity)Session["userStatus"];
			data.changedby = userStatus.user.id;

			var hasHr = userStatus.AuthorizedRoles.Count(x => x == new Guid(SHRoles.IKYonetici)) > 0 ? true : false;

			if (!(data.Manager1Approval == userStatus.user.id || data.Manager2Approval == userStatus.user.id || data.IkApproval == userStatus.user.id || data.IdUser == userStatus.user.id || hasHr))
			{
				return RedirectToAction("MyAboutIndex");
			}

			var model = new VWINV_PermitForm
			{
				User = db.GetVWSH_UserById(data.IdUser.Value),
				Calc = INV_PermitCalculator.Calculate(data),
			}.EntityDataCopyForMaterial(data, true);


			if (data.ApproveStatus == (int)EnumINV_PermitApproveStatus.TalepEdildi && userStatus.user.id == data.Manager1Approval)
			{
				ViewBag.Onay = EnumINV_PermitApproveStatus.Yonetici1Onay;
				ViewBag.Red = EnumINV_PermitApproveStatus.Yonetici1Red;
			}
			else if (data.ApproveStatus == (int)EnumINV_PermitApproveStatus.Yonetici1Onay && userStatus.user.id == data.Manager2Approval)
			{
				ViewBag.Onay = EnumINV_PermitApproveStatus.Yonetici2Onay;
				ViewBag.Red = EnumINV_PermitApproveStatus.Yonetici2Red;
			}
			else if (data.ApproveStatus == (int)EnumINV_PermitApproveStatus.Yonetici2Onay && userStatus.user.id == data.IkApproval)
			{
				ViewBag.Onay = EnumINV_PermitApproveStatus.IkKontrol;
				ViewBag.Red = EnumINV_PermitApproveStatus.IkKontrolRed;
			}

			return View(model);
		}


		[PageInfo("İzin Talebi Onay/Red", SHRoles.Personel)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(INV_Permit item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var permit = db.GetVWINV_PermitById(item.id);
			var permitUser = db.GetVWSH_UserById(permit.IdUser.Value);
			var notification = new BusinessAccess.Notification();
			permit.changed = DateTime.Now;
			permit.changedby = userStatus.user.id;
			permit.ApproveStatus = item.ApproveStatus;
			permit.ApproveDetail = item.ApproveDetail;

			var compPersonDept = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(permit.IdUser.Value, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();
			var insankaynaklari = db.GetSH_UserByRoleId(SHRoles.IKYonetici).OrderBy(a => a.CompanyId == permitUser.CompanyId).FirstOrDefault();
			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Onay || permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Red)
			{
				permit.Manager2Approval = compPersonDept.Manager2;
				permit.Manager1ApprovalDate = DateTime.Now;
				if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Onay && !permit.Manager2Approval.HasValue)
				{
					permit.ApproveStatus = (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay;
					permit.Manager2ApprovalDate = DateTime.Now;
				}
			}


			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay || permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Red)
			{
				if (insankaynaklari != null)
				{
					permit.IkApproval = insankaynaklari.id;
				}
				permit.Manager2ApprovalDate = DateTime.Now;
				if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay && !permit.IkApproval.HasValue)
				{
					permit.ApproveStatus = (Int32)EnumINV_PermitApproveStatus.IkKontrol;
					permit.IkApprovalDate = DateTime.Now;
				}
			}

			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrolRed)
			{
				permit.IkApprovalDate = DateTime.Now;
			}

			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi && (permit.IdUser == userStatus.user.id || insankaynaklari.id == userStatus.user.id ||
				(userStatus.AuthorizedRoles.Count(v => v == new Guid(SHRoles.IKYonetici))) > 0))
			{
				var rsFile = new FileUploadSave(Request).SaveAs();
				if (rsFile.Result == false)
				{
					return Json(rsFile, JsonRequestBehavior.AllowGet);
				}
			}

			//if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi && )
			//{
			//    var rsFile = new FileUploadSave(Request).SaveAs();
			//    if (rsFile.Result == false)
			//    {
			//        return Json(rsFile, JsonRequestBehavior.AllowGet);
			//    }
			//}

			var dbresults = db.UpdateINV_Permit(new INV_Permit().EntityDataCopyForMaterial(permit, true));

			if (!dbresults.result)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning(dbresults.message)
				}, JsonRequestBehavior.AllowGet);
			}

			var start = string.Format("{0:dd.MM.yyyy HH:mm}", permit.StartDate);
			var end = string.Format("{0:dd.MM.yyyy HH:mm}", permit.EndDate);
			var changed = string.Format("{0:dd.MM.yyyy HH:mm}", permit.changed);
			var calc = INV_PermitCalculator.Calculate(new INV_Permit().EntityDataCopyForMaterial(permit, true));
			var url = TenantConfig.Tenant.GetWebUrl();
			var tenantName = TenantConfig.Tenant.TenantName;

			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi && permit.IdUser == userStatus.user.id && insankaynaklari != null)
			{

				var text = @"<h3>Sayın {0},</h3> 
                <p><u>{1}</u> isimli personel {2} tarihinde, {3} - {4} tarihleri arasında onaylanan izni için ıslak imzalı dosyasını yüklemiştir. </p>
                <p>Kontrol etmek için lütfen <a href='{5}/INV/VWINV_Permit/Detail?id={6}'>Buraya tıklayınız! </a></p>
                <p>Bilgilerinize.<br>İyi Çalışmalar.</p>";
				var mesaj = string.Format(text, permit.IkApproval_Title, permit.Person_Title, changed, start, end, url, permit.id);
				var notify = string.Format("Sayın {0}, {1} isimli personel {2} tarihinde, {3} - {4} tarihleri arasında onaylanan izni ısalk imzalı dosyasını yüklemiştir",permit.IkApproval_Title,permit.Person_Title,changed,start,end);
				new Email().Template("Template1", "working.jpg", "İzin Talebi Islak İmzalı Dosyası Hakkında", mesaj)
				.Send((Int16)EmailSendTypes.IzinOnaylari, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Islak İmzalı Dosyası Hakkında.."), true);
				notification.NotificationSend(insankaynaklari.id, "İzin Talebi Islak İmzalı Dosyası Hakkında", notify);
				if (userStatus.user.id == insankaynaklari.id)
				{
					return Json(new ResultStatusUI
					{
						Result = true,
						FeedBack = feedback.Success("Dosya yükleme işleminiz başarı ile gerçekleşti.", false, Url.Action("Index", "VWINV_Permit"))
					}, JsonRequestBehavior.AllowGet);
				}

				return Json(new ResultStatusUI
				{
					Result = true,
					FeedBack = feedback.Success("Dosya yükleme işleminiz başarı ile gerçekleşti.", false, Url.Action("MyIndex", "VWINV_Permit"))
				}, JsonRequestBehavior.AllowGet);

			}

			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Onay)
			{
				var manager2 = db.GetVWSH_UserById(permit.Manager2Approval.Value);
				var text = @"<h3>Sayın {0},</h3>
                <p><u>{1}</u> isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur.</p>
                <p>Onaya gitmek için lütfen <a href='{7}/INV/VWINV_Permit/Update?id={8}'>Buraya tıklayınız! </a><br/>Bilgilerinize.</p>";
				var mesaj = string.Format(text, permit.Manager2Approval_Title, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title, url, permit.id);
				var notify = string.Format("Sayın {0}, {1} isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur. ", permit.Manager2Approval_Title, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title);
				new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
				  .Send((Int16)EmailSendTypes.IzinOnaylari, manager2.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Onayı Hakkında.."), true);
				notification.NotificationSend(manager2.id, "İzin Talebi Onayı Hakkında", notify);
			}

			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay)
			{
				var text = @"<h3>Sayın {0},</h3>
                <p><u>{1}</u> isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur.</p>
                <p>Yöneticileri <strong>{7}</strong> ve <strong>{8}</strong> tarafından onaylanmıştır.Sistem üzerinden kontrolünüz beklenmektedir.</p>
                <p>Onaya gitmek için lütfen <a href='{9}/INV/VWINV_Permit/Update?id={10}'>Buraya tıklayınız! </a><br/>Bilgilerinize.</p>";
				var mesaj = string.Format(text, permit.IkApproval_Title, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title, permit.Manager1Approval_Title, permit.Manager2Approval_Title, url, permit.id);
				var notify = string.Format("Sayın {0}, {1} isimli personel {2} tarihinde, {3} - {4} tarihleri arasında {5} {6} talebinde bulunmuştur. Yöneticileri  {7} ve {8} tarafından onaylanmıştır.Sistem üzerinden kontrolünüz beklenmektedir. ", permit.Manager2Approval_Title, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title, permit.Manager1Approval_Title, permit.Manager2Approval_Title);
				new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
			   .Send((Int16)EmailSendTypes.IzinOnaylari, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Onayı Hakkında.."), true);
				notification.NotificationSend(insankaynaklari.id, "İzin Talebi Onayı Hakkında", notify);
			}

			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Red || permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Red)
			{

				var text = @"<h3>Sayın {0},</h3>
                <p>{1} tarihinde talep ettiğiniz <u> {2} - {3} </u> tarihleri arasındaki {4} {5}, yöneticileriniz tarafından <font color='red'>reddedilmiştir.</font></p>
                <p>Bilgilerinize.<br>İyi Çalışmalar.</p>";
				var mesaj = string.Format(text, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title);
				var notify = String.Format("Sayın {0}, {1} tarihinde talep ettiğiniz {2} - {3} </u> tarihleri arasındaki {4} {5}, yöneticileriniz tarafından reddedilmiştir  ", permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title);
				new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
				  .Send((Int16)EmailSendTypes.IzinOnaylari, permitUser.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Hakkında.."), true);
				notification.NotificationSend(permitUser.id, "İzin Talebi Onayı Hakkında", notify);
			}

			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrolRed)
			{
				var text = @"<h3>Sayın {0},</h3>
                <p>{1} tarihinde talep ettiğiniz <u> {2} - {3} </u> tarihleri arasındaki {4} {5}, insan kaynakları tarafından <font color='red'>reddedilmiştir.</font></p>
                <p>Bilgilerinize.<br>İyi Çalışmalar.</p>";
				var notify = String.Format("Sayın {0}, {1} tarihinde talep ettiğiniz {2} - {3} </u> tarihleri arasındaki {4} {5}, insan kaynakları tarafından reddedilmiştir  ", permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title);
				var mesaj = string.Format(text, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title);
				notification.NotificationSend(permitUser.id, "İzin Talebi Onayı Hakkında", notify);
				new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
				 .Send((Int16)EmailSendTypes.IzinOnaylari, permitUser.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Hakkında.."), true);
			}

			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol)
			{
				var text = @"<h3>Sayın {0},</h3> 
                    <p>{1} tarihinde talep ettiğiniz <u> {2} - {3} </u> tarihleri arasındaki {4} {5}  yöneticileriniz {6} ve insan kaynakları tarafından onaylanmıştır.</p>
                    <p>İzin sürecinizin devam edebilmesi için ıslak imzalı izin formunuzu yüklemelisiniz. Yüklemek için <a href='{7}/INV/VWINV_Permit/MyIndex'>Buraya tıklayınız! </a><br/>Bilgilerinize.<br>İyi Çalışmalar.</p>";
				var mesaj = string.Format(text, permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title, permit.Manager1Approval_Title + "," + permit.Manager2Approval_Title, url);
				var notify = string.Format("Sayın {0}, {1} tarihinde talep ettiğiniz {2} - {3} tarihleri arasındaki {4} {5}, yöneticileriniz {6} ve  insan kaynakları tarafından onaylanmıştır  ", permit.Person_Title, changed, start, end, calc.Text, permit.PermitType_Title, permit.Manager1Approval_Title + "," + permit.Manager2Approval_Title);
				new Email().Template("Template1", "izinMailFoto.jpg", "İzin Talebi Onayı Hakkında", mesaj)
					.Send((Int16)EmailSendTypes.IzinSurecTamamlama, permitUser.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "İzin Talebi Hakkında.."), true);
				notification.NotificationSend(permitUser.id, "İzin Talebi Onayı Hakkında", notify);
			}

			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi && insankaynaklari != null && userStatus.user.id == insankaynaklari.id)
			{
				return Json(new ResultStatusUI
				{
					Result = true,
					FeedBack = feedback.Success("Islak imzalı dosya yükleme işlemi başarılı", false, Url.Action("Index", "VWINV_Permit"))
				}, JsonRequestBehavior.AllowGet);
			}
			if (permit.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi && (userStatus.AuthorizedRoles.Where(v => v == new Guid(SHRoles.IKYonetici))).Count() > 0)
			{
				return Json(new ResultStatusUI
				{
					Result = true,
					FeedBack = feedback.Success("Islak imzalı dosya yükleme işlemi başarılı", false, Url.Action("Index", "VWINV_Permit"))
				}, JsonRequestBehavior.AllowGet);
			}


			return Json(new ResultStatusUI
			{
				Result = true,
				FeedBack = feedback.Success("İzin yanıtlama işlemi başarılı", false, Url.Action("MyAboutIndex", "VWINV_Permit"))
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[PageInfo("İzin Talebi Silme", SHRoles.Personel)]
		public JsonResult Delete(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["UserStatus"];
			var feedback = new FeedBack();
			var item = db.GetINV_PermitById(id);

			var insankaynaklari = db.GetSH_UserByRoleId(SHRoles.IKYonetici).ToArray();
			if (item == null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Silmeye çalışılan izin bulunamadı.")
				}, JsonRequestBehavior.AllowGet);
			}

			if (item.ApproveStatus != (int)EnumINV_PermitApproveStatus.TalepEdildi)
			{
				if (insankaynaklari.Count() > 0)
				{
					if (!insankaynaklari.Select(x => x.id).Contains(userStatus.user.id))
					{
						if (!userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SistemYonetici)))
						{
							return Json(new ResultStatusUI
							{
								Result = false,
								FeedBack = feedback.Warning("Sadece işlem yapılmamış izinler silinebilir.İşlem yapılmış izinler sadece insan kaynakları tarafından silinebilmektedir.")
							}, JsonRequestBehavior.AllowGet);
						}
					}
				}
				else
				{
					return Json(new ResultStatusUI
					{
						Result = false,
						FeedBack = feedback.Warning("Sadece işlem yapılmamış izinler silinebilir.")
					}, JsonRequestBehavior.AllowGet);
				}
			}



			var dbresult = db.DeleteINV_Permit(item);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarısız")
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("İzin Talepleri Formu", SHRoles.Personel)]
		public ActionResult PermitForm(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWINV_PermitById(id);

			if (data != null && data.IdUser.HasValue)
			{
				var user = db.GetVWSH_UserById(data.IdUser.Value);

				if (user == null)
				{
					new FeedBack().Warning("Seçmiş olduğunuz form sahibi personel bulunamadı. Lütfen, yöneticiniz ile görüşün.", true);
					return RedirectToAction("Index");
				}

				else
				{
					var file = "";
					var company = db.GetVWINV_CompanyPersonLastByUserId(data.IdUser.Value);
					if (company != null)
					{
						var sysLogo = db.GetSysFilesFilePathByDataTableAndFileGroupAndDataId("CMP_Company", "İşletme Logosu", company.CompanyId.Value);
						file = sysLogo != null ? sysLogo.FilePath : "";
					}

					var model = new VWINV_PermitForm
					{
						User = user,
						Department = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(data.IdUser.Value, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments(),
						Calc = INV_PermitCalculator.Calculate(data),
						Url = TenantConfig.Tenant.GetWebUrl(),
						LogoPath = file
					}.EntityDataCopyForMaterial(data, true);


					return View(model);
				}
			}
			else
			{
				new FeedBack().Warning("Seçmiş olduğunuz form sahibi personel bulunamadı. Lütfen, yöneticiniz ile görüşün.", true);
				return RedirectToAction("MyAboutIndex");
			}

		}

		[PageInfo("Sağlık İzini Ekleme", SHRoles.IKYonetici)]
		public ActionResult HealthPermit(Guid? IdUser)
		{
			var now = DateTime.Now;
			var start = INV_PermitCalculator.CommencementDate(new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));
			var endTime = TenantConfig.Tenant.Config.WorkingTimes[start.DayOfWeek].allowTimes.OrderBy(a => a.End).Select(a => a.End).LastOrDefault();
			var end = new DateTime(start.Year, start.Month, start.Day, endTime.Hours, endTime.Minutes, 0);


			var model = new VWINV_PermitForm
			{
				id = Guid.NewGuid(),
				PermitCode = BusinessExtensions.B_GetIdCode(),
				PermitType_Title = "Sağlık Raporu / Vizite İzni",
				IdUser = IdUser,
				StartDate = start,
				EndDate = end,
				PermitTypeId = new Guid("9022A2F3-EED3-477C-A57E-21A2A37F42D5")
			};

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Sağlık İzini Ekleme.", SHRoles.IKYonetici)]
		public JsonResult HealthPermit(INV_Permit item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var user = db.GetVWSH_UserById(item.IdUser.Value);
			var feedback = new FeedBack();
			var calc = INV_PermitCalculator.Calculate(item);
			var IKYonetici = db.GetSH_UserByRoleId(SHRoles.IKYonetici).OrderBy(a => a.CompanyId == user.CompanyId).FirstOrDefault();
			var Department = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(item.IdUser.Value, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();

			item.created = DateTime.Now;
			item.createdby = userStatus.user.id;
			item.ApproveStatus = (Int32)EnumINV_PermitApproveStatus.SaglikRaporu;
			item.ArriveAdress = user.locationId_Title;
			item.AccessPhone = user.cellphone;
			item.TotalDays = calc.TotalDay;
			item.TotalHours = calc.TotalHour;
			item.Manager1ApprovalDate = item.created;
			item.Manager2ApprovalDate = item.created;
			item.IkApprovalDate = item.created;
			item.IkApproval = IKYonetici.id;
			item.PermitTypeId = new Guid("9022A2F3-EED3-477C-A57E-21A2A37F42D5");

			item.Manager1Approval = Department.Manager1;
			item.Manager2Approval = Department.Manager2;


			var validate = INV_PermitCalculator.Validate(item, user, IKYonetici);

			if (validate.result == false)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning(validate.message)
				}, JsonRequestBehavior.AllowGet);
			}

			var rsFile = new FileUploadSave(Request).SaveAs();
			var dbresult = db.InsertINV_Permit(item);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = feedback.Success("Sağlık izni ekleme işlemi gerçekleşti.", false, Request.UrlReferrer.AbsolutePath.IndexOf("HealthPermit") > -1 ? Url.Action("Index", "VWINV_Permit", new { area = "INV" }) : null)
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Avans İzini Ekleme", SHRoles.IKYonetici)]
		public ActionResult AllowancePermit(Guid? IdUser)
		{
			var now = DateTime.Now;
			var start = INV_PermitCalculator.CommencementDate(new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));
			var endTime = TenantConfig.Tenant.Config.WorkingTimes[start.DayOfWeek].allowTimes.OrderBy(a => a.End).Select(a => a.End).LastOrDefault();
			var end = new DateTime(start.Year, start.Month, start.Day, endTime.Hours, endTime.Minutes, 0);


			var model = new VWINV_PermitForm
			{
				id = Guid.NewGuid(),
				PermitCode = BusinessExtensions.B_GetIdCode(),
				PermitType_Title = "Yıllık İzin",
				StartDate = start,
				EndDate = end,
				PermitTypeId = new Guid("F8547488-3215-1235-5126-EF2323CBBCB2"),
				IdUser = IdUser ?? IdUser

			};

			return View(model);
		}

		[PageInfo("Yıllık Personel İzin Raporu", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
		public ActionResult YearlyStaffReport()
		{
			return View();
		}

		[PageInfo("Aylık Görev Raporu DataSource Methodu", SHRoles.Personel)]
		public JsonResult YearlyStaffReportDataSource(int year, List<Guid?> userIds)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];

			var permit = db.GetVWINV_PermitByDateAndUserIds(year, userIds);

			var pieChartReport = permit.GroupBy(x => x.PermitType_Title).Select(x => new
			{
				permitType_Title = x.Select(a => a.PermitType_Title).FirstOrDefault(),
				TotalDays = x.Sum(a => a.TotalDays)
			});

			var dailyReportDatas = permit.Select(c => new
			{
				personnelName = c.Person_Title,
				startDate = c.StartDate.HasValue ? c.StartDate.Value.ToString("dd/MM/yyyy") : "",
				endDate = c.EndDate.HasValue ? c.EndDate.Value.ToString("dd/MM/yyyy") : "",
				permitType_Title = c.PermitType_Title,
				TotalHoursText = c.TotalHoursText,
				TotalDays = c.TotalDays,
				pieChartReport = pieChartReport
			});
			return Json(dailyReportDatas.OrderBy(x => x.personnelName), JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Aylık Görev Raporu Chart DataSource Methodu", SHRoles.Personel)]
		public JsonResult YearlyStaffReportChartDataSource(int year, List<Guid?> userIds)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];

			var permit = db.GetVWINV_PermitByDateAndUserIds(year, userIds);

			var pieChartReport = permit.GroupBy(x => x.PermitType_Title).Select(x => new
			{
				permitType_Title = x.Select(a => a.PermitType_Title).FirstOrDefault(),
				TotalDays = x.Sum(a => Math.Round(a.TotalDays.Value)),
				TotalHours = x.Sum(a=> Math.Round(a.TotalHours.Value)),
				MostUsedPermission = permit.OrderByDescending(a => a.PermitTypeId).Select(a => a.PermitType_Title).FirstOrDefault()
			});

			return Json(pieChartReport, JsonRequestBehavior.AllowGet);
		}


		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Avans İzini Ekleme", SHRoles.IKYonetici)]
		public JsonResult AllowancePermit(INV_Permit item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var user = db.GetVWSH_UserById(item.IdUser.Value);
			var feedback = new FeedBack();
			var calc = INV_PermitCalculator.Calculate(item);
			var IKYonetici = db.GetSH_UserByRoleId(SHRoles.IKYonetici).OrderBy(a => a.CompanyId == user.CompanyId).ToArray();
			var Department = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(item.IdUser.Value, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();

			var ik = IKYonetici.Where(x => x.id == userStatus.user.id).FirstOrDefault();


			if (ik == null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("IK Yöneticisi dışında personeller avans izin ekleyemezler.")
				}, JsonRequestBehavior.AllowGet);
			}

			item.created = DateTime.Now;
			item.createdby = userStatus.user.id;
			item.ApproveStatus = (Int32)EnumINV_PermitApproveStatus.AvansIzin;
			item.ArriveAdress = user.locationId_Title;
			item.AccessPhone = user.cellphone;
			item.TotalDays = calc.TotalDay;
			item.TotalHours = calc.TotalHour;
			item.IkApprovalDate = item.created;
			item.IkApproval = ik != null ? ik.id : IKYonetici.FirstOrDefault()?.id;
			item.PermitTypeId = new Guid("F8547488-3215-1235-5126-EF2323CBBCB2");

			item.Manager1ApprovalDate = item.created;
			item.Manager2ApprovalDate = item.created;

			item.Manager1Approval = Department.Manager1;
			item.Manager2Approval = Department.Manager2;


			var validate = INV_PermitCalculator.Validate(item, user, (ik != null ? ik : IKYonetici.FirstOrDefault()));

			if (validate.result == false)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning(validate.message)
				}, JsonRequestBehavior.AllowGet);
			}

			var rsFile = new FileUploadSave(Request).SaveAs();
			var dbresult = db.InsertINV_Permit(item);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = feedback.Success("Avans izin ekleme işlemi gerçekleşti.")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Geçmişe Dönük Yıllık İzinleri Ekleme", SHRoles.IKYonetici)]
		public ActionResult PastYearlyPermits(Guid? IdUser)
		{
			var db = new WorkOfTimeDatabase();
			var now = DateTime.Now.AddDays(-1);
			var start = INV_PermitCalculator.CommencementDate(new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));
			var endTime = TenantConfig.Tenant.Config.WorkingTimes[start.DayOfWeek].allowTimes.OrderBy(a => a.End).Select(a => a.End).LastOrDefault();
			var end = new DateTime(start.Year, start.Month, start.Day, endTime.Hours, endTime.Minutes, 0);



			var model = new VWINV_PermitForm
			{
				id = Guid.NewGuid(),
				PermitCode = BusinessExtensions.B_GetIdCode(),
				IdUser = IdUser,
				StartDate = start,
				EndDate = end,
				PermitTypeId = INV_PermitCalculator.yillikIzin

			};
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Geçmişe Dönük Yıllık İzinleri Ekleme", SHRoles.IKYonetici)]
		public JsonResult PastYearlyPermits(INV_Permit item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var user = db.GetVWSH_UserById(item.IdUser.Value);
			var feedback = new FeedBack();
			var calc = INV_PermitCalculator.Calculate(item);
			var IKYonetici = db.GetSH_UserByRoleId(SHRoles.IKYonetici).OrderBy(a => a.CompanyId == user.CompanyId).FirstOrDefault();

			var Department = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(item.IdUser.Value, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();

			item.created = DateTime.Now;
			item.createdby = userStatus.user.id;
			item.ApproveStatus = (Int32)EnumINV_PermitApproveStatus.IkKontrol;
			item.ArriveAdress = user.locationId_Title;
			item.AccessPhone = user.cellphone;

			item.TotalDays = calc.TotalDay;
			item.TotalHours = calc.TotalHour;
			item.Manager1ApprovalDate = item.created;
			item.Manager2ApprovalDate = item.created;
			item.IkApprovalDate = item.created;
			item.IkApproval = IKYonetici.id;

			item.Manager1Approval = Department.Manager1;
			item.Manager2Approval = Department.Manager2;


			var validate = INV_PermitCalculator.Validate(item, user, IKYonetici);

			if (validate.result == false)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning(validate.message)
				}, JsonRequestBehavior.AllowGet);
			}

			var rsFile = new FileUploadSave(Request).SaveAs();
			var dbresult = db.InsertINV_Permit(item);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = feedback.Success("Geçmişe izin ekleme işlemi gerçekleşti.", false, Request.UrlReferrer.AbsolutePath.IndexOf("PastYearlyPermits") > -1 ? Url.Action("Index", "VWINV_Permit", new { area = "INV" }) : null)
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Tüm günleri gözetmeksizin izin ekleme", SHRoles.IKYonetici)]
		public ActionResult EveryDayPermitInsert()
		{
			var db = new WorkOfTimeDatabase();
			var now = DateTime.Now.AddDays(-1);
			var start = INV_PermitCalculator.CommencementDate(new DateTime(now.Year, now.Month, now.Day, 0, 0, 0));
			var endTime = TenantConfig.Tenant.Config.WorkingTimes[start.DayOfWeek].allowTimes.OrderBy(a => a.End).Select(a => a.End).LastOrDefault();
			var end = new DateTime(start.Year, start.Month, start.Day, endTime.Hours, endTime.Minutes, 0);

			var model = new VWINV_PermitForm
			{
				PermitCode = BusinessExtensions.B_GetIdCode(),
				StartDate = start,
				EndDate = end
			};
			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Tüm günleri gözetmeksizin izin ekleme", SHRoles.IKYonetici)]
		public JsonResult EveryDayPermitInsert(INV_Permit item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var user = db.GetVWSH_UserById(item.IdUser.Value);
			var feedback = new FeedBack();
			var calc = INV_PermitCalculator.CalculateAll(item);
			var IKYonetici = db.GetSH_UserByRoleId(SHRoles.IKYonetici).OrderBy(a => a.CompanyId == user.CompanyId).FirstOrDefault();

			var Department = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(item.IdUser.Value, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();

			item.created = DateTime.Now;
			item.createdby = userStatus.user.id;
			item.ApproveStatus = (Int32)EnumINV_PermitApproveStatus.IkKontrol;
			item.ArriveAdress = user.locationId_Title;
			item.AccessPhone = user.cellphone;

			item.TotalDays = calc.TotalDay;
			item.TotalHours = calc.TotalHour;
			item.Manager1ApprovalDate = item.created;
			item.Manager2ApprovalDate = item.created;
			item.IkApprovalDate = item.created;
			item.IkApproval = IKYonetici.id;

			item.Manager1Approval = Department.Manager1;
			item.Manager2Approval = Department.Manager2;


			var validate = INV_PermitCalculator.Validate(item, user, IKYonetici);

			if (validate.result == false)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning(validate.message)
				}, JsonRequestBehavior.AllowGet);
			}

			var rsFile = new FileUploadSave(Request).SaveAs();
			var dbresult = db.InsertINV_Permit(item);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("İzin ekleme işlemi gerçekleşti.", false, Request.UrlReferrer.AbsolutePath.IndexOf("EveryDayPermitInsert") > -1 ? Url.Action("Index", "VWINV_Permit", new { area = "INV" }) : null) : feedback.Warning(dbresult.message)
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Resmi İzinler", SHRoles.Personel)]
		public ContentResult GetOfficalPermit()
		{
			return Content(Infoline.Helper.Json.Serialize(INV_PermitCalculator.officalPermit), "application/json");
		}

		[PageInfo("İzin Detay Bilgileri", SHRoles.Personel)]
		public ContentResult GetPermitDetails(INV_Permit permit)
		{
			return Content(Infoline.Helper.Json.Serialize(INV_PermitCalculator.Calculate(permit)), "application/Json");
		}

		[PageInfo("Tüm İzin Detay Bilgileri", SHRoles.Personel)]
		public ContentResult GetAllPermitDetails(INV_Permit permit)
		{
			return Content(Infoline.Helper.Json.Serialize(INV_PermitCalculator.CalculateAll(permit)), "application/Json");
		}

		[PageInfo("Yöneticiler", SHRoles.Personel)]
		public ContentResult GetManager(Guid IdUser)
		{
			var db = new WorkOfTimeDatabase();
			var compPersonDept = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(IdUser, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();

			return Content(Infoline.Helper.Json.Serialize(compPersonDept), "application/Json");
		}

		[PageInfo("İzin Raporları", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
		public ActionResult Dashboard()
		{
			var yil = DateTime.Now.Year;
			var ay = DateTime.Now.Month;
			var db = new WorkOfTimeDatabase();
			var pageReport = new VWINV_PermitDashboardPageReport();
			var permit = db.GetVWINV_Permit();
			var ayBasi = new DateTime(yil, ay, 1);
			var aySonu = new DateTime(yil, ay, DateTime.DaysInMonth(yil, ay));
			pageReport.BuYilTalepEdilen = permit.Count();
			pageReport.BuYilOnaylanan = permit.Where(x => x.StartDate.Value.Year == DateTime.Now.Year && (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.GecmisIzin || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrol || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.SaglikRaporu || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IslakImzaYuklendi)).Count();
			pageReport.BuYilReddedilen = permit.Where(x => x.StartDate.Value.Year == DateTime.Now.Year && (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Red || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Red || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.IkKontrolRed)).Count();
			pageReport.BuYilOnayBekleyenIzinler = permit.Where(x => x.StartDate.Value.Year == DateTime.Now.Year && (x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.TalepEdildi || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici1Onay || x.ApproveStatus == (Int32)EnumINV_PermitApproveStatus.Yonetici2Onay)).Count();
			return View(pageReport);
		}

		[PageInfo("İzinler Grafiği Methodu", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici)]
		public JsonResult DashboardResult(Guid? IdUser)
		{
			try
			{
				var db = new WorkOfTimeDatabase();
				var user = db.GetVWSH_UserMyPersonIsWorking();
				var permits = IdUser.HasValue
					? db.GetVWINV_PermitByIdUser(IdUser.Value).ToArray()
					: db.GetVWINV_PermitByIdUsers(user.Select(a => a.id).ToArray());

				return Json(new
				{
					PermitsByMonth = permits != null ? new INV_PermitModel().allPermits(permits) : new ChartINV_Permit[] { },
					PermitsByMonthThisYear = permits != null ? new INV_PermitModel().allPermitsCurrentlyYear(permits) : new ChartINV_Permit[] { },
					PersonPermits = new INV_PermitModel().allPermitsByDate(permits),
					PersonPermitFuture = new INV_PermitModel().allPermitsFutureByDate(permits),
					PermitsByPosition = new INV_PermitModel().PermitPosition(permits),
					PermitsFutureByPosition = new INV_PermitModel().PermitFuturePosition(permits),
					PermitByNewJobSearch = new INV_PermitModel().PermitPersonListNewJobSearch(permits),
					PermitByMarried = new INV_PermitModel().PermitPersonListMarried(permits),
					PermitByAllDayCount = new INV_PermitModel().allDayPermit(permits),
					PermitYearlyPersonUsable = new INV_PermitModel().allPersonYearlyPermit(user)
				}, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				return Json(new ResultStatusUI { Result = false, FeedBack = new FeedBack().Error(ex.Message, "Sonuç Yüklenemedi") }, JsonRequestBehavior.AllowGet);
			}
		}
	}
}