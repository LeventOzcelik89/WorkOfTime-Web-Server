using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.WebProject.Areas.SYS.Controllers
{
    public class VWSYS_BlockMailController : Controller
    {
        [PageInfo("Engellediğim Mailler", SHRoles.Personel)]
        public JsonResult GetBlockMailList()
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var data = db.GetSYS_BlockMailByUserId(userStatus.user.id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //'Status' Ekleme için 1 silme için 0 gönder
        [PageInfo("Mail Engelleme Durumları", SHRoles.Personel)]
        public JsonResult UpdateBlockMail(bool status, int type)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedBack = new FeedBack();
            var rs = new ResultStatus { result = true };
            if (status == true)
            {
                rs &= db.InsertSYS_BlockMail(new SYS_BlockMail
                {
                    created = DateTime.Now,
                    createdby = userStatus.user.id,
                    userId = userStatus.user.id,
                    type = type
                });
            }
            else
            {
                var data = db.GetSYS_BlockMailByUserIdAndType(userStatus.user.id,type);
                if (data == null)
                {
                    return Json(new ResultStatusUI
                    {
                        Result = true,
                        FeedBack = feedBack.Success("")
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    rs &= db.DeleteSYS_BlockMail(data);
                }
            }

            return Json(new ResultStatusUI
            {
                Result = rs.result,
                FeedBack = rs.result ? feedBack.Success("İşlem Başarılı") : feedBack.Warning("İşlem Başarısız")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
