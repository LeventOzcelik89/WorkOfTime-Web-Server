using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.INV.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using static Infoline.WorkOfTime.WebProject.UIHelper.Utility;

namespace Infoline.WorkOfTime.WebProject.Areas.INV.Controllers
{
    public class VWINV_CompanyPersonAssessmentController : Controller
    {
        [PageInfo("İşe Giren Personel Değerlendirmeleri", SHRoles.Personel)]
        public ActionResult Index()
        {

            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var dateFunc = new DateFunctions();
            var assessments = db.GetVWINV_CompanyPersonAssessment();

            var compPersonDept = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(userStatus.user.id, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();
            var pageReport = new VWINV_CompanyPersonAssessmentPageReport
            {
                idUser = userStatus.user.id,

                ToplamDegerlendirilecekPersonelSayisi = assessments.Where(x => x.Manager1Approval == userStatus.user.id &&
                                                        x.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.Yonetici1Degerlendirme).Count(),

                IslemimdenGecmisDegerlendirmelerinSayisi = assessments.Where(x =>
                        (x.Manager1Approval == userStatus.user.id && x.Manager1ApprovalDate != null) ||
                        (x.Manager2Approval == userStatus.user.id && x.Manager2ApprovalDate != null) ||
                        (x.GeneralManagerApproval == userStatus.user.id && x.GeneralManagerApprovalDate != null)).Count(),

                OnayiminBeklendigiDegerlendirmeler = assessments.Where(x =>
                        (x.Manager2Approval == userStatus.user.id && x.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.Yonetici2Degerlendirme) ||
                        (x.GeneralManagerApproval == userStatus.user.id && x.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.GenelMudurDegerlendirme)).Count(),

                DegerlendirmeleriminSayisi = assessments.Where(x => x.Manager1Approval == userStatus.user.id &&
                                            x.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.Yonetici1Degerlendirme).Count() + assessments.Where(x =>
                          (x.Manager1Approval == userStatus.user.id && x.Manager1ApprovalDate != null) ||
                          (x.Manager2Approval == userStatus.user.id && x.Manager2ApprovalDate != null) ||
                          (x.GeneralManagerApproval == userStatus.user.id && x.GeneralManagerApprovalDate != null)).Count(),

                IslakImzaBeklenenDegerlendirmeler = assessments.Where(x => x.IkApproval == userStatus.user.id && x.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.IkDegerlendirme).Count(),

                SureciTamamlananDegerlendirmeler = assessments.Where(x => x.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.SurecTamamlandi).Count(),

                OnaySurecindekiDegerlendirmeler = assessments.Where(x => x.ApproveStatus != (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.SurecTamamlandi).Count(),

                ToplamDegerlendirmelerinSayisi = assessments.Count(),
            };
            return View(pageReport);
        }


        [PageInfo("Tüm Değerlendirmeler Methodu ", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonAssessment(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWINV_CompanyPersonAssessmentCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Değerlendirme Adet Methodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWINV_CompanyPersonAssessmentCount(condition.Filter);
            return count;
        }


        [PageInfo("Tüm Değerlendirmeler Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonAssessment(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Tüm Değerlendirmeler Detayı", SHRoles.Personel)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonAssessmentById(id);
            var userStatus = (PageSecurity)Session["userStatus"];
            var personel = db.GetVWSH_UserById(data.IdUser.Value);


            if (personel != null)
            {
                var companyPerson = db.GetVWINV_CompanyPersonFirstByUserId(personel.id) ?? new VWINV_CompanyPerson();

                var compPersonDept = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(personel.id, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();
                var model = new VMVWINV_CompanyPersonAssessment().B_EntityDataCopyForMaterial(data);

                var ratingData = db.GetINV_CompanyPersonAssessmentRatingByAssessmentId(data.id);

                List<INV_CompanyPersonAssessmentRating> rating = new List<INV_CompanyPersonAssessmentRating>();

                foreach (var q in ratingData)
                {
                    var objRating = new INV_CompanyPersonAssessmentRating();
                    objRating.question = q.question;
                    objRating.answer = q.answer;
                    rating.Add(objRating);
                }

                model.task = compPersonDept.Title;
                model.department = compPersonDept.Department_Title;
                model.jobStartDate = companyPerson.JobStartDate;
                model.Rating = rating.ToArray();

                return View(model);
            }

            else
            {
                new FeedBack().Warning("Personel bulunamadı.", true);
                Response.Redirect(Request.UrlReferrer.ToString());
                return null;
            }


        }


        [PageInfo("Değerlendirme Ekleme", SHRoles.Personel)]
        public ActionResult Insert()
        {
            var data = new VWINV_CompanyPersonAssessment { id = Guid.NewGuid() };
            return View(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Değerlendirme Ekleme", SHRoles.Personel)]
        public JsonResult Insert(INV_CompanyPersonAssessment item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            var dbresult = db.InsertINV_CompanyPersonAssessment(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Değerlendirme Güncelleme", SHRoles.Personel)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonAssessmentById(id);
            var userStatus = (PageSecurity)Session["userStatus"];
            var model = new VMVWINV_CompanyPersonAssessment().B_EntityDataCopyForMaterial(data);

            if (model != null)
            {
                var personel = db.GetVWSH_UserById(data.IdUser.Value);
                var ratingData = db.GetINV_CompanyPersonAssessmentRatingByAssessmentId(data.id);
                var compPersonDept = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(personel.id, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();

                var companyPerson = db.GetVWINV_CompanyPersonFirstByUserId(personel.id);

                if (ratingData.Length > 0)
                {
                    List<INV_CompanyPersonAssessmentRating> rating = new List<INV_CompanyPersonAssessmentRating>();

                    foreach (var q in ratingData)
                    {
                        var objRating = new INV_CompanyPersonAssessmentRating();
                        objRating.question = q.question;
                        objRating.answer = q.answer;
                        rating.Add(objRating);
                    }

                    model.Rating = rating.ToArray();
                }
                else
                {
                    var assessmentModel = new INV_CompanyPersonAssessmentModel();
                    var questions = assessmentModel.QuestionsByAssessmentType[(int)data.AssessmentType];

                    List<INV_CompanyPersonAssessmentRating> rating = new List<INV_CompanyPersonAssessmentRating>();

                    foreach (var q in questions)
                    {
                        var objRating = new INV_CompanyPersonAssessmentRating();
                        objRating.question = q;
                        objRating.answer = Convert.ToInt32(EnumINV_CompanyPersonAssessmentAnswer.Basarili);
                        rating.Add(objRating);
                    }

                    model.Rating = rating.ToArray();
                }

                model.task = compPersonDept.Title;
                model.department = compPersonDept.Department_Title;
                model.jobStartDate = companyPerson.JobStartDate;
            }

            return View(model);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Değerlendirme Güncelleme", SHRoles.Personel)]
        public JsonResult Update(VMVWINV_CompanyPersonAssessment item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var assessment = db.GetINV_CompanyPersonAssessmentById(item.id);


            if (assessment.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.IkDegerlendirme && !userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.IKYonetici)))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("İk personeli değilseniz bu işlemi gerçekleştiremezsiniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            if (assessment.Manager1Approval == userStatus.user.id)
            {
                if (userStatus.user.Manager1.HasValue)
                {
                    assessment.Manager2Approval = userStatus.user.Manager1;
                }
            }

            var compPersonDept = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(item.IdUser.Value, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();
            var user = db.GetVWSH_UserById(assessment.IdUser.Value);

            var IkManager = db.GetSH_UserByRoleId(SHRoles.IKYonetici).OrderBy(a => a.CompanyId == user.CompanyId).FirstOrDefault();
            var generalManager = db.GetSH_UserByRoleId(SHRoles.IdariPersonelYonetici).OrderBy(a => a.CompanyId == user.CompanyId).FirstOrDefault();


            if (assessment.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.Yonetici2Degerlendirme && !assessment.Manager2Approval.HasValue)
            {
                assessment.ApproveStatus = (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.GenelMudurDegerlendirme;
            }

            if (assessment.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.GenelMudurDegerlendirme && generalManager == null)
            {
                assessment.ApproveStatus = (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.IkDegerlendirme;
            }

            if (assessment.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.IkDegerlendirme && IkManager == null)
            {
                assessment.ApproveStatus = (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.SurecTamamlandi;
            }

            int control = 0;
            assessment.AssessmentDate = DateTime.Now;
            var dbres = new ResultStatus { result = true };
            var tenantName = TenantConfig.Tenant.TenantName;
            var trans = db.BeginTransaction();
            if (userStatus.user.id == assessment.Manager1Approval && assessment.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.Yonetici1Degerlendirme)
            {
                control++;
                assessment.changed = DateTime.Now;
                assessment.changedby = userStatus.user.id;
                assessment.Manager1Approval = compPersonDept.Manager1;
                assessment.Manager1ApprovalDate = DateTime.Now;
                assessment.Manager1ApprovalComment = item.Manager1ApprovalComment;
                assessment.ApproveStatus = Convert.ToInt32(EnumINV_CompanyPersonAssessmentApproveStatus.Yonetici2Degerlendirme);
                assessment.ConfirmStatus = item.ConfirmStatus;

                foreach (var rating in item.Rating)
                {
                    rating.created = DateTime.Now;
                    rating.createdby = userStatus.user.id;
                    rating.assessmentId = assessment.id;
                    dbres &= db.InsertINV_CompanyPersonAssessmentRating(rating, trans);
                }

                if (assessment.Manager2Approval.HasValue)
                {
                    var manager2 = db.GetSH_UserById(assessment.Manager2Approval.Value);
                    var mesaj = string.Format(@"<p><strong>{0}</strong><br/> Adlı Personelin {1} aylık Performans Derğerlendirme Formunu 1. Yönetici tamamladı. Onayınız bekleniyor.</p>",
                                    compPersonDept.Person_Title, assessment.AssessmentType);
                    new Email().Template("Template1", "degerlendirme.jpg", tenantName + " WORK OF TIME PERFORMANS DEĞERLENDİRME FORMU UYARISI", mesaj)
                      .Send((Int16)EmailSendTypes.Degerlendirme, manager2.email, string.Format("{0} | {1}", tenantName +" | WORKOFTIME", "PERFORMANS DEĞERLENDİRME FORMU"), true);
                }

            }

            if (userStatus.user.id == assessment.Manager2Approval && assessment.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.Yonetici2Degerlendirme)
            {
                assessment.changed = DateTime.Now;
                assessment.changedby = userStatus.user.id;
                assessment.Manager2Approval = compPersonDept.Manager2;
                assessment.Manager2ApprovalDate = DateTime.Now;
                assessment.Manager2ApprovalComment = item.Manager2ApprovalComment;
                assessment.ApproveStatus = Convert.ToInt32(EnumINV_CompanyPersonAssessmentApproveStatus.GenelMudurDegerlendirme);

                if (generalManager != null)
                {
                    assessment.GeneralManagerApproval = generalManager.id;
                    if (compPersonDept.Manager2 != generalManager.id)
                    {
                        var mudur = db.GetSH_UserById(assessment.GeneralManagerApproval.Value);
                        var mesaj = string.Format(@"<p><strong>{0}</strong><br/> Adlı Personelin {1} aylık Performans Derğerlendirme Formunu 2. Yönetici tamamladı. Onayınız bekleniyor.</p>",
                                        compPersonDept.Person_Title, assessment.AssessmentType);
                        new Email().Template("Template1", "degerlendirme.jpg", tenantName + " WORK OF TIME PERFORMANS DEĞERLENDİRME FORMU UYARISI", mesaj)
                          .Send((Int16)EmailSendTypes.Degerlendirme, mudur.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "PERFORMANS DEĞERLENDİRME FORMU"), true);
                    }
                }
            }
            if (generalManager != null)
            {
                if (userStatus.user.id == generalManager.id && assessment.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.GenelMudurDegerlendirme)
                {
                    assessment.GeneralManagerApproval = generalManager.id;
                    assessment.GeneralManagerApprovalDate = DateTime.Now;
                    assessment.GeneralManagerApprovalComment = compPersonDept.Manager2 == generalManager.id ? item.Manager2ApprovalComment : item.GeneralManagerApprovalComment;
                    assessment.ApproveStatus = Convert.ToInt32(EnumINV_CompanyPersonAssessmentApproveStatus.IkDegerlendirme);

                    var mesaj = string.Format(@"<p><strong>{0}</strong><br/> Adlı Personelin {1} aylık Performans Derğerlendirme Formunu Genel Müdür tamamladı. Onayınız bekleniyor.</p>",
                                   compPersonDept.Person_Title, assessment.AssessmentType);
                    if (IkManager != null)
                    {
                        new Email().Template("Template1", "degerlendirme.jpg", tenantName + " WORK OF TIME PERFORMANS DEĞERLENDİRME FORMU UYARISI", mesaj)
                                   .Send((Int16)EmailSendTypes.Degerlendirme, IkManager.email, string.Format("{0} | {1}", tenantName + " | WORKOFTIME", "PERFORMANS DEĞERLENDİRME FORMU"), true);
                    }
                }
            }

            if (IkManager != null)
            {
                if ((userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.IKYonetici))) && assessment.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.IkDegerlendirme && control == 0)
                {
                    var rsFile = new FileUploadSave(Request).SaveAs();

                    assessment.IkApproval = userStatus.user.id;
                    assessment.IkApprovalDate = DateTime.Now;
                    assessment.IKApprovalComment = item.IKApprovalComment;
                    assessment.ApproveStatus = Convert.ToInt32(EnumINV_CompanyPersonAssessmentApproveStatus.SurecTamamlandi);
                }
            }

            dbres &= db.UpdateINV_CompanyPersonAssessment(assessment, true, trans);
            if (!dbres.result)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error("Değerlendirme işlemi başarısız.")
                }, JsonRequestBehavior.AllowGet);
            }

            trans.Commit();
            return Json(new ResultStatusUI
            {
                Result = dbres.result,
                FeedBack = feedback.Success("Değerlendirme işlemi tamamlandı",false,Url.Action("Index", "VWINV_CompanyPersonAssessment", new { area = "INV" }))
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Değerlendirme Silme", SHRoles.IKYonetici, SHRoles.PersonelTalebi, SHRoles.IdariPersonelYonetici)]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new INV_CompanyPersonAssessment { id = new Guid(a) });

            var dbresult = db.BulkDeleteINV_CompanyPersonAssessment(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Onaydan Sonra Oluşan Form", SHRoles.IKYonetici)]
        public ActionResult AssessmentForm(Guid id)
        {
            var feedback = new FeedBack();
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWINV_CompanyPersonAssessmentById(id);
            var userStatus = (PageSecurity)Session["userStatus"];

            var compPersonDept = db.GetVWINV_CompanyPersonDepartmentsByIdUserAndType(data.IdUser.Value, EnumINV_CompanyDepartmentsType.Organization).FirstOrDefault() ?? new VWINV_CompanyPersonDepartments();

            var companyPerson = db.GetVWINV_CompanyPersonFirstByUserId(data.IdUser.Value);
            var ratingData = db.GetINV_CompanyPersonAssessmentRatingByAssessmentId(data.id);

            List<INV_CompanyPersonAssessmentRating> rating = new List<INV_CompanyPersonAssessmentRating>();

            foreach (var q in ratingData)
            {
                var objRating = new INV_CompanyPersonAssessmentRating();
                objRating.question = q.question;
                objRating.answer = q.answer;
                rating.Add(objRating);
            }

            if (data == null)
            {
                return Json(new ResultStatusUI
                { Result = false, FeedBack = feedback.Warning("Personel Bilgilerine Ulaşılamadı") }, JsonRequestBehavior.AllowGet);
            }

            var model = new VMVWINV_CompanyPersonAssessment
            {
                Owner = db.GetVWSH_UserById(data.IdUser.Value),
                Url = TenantConfig.Tenant.GetWebUrl() + "/INV/VWINV_CompanyPersonAssessment/Detail?id=" + data.id

            }.EntityDataCopyForMaterial(data, true);

            model.department = compPersonDept.Title;
            model.task = compPersonDept.Department_Title;
            model.jobStartDate = companyPerson.JobStartDate;
            model.Rating = rating.ToArray();
            return View(model);
        }
    }
}

