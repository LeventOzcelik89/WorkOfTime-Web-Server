using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSV_ChangedDeviceModel : VWSV_ChangedDevice
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VMSV_ChangedDeviceModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var data = db.GetVWSV_ChangedDeviceById(this.id);
            if (data!=null)
            {
                this.B_EntityDataCopyForMaterial(data, true);
            }
            return this;
        }
        public ResultStatus Save(Guid? userId = null, HttpRequestBase request = null, DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWSV_ChangedDeviceById(this.id);
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
            var dbresult = db.InsertSV_ChangedDevice(this.B_ConvertType<SV_ChangedDevice>(), this.trans);
            var inventories = db.GetVWPRD_InventoryByIds(new[] { oldInventoryId.Value,newInventoryId.Value});
            var listOfTransItems = new List<VMPRD_TransactionItems>();
            foreach (var item in inventories)
            {
                
              
                    var findProduct = db.GetVWPRD_ProductById(item.productId.Value);
                    if (findProduct != null)
                    {
                        listOfTransItems.Add(new VMPRD_TransactionItems
                        {
                            productId = item.productId,
                            quantity = 1,
                            serialCodes = item.serialcode ?? "",
                            unitPrice = findProduct.currentSellingPrice??0,
                        });
                    }
               
               
            }
            var transModelForExpens = new VMPRD_TransactionModel
            {
                inputId = null,
                inputTable = null,
                outputId = this.id,
                outputTable = "SV_ChangedDevice",
                created = this.created,
                createdby = this.createdby,
                status = (int)EnumPRD_TransactionStatus.islendi,
                items = listOfTransItems,
                date = DateTime.Now,
                code = BusinessExtensions.B_GetIdCode(),
                type = (short)EnumPRD_TransactionType.CihazDegisimi,
                id = Guid.NewGuid()
            };


            dbresult &= transModelForExpens.Save(this.createdby, trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Yeni Cihaz Değiştirme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Yeni Cihaz Değiştirme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update()
        {
            var dbresult = new ResultStatus { result = true };
            dbresult &= db.UpdateSV_ChangedDevice(this.B_ConvertType<SV_ChangedDevice>(), false, this.trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Cihaz Değiştirme güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Cihaz Değiştirme güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
        public ResultStatus Delete(DbTransaction transaction = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            //İlişkili kayıtlar kontol edilerek dilme işlemine müsade edilecek;
            var dbresult = db.DeleteSV_ChangedDevice(this.id, trans);
            if (!dbresult.result)
            {
                Log.Error(dbresult.message);
                if (transaction == null) trans.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Cihaz Değiştirme silme işlemi başarısız oldu."
                };
            }
            else
            {
                if (transaction == null) trans.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Cihaz Değiştirme silme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }
       
    }

}
