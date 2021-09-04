using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    partial class WorkOfTimeDatabase
    {
        public PRD_ProductionSchema GetPRD_ProductionSchemaByCode(string code)
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_ProductionSchema>().Where(a => a.code == code).Execute().FirstOrDefault();
            }
        }
    }
}
