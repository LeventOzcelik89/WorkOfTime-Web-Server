using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.SH.Models;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
    public class VWSH_PagesRoleController : Controller
    {

        [PageInfo("Rol Sayfa Yetkisi Tanımlama", SHRoles.SistemYonetici)]
        public ActionResult Insert(Guid? id)
        {
            var data = new VWSH_PagesRole { id = Guid.NewGuid(), roleid = id };
            return View(data);
        }

        [PageInfo("Rol Sayfa Yetkisi Tanımlama", SHRoles.SistemYonetici)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SH_PagesRole item, string ActionIds)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var trans = db.BeginTransaction();

            var ActionIdList = ActionIds.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => new Guid(x)).ToArray();
            if (!item.roleid.ToString().IsValidGuid())
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Geçerli bir rol bulunamadı veya Hiç sayfa seçilmedi")
                }, JsonRequestBehavior.AllowGet);
            }

            var list = new List<ResultStatus>();
            var pageRolelist = db.GetSH_PagesRoleByRoleId((Guid)item.roleid);
            list.Add(db.BulkDeleteSH_PagesRole(pageRolelist, trans));

            var sayfalar = db.GetSH_PagesByIds(ActionIdList);
            list.Add(db.BulkInsertSH_PagesRole(sayfalar.Select(s => new SH_PagesRole
            {
                created = DateTime.Now,
                createdby = userStatus.user.id,
                action = s.Action,
                roleid = item.roleid,
                id = Guid.NewGuid()
            }), trans));
            if (list.Count(a => a.result == false) > 0)
            {
                trans.Rollback();

                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Sayfa(lara) rol atama işlemi başarısız oldu.")
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                trans.Commit();

                return Json(new ResultStatusUI
                {
                    Result = true,
                    FeedBack = feedback.Success("Sayfa(lara) başarılı bir şekilde rol atandı.", false, Request.UrlReferrer.AbsoluteUri)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [PageInfo("Rol Sayfa Yetkisi Liste Methodu", SHRoles.SistemYonetici)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSH_PagesRole(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSH_PagesRoleCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Rol Sayfa Yetkisi TreeView DataSource ", SHRoles.SistemYonetici)]
        public ContentResult DataSourceTreeView(Guid roleId)
        {
            var db = new WorkOfTimeDatabase();
            var liste = new List<TreeViewModel>();
            var pagesRoleId = db.GetVWSH_PagesRoleByRoleId(roleId);
            var library = db.GetSH_Pages();
            var dataArea = library.GroupBy(x => x.Area).Select(x => x.Key).OrderBy(x => x).ToArray();
            foreach (var area in dataArea)
            {
                //area
                var areaTreeViewModel = new TreeViewModel();
                areaTreeViewModel.id = Guid.NewGuid();
                areaTreeViewModel.text = !string.IsNullOrEmpty(area) ? area : "Tüm Sayfa";
                areaTreeViewModel.spriteCssClass = "folder";
                //controller
                var controllerTreewModel = new List<TreeViewModel>();
                var controller = library.Where(x => x.Area == area).GroupBy(x => x.Controller).Select(x => x.Key).OrderBy(x => x).ToArray();
                foreach (var cont in controller)
                {
                    var c = new TreeViewModel();
                    c.id = Guid.NewGuid();
                    c.text = !string.IsNullOrEmpty(cont) ? cont : "Tüm Sayfa";
                    c.expanded = false;
                    c.spriteCssClass = "pdf";
                    c.items =
                        library.Where(x => x.Controller == cont)
                            .OrderBy(x => x.Action)
                            .Select(
                                s =>
                                    new TreeViewModel
                                    {
                                        id = s.id,
                                        text =
                                            s.Action +
                                            (!string.IsNullOrEmpty(s.Description) ? " ( " + s.Description + " ) " : ""),
                                        spriteCssClass = "html",
                                        @checked = s.AllowEveryone == true ? true : pagesRoleId.Where(v => v.action == s.Action).Count() > 0 ? true : false,
                                        enabled = s.AllowEveryone == true ? false : true
                                    }).ToArray();
                    controllerTreewModel.Add(c);
                }
                areaTreeViewModel.items = controllerTreewModel.ToArray();
                liste.Add(areaTreeViewModel);

            }

            var newData = new DataSend
            {
                pagesRole = pagesRoleId,
                model = liste
            };

            return Content(Infoline.Helper.Json.Serialize(newData), "application/json");
        }
    }
}

