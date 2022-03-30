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
        public double? additionalPrice { get; set; } = 0;
        public Guid productBonusId { get; set; }

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

        private ResultStatus Validator(VWPRD_ProductProgressPayment model)
        {
            var result = new ResultStatus { result = true };
            if (model.isActivated == null || model.isActivated == false)
            {
                result.message = "Ürün aktive edilmemiştir.";
                result.result = false;
                return result;
            }
            return result;
        }

        public ResultStatus Save(Guid? userId, Guid? id, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var progressPayment = db.GetVWPRD_ProductProgressPaymentById(id.Value);
            var rs = new ResultStatus { result = true };
            if (progressPayment != null)
            {
                var validate = Validator(progressPayment);
                if (!validate.result)
                {
                    return validate;
                }
                this.createdby = userId;
                this.created = DateTime.Now;
                this.id = progressPayment.id;
                rs = Approve(userId, trans);
            }
            else
            {
                rs.message = "Satış Bulunamadı.";
                rs.result = false;
            }
            return rs;
        }
        private ResultStatus Approve(Guid? userId, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var dbResult = new ResultStatus { result = true };
            var progressPayment = db.GetPRD_ProductProgressPaymentById(this.id);
            result = PaymentBounty(progressPayment, userId);
            if (result.result)
            {
                progressPayment.isProgressPayment = (int)EnumPRD_ProductProgressPaymentIsProgressPayment.approved;
                dbResult = db.UpdatePRD_ProductProgressPayment(progressPayment);
            }
            return result;
        }

        public ResultStatus PaymentBounty(PRD_ProductProgressPayment model, Guid? userId)
        {
            db = db ?? new WorkOfTimeDatabase();
            var result = new ResultStatus { result = true };
            var productBonus = db.GetVWPRD_ProductBonus();
            var productBonusPrice = db.GetVWPRD_ProductBonusPrice();
            foreach (var item in productBonus)
            {
                if (model.date >= item.startDate && model.date <= item.endDate)
                {
                    var start = item.startDate.Value.ToString("yyyy-MM-dd HH:mm");
                    var end = item.endDate.Value.ToString("yyyy-MM-dd HH:mm");
                    var data = db.GetPRD_BountyProductExistByDataQuery(item.query, model.productId, start, end);
                    if (data.Count() > 0)
                    {
                        var bonusPrice = productBonusPrice.Where(a => a.productBonusId == item.id && a.productId == model.productId).FirstOrDefault();
                        this.price = this.price + (bonusPrice.unitPrice);
                        this.additionalPrice = bonusPrice.present;
                        this.productBonusId = item.id;
                    }
                }
            }
            var bounty = new PRD_Bounty
            {
                created = DateTime.Now,
                createdby = userId,
                productTotalPrice = this.price,
                additionalPrice = this.additionalPrice,
                totalPrice = this.price + this.additionalPrice,
                companyId = model.companyId,
                id = Guid.NewGuid(),
                paymentDate = DateTime.Now,
                productBonusId = this.productBonusId,
                status = (int)EnumPRD_BountyStatus.notPayment,
            };
            var bountyProduct = new PRD_BountyProduct
            {
                id = Guid.NewGuid(),
                bountyId = bounty.id,
                imei = model.imei,
                price = this.price,
                productId = model.productId,
                salesDate = model.date,
            };
            result &= db.InsertPRD_Bounty(bounty);
            result &= db.InsertPRD_BountyProduct(bountyProduct);
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
