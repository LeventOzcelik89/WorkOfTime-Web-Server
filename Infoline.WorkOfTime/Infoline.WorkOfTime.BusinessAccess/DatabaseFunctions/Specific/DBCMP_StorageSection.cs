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
    [EnumInfo(typeof(CMP_Storage), "type")]
    public enum EnumCMP_StorageSectionType
    {
        [Description("Depo")]
        Depo = 0,
        [Description("Alt Birim")]
        AltBirim = 1
    }

    partial class WorkOfTimeDatabase
    {

        public int GetCMP_StorageSectionByPidIdCount(Guid pidId,Guid storageId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_StorageSection>().Where(a => a.pid == pidId && a.storageId == storageId).Execute().Count();
            }
        }

        public CMP_StorageSection[] GetCMP_StorageSectionByPidId(Guid? id, Guid storageId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_StorageSection>().Where(a => a.pid == id && a.storageId == storageId).Execute().ToArray();
            }
        }
    }
}
