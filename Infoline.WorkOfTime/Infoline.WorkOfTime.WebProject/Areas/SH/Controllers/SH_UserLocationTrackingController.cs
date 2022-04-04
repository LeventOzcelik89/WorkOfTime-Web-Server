using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Controllers
{
	public class SH_UserLocationTrackingController : Controller
	{
		[PageInfo("Personel Takip Haritası", SHRoles.IdariPersonelYonetici)]
		public ActionResult Map(UT_LocationUserFilter model)
		{
			return View(model);
		}

		[PageInfo("Personel Takip Haritası", SHRoles.IdariPersonelYonetici)]
		public ActionResult MapAll()
		{
			return View();
		}

		[PageInfo("Personel İzleme Haritası Data Metodu", SHRoles.IdariPersonelYonetici, SHRoles.SatisPersoneli, SHRoles.CRMYonetici, SHRoles.CRMBayiPersoneli, SHRoles.CagriMerkezi)]
		public ContentResult GetMapData(DateTime startDate, DateTime endDate, Guid userId)
		{
			var db = new WorkOfTimeDatabase();
			var trackingDatas = new SH_UserLocationTrackingMap();
			var locationTrackingDatas = db.GetVWUT_LocationTrackingByUserIdAndDates(userId, startDate, endDate);
			if (locationTrackingDatas.Count() > 0)
			{
				var user = db.GetVWSH_UserById(userId);
				trackingDatas.LocationTrackings = locationTrackingDatas;
				trackingDatas.FullName = user.FullName;
				trackingDatas.id = user.id;
				trackingDatas.Title = user.title;
				trackingDatas.ProfilePhoto = user.ProfilePhoto;
				trackingDatas.Department_Title = user.Department_Title;
			}
			return Content(Infoline.Helper.Json.Serialize(trackingDatas), "application/json");
		}

		[AllowEveryone]
		[PageInfo("Personel İzleme Haritası Data Metodu", SHRoles.Personel)]
		public ContentResult GetMapDatas()
		{
			var db = new WorkOfTimeDatabase();
			var locationTrackingDatas = db.GetVWUT_LocationConfigUser().Where(a => a.isTrackingActive == true && a.configName != null).ToList();
			return Content(Infoline.Helper.Json.Serialize(locationTrackingDatas), "application/json");
		}

		[AllowEveryone]
		public ActionResult Update(Guid id)
		{
			var db = new WorkOfTimeDatabase();
			var data = db.GetVWUT_LocationConfigUserById(id);
			return View(data);
		}

		[AllowEveryone]
		public JsonResult Update(VMUT_LocationConfigUserModel model)
		{
			var db = new WorkOfTimeDatabase();
			var userStatus = (PageSecurity)Session["userStatus"];
			var feedback = new FeedBack();

			var dbresult = model.Save(userStatus.user.id);
			var result = new ResultStatusUI
			{
				Result = dbresult.result,
				FeedBack = dbresult.result ? feedback.Success("Kaydetme işlemi başarılı") : feedback.Error("Kaydetme işlemi başarısız")
			};

			return Json(result, JsonRequestBehavior.AllowGet);
		}



	}
}

