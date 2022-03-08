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
    partial class WorkOfTimeDatabase
    {
        public DOC_DocumentScope[] GetDOC_DocumentScopeByDocumentId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<DOC_DocumentScope>().Where(a => a.documentId == id).Execute().ToArray();
            }
        }
        public DOC_DocumentScope[] GetDOC_DocumentScopeByOrganizationId(Guid organizationId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<DOC_DocumentScope>().Where(a => a.organizationUnitId == organizationId).Execute().ToArray();
            }
        }
    }
}
