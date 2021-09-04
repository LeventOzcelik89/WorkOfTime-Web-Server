using Infoline.Framework.Database;
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
    public class VWHR_PersonController : Controller
    {
        [PageInfo("CV Havuzu", SHRoles.IKYonetici)]
        public ActionResult Index()
        {
            var db = new WorkOfTimeDatabase();
            var data = new VMKeywordAndPositions
            {
                positions = db.GetHR_Position(),
                keywords = db.GetHR_Keywords()
            };

            return View(data);
        }

        [PageInfo("CV Havuzu Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_Person(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWHR_PersonCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("CV Havuzu Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_Person(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("CV Havuzu Veri Methodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            return db.GetVWHR_PersonCount(condition.Filter);
        }


        [PageInfo("CV Detayı", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici, SHRoles.PersonelTalebi)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_PersonById(id);
            if (data == null)
            {
                var idReq = Request["id"];
                if (!string.IsNullOrEmpty(idReq))
                {
                    id = new Guid(idReq);
                }
                data = db.GetVWHR_PersonById(id);
            }
            var model = new WMHR_Person().B_EntityDataCopyForMaterial(data);
            model.HR_Plans = db.GetHR_PlanByHrPersonId(id);

            model.cards = new Models.Card
            {
                Olumlu = model.HR_Plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.Olumlu).Count(),
                Olumsuz = model.HR_Plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.Olumsuz).Count(),
                DahaSonra = model.HR_Plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.DahaSonra).Count(),
                Diger = model.HR_Plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.Diger).Count(),
                Gorusulmedi = model.HR_Plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.Gorusulmedi).Count(),
                Tekrar = model.HR_Plans.Where(x => x.Result == (Int32)EnumHR_PlanResult.Tekrar).Count()
            }; 

            var personPositions = db.GetHR_PersonPositionByHrPersonId(id);
            var personKeywords = db.GetHR_PersonKeywordsByHrPersonId(id);
            model.personPosition = personPositions.Select(x => x.HrPositionId).ToArray();
            model.personKeywords = personKeywords.Select(x => x.HrKeywordsId).ToArray();
            return View(model);
        }

        [PageInfo("Hemen Mülakat Yap", SHRoles.IsGorusmesiMulakat)]
        public ActionResult ManagerInsert(Guid? id)
        {
            var db = new WorkOfTimeDatabase();
            var model = new WMHR_Person();
            var Evulate = db.GetVWPDS_EvaluateFormByInterviewFirst();
            if (Evulate != null)
            {
                var Quest = db.GetVWPDS_QuestionFormByFormId(Evulate.id);
                model.questions = Quest;
            }
            if (id.HasValue)
            {
                model.B_EntityDataCopyForMaterial(db.GetVWHR_PersonById(id.Value));
                var personPositions = db.GetHR_PersonPositionByHrPersonId(id.Value);
                var personKeywords = db.GetHR_PersonKeywordsByHrPersonId(id.Value);
                model.personPosition = personPositions.Select(x => x.HrPositionId).ToArray();
                model.personKeywords = personKeywords.Select(x => x.HrKeywordsId).ToArray();
            }
            else
            {
                model.id = Guid.NewGuid();
                model.Gender = (int)EnumHR_PersonGender.Erkek;
                model.MaritalStatus = (int)EnumHR_PersonMaritalStatus.Bekar;
            }
            return View(model);
        }

        [PageInfo("CV Ekleme, Değerlendirme Mülakat Ekleme", SHRoles.IsGorusmesiMulakat)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult ManagerInsert(WMHR_Person item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            if (item.Position == null || item.Keywords == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Lütfen bilgi/beceri veya pozisyon alanlarını doldurunuz."),
                }, JsonRequestBehavior.AllowGet);
            }
            var resultstatus = new ResultStatus();
            resultstatus.result = true;
            var hrPersonId = Request["HrPersonId"];
            var myId = hrPersonId != "" ? new Guid(hrPersonId) : item.id;


            var trans = db.BeginTransaction();

            if (item.questionResult != null)
            {
                item.item = new PDS_FormResult
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    evaluateFormId = item.questions.Select(c => c.evaluateFormId).FirstOrDefault(),
                    evaluatorId = userStatus.user.id,
                    evaluatedUserId = item.id,
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
                resultstatus &= db.InsertPDS_FormResult(item.item, trans);
                resultstatus &= db.BulkInsertPDS_QuestionResult(item.questionResult, trans);
            }

            var PositionData = item.Position.Select(x => x).ToList();
            var KeywordsData = item.Keywords.Select(x => x).ToList();

            if (hrPersonId != "")
            {
                var hrPersonData = db.GetHR_PersonById(myId);
                hrPersonData.id = myId;
                hrPersonData.ArrivalType = item.ArrivalType;
                hrPersonData.Birthday = item.Birthday;
                hrPersonData.created = DateTime.Now;
                hrPersonData.createdby = userStatus.user.id;
                hrPersonData.LocationId = item.LocationId;
                hrPersonData.Description = item.Description;
                hrPersonData.DriverLicense = item.DriverLicense;
                hrPersonData.Education = item.Education;
                hrPersonData.Email = item.Email;
                hrPersonData.ExprienceYear = item.ExprienceYear;
                hrPersonData.IdentifyNumber = item.IdentifyNumber;
                hrPersonData.Language = item.Language;
                hrPersonData.LanguageType = item.LanguageType;
                hrPersonData.MilitaryStatus = item.MilitaryStatus;
                hrPersonData.Name = item.Name;
                hrPersonData.Phone = item.Phone;
                hrPersonData.ReferenceId = item.ReferenceId;
                hrPersonData.SurName = item.SurName;
                hrPersonData.Adress = item.Adress;
                hrPersonData.Branch = item.Branch;
                hrPersonData.RetirementDate = item.RetirementDate;
                hrPersonData.SchoolName = item.SchoolName;
                hrPersonData.SalaryRangeMin = item.SalaryRangeMin;
                hrPersonData.SalaryRangeMax = item.SalaryRangeMax;
                hrPersonData.MaritalStatus = item.MaritalStatus;
                hrPersonData.Gender = item.Gender;
                hrPersonData.MilitaryExemptDetail = item.MilitaryExemptDetail;
                hrPersonData.MilitaryDoneDate = item.MilitaryDoneDate;
                resultstatus &= db.UpdateHR_Person(hrPersonData);
            }
            else
            {
                var hrPerson = new HR_Person
                {
                    id = myId,
                    ArrivalType = item.ArrivalType,
                    Birthday = item.Birthday,
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    LocationId = item.LocationId,
                    Description = item.Description,
                    DriverLicense = item.DriverLicense,
                    Education = item.Education,
                    Email = item.Email,
                    ExprienceYear = item.ExprienceYear,
                    IdentifyNumber = item.IdentifyNumber,
                    Language = item.Language,
                    LanguageType = item.LanguageType,
                    MilitaryStatus = item.MilitaryStatus,
                    Name = item.Name,
                    Phone = item.Phone,
                    ReferenceId = item.ReferenceId,
                    SurName = item.SurName,
                    Adress = item.Adress,
                    Branch = item.Branch,
                    RetirementDate = item.RetirementDate,
                    SchoolName = item.SchoolName,
                    SalaryRangeMin = item.SalaryRangeMin,
                    SalaryRangeMax = item.SalaryRangeMax,
                    MaritalStatus=item.MaritalStatus,
                    Gender=item.Gender,
                    MilitaryExemptDetail=item.MilitaryExemptDetail,
                    MilitaryDoneDate=item.MilitaryDoneDate
                };

                resultstatus &= db.InsertHR_Person(hrPerson, trans);
            }

            var personPositionList = new List<HR_PersonPosition>();
            foreach (var pos in PositionData)
            {
                var dataPosition = db.GetHR_PositionById(pos);
                var hrPersonPos = new HR_PersonPosition
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    HrPositionId = dataPosition.id,
                    HrPersonId = myId
                };
                personPositionList.Add(hrPersonPos);
            }


            var personKeyList = new List<HR_PersonKeywords>();
            foreach (var key in KeywordsData)
            {
                var dataPosition = db.GetHR_KeywordsById(key);
                var hrPersonKey = new HR_PersonKeywords
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    HrKeywordsId = dataPosition.id,
                    HrPersonId = myId
                };
                personKeyList.Add(hrPersonKey);
            }


            var hrPlan = new HR_Plan
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                PlanDate = DateTime.Now,
                Description = item.HR_Plan.Description,
                Result = item.HR_Plan.Result == null ? Convert.ToInt32(Request["Result"]) : item.HR_Plan.Result,
                HrPersonId = myId
            };

            if (item.item != null)
            {
                hrPlan.PdsEvulateFormId = item.item.evaluateFormId;
                hrPlan.PdsEvulateFormResultId = item.item.id;
            }

            var hrPlanPerson = new HR_PlanPerson
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                UserId = userStatus.user.id,
                HrPlanId = hrPlan.id
            };

            resultstatus &= db.BulkInsertHR_PersonKeywords(personKeyList, trans);
            resultstatus &= db.BulkInsertHR_PersonPosition(personPositionList, trans);
            resultstatus &= db.InsertHR_Plan(hrPlan, trans);
            resultstatus &= db.InsertHR_PlanPerson(hrPlanPerson, trans);

            if (resultstatus.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kaydetme işlemi gerçekleşirken sorun oluştu.")
                }, JsonRequestBehavior.AllowGet);
            }

            var rsFile = new FileUploadSave(Request, myId).SaveAs();

            trans.Commit();

            string url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            var cvSahibi = db.GetHR_PersonById(myId);
            var insankaynaklari = db.GetSH_UserByRoleId(SHRoles.IKYonetici).FirstOrDefault();
            if (insankaynaklari != null)
            {
                if (insankaynaklari.email != null)
                {
                    switch ((EnumHR_PlanResult)hrPlan.Result)
                    {
                        case EnumHR_PlanResult.Olumlu:
                            var text = "<h3>Merhaba {0},</h3>";
                            text += "<p>{1} adlı kişi ile {2} tarihindeki mülakata {3} adlı personel olumlu görüş bildirmiştir.</p>";
                            text += "<p>{1} adlı kişinin detayı için lütfen <a href='{4}/HR/VWHR_Person/Detail?id={5}'> Buraya tıklayınız! </a></p>";
                            text += "<p> Bilgilerinize. </p>";
                            var mesaj = string.Format(text, (insankaynaklari.firstname != null ? insankaynaklari.firstname : " ") + " " + (insankaynaklari.lastname != null ? insankaynaklari.lastname : " "), cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", hrPlan.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, url, myId);
                            new Email().Template("Template1", "demo.jpg", "Mülakat Sonucu Hakkında", mesaj)
                             .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Sonucu Hakkında.."), true);
                            break;
                        case EnumHR_PlanResult.Olumsuz:
                            //  var texts = "<h3>Merhaba {0},</h3>";
                            //  texts += "<p>Firmamıza iş başvurusunda bulunduğunuz için teşekkür ederiz. İş görüşmesi gerçekleştirdiğimiz ilgili pozisyonla ilgili süreç tamamlanmış bulunmaktadır. Tarafınızla yaptığımız iş görüşmesinin olumlu değerlendirilmediğini bildiriyoruz. </p>";
                            //  texts += "<p>Katılımınız için teşekkür eder başarılı bir kariyer dileriz.</p>";
                            //  var mesajs = string.Format(texts , cvSahibi.Name);
                            //  new Email().Template("Template1", "demo.jpg", "İş Görüşmesi Sonucu Hakkında", mesajs)
                            //   .Send(cvSahibi.Email, string.Format("{0} | {1}", "INFOLINE | WORKOFTIME", "İş Görüşmesi Sonucu Hakkında.."), true);
                            break;
                        case EnumHR_PlanResult.Tekrar:
                            var texttek = "<h3>Merhaba {0},</h3>";
                            texttek += "<p>{1} adlı kişi ile {2} tarihindeki mülakata {3} adlı personel tekrar görüşme ayarlanmasını talep etmektedir.İlgili görüşmeyi ayarlamanızı rica ederiz.</p>";
                            texttek += "<p>{1} adlı kişinin detayı için lütfen <a href='{4}/HR/VWHR_Person/Detail?id={5}'> Buraya tıklayınız! </a></p>";
                            texttek += "<p> Bilgilerinize. </p>";
                            var mesajtek = string.Format(texttek, (insankaynaklari.firstname != null ? insankaynaklari.firstname : " ") + " " + (insankaynaklari.lastname != null ? insankaynaklari.lastname : " "), cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", hrPlan.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, url, myId);
                            new Email().Template("Template1", "demo.jpg", "Mülakat Tekrarı Hakkında", mesajtek)
                             .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Tekrarı Hakkında.."), true);
                            break;
                        case EnumHR_PlanResult.Diger:
                            var textdiger = "<h3>Merhaba {0},</h3>";
                            textdiger += "<p>{1} adlı kişi ile {2} tarihindeki mülakata {3} adlı personel  <b>{4}</b> açıklamasını yapmıştır.</p>";
                            textdiger += "<p> Bilgilerinize. </p>";
                            var mesajdiger = string.Format(textdiger, (insankaynaklari.firstname != null ? insankaynaklari.firstname : " ") + " " + (insankaynaklari.lastname != null ? insankaynaklari.lastname : " "), cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", hrPlan.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, hrPlan.Description);
                            new Email().Template("Template1", "demo.jpg", "Mülakat Sonucu Hakkında", mesajdiger)
                             .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Sonucu Hakkında.."), true);
                            break;
                    }
                }
            }

            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("CV kayıt, değerlendirme ve görüş kaydınız başarı ile alınmıştır.", false, Url.Action("MyIndex", "VWHR_PlanPerson", new { area = "HR" }))
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Hemen Mülakat Yap", SHRoles.IsGorusmesiMulakat)]
        public ActionResult ManagerInsertToDetail(Guid? id)
        {
            var db = new WorkOfTimeDatabase();
            var model = new WMHR_Person();
            var Evulate = db.GetVWPDS_EvaluateFormByInterviewFirst();
            if (Evulate != null)
            {
                var Quest = db.GetVWPDS_QuestionFormByFormId(Evulate.id);
                model.questions = Quest;
            }
            if (id.HasValue)
            {
                model.B_EntityDataCopyForMaterial(db.GetVWHR_PersonById(id.Value));
                var personPositions = db.GetHR_PersonPositionByHrPersonId(id.Value);
                var personKeywords = db.GetHR_PersonKeywordsByHrPersonId(id.Value);
                model.personPosition = personPositions.Select(x => x.HrPositionId).ToArray();
                model.personKeywords = personKeywords.Select(x => x.HrKeywordsId).ToArray();
            }
            else
            {
                model.id = Guid.NewGuid();
            }
            return View(model);
        }

        [PageInfo("CV Ekleme, Değerlendirme Mülakat Ekleme", SHRoles.IsGorusmesiMulakat)]
        [HttpPost, ValidateAntiForgeryToken,AllowEveryone]
        public JsonResult ManagerInsertToDetail(WMHR_Person item, Guid? PdsEvulateFormId)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var resultstatus = new ResultStatus();
            resultstatus.result = true;
            var trans = db.BeginTransaction();

            if (item.questionResult != null)
            {
                item.item = new PDS_FormResult
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    evaluateFormId = PdsEvulateFormId.HasValue ? PdsEvulateFormId.Value :  item.questions.Select(c => c.evaluateFormId).FirstOrDefault(),
                    evaluatorId = userStatus.user.id,
                    evaluatedUserId = item.id,
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
                resultstatus &= db.InsertPDS_FormResult(item.item, trans);
                resultstatus &= db.BulkInsertPDS_QuestionResult(item.questionResult, trans);
            }
           

            var hrPlan = new HR_Plan
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                PlanDate = DateTime.Now,
                Description = item.HR_Plan.Description,
                Result = item.HR_Plan.Result == null ? Convert.ToInt32(Request["Result"]) : item.HR_Plan.Result,
                HrPersonId = item.id
            };

            if (item.item != null)
            {
                hrPlan.PdsEvulateFormId = item.item.evaluateFormId;
                hrPlan.PdsEvulateFormResultId = item.item.id;
            }

            resultstatus &= db.InsertHR_Plan(hrPlan, trans);
            db.InsertHR_PlanPerson(new HR_PlanPerson
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = userStatus.user.id,
                UserId = userStatus.user.id,
                HrPlanId = hrPlan.id
            }, trans);

            if (resultstatus.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Kaydetme işlemi gerçekleşirken sorun oluştu.")
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();
            string url = TenantConfig.Tenant.GetWebUrl();
            var tenantName = TenantConfig.Tenant.TenantName;
            var cvSahibi = db.GetHR_PersonById(item.id);
            var insankaynaklari = db.GetSH_UserByRoleId(SHRoles.IKYonetici).FirstOrDefault();
            if (insankaynaklari != null)
            {
                if (insankaynaklari.email != null)
                {
                    switch ((EnumHR_PlanResult)hrPlan.Result)
                    {
                        case EnumHR_PlanResult.Olumlu:
                            var text = "<h3>Merhaba {0},</h3>";
                            text += "<p>{1} adlı kişi ile {2} tarihindeki mülakata {3} adlı personel olumlu görüş bildirmiştir.</p>";
                            text += "<p>{1} adlı kişinin detayı için lütfen <a href='{4}/HR/VWHR_Person/Detail?id={5}'> Buraya tıklayınız! </a></p>";
                            text += "<p> Bilgilerinize. </p>";
                            var mesaj = string.Format(text, (insankaynaklari.firstname != null ? insankaynaklari.firstname : " ") + " " + (insankaynaklari.lastname != null ? insankaynaklari.lastname : " "), cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", hrPlan.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, url, item.id);
                            new Email().Template("Template1", "demo.jpg", "Mülakat Sonucu Hakkında", mesaj)
                             .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Sonucu Hakkında.."), true);
                            break;
                        case EnumHR_PlanResult.Olumsuz:
                            //  var texts = "<h3>Merhaba {0},</h3>";
                            //  texts += "<p>Firmamıza iş başvurusunda bulunduğunuz için teşekkür ederiz. İş görüşmesi gerçekleştirdiğimiz ilgili pozisyonla ilgili süreç tamamlanmış bulunmaktadır. Tarafınızla yaptığımız iş görüşmesinin olumlu değerlendirilmediğini bildiriyoruz. </p>";
                            //  texts += "<p>Katılımınız için teşekkür eder başarılı bir kariyer dileriz.</p>";
                            //  var mesajs = string.Format(texts , cvSahibi.Name);
                            //  new Email().Template("Template1", "demo.jpg", "İş Görüşmesi Sonucu Hakkında", mesajs)
                            //   .Send(cvSahibi.Email, string.Format("{0} | {1}", "INFOLINE | WORKOFTIME", "İş Görüşmesi Sonucu Hakkında.."), true);
                            break;
                        case EnumHR_PlanResult.Tekrar:
                            var texttek = "<h3>Merhaba {0},</h3>";
                            texttek += "<p>{1} adlı kişi ile {2} tarihindeki mülakata {3} adlı personel tekrar görüşme ayarlanmasını talep etmektedir.İlgili görüşmeyi ayarlamanızı rica ederiz.</p>";
                            texttek += "<p>{1} adlı kişinin detayı için lütfen <a href='{4}/HR/VWHR_Person/Detail?id={5}'> Buraya tıklayınız! </a></p>";
                            texttek += "<p> Bilgilerinize. </p>";
                            var mesajtek = string.Format(texttek, (insankaynaklari.firstname != null ? insankaynaklari.firstname : " ") + " " + (insankaynaklari.lastname != null ? insankaynaklari.lastname : " "), cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", hrPlan.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, url, item.id);
                            new Email().Template("Template1", "demo.jpg", "Mülakat Tekrarı Hakkında", mesajtek)
                             .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Tekrarı Hakkında.."), true);
                            break;
                        case EnumHR_PlanResult.Diger:
                            var textdiger = "<h3>Merhaba {0},</h3>";
                            textdiger += "<p>{1} adlı kişi ile {2} tarihindeki mülakata {3} adlı personel  <b>{4}</b> açıklamasını yapmıştır.</p>";
                            textdiger += "<p> Bilgilerinize. </p>";
                            var mesajdiger = string.Format(textdiger, (insankaynaklari.firstname != null ? insankaynaklari.firstname : " ") + " " + (insankaynaklari.lastname != null ? insankaynaklari.lastname : " "), cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", hrPlan.PlanDate), userStatus.user.firstname + " " + userStatus.user.lastname, hrPlan.Description);
                            new Email().Template("Template1", "demo.jpg", "Mülakat Sonucu Hakkında", mesajdiger)
                             .Send((Int16)EmailSendTypes.PersonelAdayMulakat, insankaynaklari.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Sonucu Hakkında.."), true);
                            break;
                    }
                }
            }

            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("CV kayıt, değerlendirme ve görüş kaydınız başarı ile alınmıştır.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Yeni CV Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert()
        {
            var data = new VWHR_Person { id = Guid.NewGuid(), Gender=(int)EnumHR_PersonGender.Erkek, MaritalStatus=(int)EnumHR_PersonMaritalStatus.Bekar };
            return View(data);
        }

        [PageInfo("Yeni CV Ekleme Methodu", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(HR_Person item, Guid[] Position, Guid[] Keywords)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var results = new ResultStatus();
            results.result = true;

            if (Position == null || Keywords == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Lütfen bilgi/beceri veya pozisyon alanlarını doldurunuz."),
                }, JsonRequestBehavior.AllowGet);
            }


            if (Position.Count() == 0 || Keywords.Count() == 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Lütfen zorunlu alanları doldurunuz."),
                }, JsonRequestBehavior.AllowGet);
            }


            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var personPositionList = new List<HR_PersonPosition>();
            foreach (var pos in Position)
            {
                var dataPosition = db.GetHR_PositionById(pos);
                if (dataPosition != null)
                {
                    personPositionList.Add(new HR_PersonPosition
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        HrPositionId = dataPosition.id,
                        HrPersonId = item.id
                    });
                }
            }


            var personKeyList = new List<HR_PersonKeywords>();
            foreach (var key in Keywords)
            {
                var dataPosition = db.GetHR_KeywordsById(key);
                if (dataPosition != null)
                {
                    personKeyList.Add(new HR_PersonKeywords
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        HrKeywordsId = dataPosition.id,
                        HrPersonId = item.id
                    });
                }
            }


            var trans = db.BeginTransaction();
            results &= db.InsertHR_Person(item, trans);
            results &= db.BulkInsertHR_PersonKeywords(personKeyList, trans);
            results &= db.BulkInsertHR_PersonPosition(personPositionList, trans);

            if (results.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Cv kaydetme işlemi başarısız oldu.Lütfen daha sonra tekrar deneyin.")
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();
            var rsFile = new FileUploadSave(Request, item.id).SaveAs();
            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Cv kaydetme işlemi başarı ile gerçekleşti.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("CV Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_PersonById(id);
            var model = new WMHR_Person().B_EntityDataCopyForMaterial(data);
            model.HR_Plans = db.GetHR_PlanByHrPersonId(id);
            var personPositions = db.GetHR_PersonPositionByHrPersonId(id);
            var personKeywords = db.GetHR_PersonKeywordsByHrPersonId(id);
            model.personPosition = personPositions.Select(x => x.HrPositionId).ToArray();
            model.personKeywords = personKeywords.Select(x => x.HrKeywordsId).ToArray();
            return View(model);
        }

        [PageInfo("Cv Güncelleme Methodu", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(HR_Person item, Guid[] Position, Guid[] Keywords)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var results = new ResultStatus();
            results.result = true;

            if (Position == null || Keywords == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Lütfen bilgi/beceri veya pozisyon alanlarını doldurunuz."),
                }, JsonRequestBehavior.AllowGet);
            }

            var PositionData = Position.Select(x => x).ToList();
            var KeywordsData = Keywords.Select(x => x).ToList();

            if (PositionData.Count() == 0 || KeywordsData.Count() == 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Lütfen bilgi/beceri veya pozisyon alanlarını doldurunuz."),
                }, JsonRequestBehavior.AllowGet);
            }

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var personPositionList = new List<HR_PersonPosition>();
            foreach (var pos in PositionData)
            {
                var dataPosition = db.GetHR_PositionById(pos);
                personPositionList.Add(new HR_PersonPosition
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    HrPositionId = dataPosition.id,
                    HrPersonId = item.id
                });
            }


            var personKeyList = new List<HR_PersonKeywords>();
            foreach (var key in KeywordsData)
            {
                var dataPosition = db.GetHR_KeywordsById(key);
                personKeyList.Add(new HR_PersonKeywords
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    HrKeywordsId = dataPosition.id,
                    HrPersonId = item.id
                });
            }

            var trans = db.BeginTransaction();
            results &= db.UpdateHR_Person(item,true,trans);
            results &= db.BulkDeleteHR_PersonKeywords(db.GetHR_PersonKeywordsByHrPersonId(item.id), trans);
            results &= db.BulkDeleteHR_PersonPosition(db.GetHR_PersonPositionByHrPersonId(item.id), trans);
            results &= db.BulkInsertHR_PersonKeywords(personKeyList, trans);
            results &= db.BulkInsertHR_PersonPosition(personPositionList, trans);

            if (results.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Cv güncelleme işlemi başarısız oldu.")
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();

            var rsFile = new FileUploadSave(Request, item.id).SaveAs();
            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Cv güncelleme işlemi başarı ile tamamlandı.", false, Url.Action("Index", "VWHR_Person", new { area = "HR" }))
            }, JsonRequestBehavior.AllowGet);

        }

        [PageInfo("CV Silme", SHRoles.IKYonetici)]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            //TO DO:Oğuz eğer burada silme işlemini yaptırırsak o kişiye ait keyword ve positionlar da silinecek
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new HR_Person { id = new Guid(a) });

            var dbresult = db.BulkDeleteHR_Person(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Personel Şube Grubu", SHRoles.Personel)]
        public ContentResult GetHR_PersonBranchGroup()
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_Person().GroupBy(x => x.Branch);
            var veri = data.Where(x => x.Key != null).Select(c => c.Key).ToArray();
            return Content(Infoline.Helper.Json.Serialize(veri), "application/json");
        }

    }
}
