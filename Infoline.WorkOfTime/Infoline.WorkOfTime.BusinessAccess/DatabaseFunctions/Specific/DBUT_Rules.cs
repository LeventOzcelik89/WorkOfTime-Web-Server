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
    [EnumInfo(typeof(UT_Rules), "type")]
    public enum EnumUT_RulesType
    {
        [Description("İzin")]
        Permit = 0,
        [Description("Görevlendirme")]
        Commission = 10,
        [Description("Masraf")]
        Transaction = 20,
        [Description("Avans")]
        Advance = 30

    }

    partial class WorkOfTimeDatabase
    {
        public UT_Rules GetUT_RulesByTypeIsDefault(int type, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_Rules>().Where(x => x.type == type && x.isDefault == true).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public UT_Rules GetUT_RulesByTypeIsDefaultById(int type, Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<UT_Rules>().Where(x => x.type == type && x.isDefault == true && x.id != id).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }
    }
}
