using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public partial class WorkOfTimeDatabase
    {

        public HR_PersonKeywords[] GetHR_PersonKeywordsByHrPersonId(Guid id)
        {
            using (var db = GetDB())
            {
                return db.Table<HR_PersonKeywords>().Where(a => a.HrPersonId == id).Execute().ToArray();
            }
        }
    }
}
