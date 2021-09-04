using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessData.Specific;
using Infoline.Web.SmartHandlers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Infoline.ProjectManagement.WebService.HandlersSpecific.CMP
{
	[Export(typeof(ISmartHandler))]
	public partial class CMP_InvoiceHandler : BaseSmartHandler
	{
		public CMP_InvoiceHandler()
			: base("CMP_InvoiceHandler")
		{

		}


		[HandleFunction("VWCMP_Invoice/GetById")]
		public void VWCMP_InvoiceGetById(HttpContext context)
		{
			try
			{
				var id = context.Request["id"];
				var model = new VMCMP_InvoiceModels { id = new Guid(id) }.Load(false, 0);
				RenderResponse(context, model);
			}
			catch (Exception ex)
			{
				RenderResponse(context, new ResultStatus() { result = false, message = ex.Message });
				return;
			}
		}

		[HandleFunction("VWCMP_Invoice/GetPageInfo")]
		public void VWCMP_InvoiceGetPageInfo(HttpContext context)
		{
			var now = DateTime.Now;
			var startOfWeek = now.AddDays(((int)(now.DayOfWeek) * -1) + 1).Date;
			var endOfWeek = startOfWeek.AddDays(7).Date;
			var startOfMonth = new DateTime(now.Year, now.Month, 1).Date;
			var endOfMonth = startOfMonth.AddMonths(1).Date;
			var startOfLastMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-1).Date;

			try
			{
				var model = new PageModel { SearchProperty = "searchField" };


				model.Filters.Add(new PageFilter
				{
					Title = "Fatura Tipine Göre",
					Items = new List<PageFilterItem> {
						new PageFilterItem{
							Title = "Satış Faturaları",
							Filter =  new BEXP { Operand1 = (COL)"direction", Operator = BinaryOperator.Equal, Operand2 = (VAL)1 }.GetSerializeObject(),
						},
						new PageFilterItem{
							Title = "Alış Faturaları",
							Filter = new BEXP { Operand1 = (COL)"direction", Operator = BinaryOperator.Equal, Operand2 = (VAL)0 }.GetSerializeObject(),
						}
					},
				});

				model.Filters.Add(new PageFilter
				{
					Title = "Tarihine Göre",
					Items = new List<PageFilterItem> {
						new PageFilterItem{
							Title = "Bugün",
							Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"issueDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)now.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"issueDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)now.AddDays(1).ToString("yyyy-MM-dd") } }.GetSerializeObject(),
						},
						new PageFilterItem{
							Title = "Bu Hafta",
							Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"issueDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfWeek.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"issueDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)endOfWeek.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
						},
						 new PageFilterItem{
							Title = "Bu Ay",
							Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"issueDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfMonth.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"issueDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)endOfMonth.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
						},
						 new PageFilterItem{
							Title = "Gelecek",
							Filter =  new BEXP { Operand1 = (COL)"issueDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)DateTime.Now.ToString("yyyy/MM/dd") }.GetSerializeObject(),
						},
						  new PageFilterItem{
							Title = "Dün",
							Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"issueDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)now.AddDays(-1).ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"issueDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)now.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
						},
						  new PageFilterItem{
							Title = "Geçen Hafta",
							Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"issueDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfWeek.AddDays(-7).ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"issueDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)startOfWeek.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
						},
						  new PageFilterItem{
							Title = "Geçen Ay",
							Filter = 
							new BEXP 
							{ 
								Operand1 = new BEXP 
								{
									Operand1 = (COL)"issueDate",
									Operator = BinaryOperator.GreaterThan,
									Operand2 = (VAL)startOfLastMonth.ToString("yyyy-MM-dd")
								}, 
								Operator = BinaryOperator.And,
								Operand2 = new BEXP 
								{ 
									Operand1 = (COL)"issueDate",
									Operator = BinaryOperator.LessThan,
									Operand2 = (VAL)startOfMonth.ToString("yyyy-MM-dd")
								} 
							}.GetSerializeObject(),
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

	}
}