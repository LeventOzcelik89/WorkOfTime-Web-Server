using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.HDM.Controllers
{
    public class VWHDM_TicketController : Controller
    {
        [PageInfo("Yardım Talepleri", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaYonetim)]
        public ActionResult Index()
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            var issueIds = db.GetHDM_IssueUserByUserId(userStatus.user.id).Select(a => a.issueId).ToList();
            var issues = db.GetHDM_Issue();

            var issueIdList = new List<Guid?>();
            issueIdList.AddRange(issueIds);

            foreach (var issueId in issueIds)
            {
                issueIdList.AddRange(issues.Where(a => a.mainId == issueId).Select(a => (Guid?)a.id));
            }

            var personelRol = new Guid(SHRoles.YardimMasaPersonel);
            var personelUserIds = db.GetSH_UserByRoleId(personelRol);
            var users = db.GetSH_UserByIds(personelUserIds);

            var model = new VMHDM_TicketIndexModel
            {
                IssueIds = issueIdList,
                PersonelUsers = users
            };

            return View(model);
        }

        [PageInfo("Yardım Taleplerim", SHRoles.YardimMasaTalep, SHRoles.YardimMasaYonetim)]
        public ActionResult IndexMy()
        {
            return View();
        }

        [PageInfo("Yardım Talepleri Metodu", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaTalep, SHRoles.YardimMasaYonetim)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHDM_Ticket(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWHDM_TicketCount(condition.Filter);
            return Content(Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Yardım Talepleri DropDown Metodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHDM_Ticket(condition);
            return Content(Helper.Json.Serialize(data), "application/json");
        }

        [AllowEveryone]
        [PageInfo("Yardım Talebi Adet Methodu")]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWHDM_TicketCount(condition.Filter);
            return count;
        }

        [PageInfo("Yardım Talebi Detayı", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaTalep, SHRoles.YardimMasaYonetim, SHRoles.SahaGorevMusteri)]
        public ActionResult Detail(Guid id)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var data = new VMHDM_TicketModel { id = id }.Load();

            if(data.IssueManagers == null)
            {
                return RedirectToAction("Index");
            }
            if (userStatus.user.id != data.requesterId && userStatus.user.id != data.assignUserId && data.IssueManagers.Where(a => a.id == userStatus.user.id).FirstOrDefault() != null && userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SistemYonetici)))
            {
                return RedirectToAction("Index");
            }

            return View(data);
        }

        [PageInfo("Talep Oluştur", SHRoles.YardimMasaTalep, SHRoles.YardimMasaYonetim)]
        public ActionResult InsertBasic(VMHDM_TicketModel item)
        {
            item.Load();
            item.channel = (int)EnumHDM_TicketChannel.WebSite;

            var userStatus = (PageSecurity)Session["userStatus"];
            item.requesterId = userStatus.user.id;
            Log.Warning(item.requesterId.ToString());
            item.Requester = new VWHDM_TicketRequester
            {
                id = item.requesterId.Value,
                email = userStatus.user.email,
                fullName = userStatus.user.FullName,
                phone = userStatus.user.cellphone,
                company = userStatus.user.Company_Title
            };

            return View(item);
        }

        [PageInfo("Yardım Talebi Oluşturma Sayfası", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaTalep, SHRoles.YardimMasaYonetim)]
        public ActionResult Insert(VMHDM_TicketModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            item.Load();

            item.requesterId = userStatus.user.id;
            item.Requester = new VWHDM_TicketRequester
            {
                id = item.requesterId.Value,
                email = userStatus.user.email,
                fullName = userStatus.user.FullName,
                phone = userStatus.user.cellphone,
                company = userStatus.user.Company_Title
            };


            return View(item);
        }

        [PageInfo("Yardım Talebi Ekleme Metodu", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaTalep, SHRoles.YardimMasaYonetim)]
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public JsonResult Insert(VMHDM_TicketModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, (int)EnumHDM_TicketActionType.YeniTalep, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Talebiniz başarı ile alınmıştır.En kısa sürede tarafınıza geri dönüş sağlanacaktır.", false, (Request.UrlReferrer.AbsolutePath == "/HDM/VWHDM_Issue/Help" ||
                                                                                                  Request.UrlReferrer.AbsolutePath == "/HDM/VWHDM_Ticket/InsertBasic" ?
                                                               Url.Action("Detail", "VWHDM_Ticket", new { area = "HDM", id = item.id }) : null)) :
                                             feedback.Warning("Kaydetme işlemi başarısız" + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Yardım Talebi Düzenleme Sayfası", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaYonetim)]
        public ActionResult Update(Guid id)
        {
            var data = new VMHDM_TicketModel { id = id }.Load();
            return View(data);
        }

        [PageInfo("Yardım Talebi Düzenleme Metodu", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaYonetim)]
        [HttpPost, ValidateAntiForgeryToken, ValidateInput(false)]
        public JsonResult Update(VMHDM_TicketModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, (int)EnumHDM_TicketActionType.TalepDuzenleme, Request);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız" + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Yardım Talebi Değerlendirme Sayfası", SHRoles.YardimMasaTalep, SHRoles.YardimMasaPersonel, SHRoles.YardimMasaYonetim)]
        [AllowEveryone]
        public ActionResult Evaluate(Guid id)
        {
            var data = new VMHDM_TicketModel { id = id }.Load();
            return View(data);
        }

        [AllowEveryone]
        [PageInfo("Yardım Talebi Özel Düzenleme Metodu", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaTalep, SHRoles.YardimMasaYonetim)]
        [HttpPost, ValidateInput(false)]
        public JsonResult UpdateCustom(VMHDM_TicketModel item, int actionType)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id, actionType);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız" + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Yardım Talebi Silme Metodu", SHRoles.YardimMasaPersonel, SHRoles.YardimMasaTalep, SHRoles.YardimMasaYonetim)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var data = new VMHDM_TicketModel { id = id }.Load();
            var dbresult = data.Delete(userStatus.AuthorizedRoles);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarısız. " + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
