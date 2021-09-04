using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPDS_EvaluateFormModel : VWPDS_EvaluateForm
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPDS_QuestionForm[] QuestionForms { get; set; }
        public VWPDS_Question[] Questions { get; set; }

        public VMPDS_EvaluateFormModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var form = db.GetVWPDS_EvaluateFormById(this.id);

            if (form != null)
            {
                this.B_EntityDataCopyForMaterial(form, true);
                this.QuestionForms = db.GetVWPDS_QuestionFormByFormId(this.id).OrderBy(a => a.groupOrder).ThenBy(a => a.questionOrder).ToArray();
            }
            else
            {
                this.formCode = BusinessExtensions.B_GetIdCode();
                this.status = true;
            }

            this.Questions = db.GetVWPDS_Question();
            return this;
        }

        public ResultStatus Save(Guid userId, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var form = db.GetVWPDS_EvaluateFormById(this.id);
            var rs = new ResultStatus { result = true };

            var nameControl = db.GetVWPDS_EvaluateFormByFormName(this.formName);

            if (this.QuestionForms.Count() == 0)
            {
                return new ResultStatus { result = false, message = "Herhangi bir soru eklemeden form kaydedilemez." };
            }

            if (nameControl != null && nameControl.id != this.id)
            {
                return new ResultStatus { result = false, message = "Bu isimde kayıtlı bir form bulunmaktadır. Lütfen form ismini değiştirin." };
            }

            foreach (var questionForm in this.QuestionForms)
            {
                questionForm.created = DateTime.Now;
                questionForm.createdby = this.createdby;
                questionForm.evaluateFormId = this.id;
            }

            if (form == null)
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
            var form = new PDS_EvaluateForm().B_EntityDataCopyForMaterial(this, true);

            var rs = db.InsertPDS_EvaluateForm(form, transaction);

            rs &= db.BulkInsertPDS_QuestionForm(this.QuestionForms.Select(a => new PDS_QuestionForm().B_EntityDataCopyForMaterial(a)), transaction);

            if (rs.result == true)
            {
                if (trans == null)
                    transaction.Commit();
                return new ResultStatus { result = true, message = "Form Ekleme işlemi başarılı." };
            }
            else
            {
                if (trans == null)
                    transaction.Rollback();
                return new ResultStatus { result = false, message = "Form Ekleme işlemi başarısız." };
            }
        }

        private ResultStatus Update(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var form = new PDS_EvaluateForm().B_EntityDataCopyForMaterial(this, true);
            var rs = db.UpdatePDS_EvaluateForm(form, false, transaction);

            var questionForms = db.GetPDS_QuestionFormByFormId(this.id);

            rs &= db.BulkDeletePDS_QuestionForm(questionForms, transaction);
            rs &= db.BulkInsertPDS_QuestionForm(this.QuestionForms.Select(a => new PDS_QuestionForm().B_EntityDataCopyForMaterial(a)).ToArray(), transaction);

            if (rs.result == true)
            {
                if (trans == null)
                    transaction.Commit();
                return new ResultStatus { result = true, message = "Form Güncelleme işlemi başarılı." };
            }
            else
            {
                if (trans == null)
                    transaction.Rollback();
                return new ResultStatus { result = false, message = "Form Güncelleme işlemi başarısız." };
            }
        }

        public ResultStatus Delete(DbTransaction trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = trans ?? db.BeginTransaction();

            var formResults = this.db.GetVWPDS_FormResultByFormId(this.id);

            if (formResults.Count() > 0)
            {
                return new ResultStatus { result = false, message = "Bu form ile değerlendirme yapıldığı için silme işlemi gerçekleştirilemez." };
            }

            var rs = new ResultStatus { result = true };

            rs &= db.DeletePDS_EvaluateForm(new PDS_EvaluateForm().B_EntityDataCopyForMaterial(this), this.trans);
            rs &= db.BulkDeletePDS_QuestionForm(this.QuestionForms.Select(a => new PDS_QuestionForm().B_EntityDataCopyForMaterial(a)), this.trans);

            if (trans == null)
            {
                if (rs.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return rs;
        }

        public ResultStatus MakeActivePassive()
        {
            this.status = !this.status;
            var rs = db.UpdatePDS_EvaluateForm(new PDS_EvaluateForm().B_EntityDataCopyForMaterial(this));
            return rs;
        }
    }
}
