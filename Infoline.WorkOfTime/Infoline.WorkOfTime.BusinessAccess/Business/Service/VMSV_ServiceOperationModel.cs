using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSV_ServiceOperationModel : VWSV_ServiceOperation
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public  Guid? companyId { get; set; }
        public List<VWPRD_TransactionItem> wastageProducts { get; set; } = new List<VWPRD_TransactionItem>();
        public VWPRD_Transaction Transaction { get; set; }
        public List<VWPRD_ProductMateriel> TreeProduct { get; set; } = new List<VWPRD_ProductMateriel>();
        public short Type { get; set; }
        public Guid? storageId { get; set; }

        public VMSV_ServiceOperationModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSV_ServiceOperationById(this.id);
            if (serviceId!=null)
            {
                var findService = db.GetVWSV_ServiceById(serviceId.Value);
                if (findService!=null)
                {
                    var serviceModel = new VMSV_ServiceModel();
                    serviceModel.GetVWPRD_ProductMateriels(findService.productId.HasValue?findService.productId.Value:new Guid());
                    TreeProduct=serviceModel.GetProductMetarials;


                }
            }
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
            var data = db.GetVWSV_ServiceOperationById(this.id);
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
            return res;
        }
        private ResultStatus Insert()
        {
            db = db ?? new WorkOfTimeDatabase();
            var res = new ResultStatus { result = true };
       
            if (this.companyId.HasValue)
            {
                var getCompany = db.GetVWCMP_CompanyById(this.companyId.Value);
                if (getCompany!=null)
                {
                    description = $"Transfer Edilen Şube: {getCompany.fullName} </br>" + description;
                }
            } 
            var dbresult = db.InsertSV_ServiceOperation(this.B_ConvertType<SV_ServiceOperation>(), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Yeni Servis Operasyonu oluşturma işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Yeni Servis Operasyonu oluşturma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateSV_ServiceOperation(this.B_ConvertType<SV_ServiceOperation>(), false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Servis Operasyonu güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Servis Operasyonu güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeleteSV_ServiceOperation(this.id, trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Servis Operasyonu silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Servis Operasyonu silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus GetStockByProductAndStorageId(Guid[] productId,Guid storageId ) { 
            var result = new ResultStatus{ result=true };
            db = db ?? new WorkOfTimeDatabase();
            result.objects= db.GetVWPRD_StockSummaryByProductIdsAndStockId(productId,storageId);
            return result;
        }
        public ResultStatus QualiltyCheck(Guid serviceId, bool status,Guid userId) {
            var result = new ResultStatus { result = true };
            db = db ?? new WorkOfTimeDatabase();
            result &= new VMSV_ServiceModel { id = serviceId }.NextStage(userId);


            return result;
            
        
        
        }
    }
}
