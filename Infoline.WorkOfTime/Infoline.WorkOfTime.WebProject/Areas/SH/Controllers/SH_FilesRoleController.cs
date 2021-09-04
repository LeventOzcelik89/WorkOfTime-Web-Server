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
    public class SH_FilesRoleController : Controller
    {
        [PageInfo("Rol Dosya Yetkisi Tanımlama", SHRoles.SistemYonetici)]
        public ActionResult Insert(Guid? id)
        {
            var data = new SH_FilesRole { id = Guid.NewGuid(), roleid = id };
            string text = "";
            if (id.HasValue)
            {
                var db = new WorkOfTimeDatabase();
                text += db.GetSH_RoleById(id.Value).rolname;
            }
            ViewBag.RolName = text;
            return View(data);
        }

        [HttpPost, ValidateAntiForgeryToken]
        [PageInfo("Rol Dosya Yetkisi Tanımlama", SHRoles.SistemYonetici)]
        public JsonResult Insert(SH_FilesRole item, string ActionFilesIds)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var trans = db.BeginTransaction();

            var filesRoleList =
                ActionFilesIds.Split(',')
                    .Where(x => !string.IsNullOrEmpty(x))
                    .Select(x => new
                    {
                        dataTable = FilesSplit(x, 0),
                        fileGroup = FilesSplit(x, 1),
                        insert = FilesSplit(x, 2) == "insert",
                        delete = FilesSplit(x, 2) == "delete",
                        preview = FilesSplit(x, 2) == "preview"
                    })
                    .Where(w => w.insert == true || w.delete == true || w.preview == true)
                    .GroupBy(g => new { g.dataTable, g.fileGroup })
                    .ToDictionary(s => s.Key, t => t.ToList())
                    .Select(x => new SH_FilesRole
                    {
                        id = Guid.NewGuid(),
                        createdby = userStatus.user.id,
                        created = DateTime.Now,
                        roleid = item.roleid,
                        dataTable = x.Key.dataTable,
                        fileGroup = x.Key.fileGroup,
                        insert = x.Value.Count(c => c.insert == true) > 0,
                        delete = x.Value.Count(c => c.delete == true) > 0,
                        preview = x.Value.Count(c => c.preview == true) > 0,
                    });


            if (!item.roleid.ToString().IsValidGuid())
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Geçerli bir rol bulunamadı veya Hiç sayfa seçilmedi")
                }, JsonRequestBehavior.AllowGet);
            }

            var list = new List<ResultStatus>();
            var pageRolelist = db.GetSH_FilesRoleByRoleId((Guid)item.roleid);
            list.Add(db.BulkDeleteSH_FilesRole(pageRolelist, trans));
            list.Add(db.BulkInsertSH_FilesRole(filesRoleList, trans));

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

        [PageInfo("Dosya Rolü Methodu", SHRoles.SistemYonetici)]
        public JsonResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetSH_FilesRole(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetSH_FilesRoleCount(condition.Filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [PageInfo("Rol Dosya Yetkisi TreeView Datasource", SHRoles.SistemYonetici)]
        public ContentResult FileDataSourceTreeView(Guid roleId)
        {
            var liste = new List<FileTreeViewModel>();
            var model = LocalFileProvider._dataTableFileGroup;
            foreach (var datatable in model)
            {
                var dataTableTreeViewModel = new FileTreeViewModel();
                dataTableTreeViewModel.id = datatable.Key;
                dataTableTreeViewModel.text = datatable.Key;
                dataTableTreeViewModel.spriteCssClass = "folder";
                foreach (var fileGroup in datatable.Value)
                {
                    var fileGroupTreeViewModel = new FileTreeViewModel
                    {
                        id = dataTableTreeViewModel.id + "__" + fileGroup.fileGroup,
                        text = fileGroup.fileGroup,
                        spriteCssClass = "image",
                        items = new List<FileTreeViewModel>
                        {
                            new FileTreeViewModel()
                            {
                                id = dataTableTreeViewModel.id + "__" + fileGroup.fileGroup + "__insert",
                                text = "Kaydet",
                                spriteCssClass = "pdf"
                            },
                            new FileTreeViewModel()
                            {
                                id = dataTableTreeViewModel.id + "__" + fileGroup.fileGroup + "__delete",
                                text = "Sil",
                                spriteCssClass = "pdf"
                            },
                            new FileTreeViewModel()
                            {
                                id = dataTableTreeViewModel.id + "__" + fileGroup.fileGroup + "__preview",
                                text = "Göster",
                                spriteCssClass = "pdf"
                            }
                        }.ToList()
                    };
                    if (dataTableTreeViewModel.items == null)
                        dataTableTreeViewModel.items = new List<FileTreeViewModel>();


                    dataTableTreeViewModel.items.Add(fileGroupTreeViewModel);
                }
                liste.Add(dataTableTreeViewModel);

            }


            var db = new WorkOfTimeDatabase();
            var rolList = new List<FileTreeViewModel>();
            var data = db.GetSH_FilesRoleByRoleId(roleId);
            foreach (var datatable in data)
            {

                if (datatable.insert == true)
                {
                    var item = new FileTreeViewModel();
                    item.id = datatable.dataTable + "__" + datatable.fileGroup + "__insert";
                    item.text = "Kaydet";
                    rolList.Add(item);
                }
                if (datatable.delete == true)
                {
                    var item = new FileTreeViewModel();
                    item.id = datatable.dataTable + "__" + datatable.fileGroup + "__delete";
                    item.text = "Sil";
                    rolList.Add(item);
                }
                if (datatable.preview == true)
                {
                    var item = new FileTreeViewModel();
                    item.id = datatable.dataTable + "__" + datatable.fileGroup + "__preview";
                    item.text = "Göster";
                    rolList.Add(item);
                }
            }


            var newData = new DataSendFile
            {
                model = liste,
                filesRole = rolList
            };


            return Content(Infoline.Helper.Json.Serialize(newData), "application/json");
        }
        string FilesSplit(string key, int index)
        {
            var array = key.Split(new[] { "__" }, StringSplitOptions.None);
            if (array.Length > index)
            {
                return array[index];
            }
            return null;
        }
    }
}
