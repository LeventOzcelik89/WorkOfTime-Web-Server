using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

   
    public partial class WorkOfTimeDatabase
    {
        public SH_WorkAccidentCertificate[] GetSH_WorkAccidentCertificateByWorkAccidentId(Guid workAccidentId)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_WorkAccidentCertificate>().Where(a => a.workAccidentId == workAccidentId).Execute().ToArray();
            }
        }
    }
}
