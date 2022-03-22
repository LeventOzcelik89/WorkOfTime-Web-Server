using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPRD_ProductProgressPaymentImportModel : VWPRD_ProductProgressPaymentImport
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public string companyCode { get; set; }
        public string companyName { get; set; }
        public CMP_Types[] CMP_Types { get; set; }
        public VMPRD_ProductProgressPaymentImportModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var productImport = db.GetVWPRD_ProductProgressPaymentImportById(this.id);
            if (productImport != null)
            {
                this.B_EntityDataCopyForMaterial(productImport, true);
            }
            if (this.companyId != null)
            {
                var company = db.GetCMP_CompanyById(this.companyId.Value);
                this.companyCode = company.code;
                this.companyName = company.name;
            }
            this.CMP_Types = db.GetCMP_Types();
            return this;
        }

        private ResultStatus Validator()
        {
            var result = new ResultStatus { result = true };
            if (this.imei == null)
            {
                result.message = "Imei alanı boş geçilemez";
                result.result = false;
                return result;
            }
            if (this.companyCode == null)
            {
                result.message = "Bayi kodu boş geçilemez";
                result.result = false;
                return result;
            }
            if (this.companyName == null)
            {
                result.message = "Bayi adı boş geçilemez";
                result.result = false;
                return result;
            }
            if (this.date == null)
            {
                result.message = "Satış tarihi alanı boş geçilemez";
                result.result = false;
                return result;
            }
            var existImei = db.GetPRD_ProductProgressPaymentImportByImei(this.imei);
            if (existImei != null && existImei.id != this.id)
            {
                result.message = existImei.imei + " imei numaralı kayıt sistemde mevcut.Başka bir imei numarası ile işleme devam ediniz";
                result.result = false;
                return result;
            }
            return result;
        }

        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var productImport = db.GetVWPRD_ProductProgressPaymentImportById(this.id);
            var rs = new ResultStatus { result = true };
            var validate = Validator();
            if (!validate.result)
            {
                return validate;
            }
            if (productImport == null)
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
            var company = db.GetCMP_CompanyByCode(this.companyCode);
            if (company == null)
            {
                result.result = false;
                result.message = this.companyCode + " bayi koduna ait bayi sistemde bulunmamaktadır.";
                return result;
            }
            if (company.name != this.companyName)
            {
                result.result = false;
                result.message = "Girilen bayi adına ait bayi sistemde bulunmamaktadır.";
                return result;
            }
            var inventory = db.GetPRD_InventoryByImei(this.imei);
            if (inventory == null)
            {
                result.result = false;
                result.message = this.imei + "imei numarasına ait bir ürün bulunamadı.";
                return result;
            }
            var model = new PRD_ProductProgressPaymentImport();
            model.id = Guid.NewGuid();
            model.created = this.created;
            model.createdby = this.createdby;
            model.imei = this.imei;
            model.productId = inventory.productId;
            model.companyId = company.id;
            model.date = this.date;
            result &= db.InsertPRD_ProductProgressPaymentImport(model.B_EntityDataCopyForMaterial(this));
            if (result.result)
            {
                var checkIsActivated = db.GetPRD_TitanDeviceActivatedByImei(this.imei);
                var checkIsInventory = db.GetPRD_InventoryByImei(this.imei);
                var checkIsFTP = db.GetPRD_EntegrationActionByImei(this.imei,this.companyId);
                var progressPayment = new PRD_ProductProgressPayment();
                progressPayment.created = DateTime.Now;
                progressPayment.createdby = this.createdby;
                progressPayment.existFTP = checkIsFTP != null ? true : false;
                progressPayment.isActivated = checkIsActivated != null ? true : false;
                progressPayment.isInventory = checkIsInventory != null ? true : false;
                progressPayment.imei = this.imei;
                progressPayment.companyId = company.id;
                progressPayment.productId = inventory.productId;
                progressPayment.date = this.date;
                progressPayment.isProgressPayment = (int)EnumPRD_ProductProgressPaymentIsProgressPayment.approving;
                var dbResult = db.InsertPRD_ProductProgressPayment(progressPayment);
                if (!dbResult.result)
                {
                    Log.Error(dbResult.message);
                }
            }
            return result;
        }
        private ResultStatus Update(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var productImport = new PRD_ProductProgressPaymentImport().B_EntityDataCopyForMaterial(this, true);
            var result = db.UpdatePRD_ProductProgressPaymentImport(productImport, false, transaction);
            if (result.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Hakediş Güncelleme işlemi başarılı." };
            }
            else
            {
                Log.Error(result.message);
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = "Hakediş Güncelleme işlemi başarısız." };
            }
        }
        public ResultStatus Delete(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var dbres = new ResultStatus { result = true };
            var productImport = db.GetPRD_ProductBonusById(this.id);
            if (productImport == null)
            {
                return new ResultStatus { result = false, message = "Hakediş zaten silinmiş." };
            }
            dbres &= db.DeletePRD_ProductProgressPaymentImport(productImport.id);
            if (dbres.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Hakediş silme işlemi başarılı oldu."
                };
            }
            else
            {
                Log.Error(dbres.message);
                if (trans == null) transaction.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Hakediş silme işlemi başarısız oldu."
                };
            }
        }
        public static ResultStatus Import(string model, Guid createdBy)
        {

            var db = new WorkOfTimeDatabase();
            var culture = new System.Globalization.CultureInfo("tr-TR", false);
            var data = new List<PRD_ProductProgressPaymentImport>();
            var excelRows = Helper.Json.Deserialize<PRD_ProductProgressPaymentImportExcel[]>(model);
            var existError = new List<ExcelResultStatus>();
            var dbDataList = db.GetPRD_ProductProgressPaymentImport().ToList();
            var rowNumber = 0;
            foreach (var excelRow in excelRows)
            {
                rowNumber++;
                var res = new ResultStatus { result = true };
                var errors = new List<string>();
                var validate = excelRow.Validate();
                if (!validate.result)
                {
                    errors.Add(validate.message);
                }

                if (errors.Count() == 0)
                {
                    var SaveModel = new VMPRD_ProductProgressPaymentImportModel();
                    SaveModel.imei = excelRow.imei;
                    SaveModel.companyCode = excelRow.companyCode;
                    SaveModel.created = DateTime.Now;
                    SaveModel.createdby = createdBy;
                    SaveModel.companyName = excelRow.companyName;
                    SaveModel.date = excelRow.date;
                    res &= SaveModel.Save(createdBy);
                }
                if (!res.result)
                {
                    Log.Error(res.message);
                    errors.Add(res.message);
                }

                existError.Add(new ExcelResultStatus
                {
                    status = errors.Count() == 0,
                    rowNumber = rowNumber,
                    message = string.Join(",", errors)
                });

            }
            if (existError.Where(a => a.status == false).Count() == excelRows.Length)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Kaydetme işlemi başarısız",
                    objects = existError
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Kaydetme işlemi başarılı." + (existError.Where(a => a.status == false).Count() > 0 ? "Bazı kayıtlarda problem oluştu." : ""),
                    objects = existError.Where(x => x.status == false).ToArray()
                };
            }
        }
    }
}
