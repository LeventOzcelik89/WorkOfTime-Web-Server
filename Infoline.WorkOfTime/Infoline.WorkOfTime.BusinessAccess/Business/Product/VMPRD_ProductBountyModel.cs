using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPRD_ProductBountyModel : VWPRD_ProductBounty
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public Guid[] Companies { get; set; }
        public Guid[] Products { get; set; }
        public VWPRD_ProductBounty[] Bounty { get; set; }
        public bool fromCompanyBased { get; set; }
        public VMPRD_ProductBountyModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var productBounty = db.GetVWPRD_ProductBountyById(this.id);
            if (productBounty != null)
            {
                this.B_EntityDataCopyForMaterial(productBounty, true);
            }
            return this;
        }

        private ResultStatus Validator()
        {
            var errors = new List<string>();
            if (this.amount <= 0)
            {
                errors.Add("Prim Miktarı 0 veya küçük olamaz!");
            }

            if (errors.Count() > 0)
            {
                return new ResultStatus
                {
                    message = string.Join("\n", errors),
                    result = false
                };
            }
            return new ResultStatus { result = true };
        }

        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var productBounty = db.GetVWPRD_ProductBountyById(this.id);
            var rs = new ResultStatus { result = true };
            var validate = Validator();
            if (!validate.result)
            {
                return validate;
            }

            if (productBounty == null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                if (this.fromCompanyBased)
                {
                    rs = this.InsertMultiple(trans);
                }
                else
                {
                    rs = Insert(trans);
                }
            }
            else
            {
                this.changedby = userId;
                this.changed = DateTime.Now;
                rs = Update(trans);
            }
            return rs;
        }
        private ResultStatus Insert(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var rs = new ResultStatus { result = true };
            var productBounty = db.GetPRD_ProductBountiesByPeriodAndProductAndCompanyId(this.month.Value, this.year.Value, this.productId.Value,this.companyId.Value);
            if (productBounty.Count() > 0)
            {
                return new ResultStatus { result = false, message = "Aynı cariye ve ürüne, belirtilen dönemde prim tanımlaması yapılmıştır!" };
            }
            rs = db.InsertPRD_ProductBounty(this.B_ConvertType<PRD_ProductBounty>(), transaction);
            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Ürün Prim Tanımlama İşlemi Başarılı." };
            }
            else
            {
                Log.Error(rs.message);
                if (trans == null) transaction.Rollback();
                Log.Error(rs.message);
                return new ResultStatus { result = false, message = "Ürün Prim Tanımlama İşlemi başarısız." };
            }
        }
        private ResultStatus InsertMultiple(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var rs = new ResultStatus { result = true };
            if (this.Bounty.Length < 0)
            {
                return new ResultStatus { result = false, message = "Dönem boş olmaz" };
            }
            if (this.Products.Length < 0)
            {
                return new ResultStatus { result = false, message = "Ürünler boş olmaz" };
            }
            if (this.companyId == null)
            {
                return new ResultStatus { result = false, message = "Firma/Cari Boş Olamaz" };
            }
            foreach (var range in Bounty)
            {
                foreach (var product in Products)
                {
                    if (Companies != null)
                    {
                        foreach (var company in Companies)
                        {
                            var isExist = db.GetPRD_ProductBountiesByPeriodAndProductAndCompanyId(range.month.Value, range.year.Value, product, company);
                            if (isExist.Count() > 0)
                            {
                                if (trans == null) transaction.Rollback();
                                return new ResultStatus { result = false, message = "Belirtilmiş olan dönem içerisin de prim tanımlaması yapılmıştır" };
                            }
                            rs &= db.InsertPRD_ProductBounty(new PRD_ProductBounty
                            {
                                amount = range.amount,
                                month = range.month,
                                year = range.year,
                                companyId = company,
                                productId = product,
                                createdby = createdby,
                                created = created
                            }, transaction);
                        }
                    }
                    else
                    {
                        var isExist = db.GetPRD_ProductBountiesByPeriodAndProductAndCompanyId(range.month.Value, range.year.Value, product, companyId.Value);
                        if (isExist.Count() > 0)
                        {
                            if (trans == null) transaction.Rollback();
                            return new ResultStatus { result = false, message = "Aynı cariye ve ürüne, belirtilen dönemde prim tanımlaması yapılmıştır!" };
                        }
                        rs &= db.InsertPRD_ProductBounty(new PRD_ProductBounty
                        {
                            amount = range.amount,
                            month = range.month,
                            year = range.year,
                            companyId = this.companyId,
                            productId = product,
                            createdby = createdby,
                            created = created
                        }, transaction);
                    }
                }
            }
            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Ürün Prim Tanımlama İşlemi Başarılı." };
            }
            else
            {
                Log.Error(rs.message);
                if (trans == null) transaction.Rollback();
                Log.Error(rs.message);
                return new ResultStatus { result = false, message = "Ürün Prim Tanımlama İşlemi başarısız." };
            }
        }
        private ResultStatus Update(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var isExist = db.GetPRD_ProductBountyByPeriodAndProductAndCompanyId(this.month.Value, this.year.Value, this.productId.Value, this.companyId.Value);
            if (isExist != null && isExist.id != id)
            {
                return new ResultStatus { result = false, message = "Aynı cariye ve ürüne, belirtilen dönemde prim tanımlaması yapılmıştır!" };
            }
            var productBounty = new PRD_ProductBounty().B_EntityDataCopyForMaterial(this, true);
            var rs = db.UpdatePRD_ProductBounty(productBounty, false, transaction);
            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Ürün Prim Güncelleme işlemi başarılı." };
            }
            else
            {
                Log.Error(rs.message);
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = "Ürün Prim Güncelleme işlemi başarısız." };
            }
        }
        public ResultStatus Delete(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var productBounty = db.GetPRD_ProductBountyById(this.id);
            if (productBounty == null)
            {
                return new ResultStatus { result = false, message = "Ürün prim tanımı silinmiş." };
            }
            var dbres = new ResultStatus { result = true };
            dbres &= db.DeletePRD_ProductBounty(productBounty);
            if (dbres.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Ürün Prim silme işlemi başarılı oldu."
                };
            }
            else
            {
                Log.Error(dbres.message);
                if (trans == null) transaction.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Ürün Prim silme işlemi başarısız oldu."
                };
            }
        }
    }
}
