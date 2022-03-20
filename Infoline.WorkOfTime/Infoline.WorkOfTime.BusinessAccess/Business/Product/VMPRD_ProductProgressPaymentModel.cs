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
    public class VMPRD_ProductProgressPaymentModel : VWPRD_ProductProgressPayment
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public double? count { get; set; } = 0;
        public double? price { get; set; } = 0;

        public VMPRD_ProductProgressPaymentModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var progressPayment = db.GetVWPRD_ProductProgressPaymentById(this.id);
            if (progressPayment != null)
            {
                this.B_EntityDataCopyForMaterial(progressPayment, true);
            }
            return this;
        }

        private ResultStatus Validator()
        {
            var result = new ResultStatus { result = true };
            if (this.isActivated == null || this.isActivated == false)
            {
                result.message = "Ürün aktive edilmemiştir.";
                result.result = false;
                return result;
            }
            if (this.isInventory == null || this.isInventory == false)
            {
                result.message = "Ürünün envanterde karşılığı bulunmamaktadır.";
                result.result = false;
                return result;
            }
            return result;
        }

        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var progressPayment = db.GetVWPRD_ProductProgressPaymentById(this.id);
            var rs = new ResultStatus { result = true };
            var validate = Validator();
            if (!validate.result)
            {
                return validate;
            }
            if (progressPayment != null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                rs = Approve(userId, trans);
            }
            return rs;
        }
        private ResultStatus Approve(Guid? userId, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var progressPayment = db.GetVWPRD_ProductProgressPaymentById(this.id);
            var productBonus = db.GetVWPRD_ProductBonus();
            var productBonusPrice = db.GetVWPRD_ProductBonusPrice();
            foreach (var item in productBonus)
            {
                var data = db.GetPRD_ProductProgressPaymentExistByDataQuery(item.query);
                if (data.Count() > 0)
                {
                    var bonusPrice = productBonusPrice.Where(a => a.productBonusId == item.id && a.productId == progressPayment.productId).FirstOrDefault();
                    this.count = data.Count();
                    this.price = this.price + (bonusPrice.unitPrice * this.count);
                }
            }
            if (this.price > 0)
            {
                var productPayment = new PRD_ProductPayment
                {
                    created = DateTime.Now,
                    createdby = userId,
                    totalPrice = this.price,
                    date = progressPayment.date,
                    id = Guid.NewGuid(),
                    companyId = progressPayment.companyId,
                    productProgressPaymentId = progressPayment.id,
                    hasThePayment = (int)EnumPRD_PRD_ProductPaymentHasThePayment.notPaid,
                };
                result = db.InsertPRD_ProductPayment(productPayment);
            }
            return result;
        }
        public static SimpleQuery UpdateDataSourceFilterMix(SimpleQuery query, PageSecurity userStatus)
        {
            BEXP filter = new BEXP
            {
                Operand1 = (COL)"isProgressPayment",
                Operator = BinaryOperator.Equal,
                Operand2 = (VAL)(int)EnumPRD_ProductProgressPaymentIsProgressPayment.approving
            };
            filter &= new BEXP
            {
                Operand1 = (COL)"CMPTypes_Title",
                Operator = BinaryOperator.Like,
                Operand2 = (VAL)"%Bayi%"
            };
            filter &= new BEXP
            {
                Operand1 = (COL)"CMPTypes_Title",
                Operator = BinaryOperator.Like,
                Operand2 = (VAL)"%Mix%"
            };
            query.Filter &= filter;
            return query;
        }
        public static SimpleQuery UpdateDataSourceFilterOperator(SimpleQuery query, PageSecurity userStatus)
        {

            BEXP filter = new BEXP
            {
                Operand1 = (COL)"isProgressPayment",
                Operator = BinaryOperator.Equal,
                Operand2 = (VAL)(int)EnumPRD_ProductProgressPaymentIsProgressPayment.approving
            };
            filter &= new BEXP
            {
                Operand1 = (COL)"CMPTypes_Title",
                Operator = BinaryOperator.Like,
                Operand2 = (VAL)"%Bayi%"
            };
            filter &= new BEXP
            {
                Operand1 = (COL)"CMPTypes_Title",
                Operator = BinaryOperator.NotLike,
                Operand2 = (VAL)"%Mix%"
            };
            query.Filter &= filter;
            return query;
        }
    }
}
