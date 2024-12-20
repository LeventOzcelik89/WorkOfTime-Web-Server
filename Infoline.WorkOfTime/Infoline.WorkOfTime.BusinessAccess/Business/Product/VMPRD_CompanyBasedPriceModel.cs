﻿using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess.Business.Product
{
    public class VMPRD_CompanyBasedPriceModel : VWPRD_CompanyBasedPrice
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public List<VMPRD_CompanyBasedPriceDetailModel> BasePriceDetailItems { get; set; } = new List<VMPRD_CompanyBasedPriceDetailModel>();
        public short discountType { get; set; }
        public VMPRD_CompanyBasedPriceModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWPRD_CompanyBasedPriceById(id);
            if (data != null)
            {
                var getSubData = db.GetPRD_CompanyBasedPriceDetailsByCompanyBasedId(data.id);
                if (getSubData != null)
                {
                    
                    foreach (var item in getSubData.OrderByDescending(x=>x.startDate))
                    {
                        if (item.discount<0)
                        {
                            item.discount = item.discount * -1;
                            this.discountType = 1;
                        }
                        else
                        {
                            this.discountType = 0;
                        }
                        
                        BasePriceDetailItems.Add(new VMPRD_CompanyBasedPriceDetailModel().B_EntityDataCopyForMaterial(item));
                    }
                }
                return this.B_EntityDataCopyForMaterial(data, true);
            }
            return this;
        }
        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            this.trans = transaction ?? db.BeginTransaction();
            var data = db.GetPRD_CompanyBasedPriceById(this.id);
            var res = new ResultStatus { result = true };
            var validation = Validator();
            if (validation.result == false)
            {
                return validation;
            }
            if (data == null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                res = Insert(userId);
            }
            else
            {
                this.changedby = userId;
                this.changed = DateTime.Now;
                res = Update();
            }
            if (res.result)
            {
                if (transaction == null) trans.Commit();
            }
            else
            {
                if (transaction == null) trans.Rollback();
            }
            return res;
        }
        private ResultStatus Validator()
        {
            db = db ?? new WorkOfTimeDatabase();
            var isExistBefore = db.GetDBPRD_CompanyBasedPriceByAllAttributes(new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(this));
            if (isExistBefore != null)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Seçmiş olduğunuz şirket - ürün - satış tipi ve koşul tipi aynı olan bir kayıt vardır. Eklemek istediğiniz yeni fiyat/iskonto bilgileri lütfen o kayıt altına giriniz."
                };
            }
            var res = new ResultStatus { result = true };
            return res;
        }
        private ResultStatus Insert(Guid? userId)
        {
            db = db ?? new WorkOfTimeDatabase();
            var validation = Validator();
            if (validation.result == false)
            {
                return validation;
            }
            var dbresult = db.InsertPRD_CompanyBasedPrice(new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(this), this.trans);
            if (this.BasePriceDetailItems.Count > 0)
            {
                foreach (var item in BasePriceDetailItems)
                {
                    if (item.discount>100)
                    {
                        dbresult.result = false;
                        dbresult.message = "İskonto 100'den büyük olamaz";
                    }
                    if (productType == (short)EnumPRD_CompanyBasedPriceProductType.AllProducts || productType == (short)EnumPRD_CompanyBasedPriceProductType.SelectedCategory)
                    {
                        item.price = null;
                    }
                    if (productType == (short)EnumPRD_CompanyBasedPriceProductType.SelectedProduct && type == (short)EnumPRD_CompanyBasedPriceType.Fiyat)
                    {
                        item.discount = null;
                    }
                    if (productType == (short)EnumPRD_CompanyBasedPriceProductType.SelectedProduct && type == (short)EnumPRD_CompanyBasedPriceType.Oran)
                    {
                        item.price = null;
                    }
                    if (sellingType == (short)EnumPRD_CompanyBasedPriceSellingType.Genel || sellingType == (short)EnumPRD_CompanyBasedPriceSellingType.Peşin)
                    {
                        item.monthCount = null;
                    }
                    if (conditionType == (short)EnumPRD_CompanyBasedPriceConditionType.Genel)
                    {
                        item.minCondition = null;
                    }
                    if (discountType==1)
                    {
                        item.discount = item.discount * -1;
                    }
                    item.companyBasedPriceId = id;
                    dbresult &= item.Insert(trans);
                }
            }
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = dbresult.message
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Müşteri bazlı fiyat oluşturma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            var validation = Validator();
            if (validation.result == false)
            {
                return validation;
            }
            dbresult &= db.UpdatePRD_CompanyBasedPrice(new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(this), false, this.trans);
            if (this.BasePriceDetailItems.Count > 0)
            {
                foreach (var item in BasePriceDetailItems)
                {
                    item.companyBasedPriceId = id;
                    dbresult &= item.Save(changedby, null, trans);
                }
            }
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Müşteri bazlı fiyat güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Müşteri bazlı fiyat güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var result = db.GetPRD_CompanyBasedPriceById(this.id);
            var dbresult = new ResultStatus { result = true };
            if (result != null)
            {
                dbresult &= db.DeletePRD_CompanyBasedPrice(new PRD_CompanyBasedPrice { id = this.id }, trans);
            }
            if (!dbresult.result)
            {
                trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Kayıt Silme İşlemi Başarısız"
                };
            }
            else
            {
                trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Kayıtlar başarılı bir şekilde silinmiştir."
                };
            }
        }
    }
}
