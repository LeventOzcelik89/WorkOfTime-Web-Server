using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using static Infoline.WorkOfTime.BusinessAccess.VMFTM_TaskModel;
namespace Infoline.WorkOfTime.WebProject.Areas.FTM.Controllers
{
    public class VWFTM_TaskController : Controller
    {
        [PageInfo("Görevler", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli)]
        public ActionResult Index(Guid? userId, string ids)
        {

            var userStatus = (PageSecurity)Session["userStatus"];

            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.BayiGorevPersoneli)))
            {
                return PartialView("~/Areas/FTM/Views/VWFTM_Task/IndexDealer.cshtml");
            }

            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)) ||
                userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) ||
                userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SistemYonetici)) ||
                userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevMusteri)))
            {

                if (userId.HasValue)
                {
                    /*Aktivite izleme sayfasında bulunan çağrı operatör işlem istatiğinde
					 Operatorlerin oluşturduğu görevleri görmesi için.*/
                    ViewBag.userId = userId;
                }

                var _ids = new List<Guid>();
                if (!String.IsNullOrEmpty(ids))
                {
                    _ids = ids.Split(',').Select(a => new Guid(a)).ToList();
                }
                if (ids == null && userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevMusteri)))
                {
                    //return RedirectToAction("");
                }

                return PartialView("~/Areas/FTM/Views/VWFTM_Task/IndexAll.cshtml", _ids);

            }

            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.YukleniciPersoneli)))
            {
                return PartialView("~/Areas/FTM/Views/VWFTM_Task/IndexYuklenici.cshtml");
            }
            else
            {
                return PartialView("~/Areas/FTM/Views/VWFTM_Task/IndexMy.cshtml");
            }

        }

        [PageInfo("Müşteri Arıza&Bakım Kayıtları", SHRoles.SahaGorevMusteri)]
        public ActionResult IndexCustomer()
        {
            return View();
        }

        [PageInfo("Personel İş Takip", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ActionResult PersonDailyReport()
        {
            return View();
        }


        [PageInfo("Aylık Görev Raporu", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ActionResult MonthlyTaskReport()
        {
            return View();
        }

        [PageInfo("Personel Ay/Yıl Bazlı Çağrı Raporu", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ActionResult MonthlyPersonelReport()
        {
            return View();
        }

        [PageInfo("Personel Ay/Yıl Bazlı Çağrı Raporu DataSource", SHRoles.Personel)]
        public ContentResult MonthlyPersonelReportData(int year, string[] months, List<Guid?> assignableUsers, Guid? customer, Guid? customerStorage)
        {

            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var task = new List<VWFTM_Task>();
            var taskOperation = new List<VWFTM_TaskOperation>();
            var monthString = "";
            if (months.Count() > 0 && months.Where(x => String.IsNullOrEmpty(x)).Count() > 0)
            {
                string[] strArray = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                months = strArray;
                monthString = String.Join(",", strArray.ToArray());
            }
            else
            {
                monthString = String.Join(",", months.ToArray());
            }
            var taskMonthReportData = new List<MonthTaskReportModel>();

            if (assignableUsers != null && assignableUsers.Count(x => x.HasValue) > 0)
            {
                foreach (var user in assignableUsers)
                {
                    taskMonthReportData.AddRange(db.GetVWFTM_MonthPersonnelTaskReport(year, monthString).Where(x => x.assignUserId == Guid.Parse(user.ToString())).ToList());
                }
            }
            else
            {
                taskMonthReportData = db.GetVWFTM_MonthPersonnelTaskReport(year, monthString).ToList();
            }

            if (customer.HasValue)
            {
                taskMonthReportData = taskMonthReportData.Where(x => x.customerId == customer.Value).ToList();
            }

            if (customerStorage.HasValue)
            {
                taskMonthReportData = taskMonthReportData.Where(x => x.customerStorageId == customerStorage.Value).ToList();
            }

            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
            {
                var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
                if (authoritys.Count() > 0)
                    taskMonthReportData = taskMonthReportData.Where(x => authoritys.Where(f => f.customerId.HasValue).Select(f => f.customerId.Value).ToArray().Contains(x.customerId)).ToList();

            }

            var personels = taskMonthReportData.GroupBy(a => a.personnelName).Select(a => a.Key).ToArray();
            var monthms = taskMonthReportData.GroupBy(a => a.month).Select(a => a.Key).ToArray();
            var resultData = new List<MonthlyPersonData>();
            var res = new List<object>();

            List<int> monthList = months.Select(s => int.Parse(s)).ToList();


            var rDict = new List<Dictionary<string, object>>();

            if (personels.Count() <= 0 && assignableUsers.Where(x => x.HasValue).Count() <= 0)
            {
                personels = db.GetVWSH_UserByRoleId(SHRoles.SahaGorevPersonel).Select(x => x.FullName).ToArray();
            }
            else if (personels.Count() <= 0 && assignableUsers.Where(x => x.HasValue).Count() > 0)
            {
                personels = db.GetVWSH_UserByIds(assignableUsers.Where(x => x.HasValue).Select(x => x.Value).ToArray()).Select(x => x.FullName).ToArray();
            }


            foreach (var personel in personels)
            {
                var personGroup = "";
                var data = taskMonthReportData.Where(a => a.personnelName == personel);
                var dict = new Dictionary<string, object>();

                if (!String.IsNullOrEmpty(data.Select(x => x.groupId_Title).FirstOrDefault()))
                {
                    personGroup = data.Select(x => x.groupId_Title).FirstOrDefault();
                }
                else
                {
                    personGroup = "Ekibi Olmayanlar";
                }


                dict.Add("Name", personel);
                dict.Add("Group_Title", personGroup);

                foreach (var month in monthList)
                {
                    var t = taskMonthReportData.Where(a => a.personnelName == personel && a.month == month && a.month > 0);

                    var nDict = new Dictionary<string, object>();
                    nDict.Add("Month", month);
                    if (t.Count() > 0 && t.Average(a => a.resolutionTime) != 0 && t.Average(a => a.responseTime) != 0)
                    {
                        nDict.Add("Reso", t.Count() == 0 ? "0" : (t.Average(a => a.resolutionTime)).ToString("N0"));
                        nDict.Add("Resp", t.Count() == 0 ? "0" : (t.Average(a => a.responseTime)).ToString("N0"));
                        nDict.Add("taskIds", string.Join(",", t.Select(x => x.taskId).ToList()));
                    }
                    else
                    {
                        nDict.Add("Reso", "0");
                        nDict.Add("Resp", "0");
                    }
                    nDict.Add("Count", t.Count());

                    dict.Add("M" + month, nDict);
                }

                rDict.Add(dict);

            }

            return Content(Helper.Json.Serialize(rDict), "application/json");

        }


        [PageInfo("Çağrı Tipi Ay/Yıl Bazlı Rapor", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ActionResult MonthlyTypeReport()
        {
            return View();
        }

        [PageInfo("Çağrı Tipi Ay/Yıl Bazlı Rapor DataSource", SHRoles.Personel)]
        public ContentResult MonthlyTypeReportData(int year, string[] months, string[] taskTypes, Guid? customer, Guid? customerStorage)
        {

            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var task = new List<VWFTM_Task>();
            var taskOperation = new List<VWFTM_TaskOperation>();


            var monthString = "";
            if (months.Count() > 0 && months.Where(x => String.IsNullOrEmpty(x)).Count() > 0)
            {
                string[] strArray = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                months = strArray;
                monthString = String.Join(",", strArray.ToArray());
            }
            else
            {
                monthString = String.Join(",", months.ToArray());
            }
            var taskMonthReportData = new List<MonthTaskReportModel>();

            if (taskTypes != null && taskTypes.Where(x => !String.IsNullOrEmpty(x)).Count() > 0)
            {
                foreach (var taskTypeTitle in taskTypes)
                {
                    taskMonthReportData.AddRange(db.GetVWFTM_MonthTaskSubjectReport(year, monthString).Where(x => x.taskType_Title == taskTypeTitle).ToList());
                }
            }
            else
            {
                taskMonthReportData = db.GetVWFTM_MonthTaskSubjectReport(year, monthString).ToList();
            }

            if (customer.HasValue)
            {
                taskMonthReportData = taskMonthReportData.Where(x => x.customerId == customer.Value).ToList();
            }

            if (customerStorage.HasValue)
            {
                taskMonthReportData = taskMonthReportData.Where(x => x.customerStorageId == customerStorage.Value).ToList();
            }

            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
            {
                var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
                if (authoritys.Count() > 0)
                    taskMonthReportData = taskMonthReportData.Where(x => authoritys.Where(f => f.customerId.HasValue).Select(f => f.customerId.Value).ToArray().Contains(x.customerId)).ToList();
            }

            var taskType_Title = taskMonthReportData.GroupBy(a => a.taskType_Title).Select(a => a.Key).ToArray();

            var res = new List<object>();

            List<int> monthList = months.Select(s => int.Parse(s)).ToList();

            var rDict = new List<Dictionary<string, object>>();


            if (taskType_Title.Count() <= 0 && taskTypes.Where(x => !String.IsNullOrEmpty(x)).Count() <= 0)
            {
                taskType_Title = EnumsProperties.EnumToArrayValues<EnumFTM_TaskType>().Select(c => c.Value).ToArray();
            }
            else if (taskType_Title.Count() <= 0 && taskTypes.Where(x => !String.IsNullOrEmpty(x)).Count() > 0)
            {
                var taskType = EnumsProperties.EnumToArrayValues<EnumFTM_TaskType>().Select(c => new { Id = c.Key, Name = c.Value }).OrderBy(a => a.Name).ToList();

                var typeString = new List<String>();
                foreach (var typeTitle in taskTypes)
                {
                    if (taskType.Where(x => x.Name == typeTitle).FirstOrDefault() != null)
                    {
                        typeString.Add(typeTitle);
                    }
                }
                taskType_Title = typeString.ToArray();
            }

            foreach (var taskTypeTitle in taskType_Title)
            {

                var data = taskMonthReportData.Where(a => a.taskType_Title == taskTypeTitle);
                var dict = new Dictionary<string, object>();

                dict.Add("taskTypeTitle", taskTypeTitle);


                foreach (var month in monthList)
                {
                    var t = taskMonthReportData.Where(a => a.taskType_Title == taskTypeTitle && a.month == month && a.month > 0);

                    var nDict = new Dictionary<string, object>();
                    nDict.Add("Month", month);
                    if (t.Count() > 0 && t.Average(a => a.resolutionTime) != 0 && t.Average(a => a.responseTime) != 0)
                    {
                        nDict.Add("Reso", t.Count() == 0 ? "0" : (t.Average(a => a.resolutionTime)).ToString("N0"));
                        nDict.Add("Resp", t.Count() == 0 ? "0" : (t.Average(a => a.responseTime)).ToString("N0"));
                        nDict.Add("taskIds", string.Join(",", t.Select(x => x.taskId).ToList()));
                    }
                    else
                    {
                        nDict.Add("Reso", "0");
                        nDict.Add("Resp", "0");
                    }
                    nDict.Add("Count", t.GroupBy(x => x.taskId).Count());
                    dict.Add("M" + month, nDict);
                }

                rDict.Add(dict);

            }
            return Content(Helper.Json.Serialize(rDict), "application/json");

        }


        [PageInfo("Aylık Görev Raporu DataSource Methodu", SHRoles.Personel)]
        public JsonResult MonthlyTaskReportDataSource(int year, string[] months, Guid? customer, Guid? customerStorage)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var task = new List<VWFTM_Task>();
            var taskOperation = new List<VWFTM_TaskOperation>();
            var monthString = String.Join(",", months.ToArray());
            var taskMonthReportData = db.GetVWFTM_MonthTaskReport(year, monthString);

            if (customer.HasValue)
            {
                taskMonthReportData = taskMonthReportData.Where(x => x.customerId == customer.Value).ToArray();
            }

            if (customerStorage.HasValue)
            {
                taskMonthReportData = taskMonthReportData.Where(x => x.customerStorageId == customerStorage.Value).ToArray();
            }

            var dailyReportDatas = taskMonthReportData.GroupBy(a => a.month).Select(c => new
            {
                text = c.Select(x => x.year).FirstOrDefault() + " " + GetMonthlyName(c.Key),
                month = c.Select(a => a.month).FirstOrDefault(),
                resolutionTime = c.Select(a => a.resolutionTime).Average(),
                responseTime = c.Select(b => b.responseTime).Average(),
                count = c.Select(b => b.responseTime).ToList(),
            });
            return Json(dailyReportDatas, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Çağrı Tipi ve Yanıtlama Süresi Ay/Yıl Bazlı Rapor DataSource", SHRoles.Personel)]
        public ContentResult MonthlyTypeLineChartData(int year, string[] months, string[] taskTypes, Guid? customer, Guid? customerStorage)
        {

            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var task = new List<VWFTM_Task>();
            var taskOperation = new List<VWFTM_TaskOperation>();


            var monthString = "";
            if (months.Count() > 0 && months.Where(x => String.IsNullOrEmpty(x)).Count() > 0)
            {
                string[] strArray = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
                months = strArray;
                monthString = String.Join(",", strArray.ToArray());
            }
            else
            {
                monthString = String.Join(",", months.ToArray());
            }
            var taskMonthReportData = new List<MonthTaskReportModel>();

            if (taskTypes != null && taskTypes.Where(x => !String.IsNullOrEmpty(x)).Count() > 0)
            {
                foreach (var taskTypeTitle in taskTypes)
                {
                    taskMonthReportData.AddRange(db.GetVWFTM_MonthTaskSubjectReport(year, monthString).Where(x => x.taskType_Title == taskTypeTitle).ToList());
                }
            }
            else
            {
                taskMonthReportData = db.GetVWFTM_MonthTaskSubjectReport(year, monthString).ToList();
            }

            if (customer.HasValue)
            {
                taskMonthReportData = taskMonthReportData.Where(x => x.customerId == customer.Value).ToList();
            }

            if (customerStorage.HasValue)
            {
                taskMonthReportData = taskMonthReportData.Where(x => x.customerStorageId == customerStorage.Value).ToList();
            }

            var taskType_Title = taskMonthReportData.GroupBy(a => a.taskType_Title).Select(a => a.Key).ToArray();

            var res = new List<object>();

            List<int> monthList = months.Select(s => int.Parse(s)).ToList();

            var rDict = new List<Dictionary<string, object>>();


            if (taskType_Title.Count() <= 0 && taskTypes.Where(x => !String.IsNullOrEmpty(x)).Count() <= 0)
            {
                taskType_Title = EnumsProperties.EnumToArrayValues<EnumFTM_TaskType>().Select(c => c.Value).ToArray();
            }
            else if (taskType_Title.Count() <= 0 && taskTypes.Where(x => !String.IsNullOrEmpty(x)).Count() > 0)
            {
                var taskType = EnumsProperties.EnumToArrayValues<EnumFTM_TaskType>().Select(c => new { Id = c.Key, Name = c.Value }).OrderBy(a => a.Name).ToList();

                var typeString = new List<String>();
                foreach (var typeTitle in taskTypes)
                {
                    if (taskType.Where(x => x.Name == typeTitle).FirstOrDefault() != null)
                    {
                        typeString.Add(typeTitle);
                    }
                }
                taskType_Title = typeString.ToArray();
            }

            var listData = new List<MonthlyTypeLineChartDataModel>();

            foreach (var taskTypeTitle in taskType_Title)
            {

                var data = taskMonthReportData.Where(a => a.taskType_Title == taskTypeTitle);
                var dict = new Dictionary<string, object>();

                dict.Add("taskTypeTitle", taskTypeTitle);


                foreach (var month in monthList)
                {
                    var t = taskMonthReportData.Where(a => a.taskType_Title == taskTypeTitle && a.month == month && a.month > 0);

                    if (t.Count() > 0 && t.Average(a => a.resolutionTime) != 0 && t.Average(a => a.responseTime) != 0)
                    {
                        listData.Add(new MonthlyTypeLineChartDataModel
                        {
                            taskType = taskTypeTitle,
                            Reso = t.Count() == 0 ? "0" : (t.Average(a => a.resolutionTime)).ToString("N0"),
                            Resp = t.Count() == 0 ? "0" : (t.Average(a => a.responseTime)).ToString("N0"),
                            Month = month,
                            taskIds = string.Join(",", t.Select(x => x.taskId).ToList())
                        });
                    }
                    else
                    {
                        listData.Add(new MonthlyTypeLineChartDataModel
                        {
                            taskType = taskTypeTitle,
                            Reso = "0",
                            Resp = "0",
                            Month = month,
                        });
                    }
                }
            }
            return Content(Helper.Json.Serialize(listData), "application/json");

        }

        [PageInfo("Aylık Görev Konu Raporu DataSource Methodu", SHRoles.Personel)]
        public JsonResult MonthlyTaskSubjectReportDataSource(int year, string[] months, Guid? customer, Guid? customerStorage)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var task = new List<VWFTM_Task>();
            var taskOperation = new List<VWFTM_TaskOperation>();
            var monthString = String.Join(",", months.ToArray());
            var taskMonthReportData = db.GetVWFTM_MonthTaskSubjectReport(year, monthString);
            var subjectTitles = taskMonthReportData.GroupBy(a => a.taskType_Title).Select(a => a.Key).ToArray();
            var monthms = taskMonthReportData.GroupBy(a => a.month).Select(a => a.Key).ToArray();


            if (taskMonthReportData.Count() > 0)
            {
                if (customer.HasValue)
                {
                    taskMonthReportData = taskMonthReportData.Where(x => x.customerId == customer).ToArray();
                }

                if (customerStorage.HasValue)
                {
                    taskMonthReportData = taskMonthReportData.Where(x => x.customerStorageId == customerStorage).ToArray();
                }
            }

            var res = new List<object>();

            foreach (var subjectTitle in subjectTitles)
            {
                foreach (var month in monthms)
                {
                    var t = taskMonthReportData.Where(a => a.taskType_Title == subjectTitle && a.month == month);
                    res.Add(new
                    {
                        taskType_Title = subjectTitle,
                        text = t.Count() > 0 ? t.Select(x => x.year).FirstOrDefault() + " " + GetMonthlyName(t.Select(x => x.month).FirstOrDefault()) : taskMonthReportData.Select(x => x.year).FirstOrDefault() + " " + GetMonthlyName(month),
                        month = month,
                        resolutionTime = t.Count() > 0 ? t.Average(a => a.resolutionTime) : 0,
                        responseTime = t.Count() > 0 ? t.Average(a => a.responseTime) : 0,
                        count = t.Select(x => x.taskId).Count()
                    });
                }
            }
            return Json(res, JsonRequestBehavior.AllowGet);

        }


        [PageInfo("Aylık Personel Görev Raporu DataSource Methodu", SHRoles.Personel)]
        public JsonResult MonthlyEmployeeTaskReportDataSource(int year, string[] months, Guid? customer, Guid? customerStorage)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var task = new List<VWFTM_Task>();
            var taskOperation = new List<VWFTM_TaskOperation>();
            var monthString = String.Join(",", months.ToArray());
            var taskMonthReportData = db.GetVWFTM_MonthPersonnelTaskReport(year, monthString);
            var personels = taskMonthReportData.GroupBy(a => a.personnelName).Select(a => a.Key).ToArray();
            var monthms = taskMonthReportData.GroupBy(a => a.month).Select(a => a.Key).ToArray();


            if (taskMonthReportData.Count() > 0)
            {
                if (customer.HasValue)
                {
                    taskMonthReportData = taskMonthReportData.Where(x => x.customerId == customer).ToArray();
                }

                if (customerStorage.HasValue)
                {
                    taskMonthReportData = taskMonthReportData.Where(x => x.customerStorageId == customerStorage).ToArray();
                }
            }


            var res = new List<object>();

            foreach (var personel in personels)
            {
                foreach (var month in monthms)
                {
                    var t = taskMonthReportData.Where(a => a.personnelName == personel && a.month == month && a.month > 0);
                    res.Add(new
                    {
                        personnelName = personel,
                        text = t.Count() > 0 ? t.Select(x => x.year).FirstOrDefault() + " " + GetMonthlyName(t.Select(x => x.month).FirstOrDefault()) : taskMonthReportData.Select(x => x.year).FirstOrDefault() + " " + GetMonthlyName(month),
                        month = month,
                        resolutionTime = t.Count() > 0 ? t.Average(a => a.resolutionTime) : 0,
                        responseTime = t.Count() > 0 ? t.Average(a => a.responseTime) : 0,
                        count = t.Select(x => x.taskId).Count()
                    });
                }
            }

            return Json(res, JsonRequestBehavior.AllowGet);
        }



        [PageInfo("Saha Görevi Ekle (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli)]
        public ActionResult Insert(VMFTM_TaskModel request, bool multiple = false)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var model = request.Load();


            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)) ||
                userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) ||
                userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SistemYonetici)))
            {
                if (multiple)
                {
                    return PartialView("~/Areas/FTM/Views/VWFTM_Task/InsertAllMultiple.cshtml", model);
                }

                model.companyId = userStatus.user.CompanyId;
                return PartialView("~/Areas/FTM/Views/VWFTM_Task/InsertAll.cshtml", model);
            }
            else if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevMusteri)))
            {
                model.customerId = userStatus.user.id;
                return PartialView("~/Areas/FTM/Views/VWFTM_Task/InsertCustomer.cshtml", model);
            }
            else
            {
                model.companyId = userStatus.user.CompanyId;
                return PartialView("~/Areas/FTM/Views/VWFTM_Task/InsertMy.cshtml", model);
            }
        }

        [PageInfo("Stoktan Malzeme Kullanımı", SHRoles.SahaGorevPersonel)]
        public ActionResult InsertTransaction(VMPRD_TransactionModel transaction,Guid? taskId, Guid? outputId)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var model = transaction.Load();
            model.taskId = taskId;
            if (!string.IsNullOrEmpty(model.tenderIds))
            {
                model.items = new List<VMPRD_TransactionItems>();
                var db = new WorkOfTimeDatabase();
                var tenders = db.GetVWCMP_TenderByIds(model.tenderIds.Split(',').Select(x => Guid.Parse(x)).ToArray());
                foreach (var tender in tenders)
                {
                    var tenderItems = db.GetCMP_InvoiceItemByInvoiceId(tender.id);
                    model.items.AddRange(tenderItems.Select(x => new VMPRD_TransactionItems
                    {
                        unitPrice = x.price,
                        quantity = x.quantity,
                        productId = x.productId
                    }).ToList());
                }
            }

            if (model.items.Count() == 1 && !model.items.Select(x => x.productId.HasValue).FirstOrDefault())
            {
                model.items = new List<VMPRD_TransactionItems>();
            }
            model.outputId = outputId;
            model.type = (int)EnumPRD_TransactionType.SarfFisi;
            return View(model);
        }

        [PageInfo("Stoktan Malzeme Kullanımı", SHRoles.SahaGorevPersonel)]
        [HttpPost]
        public ActionResult InsertTransaction(VMPRD_TransactionModel transaction,Guid? taskId,Guid? outputId,bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = new ResultStatus { result = true };
            var taskOperation = new VMFTM_TaskOperationModel();
            transaction.status = (int)EnumPRD_TransactionStatus.islendi;
            transaction.outputId = outputId;
            transaction.outputTable = "CMP_Storage";
            dbresult &= transaction.Save(userStatus.user.id);
            taskOperation.taskId = taskId;
            taskOperation.status = (int)EnumFTM_TaskOperationStatus.StoktanMalzemeKullanimi;
            taskOperation.description = transaction.description;
            taskOperation.userId = userStatus.user.id;
            taskOperation.created = DateTime.Now;
            taskOperation.createdby = userStatus.user.id;
            taskOperation.fixtureId = transaction.outputId;
            dbresult &= taskOperation.Insert(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Görev başarıyla oluşturuldu.", false,Url.Action("Detail","VWFTM_Task", new { area = "FTM", id = taskId })) :
                                             feedback.Warning(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Görev oluşturma işlemi başarısız oldu.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Saha Görev Şablonu Ekle (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        public ActionResult InsertTemplate(VMFTM_TaskModel request)
        {

            var userStatus = (PageSecurity)Session["userStatus"];
            var model = request.Load();

            model.companyId = userStatus.user.CompanyId;

            return PartialView("~/Areas/FTM/Views/VWFTM_Task/InsertAll.cshtml", model);

        }


        [PageInfo("Saha Görev Şablonları (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        public ActionResult IndexTemplates()
        {
            return View();
        }

        [PageInfo("Personelin Görevleri", SHRoles.SahaGorevYonetici)]
        public ActionResult PersonTasks(Guid? personId)
        {
            return View(personId);
        }


        //[PageInfo("Saha Görev Şablonları (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        //public ActionResult Calendar()
        //{
        //    return View();
        //}

        //[PageInfo("Saha Görev Şablonları (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        //public ActionResult CalendarDetail(VMFTM_TaskModel request)
        //{

        //    request.Load();
        //    return View(request);

        //}

        //[PageInfo("Saha Görev Şablonları (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici)]
        //public ContentResult CalendarDataSource()
        //{

        //    var res = new TaskSchedulerModel().TaskPlan.CalendarDataSource();

        //    return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = true, objects = res }), "application/json");

        //}

        [PageInfo("Personel Atama Ekranı", SHRoles.Personel)]
        public ActionResult AssignStaff(VMFTM_TaskModel request)
        {

            var model = request.Load();
            return View(model);
        }


        [HttpPost]
        [PageInfo("Personel Atama Ekranı", SHRoles.Personel)]
        public ActionResult AssignStaff(VMFTM_TaskModel request, bool isPost = false)
        {
            var model = request.Load();
            return View(model);
        }



        [AllowEveryone]
        [PageInfo("Planlanmış Başlangıç ve Bitiş Tarihi Atama", SHRoles.Personel)]
        public ActionResult AssignScheduledDate(VMFTM_TaskModel request)
        {
            var model = request.Load();
            return View(model);
        }


        [HttpPost]
        [AllowEveryone]
        [PageInfo("Planlanmış Başlangıç ve Bitiş Tarihi Atama", SHRoles.Personel)]
        public JsonResult AssignScheduledDate(VMFTM_TaskModel request, bool isPost = false)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var result = db.UpdateFTM_Task(new FTM_Task { id = request.id, planStartDate = request.planStartDate, dueDate = request.dueDate });

            return Json(new ResultStatusUI { Result = result.result, FeedBack = result.result ? feedback.Success("İşlem başarıyla gerçekleştirildi") : feedback.Warning("İşlem gerçekleştirilirken bir sorun oluştu.") });
        }


        [PageInfo("Görev Detayı", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli, SHRoles.YukleniciPersoneli)]
        public ActionResult Detail(VMFTM_TaskModel request)
        {
            var data = request.Load();
            var userStatus = (PageSecurity)Session["userStatus"];
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)) ||
                userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) ||
                userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevPersonel)) ||
                userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SistemYonetici)) ||
                userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.BayiGorevPersoneli)))
            {
                return PartialView("~/Areas/FTM/Views/VWFTM_Task/DetailAll.cshtml", data);
            }
            else
            {
                return PartialView("~/Areas/FTM/Views/VWFTM_Task/DetailCustomer.cshtml", data);
            }
        }

        [PageInfo("Görev Formu Yazdır"), ExportPDF, AllowEveryone]
        public ActionResult Print(VMFTM_TaskModel request)
        {
            var data = request.Load();
            return View(data);
        }

        [PageInfo("Saha Görevi Düzenle (Yetkili Personel/Saha Görev Yöneticisi)", SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ActionResult Update(VMFTM_TaskModel request)
        {
            var model = request.Load();
            return View(model);
        }

        [PageInfo("Saha Görevi Düzenle", SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ActionResult UpdateStaff(VMFTM_TaskModel request)
        {
            var model = request.Load();
            return View(model);
        }


        [PageInfo("Görev Haritası", SHRoles.SahaGorevYonetici)]
        public ActionResult Map()
        {
            var db = new WorkOfTimeDatabase();
            var companyUsers = db.GetVWSH_UserMyPerson();

            var companyList = companyUsers.Select(x => new VWSH_User()
            {
                created = x.created,
                ProfilePhoto = x.ProfilePhoto,
                FullName = x.FullName,
                id = x.id,
                createdby = x.createdby
            }).ToList();

            var user = db.GetVWSH_UserById(Guid.Empty);

            companyList.Add(new VWSH_User()
            {
                created = user.created,
                ProfilePhoto = user.ProfilePhoto,
                FullName = user.FullName,
                id = user.id,
                createdby = user.createdby
            });

            return View(companyList.ToArray());
        }


        [PageInfo("Tüm Saha Görevleri", SHRoles.Personel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli, SHRoles.YukleniciPersoneli)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var userStatus = (PageSecurity)Session["userStatus"];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.BayiGorevPersoneli)))
            {
                condition = UpdateQuery(condition, userStatus);
            }
            condition = new VMFTM_TaskModel().UpdateQuery(condition, userStatus, 1);
            var data = db.GetVWFTM_Task(condition).ToDataSourceResult(request);
            data.Total = db.GetVWFTM_TaskCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Saha Görevleri Miktar DataSource", SHRoles.Personel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli, SHRoles.YukleniciPersoneli)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            condition = new VMFTM_TaskModel().UpdateQuery(condition, userStatus, 1);
            var count = db.GetVWFTM_TaskCount(condition.Filter);
            return count;
        }

        [PageInfo("İşletme detayında ve depo detayında arıza yapılan ürünler için özel", SHRoles.Personel)]
        public ContentResult DataSourceCustom([DataSourceRequest] DataSourceRequest request, Guid companyId, Guid? storageId)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            request.Page = 1;
            var data = db.GetVWFTM_Task(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetFTM_TaskCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Tüm Saha Görevleri", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWFTM_Task(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Çoklu Saha Görevi Ekleme (Yetkili Personel/Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult InsertAllMultiple(VMFTM_TaskModel request, Guid[] customerIds)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = new ResultStatus { result = true };

            foreach (var customerId in customerIds)
            {
                request.id = Guid.NewGuid();
                request.code = BusinessExtensions.B_GetIdCode();
                request.customerId = customerId;
                dbresult &= request.InsertAll(userStatus.user.id, Request);

            }
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Görev/ler başarıyla oluşturuldu.") :
                                            feedback.Warning(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Görev oluşturma işlemi başarısız oldu.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Saha Görevi Ekleme (Yetkili Personel/Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult InsertAll(VMFTM_TaskModel request, Guid[] fixtureIds)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = new ResultStatus { result = true };

            if (fixtureIds != null && fixtureIds.Count() > 0)
            {
                foreach (var fixtureId in fixtureIds)
                {
                    request.id = Guid.NewGuid();
                    request.code = BusinessExtensions.B_GetIdCode();
                    request.fixtureId = fixtureId;
                    dbresult &= request.InsertAll(userStatus.user.id, Request);
                }
            }
            else
            {
                dbresult = request.InsertAll(userStatus.user.id, Request);
            }

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Görev başarıyla oluşturuldu.") :
                                             feedback.Warning(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Görev oluşturma işlemi başarısız oldu.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Saha Görevi Ekleme (Görev Personeli)", SHRoles.SahaGorevPersonel, SHRoles.BayiGorevPersoneli)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult InsertMy(VMFTM_TaskModel request, Guid[] fixtureIds)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = new ResultStatus { result = true };
            if (fixtureIds != null && fixtureIds.Count() > 0)
            {
                foreach (var fixtureId in fixtureIds)
                {
                    request.id = Guid.NewGuid();
                    request.fixtureId = fixtureId;
                    dbresult &= request.InsertMy(userStatus.user.id, Request);
                }
            }
            else
            {
                dbresult = request.InsertMy(userStatus.user.id, Request);
            }

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Görev başarıyla oluşturuldu.") :
                                             feedback.Warning(!string.IsNullOrEmpty(dbresult.message) ? dbresult.message : "Görev oluşturma işlemi başarısız oldu.")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Arıza Bakım Kaydı Oluşturma (Müşteri)", SHRoles.SahaGorevMusteri)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult InsertCustomer(VMFTM_TaskModel request)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            request.customerId = userStatus.user.CompanyId;
            var dbresult = request.InsertCustomer(userStatus.user.id, Request);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Saha Görevi Düzenleme Metodu (Yetkili Personel/Saha Görev Yöneticisi)", SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMFTM_TaskModel request, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = request.Update(userStatus.user.id);
            if (dbresult.result == true)
            {
                new FileUploadSave(Request).SaveAs();
            }
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Görev güncelleme işlemi başarılı.", false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning("Görev güncelleme başarısız. Mesaj : " + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Saha Görevi Düzenleme Metodu (Yetkili Personel/Saha Görev Yöneticisi)", SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult UpdateStaff(VMFTM_TaskModel request, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = request.UpdateStaff(userStatus.user.id);
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Görev personel atama işlemi başarılı.", false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning("Görev personel atama işlemi başarısız. Mesaj : " + dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }





        [PageInfo("Saha Görevi Sil (Saha Görev Yöneticisi)", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel)]
        [HttpPost]
        public JsonResult Delete(Guid[] id)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var list = new List<ResultStatus>();
            foreach (var item in id)
            {
                list.Add(new VMFTM_TaskModel { id = item }.Delete(userStatus.user.id));
            }

            var mesages = string.Join("<br/>", list.Where(a => a.result == false).Select(a => a.message));

            var result = new ResultStatusUI
            {
                Result = list.Count(a => a.result == false) > 0,
                FeedBack = list.Count(a => a.result == false) > 0 ? feedback.Warning("Görevlerin birkaçı silinemedi.<br/><br/>" + mesages) : feedback.Success("Görev silme işlemi başarılı")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Toplu Görev Onayı", SHRoles.SahaGorevYonetici)]
        public ActionResult ApproveAll(string taskIds)
        {
            ViewBag.TaskIds = taskIds;
            return View();
        }

        [PageInfo("Toplu Görev Onayı", SHRoles.SahaGorevYonetici)]
        [HttpPost]
        public ActionResult ApproveAll(string Description, IGeometry location, string taskIds)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var tasks = db.GetVWFTM_TaskByIds(taskIds.Split(',').Select(a => Guid.Parse(a)).ToArray());
            var dbresult = new ResultStatus { result = true };
            foreach (var task in tasks)
            {
                var item = new VMFTM_TaskOperationModel
                {
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    location = location,
                    status = (int)EnumFTM_TaskOperationStatus.CozumOnaylandi,
                    taskId = task.id,
                    userId = userStatus.user.id,
                    description = Description,
                };
                dbresult &= item.Insert(userStatus.user.id);
            }

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Görev güncelleme işlemi başarılı.", false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning("Görev güncelleme başarısız. Mesaj : " + dbresult.message)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [PageInfo("Personel Raporu", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ActionResult StaffReport()
        {
            return View();
        }


        [PageInfo("Görev Raporu", SHRoles.SahaGorevMusteri)]
        public ActionResult Dashboard()
        {
            var model = new TaskSchedulerModel();
            var res = model.GetTaskTemplatePlanList();
            return View(res);
        }

        [PageInfo("Personel Görevleri", SHRoles.Personel)]
        public ContentResult DataSourceForStaffReport(Guid[] assignableUsers, DateTime? planStartDate, DateTime? dueDate, Guid? customer, Guid? customerStorage)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var ftmTask = new VWFTM_Task[0];

            if (assignableUsers == null)
            {
                ftmTask = db.GetVWFTM_TaskTodayTask().Where(x => x.assignUserId.HasValue).ToArray();
            }

            if (assignableUsers != null && !assignableUsers.Contains(Guid.Empty) && planStartDate != null & dueDate != null)
            {
                ftmTask = db.GetVWFTM_TaskByUserIdsAndDate(assignableUsers, planStartDate.Value, dueDate.Value).Where(x => x.assignUserId.HasValue).ToArray();
            }

            if (assignableUsers != null && assignableUsers.Contains(Guid.Empty) && planStartDate != null & dueDate != null)
            {
                ftmTask = db.GetVWFTM_TaskByUserIdsAndDate(null, planStartDate.Value, dueDate.Value).Where(x => x.assignUserId.HasValue).ToArray();
            }

            if (ftmTask.Count() > 0)
            {
                if (customer.HasValue)
                {
                    ftmTask = ftmTask.Where(x => x.customerId == customer).ToArray();
                }

                if (customerStorage.HasValue)
                {
                    ftmTask = ftmTask.Where(x => x.customerStorageId == customerStorage).ToArray();
                }
            }

            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
            {
                var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
                if (authoritys.Count() > 0)
                    ftmTask = ftmTask.Where(x => authoritys.Where(f => f.customerId.HasValue).Select(f => f.customerId.Value).ToArray().Contains(x.customerId.Value)).ToArray();
            }


            var taskChart = taskChartMethod(ftmTask).ToList();

            string[] roles = new string[1] { SHRoles.SahaGorevPersonel };


            var userIds = ftmTask.Where(x => x.assignUserId.HasValue).Select(x => x.assignUserId.Value).ToArray();


            var list = new List<VWModelActivityResult>();
            var userActivityList = new List<VWModelActivityResult>();
            var taskOperatorActivityList = new List<VWModelOperatorActivityResult>();
            var taskStaffReportList = new List<VWFTM_Task>();
            var taskCustomerReportList = new List<VWModelTaskCustomerReportResult>();

            if (userIds.Length > 0)
            {
                var user = db.GetVWSH_UserByIds(userIds, null);

                foreach (var item in user)
                {
                    var model = new VWModelActivityResult();
                    model.Id = item.id;
                    model.Photo = item.ProfilePhoto;
                    model.FullName = item.FullName;
                    model.CompleteCount = ftmTask.Where(x => x.assignUserId == item.id && x.isComplete == true).Count();
                    model.WorkingNow = ftmTask.Where(x => x.assignUserId == item.id).Count() - model.CompleteCount;
                    list.Add(model);

                    var staffModel = new VWModelActivityResult
                    {
                        Id = item.id,
                        FullName = item.FullName,
                        Photo = item.ProfilePhoto,
                        taskCount = ftmTask.Where(x => x.assignUserId == item.id).Count(),
                        CompleteCount = ftmTask.Where(x => x.assignUserId == item.id && x.isComplete == true).Count(),
                    };



                    var taskModel = ftmTask.Where(x => x.assignUserId != null && x.assignUserId == item.id).ToList();

                    foreach (var taskItem in taskModel)
                    {
                        var staffReportModel = new VWFTM_Task
                        {
                            id = taskItem.id,
                            assignUser_Title = item.FullName,
                            type_Title = taskItem.type_Title,
                            type = taskItem.type,
                            planStartDate = taskItem.planStartDate.HasValue ? taskItem.planStartDate.Value : new DateTime(),
                            dueDate = taskItem.dueDate.HasValue ? taskItem.dueDate.Value : new DateTime(),
                            customer_Title = taskItem.customer_Title,
                            customerStorage_Title = taskItem.customerStorage_Title,
                            lastOperationStatus_Title = taskItem.lastOperationStatus_Title,
                            lastOperationDate = taskItem.lastOperationDate,
                            workingHour = taskItem.workingHour
                        };

                        taskStaffReportList.Add(staffReportModel);
                    }


                    taskCustomerReportList = ftmTask.GroupBy(x => x.customer_Title).Select(x => new VWModelTaskCustomerReportResult
                    {
                        FullName = x.Key,
                        allTaskCount = ftmTask.Where(c => c.customer_Title == x.Key).Count(),
                        finishedTask = ftmTask.Where(a => (a.lastOperationStatus == (Int32)EnumFTM_TaskOperationStatus.CozumBildirildi || a.lastOperationStatus == (Int32)EnumFTM_TaskOperationStatus.CozumOnaylandi) && a.customer_Title == x.Key).Count()
                    }).ToList();


                    userActivityList.Add(staffModel);

                }
            }

            var resultData = new
            {
                Staff = list.OrderBy(x => x.CompleteCount).ToArray(),
                StaffActivitys = userActivityList.OrderBy(x => x.FullName),
                staffReportModel = taskStaffReportList,
                taskCustomerReportList = taskCustomerReportList,
                TaskChart = taskChart,
            };
            return Content(Infoline.Helper.Json.Serialize(new ResultStatusUI { Result = true, Object = resultData }), "application/json");
        }


        [PageInfo("Günlük Kullanıcı Raporunun Methodu", SHRoles.Personel)]
        public JsonResult DailyUserReport(DateTime? start, List<Guid?> userIds, Guid? customer, Guid? customerStorage)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];

            if (start == null)
            {
                start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
            }

            var task = new List<VWFTM_Task>();
            var taskOperation = new List<VWFTM_TaskOperation>();
            var taskIds = new List<Guid>();
            if (userIds.Count(x => x.HasValue) == 0)
            {
                taskOperation = db.GetVWFTM_TaskOperationByCreated(start.Value, null).ToList();

                var task2 = db.GetVWFTM_TaskByAssignUserIdNotNullAndAssignableUsers(start.Value, null).ToList();
                task = db.GetVWFTM_TaskByIds(taskOperation.Where(x => x.taskId.HasValue && !x.id.In(task2.Select(a => a.id).ToArray())).GroupBy(x => x.taskId.Value).Select(a => a.Key).ToArray()).ToList();
                task.AddRange(task2);
            }
            else
            {
                taskOperation = db.GetVWFTM_TaskOperationByCreated(start.Value, userIds).ToList();

                var removedUserIds = new List<Guid?>();
                if (taskOperation.Count() > 0)
                {
                    removedUserIds.AddRange(userIds);
                    var ids = taskOperation.GroupBy(x => x.userId).Select(x => x).ToList();
                    foreach (var removeIds in ids)
                    {
                        removedUserIds.Remove(removeIds.Key);
                    }
                }

                var task2 = db.GetVWFTM_TaskByAssignUserIdNotNullAndAssignableUsers(start.Value, userIds.ToList());
                task = db.GetVWFTM_TaskByIds(taskOperation.Where(x => x.taskId.HasValue && !x.id.In(task2.Select(a => a.id).ToArray())).GroupBy(x => x.taskId.Value).Select(a => a.Key).ToArray()).ToList();

                if (taskOperation.Count() > 0 && removedUserIds.Count() > 0)
                {
                    var users = db.GetVWSH_UserByIds(userIds.Where(x => x.HasValue).Select(x => x.Value).ToArray());

                    task.AddRange(users.Select(x => new VWFTM_Task
                    {
                        id = Guid.NewGuid(),
                        assignUserId = x.id,
                        assignUser_Title = x.FullName,
                        created = DateTime.Now.AddYears(-1)
                    }));
                }
                task.AddRange(task2);

            }

            if (customer.HasValue)
            {
                task = task.Where(x => x.customerId == customer.Value).ToList();
            }

            if (customerStorage.HasValue)
            {
                task = task.Where(x => x.customerStorageId == customerStorage.Value).ToList();
            }

            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
            {
                var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
                if (authoritys.Count() > 0)
                    task = task.Where(x => authoritys.Where(f => f.customerId.HasValue).Select(f => f.customerId.Value).ToArray().Contains(x.customerId.Value)).ToList();
            }

            var dailyReport = new List<DailyPersonalReportPersonalData>();
            var dailyReportData = new List<DailyPersonalReportModel>();
            var i = 0;
            int isItOver = 0;

            foreach (var taskItem in task)
            {
                i++;
                var operation = taskOperation.Where(x => x.taskId == taskItem.id && (x.status == (Int32)EnumFTM_TaskOperationStatus.GorevBaslandi || x.status == (Int32)EnumFTM_TaskOperationStatus.CozumBildirildi || x.status == (Int32)EnumFTM_TaskOperationStatus.CozumOnaylandi));

                if (operation.Count() > 0)
                {
                    var startDate = operation.Where(a => a.status == (Int32)EnumFTM_TaskOperationStatus.GorevBaslandi && a.created.HasValue).Select(a => a.created.Value).FirstOrDefault();

                    var endDate = new DateTime();

                    var lastOperation = operation.Where(a => (a.status == (Int32)EnumFTM_TaskOperationStatus.CozumBildirildi || a.status == (Int32)EnumFTM_TaskOperationStatus.CozumOnaylandi) && a.created.HasValue).OrderBy(x => x.created).LastOrDefault();
                    if (lastOperation == null)
                    {
                        endDate = startDate.AddHours(1);
                        isItOver = 0;
                    }
                    else
                    {
                        if (lastOperation.status == (Int32)EnumFTM_TaskOperationStatus.CozumBildirildi)
                        {
                            endDate = lastOperation.created.Value;
                            isItOver = 2;
                        }
                        else
                        {
                            endDate = lastOperation.created.Value;
                            isItOver = 1;
                        }
                    }

                    dailyReportData.Add(new DailyPersonalReportModel
                    {
                        id = i,
                        start = startDate,
                        end = endDate,
                        attendees = taskItem.assignUserId.Value,
                        customer = taskItem.customer_Title ?? "-",
                        title = "",
                        taskId = taskItem.id,
                        taskCode = taskItem.code,
                        taskDescription = taskItem.description,
                        customerStorage_Title = taskItem.customerStorage_Title,
                        taskType_Title = taskItem.type_Title,
                        lastOperationStatus_Title = taskItem.lastOperationStatus_Title,
                        color = isItOver == 0 ? "#f8ac59" : isItOver == 1 ? "#22e93f" : "#1ab394",
                        taskStatus_Title = isItOver == 0 ? "Görev Devam Etmekte." : isItOver == 1 ? "Çözüm Onaylandı." : "Çözüm Bildirildi.",

                    });

                    dailyReport.Add(new DailyPersonalReportPersonalData
                    {
                        dataSource = dailyReportData.Where(x => x.taskId == taskItem.id && x.attendees == taskItem.assignUserId.Value).ToList(),
                        text = operation.Select(x => x.user_Title).FirstOrDefault(),
                        value = taskItem.assignUserId.Value
                    });
                }
                else
                {
                    var startDate = new DateTime();
                    var endDate = new DateTime();
                    bool taskStarted = false;

                    if (!taskItem.planStartDate.HasValue)
                    {
                        startDate = new DateTime(DateTime.Now.Year + 5, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0, 0);
                    }
                    else
                    {
                        startDate = taskItem.planStartDate.Value;
                    }

                    if (!taskItem.dueDate.HasValue)
                    {
                        endDate = startDate.AddHours(1);
                    }
                    else
                    {
                        endDate = taskItem.dueDate.Value;
                    }

                    var userId = new Guid();
                    var userName = "";

                    if (taskItem.assignUserId.HasValue)
                    {
                        userId = taskItem.assignUserId.Value;
                        userName = taskItem.assignUser_Title;
                        taskStarted = true;
                    }
                    else
                    {
                        userId = Guid.Parse(taskItem.assignableUserIds);
                        userName = taskItem.assignableUserTitles;
                        taskStarted = false;
                    }


                    if (taskItem.planLater.HasValue && taskItem.planLater.Value == (int)EnumFTM_TaskPlanLater.Hayir)
                    {
                        dailyReportData.Add(new DailyPersonalReportModel
                        {
                            id = i,
                            start = startDate,
                            end = endDate,
                            attendees = userId,
                            customer = taskItem.customer_Title ?? "-",
                            title = "",
                            taskId = taskItem.id,
                            taskCode = taskItem.code,
                            taskDescription = taskItem.description,
                            customerStorage_Title = taskItem.customerStorage_Title,
                            taskType_Title = taskItem.type_Title,
                            lastOperationStatus_Title = taskItem.lastOperationStatus_Title,
                            color = !taskStarted ? "#23c6c8" : "#1c84c6",
                            taskStatus_Title = !taskStarted ? "Görev Üstlenilmeyi Bekleniyor." : "Görev Üstlenildi.",
                        });
                    }

                    if (taskItem.planLater.HasValue && taskItem.planLater.Value == (int)EnumFTM_TaskPlanLater.Evet)
                    {
                        dailyReportData.Add(new DailyPersonalReportModel
                        {
                            id = i,
                            start = startDate,
                            end = endDate,
                            attendees = userId,
                            customer = taskItem.customer_Title ?? "-",
                            title = "",
                            taskId = taskItem.id,
                            taskCode = taskItem.code,
                            taskDescription = taskItem.description,
                            customerStorage_Title = taskItem.customerStorage_Title,
                            taskType_Title = taskItem.type_Title,
                            lastOperationStatus_Title = taskItem.lastOperationStatus_Title,
                            color = taskItem.planLater == 1 ? "#ff0000" : "#1c84c6",
                            taskStatus_Title = taskItem.planLater == 1 ? "Görev Planlanmış Başlangıç ve Bitişin Atamasını Bekliyor." : "Görev Üstlenildi.",

                        });
                    }


                    dailyReport.Add(new DailyPersonalReportPersonalData
                    {
                        dataSource = dailyReportData.Where(x => x.taskId == taskItem.id && x.attendees == userId).ToList(),
                        text = userName,
                        value = userId
                    });
                }

            }

            if (dailyReport.Count() <= 0)
            {
                return Json(new ResultStatusUI
                {
                    FeedBack = new FeedBack().Warning("Kayıt Bulunamadı.", false)
                }, JsonRequestBehavior.AllowGet);
            }


            var dailyReportDatas = dailyReport.GroupBy(a => a.text).Select(c => new DailyPersonalReportPersonalData
            {
                text = c.Key,
                color = c.Select(a => a.color).FirstOrDefault(),
                value = c.Select(a => a.value).FirstOrDefault(),
                dataSource = c.SelectMany(b => b.dataSource).ToList()
            });


            return Json(dailyReportDatas, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Günlük Kullanıcı Raporunun Methodu", SHRoles.Personel)]
        public JsonResult DailyUserReportPersonals(Guid[] userIds)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];

            string[] roles = new string[1] { SHRoles.SahaGorevPersonel };
            var users = db.GetVWSH_User().Where(x => x.RoleIds.In(roles)).ToList();
            var dailyReport = new List<DailyPersonalReportPersonalData>();


            dailyReport.AddRange(users.Select(x => new DailyPersonalReportPersonalData
            {
                value = x.id,
                text = x.FullName
            }));

            return Json(dailyReport, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Aktivite İzleme", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ActionResult ActivityTracking()
        {
            return View();
        }

        [PageInfo("Saha Görevi Aktivite Data Sayfası", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ContentResult ActivityResult(DateTime? startDate, DateTime? endDate)
        {
            var db = new WorkOfTimeDatabase();
            var ftmTask = startDate != null && endDate != null ? db.GetVWFTM_TaskBetweenDates(startDate, endDate) : db.GetVWFTM_Task();

            var taskChart = taskChartMethod(ftmTask).ToList();

            var sta = ftmTask.Where(x => x.assignUserId != null).GroupBy(x => x.assignUserId).ToArray();
            var resultData = new
            {
                Task = ftmTask,
                Staff = ftmTask.Where(x => x.assignUserId != null).GroupBy(x => x.assignUserId).ToArray(),
                TaskChart = taskChart,
            };
            return Content(Infoline.Helper.Json.Serialize(new ResultStatusUI { Result = true, Object = resultData }), "application/json");

        }



        [PageInfo("Tüm Saha Görevleri", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ContentResult DataSourceForActivityResult([DataSourceRequest] DataSourceRequest request)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            request.PageSize = int.MaxValue;
            var condition = KendoToExpression.Convert(request);
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var ftmTask = new VWFTM_Task[0];
            if (condition.Filter == null)
            {
                condition = VMFTM_TaskModel.ActivityTrackingUpdateQuery(condition);
                condition = new VMFTM_TaskModel().UpdateQuery(condition, userStatus, 1);
                ftmTask = db.GetVWFTM_Task(condition);
            }
            else
            {
                condition = new VMFTM_TaskModel().UpdateQuery(condition, userStatus, 1);
                ftmTask = db.GetVWFTM_Task(condition);
            }


            var ftmTaskOperation = db.GetVWFTM_TaskOperationStatusApprovedByTaskIds(ftmTask.Select(x => x.id).ToArray());
            var taskChart = taskChartMethod(ftmTask).ToList();
            string[] roles = new string[3] { SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici };
            var userIds = db.GetSH_UserRoleByRoleIds(roles).Where(a => a.userid.HasValue).Select(x => x.userid.Value).ToArray();
            var list = new List<VWModelActivityResult>();
            var userActivityList = new List<VWModelActivityResult>();
            var taskOperatorActivityList = new List<VWModelOperatorActivityResult>();

            if (userIds.Length > 0)
            {
                var user = db.GetVWSH_UserByIds(userIds, null);

                var operationStatus = db.GetVWFTM_TaskOperation().Where(x => x.status == 10).ToList();
                //TODO:FTMTask model oluşturulacak
                foreach (var item in user)
                {
                    var model = new VWModelActivityResult();
                    model.Id = item.id;
                    model.Photo = item.ProfilePhoto;
                    model.FullName = item.FullName;
                    model.CompleteCount = ftmTask.Where(x => x.assignUserId == item.id && x.isComplete == true).Count();
                    model.WorkingNow = ftmTask.Where(x => x.assignUserId == item.id).Count() - model.CompleteCount;
                    list.Add(model);

                    var workingHours = "";
                    var girdi = 0;

                    var operations = db.GetVWFTM_TaskOperationByTaskIds(ftmTask.Where(x => x.assignUserId == item.id).Select(x => x.id).ToArray());
                    if (operations.Count() > 0)
                    {
                        var groupOperations = operations.GroupBy(x => x.taskId).Select(x => new
                        {
                            Key = x.Key,
                            StartDatetime = x.Where(t => t.status == (Int32)EnumFTM_TaskOperationStatus.GorevBaslandi).Select(f => f.created).FirstOrDefault(),
                            EndDatetime = x.Where(t => t.status == (Int32)EnumFTM_TaskOperationStatus.CozumBildirildi).Select(f => f.created).FirstOrDefault()
                        }).ToArray();

                        var operationFinish = groupOperations.Where(x => x.EndDatetime.HasValue && x.StartDatetime.HasValue).ToArray();
                        foreach (var opFinish in operationFinish)
                        {
                            TimeSpan res = opFinish.EndDatetime.Value - opFinish.StartDatetime.Value;
                            girdi += Convert.ToInt32(res.TotalSeconds);
                        }

                        int yil = 0;
                        int ay = 0;
                        int gun = 0;
                        int saat = 0;
                        int dakika = 0;

                        yil = girdi / 31104000;
                        girdi = girdi - yil * 3110400;

                        ay = girdi / 2592000;
                        girdi = girdi - ay * 2592000;

                        gun = girdi / 86400;
                        girdi = girdi - gun * 86400;

                        saat = girdi / 3600;
                        girdi = girdi - saat * 3600;

                        dakika = girdi / 60;
                        girdi = girdi - dakika * 60;

                        workingHours = (yil > 0 ? yil + " yıl " : "") +
                                       (ay > 0 ? ay + " ay " : "") +
                                       (gun > 0 ? gun + " gün " : "") +
                                       (saat > 0 ? saat + " saat " : "") +
                                       (dakika > 0 ? dakika + " dakika " : "") +
                                       (girdi > 0 ? girdi + " saniye " : "");
                    }

                    var staffModel = new VWModelActivityResult
                    {
                        Id = item.id,
                        FullName = item.FullName,
                        Photo = item.ProfilePhoto,
                        taskCount = ftmTask.Where(x => x.assignUserId == item.id).Count(),
                        CompleteCount = ftmTask.Where(x => x.assignUserId == item.id && x.isComplete == true).Count(),
                        totalWorkingHours = !string.IsNullOrEmpty(workingHours) ? workingHours : "0 saat"
                    };

                    if (item.RoleIds.Contains(SHRoles.SahaGorevOperator))
                    {
                        var taskOperatorModel = new VWModelOperatorActivityResult
                        {
                            Id = item.id,
                            FullName = item.FullName,
                            OpenedTask = ftmTask.Where(x => x.createdby == item.id).Count(),
                            ApprovedTask = ftmTaskOperation.Where(x => x.createdby == item.id).Count(),
                            MyAppointmentTask = operationStatus.Where(x => x.createdby == item.id).GroupBy(a => a.taskId).Count()
                        };

                        taskOperatorActivityList.Add(taskOperatorModel);
                    }

                    userActivityList.Add(staffModel);

                }
            }

            var resultData = new
            {
                //FtmTask = ftmTask,
                Staff = list.OrderBy(x => x.CompleteCount).ToArray(),
                StaffActivitys = userActivityList.OrderByDescending(x => x.CompleteCount),
                TaskOperatorActivities = taskOperatorActivityList.OrderByDescending(x => x.OpenedTask),
                TaskChart = taskChart,
            };
            return Content(Infoline.Helper.Json.Serialize(new ResultStatusUI { Result = true, Object = resultData }), "application/json");
        }


        [PageInfo("Tüm Saha Görevleri", SHRoles.SahaGorevMusteri)]
        public ContentResult DataSourceForDashboard([DataSourceRequest] DataSourceRequest request)
        {
            request.PageSize = int.MaxValue;
            var condition = KendoToExpression.Convert(request);
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var ftmTask = new VWFTM_Task[0];
            var userStatus = (PageSecurity)Session["userStatus"];

            if (condition.Filter == null)
            {
                if (userStatus.user.CompanyId.HasValue)
                {
                    condition = VMFTM_TaskModel.DashboardUpdateQuery(condition, userStatus.user.CompanyId.Value);
                }
                else
                {
                    condition = VMFTM_TaskModel.ActivityTrackingUpdateQuery(condition);
                }
                ftmTask = db.GetVWFTM_Task(condition);
            }
            else
            {
                ftmTask = db.GetVWFTM_Task(condition);
            }
            var dictData = new List<KeyValuePair<string, int>>();
            if (userStatus.user.CompanyId.HasValue)
            {
                var cmp = db.GetCMP_CompanyById(userStatus.user.CompanyId.Value);
                if (cmp?.type == (int)EnumCMP_CompanyType.Diger)
                {
                    var reData = db.GetVWFTM_TaskSubjectTypeByCustomerIdAndTaskIds(cmp.id, ftmTask.Select(x => x.id).ToArray()).Where(x => !string.IsNullOrEmpty(x.subjectId_Title)).ToArray();
                    var res = reData.GroupBy(x => x.subjectId_Title).Select(c => new
                    {
                        name = c.Key,
                        value = c.Count()
                    }).OrderByDescending(x => x.value).ToDictionary(v => v.name, v => v.value).ToList();

                    dictData = res;
                    ftmTask = ftmTask.Where(x => x.customerId == cmp?.id).ToArray();
                }
                else
                {
                    var res = db.GetVWFTM_TaskSubjectTypeByTaskIds(ftmTask.Select(x => x.id).ToArray()).GroupBy(c => c.subjectId_Title).Select(v => new
                    {
                        name = v.Key,
                        value = v.Count()
                    }).OrderByDescending(x => x.value).ToDictionary(v => v.name, v => v.value).ToList();
                    dictData = res;
                }
            }

            var ftmTaskOperation = db.GetVWFTM_TaskOperationStatusApprovedByTaskIds(ftmTask.Select(x => x.id).ToArray());
            var taskChart = taskChartMethod(ftmTask).ToList();
            string[] roles = new string[3] { SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici };
            var userIds = db.GetSH_UserRoleByRoleIds(roles).Where(a => a.userid.HasValue).Select(x => x.userid.Value).ToArray();
            var list = new List<VWModelActivityResult>();
            var userActivityList = new List<VWModelActivityResult>();
            var taskOperatorActivityList = new List<VWModelOperatorActivityResult>();

            if (userIds.Length > 0 && ftmTask.Count() > 0)
            {
                var user = db.GetVWSH_UserByIds(userIds, null);

                var operationStatus = db.GetVWFTM_TaskOperationByTaskIdsAndStatus(ftmTask.Select(x => x.id).ToArray()).ToList();
                //TODO:FTMTask model oluşturulacak
                foreach (var item in user)
                {
                    var model = new VWModelActivityResult();
                    model.Id = item.id;
                    model.Photo = item.ProfilePhoto;
                    model.FullName = item.FullName;
                    model.CompleteCount = ftmTask.Where(x => x.assignUserId == item.id && x.isComplete == true).Count();
                    model.WorkingNow = ftmTask.Where(x => x.assignUserId == item.id).Count() - model.CompleteCount;
                    list.Add(model);

                    var workingHours = "";
                    var girdi = 0;

                    var operations = db.GetVWFTM_TaskOperationByTaskIds(ftmTask.Where(x => x.assignUserId == item.id).Select(x => x.id).ToArray());
                    if (operations.Count() > 0)
                    {
                        var groupOperations = operations.GroupBy(x => x.taskId).Select(x => new
                        {
                            Key = x.Key,
                            StartDatetime = x.Where(t => t.status == (Int32)EnumFTM_TaskOperationStatus.GorevBaslandi).Select(f => f.created).FirstOrDefault(),
                            EndDatetime = x.Where(t => t.status == (Int32)EnumFTM_TaskOperationStatus.CozumBildirildi).Select(f => f.created).FirstOrDefault()
                        }).ToArray();

                        var operationFinish = groupOperations.Where(x => x.EndDatetime.HasValue && x.StartDatetime.HasValue).ToArray();
                        foreach (var opFinish in operationFinish)
                        {
                            TimeSpan res = opFinish.EndDatetime.Value - opFinish.StartDatetime.Value;
                            girdi += Convert.ToInt32(res.TotalSeconds);
                        }

                        int yil = 0;
                        int ay = 0;
                        int gun = 0;
                        int saat = 0;
                        int dakika = 0;

                        yil = girdi / 31104000;
                        girdi = girdi - yil * 3110400;

                        ay = girdi / 2592000;
                        girdi = girdi - ay * 2592000;

                        gun = girdi / 86400;
                        girdi = girdi - gun * 86400;

                        saat = girdi / 3600;
                        girdi = girdi - saat * 3600;

                        dakika = girdi / 60;
                        girdi = girdi - dakika * 60;

                        workingHours = (yil > 0 ? yil + " yıl " : "") +
                                       (ay > 0 ? ay + " ay " : "") +
                                       (gun > 0 ? gun + " gün " : "") +
                                       (saat > 0 ? saat + " saat " : "") +
                                       (dakika > 0 ? dakika + " dakika " : "") +
                                       (girdi > 0 ? girdi + " saniye " : "");
                    }

                    var staffModel = new VWModelActivityResult
                    {
                        Id = item.id,
                        FullName = item.FullName,
                        Photo = item.ProfilePhoto,
                        taskCount = ftmTask.Where(x => x.assignUserId == item.id).Count(),
                        CompleteCount = ftmTask.Where(x => x.assignUserId == item.id && x.isComplete == true).Count(),
                        totalWorkingHours = !string.IsNullOrEmpty(workingHours) ? workingHours : "0 saat"
                    };

                    if (item.RoleIds.Contains("00000000-0000-0000-0000-620000000000"))
                    {
                        var taskOperatorModel = new VWModelOperatorActivityResult
                        {
                            Id = item.id,
                            FullName = item.FullName,
                            OpenedTask = ftmTask.Where(x => x.createdby == item.id).Count(),
                            ApprovedTask = ftmTaskOperation.Where(x => x.createdby == item.id).Count(),
                            MyAppointmentTask = operationStatus.Where(x => x.createdby == item.id).GroupBy(a => a.taskId).Count()
                        };

                        taskOperatorActivityList.Add(taskOperatorModel);
                    }

                    userActivityList.Add(staffModel);

                }
            }

            var tasks = new List<ChartTask>();

            var listdata = new List<ChartTaskList>();

            for (int i = 1; i <= 12; i++)
            {
                tasks.Add(new ChartTask
                {

                    Ay = (DateTime.Now.Month >= i
                        ? DateTime.Now.Year
                        : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),


                    tum = ftmTask.Where(b => b.created.Value.Month == i && b.created.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Count(),


                });
            }

            for (int i = 1; i <= 12; i++)
            {
                listdata.Add(new ChartTaskList
                {

                    Ay = (DateTime.Now.Month >= i
                        ? DateTime.Now.Year
                        : (DateTime.Now.Year - 1)) + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i),

                    orta = ftmTask.Where(b => b.priority == 0 && b.created.Value.Month == i && b.created.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Count(),
                    yuksek = ftmTask.Where(b => b.priority == 1 && b.created.Value.Month == i && b.created.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Count(),
                    dusuk = ftmTask.Where(b => b.priority == 2 && b.created.Value.Month == i && b.created.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Count(),
                    tum = ftmTask.Where(b => b.created.Value.Month == i && b.created.Value.Year == (DateTime.Now.Month >= i ? DateTime.Now.Year : (DateTime.Now.Year - 1))).Count()
                });
            }

            var gecmisYillar = tasks.Skip(DateTime.Now.Month).Take(12).ToList();
            var mevcutYillar = tasks.Take(DateTime.Now.Month).ToList();
            tasks = gecmisYillar;
            tasks.AddRange(mevcutYillar);

            var gecmisYil = listdata.Skip(DateTime.Now.Month).Take(12).ToList();
            var mevcutYil = listdata.Take(DateTime.Now.Month).ToList();
            listdata = gecmisYil;
            listdata.AddRange(mevcutYil);

            var resultData = new
            {
                ChartWarning = listdata,
                ChartAylik = tasks.ToArray(),
                SubjectData = dictData,
                Staff = list.OrderBy(x => x.CompleteCount).ToArray(),
                StaffActivitys = userActivityList.OrderByDescending(x => x.CompleteCount),
                TaskOperatorActivities = taskOperatorActivityList.OrderByDescending(x => x.OpenedTask),
                TaskChart = taskChart,
            };
            return Content(Infoline.Helper.Json.Serialize(new ResultStatusUI { Result = true, Object = resultData }), "application/json");
        }

        [PageInfo("Kullanıcının Bir Ay İçinde Bulunan İzinleri", SHRoles.Personel)]
        public JsonResult GetPermitUserAvailableStatus(Guid userid)
        {
            var db = new WorkOfTimeDatabase();
            var model = new VWINV_PermitAndTaskEndCount();
            var userName = db.GetSH_UserById(userid);
            model.permits = db.GetVWINV_PermitUserAvailableStatus(userid);
            model.taskCount = db.GetVWFTM_TaskByUserIdTaskOnGoing(userid);
            model.userName = userName?.firstname + " " + userName?.lastname;
            return Json(new ResultStatus
            {
                result = false,
                objects = model
            });
        }


        [PageInfo("Kullanıcının Bir Ay İçinde Bulunan İzinleri", SHRoles.Personel)]
        public JsonResult DownloadFiles(Guid taskId)
        {
            var db = new WorkOfTimeDatabase();
            var task = db.GetFTM_TaskById(taskId);
            var operations = db.GetFTM_TaskOperationByTaskId(taskId);
            var filePath = System.Configuration.ConfigurationManager.AppSettings["FilesPath"];


#if DEBUG
            filePath = Server.MapPath("/Files");
#endif
            if (!Directory.Exists(Path.Combine(filePath, "Temporary")))
            {
                Directory.CreateDirectory(Path.Combine(filePath, "Temporary"));
            }
            foreach (var item in Directory.GetFiles(Path.Combine(filePath, "Temporary")))
            {
                try
                {
                    System.IO.File.Delete(item);
                }
                catch { }
            }



            var files = new List<string>();
            foreach (var operation in operations)
            {
                if (Directory.Exists(Path.Combine(filePath, "FTM_TaskOperation", operation.id.ToString())))
                {
                    files.AddRange(Directory.GetFiles(Path.Combine(filePath, "FTM_TaskOperation", operation.id.ToString()), "*", SearchOption.AllDirectories).ToList());
                }
            }

            if (files.Count() > 0)
            {
                var webPath = "/Files/Temporary/" + task.code + "_gorev_dosya.zip";
                var zipFilePath = Path.Combine(filePath, "Temporary", task.code + "_gorev_dosya.zip");
                Infoline.Framework.Helper.Zip.Created(zipFilePath, files.ToArray(), false);
                return Json(new ResultStatusUI
                {
                    FeedBack = new FeedBack().Success("Görev dosyaları hazırlanıyor lütfen bekleyiniz.", false, webPath)
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultStatusUI
                {
                    FeedBack = new FeedBack().Warning("Görev içerisinde dosya bulunamadı.", false)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        private KeyValuePair<string, int>[] taskChartMethod(VWFTM_Task[] ftmTask)
        {
            return new List<KeyValuePair<string, int>>()
            {
                 new KeyValuePair<string, int>("Tüm Görevler", ftmTask.Count()),
                 new KeyValuePair<string, int>("Personel Ataması Bekleyenler", ftmTask.Where(x => x.assignableUserIds == null && x.isComplete == false).Count()),
                 new KeyValuePair<string, int>("Üstlenilmeyi Bekleyenler", ftmTask.Where(x => (x.assignUserId == null && x.assignableUserIds!= null) && x.isComplete ==false ).Count()),
                 new KeyValuePair<string, int>("Devam Edenler", ftmTask.Where(x => (x.assignUserId != null && x.isComplete == false)&&x.lastOperationStatus!=30&&x.lastOperationStatus != 26).Count()),
                  new KeyValuePair<string, int>("Durdurulanlar", ftmTask.Where(x => x.lastOperationStatus == 26).Count()),
                 new KeyValuePair<string, int>("Çözüm Onayı Bekleyenler", ftmTask.Where(x => x.isComplete == false && x.lastOperationStatus == 30).Count()),
                 new KeyValuePair<string, int>("Çözümlenmiş Görevler", ftmTask.Where(x => x.isComplete == true).Count()),
                  new KeyValuePair<string, int>("Görev Planlanmış Başlangıç ve Bitişin Atamasını Bekleyen Görevler", ftmTask.Where(x => x.planLater == (int)EnumFTM_TaskPlanLater.Evet).Count()),
            }.ToArray();
        }

        public string getPassingTime(DateTime? time, DateTime? created)
        {
            if (created.HasValue && time.HasValue)
            {
                var fark = (time - created.Value);
                var result = (fark.Value.Days != 0 ? fark.Value.Days + " Gün " : "");
                result += (fark.Value.Hours != 0 ? fark.Value.Hours + " Saat " : "");
                result += (fark.Value.Minutes != 0 ? fark.Value.Minutes + " Dakika " : "");
                result += (fark.Value.Seconds != 0 ? fark.Value.Seconds + " Saniye " : "");
                return result;
            }
            else
            {
                return "-";
            }
        }

        public string GetMonthlyName(int month)
        {
            var monthText = "";

            switch (month)
            {
                case 1:
                    monthText = "Ocak";
                    break;
                case 2:
                    monthText = "Şubat";
                    break;
                case 3:
                    monthText = "Mart";
                    break;
                case 4:
                    monthText = "Nisan";
                    break;
                case 5:
                    monthText = "Mayıs";
                    break;
                case 6:
                    monthText = "Haziran";
                    break;
                case 7:
                    monthText = "Temmuz";
                    break;
                case 8:
                    monthText = "Ağustos";
                    break;
                case 9:
                    monthText = "Eylül";
                    break;
                case 10:
                    monthText = "Ekim";
                    break;
                case 11:
                    monthText = "Kasım";
                    break;
                case 12:
                    monthText = "Aralık";
                    break;
                default:
                    break;
            }
            return monthText;
        }

        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
        {
            var company = new WorkOfTimeDatabase().GetVWCMP_CompanyById(userStatus.user.CompanyId.Value);

            BEXP filter = null;

            if (!String.IsNullOrEmpty(company.customerIds))
            {
                foreach (var customer in company.customerIds.Split(',').ToArray())
                {
                    filter |= new BEXP
                    {
                        Operand1 = (COL)"customerId",
                        Operator = BinaryOperator.Equal,
                        Operand2 = (VAL)customer
                    };
                }
            }
            else
            {
                filter |= new BEXP
                {
                    Operand1 = (COL)"id",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)Guid.NewGuid()
                };
            }
            query.Filter &= filter;
            return query;
        }

    }


    public class ChartTask
    {
        public string Ay { get; set; }
        public int tum { get; set; }
    }

    public class ChartTaskList
    {
        public string Ay { get; set; }
        public int orta { get; set; }
        public int yuksek { get; set; }
        public int dusuk { get; set; }
        public int tum { get; set; }
    }
}