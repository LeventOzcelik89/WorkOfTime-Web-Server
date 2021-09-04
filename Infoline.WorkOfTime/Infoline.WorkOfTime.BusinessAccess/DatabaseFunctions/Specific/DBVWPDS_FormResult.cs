using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(PDS_FormResult), "status")]
    public enum EnumPDS_FormResultStatus
    {
        [Description("Hiç Değerlendirme Yapılmadı")]
        NoEvaluate = 0,
        [Description("Değerlendirme yapıldı, güncellenebilir.")]
        Updatable = 1,
        [Description("Değerlendirme tamamlandı, güncellenemez")]
        NotUpdate = 2,
    }


    partial class WorkOfTimeDatabase
    {

        public VWPDS_FormResult[] GetVWPDS_FormResultByReport(Guid[] EvaluatorIds, Guid[] EvaluatedIds, Guid[] FormIds, Guid[] TaskIds, int Period, DateTime? StartDate, DateTime? EndDate, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var start = "";
                var end = "";

                var str = "Select * From VWPDS_FormResult with(nolock) Where 1=1";

                if (EvaluatorIds != null && EvaluatorIds.Count() > 0)
                {
                    str += " And evaluatorId In('" + String.Join("','", EvaluatorIds) + "')";
                }

                if (EvaluatedIds != null && EvaluatedIds.Count() > 0)
                {
                    str += " AND evaluatedUserId In('" + String.Join("','", EvaluatedIds) + "')";
                }

                if (TaskIds != null && TaskIds.Count() > 0)
                {
                    str += " AND formPermitTaskUserId In('" + String.Join("','", TaskIds) + "')";
                }

                if (FormIds != null && FormIds.Count() > 0)
                {
                    str += " AND evaluateFormId In('" + String.Join("','", FormIds) + "')";
                }



                if (Period == 0)
                {
                    start = StartDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    end = EndDate.Value.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else if (Period == 1)
                {
                    start = new DateTime(DateTime.Now.Year, 1, 1).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    end = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }
                else
                {
                    start = DateTime.Now.AddDays(-1 * (Period)).ToString("yyyy-MM-dd HH:mm:ss.fff");
                    end = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                }

                str += " AND created>'" + start + "' AND created<'" + end + "'";

                return db.ExecuteReader<VWPDS_FormResult>(str).ToArray();
            }
        }



        public VWPDS_FormResult[] GetVWPDS_FormResultByFormId(Guid formId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPDS_FormResult>().Where(a => a.evaluateFormId == formId).Execute().ToArray();
            }
        }

        public VWPDS_FormResult[] GetVWPDS_FormResultByEvaluatorIdEvaluatedUserIdAndFormId(Guid evaluatorId, Guid evaluatedUserId, Guid evaluateFormId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPDS_FormResult>().Where(a => a.evaluatorId == evaluatorId && a.evaluatedUserId == evaluatedUserId && a.evaluateFormId == evaluateFormId).Execute().ToArray();
            }
        }

    }

}
