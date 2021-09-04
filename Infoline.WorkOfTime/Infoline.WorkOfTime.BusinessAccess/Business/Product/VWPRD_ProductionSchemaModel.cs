using Infoline.WorkOfTime.BusinessData;
using System;
using Infoline.Framework.Database;
using System.Data.Common;
using System.Web;



namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VWPRD_ProductionSchemaModel : VWPRD_ProductionSchema
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPRD_Product product { get; set; }

        public VWPRD_ProductionSchemaModel Load()
        {

            db = db ?? new WorkOfTimeDatabase();
            var productionSchema = db.GetVWPRD_ProductionSchemaById(this.id);

            if (productionSchema != null)
            {
                this.B_EntityDataCopyForMaterial(productionSchema, true);
                if (this.productId.HasValue)
                {
                    this.product = db.GetVWPRD_ProductById(this.productId.Value);
                }
                else
                {
                    this.product = null;
                }
            }
            else
            {
                this.code = BusinessExtensions.B_GetIdCode();
            }

            return this;
        }
        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction trans = null)
        {

            db = db ?? new WorkOfTimeDatabase();
            var productionSchema = db.GetVWPRD_ProductionSchemaById(this.id);
            var rs = new ResultStatus { result = true };       
            var code = db.GetPRD_ProductionSchemaByCode(this.code);

            if (productionSchema == null)
            {
                if (code != null) return new ResultStatus { result = false, message = "Aynı şema kodu ile kayıt eklenemez." };

                this.createdby = userId;
                this.created = DateTime.Now;
                rs = Insert(trans);
            }
            else
            {
                if (code != null && (productionSchema.id != code.id && code.code == this.code)) return new ResultStatus { result = false, message = "Aynı şema koduna sahip başka kayıt var." };

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
            var productionSchema = new PRD_ProductionSchema().B_EntityDataCopyForMaterial(this, true);

            var rs = db.InsertPRD_ProductionSchema(productionSchema, transaction);

            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Üretim Şeması Ekleme İşlemi Başarılı." };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = "Üretim Şeması Ekleme İşlemi Başarısız." };
            }
        }
        private ResultStatus Update(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var productionSchema = new PRD_ProductionSchema().B_EntityDataCopyForMaterial(this, true);

            var rs = db.UpdatePRD_ProductionSchema(productionSchema, true, transaction);

            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Üretim Şeması Güncelleme İşlemi Başarılı." };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = "Üretim Şeması Güncelleme İşlemi Başarısız." };
            }
        }
        public ResultStatus Delete(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var _productionSchema = db.GetPRD_ProductionSchemaById(this.id);
            if (_productionSchema == null)
            {
                return new ResultStatus { result = false, message = "Ürün zaten silinmiş." };
            }

            var dbres = new ResultStatus { result = true };
            var transaction = trans ?? db.BeginTransaction();

            var productionSchema = new PRD_ProductionSchema().B_EntityDataCopyForMaterial(_productionSchema, true);

            //şemaya ait aşamaları silme
            var stages = db.GetPRD_ProductionSchemaStageByProductionSchemaId(this.id);
            if (stages != null || stages.Length != 0)
            {
                dbres &= db.BulkDeletePRD_ProductionSchemaStage(stages);
            }
            //şemayı silme
            dbres &= db.DeletePRD_ProductionSchema(productionSchema, trans);

            if (dbres.result == true)
            {
               
                if (trans == null) transaction.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Üretim Şeması Silme İşlemi Başarılı Oldu."
                };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Üretim Şeması Silme İşlemi Başarısız Oldu."
                };
            }
        }

        //Kaldırılacak
        public static SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus)
        {
            var db = new WorkOfTimeDatabase();
            BEXP filter = null;

            filter = new BEXP
            {
                Operand1 = new BEXP
                {
                    Operand1 = (COL)"status",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)1
                },

                Operator = BinaryOperator.Or,
                Operand2 = new BEXP
                {
                    Operand1 = (COL)"status",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)3
                }
            };

            query.Filter &= filter;

            return query;

        }


    }
}
