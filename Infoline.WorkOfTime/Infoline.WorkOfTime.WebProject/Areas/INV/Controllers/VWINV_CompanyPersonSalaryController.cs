using Infoline.Framework.Database;
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
    public class VWINV_CompanyPersonSalaryController : Controller
    {
        [PageInfo("Tüm Maaşlar", SHRoles.IKYonetici )]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonSalaryPageReportSummary() ?? new VWINV_CompanyPersonSalaryPageReport();
            return View(data);
        }


        [PageInfo("Personel Ücretleri Methodu", SHRoles.IKYonetici)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonSalary(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWINV_CompanyPersonSalaryCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Personel Ücretleri Detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonSalaryById(id);
            return View(data);
        }


        [PageInfo("Personel Ücreti Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(Guid? IdUser)
        {
            var data = new VWINV_CompanyPersonSalary
            {
                id = Guid.NewGuid(),
                IdUser = IdUser
            };
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Ücreti Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(INV_CompanyPersonSalary item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            if(!item.IdUser.HasValue || !item.StartDate.HasValue || !item.EndDate.HasValue)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Başlangıç bitiş tarih bilgisi bulunamadı.")
                },JsonRequestBehavior.AllowGet);
            }
            var control = db.GetINV_CompanyPersonSalaryBySalaryControl(item.IdUser.Value, item.StartDate, item.EndDate);
            if(control != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Belirtilen tarihler arasında maaş bilgisi mevcut.")
                }, JsonRequestBehavior.AllowGet);
            }
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var res = db.InsertINV_CompanyPersonSalary(item);

            var result = new ResultStatusUI
            {
                Result = res.result,
                FeedBack = res.result ? feedback.Success("Maaş kaydetme işlemi başarılı") : feedback.Warning("Maaş kaydetme işlemi başarısız oldu.")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Personel Ücreti Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonSalaryById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Ücreti Güncelleme", SHRoles.IKYonetici)]
        public JsonResult Update(INV_CompanyPersonSalary item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var control = db.GetINV_CompanyPersonSalaryBySalaryControl(item.IdUser.Value, item.StartDate, item.EndDate);
            if (control != null && control.id != item.id)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Belirtilen tarihler arasında maaş bilgisi mevcut.")
                }, JsonRequestBehavior.AllowGet);
            }


           
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            var res = db.UpdateINV_CompanyPersonSalary(item);


            var result = new ResultStatusUI
            {
                Result = res.result,
                FeedBack = res.result ? feedback.Success("Maaş güncelleme işlemi başarılı") : feedback.Warning("Maaş güncelleme işlemi başarısız oldu.")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [PageInfo("Personel Ücreti Silme", SHRoles.IKYonetici)]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new INV_CompanyPersonSalary { id = new Guid(a) });

            var dbresult = db.BulkDeleteINV_CompanyPersonSalary(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
