using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessData.Specific;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess.Business.Product
{
    public class VMPRD_CompanyBasedPriceDetailModel : VWPRD_CompanyBasedPriceDetail
    {

        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        [DataType(DataType.Date)]
        public new DateTime startDate { get; set; }
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
            var dbresult = new ResultStatus { result = true };
            //Validasyonlarını yap

            //Bağlı Olduğu CompanyBasedPrice Varsa Al
            var companyBasedPriceDto = new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(this);
            var companyBasedPriceRecord = db.GetDBPRD_CompanyBasedPriceByAllAttributes(companyBasedPriceDto);
            if (companyBasedPriceRecord == null) //yeni bir tane oluştur
            {
                companyBasedPriceDto.createdby = this.createdby;
                companyBasedPriceDto.created = DateTime.Now;
                dbresult &= db.InsertPRD_CompanyBasedPrice(new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(companyBasedPriceDto), this.trans);
                if (!dbresult.result)
                {
                    return new ResultStatus
                    {
                        result = false,
                        message = "Kayıt Başarısız",
                        objects = null
                    };
                }
            }

            var companyBasedPriceDetail = new PRD_CompanyBasedPriceDetail
            {
                companyBasedPriceId = companyBasedPriceRecord != null ? companyBasedPriceRecord.id : companyBasedPriceDto.id,
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
                    message = "Kayıt Başarısız",
                    objects = null
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Kayıt Başarılı",
                    objects = db.GetVWPRD_CompanyBasedPriceDetailById(companyBasedPriceDetail.id)
                };
            }
        }
        public ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            var companyBasedPriceDetail = db.GetPRD_CompanyBasedPriceDetailById(this.id);
            if (companyBasedPriceDetail != null)
            {
                dbresult &= db.UpdatePRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(this), false, this.trans);
            }
            else
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Kayıt Silinmiş",
                    objects = null
                };
            }

            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Güncelleme Başarısız",
                    objects = null
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Güncelleme Başarılı",
                    objects = this
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
       
        public VWPRD_CompanyBasedPriceDetailDto[] GetVWCompanyBasedPriceDetailByCompanyBasedPriceId(Guid id)
        {
            var db = new WorkOfTimeDatabase();
            var values= db.GetVWPRD_CompanyBasedPriceDetailsByCompanyBasedId(id);
            List<VWPRD_CompanyBasedPriceDetailDto> list = new List<VWPRD_CompanyBasedPriceDetailDto>();
            if (values!=null)
            {
                foreach (var item in values)
                {
                    list.Add(new VWPRD_CompanyBasedPriceDetailDto().B_EntityDataCopyForMaterial(item));
                }
            }

            return list.ToArray();
        }
    }
}

