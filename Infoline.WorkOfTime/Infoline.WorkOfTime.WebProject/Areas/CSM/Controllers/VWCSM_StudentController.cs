using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Integrations.CallCenter.Dto;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Infoline.WorkOfTime.WebProject.Areas.CSM.Controllers
{
	public class VWCSM_StudentController : Controller
    {
        [PageInfo("Öğrenciler", SHRoles.AdayOgrenciYonetim)]
        public ActionResult Index(string telno, string AgentName, string DialId)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetCSM_Stage();
            return View(data);
        }

        [PageInfo("CallCenter Entegrasyon", SHRoles.CRMYonetici, SHRoles.SatisPersoneli, SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        public ActionResult IndexMy(string telno, string DialId, string AgentName)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            if (!string.IsNullOrEmpty(telno))
            {
                telno = telno.Replace(" ", "").Trim().TrimStart('+').TrimStart('9');
                if (telno.StartsWith("0") == false && telno.Length == 10)
                {
                    telno = "0" + telno;
                }

                var student = db.GetCSM_StudentByPhone(telno);
                if (student != null)
                {
                    if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.AdayOgrenciYonetim)))
                    {
                        return RedirectToAction("Detail", "VWCSM_Student", new
                        {
                            area = "CSM",
                            id = student.id,
                            isAddContact = true,
                            DialId = DialId,
                            AgentName = AgentName,
                            telno = student.phone
                        });
                    }
                    else if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.AdayOgrenciAgent)))
                    {
                        return RedirectToAction("IndexMy", "VWCSM_Activity", new
                        {
                            area = "CSM",
                            studentId = student.id,
                            isAddContact = true,
                            DialId = DialId,
                            AgentName = AgentName,
                            telno = student.phone
                        });
                    }
                    else
                    {
                        return RedirectToAction("Index", "VWCSM_Student", new
                        {
                            area = "CSM"
                        });
                    }
                }
                else
                {
                    if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.AdayOgrenciYonetim)))
                    {
                        return RedirectToAction("Index", "VWCSM_Student", new
                        {
                            area = "CSM",
                            isAddContact = false,
                            DialId = DialId,
                            AgentName = AgentName,
                            telno = telno
                        });
                    }
                    else if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.AdayOgrenciAgent)))
                    {
                        return RedirectToAction("IndexMy", "VWCSM_Activity", new
                        {
                            area = "CSM",
                            studentId = student?.id,
                            isAddContact = true,
                            DialId = DialId,
                            AgentName = AgentName,
                            telno = telno
                        });
                    }
                    else
                    {
                        return RedirectToAction("Index", "VWCSM_Student", new
                        {
                            area = "CSM",
                            studentId = student?.id,
                            isAddContact = true,
                            DialId = DialId,
                            AgentName = AgentName,
                            telno = telno
                        });
                    }
                }
            }
            else
            {
                return RedirectToAction("Index", "VWCSM_Student", new
                {
                    area = "CSM"
                });
            }
        }


        [PageInfo("Öğrenciler Metodu", SHRoles.AdayOgrenciYonetim)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_Student(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCSM_StudentCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Öğrenciler Miktar DataSource", SHRoles.AdayOgrenciYonetim)]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var count = db.GetVWCSM_StudentCount(condition.Filter);
            return count;
        }

        [PageInfo("Öğrenciler Dropdown Metodu", SHRoles.Personel, SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_Student(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Öğrenci Detayı", SHRoles.AdayOgrenciYonetim)]
        public ActionResult Detail(Guid id, bool? isAddContact)
        {
            var data = new VMCSM_StudentModel { id = id }.Load();
            data.isAddContact = isAddContact;
            return View(data);
        }


        [PageInfo("Öğrenci Ekleme Sayfası", SHRoles.AdayOgrenciYonetim)]
        public ActionResult Insert(VMCSM_StudentModel student)
        {
            student.Load();
            return View(student);
        }


        [PageInfo("Öğrenci Ekleme Metodu", SHRoles.AdayOgrenciYonetim)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMCSM_StudentModel item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id);

            if (dbresult.result && (item.LastActivity != null && item.LastActivity.type.HasValue))
            {
                new FileUploadSave(Request, item.LastActivity.id).SaveAs();
                //Fbu Servis çağırma
                #region Fbu CallCenter
                var url = HttpUtility.ParseQueryString(Request.UrlReferrer.OriginalString);
                if (!string.IsNullOrEmpty(url.Get("DialId")) && !string.IsNullOrEmpty(url.Get("telno")))
                {
                    var contactStatus = db.GetCSM_StageById(item.LastActivity.stageId.Value);
                    if (contactStatus != null)
                    {
                        var resultCode = Convert.ToInt32(contactStatus.code);
                        TenantConfig.Tenant.Config.CallCenterService.SetCampaignCall(new BusinessAccess.Integrations.CallCenter.Dto.CampaignCallDto
                        {
                            AgentName = string.IsNullOrEmpty(url.Get("AgentName")) ? "sysAgent" : url.Get("AgentName"),
                            DialId = url.Get("DialId"),
                            CallbackTime = item.LastActivity.contactDate ?? DateTime.Now,
                            CallbackPhone = url.Get("telno"),
                            FinishCode = contactStatus.name,
                            ReasonCode = resultCode == 3 ? EnumReasonCode.Randevu : (resultCode == 100 ? EnumReasonCode.Hatali_Numara : (resultCode == 2 ? EnumReasonCode.Ulasilamadi : EnumReasonCode.Ulasildi))
                        });
                    }
                }
                #endregion
            }

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Öğrenci Düzenleme Sayfası", SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        public ActionResult Update(Guid id)
        {
            var data = new VMCSM_StudentModel { id = id }.Load();
            return View(data);
        }


        [PageInfo("Öğrenci Düzenleme Metodu", SHRoles.AdayOgrenciYonetim, SHRoles.AdayOgrenciAgent)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMCSM_StudentModel item)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = item.Save(userStatus.user.id);

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning(dbresult.message)
            }, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Öğrenci Silme Metodu", SHRoles.AdayOgrenciYonetim)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var student = db.GetCSM_StudentById(id);
            var activities = db.GetCSM_ActivityByStudentId(id);
            var studentDepartments = db.GetCSM_StudentDepartmentByStudentId(id);

            var trans = db.BeginTransaction();

            var dbresult = db.BulkDeleteCSM_Activity(activities, trans);
            dbresult &= db.BulkDeleteCSM_StudentDepartment(studentDepartments, trans);
            dbresult &= db.DeleteCSM_Student(student, trans);

            if (dbresult.result)
                trans.Commit();
            else
                trans.Rollback();

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Call Center Outbound Metodu", SHRoles.AdayOgrenciYonetim)]
        public ViewResult SendCallCenter(string filter, string ids)
        {
            var liste = new List<CallCenterListModel>();
            return View(new CallCenterSendViewModel
            {
                filter = filter,
                ids = ids
            });
        }

        [PageInfo("Call Center Kampanya Dönüş Methodu", SHRoles.AdayOgrenciYonetim)]
        public JsonResult GetCallList(string CampaignId)
        {
            var liste = new List<CallCenterListModel>();
            try
            {
                liste = TenantConfig.Tenant.Config.CallCenterService.GetLists(CampaignId).Select(a => new CallCenterListModel
                {
                    ListDescription = a.ListName,
                    Status = a.ListStatus,
                    ListId = a.ListId,
                }).ToList();
            }
            catch { }
            return Json(liste.ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Call Center Outbound Metodu", SHRoles.AdayOgrenciYonetim)]
        public JsonResult SendCallCenter([DataSourceRequest] DataSourceRequest request, string ids, string listName, string CampaignId)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            condition.Take = 99999;
            condition.Skip = 0;
            var students = new List<VWCSM_Student>();

            if (string.IsNullOrEmpty(ids))
            {
                students = db.GetVWCSM_Student(condition).ToList();
            }
            else
            {
                var studentIds = ids.Split(',').Select(a => Guid.Parse(a)).ToArray();
                students = db.GetVWCSM_StudentByIds(studentIds).ToList();
            }

            var rs = TenantConfig.Tenant.Config.CallCenterService.ImportListToContact(students.Where(a => a.phone != null).Select(a =>
                {
                    return new BusinessAccess.Integrations.CallCenter.Dto.ContactDto
                    {
                        UniqueId = a.id.ToString(),
                        CampaignId = CampaignId,
                        ListName = listName,
                        Email = a.email,
                        Name = (a.name ?? "").Split(',').FirstOrDefault(),
                        SurName = (a.name ?? "").Split(',').LastOrDefault(),
                        Phone = a.phone,
                    };
                }).ToList());


            return Json(new ResultStatusUI(rs));
        }

        [HttpPost]
        [PageInfo("Öğrenci ilgilendiği bölümler metodu"), AllowEveryone]
        public string GetDepartments(Guid studentId)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCSM_DepartmentByStudentId(studentId);
            var res = string.Join(",", data.Select(a => a.departmentId_Title).ToArray());
            return res;
        }



        [HttpPost]
        [PageInfo("Excel'den Aday Öğrenci Ekleme", SHRoles.AdayOgrenciYonetim)]
        public JsonResult Import(string model)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var userStatus = (PageSecurity)Session["userStatus"];
            var rsm = new ResultStatus { result = true };

            var locations = db.GetUT_LocationCityAndTownInTR();
            var stagesDB = db.GetCSM_Stage().ToList();
            var studentsDB = db.GetCSM_Student().ToList();
            var schoolDB = db.GetCSM_School().ToList();
            var departmentsDB = db.GetCSM_Department().ToList();

            var studentExcelData = Helper.Json.Deserialize<CSM_StudentExcel[]>(model);

            var studentList = new List<CSM_Student>();
            var departmentList = new List<CSM_Department>();
            var studentDeparmentList = new List<CSM_StudentDepartment>();
            var schoolList = new List<CSM_School>();
            var activityList = new List<CSM_Activity>();
            var stageList = new List<CSM_Stage>();

            var culture = new System.Globalization.CultureInfo("tr-TR", false);

            var enumGradeArray = EnumsProperties.EnumToArrayGeneric<EnumCSM_StudentGrade>().ToArray();
            var enumSourceArray = EnumsProperties.EnumToArrayGeneric<EnumCSM_StudentSource>().ToArray();

            var existErrors = new List<ExcelResult>();
            var row = 0;
            foreach (var studentData in studentExcelData)
            {
                row += 1;
                var excelResult = new ExcelResult
                {
                    status = true,
                    rowNumber = row
                };

                var fullName = studentData.name + " " + studentData.surname;
                var enumGrade = enumGradeArray.Where(a => a.Value.ToLower(culture).Contains(studentData.grade.ToLower(culture)) || a.Key.ToLower(culture).Contains(studentData.grade.ToLower(culture))).FirstOrDefault();
                var enumSource = enumSourceArray.Where(a => a.Value.ToLower(culture).Contains(studentData.source.ToLower(culture)) || a.Key.ToLower(culture).Contains(studentData.source.ToLower(culture))).FirstOrDefault();

                if (studentData.phone.Contains("+90") || studentData.phone.Contains("+9"))
                {
                    studentData.phone = studentData.phone.Replace("+90", "0");
                    studentData.phone = studentData.phone.Replace("+9", "0");
                }

                if (!string.IsNullOrEmpty(studentData.phone) && studentData.phone.StartsWith("0") == false && studentData.phone.Length == 10)
                {
                    studentData.phone = "0" + studentData.phone;
                }

                var nameValidation = StudentBackendValidations.Name(studentData.name);
                var surnameValidation = StudentBackendValidations.Surname(studentData.surname);
                var phoneValidation = StudentBackendValidations.Phone(studentData.phone);
                var departmentValidation = StudentBackendValidations.DepartmentTarget(studentData.departmentTarget);
                var cityValidation = StudentBackendValidations.City(studentData.city);
                var townValidation = StudentBackendValidations.Town(studentData.town);
                var schoolValidation = StudentBackendValidations.School(studentData.school);

                if (nameValidation.IsError && surnameValidation.IsError && phoneValidation.IsError && departmentValidation.IsError
                    && cityValidation.IsError && townValidation.IsError && schoolValidation.IsError)
                {
                    return Json(new ResultStatusUI
                    {
                        Result = true,
                        FeedBack = feedback.NullableMessage("Excel Başlıklar Eşleştirilmelidir."),
                        Object = existErrors.Where(a => a.status == false).ToArray(),
                    }, JsonRequestBehavior.AllowGet);
                }

                if (nameValidation.IsError)
                {
                    excelResult.status = false;
                    excelResult.message = nameValidation.data_error;
                }
                else if (surnameValidation.IsError)
                {
                    excelResult.status = false;
                    excelResult.message = surnameValidation.data_error;
                }
                else if (phoneValidation.IsError)
                {
                    excelResult.status = false;
                    excelResult.message = phoneValidation.data_error;
                }
                else if (departmentValidation.IsError)
                {
                    excelResult.status = false;
                    excelResult.message = departmentValidation.data_error;
                }
                else if (cityValidation.IsError)
                {
                    excelResult.status = false;
                    excelResult.message = cityValidation.data_error;
                }
                else if (townValidation.IsError)
                {
                    excelResult.status = false;
                    excelResult.message = townValidation.data_error;
                }
                else if (schoolValidation.IsError)
                {
                    excelResult.status = false;
                    excelResult.message = schoolValidation.data_error;
                }

                var isExistStudentPhone = studentsDB.Where(x => x.phone == studentData.phone).FirstOrDefault();
                if (isExistStudentPhone != null)
                {
                    excelResult.status = false;
                    excelResult.message = "Aynı telefon numarası ile kayıt mevcut";
                }

                if (!excelResult.status)
                {
                    existErrors.Add(excelResult);
                    continue;
                }

                excelResult = new ExcelResult
                {
                    status = true,
                    rowNumber = excelResult.rowNumber
                };


                var student = new CSM_Student
                {
                    id = Guid.NewGuid(),
                    name = studentData.name + " " + studentData.surname,
                    phone = studentData.phone,
                    email = studentData.email,
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    grade = enumGrade != null ? Convert.ToInt16(enumGrade.Key) : (short)EnumCSM_StudentGrade.Bilinmiyor,
                    isAllowContact = true,
                    source = enumSource != null ? Convert.ToInt16(enumSource.Key.toInt32()) : (short)EnumCSM_StudentSource.Fuar,
                    sourceDescription = studentData.sourceDescription,
                    deparmentCurrent = studentData.departmentCurrent
                };

                if (!String.IsNullOrEmpty(studentData.school))
                {
                    var isExistSchool = schoolDB.Where(a => a.name.Replace(" ", "").ToLower(culture) == studentData.school.Replace(" ", "").ToLower(culture)).FirstOrDefault();

                    if (isExistSchool == null)
                    {
                        isExistSchool = new CSM_School
                        {
                            name = studentData.school,
                            created = DateTime.Now,
                            createdby = userStatus.user.id,
                            id = Guid.NewGuid(),
                        };

                        schoolList.Add(isExistSchool);
                        schoolDB.Add(isExistSchool);
                    }

                    student.schoolId = isExistSchool.id;
                }

                foreach (var department in studentData.departmentTarget.Split('-').Select(c => c.Trim()))
                {
                    if (String.IsNullOrEmpty(department))
                        continue;

                    var isExistDepartment = departmentsDB.Where(a => a.name.Replace(" ", "").ToLower(culture) == department.Replace(" ", "").ToLower(culture)).FirstOrDefault();

                    if (isExistDepartment == null)
                    {
                        isExistDepartment = new CSM_Department
                        {
                            id = Guid.NewGuid(),
                            created = DateTime.Now,
                            createdby = userStatus.user.id,
                            name = department
                        };

                        departmentList.Add(isExistDepartment);
                        departmentsDB.Add(isExistDepartment);
                    }

                    studentDeparmentList.Add(new CSM_StudentDepartment
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        departmentId = isExistDepartment.id,
                        studentId = student.id
                    });
                }

                if (!String.IsNullOrEmpty(studentData.city))
                {
                    var city = locations.Where(x => x.type == (int)EnumUT_LocationType.Sehir && x.name == studentData.city).FirstOrDefault();

                    if (city != null)
                    {
                        student.locationId = city.id;

                        if (!String.IsNullOrEmpty(studentData.town))
                        {
                            var town = locations.Where(x => x.type == (int)EnumUT_LocationType.İlce && x.name == studentData.town).ToArray();

                            if (town.Count() > 0)
                            {
                                student.locationId = town.Where(a => a.pid == city.id).Select(a => a.id).FirstOrDefault();
                            }
                        }
                    }
                }

                studentList.Add(student);
                studentsDB.Add(student);

                var isExistsStage = stagesDB.Where(a => a.status == (int)EnumCSM_StageStatus.FormDoldurdu).FirstOrDefault();

                if (isExistsStage == null)
                {
                    isExistsStage = new CSM_Stage
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        code = "1",
                        color = "#48d5da",
                        name = "Form Dolduruldu",
                        status = (int)EnumCSM_StageStatus.FormDoldurdu,
                    };

                    stagesDB.Add(isExistsStage);
                    stageList.Add(isExistsStage);
                }

                activityList.Add(new CSM_Activity
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    date = studentData.date,
                    contactDate = studentData.contactDate,
                    studentId = student.id,
                    type = (int)EnumCSM_ActivityType.Fuar,
                    duration = studentData.duration,
                    stageId = isExistsStage.id
                });
            }

            if (existErrors.Where(a => a.status == false).Count() == studentExcelData.Count())
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Error("Kaydetme işlemi başarısız."),
                    Object = existErrors.Where(a => a.status == false).ToArray(),
                }, JsonRequestBehavior.AllowGet);
            }

            if (studentList.Count() == 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Excel'den aktarma işlemi başarısız oldu."),
                    Object = existErrors.Where(a => a.status == false).Count() > 0 ? existErrors.Where(a => a.status == false).ToArray() : null
                }, JsonRequestBehavior.AllowGet);
            }

            var trans = db.BeginTransaction();
            rsm &= db.BulkInsertCSM_Stage(stageList, trans);
            rsm &= db.BulkInsertCSM_School(schoolList, trans);
            rsm &= db.BulkInsertCSM_Department(departmentList, trans);
            rsm &= db.BulkInsertCSM_Student(studentList, trans);
            rsm &= db.BulkInsertCSM_StudentDepartment(studentDeparmentList, trans);
            rsm &= db.BulkInsertCSM_Activity(activityList, trans);

            if (!rsm.result)
            {
                trans.Rollback();
            }
            else
            {
                trans.Commit();
            }

            return Json(new ResultStatusUI
            {
                Result = rsm.result,
                FeedBack = rsm.result ? feedback.Success("Kaydetme işlemi başarılı." + (existErrors.Where(a => a.status == false).Count() > 0 ? existErrors.Where(a => a.status == false).Count() + " kayıtta bazı sorunlar yaşandı.İndirilen excelde hata durumları belirtilmektedir." : "")) : feedback.Error("Kaydetme işlemi başarısız. Mesaj : " + rsm.message),
                Object = existErrors.Where(a => a.status == false).Count() > 0 ? existErrors.Where(a => a.status == false).ToArray() : null
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
