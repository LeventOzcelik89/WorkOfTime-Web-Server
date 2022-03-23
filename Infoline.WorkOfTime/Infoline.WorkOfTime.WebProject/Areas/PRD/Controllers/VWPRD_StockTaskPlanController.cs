using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
    public class VWPRD_StockTaskPlanController : Controller
    {
        [PageInfo("Ürün Değişimi", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult Index()
        {
            return View();
        }



        [PageInfo("Abone yönetim veri methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_StockTaskPlan(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPRD_StockTaskPlanCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Abone yönetim dropdown veri methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_StockTaskPlan(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [HttpPost]
        [PageInfo("Excel'den Abone Ekleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public JsonResult Import(string model)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var culture = new System.Globalization.CultureInfo("tr-TR", false);

            var storages = db.GetCMP_Storage();
            var stockTaskPlanDb = db.GetPRD_StockTaskPlan().ToList();
            var locations = db.GetUT_LocationCityAndTownInTR();
            var companies = db.GetCMP_Company().ToList();
            var products = db.GetPRD_Product().ToList();
            var brands = db.GetPRD_Brand().ToList();

            var excelDatas = Helper.Json.Deserialize<PRD_StockTaskPlanExcel[]>(model);

            var uniqueColumn = ExcelHelper.GetColumnInfo(typeof(PRD_StockTaskPlanExcel)).Where(a => a.Unique == true).Select(a => a.Name).FirstOrDefault();

            var existError = new List<ExcelResult>();
            var excelResult = new ExcelResult
            {
                status = true,
                rowNumber = 0
            };


            foreach (var excelData in excelDatas)
            {
                excelResult.rowNumber++;

                if (string.IsNullOrEmpty(excelData.code))
                {
                    excelResult.status = false;
                    excelResult.message += " abone numarası alanı zorunludur";
                }

                if (string.IsNullOrEmpty(excelData.subscriber))
                {
                    excelResult.status = false;
                    excelResult.message += " abone adı zorunludur";
                }



                if (excelResult.status)
                {
                    var trans = db.BeginTransaction();
                    var res = new ResultStatus { result = true };
                    var uniqueColumnText = string.Format("{0}", excelData.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture);
                    var plan = stockTaskPlanDb.Where(a => string.Format("{0}", a.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture) == uniqueColumnText).FirstOrDefault();

                    var storage = db.GetCMP_StorageByCode(excelData.code);
                    if (storage != null)
                    {
                        excelResult.status = false;
                        excelResult.message += " abone numarası sistemde kayıtlı";
                    }

                    else
                    {
                        var cmp = companies.Where(x => x.name.ToLower(culture) == excelData.company.ToLower(culture)).FirstOrDefault();
                        if (cmp == null)
                        {
                            cmp = new CMP_Company
                            {
                                name = excelData.company,
                                created = DateTime.Now,
                                createdby = userStatus.user.id,
                                type = (Int16)EnumCMP_CompanyType.Benimisletmem
                            };
                            companies.Add(cmp);
                            res &= new VMCMP_CompanyModel().B_EntityDataCopyForMaterial(cmp).Save(userStatus.user.id, null, trans);

                        }
                        storage = new CMP_Storage
                        {
                            name = excelData.subscriber,
                            created = DateTime.Now,
                            createdby = userStatus.user.id,
                            address = excelData.address,
                            code = excelData.code,
                            companyId = cmp.id
                        };

                        if (!String.IsNullOrEmpty(excelData.city))
                        {
                            var city = locations.Where(x => x.type == (int)EnumUT_LocationType.Sehir && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelData.city.ToLower()).FirstOrDefault();

                            if (city != null)
                            {
                                storage.locationId = city.id;

                                if (!String.IsNullOrEmpty(excelData.town))
                                {
                                    var town = locations.Where(x => x.type == (int)EnumUT_LocationType.İlce && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelData.town.ToLower()).ToArray();

                                    if (town.Count() > 0)
                                    {
                                        storage.locationId = town.Where(a => a.pid == city.id).Select(a => a.id).FirstOrDefault();
                                    }
                                }
                            }
                        }

                        res &= db.InsertCMP_Storage(storage, trans);
                    }

                    var productControl = products.Where(x => x.name.ToLower(culture) == excelData.deviceName.ToLower(culture)).FirstOrDefault();
                    var inventoryId = Guid.NewGuid();
                    if (productControl == null)
                    {
                        var brandControl = brands.Where(x => x.name.ToLower(culture) == excelData.brand.ToLower(culture)).FirstOrDefault();
                        if (brandControl == null)
                        {
                            brandControl = new PRD_Brand
                            {
                                name = excelData.brand,
                                created = DateTime.Now,
                                createdby = userStatus.user.id
                            };
                            res &= db.InsertPRD_Brand(brandControl, trans);
                        }

                        productControl = new PRD_Product
                        {
                            stockType = (Int16)EnumPRD_ProductStockType.SeriNoluTakip,
                            brandId = brandControl.id,
                            name = excelData.deviceName,
                            code = BusinessExtensions.B_GetIdCode(),
                            type = (Int16)EnumPRD_ProductType.Diger,
                            unitId = db.GetUT_UnitByName("ADET")?.id
                        };

                        products.Add(productControl);

                        var product = new VMPRD_ProductModel().B_EntityDataCopyForMaterial(productControl);
                        res &= product.Save(userStatus.user.id, null, trans);


                    }

                    var TransactionId = Guid.NewGuid();
                    res &= new VMPRD_TransactionModel
                    {
                        id = TransactionId,
                        inputId = storage.id,
                        inputCompanyId = storage.companyId,
                        inputTable = "CMP_Storage",
                        created = DateTime.Now,
                        createdby = userStatus.user.id,
                        date = DateTime.Now,
                        type = (Int16)EnumPRD_TransactionType.AcilisFisi,
                        items = new List<VMPRD_TransactionItems>
                            {
                                new VMPRD_TransactionItems
                                {
                                    productId = productControl.id,
                                    serialCodes = excelData.serialNumber,
                                    created = DateTime.Now,
                                    createdby = userStatus.user.id,
                                    quantity = 1,
                                    transactionId = TransactionId
                                }
                            },
                        status = (Int16)EnumPRD_TransactionStatus.islendi,
                    }.Save(userStatus.user.id, trans, inventoryId);

                    if (plan != null)
                    {
                        plan.changed = DateTime.Now;
                        plan.changedby = userStatus.user.id;
                        plan.code = excelData.code;
                        plan.storageId = storage.id;
                        plan.inventoryId = inventoryId;
                        plan.taskId = null;
                        plan.status = (Int16)EnumPRD_StockTaskPlanStatus.Acilmadi;

                        res &= db.UpdatePRD_StockTaskPlan(plan, true, trans);
                    }
                    else
                    {
                        res &= db.InsertPRD_StockTaskPlan(new PRD_StockTaskPlan
                        {
                            inventoryId = inventoryId,
                            storageId = storage.id,
                            created = DateTime.Now,
                            createdby = userStatus.user.id,
                            status = (Int16)EnumPRD_StockTaskPlanStatus.Acilmadi
                        }, trans);
                    }
                    if (res.result)
                    {
                        trans.Commit();
                    }
                    else
                    {
                        trans.Rollback();
                        excelResult.status = false;
                        excelResult.message += res.message + " Bir sorun oluştu";
                    }

                }

                existError.Add(excelResult);
                excelResult = new ExcelResult
                {
                    status = true,
                    rowNumber = excelResult.rowNumber
                };


            }

            if (existError.Where(a => a.status == false).Count() == excelDatas.Length)
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

        [PageInfo("Abone Detayı", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult Detail(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_StockTaskPlanById(id);
            return View(data);
        }

        [PageInfo("Abone Ekleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult Insert()
        {
            var data = new VWPRD_StockTaskPlan { id = Guid.NewGuid() };
            return View(data);
        }

        [PageInfo("Abone Ekleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(PRD_StockTaskPlan item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertPRD_StockTaskPlan(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Abone yönetim görev ekleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult InsertTask()
        {
            var data = new WorkOfTimeDatabase().GetVWPRD_StockTaskPlan();
            return View(data);
        }

        [PageInfo("Abone yönetim görev ekleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult InsertTask(List<Guid> userIds, string subscriber, DateTime planStartDate, DateTime dueDate)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var subs = db.GetVWPRD_StockTaskPlanByStorageCodes(subscriber.Split(',').Select(a => a.ToString()).ToArray());

            if (subs.Count() == 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Girmiş olduğunuz abone numaraları ile kayıtlı abone bulunamadı.")
                }, JsonRequestBehavior.AllowGet);
            }

            var res = new ResultStatus { result = true };
            var trans = db.BeginTransaction();
            foreach (var sub in subs)
            {
                if (!sub.storageId.HasValue)
                {
                    continue;
                }
                var storage = db.GetCMP_StorageById(sub.storageId.Value);
                var taskId = Guid.NewGuid();
                sub.taskId = taskId;
                sub.status = (Int16)EnumPRD_StockTaskPlanStatus.Acildi;

                res &= db.UpdatePRD_StockTaskPlan(new PRD_StockTaskPlan().B_EntityDataCopyForMaterial(sub), true, trans);

                res &= new VMFTM_TaskModel
                {
                    id = taskId,
                    customerStorageId = storage.id,
                    customerId = storage.companyId,
                    type = (Int16)EnumFTM_TaskType.Soktak,
                    assignableUsers = userIds,
                    planStartDate = planStartDate,
                    dueDate = dueDate,
                    fixtureId = sub.inventoryId,
                    companyId = TenantConfig.Tenant.TenantCode == 1100 ? new Guid("101F88C2-DC52-4794-919B-F3B8207A68FE") : TenantConfig.Tenant.TenantCode == 1000 ? new Guid("101F88C2-DC52-4794-919B-F3B8207A68FE"): TenantConfig.Tenant.TenantCode == 1130 ? new Guid("EF98597D-9FDE-4BF9-A18B-2CEB1BFBBE96") : userStatus.user.CompanyId
                }.InsertAll(userStatus.user.id,Request, trans);

            }

            if (!res.result)
            {
                trans.Rollback();
            }
            else
            {
                trans.Commit();
            }

            var result = new ResultStatusUI
            {
                Result = res.result,
                FeedBack = res.result ? feedback.Success("Abonelere görev oluşturma işlemi başarılı") : feedback.Error("Abone görev oluşturma işlemi başarısız oldu.")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Abone Düzenleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPRD_StockTaskPlanById(id);
            return View(data);
        }

        [PageInfo("Abone Düzenleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(PRD_StockTaskPlan item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdatePRD_StockTaskPlan(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Call Center Outbound Metodu", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ViewResult SendData(string filter, string ids)
        {
            return View(new SendDataViewModel
            {
                filter = filter,
                ids = ids
            });
        }


        [HttpPost]
        [PageInfo("Veri Gönderme Metodu", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public JsonResult SendData([DataSourceRequest] DataSourceRequest request, string ids, DateTime planStartDate, DateTime dueDate, List<Guid> userIds)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedBack = new FeedBack();
            var condition = KendoToExpression.Convert(request);
            condition.Take = 99999;
            condition.Skip = 0;
            var datas = new List<VWPRD_StockTaskPlan>();

            if (string.IsNullOrEmpty(ids))
            {
                datas = db.GetVWPRD_StockTaskPlan(condition).ToList();
            }
            else
            {
                var dataIds = ids.Split(',').Select(a => Guid.Parse(a)).ToArray();
                datas = db.GetVWPRD_StockTaskPlanByIds(dataIds).ToList();
            }

            var res = new ResultStatus { result = true };
            var trans = db.BeginTransaction();
            foreach (var data in datas)
            {
                if (!data.storageId.HasValue)
                {
                    continue;
                }
                var storage = db.GetCMP_StorageById(data.storageId.Value);
                var taskId = Guid.NewGuid();
                data.taskId = taskId;
                data.status = (Int16)EnumPRD_StockTaskPlanStatus.Acildi;

                res &= db.UpdatePRD_StockTaskPlan(new PRD_StockTaskPlan().B_EntityDataCopyForMaterial(data), true, trans);

                res &= new VMFTM_TaskModel
                {
                    id = taskId,
                    customerStorageId = storage.id,
                    customerId = storage.companyId,
                    type = (Int16)EnumFTM_TaskType.Soktak,
                    assignableUsers = userIds,
                    planStartDate = planStartDate,
                    dueDate = dueDate,
                    fixtureId = data.inventoryId,
                    companyId = TenantConfig.Tenant.TenantCode == 1100 ? new Guid("101F88C2-DC52-4794-919B-F3B8207A68FE") : TenantConfig.Tenant.TenantCode == 1000 ? new Guid("101F88C2-DC52-4794-919B-F3B8207A68FE") : TenantConfig.Tenant.TenantCode == 1130 ? new Guid("EF98597D-9FDE-4BF9-A18B-2CEB1BFBBE96") : userStatus.user.CompanyId
                }.InsertAll(userStatus.user.id,Request, trans);

            }

            if (!res.result)
            {
                trans.Rollback();
            }
            else
            {
                trans.Commit();
            }

            return Json(new ResultStatusUI
            {
                Result = res.result,
                FeedBack = res.result ? feedBack.Success("Görev oluşturma işlemi başarılı bir şekilde gerçekleştirildi") : feedBack.Warning("Görev oluşturma işlemi başarısız oldu.")
            }, JsonRequestBehavior.AllowGet);
        }

    }

    public class SendDataViewModel
    {
        public string filter { get; set; }
        public string ids { get; set; }
    }
}
