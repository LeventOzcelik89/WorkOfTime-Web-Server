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
    public partial class VWCRM_ContactHandler : BaseSmartHandler
    {
        public VWCRM_ContactHandler()
            : base("VWCRM_Contact")
        {

        }

        [HandleFunction("VWCRM_Contact/GetAll")]
        public void VWCRM_ContactGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCRM_Contact(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWCRM_Contact/Insert")]
        public void VWCRM_ContactInsert(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCRM_ContactModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Save(userId);
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }


        [HandleFunction("VWCRM_Contact/ParticipantsInsert")]
        public void VWCRM_ContactParticipantsInsert(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCRM_ContactModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.ParticipantsInsert();
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCRM_Contact/GetById")]
        public void VWCRM_ContactGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = new VMCRM_ContactModel { id = new Guid((string)id) }.Load();
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

        [HandleFunction("VWCRM_Contact/GetByIdParticipants")]
        public void VWCRM_ContactGetParticipants(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var model = ParseRequest<Participant>(context);
                var data = new VMCRM_ContactModel { id = model.Id }.GetParticipants(model.Name);
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

        [HandleFunction("VWCRM_Contact/GetCount")]
        public void VWCRM_ContactGetCount(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCRM_ContactCount(cond.Filter);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCRM_Contact/GetPageInfo")]
        public void VWCRM_ContactGetPageInfo(HttpContext context)
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
                    Title = "Aktivite/Randevu Tipine Göre",
                    Items = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumCRM_ContactContactType>().Select(a => new PageFilterItem
                    {
                        Title = a.Value,
                        Filter = new BEXP { Operand1 = (COL)"ContactType", Operator = BinaryOperator.Equal, Operand2 = (VAL)a.Key }.GetSerializeObject(),
                    }).ToList(),
                });

                model.Filters.Add(new PageFilter
                {
                    Title = "Aktivite/Randevu Durumuna Göre",
                    Items = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumCRM_ContactContactStatus>().Select(a => new PageFilterItem
                    {
                        Title = a.Value,
                        Filter = new BEXP { Operand1 = (COL)"ContactStatus", Operator = BinaryOperator.Equal, Operand2 = (VAL)a.Key }.GetSerializeObject(),
                    }).ToList(),
                });

                model.Filters.Add(new PageFilter
                {
                    Title = "Başlangıç Tarihine Göre",
                    Items = new List<PageFilterItem> {
                        new PageFilterItem{
                            Title = "Bugün",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)now.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)now.AddDays(1).ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                        new PageFilterItem{
                            Title = "Bu Hafta",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfWeek.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)endOfWeek.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                         new PageFilterItem{
                            Title = "Bu Ay",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfMonth.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)endOfMonth.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                         new PageFilterItem{
                            Title = "Gelecek",
                            Filter =  new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)DateTime.Now.ToString("yyyy/MM/dd") }.GetSerializeObject(),
                        },
                          new PageFilterItem{
                            Title = "Dün",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)now.AddDays(-1).ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)now.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                          new PageFilterItem{
                            Title = "Geçen Hafta",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfWeek.AddDays(-7).ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)startOfWeek.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                          new PageFilterItem{
                            Title = "Geçen Ay",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfLastMonth.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"ContactStartDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)startOfMonth.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
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



        [HandleFunction("VWCRM_Contact/Delete")]
        public void VWCRM_ContactDelete(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCRM_ContactModel>(context);
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
