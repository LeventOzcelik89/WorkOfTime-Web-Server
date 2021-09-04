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
        public PRD_ProductionSchemaStage GetPRD_ProductionSchemaStageByCode(string code)
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_ProductionSchemaStage>().Where(a => a.code == code).Execute().FirstOrDefault();
            }
        }

        public PRD_ProductionSchemaStage GetPRD_ProductionSchemaStageByStageNumAndProductionSchemaId(int stageNum, Guid productionSchemaId)
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_ProductionSchemaStage>().Where(a => a.stageNum == stageNum && a.productionSchemaId == productionSchemaId).Execute().FirstOrDefault();
            }
        }

        public PRD_ProductionSchemaStage[] GetPRD_ProductionSchemaStageByProductionSchemaId(Guid productionSchemaId)
        {
            using (var db = GetDB())
            {
                return db.Table<PRD_ProductionSchemaStage>().Where(b => b.productionSchemaId == productionSchemaId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
    }
}


