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
        public PRD_EntegrationFiles[] GetPRD_EntegrationFilesByCreatedDate(string distName, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PRD_EntegrationFiles>().Where(a =>a.DistributorName == distName).Execute().ToArray();
            }
        }
        public PRD_EntegrationFiles GetPRD_EntegrationFilesByEntegrationId(Guid entegrationId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<PRD_EntegrationFiles>().Where(a => a.id == entegrationId).Execute().FirstOrDefault();
            }
        }
    }
}
