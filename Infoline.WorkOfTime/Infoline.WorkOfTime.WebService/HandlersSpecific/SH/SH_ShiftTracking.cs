using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Web.SmartHandlers;
using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Linq;
using System.Collections.Generic;

namespace Infoline.WorkOfTime.WebService.Handler
{
    [Export(typeof(ISmartHandler))]
    public partial class SH_ShiftTrackingHandler : BaseSmartHandler
    {
        public SH_ShiftTrackingHandler()
            : base("SH_ShiftTrackingHandler")
        {

        }

        [HandleFunction("SH_ShiftTracking/GetAll")]
        public void SH_ShiftTrackingGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetSH_ShiftTracking(cond).OrderByDescending((a)=> a.created);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("SH_ShiftTracking/GetLastAction")]
        public void SH_ShiftTrackingGetLastAction(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();

                var userId = CallContext.Current.UserId;
                var data = db.GetSH_ShiftTrackingLastRecordByUserId(userId);

                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("SH_ShiftTracking/MBInsert")]
        public void SH_ShiftTrackingMBInsert(HttpContext context)
        {
            var db = new WorkOfTimeDatabase();
            var userId = CallContext.Current.UserId;

            if (userId == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Personel bulunamadı." });
                return;
            }

            var user = db.GetVWSH_UserById(userId);
            if (user == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Personel bulunamadı." });
                return;
            }

            var tracking = ParseRequest<SH_ShiftTracking>(context);
            if (tracking == null)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Obje dönüştürülemedi." });
                return;
            }

            tracking.userId = userId;
            tracking.companyId = user.CompanyId;

            if (!string.IsNullOrEmpty(tracking.qrCodeDataText))
            {
                //code
                var storage = db.GetCMP_StorageByCode(tracking.qrCodeDataText);
                if (storage != null)
                {
                    tracking.tableId = storage.id;
                    tracking.companyId = storage.companyId;
                    tracking.tableName = "CMP_Storage";
                }
                else
                {
                    //name
                    var company = db.GetCMP_CompanyByCode(tracking.qrCodeDataText);
                    if (company != null)
                    {
                        tracking.tableId = company.id;
                        tracking.tableName = "CMP_Company";
                    }
                    else
                    {
                        var inventory = db.GetPRD_InventoryByCode(tracking.qrCodeDataText);
                        if (inventory != null)
                        {
                            tracking.tableId = inventory.id;
                            tracking.tableName = "PRD_Inventory";
                        }
                    }
                }
            }

            var cog = new Config(TenantConfig.Tenant.TenantCode.Value);
            if (cog != null && cog.shiftQrCodeRequired == true)
            {
                if (tracking.tableId == null)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Okuttuğunuz qr code için depo/şube-cari-ürün bulunamadı." });
                    return;
                }
            }

            tracking.timestamp = DateTime.Now;

            var lastUserTracking = db.GetSH_ShiftTrackingByUserId(userId);

            if (tracking.shiftTrackingStatus == lastUserTracking?.shiftTrackingStatus)
            {
                RenderResponse(context, new ResultStatus { result = false, message = "Son durumunuz zaten aynı.Lütfen kontrol deneyiniz." });
                return;
            }

            var dbresult = db.InsertSH_ShiftTracking(tracking);
            RenderResponse(context, new ResultStatus { result = dbresult.result, message = dbresult.result ? "İşlem Başarılı" : "İşlem Başarısız Oldu." });
        }


        [HandleFunction("SH_ShiftTracking/GetDataReportDetail")]
        public void SH_ShiftTrackingGetDataReportDetail(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var date = context.Request["date"];
                var userId = context.Request["userId"];

                var data = db.VWGetSH_ShiftTrackingByDateAndUserId(Convert.ToDateTime(date), new Guid(userId));

                RenderResponse(context, new ResultStatus { result = true, objects = data });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message });
            }
        }

        [HandleFunction("SH_ShiftTracking/GetDataReport")]
        public void SH_ShiftTrackingGetDataReport(HttpContext context)
        {
            try
            {
                var date = context.Request["date"];
                var userId = context.Request["userId"];
                Guid? user = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    user = new Guid(userId);
                }
                var res = new VMShiftTrackingModel().GetDataReportResult(Convert.ToDateTime(date), user);
                RenderResponse(context, new ResultStatus { result = true, objects = res });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = true, message = ex.Message });
            }
        }
        
        [HandleFunction("SH_ShiftTracking/LastRecordByDeviceId")]
        public void SH_ShiftTrackingGetLastRecordByDeviceId(HttpContext context)
        {
            try
            {
                var deviceId = context.Request["deviceId"];
                var db = new WorkOfTimeDatabase();
                var data = db.GetSH_ShiftTrackingLastRecordByDeviceId(Guid.Parse(deviceId));
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = true, message = ex.Message });
            }
        }

        [HandleFunction("SH_ShiftTracking/PdksInsert")]
        public void SH_ShiftTrackingPdksInstert(HttpContext context)
        {
            try
            {
                var token = context.Request.Headers["token"];
                if (token != "anq5tyqhj1yfbkmre0efr2kw")
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                    RenderResponse(context, new ResultStatus { result = false, message = "Yanlış Token" });
                    return;

                }
                var records = ParseRequest<List<SH_ShiftTracking>>(context); 
                var db = new WorkOfTimeDatabase();
                var dbResult = db.BulkInsertSH_ShiftTracking(records);

                RenderResponse(context, new ResultStatus { result = dbResult.result, message = dbResult.message });
                return;
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus { result = false, message = ex.Message.ToString() });
                return;
            }
        }
    }
}