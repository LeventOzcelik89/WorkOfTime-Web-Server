using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace Infoline.WorkOfTime.WebProject.Areas.CMP.Controllers
{
    public class VWCMP_StorageController : Controller
    {
        [PageInfo("Firma&Cari Şube/Depo/Kısımları", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu)]
        public ActionResult Index()
        {
            return View();
        }
        [PageInfo("Şirket Şube/Depo/Kısımları", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.IKYonetici)]
        public ActionResult IndexMy()
        {
            return View();
        }
        [PageInfo("İşletme Şube/Depo/Kısımları Grid Verileri", SHRoles.Personel, SHRoles.BayiPersoneli, SHRoles.CagriMerkezi)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_Storage(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCMP_StorageCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("İşletme Şube/Depo/Kısımları Dropdown Verileri", SHRoles.Personel, SHRoles.SahaGorevMusteri, SHRoles.BayiPersoneli, SHRoles.CagriMerkezi)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            condition = new VMFTM_TaskModel().UpdateQuery(condition, userStatus, 5);
            var data = db.GetVWCMP_Storage(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("İşletme Şube/Depo/Kısımları Detayı", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var model = new VMCMP_StorageCompany().B_EntityDataCopyForMaterial(db.GetVWCMP_StorageById(id));
            if (model.companyId.HasValue)
            {
                model.CMPCompany = db.GetVWCMP_CompanyById(model.companyId.Value);
            }
            else
            {
                model.CMPCompany = new VWCMP_Company();
            }
            var crump = "<ol class=\"breadcrumb\">";
            var crum = BreadCrumps(true, id, model.name).Substring(1).Split('#').Reverse();
            model.breadCrumps = crump + string.Join("", crum) + "<ol>";
            return View(model);
        }
        [PageInfo("İşletme Şube/Depo/Kısım Ekleme", SHRoles.StokYoneticisi)]
        public ActionResult Insert(VWCMP_Storage data)
        {
            if (string.IsNullOrEmpty(data.code))
            {
                data.code = BusinessExtensions.B_GetIdCode();
            }
            data.locationType = (int)EnumCMP_StorageLocationType.Depo;
            if (data.pid.HasValue)
            {
                var db = new WorkOfTimeDatabase();
                var pidStorage = db.GetCMP_StorageById(data.pid.Value);
                if (pidStorage != null)
                {
                    data.location = pidStorage.location;
                    data.companyId = pidStorage.companyId;
                    data.phone = pidStorage.phone;
                    data.address = pidStorage.address;
                    data.locationId = pidStorage.locationId;
                }
            }
            return View(data);
        }
        [PageInfo("İşletme Şube/Depo/Kısımları Ekleme İşlemi", SHRoles.StokYoneticisi)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(CMP_Storage item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var trans = db.BeginTransaction();
            var dbresult = db.InsertCMP_Storage(item, trans);
            if (item.supervisorId.HasValue)
            {
                var hasRole = db.GetSH_UserRoleByUserIdRoleId(item.supervisorId.Value, Guid.Parse(SHRoles.DepoSorumlusu));
                if (hasRole.Count() == 0)
                {
                    var userRole = new SH_UserRole
                    {
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        roleid = Guid.Parse(SHRoles.DepoSorumlusu),
                        userid = item.supervisorId.Value
                    };
                    dbresult &= db.InsertSH_UserRole(userRole, trans);
                }
            }
            if (!dbresult.result)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = dbresult.result,
                    Object = item.id,
                    FeedBack = feedback.Warning("Şube/Depo/Kısım kaydetme işlemi başarısız oldu.")
                }, JsonRequestBehavior.AllowGet);
            }
            trans.Commit();
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = feedback.Success("Şube/Depo/Kısım kaydetme işlemi başarılı bir şekilde gerçekleştirildi.")
            }, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("İşletme Şube/Depo/Kısım Güncelleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_StorageById(id);
            return View(data);
        }
        [PageInfo("İşletme Şube/Depo/Kısım Güncelleme İşlemi", SHRoles.StokYoneticisi)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(CMP_Storage item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            var trans = db.BeginTransaction();
            var dbresult = db.UpdateCMP_Storage(item, false, trans);
            if (item.supervisorId.HasValue)
            {
                var hasRole = db.GetSH_UserRoleByUserIdRoleId(item.supervisorId.Value, Guid.Parse(SHRoles.DepoSorumlusu));
                if (hasRole.Count() == 0)
                {
                    var userRole = new SH_UserRole
                    {
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        roleid = Guid.Parse(SHRoles.DepoSorumlusu),
                        userid = item.supervisorId.Value
                    };
                    dbresult &= db.InsertSH_UserRole(userRole, trans);
                }
            }
            if (!dbresult.result)
            {
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = dbresult.result,
                    Object = item.id,
                    FeedBack = feedback.Warning("Şube/Depo/Kısım güncelleme işlemi başarısız oldu.")
                }, JsonRequestBehavior.AllowGet);
            }
            trans.Commit();
            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                Object = item.id,
                FeedBack = feedback.Success("Şube/Depo/Kısım güncelleme işlemi başarılı oldu.")
            }, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("İşletme Şube/Depo/Kısım Silme İşlemi", SHRoles.StokYoneticisi)]
        [HttpPost]
        public JsonResult Delete(Guid[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var storages = db.GetCMP_StorageByIds(id);
            var errolist = new List<string>() { };
            foreach (var storage in storages)
            {
                var action = db.GetPRD_InventoryActionByDataId(storage.id);
                var transaction = db.GetPRD_TransactionByDataId(storage.id);
                var isHasChild = db.GetVWCMP_StorageByPid(storage.id);
                if (isHasChild)
                {
                    errolist.Add("Bu Depo/Şube/Kısımlarının Alt Depo/Şube/Kısımları olduğundan silinemez");
                }
                if (transaction.Count() > 0 || action.Count() > 0)
                {
                    errolist.Add(storage.code + " " + storage.name + " deposu içerisinde envanterler bulunduğundan silinemez.");
                }
                if (errolist.Count<=0)
                {
                    var dbresult = db.DeleteCMP_Storage(storage);
                    if (dbresult.result == false)
                    {
                        errolist.Add(storage.code + " " + storage.name + " deposu silinirken sorunlar oluştu.");
                    }
                }
            }
            var errormesage = string.Join("", errolist.Select(a => "<p>" + a + "<p/>"));
            var result = new ResultStatusUI
            {
                Result = errolist.Count() == 0,
                FeedBack = errolist.Count() == 0 ? feedback.Success("Silme işlemi başarılı şekilde gerçekleştirildi.") : feedback.Warning(errormesage)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Excelden Şube/Depo/Kısım Ekleme İşlemi", SHRoles.StokYoneticisi)]
        [HttpPost]
        public JsonResult Import(string model)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var culture = new System.Globalization.CultureInfo("tr-TR", false);
            var storagesDB = db.GetCMP_Storage().ToList();
            var companies = db.GetCMP_Company().ToArray();
            var locations = db.GetUT_LocationCityAndTownInTR();
            var excelStorages = Helper.Json.Deserialize<CMP_StorageExcel[]>(model);
            var uniqueColumn = ExcelHelper.GetColumnInfo(typeof(CMP_StorageExcel)).Where(a => a.Unique == true).Select(a => a.Name).FirstOrDefault();
            var existError = new List<ExcelResult>();
            var excelResult = new ExcelResult
            {
                status = true,
                rowNumber = 0
            };
            foreach (var excelStorage in excelStorages)
            {
                excelResult.rowNumber++;
                if (String.IsNullOrEmpty(excelStorage.name))
                {
                    excelResult.status = false;
                    excelResult.message += " Şube/Depo/Kısım Adı alanı zorunludur";
                }
                if (String.IsNullOrEmpty(excelStorage.companyCode))
                {
                    excelResult.status = false;
                    excelResult.message += " İşletme Kodu alanı zorunludur";
                }
                if (excelResult.status)
                {
                    var trans = db.BeginTransaction();
                    var res = new ResultStatus { result = true };
                    var uniqueColumnText = string.Format("{0}", excelStorage.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture);
                    var storage = storagesDB.Where(a => string.Format("{0}", a.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture) == uniqueColumnText).FirstOrDefault();
                    var company = companies.Where(a => !string.IsNullOrEmpty(a.code) && a.code.Trim().ToLower(culture) == excelStorage.companyCode.Trim().ToLower(culture)).FirstOrDefault();
                    if (company != null)
                    {
                        if (storage != null)
                        {
                            storage.changed = DateTime.Now;
                            storage.changedby = userStatus.user.id;
                        }
                        else
                        {
                            storage = new CMP_Storage
                            {
                                created = DateTime.Now,
                                createdby = userStatus.user.id,
                            };
                            storagesDB.Add(storage);
                        }
                        storage.phone = excelStorage.phone;
                        storage.name = excelStorage.name;
                        storage.fax = excelStorage.fax;
                        storage.address = excelStorage.address;
                        storage.companyId = company.id;
                        storage.code = excelStorage.code;
                        if (!String.IsNullOrEmpty(excelStorage.supervisor))
                        {
                            var companyPerson = db.GetVWSH_UserByCompanyId(company.id).Where(a => a.FullName.Trim().ToLower(culture) == excelStorage.supervisor.Trim().ToLower(culture)).FirstOrDefault();
                            if (companyPerson != null)
                            {
                                storage.supervisorId = companyPerson.id;
                            }
                        }
                        if (!String.IsNullOrEmpty(excelStorage.city))
                        {
                            var city = locations.Where(x => x.type == (int)EnumUT_LocationType.Sehir && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelStorage.city.ToLower()).FirstOrDefault();
                            if (city != null)
                            {
                                storage.locationId = city.id;
                                if (!String.IsNullOrEmpty(excelStorage.town))
                                {
                                    var town = locations.Where(x => x.type == (int)EnumUT_LocationType.İlce && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelStorage.town.ToLower()).ToArray();
                                    if (town.Count() > 0)
                                    {
                                        storage.locationId = town.Where(a => a.pid == city.id).Select(a => a.id).FirstOrDefault();
                                    }
                                }
                            }
                        }
                        if (storage.changed.HasValue)
                        {
                            res &= db.UpdateCMP_Storage(storage, false, trans);
                        }
                        else
                        {
                            res &= db.InsertCMP_Storage(storage, trans);
                        }
                    }
                    else
                    {
                        excelResult.status = false;
                        excelResult.message += " İşletme koduna sahip işletme bulunamadı.";
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
                }
                existError.Add(excelResult);
                excelResult = new ExcelResult
                {
                    status = true,
                    rowNumber = excelResult.rowNumber
                };
            }
            if (existError.Where(a => a.status == false).Count() == excelStorages.Length)
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
        [PageInfo("Depoların Ağaç Şeklinde Listelendiği Safa", SHRoles.Personel)]
        public ActionResult GetTreeView(Guid customerId)
        {
            return View(customerId);
        }
        [PageInfo("Depoların Ağaç Şeklinde Listelerin Verilerinin Alındığı Method", SHRoles.Personel)]
        public JsonResult GetTreeViewData(Guid customerId, Guid? id)
        {
            var db = new WorkOfTimeDatabase();

            var storages = db.GetVWCMP_StorageByCompanyId(customerId);
            if (storages.Count() == 0)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            return Json(storages.Where(e=>id.HasValue ? e.pid == id : e.pid == null).Select(a =>
                 new VWCMP_Storage
                {
                     id = a.id,
                     companyId_Image = a.companyId_Image,
                     companyId_Title = a.companyId_Title,
                     name=a.fullName,
                     code=a.code,
                     hasChildren = a.hasChildren
                 }),JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Depoların QR Kodlarının Yazdırılması", SHRoles.Personel)]
        public ActionResult PrintQrCodes([DataSourceRequest] DataSourceRequest request, int? type = 4, int? isLogo = 1)
        {
            var model = new List<VWCMP_Storage>();
            try
            {
                var db = new WorkOfTimeDatabase();
                request.Page = 1;
                request.PageSize = int.MaxValue;
                var condition = KendoToExpression.Convert(request);
                model = db.GetVWCMP_Storage(condition).ToList();
                ViewBag.type = type;
                ViewBag.logo = isLogo;
            }
            catch { }
            if (TenantConfig.Tenant.TenantCode == 1137)
            {
                if (type == 9)
                {
                    return PartialView("~/Areas/CMP/Views/VWCMP_Storage/Print/1137/PrintQrCodesV2.cshtml", model);
                }
                return PartialView("~/Areas/CMP/Views/VWCMP_Storage/Print/1137/PrintQrCodes.cshtml", model);
            }
            else
            {
                return PartialView("~/Areas/CMP/Views/VWCMP_Storage/Print/Default/PrintQrCodes.cshtml", model);
            }
        }
        [PageInfo("Depoların QR Kodlarının Yazdırılması Logo ve Boyutlu", SHRoles.Personel)]
        public ActionResult PrintQrCodesSizes([DataSourceRequest] DataSourceRequest request, int? height = 30, int? width = 50)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            VWCMP_Storage[] model = new VWCMP_Storage[0];
            try
            {
                var db = new WorkOfTimeDatabase();
                request.Page = 1;
                request.PageSize = int.MaxValue;
                var condition = KendoToExpression.Convert(request);
                model = db.GetVWCMP_Storage(condition);
            }
            catch { }
            var data = new QrClass
            {
                height = height,
                width = width,
                invertorys = model.Select(x => new InventoryFilterClass
                {
                    id = x.id,
                    serialcode = x.code,
                    code = x.code,
                    fullName = x.fullName,
                    productId_Title = x.fullName
                }).OrderBy(c => c.serialcode).ToArray(),
            };
            if (userStatus != null && userStatus.user.CompanyId.HasValue)
            {
                var db = new WorkOfTimeDatabase();
                var companyInformation = db.GetCMP_CompanyById(userStatus.user.CompanyId.Value);
                if (companyInformation != null)
                {
                    data.fax = companyInformation.fax;
                    data.logo = "/Content/Customers/" + TenantConfig.Tenant.TenantCode + "/images/logo.png";
                    data.phone = companyInformation.phone;
                    if (TenantConfig.Tenant.TenantCode == 1139)
                    {
                        data.weburl = "www.callay.com.tr";
                        data.logo = "/Content/Customers/1139/images/callayqr.png";
                    }
                    if (TenantConfig.Tenant.TenantCode == 1130)
                    {
                        data.weburl = "vaven.com.tr/";
                    }
                    if (TenantConfig.Tenant.TenantCode == 1137)
                    {
                        data.weburl = "www.erkantarim.com";
                    }
                }
            }
            if (TenantConfig.Tenant.TenantCode == 1137)
            {
                return PartialView("~/Areas/CMP/Views/VWCMP_Storage/Print/1137/PrintQrCodesSizes.cshtml", data);
            }
            else
            {
                return PartialView("~/Areas/CMP/Views/VWCMP_Storage/Print/Default/PrintQrCodesSizes.cshtml", data);
            }
        }
        [PageInfo("Şube/Depo/Kısım Haritası", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu)]
        public ActionResult Map()
        {
            return View();
        }
        [PageInfo("Potansiyel/Fırsat Haritası Verileri", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu)]
        public ContentResult GetMapData()
        {
            var db = new WorkOfTimeDatabase();
            var storages = db.GetVW_CMP_StorageSelected().Select(x => new
            {
                id = x.id,
                code = x.code,
                phone = x.phone,
                address = x.address,
                location = x.location,
                companyId = x.companyId,
                name = x.name,
                companyId_Title = x.companyId_Title,
                myStorage = x.myStorage
            }).ToArray();
            return Content(Infoline.Helper.Json.Serialize(storages), "application/json");
        }



        [PageInfo("Tree View için veriler", SHRoles.Personel)]
        public JsonResult TreeDataSource([DataSourceRequest] DataSourceRequest request, Guid? id)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_Storage(condition).RemoveGeographies().ToTreeDataSourceResult(request);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Tree View için veriler", SHRoles.Personel)]
        public ActionResult HierarchyDataSource(Guid? id, [DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCMP_Storage().RemoveGeographies()
                    .Where(x => x.pid == id)
                .ToDataSourceResult(request);
            return Json(data,JsonRequestBehavior.AllowGet);
        }


        public string BreadCrumps(bool first = false, Guid? pid = null, string name = "")
        {
            var text = "#<li class=\"" + (first ? "active" : "") + "\" style=\"" + (first ? "font-weight:bold;" : "") + "\">";
            text += string.Format("<a href=\"/CMP/VWCMP_Storage/Detail?id={0}\"> {1}</a>", pid, name);
            text += "</li>";
            if (pid.HasValue)
            {
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCMP_StorageById(pid.Value);
                if (data.pid.HasValue)
                {
                    text += BreadCrumps(false, data.pid, data.pid_Title);
                }
            }
            return text;
        }
    }
}
