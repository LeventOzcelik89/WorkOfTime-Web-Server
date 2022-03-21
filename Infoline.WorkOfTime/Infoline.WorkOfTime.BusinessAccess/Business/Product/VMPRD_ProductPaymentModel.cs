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

        private ResultStatus Validator(VWPRD_ProductProgressPayment model)
        {
            var result = new ResultStatus { result = true };
            return result;
        }

        //public ResultStatus ReportData(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        //{
        //    db = db ?? new WorkOfTimeDatabase();
        //    var progressPayment = db.GetVWPRD_ProductProgressPaymentById(this.id);
        //    var rs = new ResultStatus { result = true };
        //    if (progressPayment != null)
        //    {
        //        var validate = Validator(progressPayment);
        //        if (!validate.result)
        //        {
        //            return validate;
        //        }
        //        this.createdby = userId;
        //        this.created = DateTime.Now;
        //        rs = Approve(userId, trans);
        //    }
        //    return rs;
        //}
    }
}
