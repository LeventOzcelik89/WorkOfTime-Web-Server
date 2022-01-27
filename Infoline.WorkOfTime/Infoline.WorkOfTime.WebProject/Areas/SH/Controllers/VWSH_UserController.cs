using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using static Infoline.WorkOfTime.BusinessAccess.VMFTM_TaskModel;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class VWSH_UserController : Controller
    {
        [PageInfo("Personel Listesi", SHRoles.IKYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Şirket Rehberi", SHRoles.Personel)]
        public ActionResult ContactOffice()
        {
            var db = new WorkOfTimeDatabase();

            var model = db.GetVWSH_UserMyPersonIsWorking();
            var sysFiles = db.GetSYS_FilesByDataTableAndFileGroup("SH_User", "Özgeçmiş");
            var data = new List<VMSHUserAndFileResume>();
            foreach (var item in model)
            {
                var fileName = sysFiles.Where(x => x.DataId == item.id).FirstOrDefault();
                var newList = new VMSHUserAndFileResume
                {
                    file = fileName != null ? fileName.FilePath : ""
                }.B_EntityDataCopyForMaterial(item);

                data.Add(newList);
            }

            return View(data.ToArray());
        }

        [PageInfo("Kullanıcılar Methodu", SHRoles.Personel)]
        public JsonResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var userStatus = (PageSecurity)Session["userStatus"];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var result = db.GetVWSH_User(condition).RemoveGeographies().OrderByDescending(c => c.created);
            foreach (var item in result)
            {
                if (!userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.IKYonetici)))
                {
                    item.IdentificationNumber = "**************";
                }

                item.loginname = "**************";
                item.password = "**************";
            }
            var data = result.ToDataSourceResult(request);
            data.Total = db.GetVWSH_UserCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Kullanıcılar Adet Methodu"), AllowEveryone]
        public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var adet = db.GetVWSH_UserCount(condition.Filter);
            return adet;
        }

        [PageInfo("Kullanıcı Dropdown Methodu", SHRoles.Personel, SHRoles.YardimMasaTalep, SHRoles.BayiPersoneli, SHRoles.CagriMerkezi)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            var result = db.GetVWSH_User(condition);
            foreach (var item in result)
            {
                item.IdentificationNumber = "**************";
                item.loginname = "**************";
                item.password = "**************";
            }
            return Content(Infoline.Helper.Json.Serialize(result), "application/json");
        }

        [PageInfo("Personel Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert(VMSH_UserModel user)
        {
            var model = user.Load();
            return View(model);
        }

        [PageInfo("Personel Ekleme", SHRoles.IKYonetici, SHRoles.SatisPersoneli, SHRoles.SahaGorevYonetici, SHRoles.CRMYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMSH_UserModel item, short? ispost, string AbsoluteUri)
        {
            var feedBack = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var rs = item.Save();
            return Json(new ResultStatusUI
            {
                Result = rs.result,
                //FeedBack = rs.result ? feedBack.Success(rs.message, false, Url.Action("Index", "VWSH_User", new { area = "SH" })) : feedBack.Warning(rs.message)
                FeedBack = rs.result ? feedBack.Success(rs.message, false, AbsoluteUri) : feedBack.Warning(rs.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Kullanıcı Bilgileri Detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id)
        {
            var model = new VMSH_UserModel { id = id }.Load();
            var db = new WorkOfTimeDatabase();
            model.VWPA_Accounts = db.GetVWPA_AccountsByDataIdDataTable(id, "SH_User");

            var newListKeyValue = new List<KeyValue>();
            var permitTypes = db.GetINV_PermitType();
            var viewDataPermitTypesCanHourly = permitTypes.Where(x => x.CanHourly == true).Select(x => x.id).ToArray();
            var viewDataPermitTypes = permitTypes.Where(x => x.CanHourly == false).Select(x => x.id).ToArray();

            var daylyPermits = db.GetVWINV_PermitByUserIdAndPermitTypeIds(model.id, viewDataPermitTypes)
                .GroupBy(x => x.PermitType_Title)
                .Select(x => new KeyValue
                {
                    Key = x.Key,
                    Value = x.Select(f => f.TotalDays).Sum(),
                    Tooltip = "true"
                }).ToArray();

            var hourlyPermits = db.GetVWINV_PermitByUserIdAndPermitTypeIds(model.id, viewDataPermitTypesCanHourly)
                .GroupBy(x => x.PermitType_Title)
                .Select(x => new KeyValue
                {
                    Key = x.Key,
                    Value = x.Select(f => f.TotalHours).Sum(),
                    Tooltip = "false"
                }).ToArray();

            newListKeyValue.AddRange(daylyPermits);
            newListKeyValue.AddRange(hourlyPermits);

            ViewBag.permits = newListKeyValue;
            return View(model);
        }

        [PageInfo("Personel Düzenleme", SHRoles.IKYonetici)]
        public ActionResult Update(VMSH_UserModel item)
        {
            var db = new WorkOfTimeDatabase();
            var model = item.Load();
            model.VWPA_Accounts = db.GetVWPA_AccountsByDataIdDataTable(model.id, "SH_User");
            return View(model);
        }

        [PageInfo("Personel Düzenleme", SHRoles.IKYonetici, SHRoles.SatisPersoneli, SHRoles.SahaGorevYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMSH_UserModel item, short? ispost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            item.changedby = userStatus.user.id;
            item.changed = DateTime.Now;
            var dbresult = item.Save();
            if (dbresult.result == true && item.sendMail == true)
            {
                item.SendPassword();
            }
            return Json(new ResultStatusUI(dbresult), JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Yeni Şifre Gönderme", SHRoles.IKYonetici, SHRoles.SahaGorevYonetici, SHRoles.OnMuhasebe, SHRoles.StokYoneticisi)]
        public ActionResult SendPassword(VMSH_UserModel item)
        {

            var user = item.Load();
            var feedback = new FeedBack();
            var result = user.SendPassword();

            return Json(new ResultStatusUI
            {
                Result = result.result,
                FeedBack = result.result ? feedback.Success(result.message) : feedback.Warning(result.message)
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Personel İşten Çıkarma", SHRoles.IKYonetici)]
        public ActionResult Dismissal(VMSH_UserModel model)
        {
            var db = new WorkOfTimeDatabase();
            var data = model.Load();
            var zimmet = db.GetVWPRD_StockSummaryByStockId(data.id).Where(a => a.quantity > 0);
            var zimmetDusme = db.GetVWPRD_StockSummaryByStockId(data.id).Where(a => a.quantity < 0);

            var text = "";
            if (zimmet.Count() != zimmetDusme.Count())
            {
                text += zimmet.Count() > 0 ? "<p>Personel üzerinde zimmetli envanterler vardır.Bu envanterleri kullanıcının zimmetinden çıkarmadan işlem yapamazsınız.</p>" : "";
            }
            text += data.PermitYearlyUsable > 0 ? "<p>Personelin <strong>" + data.PermitYearlyUsable + "</strong> gün yıllık izin hakkı bulunmaktadır.İşten çıkarma işlemlerinde yıllık izin hakkı ödenmiş kabul edilecektir.</p>" : "";
            ViewBag.Description = text;
            return View(model);
        }

        [PageInfo("İşletme Personeli İşten Çıkarma Methodu", SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Dismissal(VMSH_UserModel model, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            model.changed = DateTime.Now;
            model.changedby = userStatus.user.id;

            var dbresult = model.Dismissal();
            if (dbresult.result == true)
            {
                new FileUploadSave(Request).SaveAs();
            }

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("İşletme personeli işten çıkarma işlemi başarılı.") : feedback.Warning("İşletme personeli işten çıkarma işlemi başarısız. Mesaj : " + dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Personel Profil Resmi"), AllowEveryone]
        public JsonResult ImageUrl(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var datas = db.GetINV_CompanyPersonDepartmentsById(id);
            var data = db.GetVWSH_UserById(datas.IdUser.Value);
            return Json(data.ProfilePhoto);
        }

        [PageInfo("Toplu Personel Özgeçmişi Kaydetme", SHRoles.IKYonetici)]
        public ActionResult CreateResumeAll()
        {
            var db = new WorkOfTimeDatabase();
            var _shUser = db.GetVWSH_UserMyPersonIsWorking().Select(v => v.id).ToArray();
            var list = new List<VMPersonALLInformation>();
            foreach (var id in _shUser)
            {
                var _user = db.GetSH_UserById(id);
                var _schools = db.GetVWSH_PersonSchoolsByUserId(id).ToArray();
                var _personCertificate = db.GetVWSH_PersonCertificateByUserId(id).ToArray();
                var _personLanguages = db.GetVWSH_PersonLanguagesByUserId(id).ToArray();
                var _personWorkExprience = db.GetVWSH_PersonWorkExperienceByUserId(id).ToArray();
                var _personCompetencies = db.GetVWSH_PersonCompetenciesByUserId(id).ToArray();
                var _personInformation = db.GetVWSH_PersonInformationByUserId(_user.id);
                var data = new VMPersonALLInformation
                {
                    PersonSchools = _schools,
                    PersonCertificate = _personCertificate,
                    PersonLanguages = _personLanguages,
                    PersonWorkExperience = _personWorkExprience,
                    PersonCompetencies = _personCompetencies,
                    PersonInformation = _personInformation
                };
                data.EntityDataCopyForMaterial(_user);
                list.Add(data);
            }
            return View(list.OrderBy(c => c.firstname).ToList());
        }

        [PageInfo("Personel Özgeçmişi Kaydetme", SHRoles.IKYonetici)]
        public ActionResult CreateResume(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var _user = db.GetVWSH_UserById(id);
            var _schools = db.GetVWSH_PersonSchoolsByUserId(id).ToArray();
            var _personCertificate = db.GetVWSH_PersonCertificateByUserId(id).ToArray();
            var _personLanguages = db.GetVWSH_PersonLanguagesByUserId(id).ToArray();
            var _personWorkExprience = db.GetVWSH_PersonWorkExperienceByUserId(id).ToArray();
            var _personCompetencies = db.GetVWSH_PersonCompetenciesByUserId(id).ToArray();
            var _personInformation = db.GetVWSH_PersonInformationByUserId(_user.id);
            var data = new VMPersonALLInformation
            {
                PersonSchools = _schools,
                PersonCertificate = _personCertificate,
                PersonLanguages = _personLanguages,
                PersonWorkExperience = _personWorkExprience,
                PersonCompetencies = _personCompetencies,
                PersonInformation = _personInformation
            };

            data.EntityDataCopyForMaterial(_user);
            return View(data);
        }

        [PageInfo("Müşteri Personel Ekleme", SHRoles.SatisPersoneli, SHRoles.SahaGorevYonetici, SHRoles.CRMYonetici)]
        public ActionResult InsertCustomer(VWSH_User model)
        {
            if (model.FullName != null)
            {
                var splitName = model.FullName.Split(' ');
                model.firstname = string.Join(" ", splitName.Take(splitName.Length - 1));
                model.lastname = splitName[splitName.Length - 1];
                model.FullName = null;
            }

            return View(model);
        }

        [PageInfo("Müşteri Personel Güncelleme", SHRoles.SatisPersoneli, SHRoles.SahaGorevYonetici)]
        public ActionResult UpdateCustomer(VMSH_UserModel item)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_UserById(item.id);
            var roles = db.GetSH_UserRoleByUserId(item.id);
            item.B_EntityDataCopyForMaterial(data);
            if (roles.Count() > 0)
            {
                item.Roles = roles.Where(x => x.roleid.HasValue).Select(x => x.roleid.Value).ToList();
            }

            return View(item);
        }

        [HttpPost]
        [PageInfo("Excel'den Benim Personellerimi Ekleme", SHRoles.IKYonetici)]
        public JsonResult Import(string model)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var culture = new System.Globalization.CultureInfo("tr-TR", false);

            var excelUsers = Helper.Json.Deserialize<SH_UserExcel[]>(model);

            var myPersonsDB = db.GetSH_UserMyPerson().ToList();
            var companies = db.GetCMP_Company();
            var locations = db.GetUT_LocationCityAndTownInTR();
            var currencies = db.GetUT_Currency();
            var userRoles = db.GetSH_UserRole();

            var TL = currencies.Where(a => a.code == "TL").FirstOrDefault();

            var uniqueColumn = ExcelHelper.GetColumnInfo(typeof(SH_UserExcel)).Where(a => a.Unique == true).Select(a => a.Name).FirstOrDefault();

            var existError = new List<ExcelResult>();
            var excelResult = new ExcelResult
            {
                status = true,
                rowNumber = 0
            };

            DateTime? nullable = null;

            foreach (var excelUser in excelUsers)
            {
                excelResult.rowNumber++;
                var trans = db.BeginTransaction();
                var res = new ResultStatus { result = true };


                if (String.IsNullOrEmpty(excelUser.loginname))
                {
                    excelResult.status = false;
                    excelResult.message += " Kimlik Numarası alanı zorunludur";
                    continue;
                }

                if (String.IsNullOrEmpty(excelUser.firstName))
                {
                    excelResult.status = false;
                    excelResult.message += " Ad alanı zorunludur";
                }

                if (String.IsNullOrEmpty(excelUser.lastName))
                {
                    excelResult.status = false;
                    excelResult.message += " Soyad alanı zorunludur";
                }

                if (String.IsNullOrEmpty(excelUser.companyCode))
                {
                    excelResult.status = false;
                    excelResult.message += " İşletme kodu alanı zorunludur";
                }

                if (String.IsNullOrEmpty(excelUser.email))
                {
                    excelResult.status = false;
                    excelResult.message += " Email alanı zorunludur";
                }

                if (String.IsNullOrEmpty(excelUser.title))
                {
                    excelResult.status = false;
                    excelResult.message += " Sigortadaki ünvanı alanı zorunludur";
                }

                if (!excelUser.birthday.HasValue)
                {
                    excelResult.status = false;
                    excelResult.message += " Doğum tarihi alanı zorunludur";
                }

                if (!excelUser.startDate.HasValue)
                {
                    excelResult.status = false;
                    excelResult.message += " İşe giriş tarihi alanı zorunludur";
                }

                if (excelResult.status)
                {
                    var uniqueColumnText = string.Format("{0}", excelUser.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture);

                    var user = myPersonsDB.Where(a => string.Format("{0}", a.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture) == uniqueColumnText).FirstOrDefault();

                    if (user != null)
                    {
                        user.changed = DateTime.Now;
                        user.changedby = userStatus.user.id;
                    }
                    else
                    {
                        var password = Guid.NewGuid().ToString().Substring(0, 8);
                        user = new SH_User
                        {
                            id = Guid.NewGuid(),
                            created = DateTime.Now,
                            createdby = userStatus.user.id,
                            password = db.GetMd5Hash(db.GetMd5Hash(password)),
                            type = (int)EnumSH_UserType.MyPerson,
                            status = true
                        };

                        myPersonsDB.Add(user);
                    }

                    user.firstname = excelUser.firstName.Trim().ToUpper();
                    user.lastname = excelUser.lastName.Trim().ToUpper();
                    user.loginname = excelUser.loginname.Trim().ToUpper();
                    user.email = excelUser.email;
                    user.cellphone = excelUser.phone;
                    user.companyCellPhone = excelUser.companyPhone;
                    user.title = excelUser.title;
                    user.birthday = excelUser.birthday.HasValue && excelUser.birthday.Value > DateTime.Now ? nullable : excelUser.birthday;
                    user.address = excelUser.address;

                    if (!String.IsNullOrEmpty(excelUser.city))
                    {
                        var city = locations.Where(x => x.type == (int)EnumUT_LocationType.Sehir && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelUser.city.ToLower()).FirstOrDefault();

                        if (city != null)
                        {
                            user.locationId = city.id;

                            if (!String.IsNullOrEmpty(excelUser.town))
                            {
                                var town = locations.Where(x => x.type == (int)EnumUT_LocationType.İlce && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelUser.town.ToLower()).ToArray();

                                if (town.Count() > 0)
                                {
                                    user.locationId = town.Where(a => a.pid == city.id).Select(a => a.id).FirstOrDefault();
                                }
                            }
                        }
                    }

                    var company = companies.Where(a => a.code.ToLower(culture) == excelUser.companyCode.ToLower(culture)).FirstOrDefault();

                    if (company != null)
                    {
                        var role = new SH_UserRole
                        {
                            id = Guid.NewGuid(),
                            created = DateTime.Now,
                            createdby = userStatus.user.id,
                            roleid = new Guid(SHRoles.Personel),
                            userid = user.id
                        };

                        if (user.changed.HasValue)
                        {
                            res &= db.UpdateSH_User(user, false, trans);

                            var roles = userRoles.Where(a => a.userid == user.id).ToArray();

                            res &= db.BulkDeleteSH_UserRole(roles, trans);
                            res &= db.InsertSH_UserRole(role, trans);
                        }
                        else
                        {
                            var companyPerson = new INV_CompanyPerson
                            {
                                id = Guid.NewGuid(),
                                created = DateTime.Now,
                                createdby = userStatus.user.id,
                                JobStartDate = excelUser.startDate,
                                CompanyId = company.id,
                                Title = user.title,
                                Level = excelUser.level,
                                IdUser = user.id,
                            };

                            var companyPersonSalary = new INV_CompanyPersonSalary
                            {
                                id = Guid.NewGuid(),
                                created = DateTime.Now,
                                createdby = userStatus.user.id,
                                StartDate = excelUser.startDate,
                                EndDate = excelUser.startDate.HasValue ? excelUser.startDate.Value.AddDays(365) : DateTime.Now.AddDays(365),
                                Salary = 0,
                                IdUser = user.id
                            };

                            var personInfo = new SH_PersonInformation
                            {
                                id = Guid.NewGuid(),
                                created = DateTime.Now,
                                createdby = userStatus.user.id,
                                UserId = user.id,
                                IdentificationNumber = user.loginname
                            };

                            var account = new PA_Account
                            {
                                id = Guid.NewGuid(),
                                created = DateTime.Now,
                                createdby = userStatus.user.id,
                                code = BusinessExtensions.B_GetIdCode(),
                                currencyId = TL.id,
                                isDefault = true,
                                name = "Ana Hesap",
                                status = true,
                                type = (int)EnumPA_AccountType.Kasa,
                                dataId = user.id,
                                dataTable = "SH_User"
                            };

                            res &= db.InsertSH_User(user, trans);
                            res &= db.InsertSH_PersonInformation(personInfo, trans);
                            res &= db.InsertINV_CompanyPerson(companyPerson, trans);
                            res &= db.InsertINV_CompanyPersonSalary(companyPersonSalary, trans);
                            res &= db.InsertPA_Account(account, trans);
                            res &= db.InsertSH_UserRole(role, trans);
                        }
                    }
                    else
                    {
                        excelResult.status = false;
                        excelResult.message += " İşletme koduna sahip işletme bulunamadı.";
                    }
                }

                if (res.result)
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                    excelResult.status = false;
                    excelResult.message += " Bir sorun oluştu";
                }

                existError.Add(excelResult);
                excelResult = new ExcelResult
                {
                    status = true,
                    rowNumber = excelResult.rowNumber
                };
            }

            if (existError.Where(a => a.status == false).Count() == excelUsers.Length)
            {
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Error("Kaydetme işlemi başarısız."),
                    Object = existError,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Kaydetme işlemi başarılı." + (existError.Where(a => a.status == false).Count() > 0 ? "Bazı kayıtlarda problem oluştu." : "")),
                    Object = existError.Where(a => a.status == false).ToArray(),
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [PageInfo("Personel Sertifika Raporu", SHRoles.IKYonetici)]
        public ActionResult CertificateReport()
        {
            return View();
        }

        [PageInfo("Personel Sertifika Raporu", SHRoles.Personel)]
        public ContentResult DataSourceForCertificateReport([DataSourceRequest] DataSourceRequest request, Guid[] certificates, Guid[] assignableUsers, DateTime? dueDate, int? filterButtonId)
        {
            request.PageSize = int.MaxValue;
            var condition = KendoToExpression.Convert(request);
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;


            var db = new WorkOfTimeDatabase();

            var shUsers = new VWSH_User[0];
            var personCertificates = new VWSH_PersonCertificate[0];

            if (certificates != null && !certificates.Contains(Guid.Parse("00000000-0000-0000-0000-000000000000")) && assignableUsers != null && !assignableUsers.Contains(Guid.Parse("00000000-0000-0000-0000-000000000000")))
            {
                shUsers = db.GetVWSH_UserByIds(assignableUsers);
                personCertificates = db.GetVWSH_PersonCertificateByCertificateTypeIds(certificates);
            }
            else if (certificates != null && certificates.Contains(Guid.Parse("00000000-0000-0000-0000-000000000000")) && assignableUsers != null && !assignableUsers.Contains(Guid.Parse("00000000-0000-0000-0000-000000000000")))
            {
                shUsers = db.GetVWSH_UserByIds(assignableUsers);
                personCertificates = db.GetVWSH_PersonCertificate();
            }
            else if (certificates != null && !certificates.Contains(Guid.Parse("00000000-0000-0000-0000-000000000000")) && assignableUsers.Contains(Guid.Parse("00000000-0000-0000-0000-000000000000")))
            {
                personCertificates = db.GetVWSH_PersonCertificateByCertificateTypeIds(certificates);
                if (personCertificates.Count() > 0)
                {
                    shUsers = db.GetVWSH_UserByIds(personCertificates.Where(x => x.UserId.HasValue).Select(x => x.UserId.Value).ToArray());
                }
            }
            else if (assignableUsers == null || assignableUsers.Contains(Guid.Parse("00000000-0000-0000-0000-000000000000")))
            {
                shUsers = db.GetVWSH_UserMyPerson();
                personCertificates = db.GetVWSH_PersonCertificate();
            }

            dueDate = !dueDate.HasValue ? DateTime.Now : dueDate.Value;

            var list = new List<VWModelActivityResult>();
            var userCertificateReportList = new List<VWPersonCertificateModel>();

            foreach (var item in shUsers)
            {
                var certificateReport = new List<VWPersonCertificateModel>();

                var personCertificateData = personCertificates.Where(x => x.UserId == item.id).ToList();

                foreach (var personCertificate in personCertificateData)
                {
                    if (!personCertificate.ExpirationDate.HasValue && (!filterButtonId.HasValue || filterButtonId == 100 || filterButtonId == 1))
                    {
                        certificateReport.Add(new VWPersonCertificateModel
                        {
                            Id = item.id,
                            PersonName = item.FullName,
                            CertificateName = personCertificate.CertificateType_Title,
                            CertificateStatus = "Aktif",
                            CertificateTimeText = "Süresiz"
                        });
                    }
                    else
                    {
                        if (personCertificate.ExpirationDate.HasValue)
                        {
                            var dueDateFormat = new DateTime(personCertificate.ExpirationDate.Value.Year, personCertificate.ExpirationDate.Value.Month, personCertificate.ExpirationDate.Value.Day);
                            var certificateModel = new VWPersonCertificateModel();
                            certificateModel.Id = item.id;
                            certificateModel.PersonName = item.FullName;
                            certificateModel.CertificateName = personCertificate.CertificateType_Title;
                            certificateModel.CertificateEndDate = dueDateFormat.ToString("dd/MM/yyyy");
                            if (personCertificate.ExpirationDate.HasValue && personCertificate.ExpirationDate.Value < dueDate.Value && filterButtonId.HasValue && filterButtonId == 0)
                            {
                                certificateModel.CertificateStatus = "Süresi Dolmuş";
                                certificateModel.CertificateTimeText = getPassingTime(dueDate.Value, personCertificate.ExpirationDate.Value, 1);
                            }
                            else if (personCertificate.ExpirationDate.HasValue && personCertificate.ExpirationDate.Value > dueDate.Value && filterButtonId.HasValue && filterButtonId == 1)
                            {
                                certificateModel.CertificateStatus = "Aktif";
                                certificateModel.CertificateTimeText = getPassingTime(personCertificate.ExpirationDate.Value, dueDate.Value, 0);
                            }
                            else if (!filterButtonId.HasValue || filterButtonId == 100)
                            {
                                certificateModel.CertificateStatus = personCertificate.ExpirationDate.Value < dueDate.Value ? "Süresi Dolmuş" : "Aktif";
                                certificateModel.CertificateTimeText = personCertificate.ExpirationDate.Value < dueDate.Value ? getPassingTime(dueDate.Value, personCertificate.ExpirationDate.Value, 1) : getPassingTime(personCertificate.ExpirationDate.Value, dueDate.Value, 0);
                            }

                            certificateReport.Add(certificateModel);
                        }
                    }
                }
                userCertificateReportList.AddRange(certificateReport);
            }


            if (filterButtonId == 0)
            {
                userCertificateReportList = userCertificateReportList.Where(x => x.CertificateStatus == "Süresi Dolmuş").ToList();
            }
            else if (filterButtonId == 1)
            {
                userCertificateReportList = userCertificateReportList.Where(x => x.CertificateStatus == "Aktif").ToList();
            }

            var resultData = new
            {
                CertificateReport = userCertificateReportList
            };
            return Content(Infoline.Helper.Json.Serialize(new ResultStatusUI { Result = true, Object = resultData }), "application/json");
        }


        public string getPassingTime(DateTime? time, DateTime? created, int status)
        {
            if (created.HasValue && time.HasValue)
            {
                var fark = (time - created.Value);
                var result = (fark.Value.Days != 0 ? fark.Value.Days + " Gün " : "1 Gün");
                result += status == 0 ? " Kaldı" : " Geçti";
                return result;
            }
            else
            {
                return "-";
            }
        }

        [HttpPost]
        [PageInfo("Excel'den Diğer İşletmelere Personel Ekleme", SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
        public JsonResult ImportCustomer(string model)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var culture = new System.Globalization.CultureInfo("tr-TR", false);

            var excelUsers = Helper.Json.Deserialize<SH_UserOtherExcel[]>(model);
            var otherPersonsDB = db.GetSH_UserOtherPerson().ToList();
            var otherCompaniesDB = db.GetCMP_CompanyOther();
            var locations = db.GetUT_LocationCityAndTownInTR();

            var existError = new List<ExcelResult>();
            var excelResult = new ExcelResult
            {
                status = true,
                rowNumber = 0
            };

            foreach (var excelUser in excelUsers)
            {
                excelResult.rowNumber++;
                var trans = db.BeginTransaction();
                var res = new ResultStatus { result = true };

                if (String.IsNullOrEmpty(excelUser.firstName))
                {
                    excelResult.status = false;
                    excelResult.message += " Ad alanı zorunludur";
                }

                if (String.IsNullOrEmpty(excelUser.lastName))
                {
                    excelResult.status = false;
                    excelResult.message += " Soyad alanı zorunludur";
                }

                if (String.IsNullOrEmpty(excelUser.companyCode))
                {
                    excelResult.status = false;
                    excelResult.message += " İşletme Kodu alanı zorunludur";
                }

                var password = Guid.NewGuid().ToString().Substring(0, 8);

                if (excelResult.status)
                {
                    var company = otherCompaniesDB.Where(a => a.code == excelUser.companyCode).FirstOrDefault();
                    if (company != null)
                    {
                        var dbUser = db.GetVWSH_UserByCompanyIdAndName(company.id, excelUser.firstName, excelUser.lastName, excelUser.email);

                        if (dbUser != null)
                        {
                            dbUser.changed = DateTime.Now;
                            dbUser.changedby = userStatus.user.id;
                        }
                        else
                        {
                            dbUser = new VWSH_User
                            {
                                id = Guid.NewGuid(),
                                created = DateTime.Now,
                                createdby = userStatus.user.id,
                                password = db.GetMd5Hash(db.GetMd5Hash(password)),
                                code = BusinessExtensions.B_GetIdCode(),
                                type = (int)EnumCMP_CompanyType.Diger,
                                status = true,
                            };

                            otherPersonsDB.Add(new SH_User().B_EntityDataCopyForMaterial(dbUser));
                        }

                        dbUser.firstname = excelUser.firstName;
                        dbUser.lastname = excelUser.lastName;
                        dbUser.cellphone = excelUser.phone;
                        dbUser.email = excelUser.email;
                        dbUser.loginname = String.IsNullOrEmpty(excelUser.email) ? BusinessExtensions.B_GetIdCode() : excelUser.email;

                        if (!String.IsNullOrEmpty(excelUser.city))
                        {
                            var city = locations.Where(x => x.type == (int)EnumUT_LocationType.Sehir && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelUser.city.ToLower()).FirstOrDefault();

                            if (city != null)
                            {
                                dbUser.locationId = city.id;

                                if (!String.IsNullOrEmpty(excelUser.town))
                                {
                                    var town = locations.Where(x => x.type == (int)EnumUT_LocationType.İlce && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelUser.town.ToLower()).ToArray();

                                    if (town.Count() > 0)
                                    {
                                        dbUser.locationId = town.Where(a => a.pid == city.id).Select(a => a.id).FirstOrDefault();
                                    }
                                }
                            }
                        }

                        if (dbUser.changed.HasValue)
                        {
                            res &= db.UpdateSH_User(new SH_User().B_EntityDataCopyForMaterial(dbUser), false, trans);
                        }
                        else
                        {
                            var user = new SH_User().B_EntityDataCopyForMaterial(this, true);

                            var companyPerson = new INV_CompanyPerson
                            {
                                created = DateTime.Now,
                                createdby = userStatus.user.id,
                                CompanyId = company.id,
                                Level = 0,
                                IdUser = user.id,
                                JobStartDate = DateTime.Now,
                            };

                            res &= db.InsertSH_User(user, trans);
                            res &= db.InsertINV_CompanyPerson(companyPerson, trans);
                        }
                    }
                    else
                    {
                        excelResult.status = false;
                        excelResult.message += " İşletme Koduna sahip işletme bulunamadı.";
                    }
                }

                if (res.result)
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                    excelResult.status = false;
                    excelResult.message += " Bir sorun oluştu";
                }

                existError.Add(excelResult);
                excelResult = new ExcelResult
                {
                    status = true,
                    rowNumber = excelResult.rowNumber
                };
            }

            if (existError.Where(a => a.status == false).Count() == excelUsers.Length)
            {
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Error("Kaydetme işlemi başarısız."),
                    Object = existError,
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Kaydetme işlemi başarılı." + (existError.Where(a => a.status == false).Count() > 0 ? "Bazı kayıtlarda problem oluştu." : "")),
                    Object = existError.Where(a => a.status == false).ToArray(),
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [PageInfo("Rehber", SHRoles.CRMYonetici)]
        public ActionResult ContactCustomerPersons()
        {
            var db = new WorkOfTimeDatabase();
            var model = db.GetVWSH_UserOtherPerson();
            return View(model);
        }

        [PageInfo("Müşteri Kişilerim", SHRoles.CRMYonetici, SHRoles.SatisPersoneli)]
        public ActionResult ContactMyCustomersPersons()
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var company = db.GetVWCMP_CompanyByCreatedby(userStatus.user.id);
            var model = db.GetVWSH_UserOtherPerson(company.Select(x => x.id).ToArray());
            return View(model);
        }

        [PageInfo("Personel'i Bilgilerini Tamamen Silme", SHRoles.SistemYonetici)]
        [HttpPost]
        public JsonResult Delete(VMSH_UserModel item)
        {
            return Json(new ResultStatusUI(item.Delete()), JsonRequestBehavior.AllowGet);
        }

        [PageInfo("İşletmemde Bulunan Saha Görev Personelleri", SHRoles.Personel)]
        public ContentResult DataSourceDropDownForTaskRoles([DataSourceRequest] DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            string[] roles = new string[4] { SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici, SHRoles.YukleniciPersoneli };
            var user = db.GetSH_UserRoleByRoleIds(roles).Select(x => x.userid).ToArray();
            var result = db.GetVWSH_UserByIds(condition, user);

            foreach (var item in result)
            {
                item.loginname = "**************";
                item.password = "**************";
            }
            return Content(Infoline.Helper.Json.Serialize(result), "application/json");
        }

        [PageInfo("İşletmemde Bulunan Saha Görev Personelleri", SHRoles.Personel)]
        public ContentResult DataSourceDropDownForTaskYuklenici([DataSourceRequest] DataSourceRequest request)
        {
            var db = new WorkOfTimeDatabase();
            var condition = KendoToExpression.Convert(request);
            var user = db.GetSH_UserRoleByRoleIds(new string[1] { SHRoles.YukleniciPersoneli }).Select(x => x.userid).ToArray();
            var result = db.GetVWSH_UserByIds(condition, user);

            foreach (var item in result)
            {
                item.loginname = "**************";
                item.password = "**************";
            }
            return Content(Infoline.Helper.Json.Serialize(result), "application/json");
        }
        [PageInfo("Şirketin Kodunu Gönderme Sayfası", SHRoles.IKYonetici)]
        public ActionResult SendCompanyCodeMail()
        {
            return View();
        }
        [PageInfo("Şirketin Kodunu Gönderme Sayfası", SHRoles.IKYonetici)]
        [HttpPost]
        public JsonResult SendCompanyCodeMail(VMSH_UserModel model)
        {
            if (model.CompanyId == null)
            {
                return Json(new ResultStatusUI { FeedBack = new FeedBack().Warning("Bayi Boş Geçilemez!"), Result = false }, JsonRequestBehavior.AllowGet);
            }
            if (model.email == null)
            {
                return Json(new ResultStatusUI { FeedBack = new FeedBack().Warning("E-Posta Boş Geçilemez!"), Result = false }, JsonRequestBehavior.AllowGet);
            }
            model.SendCompanyCode();




            return Json(new ResultStatusUI { FeedBack = new FeedBack().Success("Bayi Kodu Gönderildi"), Result = true }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Şirket Personel Listesi", SHRoles.IKYonetici)]
        public ActionResult CompanyPersonIndex()
        {
            return View();
        }
        [PageInfo("Şirket Personeli Onaylama", SHRoles.IKYonetici)]
        public JsonResult Confirm(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var findUser = db.GetSH_UserById(id);
            if (findUser == null)
            {
                return Json(new ResultStatusUI { FeedBack = new FeedBack().Warning("Böyle Bir Kullanıcı Yok!"), Result = false }, JsonRequestBehavior.AllowGet);
            }
            if (findUser.type != (short)EnumSH_UserType.CompanyPerson)
            {
                return Json(new ResultStatusUI { FeedBack = new FeedBack().Warning("Kullanıcı Bayi Personeli Tipinde Değildir!"), Result = false }, JsonRequestBehavior.AllowGet);
            }
            if (findUser.status == true)
            {
                return Json(new ResultStatusUI { FeedBack = new FeedBack().Warning("Kullanıcı Zaten Onaylanmış!"), Result = false }, JsonRequestBehavior.AllowGet);
            }
            findUser.status = true;
            var result = db.UpdateSH_User(findUser);
            if (result.result)
            {
                SendPassword(new VMSH_UserModel { id = id });
            }
            return Json(new ResultStatusUI
            {
                FeedBack = result.result ? new FeedBack().Success("Kullanıcı Aktifleştirildi. E-Posta Gönderildi") : new FeedBack().Warning("İşlem gerçekleştirilirken bir hata oluştu!"),
                Result = result.result
            }, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Bayi Personelleri Güncellemeye Yarayan Sayfa", SHRoles.IKYonetici)]
        public ActionResult UpdateCompanyCustomer(VMSH_UserModel item)
        {
            var db = new WorkOfTimeDatabase();
            var model = item.Load();
            model.VWPA_Accounts = db.GetVWPA_AccountsByDataIdDataTable(model.id, "SH_User");
            return View(model);
        }

    }
}

