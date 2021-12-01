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
            //ChartBoundSeries.add ---- getCompanyBounty
            //    if leng. == 0 
            //    addrange.
            //        rangList == 0 return
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
            var imeis = getImports.Where(x => x.imei != null).Select(x => x.imei).ToArray();
            var getEntegrationProduct = db.GetPRD_EntegrationActionBySerialNumbersOrImeis(imeis).Where(x => x.ProductId != null);

            //  FTP aracılığıyla entegrasyondan gelenler. Distribütor bu cihazları bu bayilere verdim dediği bölüm.
            var entegrationImeis = getEntegrationProduct.Where(x => x.ProductId != null).Select(x => x.Imei).ToList();
            entegrationImeis.AddRange(getEntegrationProduct.Select(x => x.SerialNo));
            //  Cihazlar aktif olduğu anda bildirimlerin yapıldığı bölüm
            var getActivatedDevice = db.GetPRD_TitanDeviceActivatedBySerialNoOrImei(imeis).Where(x => x.ProductId != null);
            var getActivatedDeviceImeis = getActivatedDevice.Select(x => x.IMEI1).ToList();
            getActivatedDeviceImeis.AddRange(getActivatedDevice.Select(x => x.SerialNumber));
            getActivatedDeviceImeis.AddRange(getActivatedDevice.Select(x => x.IMEI2));
            var getInventory = db.GetPRD_InventoryBySerialCodes(imeis).Select(x => x.serialcode);
            var grid = getImports.Select(x => new
            {
                productName = x.productId_Title,
                serialNo = x.imei,
                distControl = entegrationImeis.Contains(x.imei),
                activationControl = getActivatedDeviceImeis.Contains(x.imei),
                inventoryControl = getInventory.Contains(x.imei)
            });
            var total = new List<object>();
            foreach (var item in bounty.Where(x => !x.productId.In(getImports.Select(a => a.product_Id).ToArray())))
            {
                var distIds = getEntegrationProduct.Where(x => x.ProductId == item.productId).Select(x => x.Imei).ToList();
                distIds.AddRange(getEntegrationProduct.Where(x => x.ProductId == item.productId).Select(x => x.SerialNo));
                var titanIds = getActivatedDevice.Where(x => x.ProductId == item.productId).Select(x => x.IMEI1).ToList();
                titanIds.AddRange(getActivatedDevice.Where(x => x.ProductId == item.productId).Select(x => x.IMEI2).ToList());
                titanIds.AddRange(getActivatedDevice.Where(x => x.ProductId == item.productId).Select(x => x.SerialNumber).ToList());
                var distCount = getImports.Where(x => x.product_Id == item.productId).Where(x => distIds.Contains(x.imei)).Count();
                var titanCount = getImports.Where(x => x.product_Id == item.productId).Where(x => titanIds.Contains(x.imei)).ToList().Count();
                var obj = new
                {
                    bountyProduct = item.productId_Title,
                    bountyAmount = item.amount,
                    totalCount = getImports.Where(a => a.product_Id == item.productId).Count(),
                    totalAmount = item.amount * getImports.Where(a => a.product_Id == item.productId).Count(),
                    distCount = distCount,
                    distAmount = distCount * item.amount,
                    titanCount = titanCount,
                    titanAmount = titanCount * item.amount
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
            foreach (var item in excel)
            {
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
                result &= db.InsertPRD_EntegrationImport(items, trans);
            }
            if (result.result)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }
            var result1 = new ResultStatusUI
            {
                Result = result.result,
                FeedBack = result.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };
            return Json(result1, JsonRequestBehavior.AllowGet);
        }
    }
}
