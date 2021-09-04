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
   
    partial class WorkOfTimeDatabase
    {

        public SH_PersonReferences GetSH_PersonReferencesByAllItem(Guid userId,string ReferenceMail, string ReferencePhone, string ReferencePosition, string ReferenceUserName, string ReferenceWorkingCompany, DbTransaction tran = null)
        {
            using (var db = GetDB(tran)) 

            {
                return db.Table<SH_PersonReferences>().Where(x => x.UserId == userId && x.ReferenceMail == ReferenceMail && x.ReferencePhone == ReferencePhone && x.ReferencePosition == ReferencePosition && x.ReferenceUserName == ReferenceUserName && x.ReferenceWorkingCompany == ReferenceWorkingCompany).Execute().FirstOrDefault();
            }
        }

        public SH_PersonReferences GetSH_PersonReferencesByIdAndAllItem(Guid id, Guid userId, string ReferenceMail, string ReferencePhone, string ReferencePosition, string ReferenceUserName, string ReferenceWorkingCompany, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<SH_PersonReferences>().Where(x => x.id != id && x.UserId == userId && x.ReferenceMail == ReferenceMail && x.ReferencePhone == ReferencePhone && x.ReferencePosition == ReferencePosition && x.ReferenceUserName == ReferenceUserName && x.ReferenceWorkingCompany == ReferenceWorkingCompany).Execute().FirstOrDefault();
            }
        }
    }
}
