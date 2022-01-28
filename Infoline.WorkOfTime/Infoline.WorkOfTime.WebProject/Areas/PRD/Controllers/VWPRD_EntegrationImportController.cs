using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;
using Infoline.Framework.Database;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_EntegrationImportController : Controller
    {
        [PageInfo("Hakediş Listeleme Sayfası", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.HakEdisBayiPersoneli)]
        public ActionResult Index()
        {
            return View();
        }
        [PageInfo("Hak Ediş Raporu Sayfası", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.HakEdisBayiPersoneli)]
        public ActionResult ClaimReport()
        {
            return View();
        }
        [PageInfo("Hak Ediş Raporu Veri kaynağı", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.CRMBayiPersoneli, SHRoles.HakEdisBayiPersoneli)]
        public JsonResult ClaimReportDataSource(Guid companyId, int year, int month)
        {
            var db = new WorkOfTimeDatabase();
            var getCompany = db.GetCMP_CompanyById(companyId);
            if (getCompany == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    Object = "0",
                    FeedBack = new FeedBack().Warning("Böyle bir cari yoktur.")
                }, JsonRequestBehavior.AllowGet);
            }
            var bounty = new List<VWPRD_ProductBounty>();
            var getCompanyBounty = db.GetVWPRD_ProductBountyByPeriodAndCompanyId(month, year, companyId);
            if (getCompanyBounty.Length == 0)
            {
                if (getCompany.pid.HasValue)
                {
                    var getDistBounty = db.GetVWPRD_ProductBountyByPeriodAndCompanyId(month, year, getCompany.pid.Value);
                    if (getDistBounty.Length > 0)
                    {
                        bounty.AddRange(getDistBounty);
                    }
                    else
                    {
                        return Json(new ResultStatusUI
                        {
                            Result = false,
                            Object = "1",
                            FeedBack = new FeedBack().Warning("Cariye veya distribütöre atanmış bu aya ait prim tanımı yoktur.")
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        Object = "1",
                        FeedBack = new FeedBack().Warning("Cariye veya distribütöre atanmış bu aya ait prim tanımı yoktur.")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                bounty.AddRange(getCompanyBounty);
            }
            //  bayilerin sattım diye bildirdikleri.
            var getImports = db.GetVWPRD_EntegrationImportByPeriodAndCompanyCode(month, year, getCompany.code);
            if (getImports == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    Object = "2",
                    FeedBack = new FeedBack().Warning("Cariye ait her hangi bir veri yoktur.")
                }, JsonRequestBehavior.AllowGet);
            }
            var products = new List<string>();
            foreach (var getImport in getImports)
            {
                if (bounty.Where(a => a.productId == getImport.product_Id).Count() <= 0)
                {
                    if (getImport.product_Id.HasValue)
                    {
                        var findProduct = db.GetPRD_ProductById(getImport.product_Id.Value);
                        if (findProduct != null)
                        {
                            products.Add(findProduct.name + " | " + findProduct.code);
                        }
                    }
                }
            }
            if (products.Count() > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    Object = "3",
                    FeedBack = new FeedBack().Warning($"{string.Join("\n", products)}")
                }, JsonRequestBehavior.AllowGet);
            }
            var grid = getImports.Select(x => new
            {
                productName = x.productId_Title,
                serialNo = x.imei,
                distControl = x.entegrationAction_id.HasValue,
                activationControl = x.titanActivated_id.HasValue,
                inventoryControl = x.inventory_Id.HasValue
            });
            var total = bounty.Where(x => getImports.Where(b => b.product_Id.HasValue).Select(a => a.product_Id).Contains(x.productId)).Select(x => new
            {
                bountyProduct = x.productId_Title,
                bountyAmount = x.amount,
                totalCount = getImports.Where(a => a.product_Id == x.productId).Count(),
                totalAmount = getImports.Where(a => a.product_Id == x.productId).Count() * x.amount,
                distCount = getImports.Where(a => a.entegrationAction_id.HasValue && a.product_Id == x.productId).Count(),
                distAmount = getImports.Where(a => a.entegrationAction_id.HasValue && a.product_Id == x.productId).Count() * x.amount,
                titanCount = getImports.Where(a => a.titanActivated_id.HasValue && a.product_Id == x.productId).Count(),
                titanAmount = getImports.Where(a => a.titanActivated_id.HasValue && a.product_Id == x.productId).Count() * x.amount
            });
            var counts = new
            {
                total = grid.Count(),
                titan = grid.Where(x => x.activationControl == true).Count(),
                dist = grid.Where(x => x.distControl == true).Count(),
                env = grid.Where(x => x.inventoryControl == true).Count()
            };
            var returnObject = new
            {
                counts,
                grid,
                total
            };
            return Json(returnObject, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Hakediş Veri Kaynağı", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.HakEdisBayiPersoneli)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var condition = KendoToExpression.Convert(request);
            condition = new VMPRD_EntegrationImport().UpdateDataSource(condition, userStatus);
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_EntegrationImport(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_EntegrationImportCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("Hak Ediş Veri Ekleme Sayfası", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.HakEdisBayiPersoneli)]
        public ActionResult Insert(VMPRD_EntegrationImport item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            item.Load();
            var feedBack = new FeedBack();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
            {
                var findUserCompany = db.GetVWCMP_CompanyById(userStatus.user.CompanyId.Value);
                if (findUserCompany != null)
                {
                    var findDist = db.GetVWCMP_CompanyByIdDistributor(findUserCompany.pid.HasValue ? findUserCompany.pid.Value : new Guid());
                    if (findDist != null)
                    {
                        item.distributor_id = findDist.id;
                    }
                    else
                    {
                        return Json(new ResultStatusUI
                        {
                            Result = false,
                            FeedBack = feedBack.Warning("Herhangi bir distribütöre kayıtlı değilsiniz!")
                        });
                    }
                }
                else
                {
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = feedBack.Warning("Herhangi bir bayiye kayıtlı değilsiniz!")
                    });
                }
            }
            return View(item);
        }
        [PageInfo("Hakediş veri ekleme metodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.HakEdisBayiPersoneli)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMPRD_EntegrationImport item)
        {
            var db = new WorkOfTimeDatabase();
            var serialNumberIsExist = db.GetPRD_InventoryBySerialCodeOrImei(item.imei, item.imei);
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
            {
                item.company_Id = userStatus.user.CompanyId;
                var findUserCompany = db.GetVWCMP_CompanyById(userStatus.user.CompanyId.Value);
                if (findUserCompany != null)
                {
                    var findDist = db.GetVWCMP_CompanyByIdDistributor(findUserCompany.pid.HasValue ? findUserCompany.pid.Value : new Guid());
                    if (findDist != null)
                    {
                        item.distributor_id = findDist.id;
                    }
                }
            }
            var dbresult = item.Save(userStatus.user.id);
            var message = serialNumberIsExist == null ? "Bu IMEI numarasının sistemde karşılığı yoktur!" : "";

            if (dbresult.result)
            {
                if (CheckUserCompanyHasNullAreas(dbresult).Data!=null)
                {
                    return CheckUserCompanyHasNullAreas(dbresult);
                }
                
            }

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message + $"\n {message}") : feedback.Warning(dbresult.message)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Hak Ediş Veri Ekleme Sayfası", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.HakEdisBayiPersoneli)]
        public ActionResult Update(VMPRD_EntegrationImport item, bool? isPost)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            item.Load();
            var feedBack = new FeedBack();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
            {
                var findUserCompany = db.GetVWCMP_CompanyById(userStatus.user.CompanyId.Value);
                if (findUserCompany != null)
                {
                    var findDist = db.GetVWCMP_CompanyByIdDistributor(findUserCompany.pid.HasValue ? findUserCompany.pid.Value : new Guid());
                    if (findDist != null)
                    {
                        item.distributor_id = findDist.id;
                    }
                    else
                    {
                        return Json(new ResultStatusUI
                        {
                            Result = false,
                            FeedBack = feedBack.Warning("Herhangi bir distribütöre kayıtlı değilsiniz!")
                        });
                    }
                }
                else
                {
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = feedBack.Warning("Herhangi bir bayiye kayıtlı değilsiniz!")
                    });
                }
            }
            return View(item);
        }
        [PageInfo("Hakediş veri ekleme metodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.HakEdisBayiPersoneli)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMPRD_EntegrationImport item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
            {
                item.company_Id = userStatus.user.CompanyId;
                var findUserCompany = db.GetVWCMP_CompanyById(userStatus.user.CompanyId.Value);
                if (findUserCompany != null)
                {
                    var findDists = db.GetVWCMP_CompanyByIdDistributor(findUserCompany.pid.HasValue ? findUserCompany.pid.Value : new Guid());
                    if (findDists != null)
                    {
                        item.distributor_id = findDists.id;
                    }
                }
            }
            var findDist = db.GetCMP_CompanyById(item.distributor_id.Value);
            var findCompany = db.GetCMP_CompanyById(item.company_Id.Value);
            item.distributorCode = findDist.code;
            item.customerCode = findCompany.code;
            var dbresult = item.Save(userStatus.user.id);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Düzenleme işlemi başarılı") : feedback.Warning(dbresult.message)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Dosyadan ekleme metodu ", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.HakEdisBayiPersoneli)]
        [HttpPost]
        public JsonResult Import(string model)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var trans = db.BeginTransaction();
            var result = new ResultStatus { result = true };
            var excel = Helper.Json.Deserialize<PRD_EntegrationImportExcel[]>(model);
            var existError = new List<ExcelResult>();
            var excelResult = new ExcelResult
            {
                status = true,
                rowNumber = 0
            };
            foreach (var item in excel)
            {
                var companyCode = item.customerCode;
                excelResult.rowNumber++;
                var isExistBefore = db.Get_PRDEntegrationImportByImei(item.imei);
                if (isExistBefore != null)
                {
                    existError.Add(new ExcelResult
                    {
                        rowNumber = excelResult.rowNumber,
                        status = false,
                        message = $"{item.imei}, sistem üzerine bu imei numarası ile daha önce kayıt yapılmıştır!"
                    });
                    continue;
                }
                var isExistDist = db.GetCMP_CompanyByCode(item.distributorCode);
                if (isExistDist == null)
                {
                    existError.Add(new ExcelResult
                    {
                        rowNumber = excelResult.rowNumber,
                        status = false,
                        message = $"{item.distributorCode} kodlu distribütör yoktur!"
                    });
                    continue;
                }
                var isExistCari = db.GetCMP_CompanyByCode(item.customerCode);
                if (isExistCari == null)
                {
                    var isExistStorage = db.GetCMP_StorageByCode(item.customerCode);
                    if (isExistStorage == null)
                    {
                        existError.Add(new ExcelResult
                        {
                            rowNumber = excelResult.rowNumber,
                            status = false,
                            message = $"{item.customerCode} kodlu cari yoktur!"
                        });
                        continue;
                    }
                    else
                    {
                        var findCompany = db.GetCMP_CompanyById(isExistStorage.companyId.Value);
                        if (findCompany != null)
                        {
                            companyCode = findCompany.code;
                        }
                        else
                        {
                            existError.Add(new ExcelResult
                            {
                                rowNumber = excelResult.rowNumber,
                                status = false,
                                message = $"{item.customerCode} kodlu cari yoktur!"
                            });
                            continue;
                        }
                    }
                }
                var contractStarDate = new DateTime(1999, 1, 1);
                var distributorConfirmationDate = new DateTime(1999, 1, 1);
                var contractDateParsed = DateTime.TryParse(item.contractStartDate, out contractStarDate);
                var distributorConfirmationDateParsed = DateTime.TryParse(item.distributorConfirmationDate, out distributorConfirmationDate);
                if (!contractDateParsed)
                {
                    contractStarDate = new DateTime(1999, 1, 1);
                }
                if (!distributorConfirmationDateParsed)
                {
                    distributorConfirmationDate = new DateTime(1999, 1, 1);
                }
                var items = new PRD_EntegrationImport().B_EntityDataCopyForMaterial(item);
                items.contractStartDate = contractStarDate;
                items.year = item.Year > 0 ? item.Year : contractStarDate.Year;
                items.month = item.Month > 0 ? item.Month : contractStarDate.Month;
                items.created = DateTime.Now;
                items.createdby = userStatus.user.id;
                items.customerCode = companyCode;
                items.distributorConfirmationDate = distributorConfirmationDate;
                result &= db.InsertPRD_EntegrationImport(items);
            }
            if (existError.Count() > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Kaydetme işlemi başarılı." + (existError.Where(a => a.status == false).Count() > 0 ? "Bazı kayıtlarda problem oluştu." : "")),
                    Object = existError.Where(a => a.status == false).ToArray(),
                }, JsonRequestBehavior.AllowGet);
            }
            var result1 = new ResultStatusUI
            {
                Result = result.result,
                FeedBack = result.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning(result.message)
            };
            return Json(result1, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Hakediş Doğrulama metodu ", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.HakEdisBayiPersoneli)]
        public JsonResult VerifyData(Guid distId, Guid companyId, Guid productId, string imei, int month, int year)
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Hakediş Silme metodu ", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.HakEdisBayiPersoneli)]
        public JsonResult Delete(Guid[] id)
        {
            var result = new ResultStatus { result = true };
            var db = new WorkOfTimeDatabase();
            var trans = db.BeginTransaction();
            var model = new VMPRD_EntegrationImport();
            foreach (var item in id)
            {
                result = model.Delete(item, trans);
            }
            if (result.result)
            {
                trans.Commit();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Success("Silme işlemi başarıyla gerçekleşti")
                }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Log.Error(result.message);
                trans.Rollback();
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = new FeedBack().Warning("Hakediş Bildirimi Silme İşlemi Başarısız Oldu!")
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [AllowEveryone]
        public JsonResult CheckUserCompanyHasNullAreas()
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
            {
                var findUserCompany = db.GetCMP_CompanyById(userStatus.user.CompanyId.Value);
                var findCompanyAccount = db.GetPA_AccountByDataId(findUserCompany.id);
                if (findUserCompany != null)
                {
                    if (string.IsNullOrEmpty(findUserCompany.taxNumber)
                        || string.IsNullOrEmpty(findUserCompany.email)
                        || string.IsNullOrEmpty(findUserCompany.phone)
                        || string.IsNullOrEmpty(findUserCompany.invoiceAddress)
                        || string.IsNullOrEmpty(findUserCompany.taxOffice))
                    {
                        return Json(1, JsonRequestBehavior.AllowGet);//Kullanıcını Boş Alanları var
                    }

                }
                else
                {
                    return Json(0);//Şirketi Yok
                }
                if (findCompanyAccount.Count() <= 0 || findCompanyAccount == null)
                {
                    return Json(2);//tanımlanmış ödeme bilgisi yok;
                }

            }
            return Json(-1, JsonRequestBehavior.AllowGet);
        }
        [AllowEveryone]
        public JsonResult CheckUserCompanyHasNullAreas(ResultStatus result)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            var message = "";
            var de = Convert.ToInt32(CheckUserCompanyHasNullAreas().Data);
            if (de != -1)
            {
                if (de == 0)//
                {
                    message = "Kullanıcının ait olduğu bir şirket yoktur";
                }
                else if (de == 1)
                {
                    message = "Şirketinizin bilgileri tam girilmemiştir.Ödeme alabilmek için şirketinize ait tüm bilgileri giriniz.";
                }
                else if (de == 2)
                {
                    message = "Ödeme Bilgileri Giriniz Ödeme alabilmek için şirketinize ait ödeme bilgilerinizi giriniz. \n Anasayfa>Ödeme Hesabı Ekle";
                }

                return Json(new ResultStatusUI
                {
                    Result = result.result,
                    FeedBack = result.result ? feedback.Warning($"Kayıt Oluşturuldu </br> {message}", true) : feedback.Warning(result.message)
                }, JsonRequestBehavior.AllowGet);
                
            }
            return null;
        }
    }
}
