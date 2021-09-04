using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.HR.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.HR.Controllers
{
    public class VWHR_StaffNeedsPersonController : Controller
    {
        [PageInfo("İ.K. Personel Taleplerim", SHRoles.PersonelTalebi)]
        public ActionResult Index()
        {
            return View();
        }


        [PageInfo("İ.K. Personel Taleplerim Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_StaffNeedsPerson(condition).RemoveGeographies().OrderByDescending(c => c.created).ToDataSourceResult(request);
            data.Total = db.GetVWHR_StaffNeedsPersonCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("İ.K. Personel Taleplerim Metodu (Adet)", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var total = db.GetVWHR_StaffNeedsPersonCount(condition.Filter);
            return total;
        }

        [PageInfo("İ.K. Personel Taleplerim Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_StaffNeedsPerson(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("İ.K. Personel Taleplerim Detayı", SHRoles.PersonelTalebi)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_StaffNeedsPersonById(id);
            return View(data);
        }

        [PageInfo("İ.K. Personel Taleplerim Ekleme")]
        public ActionResult Insert()
        {
            var data = new VWHR_StaffNeedsPerson { id = Guid.NewGuid() };
            return View(data);
        }

        [PageInfo("İ.K. Personel Taleplerim Ekleme")]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(HR_StaffNeedsPerson item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertHR_StaffNeedsPerson(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("İ.K. Personel Taleplerim Güncelleme", SHRoles.PersonelTalebi)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var model = new VMHR_StaffNeedsPersonAndHrPerson();
            var data = db.GetVWHR_StaffNeedsPersonById(id);
            model.B_EntityDataCopyForMaterial(data);
            if (data.HrPersonId.HasValue) { 
            model.HRperson = db.GetVWHR_PersonById(data.HrPersonId.Value);
            }
            ViewBag.Onay = (Int32)EnumHR_StaffNeedsPersonstatus.Onay;
            ViewBag.Red = (Int32)EnumHR_StaffNeedsPersonstatus.Red;

            if (data.HrStaffNeedsId.HasValue)
            {
                model.reqCheckPersons = db.GetHR_StaffNeedsRequesterByStaffNeedId(data.HrStaffNeedsId.Value).Select(x => x.RequestId).ToList();
            }
            else
            {
                model.reqCheckPersons = new List<Guid?>();
            }
            return View(model);
        }

        


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("İ.K. Personel Taleplerim Güncelleme", SHRoles.PersonelTalebi)]
        public JsonResult Update(HR_StaffNeedsPerson item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            string url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            var data = db.GetVWHR_StaffNeedsPersonById(item.id);
            var insankaynaklariAll = db.GetSH_UserByRoleId(SHRoles.IKYonetici);
            foreach (var insankaynaklari in insankaynaklariAll)
            {
      
              if (item.status == (Int32)EnumHR_StaffNeedsPersonstatus.Onay)
              {
                  var text = "<h3>Merhaba {0},</h3>";
                  text += "<p>{1} adlı personelin {2} kodlu talebi doğrultusunda yönlendirmiş olduğunuz cv onaylanmıştır.</p>";
                  text += "<p>Yönlendirmiş olduğunuz cv detayı için lütfen <a href='{3}/HR/VWHR_Person/Detail?id={4}'> Buraya tıklayınız! </a> ve mülakat ayarlayınız.</p>";
                  text += "<p> Bilgilerinize. </p>";
                  var mesaj = string.Format(text, insankaynaklari.firstname + " " + insankaynaklari.lastname, data.createdby_Title, data.NeedCode, url, data.HrPersonId);
                  new Email().Template("Template1", "demo.jpg", "Cv Onayı Hakkında", mesaj)
                   .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Cv Onayı Hakkında"), true);
              }
             
              if (item.status == (Int32)EnumHR_StaffNeedsPersonstatus.Red)
              {
                  var text = "<h3>Merhaba {0},</h3>";
                  text += "<p>{1} adlı personelin {2} kodlu talebi doğrultusunda yönlendirmiş olduğunuz cv reddedilmiştir.</p>";
                  text += "<p>Yönlendirmiş olduğunuz cv detayı için lütfen <a href='{3}/HR/VWHR_Person/Detail?id={4}'> Buraya tıklayınız! </a></p>";
                  text += "<p> Bilgilerinize. </p>";
                  var mesaj = string.Format(text, insankaynaklari.firstname + " " + insankaynaklari.lastname, data.createdby_Title, data.NeedCode, url, data.HrPersonId);
                  new Email().Template("Template1", "demo.jpg", "Cv Onayı Hakkında", mesaj)
                   .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Cv Onayı Hakkında"), true);
              }
            }

            var staffneed = db.GetHR_StaffNeedsById(item.HrStaffNeedsId.Value);
            var statuss = item.status == (Int32)EnumHR_StaffNeedsPersonstatus.Onay ? (Int32)EnumHR_StaffNeedsStatusStatus.TalepEdenOnayladi : (Int32)EnumHR_StaffNeedsStatusStatus.TalepEdenReddetti;
            var hrperson = db.GetHR_PersonById(item.HrPersonId.Value);
            var trans = db.BeginTransaction();
            var dbresult = db.UpdateHR_StaffNeedsPerson(item,true,trans);
            dbresult &= db.UpdateHR_StaffNeeds(staffneed,true,trans);
            dbresult &= db.InsertHR_StaffNeedsStatus(new HR_StaffNeedsStatus {
              staffNeedId = staffneed.id,
              status = (short)statuss,
              description = statuss == (Int32)EnumHR_StaffNeedsStatusStatus.TalepEdenOnayladi ?
              userStatus.user.FullName +  " tarafından " + hrperson.Name + " " + hrperson.SurName + " adlı kişinin cvsi onaylanmıştır.<p> Açıklama : " + (!string.IsNullOrEmpty(item.Description) ? item.Description : "-") + " </ p ><p>Onaylanan cv detayı için <a target='_blank' href='/HR/VWHR_Person/Detail?id=" + hrperson.id + "'> Buraya tıklayınız! </a></p>" : 
              userStatus.user.FullName + " tarafından " + hrperson.Name + " " + hrperson.SurName + " adlı kişinin cvsi reddedilmiştir.<p> Açıklama : " + (!string.IsNullOrEmpty(item.Description) ? item.Description : "-") + " </ p ><p>Reddedilen cv detayı için <a target='_blank' href='/HR/VWHR_Person/Detail?id=" + hrperson.id + "'> Buraya tıklayınız! </a></p>"
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
                FeedBack = dbresult.result ? feedback.Success("Yanıtlama işlemi başarı ile gerçekleşti") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("İ.K. Personel Taleplerim Silme",SHRoles.IKYonetici)]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new HR_StaffNeedsPerson { id = new Guid(a) });

            var dbresult = db.BulkDeleteHR_StaffNeedsPerson(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
