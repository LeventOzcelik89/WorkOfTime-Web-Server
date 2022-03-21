using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPRD_ProductPaymentModel : VWPRD_ProductProgressPayment
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public double? count { get; set; } = 0;
        public double? price { get; set; } = 0;

        public VMPRD_ProductPaymentModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var productPayment = db.GetVWPRD_ProductPaymentById(this.id);
            if (productPayment != null)
            {
                this.B_EntityDataCopyForMaterial(productPayment, true);
            }
            return this;
        }
        public ResultStatus Approve(VMPRD_ProductPaymentModel model,Guid? userId)
        {
            db = db ?? new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var data = db.GetPRD_ProductPaymentById(model.id);
            if (data==null)
            {
                result.result = false;
                result.message = "Hakediş Bulunamadı.";
                return result;
            }
            if (data.hasThePayment==(int)EnumPRD_PRD_ProductPaymentHasThePayment.paid)
            {
                result.result = false;
                result.message = "Hakediş Daha Önce Onaylanmış.";
                return result;
            }
            data.hasThePayment = (int)EnumPRD_PRD_ProductPaymentHasThePayment.paid;
            data.changed = DateTime.Now;
            data.changedby = userId;
            result = db.UpdatePRD_ProductPayment(data);
            result.message = "İşlem Onaylandı";
            return result;
        }

        public List<ReportModel> ReportData(Guid companyId, int year, int month)
        {
            db = db ?? new WorkOfTimeDatabase();
            var list = new List<ReportModel>();
            var data = db.GetVWPRD_ProductPayment().Where(a => a.date.Value.Month == month && a.date.Value.Year == year && a.companyId == companyId).ToArray();
            foreach (var item in data)
            {
                var priceAllPaid = data.Where(a => a.hasThePayment == (int)EnumPRD_PRD_ProductPaymentHasThePayment.paid).Sum(a => a.totalPrice);
                var priceAllNotPaid = data.Where(a => a.hasThePayment == (int)EnumPRD_PRD_ProductPaymentHasThePayment.notPaid).Sum(a => a.totalPrice);
                var quantityTotal = data.Count();
                list.Add(new ReportModel
                {
                    companyId_Title = item.companyId_Title,
                    date = item.date,
                    hasThePayment = item.hasThePayment,
                    totalPrice = item.totalPrice,
                    totalPriceAllPaid = priceAllPaid,
                    totalPriceAllNotPaid = priceAllNotPaid,
                    totalQuantity = quantityTotal,
                    productId_Title = item.productId_Title,
                    id = item.id
                });
            }
            return list;
        }
    }
    public class ReportModel
    {
        public Guid id { get; set; }
        public string companyId_Title { get; set; }
        public double? totalPrice { get; set; }
        public DateTime? date { get; set; }
        public short? hasThePayment { get; set; }
        public string productId_Title { get; set; }
        public int totalQuantity { get; set; }
        public double? totalPriceAllPaid { get; set; }
        public double? totalPriceAllNotPaid { get; set; }
    }
}
