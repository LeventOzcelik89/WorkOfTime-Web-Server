using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CMP
{
    public class CMP_TypesController : Controller
	{
        [PageInfo("Firma&Cari Tip Tanımları", SHRoles.IKYonetici,SHRoles.DepoSorumlusu,SHRoles.SahaGorevYonetici)]
        public ActionResult Index()
		{
			return View();
		}

        [PageInfo("Firma&Cari Tipleri Methodu", SHRoles.IKYonetici, SHRoles.DepoSorumlusu, SHRoles.SahaGorevYonetici)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var page = request.Page;
		    request.Filters = new FilterDescriptor[0];
		    request.Sorts = new SortDescriptor[0];
		    request.Page = 1;
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetCMP_Types(condition).RemoveGeographies().ToDataSourceResult(request);
		    data.Total = db.GetCMP_TypesCount(condition.Filter);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Firma&Cari Tipleri Veri Methodu", SHRoles.Personel, SHRoles.CRMBayiPersoneli,SHRoles.HakEdisBayiPersoneli)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
		{
		    var condition = KendoToExpression.Convert(request);

		    var db = new WorkOfTimeDatabase();
		    var data = db.GetCMP_Types(condition);
		    return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

        [PageInfo("Firma&Cari Tipleri Detayı", SHRoles.Personel)]
        public ActionResult Detail(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
			var data = db.GetCMP_TypesById(id);
			return View(data);
		}

        [PageInfo("Firma&Cari Tipleri Ekleme", SHRoles.Personel)]
        public ActionResult Insert()
		{
		    var data = new CMP_Types { id = Guid.NewGuid() };
		    return View(data);
		}

        [PageInfo("Firma&Cari Tipleri Ekleme Methodu", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(CMP_Types item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		    item.created = DateTime.Now;
		    item.createdby = userStatus.user.id;
		    var dbresult = db.InsertCMP_Types(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

   

        [PageInfo("Firma&Cari Tipleri Güncelleme", SHRoles.Personel)]
        public ActionResult Update(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var data = db.GetCMP_TypesById(id);
		    return View(data);
		}

        [PageInfo("Firma&Cari Tipleri Güncelleme Methodu", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(CMP_Types item)
		{
		    var db = new WorkOfTimeDatabase();
		    var userStatus = (PageSecurity)Session["userStatus"];
		    var feedback = new FeedBack();
		
		    item.changed = DateTime.Now;
		    item.changedby = userStatus.user.id;
		
		    var dbresult = db.UpdateCMP_Types(item);
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}

        [PageInfo("Firma&Cari Tipleri Silme", SHRoles.Personel)]
        [HttpPost]
		public JsonResult Delete(Guid id)
		{
		    var db = new WorkOfTimeDatabase();
		    var feedback = new FeedBack();

			var isItUsed = db.GetCMP_CompanyTypeByTypeId(id);

			if (isItUsed != null)
			{
				return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Bu Cari Tipi Kullanılmaktadır. Kullanılan Cari Tipi Silinemez.") }, JsonRequestBehavior.AllowGet);
			}

			var dbresult = db.DeleteCMP_Types(id);
		
		    var result = new ResultStatusUI
		    {
		        Result = dbresult.result,
		        FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
		    };
		
		    return Json(result, JsonRequestBehavior.AllowGet);
		}
	}
}
