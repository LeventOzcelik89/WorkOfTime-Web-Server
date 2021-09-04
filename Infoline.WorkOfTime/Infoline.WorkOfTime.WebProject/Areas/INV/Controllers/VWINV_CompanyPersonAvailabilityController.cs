using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.PRJ.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.INV.Controllers
{
	public class VWINV_CompanyPersonAvailabilityController : Controller
    {

        [PageInfo("Personel Müsaitlikleri", SHRoles.ProjeYonetici)]
        public ActionResult Index()
        {

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonAvailabilitySummary() ?? new VWINV_CompanyPersonAvailabilityPageReport();
            return View(data);
        }


        [PageInfo("Personel Müsaitlikleri Methodu", SHRoles.ProjeYonetici,SHRoles.IKYonetici)]
        JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonAvailability(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWINV_CompanyPersonAvailabilityCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Personel Müsaitlik Detayı", SHRoles.ProjeYonetici)]
        ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonAvailabilityById(id);
            return View(data);
        }


        [PageInfo("Personel Müsaitliği Ekleme", SHRoles.ProjeYonetici)]
        public ActionResult Insert()
        {
            var data = new VWINV_CompanyPersonAvailability { id = Guid.NewGuid() };
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Müsaitliği Ekleme",SHRoles.ProjeYonetici)]
        public JsonResult Insert(INV_CompanyPersonAvailability item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertINV_CompanyPersonAvailability(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Personel Müsaitliği Güncelleme",SHRoles.ProjeYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonAvailabilityById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Müsaitliği Güncelleme",SHRoles.ProjeYonetici)]
        public JsonResult Update(INV_CompanyPersonAvailability item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateINV_CompanyPersonAvailability(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Personel Müsaitliği Silme",SHRoles.ProjeYonetici)]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new INV_CompanyPersonAvailability { id = new Guid(a) });

            var dbresult = db.BulkDeleteINV_CompanyPersonAvailability(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Personel Müsaitliği Önizleme", SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
        public ActionResult Preview(Guid IdProject, EnumINV_CompanyPersonAvailabilityType type)
        {
            var data = new INV_CompanyPersonAvailabilityModel(IdProject, type).GetSchema();
            return View(data);
        }


        [PageInfo("Personel Müsaitliği Toplu Ekleme", SHRoles.ProjeYonetici)]
        public ActionResult Upsert(Guid IdProject, EnumINV_CompanyPersonAvailabilityType type)
        {
            var data = new INV_CompanyPersonAvailabilityModel(IdProject, type).GetSchema();
            return View(data);
        }


        [HttpPost]
        [PageInfo("Personel Müsaitliği Toplu Ekleme", SHRoles.ProjeYonetici)]
        public ContentResult Upsert(INV_CompanyPersonAvailability[] pa, Guid IdProject)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = new List<ResultStatus>();
            var result = new ResultStatusUI();
            var trans = db.BeginTransaction();

            if (pa.Count() > 0)
            {
                var gecmis = db.GETINV_CompanyPersonAvailabilityByProjectId(IdProject, pa.Select(a => a.type.Value).FirstOrDefault()).ToArray();
                if (gecmis.Count() > 0)
                {
                    dbresult.Add(db.BulkDeleteINV_CompanyPersonAvailability(gecmis, trans));
                }
            }

            foreach (var item in pa)
            {
                item.created = DateTime.Now;
                item.createdby = userStatus.user.id;
                item.IdProject = IdProject;
            }

            dbresult.Add(db.BulkInsertINV_CompanyPersonAvailability(pa, trans));


            var count = dbresult.Count(a => a.result == false);


            if (count == 0)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }

            return Content(Infoline.Helper.Json.Serialize(new ResultStatusUI
            {
                Result = count == 0,
                FeedBack = count == 0 ? feedback.Success("Personel Maliyet Şeması Başarı ile Kaydedildi") : feedback.Error("Personel Maliyet Şeması Kaydedilirken bir sonuç oluştu")
            }), "application/json");
        }

        [PageInfo("Personel Proje Müsaitlik Hesaplama", SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
        public ContentResult Calculate(VWINV_CompanyPersonAvailability[] pa, Guid IdProject)
        {
            var data = new ProjectPersonModel(pa).Calculate().Select(a => new
            {
                IdUser = a.IdUser,
                StartDate = a.StartDate.Value.ToString("MM/yyyy"),
                Salary = a.Salary
            });
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Personel Doluluk Oranları", SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
        public ActionResult Occupancy()
        {
            var db = new WorkOfTimeDatabase();
            return View();
        }

        [HttpPost]
        [PageInfo("Personel Doluluk Oranları", SHRoles.ProjeYonetici, SHRoles.IKYonetici)]
        public ContentResult Occupancy(string IdUsers, string IdProject, string Start, string End)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonAvailabilityFilter(IdUsers.Split(',').Select(x => new Guid(x)).ToArray(), IdProject.Split(',').Select(x => new Guid(x)).ToArray(), Convert.ToDateTime(Start), Convert.ToDateTime(End));
            return Content(Infoline.Helper.Json.Serialize(new { Data = data }), "aplication/json");
        }
    }
}
