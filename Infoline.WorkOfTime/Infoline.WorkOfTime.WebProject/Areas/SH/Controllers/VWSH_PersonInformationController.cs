using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class VWSH_PersonInformationController : Controller
    {

        [PageInfo("Personel Bilgileri Methodu", SHRoles.IKYonetici)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_PersonInformation(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSH_PersonInformationCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Personel Bilgileri Detayı", SHRoles.IKYonetici)]
        public ActionResult Detail(Guid id, Guid? userid, string tckimlik)
        {
            var db = new WorkOfTimeDatabase();
            var data = userid.HasValue ? db.GetVWSH_PersonInformation().Where(x => x.UserId == userid).FirstOrDefault() : db.GetVWSH_PersonInformationById(id);
            return View(data ?? new VWSH_PersonInformation());
        }

        [PageInfo("Personel Bilgisi Güncelleme", SHRoles.IKYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_PersonInformationById(id);
            return View(data);
        }


        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Personel Bilgisi Güncelleme", SHRoles.IKYonetici)]
        public JsonResult Update(SH_PersonInformation item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            var dbrow = db.GetSH_PersonInformationByUserId(item.UserId.Value);

            var datanew = new SH_PersonInformation().B_EntityDataCopyForMaterial(item);
            if(dbrow != null) { 
            datanew.id = dbrow.id;
            }

            var dbresult = dbrow == null ? db.InsertSH_PersonInformation(item) : db.UpdateSH_PersonInformation(datanew);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kişisel bilgileri düzenleme işlemi başarıyla gerçekleşti..") : feedback.Error("Kişisel bilgileri düzenleme işlemi başarısız..")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
