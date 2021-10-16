using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.FTM.Controllers
{
    public class VWFTM_TaskFormController : Controller
    {
        [PageInfo("Görev Formları", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Envantar Görev Formları DataSource", SHRoles.Personel)]
        public ContentResult DataSource([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var userStatus = (PageSecurity)Session["userStatus"];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            condition = new VMFTM_TaskModel().UpdateQuery(condition, userStatus, 4);
            var data = db.GetVWFTM_TaskForm(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWFTM_TaskFormCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [AllowEveryone]
        [PageInfo("Envantar Görev Miktar DataSource", SHRoles.Personel)]
        public int DataSourceCount([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            condition = new VMFTM_TaskModel().UpdateQuery(condition, userStatus, 4);
            var count = db.GetVWFTM_TaskFormCount(condition.Filter);
            return count;
        }

        [AllowEveryone]
        [PageInfo("Envantar Görev Formları (Dropdown)")]
        public ContentResult DataSourceDropDown([DataSourceRequest]DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWFTM_TaskForm(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Envantar Görev Form Detayı", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult Detail(VMFTM_TaskForm model)
        {
            var db = new WorkOfTimeDatabase();
            var formRelations = db.GetFTM_TaskFormRelationByFormId(model.id);
            var form = db.GetVWFTM_TaskFormById(model.id);
            model = model.EntityDataCopyForMaterial(form, false);
            model.productId = formRelations.Select(a => a.productId).FirstOrDefault();
            model.inventoryId = formRelations.Select(a => a.inventoryId).ToArray();
            return View(model);
        }

        [PageInfo("Envantar Görev Formu Ekleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult Insert(VMFTM_TaskForm model)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWFTM_TaskFormById(model.id);
            if (data != null)
            {
                var formRelations = db.GetFTM_TaskFormRelationByFormId(model.id);
                model = model.EntityDataCopyForMaterial(data, false);
                model.id = Guid.NewGuid();
                model.code = BusinessExtensions.B_GetIdCode();
                model.name = model.name + " Kopya";
                model.productId = formRelations.Select(a => a.productId).FirstOrDefault();
                model.inventoryId = formRelations.Select(a => a.inventoryId).ToArray();
                model.companyId = formRelations.Select(a => a.companyId).FirstOrDefault();
                model.companyStorageId = formRelations.Select(a => a.companyStorageId).FirstOrDefault();
            }
            else
            {
                model.isActive = (int)EnumFTM_TaskFormIsActive.Aktif;
                model.code = BusinessExtensions.B_GetIdCode();
                model.type = model.type ?? (int)EnumFTM_TaskType.Bakim;
            }

            return View(model);
        }

        [HttpPost]
        [PageInfo("Envantar Görev Formu Ekleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public JsonResult Insert(VMFTM_TaskForm item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var userStatus = (PageSecurity)Session["userStatus"];
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;

            if (string.IsNullOrEmpty(item.json))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Form oluşturulmamış kaydedemezsiniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            var taskform = db.GetFTM_TaskFormById(item.id);
            if (taskform != null)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Form kaydetme işleminde sorun oluştu.")
                }, JsonRequestBehavior.AllowGet);
            }

            var trans = db.BeginTransaction();
            var form = new FTM_TaskForm { }.EntityDataCopyForMaterial(item, true);
            var taskFormRelationList = new List<FTM_TaskFormRelation>();

            if (item.productId.HasValue)
            {
                if (item.inventoryId != null && item.inventoryId.Count() > 0)
                {
                    taskFormRelationList.AddRange(item.inventoryId.Select(a => new FTM_TaskFormRelation
                    {
                        productId = item.productId,
                        formId = form.id,
                        inventoryId = a,
                        companyId = item.companyId,
                        companyStorageId = item.companyStorageId,
                        created = form.created,
                        createdby = form.createdby,
                    }));
                }
                else
                {
                    taskFormRelationList.Add(new FTM_TaskFormRelation
                    {
                        productId = item.productId,
                        formId = form.id,
                        inventoryId = null,
                        companyId = item.companyId,
                        companyStorageId = item.companyStorageId,
                        created = form.created,
                        createdby = form.createdby,
                    });
                }
            }

            if (item.companyId.HasValue)
            {
                taskFormRelationList.Add(new FTM_TaskFormRelation
                {
                    formId = item.id,
                    inventoryId = null,
                    companyId = item.companyId,
                    companyStorageId = item.companyStorageId,
                    created = form.created,
                    createdby = form.createdby,
                });
            }

            var dbresult = db.InsertFTM_TaskForm(form, trans);
            dbresult &= db.BulkInsertFTM_TaskFormRelation(taskFormRelationList, trans);

            if (dbresult.result) trans.Commit(); else trans.Rollback();

            return Json(new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı", false, Url.Action("Index", "VWFTM_TaskForm", new { area = "FTM" })) : feedback.Warning("Kaydetme işlemi başarısız")
            }, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Envantar Görev Formu Düzenleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var model = new VMFTM_TaskForm().EntityDataCopyForMaterial(db.GetVWFTM_TaskFormById(id), true);
            var formRelations = db.GetFTM_TaskFormRelationByFormId(id);
            model.productId = formRelations.Select(a => a.productId).FirstOrDefault();
            model.inventoryId = formRelations.Select(a => a.inventoryId).ToArray();
            model.companyId = formRelations.Select(a => a.companyId).FirstOrDefault();
            model.companyStorageId = formRelations.Select(a => a.companyStorageId).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        [PageInfo("Envantar Görev Formu Düzenleme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevYonetici)]
        public JsonResult Update(VMFTM_TaskForm item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            var trans = db.BeginTransaction();

            if (string.IsNullOrEmpty(item.json))
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Form oluşturulmamış kaydedemezsiniz.")
                }, JsonRequestBehavior.AllowGet);
            }

            var formDB = db.GetVWFTM_TaskFormById(item.id);
            var formRelations = db.GetFTM_TaskFormRelationByFormId(item.id);
            var form = new FTM_TaskForm
            {
                id = item.id,
                created = formDB.created,
                createdby = formDB.createdby,
                changed = DateTime.Now,
                changedby = userStatus.user.id,
                name = item.name,
                code = item.code,
                type = item.type,
                isActive = item.isActive,
                json = item.json,
            };

            var taskFormRelationList = new List<FTM_TaskFormRelation>();

            if (item.productId.HasValue)
            {
                if (item.inventoryId != null && item.inventoryId.Count() > 0)
                {
                    taskFormRelationList.AddRange(item.inventoryId.Select(a => new FTM_TaskFormRelation
                    {
                        productId = item.productId,
                        formId = item.id,
                        inventoryId = a,
                        companyId = item.companyId,
                        companyStorageId = item.companyStorageId,
                        created = form.created,
                        createdby = form.createdby,
                    }));
                }
                else
                {
                    taskFormRelationList.Add(new FTM_TaskFormRelation
                    {
                        productId = item.productId,
                        formId = item.id,
                        inventoryId = null,
                        companyId = item.companyId,
                        companyStorageId = item.companyStorageId,
                        created = form.created,
                        createdby = form.createdby,
                    });
                }
            }
			if (item.companyId.HasValue)
			{
                taskFormRelationList.Add(new FTM_TaskFormRelation
                {
                    formId = item.id,
                    inventoryId = null,
                    companyId = item.companyId,
                    companyStorageId = item.companyStorageId,
                    created = form.created,
                    createdby = form.createdby,
                });
            }

            var dbresult = db.UpdateFTM_TaskForm(form, true, trans);
            dbresult &= db.BulkDeleteFTM_TaskFormRelation(formRelations, trans);
            dbresult &= db.BulkInsertFTM_TaskFormRelation(taskFormRelationList, trans);

            if (dbresult.result) trans.Commit(); else trans.Rollback();

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı", false, Url.Action("Update", "VWFTM_TaskForm", new { area = "FTM", item.id })) : feedback.Warning("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [PageInfo("Envantar Görev Formu Silme", SHRoles.SahaGorevOperator, SHRoles.SahaGorevPersonel, SHRoles.SahaGorevYonetici)]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var trans = db.BeginTransaction();
            var formRelations = db.GetFTM_TaskFormRelationByFormId(id);
            var form = db.GetVWFTM_TaskFormById(id);
            if (form.usedCount > 0)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("Form kullanıldığından form silinemez.")
                }, JsonRequestBehavior.AllowGet);
            }

            var dbresult = db.BulkDeleteFTM_TaskFormRelation(formRelations, trans);
            dbresult &= db.DeleteFTM_TaskForm(id, trans);

            if (dbresult.result) trans.Commit(); else trans.Rollback();
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Silme işlemi başarılı") : feedback.Warning("Silme işlemi başarılı")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}

