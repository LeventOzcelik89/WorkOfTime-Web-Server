using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPRD_EntegrationImport : VWPRD_EntegrationImport
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VMPRD_EntegrationImport Load()
        {
            this.db = db ?? new WorkOfTimeDatabase();
            var data = db.GetVWPRD_EntegrationImportById(this.id);
            if (data != null)
            {
                return this.B_EntityDataCopyForMaterial(data, true);
            }
            return this;
        }
        public ResultStatus Save(Guid? userId = null, DbTransaction transaction = null)
        {
            var result = new ResultStatus();
            db = db ?? new WorkOfTimeDatabase();
            trans = transaction ?? db.BeginTransaction();
            var data = db.GetVWPRD_EntegrationImportById(this.id);
            if (distributor_id.HasValue)
            {
                var findDist = db.GetCMP_CompanyById(distributor_id.Value);
                distributorCode = findDist.code;
            }
            
            var findCompany = db.GetCMP_CompanyById(company_Id.Value);
            var findProduct = db.GetPRD_ProductById(product_Id.Value);
        
            customerCode = findCompany.code;
            productModel = findProduct.name;
            if (data == null)
            {
                this.created = DateTime.Now;
                this.createdby = userId;
                result = Insert();
            }
            else
            {
                this.changed = DateTime.Now;
                this.changedby = userId;
                result = Update();
            }
            if (result.result)
            {
                if (transaction == null)
                    this.trans.Commit();
                return result;
            }
            else
            {
                if (transaction == null)
                    this.trans.Rollback();
            }
            return result;
        }
        private ResultStatus Update()
        {
            var result = new ResultStatus { result = true };
            db = db ?? new WorkOfTimeDatabase();
            var isExist = db.GetPRD_EntegrationImportByImei(this.imei);
            if (isExist != null && isExist.id != id)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Aynı imei ile daha önce hakediş bildirilmiştir!"
                };
            }
            result &= db.UpdatePRD_EntegrationImport(new PRD_EntegrationImport().B_EntityDataCopyForMaterial(this));
            if (!result.result)
            {
                Log.Error(result.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Dosya güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Dosyadan güncelleme işlemi başarılı oldu."
                };
            }
        }
        private ResultStatus Insert()
        {
            var result = new ResultStatus { result = true };
            db = db ?? new WorkOfTimeDatabase();
            var isExist = db.GetPRD_EntegrationImportByImei(this.imei);
            if (isExist != null)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Aynı imei ile daha önce hakediş bildirilmiştir!"
                };
            }

            result &= db.InsertPRD_EntegrationImport(new PRD_EntegrationImport().B_EntityDataCopyForMaterial(this));
            if (!result.result)
            {
                Log.Error(result.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Hakediş Bildirimi Ekleme İşlemi Başarısız Oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Hakediş Bildirimi Ekleme İşlemi Başarıyla Tamamlandı."
                };
            }
        }
        public ResultStatus Delete(Guid id, DbTransaction transaction = null)
        {

            var db = new WorkOfTimeDatabase();
            var trans = transaction ?? db.BeginTransaction();
            var result = new ResultStatus { result = true };
            var isExist = db.GetPRD_EntegrationImportById(id);
            if (isExist == null)
            {
                return new ResultStatus { result = false, message = "Böyle bir kayıt yoktur" };
            }
            result &= db.DeletePRD_EntegrationImport(isExist, trans);
            if (result.result)
            {
                if (transaction == null)
                {
                    trans.Commit();
                }
                return new ResultStatus
                {
                    result = true,
                    message = "Hakediş bildirim(ler)i başarıyla silindi."
                };
            }
            else
            {
                trans.Rollback();
                Log.Error(result.message);
                return new ResultStatus
                {
                    result = false,
                    message = "Hakediş bildirim(ler)i silme işlemi başarısız oldu."
                };
            }
        }
        public SimpleQuery UpdateDataSource(SimpleQuery query, PageSecurity userStatus)
        {
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.HakEdisBayiPersoneli)))
            {
                query.Filter &= new BEXP
                {
                    Operand1 = (COL)"company_Id",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)userStatus.user.CompanyId ?? new Guid()
                };
            }
            else
            {
                return query;
            }
            return query;
        }
    }
}
