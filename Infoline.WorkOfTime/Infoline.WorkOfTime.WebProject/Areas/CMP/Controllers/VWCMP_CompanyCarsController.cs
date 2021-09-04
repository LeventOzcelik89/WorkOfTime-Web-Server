using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CMP.Controllers
{
	public class VWCMP_CompanyCarsController : Controller
	{
		[PageInfo("Araç Listesi", SHRoles.Personel)]
		public ActionResult Index()
		{
		    return View();
		}

		[PageInfo("Araç Veri Methodu", SHRoles.Personel)]
		public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCMP_CompanyCars(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetVWCMP_CompanyCarsCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Araç Dropdown Veri Methodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCMP_CompanyCars(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Araç Detayı",  SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
		public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCMP_CompanyCarsById(id);
		    return View(data);
		}

		[PageInfo("Araç Tanımla", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
		public ActionResult Insert()
		{
		    var data = new VWCMP_CompanyCars { id = Guid.NewGuid() };
		    return View(data);
		}

		[PageInfo("Araç Tanımla", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi,SHRoles.Personel)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(CMP_CompanyCars item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

			if(!item.companyId.HasValue)
            {
				item.companyId = userStatus.user.CompanyId.HasValue ? userStatus.user.CompanyId : db.GetCMP_CompanyByOwner().FirstOrDefault()?.id;
            }

			var control = db.GetCMP_CompanyCarsByPlate(item.plate);
			if (control != null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Aynı plaka kaydı zaten sistemde kayıtlıdır. Plakanızı kontrol ediniz.")
				}, JsonRequestBehavior.AllowGet);
			}

			item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertCMP_CompanyCars(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Araç ekleme işlemi başarılı") : feedback.Warning("Araç ekleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Araç Düzenle", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
		public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetVWCMP_CompanyCarsById(id);
		    return View(data);
		}

		[PageInfo("Araç Düzenle", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(CMP_CompanyCars item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();

			var _control = db.GetCMP_CompanyCarsPlateByUpdate(item.id, item.plate);
			if (_control != null)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning("Aynı plaka kaydı zaten sistemde kayıtlıdır. Plakanızı kontrol ediniz.")
				}, JsonRequestBehavior.AllowGet);
			}

			item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateCMP_CompanyCars(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Araç güncelleme işlemi başarılı") : feedback.Warning("Araç güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Araç Sil", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
		[HttpPost]
		public JsonResult Delete(string[] id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();
		
		    var item = id.Select(a => new CMP_CompanyCars { id = new Guid(a) });
		
		    var dbresult = db.BulkDeleteCMP_CompanyCars(item);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Araç silme işlemi başarılı") : feedback.Warning("Araç silme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
