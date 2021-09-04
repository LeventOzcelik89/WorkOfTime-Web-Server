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
    [EnumInfo(typeof(DOC_DocumentRevisionRequestConfirmation), "status")]
    public enum EnumDOC_DocumentRevisionRequestConfirmationStatus
    {
        [Description("Reddet")]
        Red = 0,
        [Description("Onayla")]
        Onay = 1
    }

    partial class WorkOfTimeDatabase
    {
        public DOC_DocumentRevisionRequestConfirmation[] GetDOC_DocumentRevisionRequestConfirmationByDocumentId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<DOC_DocumentRevisionRequestConfirmation>().Where(a => a.documentId == id).Execute().ToArray();
            }
        }


        public DOC_DocumentRevisionRequestConfirmation[] GetDOC_DocumentRevisionRequestConfirmationByRequestId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<DOC_DocumentRevisionRequestConfirmation>().Where(a => a.revisionRequestId == id).Execute().ToArray();
            }
        }
    }
}
