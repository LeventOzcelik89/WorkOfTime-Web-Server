﻿using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.Web.Utility;
using Kendo.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Infoline.Framework.Database;

namespace Infoline.WorkOfTime.WebProject.Areas.SV.Controllers
{
    public class VWSV_DeviceProblemController : Controller
    {
        [PageInfo("Servise Ait Cihazın Sorunlarının Listelendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Index()
        {
            return View();
        }

        [PageInfo("Servise Ait Cihazın Sorunlarının Listelendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu, SHRoles.CagriMerkezi)]
        public ContentResult DataSource([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var page = request.Page;
            request.Filters = new FilterDescriptor[0];
            request.Sorts = new SortDescriptor[0];
            request.Page = 1;
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSV_DeviceProblem(condition).RemoveGeographies().ToDataSourceResult(request);
            data.Total = db.GetVWSV_DeviceProblemCount(condition.Filter);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }
        [PageInfo("Servise Ait Cihazın Sorunlarının Listelendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu, SHRoles.CagriMerkezi)]

        public ContentResult DataSourceDropDown([DataSourceRequest] DataSourceRequest request)
        {
            var condition = KendoToExpression.Convert(request);

            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSV_DeviceProblem(condition);
            return Content(Infoline.Helper.Json.Serialize(data), "application/json");
        }

        [PageInfo("Servise Ait Cihazın Sorunlarının Detayının Olduğu Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Detail(Guid id)
        {

            var data = new VMSV_DeviceProblemModel { id = id }.Load();
            return View(data);
        }

        [PageInfo("Servise Ait Cihazın Sorunlarının Eklendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Insert()
        {
            var data = new VWSV_DeviceProblem { id = Guid.NewGuid() };
            return View(data);
        }

        [PageInfo("Servise Ait Cihazın Sorunlarının Eklendiği Method", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SV_DeviceProblem item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            item.created = DateTime.Now;
            item.createdby = userStatus.user.id;
            var dbresult = db.InsertSV_DeviceProblem(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [PageInfo("Servise Ait Cihazın Sorunlarının Güncellendiği Sayfa", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var data = db.GetVWSV_DeviceProblemById(id);
            return View(data);
        }
        [PageInfo("Servise Ait Cihazın Sorunlarının Güncellendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SV_DeviceProblem item)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();

            item.changed = DateTime.Now;
            item.changedby = userStatus.user.id;

            var dbresult = db.UpdateSV_DeviceProblem(item);
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Güncelleme işlemi başarılı") : feedback.Error("Güncelleme işlemi başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Servise Ait Cihazın Sorunlarının Silindiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = new VMSV_DeviceProblemModel { id = id }.Delete(userStatus.user.id);

            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Cihaz Sorunu Silme İşlemi Başarılı") : feedback.Warning("Cihaz Sorunu Silme İşlemi Başarısız")
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [PageInfo("Servise Ait Cihazın Teknik Servis  Sorunlarının Eklendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu )]
        public ActionResult AddMultipleDeviceProblem(VMSV_DeviceProblemModel model)
        {
            return View(model);
        }
        [PageInfo("Servise Ait Cihazın Teknik Servis  Sorunlarının Eklendiği Metod", SHRoles.TeknikServisYoneticiRolu, SHRoles.TeknikServisBayiRolu)]
        public JsonResult InsertMultiple(VMSV_DeviceProblemModel model)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)Session["userStatus"];
            var feedback = new FeedBack();
            var dbresult = new ResultStatus { result = true };
            var trans = db.BeginTransaction();
            if (model.Problems != null)
            {
                foreach (var item in model.Problems)
                {
                    item.type = (short)EnumSV_DeviceProblemType.Service;
                    item.serviceId = model.serviceId;
                    item.inventoryId = model.inventoryId;
                    dbresult &= item.Save(userStatus.user.id, null, trans);
                }
                dbresult &= new VMSV_ServiceOperationModel
                {
                    description = "Teknik Servis Bulgusu Eklendi",
                    serviceId = model.serviceId,
                    status = (int)EnumSV_ServiceOperation.PartDefinied,
                }.Save(userStatus.user.id, null, trans);
            }
            if (model.ServiceNotifications != null)
            {
                foreach (var item in model.ServiceNotifications)
                {
                    dbresult &= new VMSV_ServiceOperationModel
                    {

                        dataId = item.id,
                        description = (db.GetVWPRD_ProductById(item.id)?.currentServicePrice * item.quantity).ToString() ?? "",
                        dataTable = "PRD_Product",
                        serviceId = model.serviceId,
                        status = (short)EnumSV_ServiceOperation.ServicePriceAdded
                    }.Save(userStatus.user.id, null, trans);
                }
            }
            if (dbresult.result)
            {
                trans.Commit();
            }
            else
            {
                Log.Error(dbresult.message);
                trans.Rollback();
            }
            var result = new ResultStatusUI
            {
                Result = dbresult.result,
                FeedBack = dbresult.result ? feedback.Success("Cihaz Problemleri Tanımlaması Başarılı",false,Request.UrlReferrer.AbsoluteUri) : feedback.Warning("Cihaz Problemleri Tanımlaması Başarısız")
            };
            return Json(result, JsonRequestBehavior.AllowGet);

        }
    }
}
