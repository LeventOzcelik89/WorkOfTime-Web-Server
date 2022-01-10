using BotDetect;
using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Infoline.WorkOfTime.WebProject.Areas.PRD.Controllers
{
	public class VWPRD_ProductController : Controller
	{
		[PageInfo("Ürün&Hizmet Listesi", SHRoles.StokYoneticisi, SHRoles.UretimYonetici)]
		public ActionResult Index()
		{
			return View();
		}

		[PageInfo("Ürün Tanımları Grid Metodu", SHRoles.Personel, SHRoles.BayiPersoneli)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var page = request.Page;
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_Product(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWPRD_ProductCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Ürün Tanımları Dropdown Metodu", SHRoles.Personel, SHRoles.BayiPersoneli, SHRoles.CagriMerkezi)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWPRD_Product(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Ürün Tanımları Dropdown Metodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDownForInventory([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			var data = new List<VWPRD_Product>();
			var stockSummary = db.GetVWPRD_StockSummary(condition);
			if (stockSummary != null)
			{
				var productIds = stockSummary.Where(a => a.productId.HasValue && a.quantity > 0).Select(a => a.productId.Value).ToArray();
				var products = db.GetVWPRD_ProductByIds(productIds);
				if (products.Count() > 0)
				{
					data.AddRange(products.ToList());
				}
			}
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

	
		[PageInfo("Ürün Tanımları Adet Metodu", SHRoles.Personel, SHRoles.BayiPersoneli, SHRoles.CagriMerkezi)]
		public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var db = new WorkOfTimeDatabase();
			return db.GetVWPRD_ProductCount(condition.Filter);
		}

		[PageInfo("Ürün Tanımları Detayı", SHRoles.StokYoneticisi, SHRoles.UretimYonetici)]
		public ActionResult Detail(VMPRD_ProductModel item)
		{
			var data = item.Load();
			return View(data);
		}

		[PageInfo("Ürün Tanımı Ekleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public ActionResult Insert(VMPRD_ProductModel item)
		{
			var data = item.Load();
			if (data.created != null)
			{
				data.id = Guid.NewGuid();
				data.code = BusinessExtensions.B_GetIdCode();
				data.barcodeType = data.barcodeType ?? (int)EnumPRD_ProductbarcodeType.EAN13;
				data.name = data.name + " Kopya";
				data.type = data.type ?? (int)EnumPRD_ProductType.Hizmet;
				data.stockType = data.stockType ?? (int)EnumPRD_ProductStockType.Stoksuz;
			}
			return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Ürün Tanımı Ekleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaTalebi, SHRoles.SatinAlmaPersonel, SHRoles.SatisPersoneli)]
		public JsonResult Insert(VMPRD_ProductModel item, int[] status)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			item.status = status == null ? null : (short?)status.Sum();
			var dbresult = item.Save(userStatus.user.id, Request);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				Object = item.id,
				FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Warning("Kaydetme işlemi başarısız.Mesaj : " + dbresult.message)
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Ürün Tanımı Güncelleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaPersonel, SHRoles.SatinAlmaTalebi, SHRoles.SatisPersoneli)]
		public ActionResult Update(VMPRD_ProductModel item)
		{
			var data = item.Load();
			return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Ürün Tanımı Güncelleme", SHRoles.StokYoneticisi, SHRoles.DepoSorumlusu, SHRoles.SatinAlmaPersonel, SHRoles.SatinAlmaTalebi, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public JsonResult Update(VMPRD_ProductModel item, int[] status)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			item.status = status == null ? null : (short?)status.Sum();
			var dbresult = item.Save(userStatus.user.id, Request);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				Object = item.id,
				FeedBack = dbresult.result ? feedback.Success(dbresult.message, false, Request.UrlReferrer.AbsoluteUri) : feedback.Warning(dbresult.message)
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[PageInfo("Ürün Tanımı Silme", SHRoles.StokYoneticisi)]
		public JsonResult Delete(VMPRD_ProductModel item)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();
			var dbresult = item.Delete();
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
			};
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[PageInfo("Excel'den Ürün Ekleme", SHRoles.DepoSorumlusu, SHRoles.SatinAlmaPersonel, SHRoles.SatinAlmaTalebi, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public JsonResult Import(string model)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var rs = new ResultStatus { result = true };
			var feedback = new FeedBack();
			var culture = new System.Globalization.CultureInfo("tr-TR", false);

			var productsDB = db.GetPRD_Product().ToList();
			var brandsDB = db.GetPRD_Brand().ToList();
			var categoriesDB = db.GetPRD_Category().ToList();
			var unitsDB = db.GetUT_Unit().ToList();
			var currenciesDB = db.GetUT_Currency().ToList();
			var productPricesDB = db.GetPRD_ProductPrice();
			var productPointsDB = db.GetPRD_ProductPointSelling();

			var enumTypeArray = EnumsProperties.EnumToArrayGeneric<EnumPRD_ProductType>().ToArray();
			var enumStockTypeArray = EnumsProperties.EnumToArrayGeneric<EnumPRD_ProductStockType>().ToArray();
			var enumBarcodeTypeArray = EnumsProperties.EnumToArrayGeneric<EnumPRD_ProductbarcodeType>().ToArray();

			var products = Helper.Json.Deserialize<PRD_ProductExcel[]>(model);
			var uniqueColumn = ExcelHelper.GetColumnInfo(typeof(PRD_ProductExcel)).Where(a => a.Unique == true).Select(a => a.Name).FirstOrDefault();

			var currencyTL = currenciesDB.Where(a => a.code.Replace(" ", "").ToLower(culture) == "TL".ToLower(culture)).FirstOrDefault();

			var existError = new List<ExcelResult>();
			var excelResult = new ExcelResult
			{
				status = true,
				rowNumber = 0
			};

			foreach (var prod in products)
			{
				excelResult.rowNumber++;
				var _trans = db.BeginTransaction();
				var res = new ResultStatus { result = true };
				var nameControl = ProductBackendValidations.Name(prod.name);
				var codeControl = ProductBackendValidations.Code(prod.code);
				var unitControl = ProductBackendValidations.Unit(prod.unit);
				if (nameControl.IsError)
				{
					excelResult.status = false;
					excelResult.message += nameControl.data_error;
				}
				if (codeControl.IsError)
				{
					excelResult.status = false;
					excelResult.message += codeControl.data_error;
				}
				if (unitControl.IsError)
				{
					excelResult.status = false;
					excelResult.message += unitControl.data_error;
				}

				var uniqueColumnText = string.Format("{0}", prod.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture);
				var product = productsDB.Where(a => string.Format("{0}", a.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture) == uniqueColumnText).FirstOrDefault();

				var enumType = enumTypeArray.Where(a => a.Value.ToLower(culture).Contains(prod.type.ToLower(culture)) || a.Key.ToLower(culture).Contains(prod.type.ToLower(culture))).FirstOrDefault();
				if (enumType == null)
				{
					excelResult.status = false;
					excelResult.message += " Türü bölümünü örnek excel'de ki gösterilen şekilde doldurunuz.";
				}
				var enumStockType = enumStockTypeArray.Where(a => a.Value.ToLower(culture).Contains(prod.stockType.ToLower(culture)) || a.Key.ToLower(culture).Contains(prod.stockType.ToLower(culture))).FirstOrDefault();
				if (enumStockType == null)
				{
					excelResult.status = false;
					excelResult.message += " Stok Takip Tipi bölümünü örnek excel'de ki gösterilen şekilde doldurunuz.";
				}
				var enumBarcodeType = enumBarcodeTypeArray.Where(a => a.Value.ToLower(culture).Contains(prod.barcodeType.ToLower(culture)) || a.Key.ToLower(culture).Contains(prod.barcodeType.ToLower(culture))).FirstOrDefault();

				if (enumBarcodeType == null)
				{
					excelResult.status = false;
					excelResult.message += " Barkod Tipi bölümünü örnek excel'de ki gösterilen şekilde doldurunuz.";
				}

				if (product != null)
				{
					product.changed = DateTime.Now;
					product.changedby = userStatus.user.id;
				}
				else
				{
					product = new PRD_Product
					{
						id = Guid.NewGuid(),
						created = DateTime.Now,
						createdby = userStatus.user.id,
					};

					productsDB.Add(product);
				}

				product.name = prod.name;
				product.code = prod.code;
				product.barcode = prod.barcode;
				product.description = prod.description;
				product.type = enumType != null ? Convert.ToInt16(enumType.Key) : (short)EnumPRD_ProductType.Diger;
				product.stockType = enumStockType != null ? Convert.ToInt16(enumStockType.Key) : (short)EnumPRD_ProductStockType.NormalTakip;
				product.status = (int)EnumPRD_ProductStatus.satinalma + (int)EnumPRD_ProductStatus.satis;

				if (enumBarcodeType != null)
				{
					product.barcodeType = Convert.ToInt16(enumBarcodeType.Key);
				}

				if (!String.IsNullOrEmpty(prod.unit))
				{
					var isExistUnit = unitsDB.Where(a => a.name.Replace(" ", "").ToLower(culture) == prod.unit.Replace(" ", "").ToLower(culture)).FirstOrDefault();

					if (isExistUnit == null)
					{
						isExistUnit = new UT_Unit
						{
							id = Guid.NewGuid(),
							created = DateTime.Now,
							createdby = userStatus.user.id,
							name = prod.unit,
						};

						res &= db.InsertUT_Unit(isExistUnit, _trans);
						unitsDB.Add(isExistUnit);
					}

					product.unitId = isExistUnit.id;
				}
				else
				{
					product.unitId = unitsDB.Where(a => a.name.ToLower(culture) == "ADET".ToLower(culture)).Select(a => a.id).FirstOrDefault();
				}

				if (!String.IsNullOrEmpty(prod.category))
				{
					var isExistCategory = categoriesDB.Where(a => a.name.Replace(" ", "").ToLower(culture) == prod.category.Replace(" ", "").ToLower(culture)).FirstOrDefault();

					if (isExistCategory == null)
					{
						var isExistOther = categoriesDB.Where(a => a.name.Replace(" ", "").ToLower(culture) == "diğer").FirstOrDefault();

						if (isExistOther == null)
						{
							isExistOther = new PRD_Category
							{
								id = Guid.NewGuid(),
								created = DateTime.Now,
								createdby = userStatus.user.id,
								name = "Diğer"
							};

							res &= db.InsertPRD_Category(isExistOther, _trans);
							categoriesDB.Add(isExistOther);
						}

						isExistCategory = new PRD_Category
						{
							id = Guid.NewGuid(),
							created = DateTime.Now,
							createdby = userStatus.user.id,
							name = prod.category,
							pid = isExistOther.id
						};

						res &= db.InsertPRD_Category(isExistCategory, _trans);
						categoriesDB.Add(isExistCategory);
					}

					product.categoryId = isExistCategory.id;
				}

				if (!String.IsNullOrEmpty(prod.brand))
				{
					var isExistBrand = brandsDB.Where(a => a.name.Replace(" ", "").ToLower(culture) == prod.brand.Replace(" ", "").ToLower(culture)).FirstOrDefault();

					if (isExistBrand == null)
					{
						isExistBrand = new PRD_Brand
						{
							id = Guid.NewGuid(),
							created = DateTime.Now,
							createdby = userStatus.user.id,
							name = prod.brand,
						};

						res &= db.InsertPRD_Brand(isExistBrand, _trans);
						brandsDB.Add(isExistBrand);
					}

					product.brandId = isExistBrand.id;
				}

				if (prod.criticalStock != null)
				{
					product.isCriticalStock = true;
					product.criticalStock = prod.criticalStock;
				}
				else
				{
					product.isCriticalStock = false;
				}

				if (excelResult.status)
				{
					if (product.changedby.HasValue)
					{
						res &= db.UpdatePRD_Product(product, false, _trans);
					}
					else
					{
						res &= db.InsertPRD_Product(product, _trans);
					}
				}

				var currency = currenciesDB.Where(a => a.code.Replace(" ", "").ToLower(culture) == prod.currency.Replace(" ", "").ToLower(culture) || a.name.Replace(" ", "").ToLower(culture) == prod.currency.Replace(" ", "").ToLower(culture)).FirstOrDefault();

				if (prod.buyingPrice != null)
				{
					if (currency != null)
					{
						var dbPriceControl = productPricesDB.Where(a => a.price == prod.buyingPrice && a.type == (int)EnumPRD_ProductPriceType.Alis && a.currencyId == currency.id).FirstOrDefault();

						if (dbPriceControl == null)
						{
							var newProductPrice = new PRD_ProductPrice
							{
								id = Guid.NewGuid(),
								created = DateTime.Now,
								createdby = userStatus.user.id,
								price = prod.buyingPrice,
								productId = product.id,
								type = (int)EnumPRD_ProductPriceType.Alis,
								currencyId = currency.id
							};

							res &= db.InsertPRD_ProductPrice(newProductPrice, _trans);
						}
					}
					else
					{
						if (!product.changedby.HasValue)
						{
							var newProductPrice = new PRD_ProductPrice
							{
								id = Guid.NewGuid(),
								created = DateTime.Now,
								createdby = userStatus.user.id,
								price = prod.buyingPrice,
								productId = product.id,
								type = (int)EnumPRD_ProductPriceType.Alis,
								currencyId = currencyTL.id
							};

							res &= db.InsertPRD_ProductPrice(newProductPrice, _trans);
						}
					}
				}

				if (prod.sellingPrice != null)
				{
					if (currency != null)
					{
						var dbPriceControl = productPricesDB.Where(a => a.price == prod.sellingPrice && a.type == (int)EnumPRD_ProductPriceType.Satis && a.currencyId == currency.id).FirstOrDefault();

						if (dbPriceControl == null)
						{
							var newProductPrice = new PRD_ProductPrice
							{
								id = Guid.NewGuid(),
								created = DateTime.Now,
								createdby = userStatus.user.id,
								price = prod.sellingPrice,
								productId = product.id,
								type = (int)EnumPRD_ProductPriceType.Satis,
								currencyId = currency.id
							};

							res &= db.InsertPRD_ProductPrice(newProductPrice, _trans);
						}
					}
					else
					{
						if (!product.changedby.HasValue)
						{
							var newProductPrice = new PRD_ProductPrice
							{
								id = Guid.NewGuid(),
								created = DateTime.Now,
								createdby = userStatus.user.id,
								price = prod.sellingPrice,
								productId = product.id,
								type = (int)EnumPRD_ProductPriceType.Satis,
								currencyId = currencyTL.id
							};

							res &= db.InsertPRD_ProductPrice(newProductPrice, _trans);
						}
					}
				}

				if (res.result)
				{
					_trans.Commit();
				}
				else
				{
					_trans.Rollback();
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

			if (existError.Where(a => a.status == false).Count() == products.Length)
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

		[PageInfo("Ürün Stok Raporu", SHRoles.SistemYonetici, SHRoles.DepoSorumlusu)]
		public ActionResult StockReport()
		{
			var model = new StockReportModel().Load();
			return View(model);
		}
	}
}
