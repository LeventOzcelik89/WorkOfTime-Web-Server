using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.Infrastructure.Implementation;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CMP.Controllers
{
	public class VWCMP_OrderController : Controller
	{
		[PageInfo("Satış Siparişleri", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.MuhasebeSatis, SHRoles.SatisFatura, SHRoles.DepoSorumlusu, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli)]
		public ActionResult IndexSelling()
		{
			return View();
		}

		[PageInfo("Satış Sipariş Raporu", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.MuhasebeSatis, SHRoles.SatisFatura, SHRoles.DepoSorumlusu, SHRoles.CRMYonetici)]
		public ActionResult ReportSelling(string orderIds)
		{
			var orderids = orderIds.Split(',').Select(a => a.B_ToGuid()).Where(a => a.HasValue).Select(a => a.Value).ToArray();
			var db = new WorkOfTimeDatabase();
			var list = new List<VMCMP_OrderModels>();
			foreach (var orderid in orderids)
			{
				var orderModels = new VMCMP_OrderModels { id = orderid }.Load(false);
				list.Add(orderModels);
			}
			return View(list);
		}


		[PageInfo("Toplu Sipariş Onay", SHRoles.SatisPersoneli, SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public JsonResult BulkConfirm(string orderIds)
		{
			var orderids = orderIds.Split(',').Select(a => a.B_ToGuid()).Where(a => a.HasValue).Select(a => a.Value).ToArray();
			var feedback = new FeedBack();
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var list = new List<ResultStatus>();
			foreach (var orderid in orderids)
			{
				try
				{
					var orderModel = new VMCMP_OrderModels { id = orderid }.Load(false);
					if (orderModel.status == (int)EnumCMP_OrderStatus.CevapBekleniyor)
					{
						list.Add(orderModel.UpdateStatus((int)EnumCMP_OrderStatus.Onay, userStatus.user.id));
					}
				}
				catch
				{
					list.Add(new ResultStatus { result = false, message = "Sipariş onaylanırken sorun oluştu." });
				}

			}

			return Json(new ResultStatusUI
			{
				Result = true,
				FeedBack = feedback.Success("Sipariş onaylama işlemi başarılı", false, Url.Action("IndexSelling", "VWCMP_Order", new { area = "CMP" }))
			}, JsonRequestBehavior.AllowGet);
		}


		[PageInfo("Siparişler Metodu", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.MuhasebeSatis, SHRoles.SatisFatura, SHRoles.DepoSorumlusu, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli)]
		public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
		{
			request = UpdateRequest(request);
			var condition = KendoToExpression.Convert(request);
			request.Page = 1;
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CRMBayiPersoneli)))
			{
				condition = UpdateQuery(condition, userStatus);
			}

			var data = db.GetVWCMP_Order(condition).RemoveGeographies().ToDataSourceResult(request);
			data.Total = db.GetVWCMP_OrderCount(condition.Filter);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Siparişler Adet Metodu", SHRoles.Personel, SHRoles.CRMBayiPersoneli)]
		public int DataSourceCount([DataSourceRequest] DataSourceRequest request)
		{
			request = UpdateRequest(request);
			var condition = KendoToExpression.Convert(request);
			var userStatus = (PageSecurity)Session["userStatus"];
			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.CRMBayiPersoneli)))
			{
				condition = UpdateQuery(condition, userStatus);
			}
			var db = new WorkOfTimeDatabase();
			var total = db.GetVWCMP_OrderCount(condition.Filter);
			return total;
		}

		[PageInfo("Siparişler Veri Metodu", SHRoles.Personel)]
		public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
		{
			var condition = KendoToExpression.Convert(request);

			var db = new WorkOfTimeDatabase();
			var data = db.GetVWCMP_Order(condition);
			return Content(Infoline.Helper.Json.Serialize(data), "application/json");
		}

		[PageInfo("Satış Sipariş Detayı", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.MuhasebeSatis, SHRoles.SatisFatura, SHRoles.DepoSorumlusu, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli)]
		public ActionResult DetailSelling(Guid id)
		{
			var data = new VMCMP_OrderModels { id = id }.Load(false);
			ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
			return View(data);
		}

		[PageInfo("Satış Siparişi Ekleme", SHRoles.SatisPersoneli, SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli)]
		public ActionResult InsertSelling(VMCMP_OrderModels item, bool? transform = false)
		{
			item.Load(transform);
			if (item.presentationId.HasValue)
			{
				var userStatus = (PageSecurity)Session["userStatus"];
				item.supplierId = userStatus.user.CompanyId.HasValue ? userStatus.user.CompanyId.Value : item.supplierId;
			}
			return View(item);
		}

		[PageInfo("Sipariş Ekleme Metodu", SHRoles.SatisPersoneli, SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli)]
		[HttpPost, ValidateAntiForgeryToken]
		public JsonResult Insert(VMCMP_OrderModels item)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			var dbresult = item.Save(userStatus.user.id, Request);

			return Json(new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Sipariş kaydetme işlemi başarılı", false) :
						   feedback.Warning("Sipariş kaydetme işlemi başarısız. Mesaj : " + dbresult.message)
			}, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Satış Siparişi Düzenleme", SHRoles.SatisPersoneli, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public ActionResult UpdateSelling(Guid id)
		{
			var data = new VMCMP_OrderModels { id = id }.Load(false);
			ViewBag.EnumProperties = EnumsProperties.EnumToArrayGeneric<EnumCMP_InvoiceActionType>().ToArray();
			return View(data);
		}

		[HttpPost, ValidateAntiForgeryToken]
		[PageInfo("Teklif Düzenleme Metodu", SHRoles.SatisPersoneli, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public JsonResult Update(VMCMP_OrderModels item)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			var dbresult = item.Save(userStatus.user.id, Request);

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Sipariş düzenleme işlemi başarılı", false) :
						   feedback.Warning("Sipariş düzenleme işlemi başarısız. Mesaj : " + dbresult.message)
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Sipariş Not Ekleme Metodu", SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.MuhasebeSatis, SHRoles.SatisFatura, SHRoles.DepoSorumlusu, SHRoles.CRMYonetici)]
		public ContentResult InsertNote(Guid orderId, string note)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			var dbres = new VMCMP_OrderModels { id = orderId }.Load(false).InsertNote(userStatus.user.id, note);

			return Content(Infoline.Helper.Json.Serialize(dbres), "application/json");
		}

		[PageInfo("Sipariş Onay-Red Metodu", SHRoles.SatisPersoneli, SHRoles.SatisOnaylayici, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public ContentResult UpdateStatus(Guid orderId, int type)
		{
			var feedback = new FeedBack();
			var userStatus = (PageSecurity)Session["userStatus"];
			var dbresult = new VMCMP_OrderModels { id = orderId }.Load(false).UpdateStatus(type, userStatus.user.id);

			return Content(Infoline.Helper.Json.Serialize(new ResultStatusUI
			{
				Result = dbresult.result,
				Object = dbresult.objects,
				FeedBack = dbresult.result ? feedback.Success("İşlem başarılı Mesaj : " + dbresult.message, false) :
						   feedback.Warning("İşlem başarısız. Mesaj : " + dbresult.message)
			}), "application/json");
		}

		[HttpPost]
		[PageInfo("Sipariş Silme Metodu", SHRoles.SatisPersoneli, SHRoles.SatisPersoneli, SHRoles.CRMYonetici)]
		public JsonResult Delete(Guid id)
		{
			var feedback = new FeedBack();

			var dbresult = new VMCMP_OrderModels { id = id }.Delete();

			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning(dbresult.message)
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}

		[PageInfo("Sipariş Yazdır"), ExportPDF, AllowEveryone]
		public ActionResult Print(Guid id, int? type)
		{
			var model = new VMCMP_OrderModels { id = id }.Load(false).GetFormTemplate(type);
			return View(model);
		}

		public DataSourceRequest UpdateRequest(DataSourceRequest request)
		{
			var userStatus = (PageSecurity)Session["userStatus"];
			if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SatisPersoneli)) && !userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SatisOnaylayici)))
			{
				IFilterDescriptor composit = new FilterDescriptor { Value = userStatus.user.id, Operator = FilterOperator.IsEqualTo, Member = "createdby" };
				foreach (var item in userStatus.ChildPersons.Where(a => a.IdUser.HasValue && a.IdUser != userStatus.user.id).GroupBy(a => a.IdUser))
				{
					composit = new CompositeFilterDescriptor
					{
						LogicalOperator = FilterCompositionLogicalOperator.Or,
						FilterDescriptors = new FilterDescriptorCollection{
							composit,
							new FilterDescriptor { Value = userStatus.user.id, Operator = FilterOperator.IsEqualTo, Member = "createdby" },
						}
					};
				}
				if (request.Filters != null && request.Filters.Count() > 0)
				{
					composit = new CompositeFilterDescriptor
					{
						LogicalOperator = FilterCompositionLogicalOperator.And,
						FilterDescriptors = new FilterDescriptorCollection
						{
							composit,
							request.Filters[0]
						}
					};
				}
				request.Filters = new List<IFilterDescriptor>() { composit };
			}
			return request;
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
						Operand1 = (COL)"customerId",
						Operator = BinaryOperator.Equal,
						Operand2 = (VAL)customer
					};
				}
			}
			
			query.Filter &= filter;
			return query;
		}

	}
}
