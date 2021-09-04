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

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {


        public VWPDS_QuestionResult[] GetVWPDS_QuestionResultByFormResultId(Guid formResultId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWPDS_QuestionResult>().Where(a => a.formResultId == formResultId).Execute().ToArray();
            }
        }

        public VWPDS_QuestionResult[] GetVWPDS_QuestionResultByFormResultIds(Guid[] ids)
        {
            using (var db = GetDB())
            {
                var str = " formResultId In('" + String.Join("','", ids) + "')";
                return db.ExecuteReader<VWPDS_QuestionResult>("select * from VWPDS_QuestionResult Where " + str).ToArray();
            }   
        }
    }
}
