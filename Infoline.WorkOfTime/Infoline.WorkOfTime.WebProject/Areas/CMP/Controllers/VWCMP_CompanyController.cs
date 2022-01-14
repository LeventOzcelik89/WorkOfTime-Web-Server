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
	public class VWCMP_CompanyController : Controller
	{
		[PageInfo("Firma&Cari Listesi", SHRoles.CRMYonetici, SHRoles.OnMuhasebe, SHRoles.ProjeYonetici, SHRoles.SahaGorevYonetici, SHRoles.StokYoneticisi)]
		public ActionResult Index()
		{
			var db = new WorkOfTimeDatabase();

			VMCMP_CompanyModel model = new VMCMP_CompanyModel
			{
				CMP_Types = db.GetCMP_Types()
			};

			return View(model);
		}

		[PageInfo("Şirket Bilgileri", SHRoles.IKYonetici, SHRoles.IdariPersonelYonetici, SHRoles.OnMuhasebe)]
		public ActionResult IndexMy()
		{
			return View();
		}

		[PageInfo("Müşterileri Firmalarım", SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public ActionResult IndexMyCustomer()
		{
			return View();
		}

		[PageInfo("Tüm Müşteri Firmaları", SHRoles.CRMYonetici)]
		public ActionResult IndexCustomer()
		{
			return View();
		}

		[PageInfo("Tüm Cari Listesi", SHRoles.Personel, SHRoles.BayiPersoneli)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			request.Filters = new FilterDescriptor[0];
			request.Sorts = new SortDescriptor[0];
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWCMP_Company(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWCMP_CompanyCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Firma&Cari Listesi Dropdown Verileri", SHRoles.Personel, SHRoles.BayiPersoneli,SHRoles.BayiGorevPersoneli)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var userStatus = (PageSecurity)Session["userStatus"];
			var db = new WorkOfTimeDatabase();
			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.BayiPersoneli)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.BayiGorevPersoneli)))
			{
				condition = UpdateQuery(condition, userStatus);
			}
			condition = new VMFTM_TaskModel().UpdateQuery(condition, userStatus,2);
			var data = db.GetVWCMP_Company(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}
		[PageInfo("Firma&Cari Listesi Dropdown Verileri", SHRoles.Personel, SHRoles.BayiPersoneli, SHRoles.BayiGorevPersoneli)]
		public ContentResult DataSourceDropDownBayi([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var userStatus = (PageSecurity)Session["userStatus"];
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWCMP_Company(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Firma&Cari Listesi Bayi Personeli Dropdown Verileri", SHRoles.Personel, SHRoles.BayiPersoneli)]
		public ContentResult DataSourceDropDownSpesific([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);
			var userStatus = (PageSecurity)Session["userStatus"];
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWCMP_Company(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Firma&Cari Detay Sayfası", SHRoles.Personel)]
		public ActionResult Detail(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var model = new VMCMP_CompanyModel { id = id }.Load();
			model.VWPA_Accounts = db.GetVWPA_AccountsByDataIdDataTable(id, "CMP_Company");
			var _presentation = db.GetCRM_PresentationByCompanyId(id);
			var _crmContact = db.GetCRM_ContactByPresentationIds(_presentation.Select(c => c.id).ToArray());
			ViewBag.PresentationIds = _presentation.Select(c => c.id).ToArray();
			ViewBag.ContactIds = _crmContact.Select(c => c.id).ToArray();

			var crump = "<ol class=\"breadcrumb\">";
			var crum = BreadCrumps(true, id, model.name).Substring(1).Split('#').Reverse();
			model.breadCrumps = crump + string.Join("", crum) + "<ol>";

			return View(model);
		}

		[PageInfo("Firma&Cari Ekleme Sayfası", SHRoles.Personel, SHRoles.BayiPersoneli, SHRoles.CagriMerkezi)]
		public ActionResult Insert(VMCMP_CompanyModel data)
		{
			return View(data.Load());
		}

		[PageInfo("Firma&Cari Ekleme Sayfası", SHRoles.Personel, SHRoles.BayiPersoneli, SHRoles.CagriMerkezi)]
		[HttpPost]
		public JsonResult Insert(VMCMP_CompanyModel item, bool? isPost)
		{
			var db = new WorkOfTimeDatabase();
			var feedback = new FeedBack();
			var userStatus = (PageSecurity)Session["userStatus"];
			var trans = db.BeginTransaction();

			var dbresult = item.Save(userStatus.user.id, Request, trans);

			dbresult &= db.InsertCMP_CompanyRelation(new CMP_CompanyRelation
			{
				created = DateTime.Now,
				createdby = userStatus.user.id,
				companyId = userStatus.user.CompanyId,
				customerId = item.id
			}, trans);

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
				Object = dbresult.result ? db.GetVWCMP_StorageByCompanyId(item.id).RemoveGeographies() : new VWCMP_Storage[0],
				FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Firma&Cari Güncelleme Sayfası", SHRoles.Personel, SHRoles.BayiPersoneli, SHRoles.CagriMerkezi)]
		public ActionResult Update(VMCMP_CompanyModel item)
		{
			item.Load();
			return View(item);
		}

		[PageInfo("Firma&Cari Güncelleme Sayfası", SHRoles.Personel, SHRoles.BayiPersoneli, SHRoles.CagriMerkezi)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Update(VMCMP_CompanyModel item, EnumCMP_CompanyType[] type)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			item.types = type;
			var dbresult = item.Save(userStatus.user.id, Request);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				Object = item.id,
				FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		[PageInfo("Firma&Cari Silme İşlemi", SHRoles.CRMYonetici)]
		public JsonResult Delete(Guid id)
		{
			var feedback = new FeedBack();
			var dbresult = new VMCMP_CompanyModel { id = id }.Delete();

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning(dbresult.message)
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Firma Lokasyonu Güncelleme Methodu", SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public ContentResult UpdateCompanyLocation(Guid companyId, string location)
		{

			var db = new WorkOfTimeDatabase();
			var company = db.GetCMP_CompanyById(companyId);

			if (company == null)
			{
				return Content(
					Infoline.Helper.Json.Serialize(
						new ResultStatus
						{
							result = false,
							objects = new FeedBack().Warning("Firma Bulunamadı !... Lütfen sayfayı yenileyip tekrar deneyin...")
						}), "application/json"
					);
			}

			var geom = new NetTopologySuite.IO.WKTReader().Read(location);
			if (geom == null)
			{
				return Content(Helper.Json.Serialize(
						new ResultStatus
						{
							result = false,
							objects = new FeedBack().Warning("Geçersiz Konum Bilgisi !... Lütfen sayfayı yenileyip tekrar deneyin...")
						}), "application/json"
					);
			}

			var town = db.GetVWUT_LocationGetByPoint(location);
			if (town != null)
			{
				company.openAddressLocationId = town.id;
			}
			company.location = geom;

			var dbresult = db.UpdateCMP_Company(company);

			return Content(Helper.Json.Serialize(
				 new ResultStatus
				 {
					 result = dbresult.result,
					 objects =
						 dbresult.result == true
							 ? new FeedBack().Success("Müşteri Firma konumu başarıyla güncellendi.")
							 : new FeedBack().Error(dbresult.message)
				 }), "application/json"
			 );
		}

		[PageInfo("Diger Firmalar", SHRoles.Personel)]
		public ContentResult GetOtherCompaniesForDropDown()
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWCMP_CompanyOtherCompanies();
			return Content(Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Şirket Yönetmelikleri", SHRoles.Personel)]
		public ActionResult PersonnelRegulationDocument()
		{
			return View();
		}

		[AllowEveryone]
		[PageInfo("Şirket QR Kodlarının Yazdırılması")]
		public ActionResult PrintQrCodes([DataSourceRequest] DataSourceRequest request, int? type = 4)
		{
			VWCMP_Company[] model = new VWCMP_Company[0];
			try
			{
				var db = new WorkOfTimeDatabase();
				request.Page = 1;
				request.PageSize = int.MaxValue;
				var condition = KendoToExpression.Convert(request);
				model = db.GetVWCMP_Company(condition);
				ViewBag.type = type;
			}
			catch { }
			return View(model);
		}

		[HttpPost]
		[PageInfo("Excel'den Cari Ekleme", SHRoles.IKYonetici)]
		public JsonResult Import(string model)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var culture = new System.Globalization.CultureInfo("tr-TR", false);

			var currencies = db.GetUT_Currency();
			var companiesDB = db.GetCMP_Company().ToList();
			var additionalPropertiesDB = db.GetSYS_TableAdditionalPropertyByDataTable("CMP_Company");
			var locations = db.GetUT_LocationCityAndTownInTR();
			var TL = currencies.Where(a => a.code == "TL").FirstOrDefault();

			var excelCompanies = Helper.Json.Deserialize<CMP_CompanyExcel[]>(model);

			var additionalKeyValuePairs = Helper.Json.Deserialize<List<Dictionary<string, string>>>(model);
			var additionalProperties = new string[] { };

			if (TenantConfig.Tenant.Config.CustomProperty.ContainsKey("CMP_Company"))
			{
				additionalProperties = TenantConfig.Tenant.Config.CustomProperty["CMP_Company"];
			}

			var uniqueColumn = ExcelHelper.GetColumnInfo(typeof(CMP_CompanyExcel)).Where(a => a.Unique == true).Select(a => a.Name).FirstOrDefault();

			var existError = new List<ExcelResult>();
			var excelResult = new ExcelResult
			{
				status = true,
				rowNumber = 0
			};

			foreach (var excelCmp in excelCompanies.Select((company, index) => new { company = company, index = index }))
			{
				excelResult.rowNumber++;

				var excelCompany = excelCmp.company;

				if (String.IsNullOrEmpty(excelCompany.name))
				{
					excelResult.status = false;
					excelResult.message += " İşletme adı alanı zorunludur";
				}

				if (excelResult.status)
				{
					var trans = db.BeginTransaction();
					var res = new ResultStatus { result = true };

					var uniqueColumnText = string.Format("{0}", excelCompany.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture);
					var company = companiesDB.Where(a => string.Format("{0}", a.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture) == uniqueColumnText).FirstOrDefault();

					if (company != null && company.type == (int)EnumCMP_CompanyType.Benimisletmem)
					{
						company.changed = DateTime.Now;
						company.changedby = userStatus.user.id;
					}
					else
					{
						company = new CMP_Company
						{
							id = Guid.NewGuid(),
							created = DateTime.Now,
							createdby = userStatus.user.id,
							type = (int)EnumCMP_CompanyType.Diger,
							code = String.IsNullOrEmpty(excelCompany.code) ? BusinessExtensions.B_GetIdCode() : excelCompany.code,
							taxType = (int)EnumCMP_CompanyTaxType.TuzelKisi
						};

						companiesDB.Add(company);
					}

					if (!string.IsNullOrEmpty(excelCompany.commercialTitle))
					{
						company.commercialTitle = excelCompany.commercialTitle;
					}

					if (!string.IsNullOrEmpty(excelCompany.email))
					{
						company.email = excelCompany.email;
					}

					if (!string.IsNullOrEmpty(excelCompany.fax))
					{
						company.fax = excelCompany.fax;
					}

					if (!string.IsNullOrEmpty(excelCompany.name))
					{
						company.name = excelCompany.name;
					}

					if (!string.IsNullOrEmpty(excelCompany.phone))
					{
						company.phone = excelCompany.phone;
					}

					if (!string.IsNullOrEmpty(excelCompany.taxNumber))
					{
						company.taxNumber = excelCompany.taxNumber;
					}

					if (!string.IsNullOrEmpty(excelCompany.taxOffice))
					{
						company.taxOffice = excelCompany.taxOffice;
					}

					if (!string.IsNullOrEmpty(excelCompany.openAddress))
					{
						company.openAddress = excelCompany.openAddress;
					}

					if (!string.IsNullOrEmpty(excelCompany.invoiceAddress))
					{
						company.invoiceAddress = excelCompany.invoiceAddress;
					}

					if (!string.IsNullOrEmpty(excelCompany.mersisNo))
					{
						company.mersisNo = excelCompany.mersisNo;
					}

					if (!string.IsNullOrEmpty(excelCompany.location))
					{
						try
						{
							var wkt = string.Format("POINT ({0})", string.Join(" ", excelCompany.location.Split(',').Select(a => a.Trim()).Reverse()));
							company.location = new NetTopologySuite.IO.WKTReader().Read(wkt);
						}
						catch { }
					}

					if (!String.IsNullOrEmpty(excelCompany.city))
					{
						var city = locations.Where(x => x.type == (int)EnumUT_LocationType.Sehir && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelCompany.city.ToLower()).FirstOrDefault();

						if (city != null)
						{
							company.openAddressLocationId = city.id;
							company.invoiceAddressLocationId = city.id;

							if (!String.IsNullOrEmpty(excelCompany.town))
							{
								var town = locations.Where(x => x.type == (int)EnumUT_LocationType.İlce && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelCompany.town.ToLower()).ToArray();

								if (town.Count() > 0)
								{
									company.openAddressLocationId = town.Where(a => a.pid == city.id).Select(a => a.id).FirstOrDefault();
									company.invoiceAddressLocationId = town.Where(a => a.pid == city.id).Select(a => a.id).FirstOrDefault();
								}
							}
						}
					}

					if (company.changed.HasValue)
					{
						res &= db.UpdateCMP_Company(company, false, trans);
					}
					else
					{
						res &= db.InsertCMP_Company(company, trans);

						res &= db.InsertCMP_Storage(new CMP_Storage
						{
							id = Guid.NewGuid(),
							created = DateTime.Now,
							createdby = userStatus.user.id,
							code = BusinessExtensions.B_GetIdCode(),
							name = "Ana Şube/Depo/Kısım",
							locationId = company.openAddressLocationId,
							companyId = company.id,
							address = company.openAddress
						}, trans);

						res &= db.InsertPA_Account(new PA_Account()
						{
							created = DateTime.Now,
							createdby = userStatus.user.id,
							code = BusinessExtensions.B_GetIdCode(),
							currencyId = TL.id,
							isDefault = true,
							name = "Ana Hesap",
							status = true,
							type = (int)EnumPA_AccountType.Kasa,
							dataId = company.id,
							dataTable = "CMP_Company"
						}, trans);
					}

					foreach (var addition in additionalProperties)
					{
						if (!string.IsNullOrEmpty(additionalKeyValuePairs[excelCmp.index][addition]) && res.result)
						{
							var controlAddition = additionalPropertiesDB.Where(a => a.propertyKey == addition && a.dataId == company.id && a.dataTable == "CMP_Company").FirstOrDefault();

							if (controlAddition == null)
							{
								res &= db.InsertSYS_TableAdditionalProperty(new SYS_TableAdditionalProperty
								{
									id = Guid.NewGuid(),
									created = DateTime.Now,
									createdby = userStatus.user.id,
									dataId = company.id,
									dataTable = "CMP_Company",
									propertyKey = addition,
									propertyValue = additionalKeyValuePairs[excelCmp.index][addition]
								}, trans);
							}

							else
							{
								controlAddition.propertyValue = additionalKeyValuePairs[excelCmp.index][addition];
								res &= db.UpdateSYS_TableAdditionalProperty(controlAddition, true, trans);
							}
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
				}

				existError.Add(excelResult);
				excelResult = new ExcelResult
				{
					status = true,
					rowNumber = excelResult.rowNumber
				};
			}

			if (existError.Where(a => a.status == false).Count() == excelCompanies.Length)
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

		[HttpPost]
		[PageInfo("Excel'den Diğer İşletmeleri Ekleme", SHRoles.CRMYonetici, SHRoles.SatisPersoneli)]
		public JsonResult ImportOther(string model)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();
			var culture = new System.Globalization.CultureInfo("tr-TR", false);

			var currencies = db.GetUT_Currency();
			var companiesDB = db.GetCMP_Company().ToList();
			var additionalPropertiesDB = db.GetSYS_TableAdditionalPropertyByDataTable("CMP_Company");
			var locations = db.GetUT_LocationCityAndTownInTR();
			var TL = currencies.Where(a => a.code == "TL").FirstOrDefault();

			var excelCompanies = Helper.Json.Deserialize<CMP_OtherCompanyExcel[]>(model);

			var additionalKeyValuePairs = Helper.Json.Deserialize<List<Dictionary<string, string>>>(model);
			var additionalProperties = new string[] { };

			var uniqueColumn = ExcelHelper.GetColumnInfo(typeof(CMP_OtherCompanyExcel)).Where(a => a.Unique == true).Select(a => a.Name).FirstOrDefault();

			if (TenantConfig.Tenant.Config.CustomProperty.ContainsKey("CMP_Company"))
			{
				additionalProperties = TenantConfig.Tenant.Config.CustomProperty["CMP_Company"];
			}

			var existError = new List<ExcelResult>();
			var row = 0;

			foreach (var excelCmp in excelCompanies.Select((company, index) => new { company = company, index = index }))
			{
				row++;

				var excelCompany = excelCmp.company;

				var errors = new List<string>();

				var nameValidation = CustomerBackendValidations.Name(excelCompany.name);

				if (nameValidation.IsError)
				{
					errors.Add(nameValidation.data_error);
				}



				var isExistCompanyName = companiesDB.Where(x => x.name == excelCmp.company.name).FirstOrDefault();
				if (isExistCompanyName != null)
				{
					errors.Add("Aynı işletme adı ile kayıt mevcut");
				}


				var trans = db.BeginTransaction();
				var res = new ResultStatus { result = true };

				if (excelCompany.phone.Contains("+90") || excelCompany.phone.Contains("+9"))
				{
					excelCompany.phone = excelCompany.phone.Replace("+90", "0");
					excelCompany.phone = excelCompany.phone.Replace("+9", "0");
				}

				if (!string.IsNullOrEmpty(excelCompany.phone) && excelCompany.phone.StartsWith("0") == false && excelCompany.phone.Length == 10)
				{
					excelCompany.phone = "0" + excelCompany.phone;
				}

				var uniqueColumnText = string.Format("{0}", excelCompany.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture);
				var company = companiesDB.Where(a => string.Format("{0}", a.GetPropertyValue(uniqueColumn)).Trim().ToLower(culture) == uniqueColumnText).FirstOrDefault();

				var data = new VMCMP_CompanyModel();
				data.code = excelCompany.code;

				data.Load();

				if (company != null && company.type == (int)EnumCMP_CompanyType.Diger)
				{
					data.B_EntityDataCopyForMaterial(excelCompany, true);
				}
				else
				{
					data.id = Guid.NewGuid();
					data.created = DateTime.Now;
					data.createdby = userStatus.user.id;
					data.type = (int)EnumCMP_CompanyType.Diger;
					data.code = String.IsNullOrEmpty(excelCompany.code) ? BusinessExtensions.B_GetIdCode() : excelCompany.code;
				}

				data.commercialTitle = excelCompany.commercialTitle;
				data.email = excelCompany.email;
				data.fax = excelCompany.fax;
				data.name = excelCompany.name;
				data.phone = excelCompany.phone;
				data.taxNumber = excelCompany.taxNumber;
				data.taxOffice = excelCompany.taxOffice;
				data.openAddress = excelCompany.openAddress;
				data.invoiceAddress = excelCompany.invoiceAddress;

				if (!String.IsNullOrEmpty(excelCompany.city))
				{
					var city = locations.Where(x => x.type == (int)EnumUT_LocationType.Sehir && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelCompany.city.ToLower()).FirstOrDefault();

					if (city != null)
					{
						data.openAddressLocationId = city.id;
						data.invoiceAddressLocationId = city.id;

						if (!String.IsNullOrEmpty(excelCompany.town))
						{
							var town = locations.Where(x => x.type == (int)EnumUT_LocationType.İlce && !String.IsNullOrEmpty(x.name) && x.name.ToLower() == excelCompany.town.ToLower()).ToArray();

							if (town.Count() > 0)
							{
								data.openAddressLocationId = town.Where(a => a.pid == city.id).Select(a => a.id).FirstOrDefault();
								data.invoiceAddressLocationId = town.Where(a => a.pid == city.id).Select(a => a.id).FirstOrDefault();
							}
						}
					}
				}


				if (errors.Count == 0)
				{
					data.Save(userStatus.user.id);

					if (res.result)
					{
						trans.Commit();
					}
					else
					{
						trans.Rollback();
						errors.Add(res.message);
					}


					if (!String.IsNullOrEmpty(excelCompany.responsible1_Title))
					{
						var personSplit1 = excelCompany.responsible1_Title.Split(' ');

						new VMSH_UserModel
						{
							created = DateTime.Now,
							createdby = userStatus.user.id,
							loginname = excelCompany.responsible_1Email,
							firstname = string.Join(" ", personSplit1.Take(personSplit1.Length - 1)),
							lastname = personSplit1[personSplit1.Length - 1],
							email = excelCompany.responsible_1Email,
							phone = excelCompany.responsible1_Phone,
							CompanyId = data.id
						}.Save(trans);
					}

					if (!String.IsNullOrEmpty(excelCompany.responsible2_Title))
					{
						var personSplit2 = excelCompany.responsible2_Title.Split(' ');

						new VMSH_UserModel
						{
							created = DateTime.Now,
							createdby = userStatus.user.id,
							loginname = excelCompany.responsible_2Email,
							firstname = string.Join(" ", personSplit2.Take(personSplit2.Length - 1)),
							lastname = personSplit2[personSplit2.Length - 1],
							email = excelCompany.responsible_2Email,
							phone = excelCompany.responsible2_Phone,
							CompanyId = data.id,
						}.Save(trans);
					}

					foreach (var addition in additionalProperties)
					{
						if (!string.IsNullOrEmpty(additionalKeyValuePairs[excelCmp.index][addition]) && res.result)
						{
							var controlAddition = additionalPropertiesDB.Where(a => a.propertyKey == addition && a.dataId == company.id && a.dataTable == "CMP_Company").FirstOrDefault();

							if (controlAddition == null)
							{
								res &= db.InsertSYS_TableAdditionalProperty(new SYS_TableAdditionalProperty
								{
									id = Guid.NewGuid(),
									created = DateTime.Now,
									createdby = userStatus.user.id,
									dataId = data.id,
									dataTable = "CMP_Company",
									propertyKey = addition,
									propertyValue = additionalKeyValuePairs[excelCmp.index][addition]
								}, trans);
							}

							else
							{
								controlAddition.propertyValue = additionalKeyValuePairs[excelCmp.index][addition];
								res &= db.UpdateSYS_TableAdditionalProperty(controlAddition, true, trans);
							}
						}
					}
				}




				existError.Add(new ExcelResult
				{
					status = errors.Count() == 0,
					rowNumber = row,
					message = errors.Count() == 0 ? null : string.Join(",", errors)
				});

			}

			if (existError.Where(a => a.status == false).Count() == excelCompanies.Length)
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

		public string BreadCrumps(bool first = false, Guid? pid = null, string name = "")
		{
			var text = "#<li class=\"" + (first ? "active" : "") + "\" style=\"" + (first ? "font-weight:bold;" : "") + "\">";
			text += string.Format("<a href=\"/CMP/VWCMP_Company/Detail?id={0}\"> {1}</a>", pid, name);
			text += "</li>";
			if (pid.HasValue)
			{
				var db = new WorkOfTimeDatabase();
				var data = db.GetVWCMP_CompanyById(pid.Value);
                if (data!=null)
                {
					if (data.pid.HasValue)
					{
						text += BreadCrumps(false, data.pid, data.pid_Title);
					}
				}
				
			}
			return text;
		}

		public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
		{
			var company = new WorkOfTimeDatabase().GetVWCMP_CompanyById(userStatus.user.CompanyId.Value);

			BEXP filter = null;

			if (!String.IsNullOrEmpty(company.customerIds))
			{
				foreach (var customer in company.customerIds.Split(',').ToArray())
				{
					filter |= new BEXP
					{
						Operand1 = (COL)"id",
						Operator = BinaryOperator.Equal,
						Operand2 = (VAL)customer
					};
				}
			}
			else
			{
				filter |= new BEXP
				{
					Operand1 = (COL)"id",
					Operator = BinaryOperator.Equal,
					Operand2 = (VAL)Guid.NewGuid()
				};
			}
			query.Filter &= filter;
			return query;
		}
	}
}
