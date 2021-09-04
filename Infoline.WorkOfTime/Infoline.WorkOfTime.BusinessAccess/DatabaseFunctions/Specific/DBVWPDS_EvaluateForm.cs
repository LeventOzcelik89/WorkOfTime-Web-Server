using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(PDS_EvaluateForm), "formType")]
    public enum EnumPDS_EvaluateFormType
    {
        [Description("Genel Form")]
        General = 0,
        [Description("Performans Form")]
        Performance = 1,
        [Description("İş Görüşme Değerlendirme Formu")]
        Interview = 2,
        [Description("Araştırma Formu")]
        Survey = 3

    }

    [EnumInfo(typeof(PDS_EvaluateForm), "status")]
    public enum EnumPDS_EvaluateFormStatus
    {
        [Description("Pasif")]
        Passive = 0,
        [Description("Aktif")]
        Active = 1,

    }

    partial class WorkOfTimeDatabase
    {
        public VWPDS_EvaluateForm GetVWPDS_EvaluateFormByFormName(string formName, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPDS_EvaluateForm>().Where(a => a.formName == formName).Execute().FirstOrDefault();
            }
        }
        public VWPDS_EvaluateForm GetVWPDS_EvaluateFormByInterviewFirst()
        {
            using (var db = GetDB())

            {
                return db.Table<VWPDS_EvaluateForm>().Where(a => a.formType == (Int32)EnumPDS_EvaluateFormType.Interview).OrderByDesc(c => c.created).Execute().FirstOrDefault();
            }
        }

    }
}
