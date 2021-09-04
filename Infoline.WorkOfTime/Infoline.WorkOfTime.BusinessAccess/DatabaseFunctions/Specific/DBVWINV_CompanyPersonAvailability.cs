using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {
        public VWINV_CompanyPersonAvailabilityPageReport GetVWINV_CompanyPersonAvailabilitySummary(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonAvailabilityPageReport>().Execute().FirstOrDefault();
            }
        }

        public VWINV_CompanyPersonAvailability[] GetVWINV_CompanyPersonAvailabilityByProjectId(Guid IdProject, int type, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonAvailability>().Where(a => a.IdProject == IdProject && a.type == type).Execute().ToArray();
            }
        }
        public VWINV_CompanyPersonAvailability[] GETVWINV_CompanyPersonAvailabilityByIdUser(Guid IdUser, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonAvailability>().Where(x => x.IdUser == IdUser).Execute().ToArray();
            }
        }



        public avabilit[] GetVWINV_CompanyPersonAvailabilityFilter(Guid[] IdUsers, Guid[] projectId, DateTime start, DateTime end, DbTransaction tran = null)
        {

            var personIds = String.Join("','", IdUsers);
            var projectIds = String.Join("','", projectId);
            using (var db = GetDB(tran))
            {
                var res = db.ExecuteReader<avabilit>("Select CONCAT(MONTH(StartDate),'.', YEAR(StartDate)) AS tarih, Sum(rate) AS rate,Person_Title  AS personel,Project_Title AS proje From  VWINV_CompanyPersonAvailability  where StartDate >= {0} and EndDate <= {1} and IdUser In('" + personIds + "') and IdProject In('" + projectIds + "') group by CONCAT(MONTH(StartDate),'.', YEAR(StartDate)), Person_Title, Project_Title", start, end).ToArray();
                return res;
            }
        }
    }

    public class avabilit
    {
        public string tarih { get; set; }
        public double? rate { get; set; }
        public string personel { get; set; }
        public string proje { get; set; }
    }


}



