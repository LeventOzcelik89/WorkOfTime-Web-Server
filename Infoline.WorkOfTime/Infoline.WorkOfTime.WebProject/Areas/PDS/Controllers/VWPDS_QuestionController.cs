using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PDS.Controllers
{
    public class VWPDS_QuestionController : Controller
    {
        [PageInfo("Sorular Methodu", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_Question(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWPDS_QuestionCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Sorular Veri Methodu", SHRoles.Personel)]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_Question(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Soru Ekleme", SHRoles.IKYonetici)]
        public ActionResult Insert()
        {
            var data = new VWPDS_Question { id = Guid.NewGuid() };
            return View(data);
        }


        [HttpPost]
        [PageInfo("Soru Ekleme", SHRoles.IKYonetici)]
        public JsonResult Insert(PDS_Question item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;

            var res = new ResultStatusUI();

            var q = db.GetPDS_QuestionByQuestion(item.question);

            if (q == null)
            {
                var elem = new PDS_Question
                {
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    question = item.question
                };

                var dbresult = db.InsertPDS_Question(elem);

                res.Result = dbresult.result;
                res.Object = elem;
            }

            else
            {
                res.Result = false;
                res.Object = null;
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Tüm Sorular Methodu", SHRoles.IKYonetici)]
        public JsonResult GetQuestions()
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            var questions = db.GetPDS_Question().ToArray();
            return Json(questions.Select(a => new { Id = a.id, Question = a.question }).ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}
