using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.CRM.Controllers
{
	public class VWCRM_ContactUserController : Controller
    {
        [PageInfo("Aktivite/Randevu Kullanıcısı Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_ContactUser(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWCRM_ContactUserCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Aktivite/Randevu Kullanıcısı Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWCRM_ContactUser(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }



        [HttpPost]
        [PageInfo("Aktivite/Randevu Kullanıcısı Silme")]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new CRM_ContactUser { id = new Guid(a) });

            var dbresult = db.BulkDeleteCRM_ContactUser(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Aktivite/Randevu Kullanıcısı Anında Ekleme",SHRoles.SatisPersoneli,SHRoles.CRMYonetici)]
        public ContentResult InstantInsert(int contactType, string name, Guid id, Guid ContactId, Guid PresentationId)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var db = new WorkOfTimeDatabase();

            var presentation = db.GetCRM_PresentationById(PresentationId);

            string firstName = "";
            string lastName = "";
            if (name != "")
            {
                var nameCount = name.Split();
                if (nameCount.Count() > 0)
                {
                    if (nameCount.Count() == 3)
                    {
                        firstName = nameCount[0];
                        lastName = nameCount[1] + " " + nameCount[2];
                    }
                    else if (nameCount.Count() == 2)
                    {
                        firstName = nameCount[0];
                        lastName = nameCount[1];
                    }
                    else if (nameCount.Count() == 1)
                    {
                        firstName = nameCount[0];
                    }
                    else
                    {
                        firstName = name;
                    }
                }
            }


            var user = new SH_User
            {
                id = id,
                createdby = userStatus.user.id,
                firstname = firstName,
                lastname = lastName,
                code = BusinessExtensions.B_GetIdCode(),
                type = (int)EnumSH_UserType.OtherPerson,
                loginname = firstName,
                password = db.GetMd5Hash(db.GetMd5Hash("123456")),
                status = false
            };

            var ourCompanies = db.GetVWCMP_CompanyMyCompanies().FirstOrDefault();

            var compId = contactType == (int)EnumCRM_ContactUserUserType.OwnerUser ? (ourCompanies != null ? ourCompanies.id : (Guid?)null) :
                contactType == (int)EnumCRM_ContactUserUserType.CustomerUser ? presentation.CustomerCompanyId :
                contactType == (int)EnumCRM_ContactUserUserType.ChannelUser ? presentation.ChannelCompanyId : presentation.CustomerCompanyId;

            var compPerson = new INV_CompanyPerson
            {
                createdby = userStatus.user.id,
                IdUser = user.id,
                CompanyId = compId,
                JobStartDate = DateTime.Now,
                Title = "CRM",
                Level = 1
            };

            var trans = db.BeginTransaction();

            var dbres = db.InsertSH_User(user, trans);
            dbres &= db.InsertINV_CompanyPerson(compPerson, trans);

            if (dbres.result == true)
            {
                trans.Commit();
            }
            else
            {
                trans.Rollback();
            }

            return Content(Infoline.Helper.Json.Serialize(new ResultStatus { result = dbres.result, objects = user.id }), "application/json");
        }
    }
}
