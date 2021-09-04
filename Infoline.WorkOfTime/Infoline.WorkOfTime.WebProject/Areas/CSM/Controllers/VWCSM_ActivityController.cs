using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.Dto;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.CSM.Controllers
{
	public class VWCSM_ActivityController : Controller
    {
        [PageInfo("Öğrenci Görüşmeleri", SHRoles.AdayOgrenciYonetim)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Aktiviteler Call Center", SHRoles.AdayOgrenciAgent)]
        public ActionResult IndexMy(Guid? studentId, bool? isAddContact, string DialId, string AgentName, string telno)
        {
            var model = new VMCSM_StudentModel { id = studentId.HasValue ? studentId.Value : Guid.NewGuid(), isAddContact = isAddContact, phone = telno }.Load();
            return View(model);
        }


        [PageInfo("Aktiviteler Metodu", SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_Activity(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCSM_ActivityCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Aktiviteler Miktar DataSource", SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWCSM_ActivityCount(condition.Filter);
            return count;
        }


        [PageInfo("Aktiviteler Dropdown Methodu", SHRoles.Personel, SHRoles.AdayOgrenciYonetim)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_Activity(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Aktivite Detayı", SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_ActivityById(id);
            return View(data);
        }


        [PageInfo("Aktivite Ekleme Sayfası", SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        public ActionResult Insert(VWCSM_Activity item)
        {
            return View(item);
        }


        [PageInfo("Aktivite Ekleme Metodu", SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(CSM_Activity item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            if (item.studentId == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Öğrenci bulunamadı.")
                }, JsonRequestBehavior.AllowGet);
            }

            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var trans = db.BeginTransaction();
            var dbresult = db.InsertCSM_Activity(item, trans);

            if (dbresult.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kaydetme işlemi başarısız oldu.")
                }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                new FileUploadSave(Request).SaveAs();
                var url = HttpUtility.ParseQueryString(Request.UrlReferrer.OriginalString);

                if (!string.IsNullOrEmpty(url.Get("DialId")) && !string.IsNullOrEmpty(url.Get("telno")))
                {
                    var contactStatus = db.GetCSM_StageById(item.stageId.Value);
                    if (contactStatus != null)
                    {
                        var resultCode = Convert.ToInt32(contactStatus.code);
                        TenantConfig.Tenant.Config.CallCenterService.SetCampaignCall(new CampaignCallDto
                        {
                            AgentName = string.IsNullOrEmpty(url.Get("AgentName")) ? "sysAgent" : url.Get("AgentName"),
                            DialId = url.Get("DialId"),
                            CallbackTime = item.contactDate ?? DateTime.Now,
                            CallbackPhone = url.Get("telno"),
                            FinishCode = contactStatus.name,
                            ReasonCode = resultCode == 3 ? EnumReasonCode.Randevu : (resultCode == 100 ? EnumReasonCode.Hatali_Numara : (resultCode == 2 ? EnumReasonCode.Ulasilamadi : EnumReasonCode.Ulasildi))
                        });
                    }
                }
                trans.Commit();
            }

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Aktivite Düzenleme Sayfası", SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_ActivityById(id);
            return View(data);
        }


        [PageInfo("Aktivite DÜzenleme Metodu", SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(CSM_Activity item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateCSM_Activity(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Aktivite Silme Metodu", SHRoles.AdayOgrenciYonetim)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var activity = db.GetCSM_ActivityById(id);
            var dbresult = db.DeleteCSM_Activity(activity);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Aktivite Aşama Güncelle", SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        public ActionResult UpdateStage(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_ActivityById(id);
            return View(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Aktivite Güncelleme", SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        public JsonResult UpdateStage(CSM_Activity item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            item.changedby = userStatus.user.id;

            if (!item.stageId.HasValue)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Aşama seçmeden işleme devam edilemez.")
                }, JsonRequestBehavior.AllowGet);
            }

            var activity = db.GetCSM_ActivityById(item.id);
            if (activity.stageId != item.stageId)
            {
                activity.changedby = userStatus.user.id;
                activity.stageId = item.stageId;
                var trans = db.BeginTransaction();

                var dbresult = db.UpdateCSM_Activity(item, false, trans);
                if (dbresult.result == false)
                {
                    trans.Rollback();
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = new FeedBack().Error(dbresult.message)
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    trans.Commit();
                }
            }

            return Json(new ResultStatusUI
            {
                Result = true,
                Object = activity,
                FeedBack = new FeedBack().Success("Aktivite Aşaması Güncellendi.")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
