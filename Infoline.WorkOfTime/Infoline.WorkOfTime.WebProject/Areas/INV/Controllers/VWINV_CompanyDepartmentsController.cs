using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.INV.Controllers
{
	public class VWINV_CompanyDepartmentsController : Controller
	{
		[PageInfo("Organizasyon Şeması Yönetimi", SHRoles.IKYonetici)]
		public ActionResult Index()
		{
			var db = new WorkOfTimeDatabase();

			var userStatus = (PageSecurity)Session["userStatus"];
			var genelOrganization = db.GetINV_CompanyDepartmentByType(EnumINV_CompanyDepartmentsType.Organization);

			if (genelOrganization == null)
			{
				db.InsertINV_CompanyDepartments(new INV_CompanyDepartments
				{
					id = Guid.NewGuid(),
					createdby = userStatus.user.id,
					Name = "Yönetim Kurulu",
					PID = null,
					ProjectId = null,
					Type = (int)EnumINV_CompanyDepartmentsType.Organization
				});
			}

			var data = db.GetVWINV_CompanyDepartmentsPageReport().FirstOrDefault() ?? new VWINV_CompanyDepartmentsPageReport();
			return View(data);
		}

		[PageInfo("Organizasyon Şeması Methodu", SHRoles.IKYonetici, SHRoles.ProjeYonetici, SHRoles.ProjePersonel)]
		public JsonResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWINV_CompanyDepartments(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWINV_CompanyDepartmentsCount(condition.Filter);
			return Json(data, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("Tüm Organizasyon Şemaları", SHRoles.Personel)]
		public ActionResult GetOrganization(Guid id)
		{
			var db = new WorkOfTimeDatabase();

			var departments = db.GetVWINV_CompanyDepartmentsByRootId(id);
			var personels = db.GetVWINV_CompanyPersonDepartmentsInDepartmentId(departments.Select(a => a.id).ToArray());
			var model = OrganizationCreate(id, departments, personels);
			return Content(Infoline.Helper.Json.Serialize(model), "aplication/json");
		}


		[PageInfo("Organizasyon Şeması Veri Methodu", SHRoles.IKYonetici, SHRoles.ProjeYonetici, SHRoles.ProjePersonel,SHRoles.Personel)]
		public JsonResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWINV_CompanyDepartments(condition).RemoveGeographies();
			return Json(data, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Organizasyon Şeması Detay", SHRoles.IKYonetici, SHRoles.ProjeYonetici)]
		public ActionResult Detail(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWINV_CompanyDepartmentsById(id);
			var pidData = db.GetVWINV_CompanyDepartments().Where(x => x.PID == id).ToArray();
			var pageReport = new DepartmentsDetailPageReport();

			pageReport.ToplamBirimSayisi = pidData.Count();
			pageReport.YanBirimSayisi = 0;
			pageReport.SonEklenenBirim = pidData.OrderByDescending(x => x.created).FirstOrDefault()?.Name;
			ViewBag.PageReport = pageReport;

			return View(data);
		}


		[PageInfo("Organizasyon Şeması Ekleme", SHRoles.IKYonetici, SHRoles.ProjeYonetici)]
		public ActionResult Insert(Guid? PID)
		{
			var data = new VWINV_CompanyDepartments
			{
				id = Guid.NewGuid(),
				PID = PID
			};

			return View(data);
		}

		[PageInfo("Organizasyon Şeması Ekleme", SHRoles.IKYonetici, SHRoles.ProjeYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(INV_CompanyDepartments item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			item.created = DateTime.Now;
			item.createdby = userStatus.user.id;


			var parentDepartment = db.GetINV_CompanyDepartmentsById(item.PID.Value);

			if (parentDepartment != null)
			{
				item.ProjectId = parentDepartment.ProjectId;
				item.Type = parentDepartment.Type;
			}

			var dbresult = db.InsertINV_CompanyDepartments(item);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error(dbresult.message, "Kaydetme işlemi başarısız"),
				Object = item
			};

			new ManagersCalculator().Run();

			return Json(result, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("Organizasyon Şeması Güncelleme", SHRoles.IKYonetici, SHRoles.ProjeYonetici)]
		public ActionResult Update(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWINV_CompanyDepartmentsById(id);
			return View(data);
		}


		[PageInfo("Organizasyon Şeması Güncelleme", SHRoles.IKYonetici, SHRoles.ProjeYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(INV_CompanyDepartments item)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			item.changed = DateTime.Now;
			item.changedby = userStatus.user.id;

			var dbresult = db.UpdateINV_CompanyDepartments(item);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
			};
			new ManagersCalculator().Run();
			return Json(result, JsonRequestBehavior.AllowGet);
		}


		[HttpPost]
		[PageInfo("Organizasyon Şeması Silme", SHRoles.IKYonetici, SHRoles.ProjeYonetici)]
		public JsonResult Delete(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();
			var departman = db.GetINV_CompanyDepartmentsById(id);

			if (departman.PID == null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Silme işlemi başarısız, Ana departman silinemez.")
				}, JsonRequestBehavior.AllowGet);
			}


			var dbdepartments = db.GetVWINV_CompanyDepartments();
			var departments = new List<INV_CompanyDepartments>();
			GetDepartments(departments, dbdepartments, departman.id);

			if (departments.Count() > 1)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Departmanın alt departmanları mevcut bu işlemi gerçekleştiremezsiniz.")
				}, JsonRequestBehavior.AllowGet);
			}

			var personels = db.GetINV_CompanyPersonDepartments().Where(a => departments.Select(c => c.id).Contains(a.DepartmentId.HasValue ? a.DepartmentId.Value : Guid.NewGuid()) && (a.EndDate == null || a.EndDate > DateTime.Now)).ToArray();

			if (personels.Count() > 0)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Departmanda çalışan personeller bulunmaktadır bu işlemi gerçekleştiremezsiniz.")
				}, JsonRequestBehavior.AllowGet);
			}
			var documentScope = db.GetDOC_DocumentScopeByOrganizationId(departman.id);
			var dbresult = db.BulkDeleteDOC_DocumentScope(documentScope);
			dbresult &= db.DeleteINV_CompanyDepartments(departman);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error(dbresult.message, "Silme işlemi başarısız")
			};

			new ManagersCalculator().Run();

			return Json(result, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("Organizasyon Şeması Departmanları", SHRoles.Personel)]
		public void GetDepartments(List<INV_CompanyDepartments> addList, VWINV_CompanyDepartments[] list, Guid Rootid)
		{
			addList.Add(new INV_CompanyDepartments { id = Rootid });
			foreach (var item in list)
			{
				if (item.PID == Rootid)
				{
					GetDepartments(addList, list, item.id);
				}
			}
		}

		[PageInfo("Şirket Organizasyon", SHRoles.Personel)]
		public ActionResult Preview(Guid? id, Guid? IdProject)
		{
			var db = new WorkOfTimeDatabase();
			var model = new VWINV_CompanyDepartments();
			if (id == null && IdProject == null)
			{
				model = db.GetVWINV_CompanyDepartmentsByPidNullAndTypeOrganization();
			}
			if (id.HasValue)
			{
				model = db.GetVWINV_CompanyDepartmentsById(id.Value);
			}

			if (IdProject.HasValue)
			{
				model = db.GetVWINV_CompanyDepartmentsByIdProjectBase(IdProject.Value);
			}
			if (model == null)
			{
				model = db.GetVWINV_CompanyDepartmentsByPidNullAndTypeOrganization();
			}
			return View(model);
		}


		[PageInfo("Organizasyon Şeması Toplu Ekleme", SHRoles.IKYonetici, SHRoles.ProjeYonetici)]
		public ActionResult Upsert(Guid? id, Guid? IdProject)
		{
			var db = new WorkOfTimeDatabase();

			var model = new VWINV_CompanyDepartments();
			if (id.HasValue)
			{
				model = db.GetVWINV_CompanyDepartmentsById(id.Value);
			}

			if (IdProject.HasValue)
			{
				model = db.GetVWINV_CompanyDepartmentsByIdProjectBase(IdProject.Value);
			}
			return View(model);
		}


		[PageInfo("Organizasyon Şeması Yazdırma", SHRoles.IKYonetici, SHRoles.ProjeYonetici), AllowEveryone]
		public ActionResult UpsertPrint(Guid? id, Guid? IdProject)
		{
			var db = new WorkOfTimeDatabase();

			var model = new VWINV_CompanyDepartments();
			if (id.HasValue)
			{
				model = db.GetVWINV_CompanyDepartmentsById(id.Value);
			}

			if (IdProject.HasValue)
			{
				model = db.GetVWINV_CompanyDepartmentsByIdProjectBase(IdProject.Value);
			}
			return View(model);
		}



		[PageInfo("Organizasyon Şeması Departman Ekleme", SHRoles.IKYonetici, SHRoles.ProjeYonetici)]
		private Organization OrganizationCreate(Guid rootId, VWINV_CompanyDepartments[] departments, VWINV_CompanyPersonDepartments[] personels)
		{
			var root = departments.Where(a => a.id == rootId).FirstOrDefault();

			var persons = string.Empty;

			if (root.Type == (int)EnumINV_CompanyDepartmentsType.Matrix)
			{



				persons = personels.Any(a => a.DepartmentId == rootId && !string.IsNullOrEmpty(a.Person_Title)) ?

				string.Join("", personels.Where(a => a.DepartmentId == rootId && !string.IsNullOrEmpty(a.Person_Title))
				.Select(a => "<div class='person clearfix " + (a.Manager1 == null || a.Manager2 == null ? "nomanager" : "") + "' id='" + a.id + "'>" + a.Person_Title +
				(a.Title != null ? "<br/> (" + (a.Title.ToString()) + ")" : "") + ((a.EndDate == null || a.EndDate > DateTime.Now) ? "" : "<br/> Görev Süresi Doldu")
				 + "</div>").ToArray()) : "<div>Personel ataması yapılmamış.</div>";



			}
			else
			{
				persons = personels
					.Any(a => a.DepartmentId == rootId && !string.IsNullOrEmpty(a.Person_Title) && (a.EndDate == null || a.EndDate > DateTime.Now))
						? string.Join("",
							personels
							.Where(a => a.DepartmentId == rootId && !string.IsNullOrEmpty(a.Person_Title) && (a.EndDate == null || a.EndDate > DateTime.Now))
							.Select(a => "<div class='person clearfix " + (a.Manager1 == null || a.Manager2 == null ? "nomanager" : "") + "' id='" + a.id + "'>" + a.Person_Title + (a.Title != null ? "<br/> (" + (a.Title != null ? a.Title.ToString() : "") + ")" : "") + "</div>")
							.ToArray()
						)
						: "<div>Personel ataması yapılmamış.</div>";
			}

			var model = new Organization
			{
				id = root.id,
				department = root.Name,
				color = "#fdc44e",
				personels = persons,
				children = departments.Where(a => a.PID == rootId).Select(a => OrganizationCreate(a.id, departments, personels)).ToArray(),
			};
			return model;
		}
	}
}
