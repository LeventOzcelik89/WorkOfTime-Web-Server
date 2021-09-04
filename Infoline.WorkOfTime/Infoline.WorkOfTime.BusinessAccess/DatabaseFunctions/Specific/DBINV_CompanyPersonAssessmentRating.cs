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
    [EnumInfo(typeof(INV_CompanyPersonAssessmentRating), "answer")]
        public enum EnumINV_CompanyPersonAssessmentAnswer
        {
            [Description("Başarılı")]
            Basarili = 10,
            [Description("Gelişmesi Gerekli")]
            GelismesiGerekli = 20,
            [Description("Başarısız")]
            Basarisiz = 30,
        }
    partial class WorkOfTimeDatabase
    {
        public INV_CompanyPersonAssessmentRating[] GetINV_CompanyPersonAssessmentRatingByAssessmentId(Guid assessmentId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<INV_CompanyPersonAssessmentRating>().Where(a => a.assessmentId == assessmentId).Execute().ToArray();
            }
        }
    }
}
