using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_InventoryController : Controller
	{
		[PageInfo("Envanter Listesi", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi)]
		public ActionResult Index()
		{
			return View();
		}

		[PageInfo("Bakım Envanterleri", SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator)]
		public ActionResult IndexMaintance(VWPRD_Inventory item)
		{
			item.code = item.code ?? BusinessExtensions.B_GetIdCode();
			return View(item);
		}

		[PageInfo("Bakım Envanteri Listesi", SHRoles.SahaGorevMusteri)]
		public ActionResult IndexCustomer(VWPRD_Inventory item)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var db = new WorkOfTimeDatabase();
			var model = new List<Guid?>() { userStatus.user.id };
			if (userStatus.user.CompanyId.HasValue)
			{
				model.Add(userStatus.user.CompanyId.Value);
				model.AddRange(db.GetCMP_StorageByCompanyId(userStatus.user.CompanyId.Value).Select(a => (Guid?)a.id).ToList());
			}
			return View(model);
		}

		[PageInfo("Envanterler DataSource", SHRoles.Personel, SHRoles.SahaGorevMusteri)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_Inventory(condition).ToDataSourceResult(request);
			data.Total = db.GetVWPRD_InventoryCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("IOT Kamera DataSource", SHRoles.Personel)]
		public ContentResult DataSourceCameraLog([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetIOT_CameraLog(condition).ToDataSourceResult(request);
			data.Total = db.GetIOT_CameraLogCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Envanterler DataSource Count", SHRoles.Personel)]
		public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			return db.GetVWPRD_InventoryCount(condition.Filter);
		}

		[PageInfo("Envanterler DataSourceDropDown", SHRoles.Personel, SHRoles.SahaGorevMusteri, SHRoles.BayiGorevPersoneli, SHRoles.YukleniciPersoneli)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_Inventory(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Envanter Detayı", SHRoles.DepoSorumlusu, SHRoles.StokYoneticisi, SHRoles.SahaGorevYonetici, SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevMusteri)]
		public ActionResult Detail(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_InventoryById(id);
			var additionalControl = db.GetVWSYS_TableAdditionalPropertyByDataIdAndDataTable(id, "PRD_Inventory");

			if (additionalControl.Count() > 0)
			{
				var model = new VMPRD_InventoryLog().B_EntityDataCopyForMaterial(data);
				var ipAdress = additionalControl.Where(a => a.propertyKey == "IpAdres").Select(a => a.propertyValue).FirstOrDefault();
				model.CameraLogs = db.GetIOT_CameraLogByIPAdress(ipAdress).OrderByDescending(a => a.created).ToArray();
				return PartialView("~/Areas/PRD/Views/VWPRD_Inventory/DetailIOT.cshtml", model);
			}

			return View(data);
		}

		[PageInfo("Bakım Envanter Ekleme", SHRoles.SahaGorevYonetici)]
		public ActionResult InsertMaintance(VWPRD_Inventory item)
		{
			item.code = item.code ?? BusinessExtensions.B_GetIdCode();
			return View(item);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Bakım Envanteri Ekleme", SHRoles.SahaGorevYonetici)]
		public JsonResult InsertMaintance(VWPRD_Inventory item, bool? Ispost)
		{
			var feedback = new FeedBack();
			if (item.serialcode == null)
			{
				return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Lütfen seri kodu giriniz.") }, JsonRequestBehavior.AllowGet);
			}

			var userStatus = (PageSecurity)Session["userStatus"];
			var db = new WorkOfTimeDatabase();
			var inventoriesDB = db.GetPRD_InventoriesByProductId((Guid)item.productId);
			var stroge = db.GetCMP_StorageById(item.firstActionDataId.Value) ?? new CMP_Storage();
			var serialCodes = (item.serialcode ?? "").Split(',');
			var actions = new List<PRD_InventoryAction>();
			var inventories = new List<PRD_Inventory>();
			var existSerialCode = new List<string>();
			foreach (var serialCode in serialCodes)
			{
				var inventory = inventoriesDB.Where(a => a.serialcode == serialCode).FirstOrDefault();
				if (inventory != null)
				{
					existSerialCode.Add(serialCode);
					continue;
				}

				inventory = new PRD_Inventory
				{
					created = DateTime.Now,
					createdby = userStatus.user.id,
					productId = item.productId,
					serialcode = serialCode,
					code = BusinessExtensions.B_GetIdCode(),
				};

				inventories.Add(inventory);

				var action = new List<PRD_InventoryAction>()
				{
					new PRD_InventoryAction {
						dataTable = "CMP_Company",
						dataId = item.firstActionDataId,
						inventoryId = inventory.id,
						created  = inventory.created,
						dataCompanyId = item.firstActionDataCompanyId,
						createdby = inventory.createdby,
						location = null,
						type = (int)EnumPRD_InventoryActionType.BakimEnvanteri
					},
					new PRD_InventoryAction {
						dataTable = "CMP_Storage",
						dataId = item.firstActionDataId,
						inventoryId = inventory.id,
						created  = DateTime.Now.AddMilliseconds(10),
						createdby = inventory.createdby,
						dataCompanyId = item.firstActionDataCompanyId,
						location = stroge?.location,
						type = (int)EnumPRD_InventoryActionType.Depoda
					},
				};

				actions.AddRange(action);
			}


			if (existSerialCode.Count() > 0)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning(string.Format("{0} serinumaraları sistemde zaten mevcut lütfen kontrol ediniz.", string.Join(",", existSerialCode)))
				}, JsonRequestBehavior.AllowGet);
			}


			var trans = db.BeginTransaction();
			var dbresult = db.BulkInsertPRD_Inventory(inventories, trans);
			dbresult &= db.BulkInsertPRD_InventoryAction(actions, trans);

			if (dbresult.result) { trans.Commit(); } else { trans.Rollback(); }

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Bakım envanteri girişi başarılı.") : feedback.Warning("Bakım envanteri girişi başarısız.")
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Bakım Envanteri Transfer", SHRoles.SahaGorevYonetici)]
		public ActionResult MoveMaintance(VWPRD_Inventory item)
		{
			var db = new WorkOfTimeDatabase();
			item = db.GetVWPRD_InventoryById(item.id);
			return View(item);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Bakım Envanteri Transfer", SHRoles.SahaGorevYonetici)]
		public JsonResult MoveMaintance(VWPRD_Inventory item, bool? Ispost)
		{
			var feedback = new FeedBack();
			if (item.firstActionDataId == null) return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("İşletme seçimi zorunludur.") }, JsonRequestBehavior.AllowGet);
			if (item.lastActionDataId == null) return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Depo/Şube seçimi zorunludur.") }, JsonRequestBehavior.AllowGet);

			var db = new WorkOfTimeDatabase();
			var inventory = db.GetVWPRD_InventoryById(item.id);
			if (inventory == null) return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Envanter seçimi zorunludur.") }, JsonRequestBehavior.AllowGet);

			var userStatus = (PageSecurity)Session["userStatus"];
			var updateAction = db.GetPRD_InventoryActionById(inventory.firstActionId.Value);

			if (updateAction != null)
			{
				updateAction.dataId = item.firstActionDataId;
			}


			var storage = db.GetCMP_StorageById(item.lastActionDataId.Value);
			var insertAction = new PRD_InventoryAction
			{
				dataTable = "CMP_Storage",
				dataId = item.lastActionDataId,
				inventoryId = item.id,
				created = DateTime.Now,
				createdby = userStatus.user.id,
				location = storage?.location,
				type = (int)EnumPRD_InventoryActionType.Depoda
			};

			var trans = db.BeginTransaction();
			var dbresult = db.InsertPRD_InventoryAction(insertAction, trans);
			dbresult &= db.UpdatePRD_InventoryAction(updateAction, true, trans);

			if (dbresult.result)
			{
				trans.Commit();
			}
			else
			{
				trans.Rollback();
			}

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Bakım envanteri transferi başarılı.") : feedback.Warning("Bakım envanteri transferi başarısız.")
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Envanter Düzenleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SahaGorevYonetici)]
		public ActionResult Update(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_InventoryById(id);
			return View(data);
		}

		[PageInfo("Envanter Düzenleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SahaGorevYonetici)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(PRD_Inventory item)
		{
			var feedback = new FeedBack();
			if (item.serialcode == null)
			{
				return Json(new ResultStatusUI { Result = false, FeedBack = feedback.Warning("Lütfen seri kodu giriniz.") }, JsonRequestBehavior.AllowGet);
			}
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			item.changed = DateTime.Now;
			item.changedby = userStatus.user.id;

			var existSerial = db.GetPRD_InventoryExistBySerialCode((Guid)item.productId, item.id, item.serialcode);
			if (existSerial)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning(item.serialcode + " seri numaralı envanter zaten mevcut.")
				}, JsonRequestBehavior.AllowGet);
			}

			var existCode = db.GetPRD_InventoryExistBySerialCode((Guid)item.productId, item.id, item.code);
			if (existCode)
			{
				return Json(new ResultStatusUI
				{
					Result = false,
					FeedBack = feedback.Warning(item.code + " kodlu envanter zaten mevcut.")
				}, JsonRequestBehavior.AllowGet);
			}


			var dbresult = db.UpdatePRD_Inventory(item);

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Warning("Güncelleme işlemi başarısız")
			}, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[PageInfo("Envanter Silme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SahaGorevYonetici)]
		public JsonResult Delete(Guid[] id)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();

			var errorList = new List<string>();
			var inventories = db.GetPRD_InventoryByIds(id);
			var tasks = db.GetFTM_TaskByFixtureIds(id);

			var resTrue = 0;
			foreach (var inventory in inventories)
			{
				var gorevadedi = tasks.Where(a => a.fixtureId == inventory.id).Count();
				if (gorevadedi > 0)
				{
					errorList.Add(inventory.serialcode);
					continue;
				}
				var inventoryAction = db.GetPRD_InventoryActionByInventoryId(inventory.id);
				if (inventoryAction.Count(a => a.transactionId != null) > 0)
				{
					errorList.Add(inventory.serialcode);
					continue;
				}

				var inventoryTask = db.GetPRD_InventoryTaskByInventoryId(inventory.id);
				var trans = db.BeginTransaction();
				var dbresult = db.BulkDeletePRD_InventoryTask(inventoryTask, trans);
				dbresult &= db.BulkDeletePRD_InventoryAction(inventoryAction, trans);
				dbresult &= db.DeletePRD_Inventory(inventory, trans);

				if (dbresult.result)
				{
					resTrue = resTrue + 1;
					trans.Commit();
				}
				else
				{
					errorList.Add(inventory.serialcode);
					trans.Rollback();
				}
			}

			var result = new ResultStatusUI
			{
				Result = errorList.Count() == 0,
				FeedBack = errorList.Count() == 0 ? feedback.Success("Silme işlemi başarılı") : feedback.Warning(string.Format("Silme işlemi başarısız {0} adet kayıt'tan {1} adedi silindi.", inventories.Count(), resTrue) + " Ürüne ait görev veya hareket olduğundan işlem gerçekleştirilemedi.")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[AllowEveryone]
		[PageInfo("Envanterlerin QR Kodlarının Yazdırılması", SHRoles.Personel)]
		public ActionResult PrintQrCodes([DataSourceRequest] DataSourceRequest request, int? type = 4, int? isLogo = 1)
		{
			var model = new List<VWPRD_Inventory>();
			try
			{
				var db = new WorkOfTimeDatabase();
				request.Page = 1;
				request.PageSize = int.MaxValue;
				var condition = KendoToExpression.Convert(request);
				model = db.GetVWPRD_Inventory(condition).ToList();
				ViewBag.type = type;
				ViewBag.logo = isLogo;
			}
			catch { }


			if (TenantConfig.Tenant.TenantCode == 1137)
			{
				if (type == 9)
				{
					return PartialView("~/Areas/PRD/Views/VWPRD_Inventory/Print/1137/PrintQrCodesV2.cshtml", model);
				}
				return PartialView("~/Areas/PRD/Views/VWPRD_Inventory/Print/1137/PrintQrCodes.cshtml", model);
			}
			else
			{
				return PartialView("~/Areas/PRD/Views/VWPRD_Inventory/Print/Default/PrintQrCodes.cshtml", model);
			}
		}

		[AllowEveryone]
		[PageInfo("Envanterlerin QR Kodlarının Yazdırılması Logo ve Boyutlu", SHRoles.Personel)]
		public ActionResult PrintQrCodesSizes([DataSourceRequest] DataSourceRequest request, int? height = 30, int? width = 50)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			VWPRD_Inventory[] model = new VWPRD_Inventory[0];

			try
			{
				var db = new WorkOfTimeDatabase();
				request.Page = 1;
				request.PageSize = int.MaxValue;
				var condition = KendoToExpression.Convert(request);
				model = db.GetVWPRD_Inventory(condition);
			}
			catch { }


			var data = new QrClass
			{
				height = height,
				width = width,
				invertorys = model.Select(x => new InventoryFilterClass
				{
					id = x.id,
					serialcode = x.serialcode,
					code = x.code,
					fullName = x.fullName,
					productId_Title = x.productId_Title
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
				return PartialView("~/Areas/PRD/Views/VWPRD_Inventory/Print/1137/PrintQrCodesSizes.cshtml", data);
			}
			else
			{
				return PartialView("~/Areas/PRD/Views/VWPRD_Inventory/Print/Default/PrintQrCodesSizes.cshtml", data);
			}
		}

		[AllowEveryone]
		[PageInfo("Envanterin QR Kodunu Yazdırılması Logo ve Boyutlu", SHRoles.Personel)]
		public ActionResult PrintQrCodeSize(Guid id, int? height = 30, int? width = 50)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			VWPRD_Inventory model = new VWPRD_Inventory();

			try
			{
				var db = new WorkOfTimeDatabase();
				model = db.GetVWPRD_InventoryById(id);
			}
			catch { }


			var data = new QrClass
			{
				height = height,
				width = width,
				invertory = new InventoryFilterClass()
				{
					id = model.id,
					serialcode = model.serialcode,
					code = model.productId_Title + " | " + model.serialcode + " | " + model.code,
					fullName = model.fullName,
					productId_Title = model.productId_Title
				}
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
				}
			}

			if (TenantConfig.Tenant.TenantCode == 1137)
			{
				return PartialView("~/Areas/PRD/Views/VWPRD_Inventory/Print/1137/PrintQrCodeSize.cshtml", data);
			}
			else
			{
				return PartialView("~/Areas/PRD/Views/VWPRD_Inventory/Print/Default/PrintQrCodeSize.cshtml", data);
			}
		}

		[PageInfo("Personel Envanter Özeti", SHRoles.Personel)]
		public ActionResult DetailInventory(Guid? productId, Guid? stockId)
		{
			var model = new DetailInventoryModel
			{
				ProductId = productId,
				StockId = stockId,
			};
			return View(model);
		}
	}
}
