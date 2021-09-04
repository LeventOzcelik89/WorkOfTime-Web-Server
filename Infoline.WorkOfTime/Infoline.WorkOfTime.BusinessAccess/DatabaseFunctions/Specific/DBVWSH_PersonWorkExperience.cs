using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {
        public VWSH_PersonWorkExperience GetVWSH_PersonWorkExperienceIds(Guid UserId, string CompanyName, string WorkingPosition, DateTime JobStartDate, DateTime JobEndDate, string JobDescription, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_PersonWorkExperience>().Where(x => x.UserId == UserId && x.CompanyName == CompanyName && x.WorkingPosition == WorkingPosition &&  x.JobStartDate == JobStartDate && x.JobEndDate == JobEndDate && x.JobDescription == JobDescription).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public VWSH_PersonWorkExperience[] GetVWSH_PersonWorkExperienceByUserId(Guid UserId)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_PersonWorkExperience>().Where(a => a.UserId == UserId).Execute().ToArray();
            }
        }
    }
}