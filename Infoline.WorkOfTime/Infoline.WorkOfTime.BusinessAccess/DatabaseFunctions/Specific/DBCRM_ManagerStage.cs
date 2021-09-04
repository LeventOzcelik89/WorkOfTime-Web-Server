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

        public CRM_ManagerStage GetCRM_ManagerStageControl(string code)
        {
            using (var db = GetDB())
            {
                return db.Table<CRM_ManagerStage>().Where(a => a.Code == code).Execute().FirstOrDefault();
            }
        }

        public CRM_ManagerStage GetCRM_ManagerStageUpdate(Guid id, string name)
        {
            using (var db = GetDB())
            {
                return db.Table<CRM_ManagerStage>().Where(a => a.id != id && a.Code == name).Execute().FirstOrDefault();
            }
        }

        public CRM_ManagerStage GetCRM_ManagerStageDefaultValue()
        {
            using (var db = GetDB())
            {
                return db.Table<CRM_ManagerStage>().OrderBy(a => a.Code).Execute().FirstOrDefault();
            }
        }

    }
}
