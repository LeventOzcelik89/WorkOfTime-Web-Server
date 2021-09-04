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
    [EnumInfo(typeof(VWDOC_DocumentRevisionRequest), "lastStatus")]
    public enum EnumVWDOC_DocumentRevisionRequest
    {
        [Description("Revizyon Talebi Yapıldı"), Generic("icon", "icon-updown-circle", "color", "")]
        talepyapildi = 0,
        [Description("Revizyon Talebi Reddedildi"), Generic("icon", "icon-cancel", "color", "")]
        talepreddedildi = 1,
        [Description("Onay Bekleniyor"), Generic("icon", "icon-progress-2", "color", "")]
        onaybekleniyor = 2,
        [Description("Onaylandı"), Generic("icon", "icon-ok", "color", "")]
        onaylandi = 3
    }

    partial class WorkOfTimeDatabase
    {
        public DOC_DocumentRevisionRequest[] GetDOC_DocumentRevisionRequestByDocumentId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<DOC_DocumentRevisionRequest>().Where(a => a.documentId == id).Execute().ToArray();
            }
        }

    }
}
