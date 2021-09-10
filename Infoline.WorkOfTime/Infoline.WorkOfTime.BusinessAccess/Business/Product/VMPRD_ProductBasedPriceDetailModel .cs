using Infoline.Framework.Database;
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
    public class VMPRD_CompanyBasedPriceDetailModel: VWPRD_CompanyBasedPriceDetail
    {

        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VMPRD_CompanyBasedPriceDetailModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetPRD_CompanyBasedPriceById(this.id);
            if (data != null)
            {
                this.B_EntityDataCopyForMaterial(data, true);
            }
            return this;
        }
        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWPRD_CompanyBasedPriceDetailById(this.id);
            var res = new ResultStatus { result = true };
            var validation = Validator();
            if (validation.result == false)
            {
                return validation;
            }
            if (data == null)
            {
                res = Insert();
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
        public ResultStatus Insert()
        {
            db = db ?? new WorkOfTimeDatabase();
            var dbresult = new ResultStatus { result = true};
            //Validasyonlarını yap

            //Bağlı Olduğu CompanyBasedPrice Varsa Al
            var companyId = this.companyId; 
            var productId = this.productId; 
            var categoryId = this.categoryId; 
            var conditionType = this.conditionType; 
            var sellingType = this.sellingType;

            var companyBasedPrice = new PRD_CompanyBasedPrice
            {
                companyId = companyId,
                productId = productId,
                categoryId = categoryId,
                conditionType = conditionType,
                sellingType = sellingType
            };
            var companyBasedPriceRecord = db.GetDBPRD_CompanyBasedPriceByAllAttributes(companyBasedPrice);
            if(companyBasedPriceRecord == null) //yeni bir tane oluştur
            {
                companyBasedPrice.createdby = this.createdby;
                companyBasedPrice.created = DateTime.Now;
                dbresult &= db.InsertPRD_CompanyBasedPrice(new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(companyBasedPrice),this.trans);
                if (!dbresult.result)
                {
                    return new ResultStatus
                    {
                        result = false,
                        message = "Kayıt Başarısız"
                    };
                }
            }

            var companyBasedPriceDetail = new PRD_CompanyBasedPriceDetail
            {
                companyBasedPriceId = companyBasedPriceRecord != null ? companyBasedPriceRecord.id : companyBasedPrice.id,
                minCondition = this.minCondition,
                type = this.type,
                discount = this.type,
                price = this.price,
                startDate = this.startDate,
                endDate = this.endDate,
                created = DateTime.Now,
                createdby = this.createdby
            };
            dbresult &= db.InsertPRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(companyBasedPriceDetail), this.trans);
            //dbresult &= db.InsertPRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(this), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Kayıt Başarısız"
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Kayıt Başarılı"
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            var companyBasedPriceDetail = db.GetPRD_CompanyBasedPriceDetailById(this.id);
            if(companyBasedPriceDetail != null)
            {
                dbresult &= db.UpdatePRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(companyBasedPriceDetail), false, this.trans);
            }
            else
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Kayıt Silinmiş"
                };
            }

            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Güncelleme Başarısız"
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Güncelleme Başarılı"
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeletePRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail { id = this.id }, trans);
            if (!dbresult.result)
            {
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Silme İşlemi Başarısız"
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Silme İşlemi Başarılı"
                };
            }
        }
        public ResultStatus ValidateCompanyBasedIsExistBefore()
        {
            var db = new WorkOfTimeDatabase();
            var result=db.GetVWPRD_CompanyBasedDetailIsExistBefore(this);
            if (result==null)
            {
                return new ResultStatus { result = false };
            }
            else
            {
                return new ResultStatus { result = true, objects = result };
            }
        }
    }
}
