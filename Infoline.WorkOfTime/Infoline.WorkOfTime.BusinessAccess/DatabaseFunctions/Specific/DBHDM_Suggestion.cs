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

    [EnumInfo(typeof(HDM_Suggestion), "status")]
    public enum EnumHDM_SuggestionStatus
    {
        [Description("Taslak")]
        Taslak = 0,
        [Description("Yayında")]
        Yayında = 1,
    }

    partial class WorkOfTimeDatabase
    {
        public HDM_Suggestion[] GetHDM_SuggestionByIssueIds(Guid[] issueIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<HDM_Suggestion>().Where(a => a.issueId.In(issueIds)).Execute().ToArray();
            }
        }

        public HDM_Suggestion[] GetHDM_SuggestionsByIssueId(Guid issueId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<HDM_Suggestion>().Where(a => a.issueId == issueId).Execute().ToArray();
            }
        }
    }
}
