using Infoline.Framework.Database;
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

                if (departmentParentTwo != null)
                {
                    var departmentParentPersonTwo = DepartmentsPersons.Where(a => a.DepartmentId == departmentParentTwo.id);
                    var departmentParentPersonManagerTwo = departmentParentPersonTwo.Where(a => a.IdUser.HasValue && Users.Contains(a.IdUser.Value)).ToArray();
                    All.AddRange(departmentParentPersonManagerTwo);
                }

                GetManagers(All, department.PID);
            }
        }

        public IEnumerable<T> PermissionCalculator<T>(Guid userId,Guid id) where T : InfolineTable, new()
        {
            var db = new WorkOfTimeDatabase();
            var shuser = db.GetVWSH_UserById(userId);
            var getManagers = db.GetINV_CompanyPersonDepartmentsByIdUserAndTypeCurrentWork(userId, (int)EnumINV_CompanyDepartmentsType.Organization);
            List<T> returnList = null;
            var transactionConfirmation = new List<PA_TransactionConfirmation>();
            var advanceConfirmation = new List<PA_AdvanceConfirmation>();
            var getType = typeof(T);
            var rulesUser = new VWUT_RulesUser();
            var rulesUserStages = new VWUT_RulesUserStage[0];
            if (getType == typeof(PA_AdvanceConfirmation))
            {
                rulesUser = db.GetVWUT_RulesUserByUserIdAndType(userId, (Int16)EnumUT_RulesType.Advance);
                rulesUserStages = new VWUT_RulesUserStage[0];
                if (rulesUser == null)
                {
                    var defaultRule = db.GetUT_RulesByTypeIsDefault((Int16)EnumUT_RulesType.Advance);
                    rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(defaultRule.id);
                }
                else
                {
                    rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(rulesUser.rulesId.Value);
                }

            }
            else if (getType==typeof(PA_TransactionConfirmation))
            {
                rulesUser = db.GetVWUT_RulesUserByUserIdAndType(userId, (Int16)EnumUT_RulesType.Transaction);
                rulesUserStages = new VWUT_RulesUserStage[0];
                if (rulesUser == null)
                {
                    var defaultRule = db.GetUT_RulesByTypeIsDefault((Int16)EnumUT_RulesType.Transaction);
                    rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(defaultRule.id);
                }
                else
                {
                    rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(rulesUser.rulesId.Value);
                }
            }

            if (rulesUserStages.Count() > 0)
            {
               
                foreach (var rulesStage in rulesUserStages.OrderBy(a => a.ruleType == (int)EnumUT_RulesUserStage.SonOnaylayici ? 10000 : a.order))
                {

                    //yönetici departmanda var mı ? 
                    var isInDepartman = shuser.Manager1 == rulesStage.userId ? true
                              : shuser.Manager2 == rulesStage.userId ? true
                              : shuser.Manager3 == rulesStage.userId ? true
                              : shuser.Manager4 == rulesStage.userId ? true
                              : shuser.Manager5 == rulesStage.userId ? true
                              : shuser.Manager6 == rulesStage.userId ? true
                              : false;
                    Guid? assingUser = null;
                    switch ((EnumUT_RulesUserStage)rulesStage.type)
                    {
                        case EnumUT_RulesUserStage.Manager1:
                        case EnumUT_RulesUserStage.Manager2:
                        case EnumUT_RulesUserStage.Manager3:
                        case EnumUT_RulesUserStage.Manager4:
                        case EnumUT_RulesUserStage.Manager5:
                        case EnumUT_RulesUserStage.Manager6:
                            assingUser = (
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager1 ? shuser?.Manager1 :
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager2 ? shuser?.Manager2 :
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager3 ? shuser?.Manager3 :
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager4 ? shuser?.Manager4 :
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager5 ? shuser?.Manager5 :
                     rulesStage.type == (Int16)EnumUT_RulesUserStage.Manager6 ? shuser?.Manager6 :
                      null);
                            //  eğer yöneticiler son onaylayıcı veya rolebaglu veya secim ise devam 
                            var isUserExistBefore = rulesUserStages.Where(x => (x.type != rulesStage.type)
                             && x.userId.HasValue
                             && x.userId == assingUser);
                            if (isUserExistBefore.Count() > 0)
                            {
                                continue;
                            }
                            break;
                        case EnumUT_RulesUserStage.RoleBagliSecim:
                            if (!rulesStage.roleId.HasValue)
                            {
                                return null;
                            }
                            var getRole = db.GetVWSH_RoleById(rulesStage.roleId.Value);
                            if (getRole == null)
                            {
                                return null;
                            }
                            var getRoleUsers = db.GetVWSH_UserByRoleId(getRole.id.ToString());
                            if (getRoleUsers == null)
                            {
                                return null;
                            }
                            var apprvList = new List<VWSH_User>();
                            foreach (var user in getRoleUsers)
                            {
                                //onaylacak kişi departmanda var mı ? 
                                var hasRoleInDepartman = shuser.Manager1 == user.id ? true
                                    : shuser.Manager2 == user.id ? true
                                    : shuser.Manager3 == user.id ? true
                                    : shuser.Manager4 == user.id ? true
                                    : shuser.Manager5 == user.id ? true
                                    : shuser.Manager6 == user.id ? true
                                    : false;
                                if (hasRoleInDepartman)
                                {
                                    apprvList.Add(user);//eğer varsa listeye ekle
                                }
                            }
                            var isApprv = apprvList.Where(x => x.id != shuser.id);//onaylayacak kişileri isteği onaylayacak kişi olmayanları getir.
                            if (apprvList.Count() > 0 && isApprv.Count() > 0)//eğer onaylayacak kişi varsa 
                            {
                                assingUser = isApprv.FirstOrDefault().id;//ilkini getir
                            }
                            else
                            {
                                var roleUser = getRoleUsers.Where(x => x.id != shuser.id);//eğer onaylayacak kimse yoksa kendi olmayanı getir
                                if (roleUser.Count() > 0)
                                {
                                    assingUser = roleUser.FirstOrDefault().id;//ilkini al 
                                }
                            }
                            isUserExistBefore = rulesUserStages.Where(x => (x.type != rulesStage.type)
                             && x.userId.HasValue
                             && x.userId == assingUser);
                            if (isUserExistBefore.Count() > 0)
                            {
                                continue;
                            }
                            break;
                        case EnumUT_RulesUserStage.SecimeBagliKullanici:
                            assingUser = rulesStage.userId;
                            isUserExistBefore = rulesUserStages.Where(x => (x.type != rulesStage.type)
                              && x.userId.HasValue
                              && x.userId == assingUser);
                            if (isUserExistBefore.Count() > 0)
                            {
                                continue;
                            }
                            break;
                        case EnumUT_RulesUserStage.SonOnaylayici:
                            //son onaylacak kişi, istek yapanın son kullanıcılarında yoksa bu adımı geç
                            if (!isInDepartman)
                            {
                                continue;
                            }
                            //son onaylayacak kullanıcı kişinin son adımlarında varsa son onaylayacak kullanıcıyı ekle
                            assingUser = rulesStage.userId;
                            break;
                        default:
                            break;
                    }
                    if (assingUser == null)
                    {
                        continue;//eğer onaylayacak kimsesi yoksa bunu veri tabanına ekleme;
                    }
                    if (assingUser == shuser.id)
                    {
                        continue;
                    }

                    if (getType == typeof(PA_AdvanceConfirmation))
                    {
                        
                        var isUsedBefore =advanceConfirmation.Where(x =>x.userId == assingUser).OrderByDescending(x => x.ruleOrder);
                        if (isUsedBefore.Count() > 0)
                        {
                            advanceConfirmation.Remove(isUsedBefore.FirstOrDefault());
                        }
                        advanceConfirmation.Add(new PA_AdvanceConfirmation
                        {
                            created = DateTime.Now,
                            createdby = userId,
                            advanceId = id,
                            ruleType = rulesStage.type,
                            ruleOrder = rulesStage.order,
                            ruleUserId = rulesStage.userId,
                            ruleRoleId = rulesStage.roleId,
                            ruleTitle = rulesStage.title,
                            userId = assingUser
                        });

                    }
                    else if (getType == typeof(PA_TransactionConfirmation))
                    {
                        var isUsedBefore = transactionConfirmation.Where(x => x.userId == assingUser).OrderByDescending(x => x.ruleOrder);
                        if (isUsedBefore.Count() > 0)
                        {
                            transactionConfirmation.Remove(isUsedBefore.FirstOrDefault());
                        }
                        transactionConfirmation.Add(new PA_TransactionConfirmation
                        {
                            created = DateTime.Now,
                            createdby = userId,
                            transactionId = id,
                            ruleType = rulesStage.type,
                            ruleOrder = rulesStage.order,
                            ruleUserId = rulesStage.userId,
                            ruleRoleId = rulesStage.roleId,
                            ruleTitle = rulesStage.title,
                            userId = assingUser
                        });
                    }
                }
                if (getType == typeof(PA_AdvanceConfirmation))
                {
                    return (IEnumerable<T>)advanceConfirmation;
                }
                else if (getType == typeof(PA_TransactionConfirmation))
                {
                    return (IEnumerable<T>)transactionConfirmation;
                }
            }
            else
            {
                return null;
            }



            return  null;
        }



    }

}
