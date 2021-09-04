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
    public class VWHR_PlanController : Controller
    {
        [PageInfo("Mülakat Planlamaları", SHRoles.IKYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Mülakat Planlamaları Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_Plan(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWHR_PlanCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Mülakat Planlamaları Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_Plan(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("Mülakat Planlamaları Detayı", SHRoles.IKYonetici, SHRoles.PersonelTalebi)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var datas = new VMDetailPlanPersonAndPds();
            datas.HR_Plan = db.GetVWHR_PlanById(id);
            datas.HR_Person = db.GetHR_PersonById(datas.HR_Plan.HrPersonId.Value);
            var list = new List<PDS_QuestionResult>();
            if (datas.HR_Plan.PdsEvulateFormResultId != null)
            {
                datas.questions = db.GetVWPDS_QuestionFormByFormId(datas.HR_Plan.PdsEvulateFormId.Value);
                var questionRes = db.GetPDS_QuestionResultByFormResultId(datas.HR_Plan.PdsEvulateFormResultId.Value);
                for (int i = 0; i < datas.questions.Count(); i++)
                {
                    list.Add(questionRes.Where(c => c.questionFormId == datas.questions[i].id).FirstOrDefault());
                }
                datas.questionResult = list.ToArray();
            }

            return View(datas);
        }


        [PageInfo("Değerlendirme Detayı", SHRoles.IKYonetici, SHRoles.PersonelTalebi)]
        public ActionResult DetailForPds(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var datas = new VMDetailPlanPersonAndPds();
            datas.HR_Plan = db.GetVWHR_PlanById(id);
            datas.HR_Person = db.GetHR_PersonById(datas.HR_Plan.HrPersonId.Value);
            var list = new List<PDS_QuestionResult>();
            if (datas.HR_Plan.PdsEvulateFormResultId != null)
            {
                datas.questions = db.GetVWPDS_QuestionFormByFormId(datas.HR_Plan.PdsEvulateFormId.Value);
                var questionRes = db.GetPDS_QuestionResultByFormResultId(datas.HR_Plan.PdsEvulateFormResultId.Value);
                for (int i = 0; i < datas.questions.Count(); i++)
                {
                    list.Add(questionRes.Where(c => c.questionFormId == datas.questions[i].id).FirstOrDefault());
                }
                datas.questionResult = list.ToArray();
            }
            return View(datas);
        }

        [AllowEveryone]
        [PageInfo("Değerlendirme Ekle", SHRoles.IKYonetici)]
        public ActionResult InsertForPds(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var datas = new VMDetailPlanPersonAndPds();
            datas.questions = db.GetVWPDS_QuestionFormByFormId(id);
            return View(datas);
        }


        [PageInfo("Mülakat Planlamaları Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(Guid? hrPersonId, Guid? staffNeedId)
        {
            var data = new VWHR_Plan
            {
                id = Guid.NewGuid(),
                HrPersonId = hrPersonId,
                StaffNeedsId = staffNeedId
            };
            return View(data);
        }

        [PageInfo("Mülakat planlama işleminin gerçekleştirileceği methoddur.", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(HR_Plan item, Guid[] userList)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var results = new ResultStatus();
            results.result = true;

            if (userList != null && userList.Count() == 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Lütfen personel seçimi gerçekleştirin.")
                }, JsonRequestBehavior.AllowGet);
            }
            var planPersonList = new List<HR_PlanPerson>();
            var trans = db.BeginTransaction();

            foreach (var user in userList)
            {
                planPersonList.Add(new HR_PlanPerson
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    HrPlanId = item.id,
                    UserId = user
                });

                if (db.GetSH_UserRoleByUserId(user).Where(x => x.roleid == new Guid(SHRoles.IsGorusmesiMulakat)).Count() == 0)
                {
                    results &= db.InsertSH_UserRole(new SH_UserRole
                    {
                        roleid = new Guid(SHRoles.IsGorusmesiMulakat),
                        userid = user,
                        createdby = userStatus.user.id
                    }, trans);
                }
            }

            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            item.Result = (Int32)EnumHR_PlanResult.Gorusulmedi;
            results &= db.InsertHR_Plan(item, trans);
            results &= db.BulkInsertHR_PlanPerson(planPersonList, trans);


            if (item.StaffNeedsId.HasValue)
            {
                var hrperson = db.GetHR_PersonById(item.HrPersonId.Value);
                var users = db.GetVWSH_UserByIds(planPersonList.Where(c => c.UserId.HasValue).Select(c => c.UserId.Value).ToArray());
                var needStatus = new HR_StaffNeedsStatus
                {
                    staffNeedId = item.StaffNeedsId.Value,
                    status = (Int32)EnumHR_StaffNeedsStatusStatus.MulakatAyarlandi,
                    description = "<p>" + userStatus.user.FullName + " tarafından mülakat ayarlanmıştır.</p>" +
                                  "<p> Mülakat Gerçekleştirilecek Kişi : <strong>" + hrperson.Name + " " + hrperson.SurName + "</strong></p>" +
                                  "<p> Mülakata Katılacak Personeller : " + string.Join(" ,", users.Select(c => c.FullName).ToArray()) + "</p>" +
                                  "<p> Mülakat Tarihi : " + string.Format("{0:dd.MM.yyyy HH:mm}", item.PlanDate) + "</p>"

                };

                results &= db.InsertHR_StaffNeedsStatus(needStatus, trans);
            }

            if (results.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Mülakat ayarlanırken problem oluştu.Lütfen tekrar deneyiniz.")
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                trans.Commit();
                if (item.CompanyId != null)
                {
                    var compLocation = db.GetVWCMP_CompanyById(item.CompanyId.Value);
                    if (compLocation != null)
                    {
                        if (compLocation.location != null)
                        {
                            if (item.HrPersonId.HasValue)
                            {
                                var cvSahibi = db.GetHR_PersonById(item.HrPersonId.Value);
                                var insankaynaklari = db.GetSH_UserByRoleId(SHRoles.IKYonetici);
                                var tenantName = TenantConfig.Tenant.TenantName;
                                if (item.mailSend == true)
                                {

                                    var text = "<h3>Sn. {0},</h3>";
                                    text += "<p> Mülakat programına ait detaylı bilgiyi aşağıda tarafınıza sunarız. Gelirken yanınızda bir adet fotoğraf bulundurmanızı rica ederiz.</p>";
                                    text += "<b> {1} </b>";
                                    text += "<p></p>";
                                    text += "<b> İnsan Kaynakları Direktörlüğü </b>";
                                    text += "<p></p>";
                                    text += "<p>Tarih ve Saat : {2}</p>";
                                    text += "<p>Adres : {5}</p>";
                                    text += "<p>Toplantı Linki : {6}</p>";
                                    text += "<p>Mülakat konumuna gitmek için <a href='maps.google.com/maps/?q={3},{4}'> Buraya tıklayınız! </a></p>";
                                    var mesaj = string.Format(text, cvSahibi.Name + " " + cvSahibi.SurName, compLocation.fullName, string.Format("{0:dd.MM.yyyy HH:mm}", item.PlanDate), compLocation.location.Coordinate.Y.ToString().Replace(",", "."), compLocation.location.Coordinate.X.ToString().Replace(",", "."), compLocation.openAddress,item.contactLink);
                                    new Email().Template("Template1", "demo.jpg", "MÜLAKAT DAVETİ", mesaj)
                                     .Send((Int16)EmailSendTypes.PersonelAdayMulakat, cvSahibi.Email, 
                                     string.Format("{0} | {1}", tenantName +" | WORKOFTIME", "MÜLAKAT DAVETİ Hakkında"), true, null, null,
                                     (insankaynaklari.Count() > 0 ?
                                     insankaynaklari.Select(x => x.email).ToArray() :
                                     new string[] { "ik@ik.com.tr" }));
                                }
                                var url = TenantConfig.Tenant.GetWebUrl();
                                foreach (var user in userList)
                                {
                                    var userbilgi = db.GetSH_UserById(user);
                                    var textUser = "<h3>Merhaba {0},</h3>";
                                    textUser += "<p>{1} adlı şirkette {2} tarihinde {3} adlı kişiyle mülakat planlanmıştır.</p>";
                                    textUser += "<p>Mülakatlarınızı görmek için lütfen <a href='{4}/HR/VWHR_PlanPerson/MyIndex'> Buraya tıklayınız! </a></p>";
                                    textUser += "<p> Saygılarımızla. </p>";
                                    var mesajs = string.Format(textUser, userbilgi.firstname + " " + userbilgi.lastname, compLocation.fullName, string.Format("{0:dd.MM.yyyy HH:mm}", item.PlanDate), cvSahibi.Name + " " + cvSahibi.SurName, url);
                                    new Email().Template("Template1", "demo.jpg", "Mülakat Planı Hakkında", mesajs)
                                        .Send((Int16)EmailSendTypes.PersonelAdayMulakat, userbilgi.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Hakkında.."), true);
                                }

                                var emailUsers = string.Join(";", db.GetVWSH_UserByIds(userList.ToArray()).Select(x => x.email).ToArray());
                                if (!string.IsNullOrEmpty(emailUsers))
                                {
                                    new Email().SendCalendar(emailUsers, cvSahibi.Name + " " + cvSahibi.SurName + " adlı kişi ile mülakat yapılacaktır.", cvSahibi.Name + " " + cvSahibi.SurName + " adlı kişi ile iş görüşmesi yapılacaktır.", item.PlanDate.Value, item.PlanDate.Value.AddMinutes(30));
                                }
                            }
                        }
                    }
                }
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Mülakat ayarlama işlemi başarı ile gerçekleşti.",false, Request.UrlReferrer.AbsoluteUri)
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [PageInfo("Mülakat planlama işleminin düzenleneceği sayfaya yönlendiren methoddur.", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetHR_PlanPersonByPlanId(id);
            var model = new VMHR_PlanAndPerson();
            model.hrPlan = db.GetVWHR_PlanById(id);
            model.persons = data.Select(c => c.UserId).ToArray();
            return View(model);
        }

        [PageInfo("Mülakat planlama işleminin düzenlemesinin gerçekleştileceği methoddur.", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMHR_PlanAndPerson item, Guid?[] userList, Guid? CompanyId)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var results = new ResultStatus();
            results.result = true;

            item.hrPlan.changed = DateTime.Now;
            item.hrPlan.changedby = userStatus.user.id;

            var plan = db.GetHR_PlanById(item.hrPlan.id);


            if (plan.Result != (Int32)EnumHR_PlanResult.Gorusulmedi && plan.Result != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Sonucu bulunan mülakatta düzenleme gerçekleştiremezsiniz..")
                }, JsonRequestBehavior.AllowGet);
            }


            var planperson = db.GetHR_PlanPersonByPlanId(item.hrPlan.id);

            var gorusmedekieskipersoneller = planperson.Select(x => x.UserId);
            var yenipersoneller = userList;
            var compLocation = db.GetVWCMP_CompanyById(item.hrPlan.CompanyId.HasValue ? item.hrPlan.CompanyId.Value : CompanyId.HasValue ? CompanyId.Value : new Guid());
            var tenantName = TenantConfig.Tenant.TenantName;


            foreach (var eskiperson in gorusmedekieskipersoneller)
            {
                var control = yenipersoneller.Where(x => x == eskiperson);
                if (control.Count() > 0)
                {
                    if (plan.PlanDate != item.hrPlan.PlanDate)
                    {
                        var eskiuserr = db.GetSH_UserById(eskiperson.Value);
                        var text = "<h3>Merhaba {0},</h3>";
                        text += "<p>{1} adlı şirkette {2} tarihinde gerçekleştireceğiniz mülakat {3} tarihi olarak değiştirilmiştir</p>";
                        text += "<p> Saygılarımızla. </p>";
                        text += "<p> {1} </p>";
                        var mesaj = string.Format(text, eskiuserr.firstname + " " + eskiuserr.lastname, (compLocation != null ? compLocation.fullName : ""), string.Format("{0:dd.MM.yyyy HH:mm}", plan.PlanDate), string.Format("{0:dd.MM.yyyy HH:mm}", item.hrPlan.PlanDate));
                        new Email().Template("Template1", "demo.jpg", "Mülakat Zaman Planı Hakkında", mesaj)
                         .Send((Int16)EmailSendTypes.PersonelAdayMulakat, eskiuserr.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Zaman Planı Hakkında.."), true);
                    }
                }
                else
                {
                    var eskiuserr = db.GetSH_UserById(eskiperson.Value);

                    var text = "<h3>Merhaba {0},</h3>";
                    text += "<p>{1} adlı şirkette {2} tarihinde gerçekleştireceğiniz mülakattan çıkarıldınız.</p>";
                    text += "<p> Bilgilerinize. </p>";
                    text += "<p> {1} </p>";
                    var mesaj = string.Format(text, eskiuserr.firstname + " " + eskiuserr.lastname, (compLocation != null ? compLocation.fullName : ""), string.Format("{0:dd.MM.yyyy HH:mm}", plan.PlanDate));
                    new Email().Template("Template1", "demo.jpg", "İş Görüşmesi Planı Hakkında", mesaj)
                     .Send((Int16)EmailSendTypes.PersonelAdayMulakat, eskiuserr.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Hakkında.."), true);
                }
            }

            var cvSahibi = db.GetHR_PersonById(plan.HrPersonId.Value);
            foreach (var yeniperson in yenipersoneller)
            {
                var cont = gorusmedekieskipersoneller.Where(c => c == yeniperson);
                if (cont.Count() == 0)
                {
                    var yenipersons = db.GetSH_UserById(yeniperson.Value);

                    var texts = "<h3>Merhaba {0},</h3>";
                    texts += "<p>{1} adlı kişi ile {2} tarihinde mülakat yapılacaktır.Siz de görüşme planına eklendiniz.</p>";
                    texts += "<p> Bilgilerinize. </p>";
                    var mesaj = string.Format(texts, yenipersons.firstname + " " + yenipersons.lastname, cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", item.hrPlan.PlanDate));
                    new Email().Template("Template1", "demo.jpg", "Mülakat Planı Hakkında", mesaj)
                      .Send((Int16)EmailSendTypes.PersonelAdayMulakat, yenipersons.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Hakkında.."), true);
                }
            }

            if (plan.PlanDate != item.hrPlan.PlanDate)
            {
                if (compLocation != null)
                {
                    if (compLocation.location != null)
                    {
                        var textUser = "<h3>Merhaba {0},</h3>";
                        textUser += "<p>{1} tarihinde gerçekleştireceğiniz mülakat planında değişiklik olmuştur. Yeni görüşme planı detayları aşağıdaki gibidir;</p>";
                        textUser += "<b> {5} </b>";
                        textUser += "<p></p>";
                        textUser += "<b> İnsan Kaynakları Direktörlüğü </b>";
                        textUser += "<p></p>";
                        textUser += "<p>Tarih ve Saat : {2}</p>";
                        textUser += "<p>Adres : {6}</p>";
                        textUser += "<p>Mülakat konumuna gitmek için <a href='maps.google.com/maps/?q={3},{4}'> Buraya tıklayınız! </a></p>";
                        textUser += "<p> Bilgilerinize. </p>";
                        var mesaj = string.Format(textUser, cvSahibi.Name + " " + cvSahibi.SurName, string.Format("{0:dd.MM.yyyy HH:mm}", plan.PlanDate), string.Format("{0:dd.MM.yyyy HH:mm}", item.hrPlan.PlanDate), (compLocation != null ? compLocation.location.Coordinate.Y.ToString().Replace(",", ".") : ""), (compLocation != null ? compLocation.location.Coordinate.X.ToString().Replace(",", ".") : ""), (compLocation != null ? compLocation.fullName : ""), (compLocation != null ? compLocation.openAddress : ""));
                        new Email().Template("Template1", "demo.jpg", "Mülakat Zaman Planı Hakkında", mesaj)
                             .Send((Int16)EmailSendTypes.PersonelAdayMulakat, cvSahibi.Email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "Mülakat Zaman Planı Hakkında.."), true);
                    }
                }
            }

            var newUserList = new List<HR_PlanPerson>();
            var trans = db.BeginTransaction();

            foreach (var list in yenipersoneller)
            {
                if (list.HasValue)
                {
                    newUserList.Add(new HR_PlanPerson
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        HrPlanId = plan.id,
                        UserId = list
                    });

                    if (db.GetSH_UserRoleByUserId(list.Value).Where(x => x.roleid == new Guid(SHRoles.IsGorusmesiMulakat)).Count() == 0)
                    {
                        results &= db.InsertSH_UserRole(new SH_UserRole
                        {
                            roleid = new Guid(SHRoles.IsGorusmesiMulakat),
                            userid = list.Value,
                            createdby = userStatus.user.id
                        }, trans);
                    }
                }
            }

            results &= db.BulkDeleteHR_PlanPerson(planperson, trans);
            results &= db.BulkInsertHR_PlanPerson(newUserList, trans);
            results &= db.UpdateHR_Plan(new HR_Plan().B_EntityDataCopyForMaterial(item.hrPlan),false, trans);

            if (results.result == false)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Mülakat düzenleme işlemi başarısız oldu.")
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();
            return Json(new ResultStatusUI
            {
                Result = true,
                FeedBack = feedback.Success("Mülakat güncelleme işlemi başarı ile gerçekleşti.")
            }, JsonRequestBehavior.AllowGet);

        }

        [PageInfo("Mülakat Planlamaları Silme", SHRoles.IKYonetici)]
        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new HR_Plan { id = new Guid(a) });

            var dbresult = db.BulkDeleteHR_Plan(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Mülakat Planlamaları Personel Methodu", SHRoles.IKYonetici)]
        public JsonResult GetHrPlanPersonId(Guid PlanId)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWHR_PlanPersonByPlanIdAndResultNotGorusulmedi(PlanId);
            if (data == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new ResultStatusUI
            {
                Object = data.id,
                Result = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
