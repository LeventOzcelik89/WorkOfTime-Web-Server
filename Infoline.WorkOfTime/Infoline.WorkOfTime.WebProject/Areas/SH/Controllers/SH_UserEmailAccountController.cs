using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infoline.Framework.Helper;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class SH_UserEmailAccountController : Controller
    {
        [PageInfo("Kullanıcı Email Bilgileri Sayfası", SHRoles.Personel)]

        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Kullanıcı Email Bilgileri", SHRoles.Personel)]

        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetSH_UserEmailAccount(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetSH_UserEmailAccountCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Kullanıcı Email Bilgileri Dropdown", SHRoles.Personel)]

        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var condition = Infoline.WorkOfTime.BusinessAccess.Models.SH_UserEmailCalendarModel.UpdateQuery(KendoToExpression.Convert(request), userStatus);
            var db = new WorkOfTimeDatabase();
            var data = db.GetSH_UserEmailAccount(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }


        [PageInfo("Kullanıcı Email Ekleme Sayfası", SHRoles.Personel)]

        public ActionResult Insert()
        {
            var data = new SH_UserEmailAccount { id = Guid.NewGuid() };
            return View(data);
        }

        [PageInfo("Kullanıcı Email Ekleme", SHRoles.Personel)]

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SH_UserEmailAccount item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            item.password = CryptoEncrypt(item.password);
            item.userid = userStatus.user.id;

            var isDefaultYesData = db.GetSH_UserEmailAccountByUserIdİsDefaultTrue(userStatus.user.id);
            if (isDefaultYesData == null)
            {
                item.isDefault = (int)EnumSH_UserEmailAccountisDefault.Evet;
            }
            else
            {
                if (isDefaultYesData.isDefault == (int)EnumSH_UserEmailAccountisDefault.Evet && item.isDefault == (int)EnumSH_UserEmailAccountisDefault.Evet)
                {
                    isDefaultYesData.isDefault = (int)EnumSH_UserEmailAccountisDefault.Hayir;
                    db.UpdateSH_UserEmailAccount(isDefaultYesData);
                }
            }

            var dbresult = db.InsertSH_UserEmailAccount(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? (isDefaultYesData != null && isDefaultYesData.isDefault == (int)EnumSH_UserEmailAccountisDefault.Hayir) ? feedback.Success("" + isDefaultYesData.email + " E-Posta adresiniz yerine " + item.email + " adresiniz varsayılan E-Posta olarak atanmıştır.") : feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Kullanıcı Email Güncelleme Sayfası", SHRoles.Personel)]

        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetSH_UserEmailAccountById(id);
            data.password = CryptoDecrypt(data.password);
            return View(data);
        }

        [PageInfo("Kullanıcı Email Güncelleme", SHRoles.Personel)]

        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SH_UserEmailAccount item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;
            item.userid = userStatus.user.id;
            item.password = CryptoEncrypt(item.password);

            var dbresult = db.UpdateSH_UserEmailAccount(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Kullanıcı Email Silme", SHRoles.Personel)]

        [HttpPost]
        public JsonResult Delete(string[] id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            var item = id.Select(a => new SH_UserEmailAccount { id = new Guid(a) });

            var dbresult = db.BulkDeleteSH_UserEmailAccount(item);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Error("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string CryptoEncrypt(string text)
        {
            CryptographyHelper crypto = new CryptographyHelper();
            return crypto.Encrypt(text);
        }

        private string CryptoDecrypt(string text)
        {
            CryptographyHelper crypto = new CryptographyHelper();
            return crypto.Decrypt(text);
        }

    }
}
