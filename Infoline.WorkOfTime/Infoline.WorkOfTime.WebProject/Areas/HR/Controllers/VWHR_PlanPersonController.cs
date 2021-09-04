using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.HR.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.HR.Controllers
{
	public class VWHR_PlanPersonController : Controller
    {
        [PageInfo("Personellerin Planı Mülakatları", SHRoles.IKYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Mülakatlar   ", SHRoles.IsGorusmesiMulakat,SHRoles.IKYonetici)]
        public ActionResult MyIndex()
        {
            return View();
        }

        [PageInfo("Personellerin Planlanan Mülakatları Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_PlanPerson(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWHR_PlanPersonCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Planlanan Mülakatlar Adet Methodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var total = db.GetVWHR_PlanPersonCount(condition.Filter);
            return total;
        }

        [PageInfo("Planlanan Mülakatlar Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_PlanPerson(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Planlanan Mülakatlar Detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_PlanPersonById(id);
            return View(data);
        }
        [PageInfo("Planlanan Mülakatlar Ekleme", SHRoles.IKYonetici,SHRoles.IsGorusmesiMulakat)]
        public ActionResult Insert(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var hrPlan = db.GetVWHR_PlanById(id);
            var model = new VMHR_PlanAndPerson
            {
                id = Guid.NewGuid(),
                HrPersonId = hrPlan.HrPersonId,
                HrPlanId = id
            };

            if (hrPlan.PdsEvulateFormId.HasValue)
            {
                var Quest = db.GetVWPDS_QuestionFormByFormId(hrPlan.PdsEvulateFormId.Value);
                model.questions = Quest;
            }
            model.HrPerson = db.GetHR_PersonById(model.HrPersonId.Value);
            model.hrPlan = hrPlan;
            var personPositions = db.GetHR_PersonPositionByHrPersonId(model.HrPersonId.Value);
            var personKeywords = db.GetHR_PersonKeywordsByHrPersonId(model.HrPersonId.Value);
            model.personPosition = personPositions.Select(x => x.HrPositionId).ToArray();
            model.personKeywords = personKeywords.Select(x => x.HrKeywordsId).ToArray();
            return View(model);
        }
        [PageInfo("Planlanan Mülakatlar Çoklu Ekleme", SHRoles.IKYonetici,  SHRoles.IsGorusmesiMulakat)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMHR_PlanAndPerson item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var resultstatus = new ResultStatus();
            resultstatus.result = true;

            var data = db.GetHR_PlanById(item.id);
            var trans = db.BeginTransaction();
            if (data.PdsEvulateFormId.HasValue)
            {
                item.item = new PDS_FormResult
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    evaluateFormId = data.PdsEvulateFormId.Value,
                    evaluatorId = userStatus.user.id,
                    evaluatedUserId = item.HrPersonId,
                };

                double sumFactor = 0;

                foreach (var quest in item.questions)
                {
                    sumFactor += quest.factor.HasValue ? quest.factor.Value : 0;
                }

                foreach (var quesRes in item.questionResult)
                {
                    var datas = item.questions.Where(c => c.id == quesRes.questionFormId).FirstOrDefault();
                    quesRes.point = ((datas.factor * quesRes.score) / 5) * (100 / sumFactor);
                    quesRes.formResultId = item.item.id;
                }

                data.PdsEvulateFormId = item.item.evaluateFormId;
                data.PdsEvulateFormResultId = item.item.id;

                resultstatus &= db.InsertPDS_FormResult(item.item, trans);
                resultstatus &= db.BulkInsertPDS_QuestionResult(item.questionResult, trans);
            }
            

            item.HrPerson = db.GetHR_PersonById(item.HrPerson.id);
            resultstatus &= db.UpdateHR_Person(item.HrPerson, false, trans);

            //if (data.StaffNeedsId.HasValue)
            //{
            //    resultstatus &= db.InsertHR_StaffNeedsStatus(new HR_StaffNeedsStatus { 
            //      staffNeedId = data.StaffNeedsId.Value,
            //   //   status = (Int32)EnumHR_StaffNeedsStatusStatus.MulakatAyarlandi
            //    },trans);
            //}

            data.changedby = userStatus.user.id;
            data.changed = DateTime.Now;
            data.Result = item.Result;
            data.Description = item.Description != null ? item.Description : Request["Description"] != null ? Request["Description"].Replace(",", "") : null;
            data.PlanDate = DateTime.Now;

            resultstatus &= db.UpdateHR_Plan(data, false, trans);

            string url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            var cvSahibi = db.GetHR_PersonById(item.HrPersonId.Value);
            var insankaynaklariAll = db.GetSH_UserByRoleId(SHRoles.IKYonetici);

            foreach (var insankaynaklari in insankaynaklariAll)
            {
                switch ((EnumHR_PlanResult)item.Result)
                {
                    case EnumHR_PlanResult.Olumlu:
                        var text = "<h3>Merhaba {0},</h3>";
                        text += "<p>{1} adlı kişi ile {2} tarihindeki mülakata {3} adlı personel olumlu görüş bildirmiştir.</p>";
                        text += "<p>{1} adlı kişinin detayı için lütfen <a href='{4}/HR/VWHR_Person/Detail?id={5}'> Buraya tıklayınız! </a></p>";
                        text += "<p> Bilgilerinize. </p>";
                        var mesaj = string.Format(text, insankaynaklari.firstname + " " + insankaynaklari.lastname, cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", data.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, url, item.HrPersonId);
                        new Email().Template("Template1", "demo.jpg", "Mülakat Sonucu Hakkında", mesaj)
                         .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName +" | WORKOFTIME", "Mülakat Sonucu Hakkında.."), true);
                        break;
                    case EnumHR_PlanResult.Olumsuz:
                        //  var texts = "<h3>Merhaba {0},</h3>";
                        //  texts += "<p>Firmamıza iş başvurusunda bulunduğunuz için teşekkür ederiz. mülakat gerçekleştirdiğimiz ilgili pozisyonla ilgili süreç tamamlanmış bulunmaktadır. Tarafınızla yaptığımız Mülakatnin olumlu değerlendirilmediğini bildiriyoruz. </p>";
                        //  texts += "<p>Katılımınız için teşekkür eder başarılı bir kariyer dileriz.</p>";
                        //  var mesajs = string.Format(texts, cvSahibi.Name);
                        //  new Email().Template("Template1", "demo.jpg", "Mülakat Sonucu Hakkında", mesajs)
                        //   .Send(cvSahibi.Email, string.Format("{0} | {1}", "INFOLINE | WORKOFTIME", "Mülakat Sonucu Hakkında.."), true);
                        break;
                    case EnumHR_PlanResult.Tekrar:
                        var texttek = "<h3>Merhaba {0},</h3>";
                        texttek += "<p>{1} adlı kişi ile {2} tarihindeki mülakat için {3} adlı personel tekrar mülakat ayarlanmasını talep etmektedir.İlgili görüşmeyi ayarlamanızı rica ederiz.</p>";
                        texttek += "<p>{1} adlı kişinin detayı için lütfen <a href='{4}/HR/VWHR_Person/Detail?id={5}'> Buraya tıklayınız! </a></p>";
                        texttek += "<p> Bilgilerinize. </p>";
                        var mesajtek = string.Format(texttek, insankaynaklari.firstname + " " + insankaynaklari.lastname, cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", data.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, url, item.HrPersonId);
                        new Email().Template("Template1", "demo.jpg", "Mülakat Tekrarı Hakkında", mesajtek)
                         .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Tekrarı Hakkında.."), true);
                        break;
                    case EnumHR_PlanResult.Diger:
                        var textdiger = "<h3>Merhaba {0},</h3>";
                        textdiger += "<p>{1} adlı kişi ile {2} tarihindeki mülakata {3} adlı personel  <b>{4}</b> açıklamasını yapmıştır.</p>";
                        textdiger += "<p> Bilgilerinize. </p>";
                        var mesajdiger = string.Format(textdiger, insankaynaklari.firstname + " " + insankaynaklari.lastname, cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", data.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, item.Description);
                        new Email().Template("Template1", "demo.jpg", "Mülakat Sonucu Hakkında", mesajdiger)
                         .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Sonucu Hakkında.."), true);
                        break;

                    case EnumHR_PlanResult.DahaSonra:
                        break;
                }
            }

            if (resultstatus.result == true)
            {
                trans.Commit();

                var rsFile = new FileUploadSave(Request).SaveAs();
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Görüşme başarı ile kaydedildi.")
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Rollback();
            return Json(new ResultStatusUI
            {
                Result = false,
                FeedBack = feedback.Error("Kaydetme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);

        }

        [PageInfo("Planlanan Mülakatlar Güncelleme", SHRoles.IKYonetici, SHRoles.PersonelTalebi)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var hrPlanPerson = db.GetVWHR_PlanPersonById(id);
            if(hrPlanPerson == null)
            {
                var reqId = Request["id"];
                if (!string.IsNullOrEmpty(reqId))
                {
                    id = new Guid(reqId);
                    hrPlanPerson = db.GetVWHR_PlanPersonById(id);
                }
            }
            var model = new VMHR_PlanAndPerson().B_EntityDataCopyForMaterial(hrPlanPerson);

            model.HrPerson = db.GetHR_PersonById(model.HrPersonId.Value);
            if (hrPlanPerson != null)
            {
                var hrPlan = db.GetVWHR_PlanById(hrPlanPerson.HrPlanId.Value);
                if (hrPlan != null && hrPlan.PdsEvulateFormId.HasValue && hrPlan.PdsEvulateFormResultId.HasValue)
                {
                    var Quest = db.GetVWPDS_QuestionFormByFormId(hrPlan.PdsEvulateFormId.Value).OrderBy(c => c.groupOrder).ThenBy(v => v.questionOrder).ToArray();
                    var formResult = db.GetVWPDS_FormResultById(hrPlan.PdsEvulateFormResultId.Value);
                    if (formResult != null)
                    {
                        var qr = db.GetPDS_QuestionResultByFormResultId(formResult.id);
                        model.questionResult = qr;
                        model.questions = Quest;
                    }
                }
                model.hrPlan = hrPlan;
            }

            var personPositions = db.GetHR_PersonPositionByHrPersonId(model.HrPersonId.Value);
            var personKeywords = db.GetHR_PersonKeywordsByHrPersonId(model.HrPersonId.Value);
            model.personPosition = personPositions.Select(x => x.HrPositionId).ToArray();
            model.personKeywords = personKeywords.Select(x => x.HrKeywordsId).ToArray();

            return View(model);
        }

        [PageInfo("Personellerin planlanan Mülakatların güncelleme işlemi", SHRoles.IKYonetici, SHRoles.PersonelTalebi)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMHR_PlanAndPerson item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var resultstatus = new ResultStatus();
            resultstatus.result = true;
            var data = db.GetHR_PlanById(item.HrPlanId.HasValue ? item.HrPlanId.Value : System.UIHelper.Guid.Null);

            if (data == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Plan Bulunamadı."),
                }, JsonRequestBehavior.AllowGet);
            }

            var trans = db.BeginTransaction();

            if (item.questionResult != null)
            {
                double sumFactor = 0;

                foreach (var quest in item.questions)
                {
                    sumFactor += quest.factor.HasValue ? quest.factor.Value : 0;
                }

                foreach (var quesRes in item.questionResult)
                {
                    var datas = item.questions.Where(c => c.id == quesRes.questionFormId).FirstOrDefault();
                    quesRes.point = ((datas.factor * quesRes.score) / 5) * (100 / sumFactor);
                    quesRes.formResultId = data.PdsEvulateFormResultId;
                }

                resultstatus &= db.BulkUpdatePDS_QuestionResult(item.questionResult, false, trans);
            }

            item.HrPerson = db.GetHR_PersonById(item.HrPerson.id);
            resultstatus &= db.UpdateHR_Person(item.HrPerson, false, trans);

            data.changedby = userStatus.user.id;
            data.changed = DateTime.Now;
            data.Result = item.Result;
            data.Description = item.Description == null ? Request["Description"].Replace(",", "") : item.Description;
            resultstatus &= db.UpdateHR_Plan(data, false, trans);
            string url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            var cvSahibi = db.GetHR_PersonById(item.HrPersonId.Value);
            var insankaynaklariAll = db.GetSH_UserByRoleId(SHRoles.IKYonetici);


            foreach (var insankaynaklari in insankaynaklariAll)
            {

                switch ((EnumHR_PlanResult)item.Result)
                {
                    case EnumHR_PlanResult.Olumlu:
                        var text = "<h3>Merhaba {0},</h3>";
                        text += "<p>{1} adlı kişi ile {2} tarihindeki mülakata {3} adlı personel olumlu görüş bildirmiştir.</p>";
                        text += "<p>{1} adlı kişinin detayı için lütfen <a href='{4}/HR/VWHR_Person/Detail?id={5}'> Buraya tıklayınız! </a></p>";
                        text += "<p> Bilgilerinize. </p>";
                        var mesaj = string.Format(text, insankaynaklari.firstname + " " + insankaynaklari.lastname, cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", data.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, url, item.HrPersonId);
                        new Email().Template("Template1", "demo.jpg", "Mülakat Sonucu Hakkında", mesaj)
                         .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Sonucu Hakkında.."), true);
                        break;
                    case EnumHR_PlanResult.Olumsuz:
                        //  var texts = "<h3>Merhaba {0},</h3>";
                        //  texts += "<p>Firmamıza iş başvurusunda bulunduğunuz için teşekkür ederiz. Mülakat gerçekleştirdiğimiz ilgili pozisyonla ilgili süreç tamamlanmış bulunmaktadır. Tarafınızla yaptığımız Mülakatnin olumlu değerlendirilmediğini bildiriyoruz. </p>";
                        //  texts += "<p>Katılımınız için teşekkür eder başarılı bir kariyer dileriz.</p>";
                        //  var mesajs = string.Format(texts, cvSahibi.Name);
                        //  new Email().Template("Template1", "demo.jpg", "Mülakat Sonucu Hakkında", mesajs)
                        //   .Send(cvSahibi.Email, string.Format("{0} | {1}", "INFOLINE | WORKOFTIME", "Mülakat Sonucu Hakkında.."), true);
                        break;
                    case EnumHR_PlanResult.Tekrar:
                        var texttek = "<h3>Merhaba {0},</h3>";
                        texttek += "<p>{1} adlı kişi ile {2} tarihindeki mülakata {3} adlı personel tekrar görüşme ayarlanmasını talep etmektedir.İlgili görüşmeyi ayarlamanızı rica ederiz.</p>";
                        texttek += "<p>{1} adlı kişinin detayı için lütfen <a href='{4}/HR/VWHR_Person/Detail?id={5}'> Buraya tıklayınız! </a></p>";
                        texttek += "<p> Bilgilerinize. </p>";
                        var mesajtek = string.Format(texttek, insankaynaklari.firstname + " " + insankaynaklari.lastname, cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", data.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, url, item.HrPersonId);
                        new Email().Template("Template1", "demo.jpg", "Mülakat Tekrarı Hakkında", mesajtek)
                         .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Tekrarı Hakkında.."), true);
                        break;
                    case EnumHR_PlanResult.Diger:
                        var textdiger = "<h3>Merhaba {0},</h3>";
                        textdiger += "<p>{1} adlı kişi ile {2} tarihindeki mülakata {3} adlı personel  <b>{4}</b> açıklamasını yapmıştır.</p>";
                        textdiger += "<p> Bilgilerinize. </p>";
                        var mesajdiger = string.Format(textdiger, insankaynaklari.firstname + " " + insankaynaklari.lastname, cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", data.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, item.Description);
                        new Email().Template("Template1", "demo.jpg", "Mülakat Sonucu Hakkında", mesajdiger)
                         .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Sonucu Hakkında.."), true);
                        break;

                    case EnumHR_PlanResult.DahaSonra:
                        break;
                }
            }

            if (resultstatus.result == true)
            {
                trans.Commit();

                var rsFile = new FileUploadSave(Request).SaveAs();

                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Görüşme başarı ile kaydedildi.")
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Rollback();
            return Json(new ResultStatusUI
            {
                Result = false,
                FeedBack = feedback.Error("Kaydetme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);

        }

        [PageInfo("Personellerin Mülakat Silme", SHRoles.IKYonetici)]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new HR_PlanPerson { id = new Guid(a) });

            var dbresult = db.BulkDeleteHR_PlanPerson(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
