using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Infoline.WorkOfTime.BusinessAccess
{
    partial class WorkOfTimeDatabase
    {
        public CMP_CompanyFileSelector[] GetVWCMP_CompanyFileSelectorByCustomerId(Guid customerId,DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<CMP_CompanyFileSelector>().Where(a => a.customerId == customerId).Execute().ToArray();
            }
        }

    }
}
