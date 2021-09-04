using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc.UI;
using System;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.PDS.Controllers
{
    public class VWPDS_QuestionFormController : Controller
    {
        [AllowEveryone]
        [PageInfo("Sorular Formu Veri Methodu")]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWPDS_QuestionForm(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
    }
}
