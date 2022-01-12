using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSV_DeviceProblemModel : VWSV_DeviceProblem
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public List<VMSV_DeviceProblemModel> Problems { get; set; }
        public Guid? inventoryId { get; set; }
        public List<VWPRD_Product> ServiceNotifications { get; set; }
        public VMSV_DeviceProblemModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSV_DeviceProblemById(this.id);
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
            var data = db.GetVWSV_DeviceProblemById(this.id);
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
            //Validasyonlarını yap

            var dbresult = db.InsertSV_DeviceProblem(this.B_ConvertType<SV_DeviceProblem>(), this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Yeni Cihaz Problemi oluşturma işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Yeni Cihaz Problemi oluşturma işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateSV_DeviceProblem(this.B_ConvertType<SV_DeviceProblem>(), false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Cihaz Problemi güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Cihaz Problemi güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(Guid userId, DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var findProblem = db.GetVWSV_DeviceProblemById(this.id);
            var dbresult = db.DeleteSV_DeviceProblem(this.id, trans);
            dbresult = new VMSV_ServiceOperationModel
            {
                description = $"{findProblem.productId_Title} ürününde olan  {findProblem.problemTypeId_Title} adlı sorun silinmiştir.",  
                serviceId = findProblem.serviceId,
                dataId = findProblem.id,
                dataTable = "SV_DeviceProblem",
                status =(short)EnumSV_ServiceOperation.PartDeleted}.Save(userId, null, trans);


            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Cihaz Problemi silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Cihaz Problemi silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }

    }
}
