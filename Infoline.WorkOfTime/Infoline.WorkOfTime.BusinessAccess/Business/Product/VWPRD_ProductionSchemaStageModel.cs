using Infoline.WorkOfTime.BusinessData;
using System;
using Infoline.Framework.Database;
using System.Data.Common;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VWPRD_ProductionSchemaStageModel : VWPRD_ProductionSchemaStage 
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }

        public VWPRD_ProductionSchemaStageModel Load()
        {

            db = db ?? new WorkOfTimeDatabase();
            var productionSchemaStage = db.GetVWPRD_ProductionSchemaStageById(this.id);

            if (productionSchemaStage != null)
            {
                this.B_EntityDataCopyForMaterial(productionSchemaStage, true);
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
            var productionSchemaStage = db.GetVWPRD_ProductionSchemaStageById(this.id);
            var rs = new ResultStatus { result = true };
            var code = db.GetPRD_ProductionSchemaStageByCode(this.code);

            //en son sırayı almak için
            if (!this.productionSchemaId.HasValue)
            {
                return new ResultStatus
                {
                    message = "Bağlı olan bir üretim şeması bulunamadı",
                    result = false
                };
            }

            if (productionSchemaStage == null)
            {
                if (code != null) return new ResultStatus { result = false, message = "Aynı aşama kodu ile aşama eklenemez." };

                this.createdby = userId;
                this.created = DateTime.Now;
                rs = Insert(trans);
            }
            else
            {
                if (code != null && (productionSchemaStage.id != code.id && code.code == this.code))
                {
                    return new ResultStatus { result = false, message = "Aynı aşama kodu ile aşama eklenemez." };
                }

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

            this.stageNum = 1;
            var lastProductionSchemaStageRecord = db.GetPRD_ProductionSchemaStageByProductionSchemaId(this.productionSchemaId.Value).OrderByDescending(a => a.stageNum).FirstOrDefault();
            if (lastProductionSchemaStageRecord != null && lastProductionSchemaStageRecord.stageNum.HasValue)
            {
                this.stageNum = lastProductionSchemaStageRecord.stageNum + 1;
            }

            var productionSchemaStage = new PRD_ProductionSchemaStage().B_EntityDataCopyForMaterial(this, true);
            var rs = db.InsertPRD_ProductionSchemaStage(productionSchemaStage, transaction);

            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Üretim Şemasına Aşama Ekleme İşlemi Başarılı." };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = rs.message };
            }
        }
        private ResultStatus Update(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var productionSchemaStage = new PRD_ProductionSchemaStage().B_EntityDataCopyForMaterial(this, true);

            var rs = db.UpdatePRD_ProductionSchemaStage(productionSchemaStage, true, transaction);

            if (rs.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus { result = true, message = "Aşama Güncelleme İşlemi Başarılı." };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus { result = false, message = "Aşama Güncelleme İşlemi Başarısız." };
            }
        }
        public ResultStatus Delete(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var _productionSchemaStage = db.GetPRD_ProductionSchemaStageById(this.id);
            if (_productionSchemaStage == null)
            {
                return new ResultStatus { result = false, message = "Aşama zaten silinmiş." };
            }

            var dbres = new ResultStatus { result = true };
            var transaction = trans ?? db.BeginTransaction();

            //bundan sonraki aşamaların sırasını bir küçültme
            var stageListAfterThis = db.GetPRD_ProductionSchemaStageByProductionSchemaId(_productionSchemaStage.productionSchemaId.Value);
            var changedStages = new List<PRD_ProductionSchemaStage>();
            foreach (var s in stageListAfterThis)
            {
                if (s.stageNum > _productionSchemaStage.stageNum)
                {
                    s.stageNum -= 1;
                    changedStages.Add(s);
                }
            }
            if (changedStages.Count > 0)
            {
                dbres &= db.BulkUpdatePRD_ProductionSchemaStage(changedStages);
            }
            var productionSchemaStage = new PRD_ProductionSchemaStage().B_EntityDataCopyForMaterial(_productionSchemaStage, true);
            dbres &= db.DeletePRD_ProductionSchemaStage(productionSchemaStage, trans);

            if (dbres.result == true)
            {
                if (trans == null) transaction.Commit();
                return new ResultStatus
                {
                    result = true,
                    message = "Üretim Şemasına Ait Aşama Silme İşlemi Başarılı Oldu."
                };
            }
            else
            {
                if (trans == null) transaction.Rollback();
                return new ResultStatus
                {
                    result = false,
                    message = "Üretim Şemasına Ait Aşama Silme İşlemi Başarısız Oldu."
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
