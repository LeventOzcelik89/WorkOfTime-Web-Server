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
namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_EntegrationImportController : Controller
    {
        [PageInfo("Hak Ediş Raporu Sayfası", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        public ActionResult ClaimReport()
        {
            return View();
        }
        [PageInfo("Hak Ediş Raporu Veri kaynağı", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
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
            var imeis = getImports.Where(x => x.imei != null).Select(x => x.imei).ToArray();
            var getEntegrationProduct = db.GetPRD_EntegrationActionBySerialNumbersOrImeis(imeis).Where(x => x.ProductId != null);
            var grid = getImports.Select(x => new
            {
                productName = x.productId_Title,
                serialNo = x.imei,
                distControl = x.distributor_id.HasValue,
                activationControl = x.entegrationAction_id.HasValue,
                inventoryControl = x.inventory_Id.HasValue
            });
            var total = new List<object>();
            foreach (var item in bounty.Where(x => !x.productId.In(getImports.Where(b => b.product_Id.HasValue).Select(a => a.product_Id).ToArray())))
            {
                var obj = new
                {
                    bountyProduct = item.productId_Title,
                    bountyAmount = item.amount,
                    totalCount = getImports.Where(a => a.product_Id == item.productId).Count(),
                    totalAmount = item.amount * getImports.Where(a => a.product_Id == item.productId).Count(),
                    distCount = getImports.Where(x => x.distributor_id.HasValue && x.product_Id == item.productId).Count(),
                    distAmount = getImports.Where(x => x.distributor_id.HasValue && x.product_Id == item.productId).Count() * item.amount,
                    titanCount = getImports.Where(x => x.entegrationAction_id.HasValue && x.product_Id == item.productId).Count(),
                    titanAmount = getImports.Where(x => x.entegrationAction_id.HasValue && x.product_Id == item.productId).Count() * item.amount
                };
                total.Add(obj);
            }

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
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_EntegrationImport(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_EntegrationImportCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("Entegrasyonda gelen dosya ekleme metodu", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(PRD_EntegrationImport item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertPRD_EntegrationImport(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Dosyadan ekleme metodu ", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
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
                excelResult.rowNumber++;
                var isExist = db.GetCMP_CompanyByCode(item.customerCode);
                if (isExist==null)
                {
                    existError.Add(new ExcelResult {
                    rowNumber= excelResult.rowNumber,
                    status=false,
                    message=$"{item.customerCode} kodlu cari yoktur!"
                    });
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
                items.contractStartDate = contractStarDate;
                items.created = DateTime.Now;
                items.createdby = userStatus.user.id;
                items.distributorConfirmationDate = distributorConfirmationDate;
                if (isExist!=null)
                {
      result &= db.InsertPRD_EntegrationImport(items);
                }
          
              
            }
            if (existError.Count()>0)
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
    }
}
