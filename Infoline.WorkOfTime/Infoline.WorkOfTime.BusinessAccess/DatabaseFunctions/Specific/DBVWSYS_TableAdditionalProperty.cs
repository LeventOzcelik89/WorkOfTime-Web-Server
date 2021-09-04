using System;
using System.Linq;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public VWSYS_TableAdditionalProperty[] GetVWSYS_TableAdditionalPropertyByDataIdAndDataTable(Guid dataId, string dataTable, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSYS_TableAdditionalProperty>().Where(a => a.dataId == dataId && a.dataTable == dataTable).Execute().ToArray();
            }
        }



    }
}
