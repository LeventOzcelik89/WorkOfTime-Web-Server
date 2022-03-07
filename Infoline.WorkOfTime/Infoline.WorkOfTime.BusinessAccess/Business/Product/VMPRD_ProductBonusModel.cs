using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPRD_ProductBonusModel : VWPRD_ProductBonus
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public List<Dictionary<string, string>> products { get; set; } = new List<Dictionary<string, string>>();
        public List<Dictionary<string, string>> companies { get; set; } = new List<Dictionary<string, string>>();
        public List<VWPRD_ProductBonusPrice> ProductItems { get; set; }
        public Guid? bonusProductId { get; set; }
        public VMPRD_ProductBonusModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var productBonus = db.GetVWPRD_ProductBonusById(this.id);
            if (productBonus != null)
            {
                this.ProductItems = db.GetVWPRD_ProductBonusPriceByProductBonusId(this.id).ToList();
                this.B_EntityDataCopyForMaterial(productBonus, true);
            }
            this.code = this.code ?? BusinessExtensions.B_GetIdCode();
            this.products = this.GetProductList();
            this.companies = this.GetCompanyList();
            return this;
        }

        private ResultStatus Validator()
        {
            var result = new ResultStatus { result = true };
            foreach (var item in this.ProductItems)
            {
                if (item.unitPrice == 0)
                {
                    result.message = "Ürün birim fiyatı 0 olamaz.";
                    result.result = false;
                    return result;
                }
                if (item.productId==null)
                {
                    result.message = "Ürün alanı boş geçilemez.";
                    result.result = false;
                    return result;
                }
            }
            if (this.ruleName == null)
            {
                result.message = "Kural adı boş geçilemez";
                result.result = false;
                return result;
            }
            if (this.query == null)
            {
                result.message = "Query alanı boş geçilemez";
                result.result = false;
                return result;
            }
            var existRuleName = db.GetVWPRD_ProductBonusByRuleName(this.ruleName);
            if (existRuleName != null && existRuleName.id != this.id)
            {
                result.message = existRuleName.ruleName + "kural adı sistemde mevcut.Başka bir kural adı ile işleme devam ediniz";
                result.result = false;
                return result;
            }
            return result;
        }

        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var productBonus = db.GetVWPRD_ProductBonusById(this.id);
            var rs = new ResultStatus { result = true };
            var validate = Validator();
            if (!validate.result)
            {
                return validate;
            }
            if (productBonus == null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                rs = Insert(trans);
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
            var result = new ResultStatus { result = true };
            result &= db.InsertPRD_ProductBonus(new PRD_ProductBonus().B_EntityDataCopyForMaterial(this));
            if (ProductItems.Count() > 0)
            {
                foreach (var item in ProductItems)
                {
                    item.created = this.created;
                    item.createdby = this.createdby;
                }
                result &= db.BulkInsertPRD_ProductBonusPrice(this.ProductItems.Select(a => new PRD_ProductBonusPrice().B_EntityDataCopyForMaterial(a)), trans);
            }
            return result;
        }
        private ResultStatus Update(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var productBonus = new PRD_ProductBonus().B_EntityDataCopyForMaterial(this, true);
            var result = db.UpdatePRD_ProductBonus(productBonus, false, transaction);
            if (productBonus != null)
            {
                var productBonusPrice = db.GetPRD_ProductBonusPriceByProductBonusId(productBonus.id);
                result &= db.BulkDeletePRD_ProductBonusPrice(productBonusPrice.ToList());
            }
            if (ProductItems.Count() > 0)
            {
                foreach (var item in ProductItems)
                {
                    item.created = this.created;
                    item.createdby = this.createdby;
                }
                result &= db.BulkInsertPRD_ProductBonusPrice(this.ProductItems.Select(a => new PRD_ProductBonusPrice().B_EntityDataCopyForMaterial(a)), trans);
            }
            if (result.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Prim Kuralı Güncelleme işlemi başarılı." };
            }
            else
            {
                Log.Error(result.message);
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = "Prim Kuralı Güncelleme işlemi başarısız." };
            }
        }
        public ResultStatus Delete(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var productBonus = db.GetPRD_ProductBonusById(this.id);

            if (productBonus == null)
            {
                return new ResultStatus { result = false, message = "Prim kuralı zaten silinmiş." };
            }
            var dbres = new ResultStatus { result = true };
            if (productBonus != null)
            {
                var productBonusPrice = db.GetPRD_ProductBonusPriceByProductBonusId(productBonus.id);
                dbres &= db.BulkDeletePRD_ProductBonusPrice(productBonusPrice.ToList());
            }
            dbres &= db.DeletePRD_ProductBonus(productBonus);
            if (dbres.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Prim kuralı silme işlemi başarılı oldu."
                };
            }
            else
            {
                Log.Error(dbres.message);
                if (trans == null) transaction.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Prim kuralı silme işlemi başarısız oldu."
                };
            }
        }
        public List<Dictionary<string, string>> GetProductList()
        {
            var db = new WorkOfTimeDatabase();
            var products = db.GetPRD_Product().Where(a => a.stockType == (int)EnumPRD_ProductStockType.SeriNoluTakip && a.type == (int)EnumPRD_ProductType.TicariMal).ToDictionary(a => a.id, b => b.name).ToArray();
            var product = new List<Dictionary<string, string>>();
            foreach (var item in products)
            {
                product.Add(new Dictionary<string, string> { { item.Key.ToString(), item.Value.ToString() } });
            }
            return product;
        }
        public List<Dictionary<string, string>> GetCompanyList()
        {
            var db = new WorkOfTimeDatabase();
            var companies = db.GetVWCMP_Company().Where(a => a.CMPTypes_Title != null && a.CMPTypes_Title.Contains("Bayi")).ToDictionary(a => a.id, b => b.name).ToArray();
            var company = new List<Dictionary<string, string>>();
            foreach (var item in companies)
            {
                company.Add(new Dictionary<string, string> { { item.Key.ToString(), item.Value.ToString() } });
            }
            return company;
        }
    }
    public class PRD_ProductClassModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }
    public class CMP_CompanyClassModel
    {
        public Guid id { get; set; }
        public string name { get; set; }
    }
}
