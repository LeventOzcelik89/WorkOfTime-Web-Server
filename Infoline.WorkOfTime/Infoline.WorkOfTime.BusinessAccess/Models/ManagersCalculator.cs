using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public class ManagersCalculator
    {

        public static VWINV_CompanyPersonDepartments[] DepartmentsPersons { get; set; }
        public static INV_CompanyDepartments[] Departments { get; set; }
        public static Guid[] Users { get; set; }

        public ManagersCalculator()
        {
            var db = new WorkOfTimeDatabase();
            Users = db.GetSH_User().Where(a => a.status == true).Select(a => a.id).ToArray();
            Departments = db.GetINV_CompanyDepartments();
            DepartmentsPersons = db.GetMyINV_CompanyPersonDepartments();

        }


        public void Run()
        {
            var db = new WorkOfTimeDatabase();
            var updateList = new List<INV_CompanyPersonDepartments>();

            foreach (var item in DepartmentsPersons)
            {
                if (!item.DepartmentId.HasValue) continue;
                var cpd = new INV_CompanyPersonDepartments
                {
                    id = item.id,
                    changed = DateTime.Now,
                    created = item.created,
                    changedby = item.changedby,
                    createdby = item.createdby,
                    DepartmentId = item.DepartmentId,
                    IdUser = item.IdUser,
                    EndDate = item.EndDate,
                    StartDate = item.StartDate,
                    Title = item.Title,
                    IsBasePosition = item.IsBasePosition
                };

                var personManagers = new List<VWINV_CompanyPersonDepartments>();
                GetManagers(personManagers, item.DepartmentId);

                cpd.Manager1 = personManagers.Select(a => a.IdUser).FirstOrDefault();
                cpd.Manager2 = personManagers.Where(a => a.IdUser != cpd.Manager1).Select(a => a.IdUser).FirstOrDefault();
                cpd.Manager3 = personManagers.Where(a => a.IdUser != cpd.Manager1 && a.IdUser != cpd.Manager2).Select(a => a.IdUser).FirstOrDefault();
                cpd.Manager4 = personManagers.Where(a => a.IdUser != cpd.Manager1 && a.IdUser != cpd.Manager2 && a.IdUser != cpd.Manager3).Select(a => a.IdUser).FirstOrDefault();
                cpd.Manager5 = personManagers.Where(a => a.IdUser != cpd.Manager1 && a.IdUser != cpd.Manager2 && a.IdUser != cpd.Manager3 && a.IdUser != cpd.Manager4).Select(a => a.IdUser).FirstOrDefault();
                cpd.Manager6 = personManagers.Where(a => a.IdUser != cpd.Manager1 && a.IdUser != cpd.Manager2 && a.IdUser != cpd.Manager3 && a.IdUser != cpd.Manager4 && a.IdUser != cpd.Manager5).Select(a => a.IdUser).FirstOrDefault();

                updateList.Add(cpd);
            }

            var result = db.BulkUpdateINV_CompanyPersonDepartments(updateList, true);
        }

        public VWINV_CompanyPersonDepartments[] GetAllChilds(Guid userId)
        {
            var res = new List<VWINV_CompanyPersonDepartments>();
            var departmentUsers = DepartmentsPersons
                .Where(a => a.IdUser == userId &&
                a.IsBasePosition == true &&
                (a.EndDate == null || a.EndDate >= DateTime.Now))
                .ToArray();

            if (departmentUsers.Count() == 0)
            {
                return res.ToArray();
            }

            res.AddRange(departmentUsers);

            var departments = Departments
                .Where(a => departmentUsers.Where(b => b.DepartmentId.HasValue).Select(b => b.DepartmentId.Value).Contains(a.id))
                .ToArray();

            foreach (var depart in departments)
            {
                res.AddRange(GetChilds(depart));
            }

            return res.Distinct().ToArray();

        }

        private VWINV_CompanyPersonDepartments[] GetChilds(INV_CompanyDepartments item)
        {
            var res = new List<VWINV_CompanyPersonDepartments>();

            var childDeparts = Departments
                .Where(a => a.PID == item.id)
                .ToArray();

            var childDepartPersons = DepartmentsPersons
                .Where(a => a.DepartmentId.HasValue && childDeparts.Select(b => b.id).Contains(a.DepartmentId.Value))
                .ToArray();

            res.AddRange(childDepartPersons);

            if (childDeparts.Count() == 0)
            {
                return res.ToArray();
            }
            else
            {
                foreach (var childDepart in childDeparts)
                {
                    res.AddRange(GetChilds(childDepart));
                }
            }

            return res.ToArray();
        }

        private void GetManagers(List<VWINV_CompanyPersonDepartments> All, Guid? Departmentid)
        {

            var department = Departments.Where(a => a.id == Departmentid).FirstOrDefault();
            var departmentParent = Departments.Where(a => a.id == department.PID && a.Type == department.Type && a.ProjectId == department.ProjectId).FirstOrDefault();
            
            if (departmentParent != null)
            {
                var departmentParentTwo = Departments.Where(x => x.id == departmentParent.PID && x.Type == departmentParent.Type).FirstOrDefault();
                var departmentParentPerson = DepartmentsPersons.Where(a => a.DepartmentId == departmentParent.id);
                var departmentParentPersonManager = departmentParentPerson.Where(a => a.IdUser.HasValue && Users.Contains(a.IdUser.Value)).ToArray();
                All.AddRange(departmentParentPersonManager);

                if(departmentParentTwo != null)
                {
                    var departmentParentPersonTwo = DepartmentsPersons.Where(a => a.DepartmentId == departmentParentTwo.id);
                    var departmentParentPersonManagerTwo = departmentParentPersonTwo.Where(a => a.IdUser.HasValue && Users.Contains(a.IdUser.Value)).ToArray();
                    All.AddRange(departmentParentPersonManagerTwo);
                }

                GetManagers(All, department.PID);
            }
        }

        public IEnumerable<InfolineTable> PermissionCalculator<T>(Guid userId)
        {
            //var getType = T.GetType();
            //if (getType==new PA_TransactionConfirmation().GetType())
            //{
            //    var confirm = new List<T>();
            //    confirm.Add(new PA_Transaction { });


            //    return confirm;



            //}





            return null;
        }

    }

}
