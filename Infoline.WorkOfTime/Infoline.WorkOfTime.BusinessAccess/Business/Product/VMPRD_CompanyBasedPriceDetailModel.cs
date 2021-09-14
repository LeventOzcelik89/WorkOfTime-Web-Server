using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessData.Specific;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess.Business.Product
{
    public class VMPRD_CompanyBasedPriceDetailModel : VWPRD_CompanyBasedPriceDetail
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public short companyType { get; set; }
        public short productType { get; set; }
        public VMPRD_CompanyBasedPriceDetailModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetPRD_CompanyBasedPriceDetailById(this.id);
            
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
                this.createdby = userId;
                this.created = DateTime.Now;
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
            if (isStartDateBiggerThanEndDate())
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Başlangıç tarihi, bitiş tarihinden büyük olamaz!"
                };
            }
            if (!CheckDates())
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Aynı tarih aralığında kayıt vardır. \n Başlangıç Tarihi: " + this.startDate + "\n" + " Bitiş Tarihi: " + this.endDate
                };

            }



            return res;
        }
        public ResultStatus Insert(DbTransaction trans=null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var dbresult = new ResultStatus { result = true };
            if (!Validator().result)
            {
                return Validator();
            }
            dbresult &= db.InsertPRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(this),trans);
            return dbresult;
        }
        public ResultStatus Update()
        {
            db = db ?? new WorkOfTimeDatabase();
            var dbresult = new ResultStatus { result = true };
            if (!Validator().result)
            {
                return Validator();
            }
            dbresult &= db.UpdatePRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(this));
            return dbresult;

        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var dbresult = new ResultStatus { result = true };
            var find = db.GetPRD_CompanyBasedPriceDetailById(this.id);
            if (find != null)
            {
                dbresult &= db.DeletePRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail { id = this.id }, trans);
            }
            else
            {
                dbresult.result = false;
                dbresult.message = "Böyle bir kayıt yoktur!";
            }
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
        private bool CheckDates()
        {
            var item = new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(this);
            var sameRecordList = db.GetPRD_CompanyBasedPriceDetailWithSameData(item);
            if (sameRecordList.Length == 0)
            {
                return true;
            }
            else
            {
                //aynı tarihlere denk gelmiyorsa izin ver (true döndür)
                foreach (var record in sameRecordList)
                {
                    if (item.endDate >= record.startDate && item.startDate <= record.endDate)
                    {
                        return false;
                    }
                    if (item.startDate <= record.endDate && item.endDate >= record.startDate)
                    {
                        return false;
                    }
                    if (item.startDate <= record.startDate && item.endDate >= record.endDate)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        private bool isStartDateBiggerThanEndDate()
        {
            if (startDate != null && endDate != null)
            {
                if (this.startDate > this.endDate)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
