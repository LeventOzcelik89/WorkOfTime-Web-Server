using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
	public class VMPDS_QuestionFormResult
    {
        public VWPDS_QuestionResult QuestionResult { get; set; }
        public VWPDS_QuestionForm QuestionForm { get; set; }
    }

    public class VMVWPDS_QuestionObject
    {
        public VWPDS_QuestionResult QuestionResult { get; set; }
        public VWPDS_QuestionForm QuestionForm { get; set; }
    }

    public class VWPDS_PerformanceEvaluateReport
    {
        public Guid[] EvaluatorIds { get; set; }
        public Guid[] EvaluatedIds { get; set; }
        public Guid[] FormIds { get; set; }
        public Guid[] TaskIds { get; set; }
        public string[] Questions { get; set; }
        public int Period { get; set; }
        public DateTime performanceStartDate { get; set; }
        public DateTime performanceEndDate { get; set; }
        public string[] GroupNames { get; set; }
        public string filterType { get; set; }

    }

    public class VWPDS_PersonalPerformanceReport
    {
        public string PersonelName { get; set; }
        public string EvaluatedPersonel { get; set; }
        public Guid PersonId { get; set; }
        public string ProfilePhoto { get; set; }
        public string PersonTitle { get; set; }
        public string Company { get; set; }
        public bool detail { get; set; }
    }

    public class VMVWPDS_FormPermitTaskUserDetail : VWPDS_FormPermitTaskUser
    {
        public int BuAyYaptigimDegerlendirmeSayisi { get; set; }
        public double? EnYuksekPuan { get; set; }
        public double? EnDusukPuan { get; set; }
        public double? OrtalamaPuan { get; set; }
    }

    public class VWPDS_FormResultCustom : VWPDS_FormResult
    {
        public VWPDS_QuestionResult[] QuestionResult { get; set; }
    }

    public class VMPDS_FormResultModel : VWPDS_FormResult
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWPDS_EvaluateForm Form { get; set; }
        public VWPDS_FormPermitTaskUser FormPermitTaskUser { get; set; }
        public VMPDS_QuestionFormResult[] QuestionFormResults { get; set; }

        public VMPDS_FormResultModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var formResult = db.GetVWPDS_FormResultById(this.id);

            if (formResult != null)
            {
                this.B_EntityDataCopyForMaterial(formResult, true);
            }
            else
            {
                this.id = Guid.NewGuid();
                this.formType = (int)EnumPDS_EvaluateFormType.Interview;
            }

            if (this.formPermitTaskUserId.HasValue)
            {
                this.FormPermitTaskUser = db.GetVWPDS_FormPermitTaskUserById(this.formPermitTaskUserId.Value);
                this.evaluatedUserId = this.FormPermitTaskUser.evaluatedUserId;
                this.evaluatorId = this.FormPermitTaskUser.evaluatorId;

                if (this.FormPermitTaskUser.evaluateFormId.HasValue)
                {
                    this.evaluateFormId = this.FormPermitTaskUser.evaluateFormId;
                }
            }

            if (this.evaluateFormId.HasValue)
            {
                this.Form = db.GetVWPDS_EvaluateFormById(this.evaluateFormId.Value);
                var questionForms = db.GetVWPDS_QuestionFormByFormId(this.evaluateFormId.Value).OrderBy(a => a.groupOrder).ThenBy(a => a.questionOrder).ToArray();
                var questionResults = db.GetVWPDS_QuestionResultByFormResultId(this.id);

                var questionFormResultList = new List<VMPDS_QuestionFormResult>();
                foreach (var qf in questionForms)
                {
                    questionFormResultList.Add(new VMPDS_QuestionFormResult
                    {
                        QuestionForm = qf,
                        QuestionResult = questionResults.Where(a => a.questionFormId == qf.id).FirstOrDefault() ?? new VWPDS_QuestionResult()
                    });
                }

                this.QuestionFormResults = questionFormResultList.ToArray();
            }



            return this;
        }
        public ResultStatus Save(Guid userid, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var formResult = db.GetVWPDS_FormResultById(this.id);
            this.trans = trans ?? db.BeginTransaction();
            var rs = new ResultStatus { result = true };

            var sumFactor = this.QuestionFormResults.Select(a => a.QuestionForm).Sum(a => a.factor.Value);

            foreach (var questionFormResult in this.QuestionFormResults)
            {
                if (questionFormResult.QuestionResult.score == 0)
                {
                    sumFactor -= questionFormResult.QuestionForm.factor.Value;
                }
            }

            if(sumFactor == 0)
            {
                rs.result = false;
                rs.message = "En az 1 soru değerlendirmiş olmanız gerekmektedir.";
                this.trans.Rollback();
                return rs;
            }
            
            foreach (var questionFormResult in this.QuestionFormResults)
            {
                var point = (questionFormResult.QuestionResult.score * questionFormResult.QuestionForm.factor * 100) / (5 * sumFactor);
                questionFormResult.QuestionResult.point = point.Value;
            }

            if (formResult == null)
            {
                this.createdby = userid;
                this.created = DateTime.Now;
                rs = Insert();
            }
            else
            {
                this.changedby = userid;
                this.changed = DateTime.Now;
                rs = Update();
            }

            if (trans == null)
            {
                if (rs.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return rs;
        }

        private ResultStatus Insert()
        {
            var rs = this.db.InsertPDS_FormResult(new PDS_FormResult().B_EntityDataCopyForMaterial(this), this.trans);
            rs &= this.db.BulkInsertPDS_QuestionResult(this.QuestionFormResults.Select(a => new PDS_QuestionResult().B_EntityDataCopyForMaterial(a.QuestionResult)).ToList(), this.trans);
            return rs;
        }

        private ResultStatus Update()
        {
            var rs = db.UpdatePDS_FormResult(new PDS_FormResult().B_EntityDataCopyForMaterial(this), false, this.trans);
            var questionResults = db.GetPDS_QuestionResultByFormResultId(this.id);
            rs &= db.BulkDeletePDS_QuestionResult(questionResults, this.trans);
            rs &= db.BulkInsertPDS_QuestionResult(this.QuestionFormResults.Select(a => new PDS_QuestionResult().B_EntityDataCopyForMaterial(a.QuestionResult)).ToList(), this.trans);

            return rs;
        }
    }
}
