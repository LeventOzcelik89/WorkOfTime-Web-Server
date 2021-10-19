using Infoline.Framework.Database;
using Infoline.Web.SmartHandlers;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.Web;

namespace Infoline.WorkOfTime.WebService
{
    [Export(typeof(ISmartHandler))]
    public partial class VWCMP_TenderHandlers : BaseSmartHandler
    {
        public object VWCMP_TenderModels { get; private set; }

        public VWCMP_TenderHandlers()
            : base("VWCMP_Tender")
        {

        }

        [HandleFunction("VWCMP_Tender/GetAll")]
        public void VWCMP_TenderGetAll(HttpContext context)
        {
            try
            {
                var c = ParseRequest<Condition>(context);
                var cond = c != null ? CondtionToQuery.Convert(c) : new SimpleQuery();
                var db = new WorkOfTimeDatabase();
                var data = db.GetVWCMP_Tender(cond);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCMP_Tender/GetById")]
        public void CMP_TenderTransactionGetById(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var id = context.Request["id"];
                var datas = new VMCMP_TenderModels { id = new Guid(id) }.Load(false, null);
                RenderResponse(context, datas);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCMP_Tender/Insert")]
        public void CMP_TenderInsert(HttpContext context)
        {
            try
            {
                var db = new WorkOfTimeDatabase();
                var model = ParseRequest<VMCMP_TenderModels>(context);
                var userId = CallContext.Current.UserId;
                var rs = model.Save(userId);
                RenderResponse(context, new ResultStatus() { result = true, message = "Teklif başarılı bir şekilde oluşturuldu.", objects = null });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCMP_Tender/GetRateExchange")]
        public void CMP_TenderGetRateExchange(HttpContext context)
        {
            try
            {
                var date = context.Request["date"];
                var convertDate = Convert.ToDateTime(date);
                if (convertDate > DateTime.Now.Date)
                {
                    RenderResponse(context, new ResultStatus { result = false, message = "Lütfen gelecek tarih girmeyiniz." });
                    return;
                }
                else if (convertDate == DateTime.Now.Date)
                {
                    RenderResponse(context, CurrencyExchangeRates.GetAllCurrenciesTodaysExchangeRates());
                    return;
                }

                else
                {
                    RenderResponse(context, CurrencyExchangeRates.GetAllCurrenciesHistoricalExchangeRates(convertDate));
                    return;
                }
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCMP_Tender/GetPageInfo")]
        public void VWCMP_TenderGetPageInfo(HttpContext context)
        {
            try
            {
                var userId = CallContext.Current.UserId;
                var data = new VMCMP_TenderModels().GetMyTenderPageInfo(userId);
                RenderResponse(context, data);
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

        [HandleFunction("VWCMP_Tender/TenderTransferToOrder")]
        public void VWCMP_TenderTransferToOrder(HttpContext context)
        {
            try
            {
                var userId = CallContext.Current.UserId;
                var db = new WorkOfTimeDatabase();
                var model = ParseRequest<VMCMP_OrderModels>(context);
                var res = model.Save(userId);
                RenderResponse(context, new ResultStatus() { result = true, message = "Teklif Başarılı Bir Şekilde Siparişe Dönüştürüldü." });
            }
            catch (Exception ex)
            {
                RenderResponse(context, new ResultStatus() { result = false, message = ex.Message.ToString() });
            }
        }

    }
}
