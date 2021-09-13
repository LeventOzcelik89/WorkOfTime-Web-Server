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
        public bool isPrice { get; set; }
        public string _CompanyBasedPriceDetailModels { get; set; }
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

        public ResultStatus InsertInline()
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = db.BeginTransaction();
            if (CheckDates()!=true)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Aynı tarhiler arasında kayıt vardır!"
                };
            }
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.InsertPRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(this), this.trans);
            if (dbresult.result==true)
            {
                trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Kayıt Başarılı"
                };
            }
            else
            {
                trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Kayıt Başarısız"
                };
            }
        }
        public ResultStatus UpdateInline()
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = db.BeginTransaction();
            var dbresult = new ResultStatus { result = true };
            if (CheckDates() != true)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Aynı tarhiler arasında kayıt vardır!"
                };
            }
            dbresult &= db.UpdatePRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(this),true, this.trans);

            if (dbresult.result == true)
            {
                trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Kayıt Başarılı"
                };
            }
            else
            {
                trans.Rollback();
                return new ResultStatus
                {
                    result = true,
                    message = "Kayıt Başarısız"
                };
            }
        }

        public ResultStatus Insert()
        {
            db = db ?? new WorkOfTimeDatabase();
            var dbresult = new ResultStatus { result = true };
            var companyBasedPriceRecord = db.GetDBPRD_CompanyBasedPriceByAllAttributes(new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(this));
            if (companyBasedPriceRecord == null)
            {
                dbresult &= db.InsertPRD_CompanyBasedPrice(new PRD_CompanyBasedPrice().B_EntityDataCopyForMaterial(this), this.trans);
                if (dbresult.result)
                {
                    if (_CompanyBasedPriceDetailModels != null)
                    {
                        var companyBaseList = Infoline.Helper.Json.Deserialize<List<VWPRD_CompanyBasedPriceDetailDto>>(this._CompanyBasedPriceDetailModels);
                        if (companyBaseList.Count > 0)
                        {
                            foreach (var item in companyBaseList)
                            {
                                if (this.CheckDates(item))
                                {
                                    item.companyBasedPriceId = this.id;
                                    dbresult &= db.InsertPRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(item), this.trans);
                                }
                                else
                                {
                                    return new ResultStatus
                                    {
                                        result = false,
                                        message = "Daha önceden aynı tarihler aralıkları çakışıyor \n Başlangıç Tarihi: " + item.startDate + "\n" + " Bitiş Tarihi: " + item.endDate
                                    };
                                }
                            }
                        }
                    }
                }
                if (dbresult.result)
                {
                    return new ResultStatus
                    {
                        result = true,
                        message = "Kayıt Başarılı"
                    };
                }
            }
            else
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Daha önceden böyle bir kayıt girilmiştir"
                };
            }
            return new ResultStatus
            {
                result = false,
                message = "Daha önceden böyle bir kayıt girilmiştir"
            };
        }
        public ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            if (_CompanyBasedPriceDetailModels != null)
            {
                var companyBaseList = Infoline.Helper.Json.Deserialize<List<VWPRD_CompanyBasedPriceDetailDto>>(this._CompanyBasedPriceDetailModels);
                if (companyBaseList.Count > 0)
                {
                    foreach (var item in companyBaseList)
                    {
                        if (item.id.HasValue)
                        {
                            var getCompanyBasedPriceDetail = db.GetPRD_CompanyBasedPriceDetailById(item.id.Value);
                            if (getCompanyBasedPriceDetail != null)
                            {
                                dbresult &= db.DeletePRD_CompanyBasedPriceDetail(new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(item), this.trans);
                            }
                        }
                    }
                }
                if (dbresult.result)
                {
                    dbresult &= Insert();
                    if (dbresult.result)
                    {
                        return new ResultStatus
                        {
                            result = true,
                            message = "Güncelleme  Başarılı"
                        };
                    }
                }
              
            }
            return new ResultStatus
            {
                result = true,
                message = "Güncelleme  Başarısız"
            };
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
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
            var values = db.GetVWPRD_CompanyBasedPriceDetailsByCompanyBasedId(id);
            List<VWPRD_CompanyBasedPriceDetailDto> list = new List<VWPRD_CompanyBasedPriceDetailDto>();
            if (values != null)
            {
                foreach (var item in values)
                {
                    list.Add(new VWPRD_CompanyBasedPriceDetailDto().B_EntityDataCopyForMaterial(item));
                }
            }
            return list.ToArray();
        }
        public bool CheckDates(VWPRD_CompanyBasedPriceDetailDto obje)
        {
            var item = new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(obje);
            var sameRecordList = db.GetPRD_CompanyBasedPriceDetailWithSameData(item);
            if (sameRecordList.Length==0)
            {
                return true;
            }
            else
            {
                //aynı tarihlere denk gelmiyorsa izin ver (true döndür)
                foreach (var record in sameRecordList)
                {
                    if (this.endDate >= record.startDate && this.startDate <= record.endDate)
                    {
                        return false;
                    }
                    if (this.startDate <= record.endDate && this.endDate >= record.startDate)
                    {
                        return false;
                    }
                    if (this.startDate <= record.startDate && this.endDate >= record.endDate)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public bool CheckDates()
        {
            var item = new PRD_CompanyBasedPriceDetail().B_EntityDataCopyForMaterial(this);
            var sameRecordList = db.GetPRD_CompanyBasedPriceDetailWithSameData(item);
            if (sameRecordList.Length==0)
            {
                return true;
            }
            else
            {
                //aynı tarihlere denk gelmiyorsa izin ver (true döndür)
                foreach (var record in sameRecordList)
                {
                    if (this.endDate >= record.startDate && this.startDate <= record.endDate)
                    {
                        return false;
                    }
                    if (this.startDate <= record.endDate && this.endDate >= record.startDate)
                    {
                        return false;
                    }
                    if (this.startDate <= record.startDate && this.endDate >= record.endDate)
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
