using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.INV.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.INV.Controllers
{
	public class INV_CompanyPersonController : Controller
    {

        [PageInfo("İşletme Personelleri Methodu", SHRoles.IKYonetici)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPerson(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWINV_CompanyPersonCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("İşletme Personelleri Veri Methodu", SHRoles.IKYonetici)]
        public JsonResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            var result = db.GetVWINV_CompanyPerson(condition);
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("İşletme Personeli Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(VWINV_CompanyPerson data)
        {
            var db = new WorkOfTimeDatabase();

            var mevcut = db.GetVWSH_UserById(data.IdUser.Value);
            data = db.GetVWINV_CompanyPersonById(mevcut.CompanyPersonId.Value);
            data.id = Guid.NewGuid();

            if (data.IdUser.HasValue)
            {

                if (mevcut.IsWorking == true)
                {
                    var text = "<p>Personel (" + mevcut.FullName + ") <strong>" + mevcut.Company_Title + "</strong> işletmesinde zaten çalışmaktadır.</p>";
                    text += mevcut.PermitYearlyUsable > 0 ? "<p>Personelin <strong>" + mevcut.PermitYearlyUsable + "</strong> gün yıllık izin hakkı bulunmaktadır ve bu işlem sonrası hakları korunacaktır.</p>" : "";
                    ViewBag.Warning = text;
                }
                else
                {
                    ViewBag.Warning = "İş çıkışı yapılmış bir personeli tekrar iş'e almak istediğinizden eminmisiniz.";
                }
            }
            return View(data);
        }

        [PageInfo("İşletme Personeli Ekleme", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(INV_CompanyPerson item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var user = db.GetVWSH_UserById(item.IdUser.Value);
            var updateRow = db.GetINV_CompanyPersonById(user.CompanyPersonId.Value);
            updateRow.JobEndDate = item.JobStartDate;
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var dbresult = db.InsertINV_CompanyPerson(item);
            dbresult &= db.UpdateINV_CompanyPerson(updateRow);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Personel İşletme bilgisi ekleme işlemi başarılı.") : feedback.Error("Personel İşletme bilgisi ekleme işlemi başarısız.")
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("İşletme Personeli Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(INV_CompanyPerson item)
        {
            var db = new WorkOfTimeDatabase();
            var current = db.GetVWINV_CompanyPersonById(item.id);
            var all = db.GetVWINV_CompanyPersonByIdUser(current.IdUser.Value).OrderByDescending(a => a.JobEndDate);
            ViewBag.IsFirst = all.Select(a => a.id).FirstOrDefault() == current.id;
            return View(current);
        }

        [PageInfo("İşletme Personeli Güncelleme", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(INV_CompanyPerson item, bool? Ispost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;


            var dbresult = db.UpdateINV_CompanyPerson(item, false);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Personel İşletme bilgisii güncelleme işlemi başarılı.") : feedback.Error("Personel İşletme bilgisi güncelleme işlemi başarısız.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("İşletme Personel Grafikleri", SHRoles.IKYonetici)]
        public ActionResult Dashboard()
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonPageReportSummary() ?? new VWINV_CompanyPersonPageReport();
            return View(data);
        }

        [PageInfo("Personel Raporları", SHRoles.IKYonetici)]
        public JsonResult DashboardResult()
        {
            try
            {
                var DashboardModel = new INV_CompanyPersonDashboardModel();
                return Json(DashboardModel.GetResults(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResultStatusUI { Result = false, FeedBack = new FeedBack().Error(ex.Message, "Sonuç Yüklenemedi") }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

