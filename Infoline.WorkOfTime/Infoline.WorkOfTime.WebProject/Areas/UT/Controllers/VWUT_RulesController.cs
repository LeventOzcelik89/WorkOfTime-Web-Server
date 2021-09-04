using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.UT.Controllers
{
    public class VWUT_RulesController : Controller
    {
        [PageInfo("Kurallar", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        public ActionResult Index()
        {
            return View();
        }


        [PageInfo("Kural tanımlamalarının veri methodu.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWUT_Rules(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWUT_RulesCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Kural tanımlamalarının dropdown veri methodu.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWUT_Rules(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

 


        [PageInfo("Kural tanımlamaları ekleme methodu.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        public ActionResult Insert(VMUT_RulesModel data)
        {
            data.Load();
            return View(data);
        }

        [PageInfo("Kural tanımlamaları ekleme methodu.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(VMUT_RulesModel item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var ruleControl = db.GetUT_RulesByTypeIsDefault((short)item.type);
            if (ruleControl != null && item.isDefault == true)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Daha önce aynı tip için varsayılan kural atanmış.")
                }, JsonRequestBehavior.AllowGet);
            }

            var dbresult = item.Save(userStatus.user.id);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Kullanıcı kurallarının detay sayfasıdır.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        public ActionResult Detail(VMUT_RulesModel data)
        {
            data.Load();
            return View(data);
        }


        [PageInfo("Kural tanımlamaları düzenleme methodu.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        public ActionResult Update(VMUT_RulesModel data)
        {
            data.Load();
            return View(data);
        }

        [PageInfo("Kural tanımlamaları düzenleme methodu.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(VMUT_RulesModel item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

			if (item.isDefault.HasValue && item.isDefault == true)
			{
                var ruleDef = db.GetUT_RulesByTypeIsDefaultById((Int16)EnumUT_RulesType.Transaction, item.id);

                if (ruleDef != null)
                {
                    return Json(new ResultStatusUI
                    {
                        Result = false,
                        FeedBack = feedback.Warning("Aynı tip için daha önce default kural atanmış.")
                    }, JsonRequestBehavior.AllowGet);
                }
            }


            var dbresult = item.Save(userStatus.user.id);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success(dbresult.message) : feedback.Warning(dbresult.message)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [PageInfo("Kural tanımlamaları silme methodu.", SHRoles.SistemYonetici, SHRoles.IKYonetici)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var rules = db.GetUT_RulesUserByRuleId(id);
            if (rules.Count() > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Silmek istediğiniz kurala ait kullanıcı ataması mevcut.")
                }, JsonRequestBehavior.AllowGet);
            }


            var item = db.GetUT_RulesById(id);

            //TODO:Oğuz burada bu tanımlama ile ilgili işlemler yapıldı ise silinmemesi sağlanacak.
            var dbresult = db.DeleteUT_Rules(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}
