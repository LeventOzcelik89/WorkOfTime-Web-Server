using System;
using System.Data.Common;
using System.Linq;
using Infoline.WorkOfTime.BusinessData;
using Infoline.Framework.Database;
using System.Collections.Generic;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public partial class WorkOfTimeDatabase
    {

        public VWSH_User[] GetVWSH_UserMyPerson(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.type == (int)EnumSH_UserType.MyPerson).Execute().ToArray();
            }
        }

        public VWSH_User[] GetVWSH_UserMyPersonIsWorking(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.type == (int)EnumSH_UserType.MyPerson && a.IsWorking == true).Execute().ToArray();
            }
        }

        public VWSH_User[] GetVWSH_UserMyPersonIsWorkingIDS(IEnumerable<Guid> userids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a =>
                        a.type == (int)EnumSH_UserType.MyPerson &&
                        a.IsWorking == true &&
                        a.id.In(userids.ToArray())
                    ).Execute().ToArray();
            }
        }

        public VWSH_User[] GetVWSH_UserOtherPerson(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.type == (int)EnumSH_UserType.OtherPerson).Execute().ToArray();
            }
        }

        public VWSH_User[] GetVWSH_UserOtherPerson(Guid[] companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.type == (int)EnumSH_UserType.OtherPerson && a.CompanyId.In(companyId)).Execute().OrderBy(x => x.FullName).ToArray();
            }
        }

        public VWSH_User[] GetVWSH_UserOtherPersonWithCond(SimpleQuery simpleQuery, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSH_User>().ExecuteSimpleQuery(simpleQuery).ToArray();
            }
        }
        public VWSH_User[] GetVWSH_UserByIds(Guid[] id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.ExecuteReader<VWSH_User>("SELECT * From VWSH_User with(nolock) where id in(" + string.Format("'{0}'", string.Join("','", id)) + ")").ToArray();
            }
        }

        public VWSH_User[] GetVWSH_UserByCompanyIdEndPersonNotNull(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.CompanyId == companyId && a.IsWorking == true).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public VWSH_User[] GetVWSH_UserByCompanyId(Guid companyId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.CompanyId == companyId).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public VWSH_User[] GetVWSH_UserByRoleId(string roleId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.RoleIds.Contains(roleId)).Execute().ToArray();
            }
        }

        public VWSH_User GetVWSH_UserByCompanyIdAndName(Guid companyId, string name, string lastName, string email, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.CompanyId == companyId && ((a.firstname == name && a.lastname == lastName) || a.email == email)).Execute().FirstOrDefault();
            }
        }

        public VWSH_User[] GetVWSH_UserByCompanyIds(Guid[] companyIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.CompanyId.In(companyIds)).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public VWSH_User[] GetSH_UserByCalendar(DateTime start, DateTime end)
        {

            using (var db = GetDB())
            {
                var _company = GetVWCMP_CompanyMyCompanies().Select(c => c.id).ToArray();

                return db.Table<VWSH_User>().Where(a => a.IsWorking == true && a.CompanyId.In(_company) && a.birthday >= start && a.birthday <= end).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public VWSH_User[] GetPersonsBirthdayByToday(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.birthday != null).Execute().
                    Where(a => a.birthday.Value.Date.Day == DateTime.Now.Date.Day && a.birthday.Value.Date.Month == DateTime.Now.Date.Month).ToArray();
            }
        }

        public VWSH_User[] GetPersonsJobStartByToday(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.JobStartDate != null).Execute().
                    Where(a => a.JobStartDate.Value.Date.Year != DateTime.Now.Date.Year &&
                    a.JobStartDate.Value.Date.Day == DateTime.Now.Date.Day && a.JobStartDate.Value.Date.Month == DateTime.Now.Date.Month).ToArray();
            }
        }

        public LogTable[] GetSH_UserLog(Guid userId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                var query = "Select Top 20 * from(" +
                    "SELECT '/SH/VWSH_User/Detail' as tablo, 'fa fa-user' as icon, 'Yeni kullanıcı eklendi.' as description, id, created FROM SH_User with(nolock) where createdby = '" + userId + "' UNION ALL " +
                    "SELECT '/CMP/VWCMP_Company/Detail' as tablo, 'fa fa-building' as icon, 'Yeni işletme eklendi.' as description, id, created FROM CMP_Company with(nolock) where createdby = '" + userId + "' UNION ALL " +
                    "SELECT '/CRM/VWCRM_Contact/Detail' as tablo, 'fa fa-users' as icon, 'Yeni randevu oluşturuldu.' as description, id, created FROM CRM_Contact with(nolock) where createdby = '" + userId + "' UNION ALL " +
                    "SELECT '/CRM/VWCRM_Presentation/Detail' as tablo, 'fa fa-dollar' as icon, 'Yeni potansiyel oluşturuldu.' as description, id, created FROM CRM_Presentation with(nolock) where createdby = '" + userId + "' UNION ALL  " +
                    "SELECT '/FTM/VWFTM_Task/Detail' as tablo, 'fa fa-wrench' as icon, 'Yeni saha görevi oluşturuldu.' as description, id, created FROM FTM_Task with(nolock) where createdby = '" + userId + "' UNION ALL " +
                    "SELECT '/HR/VWHR_StaffNeeds/Detail' as tablo, 'fa fa-binoculars' as icon, 'Yeni personel talebinde yapıldı.' as description, id, created FROM HR_StaffNeeds with(nolock) where createdby = '" + userId + "' UNION ALL " +
                    "SELECT '/INV/VWINV_Commissions/Detail' as tablo, 'fa fa-car' as icon, 'Görevlendirme talebi yapıldı.' as description, id, created FROM INV_Commissions with(nolock) where createdby = '" + userId + "' UNION ALL " +
                    "SELECT '/INV/VWINV_Permit/Detail' as tablo, 'fa fa-umbrella' as icon, 'İzin talebi yapıldı.' as description, id, created FROM INV_Permit with(nolock) where createdby = '" + userId + "' UNION ALL " +
                    "SELECT '/PDS/VWPDS_EvaluateForm/Detail' as tablo, 'fa fa-area-chart' as icon, 'Değerlendirme formu eklendi' as description, id, created FROM PDS_EvaluateForm with(nolock) where createdby = '" + userId + "' UNION ALL " +
                    "SELECT '/PDS/VWPDS_FormResult/Detail' as tablo, 'fa fa-line-chart' as icon, 'Değerlendirme formu dolduruldu.' as description, id, created FROM PDS_FormResult with(nolock) where createdby = '" + userId + "' UNION ALL " +
                    "SELECT '/PRD/VWPRD_Inventory/Detail' as tablo, 'fa fa-cube' as icon, 'Yeni envanter eklendi.' as description, id, created FROM PRD_Inventory with(nolock) where createdby = '" + userId + "' UNION ALL " +
                    "SELECT '/PRD/VWPRD_Product/Detail' as tablo, 'fa fa-cubes' as icon, 'Yeni ürün eklendi.' as description, id, created FROM PRD_Product with(nolock) where createdby = '" + userId + "' UNION ALL " +
                    "SELECT '/PRJ/VWPRJ_Project/Detail' as tablo, 'fa fa-briefcase' as icon, 'Yeni proje eklendi.' as description, id, created FROM PRJ_Project with(nolock) where createdby = '" + userId + "') as query order by created desc";

                var result = db.ExecuteReader<LogTable>(query).ToArray();
                return result;
            }
        }
        public VWSH_User[] GetVWSH_UserByIds(SimpleQuery simpleQuery, Guid?[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {

                return db.Table<VWSH_User>().ExecuteSimpleQuery(simpleQuery).Where(x => ids.Contains(x.id)).ToArray();
            }
        }



        public VWSH_User[] GetVWSH_UserByCompanyIdAndUserIds(Guid companyId, Guid[] ids, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.CompanyId == companyId && a.id.In(ids)).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public VWSH_User[] GetVWSH_UserMyPersonByIds(Guid[] id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.id.In(id) && a.type == (int)EnumSH_UserType.MyPerson).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }

        public string[] GetVWSH_UserEmails(DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWSH_User>().Where(a => a.email != null).Select(a => new VWSH_User { email = a.email }).Execute().Select(a => a.email).Distinct().ToArray();
            }
        }
    }

    public class LogTable
    {
        public string icon { get; set; }
        public string tablo { get; set; }
        public string description { get; set; }
        public Guid id { get; set; }
        public DateTime created { get; set; }
    }

}