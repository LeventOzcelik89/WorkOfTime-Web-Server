using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Web.Mvc;
using System.Collections.Generic;
using Infoline.WorkOfTime.BusinessData;
using System.Linq;

namespace Infoline.WorkOfTime.WebProject.Areas.SYS.Controllers
{
	public class VWSYS_TableAdditionalPropertyController : Controller
	{
        [PageInfo("Ek özellikler", SHRoles.Personel)]
        public ActionResult Index(Guid DataId, string DataTable)
		{
            var db = new WorkOfTimeDatabase();
            var data = TenantConfig.Tenant.Config.CustomProperty[DataTable];
            var list = new List<VWSYS_TableAdditionalProperty>();

            var properties = db.GetVWSYS_TableAdditionalPropertyByDataIdAndDataTable(DataId, DataTable);
            
            foreach (var item in data)
            {
                list.Add(new VWSYS_TableAdditionalProperty { 
                    dataTable = DataTable,
                    dataId = DataId,
                    propertyKey = item,
                    propertyValue = properties.Where(a => a.propertyKey.ToLower() == item.ToLower()).Select(a=>a.propertyValue).FirstOrDefault()
                });
            }

            return View(list.ToArray());
        }

        [PageInfo("Cari detayı özellikler", SHRoles.Personel)]
        public ActionResult Detail(Guid DataId, string DataTable) {
            var db = new WorkOfTimeDatabase();
            var model = new VMSYS_TableAdditionalProperty
            {
                properties = db.GetVWSYS_TableAdditionalPropertyByDataIdAndDataTable(DataId, DataTable),
                dataId = DataId,
                dataTable = DataTable
            };
            return View(model);
        }

        public ActionResult DetailINVCompany(Guid DataId, string DataTable)
        {
            var db = new WorkOfTimeDatabase();
            var properties = db.GetVWSYS_TableAdditionalPropertyByDataIdAndDataTable(DataId, DataTable);
            return View(properties);
        }

        [PageInfo("Ek özellik ekle",SHRoles.Personel)]
        public ActionResult Insert(SYS_TableAdditionalProperty item)
        {
            return View(item);
        }

        [PageInfo("Ek özellik ekle methodu", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Insert(SYS_TableAdditionalProperty item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();

            if (!item.dataId.HasValue)
            {
                return Json(new ResultStatusUI
                {
                    Result = false,
                    FeedBack = feedback.Warning("idsi boş olamaz."),
                });
            }

            var dbres = db.InsertSYS_TableAdditionalProperty(item);
            var data = db.GetSYS_TableAdditionalPropertyByDataIdAndDataTable(item.dataId.Value, item.dataTable);

            return Json(new ResultStatusUI
            {
                Result = dbres.result,
                FeedBack = dbres.result ? feedback.Success("Ek özellik ekleme işlemi başarılı") : feedback.Warning("Ek özellik ekleme işlemi başarısız oldu."),
                Object = data
            });
        }

        [PageInfo("Ek özellik düzenle", SHRoles.Personel)]
        public ActionResult Update(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var sysadditional = db.GetSYS_TableAdditionalPropertyById(id);
            return View(sysadditional);
        }

        [PageInfo("Ek özellik düzenle", SHRoles.Personel)]
        [HttpPost, ValidateAntiForgeryToken]
        public JsonResult Update(SYS_TableAdditionalProperty item, bool? isPost)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var dbres = db.UpdateSYS_TableAdditionalProperty(item);
            var data = db.GetSYS_TableAdditionalPropertyByDataIdAndDataTable(item.dataId.Value, item.dataTable);
            return Json(new ResultStatusUI
            {
                Result = dbres.result,
                FeedBack = dbres.result ? feedback.Success("Ek özellik güncelleme işlemi başarılı") : feedback.Warning("Ek özellik güncelleme işlemi başarısız oldu."),
                Object = data
            });
        }

        [PageInfo("Ek özellik silme", SHRoles.Personel)]
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var feedback = new FeedBack();
            var data = db.GetSYS_TableAdditionalPropertyById(id);
            var dbres = db.DeleteSYS_TableAdditionalProperty(data);
            return Json(new ResultStatusUI
            {
                 Result = dbres.result,
                 FeedBack = dbres.result ? feedback.Success("Ek özellik silme işlemi başarılı") : feedback.Warning("Ek özellik silme işlemi başarısız oldu")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
