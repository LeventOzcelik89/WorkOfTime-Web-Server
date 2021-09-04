using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PDS.Controllers
{
	public class VWPDS_FormResultController : Controller
    {
        [PageInfo("Form Sonuçları", SHRoles.SistemYonetici,SHRoles.IdariPersonelYonetici)]
        public ActionResult Index(Guid id)
        {
            var db = new WorkOfTimeDatabase();

            var data = db.GetVWPDS_FormPermitTaskUserById(id);

            var model = new VMVWPDS_FormPermitTaskUserDetail().EntityDataCopyForMaterial(data);

            if (data.evaluateFormId != null && data.evaluatedUserId != null && data.evaluatorId != null)
            {
                var vm = db.GetVWPDS_FormResultByEvaluatorIdEvaluatedUserIdAndFormId(data.evaluatorId.Value, data.evaluatedUserId.Value, data.evaluateFormId.Value);
                model.OrtalamaPuan = vm.Count() == 0 ? 0 : vm.Average(a => a.score.Value);
                model.EnDusukPuan = vm.OrderBy(a => a.score).Select(a => a.score.Value).FirstOrDefault();
                model.EnYuksekPuan = vm.OrderByDescending(a => a.score).Select(a => a.score.Value).FirstOrDefault();
                model.BuAyYaptigimDegerlendirmeSayisi = vm.Where(s => s.created.Value.Month == DateTime.Now.Month && s.created.Value.Year == DateTime.Now.Year).Count();
            }
            return View(model);
        }

        [PageInfo("Tüm Değerlendirmeler", SHRoles.IKYonetici)]
        public ActionResult AllIndex()
        {
            var db = new WorkOfTimeDatabase();
            var seriesColor = new[] { "#ed5565", "#1ab394", "#1c84c6", "#4682b4" }.ToArray();
            var seriesIcons = new[] { "fa text-success fa-bars", "fa text-success fa-area-chart", "fa text-success fa-line-chart", "fa text-success fa-bullhorn" }.ToArray();
            var data = db.GetPDS_EvaluateForm().Where(y => y.status == true).Select(x => new KeyValue { Key = x.formName, Value = seriesIcons[x.formType.HasValue ? x.formType.Value : 1], Color = seriesColor[x.formType.HasValue ? x.formType.Value : 1] }).ToArray();
            return View(data);
        }

        [PageInfo("Form Sonucu Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_FormResult(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPDS_FormResultCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Form Sonucu Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_FormResult(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Form Sonucu Veri Methodu", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWPDS_FormResultCount(condition.Filter);
            return count;
        }

        [PageInfo("Form Sonucu Ekleme", SHRoles.Personel)]
        public ActionResult Insert(VMPDS_FormResultModel item)
        {
            item.Load();
            return View(item);
        }

        [HttpPost]
        [PageInfo("Form Sonucu Ekleme Metodu", SHRoles.Personel)]
        public JsonResult Insert(VMPDS_FormResultModel item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbres = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbres.result,
                FeedBack = dbres.result == true ? feedback.Success("Değerlendirme işlemi başarılı.") : feedback.Warning("Değerlendirme işlemi başarısız. Mesaj : " + dbres.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Form Sonucu Güncelleme", SHRoles.Personel)]
        public ActionResult Update(Guid id)
        {
            var item = new VMPDS_FormResultModel { id = id }.Load();
            return View(item);
        }

        [HttpPost]
        [PageInfo("Form Sonucu Güncelleme Metodu", SHRoles.Personel)]
        public JsonResult Update(VMPDS_FormResultModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbres = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbres.result,
                FeedBack = dbres.result == true ? feedback.Success("Değerlendirme güncelleme işlemi başarılı.") : feedback.Warning("Değerlendirme güncelleme işlemi başarısız. Mesaj : " + dbres.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Form Sonucu Detayı", SHRoles.Personel)]
        public ActionResult Detail(Guid id)
        {
            var item = new VMPDS_FormResultModel { id = id }.Load();
            return View(item);
        }

        [PageInfo("Personel Performans Raporları", SHRoles.IKYonetici)]
        public ActionResult PersonalPerformanceReport(Guid id, bool? detail)
        {
            var db = new WorkOfTimeDatabase();

            var user = db.GetVWSH_UserById(id);


            var model = new VWPDS_PersonalPerformanceReport();

            if (user == null)
            {
                return View(model);
            }


            model.PersonelName = user.FullName;
            model.PersonId = id;
            model.ProfilePhoto = user.ProfilePhoto;
            model.PersonTitle = user.CompanyPerson_Title;
            model.Company = user.Company_Title;
            model.detail = detail.HasValue ? true : false;
            return View(model);

        }

        [PageInfo("Performans Değerlendirmesi Raporları", SHRoles.IKYonetici)]
        public ActionResult PerformanceEvaluateReport()
        {
            return View();
        }

        [HttpPost]
        [PageInfo("Performans Raporları Filtresi", SHRoles.IKYonetici)]
        public ContentResult PerformanceReportFilter(VWPDS_PerformanceEvaluateReport item)
        {

            var db = new WorkOfTimeDatabase();

            Guid[] taskIds = null;
            if (item.TaskIds != null)
            {
                taskIds = db.GetVWPDS_FormPermitTaskUserByTaskIds(item.TaskIds).Select(a => a.id).ToArray();
            }

            var data = db.GetVWPDS_FormResultByReport(item.EvaluatorIds, item.EvaluatedIds, item.FormIds, taskIds, item.Period, item.performanceStartDate, item.performanceEndDate).Where(a => a.formType == (Int32)EnumPDS_EvaluateFormType.Performance).ToArray();

            if (data.Length == 0)
            {
                var feedback = new FeedBack();
                var result = new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Seçilen formlarda personelin değerlendirmesi bulunamadı.")
                };

                return Content(Infoline.Helper.Json.Serialize(result), "application/json");
            }

            var formResultIds = data.Select(a => a.id).ToArray();
            var questionResults = db.GetVWPDS_QuestionResultByFormResultIds(formResultIds);

            if (item.Questions != null)
            {
                questionResults = questionResults.Where(a => item.Questions.Contains(a.question)).ToArray();
            }

            if (item.GroupNames != null)
            {
                questionResults = questionResults.Where(a => item.GroupNames.Contains(a.groupName)).ToArray();
            }

            var FormResultData = data.Select(a => new VWPDS_FormResultCustom
            {
                QuestionResult = questionResults.Where(c => c.formResultId == a.id).ToArray()
            }.EntityDataCopyForMaterial(a, true)).ToArray();

            if (item.filterType == "general")
            {
                return Content(Infoline.Helper.Json.Serialize(""), "application/json");
            }

            else
            {
                var generalCardsData = new
                {
                    count = FormResultData.Count(),
                    avg = FormResultData.Average(a => a.score),
                    minPoint = FormResultData.OrderBy(a => a.score).Select(a => new
                    {
                        title = a.created.Value.Date,
                        min = a.score,
                        id = a.id
                    }).FirstOrDefault(),
                    maxPoint = FormResultData.OrderByDescending(a => a.score).Select(a => new
                    {
                        title = a.created.Value.Date,
                        max = a.score,
                        id = a.id
                    }).FirstOrDefault(),
                };

                var formChartData = FormResultData.GroupBy(a => a.created.Value.Date).Select(s => new
                {
                    key = s.Key,
                    avg = s.Average(c => c.score),
                    min = s.Min(c => c.score),
                    max = s.Max(c => c.score),
                    formGrouping = s.GroupBy(c => c.formName).Select(a => new
                    {
                        name = a.Key,
                        score = a.Average(g => g.score),
                    }),
                    evalatorGrouping = s.GroupBy(c => c.evaluator_Title).Select(a => new
                    {
                        name = a.Key,
                        score = a.Average(g => g.score),
                    }),
                }).OrderBy(c => c.key).ToArray();


                var chartByFormName = FormResultData.GroupBy(a => a.formName).Select(a => new
                {
                    title = a.Key,
                    Data = a.GroupBy(b => b.created.Value.Date).Select(s => new
                    {
                        title = s.Key,
                        avg = s.Average(d => d.score),
                        min = s.Min(c => c.score),
                        max = s.Max(c => c.score),
                        groupName = s.SelectMany(c => c.QuestionResult).Where(c => c.score != 0).GroupBy(g => g.groupName).Select(sc => new
                        {
                            title = sc.Key,
                            avg = sc.Average(c => c.percentScore),
                            min = sc.Min(c => c.percentScore),
                            max = sc.Max(c => c.percentScore),
                            data = sc.GroupBy(c => c.question).Select(x => new
                            {
                                title = x.Key,
                                avg = x.Average(c => c.percentScore),
                                min = x.Min(c => c.percentScore),
                                max = x.Max(c => c.percentScore),
                            }),
                        }).ToArray(),
                        question = s.SelectMany(c => c.QuestionResult).Where(c => c.score != 0).GroupBy(g => g.question).Select(sc => new
                        {
                            title = sc.Key,
                            avg = sc.Average(c => c.percentScore),
                            min = sc.Min(c => c.percentScore),
                            max = sc.Max(c => c.percentScore),
                        })
                    }).OrderBy(c => c.title).ToArray()
                }).ToArray();

                var dataByFormAndGroup = FormResultData.GroupBy(a => a.formName).Select(a => new
                {
                    title = a.Key,
                    avg = a.Average(s => s.score),
                    min = a.Min(s => s.score),
                    max = a.Max(s => s.score),
                    group = a.SelectMany(c => c.QuestionResult).Where(c => c.score != 0).GroupBy(g => g.groupName).Select(s => new
                    {
                        title = s.Key,
                        avg = s.Average(c => c.percentScore),
                        min = s.Min(c => c.percentScore),
                        max = s.Max(c => c.percentScore),
                        data = s.GroupBy(t => t.question).Select(x => new
                        {
                            title = x.Key,
                            avg = x.Average(c => c.percentScore),
                            min = x.Min(c => c.percentScore),
                            max = x.Max(c => c.percentScore),
                        }).ToArray()
                    }).ToArray()
                });


                var resultGroupSelect = item.GroupNames != null ? FormResultData.SelectMany(a => a.QuestionResult).Where(c => c.score != 0)
                            .Where(c => string.Join(",", item.GroupNames).Contains(c.groupName))
                            .GroupBy(b => b.created.Value.Date).Select(d => new
                            {
                                title = d.Key,
                                avg = d.Average(c => c.percentScore),
                                min = d.Min(c => c.percentScore),
                                max = d.Max(c => c.percentScore),
                            }).OrderBy(q => q.title).ToArray() : null;

                var resultGroup = item.GroupNames != null ? FormResultData.SelectMany(a => a.QuestionResult).Where(c => c.score != 0)
                            .Where(c => string.Join(",", item.GroupNames).Contains(c.groupName))
                            .GroupBy(s => s.groupName).Select(sc => new
                            {
                                title = sc.Key,
                                avg = sc.Average(c => c.percentScore),
                                min = sc.Min(c => c.percentScore),
                                max = sc.Max(c => c.percentScore),
                                data = sc.ToArray().GroupBy(d => d.created.Value.Date).Select(x => new
                                {
                                    title = x.Key,
                                    avg = x.Average(c => c.percentScore),
                                    min = x.Min(c => c.percentScore),
                                    max = x.Max(c => c.percentScore),
                                    data = x.ToArray().GroupBy(z => z.question).Select(s => new
                                    {
                                        title = s.Key,
                                        avg = s.Average(c => c.percentScore),
                                        min = s.Min(c => c.percentScore),
                                        max = s.Max(c => c.percentScore),
                                    }),
                                }).OrderBy(x => x.title).ToArray(),
                            }).ToArray() : null;

                var questions = item.Questions != null ?
                    FormResultData.SelectMany(a => a.QuestionResult).Where(c => c.score != 0)
                    .GroupBy(a => a.created.Value.Date).Select(b => new
                    {
                        title = b.Key,
                        avg = b.Average(c => c.percentScore),
                        min = b.Min(c => c.percentScore),
                        max = b.Max(c => c.percentScore),
                        data = b.GroupBy(c => c.question).Select(s => new
                        {
                            title = s.Key,
                            avg = s.Average(c => c.percentScore),
                            min = s.Min(c => c.percentScore),
                            max = s.Max(c => c.percentScore),
                        })
                    }).OrderBy(x => x.title).ToArray() : null;


                var model = new
                {
                    GeneralCards = generalCardsData,
                    GeneralCharts = formChartData,
                    FormGroupData = dataByFormAndGroup,
                    FormResultData = resultGroup,
                    DataByFormAndGroupName = chartByFormName,
                    FormResultGroup = resultGroupSelect,
                    DataByQuestions = questions
                };


                return Content(Infoline.Helper.Json.Serialize(model), "application/json");
            }
        }

        [HttpPost]
        [PageInfo("Grup İsimleri Formu", SHRoles.IKYonetici)]
        public JsonResult GetGroupNamesByFormId(Guid formId)
        {
            var db = new WorkOfTimeDatabase();
            var result = db.GetVWPDS_QuestionFormByFormId(formId).GroupBy(a => a.groupName).Select(a => new { Key = a.Key, Id = a.Key }).ToArray();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
