using Infoline.Framework.Database;
using Infoline.ProjectManagement.WebService.Models;
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
    public partial class VWCRM_PresentationHandler : BaseSmartHandler
    {
        public VWCRM_PresentationHandler()
            : base("VWCRM_Presentation")
        {
        }
        [HandleFunction("VWCRM_Presentation/GetCount")]
        public void VWCRM_PresentationGetCount(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                cond = VMCRM_PresentationModel.UpdateQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity());
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCRM_PresentationCount(cond.Filter);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
        [HandleFunction("VWCRM_Presentation/GetAll")]
        public void VWCRM_PresentationGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                cond = VMCRM_PresentationModel.UpdateQuery(cond, Infoline.ProjectManagement.ServiceModel.GetPageSecurity());
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCRM_Presentation(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
        [HandleFunction("VWCRM_Presentation/GetPageInfo")]
        public void VWCRM_PresentationGetPageInfo(HttpContext context)
        {
            var now = DateTime.Now;
            var startOfWeek = now.AddDays(((int)(now.DayOfWeek) * -1) + 1).Date;
            var endOfWeek = startOfWeek.AddDays(7).Date;
            var startOfMonth = new DateTime(now.Year, now.Month, 1).Date;
            var endOfMonth = startOfMonth.AddMonths(1).Date;
            var startOfLastMonth = new DateTime(now.Year, now.Month, 1).AddMonths(-1).Date;
            try
            {
                var db = new WorkOfTimeDatabase();
                var data = db.GetCRM_ManagerStage();
                var model = new PageModel { SearchProperty = "searchField" };
                model.Filters.Add(new PageFilter
                {
                    Title = "Aşama",
                    Items = data.Select(a => new PageFilterItem
                    {
                        Title = a.Name,
                        Filter = new BEXP { Operand1 = (COL)"PresentationStageId", Operator = BinaryOperator.Equal, Operand2 = (VAL)a.id }.GetSerializeObject(),
                    }).ToList(),
                });
                model.Filters.Add(new PageFilter
                {
                    Title = "Önem Derecesi",
                    Items = Infoline.Helper.EnumsProperties.EnumToArrayGeneric<EnumCRM_PresentationPriorityLevel>().Select(a => new PageFilterItem
                    {
                        Title = a.Value,
                        Filter = new BEXP { Operand1 = (COL)"PriorityLevel", Operator = BinaryOperator.Equal, Operand2 = (VAL)a.Key }.GetSerializeObject(),
                    }).ToList(),
                });
                model.Filters.Add(new PageFilter
                {
                    Title = "Son İşlem Tarihi",
                    Items = new List<PageFilterItem> {
                        new PageFilterItem{
                            Title = "Bugün",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)now.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)now.AddDays(1).ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                        new PageFilterItem{
                            Title = "Dün",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)now.AddDays(-1).ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)now.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                         new PageFilterItem{
                            Title = "Bu Hafta",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfWeek.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)endOfWeek.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                         new PageFilterItem{
                            Title = "Geçen Hafta",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfWeek.AddDays(-7).ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)startOfWeek.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                          new PageFilterItem{
                            Title = "Bu Ay",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfMonth.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)endOfMonth.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
                        },
                          new PageFilterItem{
                            Title = "Geçen Ay",
                            Filter = new BEXP { Operand1 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.GreaterThan, Operand2 = (VAL)startOfLastMonth.ToString("yyyy-MM-dd") }, Operator = BinaryOperator.And, Operand2 = new BEXP { Operand1 = (COL)"LastActivityDate", Operator = BinaryOperator.LessThan, Operand2 = (VAL)startOfMonth.ToString("yyyy-MM-dd") } }.GetSerializeObject(),
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
        [HandleFunction("VWCRM_Presentation/GetById")]
        public void VWCRM_PresentationGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = new VMCRM_PresentationModel { id = new Guid((string)id) }.Load();
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
        [HandleFunction("VWCRM_Presentation/Insert")]
        public void VWCRM_PresentationInsert(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCRM_PresentationModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Save(userId);
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
        [HandleFunction("VWCRM_Presentation/Update")]
        public void VWCRM_PresentationUpdate(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCRM_PresentationModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Save(userId);
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
        [HandleFunction("VWCRM_Presentation/Delete")]
        public void VWCRM_PresentationDelete(HttpContext context)
        {
            try
            {
                var model = ParseRequest<VMCRM_PresentationModel>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Delete();
                RenderResponse(context, rs);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
        [HandleFunction("VWCRM_Presentation/Guhem")]
        public void VWCRM_PresentationGuhem(HttpContext context)
        {
            try
            {
                var model = ParseRequest<GuhemWeb>(context);
                if (model == null)
                {
                    RenderResponse(context, new ResultStatus() { result = false, message = "Object not be null!" });
                    return;
                }
                var result = new ResultStatus { result = true };
                var db = new WorkOfTimeDatabase();
                var trans = db.BeginTransaction();
                var userId = CallContext.Current.UserId;
                var getCustomer = db.GetCMP_CompanyByName(model.SchoolName);
                if (getCustomer == null)
                {
                    getCustomer = new CMP_Company
                    {
                        created = DateTime.Now,
                        createdby = userId,
                        code = BusinessExtensions.B_GetIdCode(),
                        id = Guid.NewGuid(),
                        name = model.SchoolName
                    };
                    result = db.InsertCMP_Company(getCustomer, trans);
                }
                var user = new SH_User
                {
                    firstname = model.ResponsibleName,
                    lastname = model.ResponsibleLastName,
                    email = model.ResponsibleEmail,
                    phone = model.ResponsiblePhone,
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userId,
                };
                var companyPerson = new INV_CompanyPerson
                {
                    created = DateTime.Now,
                    createdby = userId,
                    CompanyId = getCustomer.id,
                    Title = "Potansiyel Müşteri",
                    Level = 0,
                    IdUser = user.id,
                    JobStartDate = DateTime.Now,
                };
                result &= db.InsertSH_User(user, trans);
                result &= db.InsertINV_CompanyPerson(companyPerson, trans);
                var potentialOpportunity = new CRM_Presentation
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userId,
                    Name = model.SchoolName + " potansiyel fırsat",
                    CustomerCompanyId = getCustomer.id,
                    PresentationStageId = db.GetCRM_ManagerStageDefaultValue().id,
                    CompletionRate = 10,
                    SalesPersonId = db.GetSH_UserByRoleId("00000000-0000-0000-0000-420000000000").FirstOrDefault().id,
                    PlaceofArrival=(Int32)EnumCRM_PresentationPlaceofArrival.Web
                };
                result &= db.InsertCRM_Presentation(potentialOpportunity, trans);
                var contact = new CRM_Contact
                {
                    id = Guid.NewGuid(),
                    customerId = getCustomer.id,
                    ContactStatus = 1,
                    ContactStartDate = model.StartDate,
                    ContactEndDate = model.EndDate,
                    PresentationId = potentialOpportunity.id,
                    PresentationStageId = db.GetCRM_ManagerStageDefaultValue().id,
                    created = DateTime.Now,
                    createdby = userId,
                    Description = "Katılımcı Sayısı:" + model.ParticipantCount + " Yaş Aralığı:" + model.RangeOfAge
                };
                var calendar = new INV_CompanyPersonCalendar
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userId,
                    Description = model.SchoolName + " ile potansiyel fırsat",
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    Title = model.SchoolName + " ile potansiyel fırsat",
                    Type = (Int32)EnumINV_CompanyPersonCalendarType.Hatirlatma,
                };
                var calenderUser = new INV_CompanyPersonCalendarPersons
                {
                    created = DateTime.Now,
                    createdby = userId,
                    IDPersonCalendar = calendar.id,
                    IdUser = db.GetSH_UserByRoleId("00000000-0000-0000-0000-420000000000").FirstOrDefault().id
                };
                result &= db.InsertCRM_Contact(contact, trans);
                result &= db.InsertINV_CompanyPersonCalendar(calendar, trans);
                result &= db.InsertINV_CompanyPersonCalendarPersons(calenderUser, trans);
                if (result.result)
                {
                    trans.Commit();
                    RenderResponse(context, new ResultStatus() { result = true, message = "Success" });
                }
                else
                {
                    trans.Rollback();
                    RenderResponse(context, new ResultStatus() { result = true, message = result.message });
                }
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}
