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
        [PageInfo("Hak Ediş Raporu Sayfası", SHRoles.Personel)]
        public ActionResult ClaimReport()
        {

            return View();

        }
        [PageInfo("Hak Ediş Raporu Veri kaynağı", SHRoles.Personel)]
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
            var getImports = db.GetPRD_EntegrationImportByPeriodAndCompanyCode(month, year, getCompany.code);
            if (getImports == null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    Object = "2",
                    FeedBack = new FeedBack().Warning("Cariye ait her hangi bir veri yoktur.")
                }, JsonRequestBehavior.AllowGet);
            }

            var imeis = getImports.Select(x => x.imei).ToArray();
            var getEntegrationProduct = db.GetPRD_EntegrationActionBySerialNumbersOrImeis(imeis);
            //  FTP aracılığıyla entegrasyondan gelenler. Distribütor bu cihazları bu bayilere verdim dediği bölüm.
            var entegrationImeis = getEntegrationProduct.Select(x => x.Imei).ToList();
            entegrationImeis.AddRange(getEntegrationProduct.Select(x => x.SerialNo));
            //  Cihazlar aktif olduğu anda bildirimlerin yapıldığı bölüm
            var getActivatedDevice = db.GetPRD_TitanDeviceActivatedBySerialNoOrImei(imeis);
            var getActivatedDeviceImeis = getActivatedDevice.Select(x => x.IMEI1).ToList();
            getActivatedDeviceImeis.AddRange(getActivatedDevice.Select(x => x.SerialNumber));
            getActivatedDeviceImeis.AddRange(getActivatedDevice.Select(x => x.IMEI2));

            var grid = getImports.Select(x => new
            {
                productName = x.productModel,
                serialNo = x.imei,
                distControl = entegrationImeis.Contains(x.imei),
                activationControl = getActivatedDeviceImeis.Contains(x.imei),
                inventoryControl = db.GetPRD_InventoryBySerialCodeOrImei(x.imei, x.imei) != null,
            });
            var total = bounty.Select(x => new
            {
                bountyProduct = x.productId_Title,
                bountyAmount = x.amount,
                totalCount = getImports.Length,
                totalAmount = x.amount * getImports.Length,
                distCount = getImports.Select(a => entegrationImeis.Contains(a.imei)).Count(),
                distAmount = getImports.Select(a => entegrationImeis.Contains(a.imei)).Count() * x.amount,
                titanCount = getImports.Select(a => getActivatedDeviceImeis.Contains(a.imei)).Count(),
                titanAmount = getImports.Select(a => getActivatedDeviceImeis.Contains(a.imei)).Count() * x.amount
            });


            var returnObject = new
            {
                bounty,
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
        [PageInfo("Entegrasyonda gelen dosya ekleme metodu", SHRoles.Personel)]
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
        [PageInfo("Dosyadan ekleme metodu ", SHRoles.Personel)]
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
