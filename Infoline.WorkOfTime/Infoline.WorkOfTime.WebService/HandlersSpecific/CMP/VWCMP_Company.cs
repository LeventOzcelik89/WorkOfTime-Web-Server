using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebService.Handler;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWCMP_CompanyHandlers : BaseSmartHandler
    {
        public VWCMP_CompanyHandlers()
            : base("VWCMP_Company")
        {

        }
        [HandleFunction("VWCMP_Company/SpecGetAll")]
        public void VWCMP_CompanySpecGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var userId = CallContext.Current.UserId;
                var db = new WorkOfTimeDatabase();
                var childPersons = new ManagersCalculator().GetAllChilds(CallContext.Current.UserId);
                var childUserList = new List<Guid>();
                childUserList.Add(userId);
                foreach (var chilPerson in childPersons.Where(x => x.IdUser != CallContext.Current.UserId))
                {
                    if (!chilPerson.IdUser.HasValue)
                    {
                        continue;
                    }
                    var user = db.GetVWSH_UserById(chilPerson.IdUser.Value);
                    if (user != null)
                    {
                        childUserList.Add(user.id);
                    }
                }

                cond.Filter &= new BEXP
                {
                    Operand1 = (COL)"createdby",
                    Operator = BinaryOperator.In,
                    Operand2 = new ARR { Values = childUserList.Select(a => (VAL)a).ToArray() },
                };

                var data = db.GetVWCMP_Company(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
        [HandleFunction("VWCMP_Company/GetById")]
        public void VWCMP_CompanyGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var data = new VMCMP_CompanyModel().LoadCompanyDetail(new Guid((string)id));
                RenderResponse(context, new ResultStatus() { result = true, objects = data });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCMP_Company/CompanyInsertWithUser")]
        public void VWCMP_CompanyInsert(HttpContext context)
        {
            try
            {
                var item = ParseRequest<VMCMP_CompanyModel>(context);
                var userId = CallContext.Current.UserId;
                var db = new WorkOfTimeDatabase();
                item.Save(userId);
                RenderResponse(context, new ResultStatus() { result = true, message = "Müşteri Başarılı Bir Şekilde Eklendi" });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }
    }
}
