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

        private ResultStatus Validator(VWPRD_ProductProgressPayment model)
        {
            var result = new ResultStatus { result = true };
            if (model.isActivated == null || model.isActivated == false)
            {
                result.message = "Ürün aktive edilmemiştir.";
                result.result = false;
                return result;
            }
            if (model.isInventory == null || model.isInventory == false)
            {
                result.message = "Ürünün envanterde karşılığı bulunmamaktadır.";
                result.result = false;
                return result;
            }
            return result;
        }

        public ResultStatus Save(Guid? userId,Guid? id, HttpRequestBase request = null, DbTransaction trans = null)
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
            var progressPayment = db.GetPRD_ProductProgressPaymentById(this.id);
            var productBonus = db.GetVWPRD_ProductBonus();
            var productBonusPrice = db.GetVWPRD_ProductBonusPrice();
            foreach (var item in productBonus)
            {
                var data = db.GetPRD_ProductProgressPaymentExistByDataQuery(item.query,this.id);
                if (data.Count() > 0)
                {
                    var bonusPrice = productBonusPrice.Where(a => a.productBonusId == item.id && a.productId == progressPayment.productId).FirstOrDefault();
                    this.price = this.price + (bonusPrice.unitPrice);
                }
            }
           
                var productPayment = new PRD_ProductPayment
                {
                    created = DateTime.Now,
                    createdby = userId,
                    totalPrice = this.price,
                    date = progressPayment.date,
                    id = Guid.NewGuid(),
                    productId = progressPayment.productId,
                    companyId = progressPayment.companyId,
                    productProgressPaymentId = progressPayment.id,
                    hasThePayment = (int)EnumPRD_PRD_ProductPaymentHasThePayment.notPaid,
                };
                result = db.InsertPRD_ProductPayment(productPayment);
                if (result.result)
                {
                    progressPayment.isProgressPayment = (int)EnumPRD_ProductProgressPaymentIsProgressPayment.approved;
                    var dbresult = db.UpdatePRD_ProductProgressPayment(progressPayment);
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
