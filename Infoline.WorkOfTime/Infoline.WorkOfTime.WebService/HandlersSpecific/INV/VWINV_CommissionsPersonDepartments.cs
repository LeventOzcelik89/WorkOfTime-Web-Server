using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Mobile;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Web.SmartHandlers;
using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Linq;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class VWINV_CommissionsPersonDepartmentsHandler : BaseSmartHandler
    {
        public VWINV_CommissionsPersonDepartmentsHandler()
            : base("VWINV_CommissionsPersonDepartmentsHandler")
        {

        }

        [HandleFunction("VWINV_CommissionsPersonDepartments/GetPersons")]
        public void VWINV_CommissionsPersonDepartmentsGetPersons(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var userId = CallContext.Current.UserId;

                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();


                cond.Filter &= new BEXP
                {
                    Operand1 = (COL)"IsBasePosition",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)true

                };
                cond.Filter &= new BEXP
                {
                    Operand1 = (COL)"OrganizationType",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)(Int32)EnumINV_CompanyDepartmentsType.Organization

                };
                cond.Filter &= new BEXP
                {
                    Operand1 = (COL)"StartDate",
                    Operator = BinaryOperator.LessThan,
                    Operand2 = (VAL)DateTime.Now
                };
                cond.Filter &= new BEXP
                {

                    Operand1 = new BEXP
                    {
                        Operand1 = (COL)"EndDate",
                        Operator = BinaryOperator.IsNull,
                    },
                    Operator = BinaryOperator.Or,
                    Operand2 = new BEXP
                    {
                        Operand1 = (COL)"EndDate",
                        Operator = BinaryOperator.GreaterThan,
                        Operand2 = (VAL)DateTime.Now
                    }
                };
                cond.Filter &= new BEXP
                {
                    Operand1 = new BEXP
                    {
                        Operand1 = (COL)"IdUser",
                        Operator = BinaryOperator.Equal,
                        Operand2 = (VAL)userId
                    },
                    Operator = BinaryOperator.Or,
                    Operand2 = new BEXP
                    {
                        Operand1 = new BEXP
                        {
                            Operand1 = (COL)"Manager1",
                            Operator = BinaryOperator.Equal,
                            Operand2 = (VAL)userId
                        },
                        Operator = BinaryOperator.Or,
                        Operand2 = new BEXP
                        {
                            Operand1 = (COL)"Manager2",
                            Operator = BinaryOperator.Equal,
                            Operand2 = (VAL)userId
                        }
                    }
                };
                cond.Filter &= new BEXP
                {
                    Operand1 = (COL)"Person_Title",
                    Operator = BinaryOperator.IsNotNull,
                };

                var userIds = db.GetVWINV_CompanyPersonDepartments(cond);

                if (userIds.Count() > 0)
                {
                    var _userIds = userIds.GroupBy(a => a.IdUser).Select(a => a.Key.HasValue ? a.Key.Value : new Guid()).ToArray();
                    if (_userIds.Count() > 0)
                    {
                        var users = db.GetVWSH_UserByIds(_userIds).Select(a => new { a.id, a.FullName, a.ProfilePhoto, a.searchField }).ToArray();
                        RenderResponse(context, users);
                        return;
                    }
                }

                RenderResponse(context, new ResultStatus() { result = true, message = "Personel Bulunamadı!" });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}