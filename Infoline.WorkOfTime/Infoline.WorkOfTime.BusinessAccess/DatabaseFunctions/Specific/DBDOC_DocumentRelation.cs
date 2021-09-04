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
        public DOC_DocumentRelation[] GetDOC_DocumentRelationByDocumentId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<DOC_DocumentRelation>().Where(a => a.documentId == id).Execute().ToArray();
            }
        }

        public DOC_DocumentRelation[] GetDOC_DocumentRelationByDocumentRelationId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<DOC_DocumentRelation>().Where(a => a.documentRelationId == id).Execute().ToArray();
            }
        }
    }
}
