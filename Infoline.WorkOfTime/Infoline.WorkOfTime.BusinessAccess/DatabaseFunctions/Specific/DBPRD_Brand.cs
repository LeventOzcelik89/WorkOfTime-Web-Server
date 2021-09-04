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
        public PRD_Brand GetPRD_BrandByName(string name)
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_Brand>().Where(a => a.name == name).Execute().FirstOrDefault();
            }
        }

        public PRD_Brand GetPRD_BrandByNameByUpdateName(Guid id, string name, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<PRD_Brand>().Where(a => a.id != id && a.name == name).OrderBy(a => a.created).Execute().FirstOrDefault();
            }
        }
    }
}

