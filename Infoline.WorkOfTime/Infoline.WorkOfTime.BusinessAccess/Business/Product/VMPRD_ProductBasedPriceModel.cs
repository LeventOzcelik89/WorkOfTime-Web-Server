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
        public VMPRD_CompanyBasedPriceModel BasePrice { get; set; }
        //public VMPRD_ProductBasedPriceDetailMode[] BasePriceDetailList { get; set; }
        public VMPRD_CompanyBasedPriceModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetPRD_CompanyBasedPriceById(id);
            if (data != null)
            {
               return this.B_EntityDataCopyForMaterial(data, true);
            }
            return this;
        }
        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetPRD_CompanyBasedPriceById(this.id);
            var res = new ResultStatus { result = true };
            if (BasePrice == null)
            {
                return new ResultStatus { message = "Nesne boş olamaz", result = false };
            }
            //if (BasePriceDetailList == null)
            //{
            //    return new ResultStatus { message = "Ürün detayları boş olamaz", result = false };
            //}
            var validation = Validator();
            if (validation.result == false)
            {
                return validation;
            }
            if (data == null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                
                if (ValidatorForUpsert().result)
                {
                    res = Insert();
                }
                else
                {
                    res = Update();
                }

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
            var res = new ResultStatus { result = true };
            return res;
        }
        private ResultStatus Insert()
        {
            db = db ?? new WorkOfTimeDatabase();
            var dbresult = db.InsertPRD_CompanyBasedPrice(new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(BasePrice), this.trans);
            //foreach (var item in BasePriceDetailList)
            //{
            //    item.companyBasedPriceId = BasePrice.id;
            //    item.Save();
            //}
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Çalışan Durum Değişiklikleri oluşturma işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Çalışan Durum Değişiklikleri oluşturma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdatePRD_CompanyBasedPrice(new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(this), false, this.trans);
            //foreach (var item in BasePriceDetailList)
            //{
            //    item.companyBasedPriceId = BasePrice.id;
            //    item.Save();
            //}
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Çalışan Durum Değişiklikleri güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Çalışan Durum Değişiklikleri güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();

            //İlişkili kayıtlar kontol edilerek silme işlemine müsade edilecek;
            var result= db.GetPRD_CompanyBasedPriceDetailsByCompanyBasedId(this.id);
            var dbresult = new ResultStatus { result = true };
            if (result!=null)
            { 
               dbresult &= db.BulkDeletePRD_CompanyBasedPriceDetail(result,trans);
            }

            dbresult &= db.DeletePRD_CompanyBasedPrice(new PRD_CompanyBasedPrice { id = this.id }, trans);

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
        private ResultStatus ValidatorForUpsert()
        {
            var db = new WorkOfTimeDatabase();
          
            return new ResultStatus { result = true };
        }
    }
}


