using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessData.Specific;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebService.HandlersSpecific
{
    [Export(typeof(ISmartHandler))]
    public partial class VWCMP_OrderHandler : BaseSmartHandler
    {
        public VWCMP_OrderHandler()
            : base("VWCMP_Order")
        {

        }

        [HandleFunction("VWCMP_Order/GetAll")]
        public void VWCMP_OrderGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                cond = VMCMP_OrderModels.UpdateQueryAllOrder(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity());
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCMP_Order(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCMP_Order/CustomerDropDownGetAll")]
        public void VWCMP_OrderCustomerDropDownGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                cond = VMCMP_OrderModels.UpdateQueryCustomerDropDown(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity());
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCMP_Company(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCMP_Order/StorageDropDownGetAll")]
        public void VWCMP_OrderStorageDropDownGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                cond = VMCMP_OrderModels.UpdateQueryStorageDropDown(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity());
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCMP_Storage(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWCMP_Order/Insert")]
        public void VWCMP_OrderInsert(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var model = ParseRequest<VMCMP_OrderModels>(context);
                var userId = CallContext.Current.UserId;
                var invoiceModel = new List<VWCMP_InvoiceItem>();

                if (model.InvoiceItems == null)
                {
                    RenderResponse(context, new ResultStatus() { result = false, message = "Hiç ürün eklemeden sipariş oluşturamazsınız. Lütfen ürün ekleyiniz." });
                    return;
                }

                model.rateExchange = model.rateExchange == null ? model.rateExchange = 1.0 : model.rateExchange;
                model.rowNumber = String.IsNullOrEmpty(model.rowNumber) ? BusinessExtensions.B_GetIdCode() : model.rowNumber;

                var products = db.GetVWPRD_ProductByIds(model.InvoiceItems.Select(x => x.productId.Value).ToArray()).ToArray();

                foreach (var prod in products.ToList())
                {
                    var quantity = model.InvoiceItems.Where(x => x.productId == prod.id).Select(x => x.quantity).FirstOrDefault();
                    invoiceModel.Add(new VWCMP_InvoiceItem
                    {
                        productId = prod.id,
                        quantity = quantity,
                        unitId = prod.unitId,
                        KDV = 18.0,
                        KDVType = 0,
                        price = prod.currentSellingPrice
                    });
                }

                model.status = 0;
                model.discountType = 0;
                model.discountPercent = 0;
                model.paymentType = 0;
                model.InvoiceItems = invoiceModel;
                model.direction = 1;
                model.type = 4;
                model.IsTransform = false;
                model.issueDate = DateTime.Now;
                model.currencyId = Guid.Parse("7ec29dca-9073-4028-90f2-d5613357117a");
                var rs = model.Save(userId);
                RenderResponse(context, rs);

            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCMP_Order/Update")]
        public void VWCMP_OrderUpdate(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCMP_OrderModels>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Save(userId);
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCMP_Order/GetById")]
        public void VWCMP_OrderGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = new VMCMP_OrderModels { id = new Guid((string)id) }.Load(false);
                RenderResponse(context, new ResultStatus
                {
                    result = true,
                    objects = data,
                });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWCMP_Order/GetCount")]
        public void VWCMP_OrderGetCount(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCMP_OrderCount(cond.Filter);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCMP_Order/GetPageInfo")]
        public void VWCMP_OrderGetPageInfo(HttpContext context)
        {
            try
            {
                var now = DateTime.Now;
                var startOfWeek = now.AddDays(((int)(now.DayOfWeek) * -1) + 1).Date;
                var endOfWeek = startOfWeek.AddDays(7).Date;
                var startOfMonth = new DateTime(now.Year, now.Month, 1).Date;
                var endOfMonth = startOfMonth.AddMonths(1).Date;
                var startOfLastMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-1).Date;

                var model = new PageModel { SearchProperty = "searchField" };


                model.Filters.Add(new PageFilter
                {
                    Title = "Sipariş Sürecine Göre",
                    Items = new List<PageFilterItem> {
                        new PageFilterItem{
                            Title = "Onay Bekleyen Siparişler",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"status", Operator = BinaryOperator.Equal, Operand2 = (VAL)(int)EnumCMP_OrderStatus.CevapBekleniyor }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"direction", Operator = BinaryOperator.Equal, Operand2 = (VAL)(int)EnumCMP_InvoiceDirectionType.Satis } }.GetSerializeObject(),
                        },
                        new PageFilterItem{
                            Title = "Onaylanan Siparişler",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"status", Operator = BinaryOperator.Equal, Operand2 = (VAL)(int)EnumCMP_OrderStatus.Onay }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"direction", Operator = BinaryOperator.Equal, Operand2 = (VAL)(int)EnumCMP_InvoiceDirectionType.Satis } }.GetSerializeObject(),
                        },
                         new PageFilterItem{
                            Title = "Faturası Kesilen Siparişler",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"status", Operator = BinaryOperator.Equal, Operand2 = (VAL)(int)EnumCMP_OrderStatus.SurecTamamlandi }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"direction", Operator = BinaryOperator.Equal, Operand2 = (VAL)(int)EnumCMP_InvoiceDirectionType.Satis } }.GetSerializeObject(),
                        },
                         new PageFilterItem{
                            Title = "Reddedilen Siparişler",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"status", Operator = BinaryOperator.Equal, Operand2 = (VAL)(int)EnumCMP_OrderStatus.Red }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"direction", Operator = BinaryOperator.Equal, Operand2 = (VAL)(int)EnumCMP_InvoiceDirectionType.Satis } }.GetSerializeObject(),
                        }
                    },
                });


				model.Filters.Add(new PageFilter
				{
					Title = "Sipariş Tarihine Göre",
					Items = new List<PageFilterItem> {
						new PageFilterItem{
							Title = "Bugün",
							Filter = CreateFilter(now.ToString("yyyy-MM-dd"),now.AddDays(1).ToString("yyyy-MM-dd")).GetSerializeObject()
						},
						new PageFilterItem{
							Title = "Dün",
							Filter = CreateFilter(now.AddDays(-1).ToString("yyyy-MM-dd"),now.ToString("yyyy-MM-dd")).GetSerializeObject()
						},
						 new PageFilterItem{
							Title = "Bu Hafta",
							Filter = CreateFilter(startOfWeek.ToString("yyyy-MM-dd"),endOfWeek.ToString("yyyy-MM-dd")).GetSerializeObject()
						},
						 new PageFilterItem{
							Title = "Geçen Hafta",
							Filter = CreateFilter(startOfWeek.AddDays(-7).ToString("yyyy-MM-dd"),startOfWeek.ToString("yyyy-MM-dd")).GetSerializeObject()
						},
						  new PageFilterItem{
							Title = "Bu Ay",
							Filter = CreateFilter(startOfMonth.ToString("yyyy-MM-dd"),endOfMonth.ToString("yyyy-MM-dd")).GetSerializeObject()
						},
						  new PageFilterItem{
							Title = "Geçen Ay",
							Filter = CreateFilter(startOfLastMonth.ToString("yyyy-MM-dd"),startOfMonth.ToString("yyyy-MM-dd")).GetSerializeObject()
						}
					},
				});

                RenderResponse(context, model);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }



		public BEXP CreateFilter(string startDate, string endDate)
		{
			var filter = new BEXP
			{
				Operand1 = new BEXP
                {
                    Operand1 = (COL)"issueDate",
                    Operator = BinaryOperator.GreaterThanOrEqual,
                    Operand2 = (VAL)startDate
                },

                Operator = BinaryOperator.And,
				Operand2 = new BEXP
                {
                    Operand1 = (COL)"issueDate",
                    Operator = BinaryOperator.LessThanOrEqual,
                    Operand2 = (VAL)endDate
                }
            };

            return filter;
		}

		[HandleFunction("VWCMP_Order/Delete")]
		public void VWCMP_OrderDelete(HttpContext context)
		{
			try
			{
				var model = ParseRequest<VMCMP_OrderModels>(context);
				var userId = CallContext.Current.UserId;
				var rs = model.Delete();
				RenderResponse(context, rs);
			}
			catch (Exception ex)
			{
				RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
			}
		}

    }
}
