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

        public PDS_QuestionForm[] GetPDS_QuestionFormByFormId(Guid formId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PDS_QuestionForm>().Where(a => a.evaluateFormId == formId).Execute().ToArray();
            }
        }
    }
}
