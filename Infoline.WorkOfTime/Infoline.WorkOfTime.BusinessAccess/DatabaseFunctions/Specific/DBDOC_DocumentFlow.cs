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
    [EnumInfo(typeof(DOC_DocumentFlow), "type")]
    public enum EnumDOC_DocumentFlowType
    {
        [Description("Organizasyon")]
        OrganizationUnit = 0,
        [Description("Kullanıcı")]
        User = 1
    }

    partial class WorkOfTimeDatabase
    {
        public DOC_DocumentFlow[] GetDOC_DocumentFlowByDocumentId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<DOC_DocumentFlow>().Where(a => a.documentId == id).Execute().ToArray();
            }
        }

        public VWDOC_DocumentFlow[] GetVWDOC_DocumentFlowByDocumentId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWDOC_DocumentFlow>().Where(a => a.documentId == id).Execute().ToArray();
            }
        }
    }
}
