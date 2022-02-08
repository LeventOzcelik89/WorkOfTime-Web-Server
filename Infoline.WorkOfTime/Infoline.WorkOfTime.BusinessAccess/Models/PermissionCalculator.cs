using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Models
{
    public class ConfirmationCalculator
    {
        private WorkOfTimeDatabase db = new WorkOfTimeDatabase();
        private VWSH_User shuser { get; set; } = null;
        private Guid MainId { get; set; }
        private Guid UserId { get; set; }
        private VWINV_CompanyPersonDepartments[] managers { get; set; } = null;
        VWUT_RulesUser rulesUser { get; set; } = new VWUT_RulesUser();
        VWUT_RulesUserStage[] rulesUserStages { get; set; }
        public IEnumerable<PA_AdvanceConfirmation> GetAdvance { get; }
        public IEnumerable<PA_TransactionConfirmation> GetTransaction { get; set; }
        class ConfirmationCalculatorHandler
        {
            public Guid? userId { get; set; }
            public short? status { get; set; }
            public string description { get; set; }
            public short? ruleOrder { get; set; }
            public short? ruleType { get; set; }
            public Guid? ruleUserId { get; set; }
            public Guid? ruleRoleId { get; set; }
            public string ruleTitle { get; set; }
            public DateTime created { get; set; }
            public Guid? createdBy { get; set; }
            public Guid MainId { get; set; }

        }
        public ConfirmationCalculator(Guid userId, Guid MainId, EnumUT_RulesType ruleType)
        {
            this.MainId = MainId;
            this.UserId = userId;
            shuser = db.GetVWSH_UserById(userId);
            if (shuser == null)
            {
                return;
            }
            var getManagers = db.GetINV_CompanyPersonDepartmentsByIdUserAndTypeCurrentWork(userId, (int)EnumINV_CompanyDepartmentsType.Organization);
            if (getManagers == null)
            {
                return;
            }
            this.managers = getManagers;

            var handle = Handle(ruleType);
            this.GetAdvance = handle.Select(x => new PA_AdvanceConfirmation
            {
                advanceId = x.MainId,
                created = x.created,
                createdby = x.createdBy,
                ruleOrder = x.ruleOrder,
                ruleRoleId = x.ruleRoleId,
                ruleTitle = x.ruleTitle,
                ruleType = x.ruleType,
                ruleUserId = x.ruleUserId,
                userId = x.userId
            });
            this.GetTransaction = handle.Select(x => new PA_TransactionConfirmation
            {
                transactionId = x.MainId,
                created = x.created,
                createdby = x.createdBy,
                ruleOrder = x.ruleOrder,
                ruleRoleId = x.ruleRoleId,
                ruleTitle = x.ruleTitle,
                ruleType = x.ruleType,
                ruleUserId = x.ruleUserId,
                userId = x.userId
            });
        }
        private void RuleFinder(EnumUT_RulesType ruleType)
        {
            rulesUser = db.GetVWUT_RulesUserByUserIdAndType(this.UserId, (Int16)ruleType);
            rulesUserStages = new VWUT_RulesUserStage[0];
            if (rulesUser == null)
            {
                var defaultRule = db.GetUT_RulesByTypeIsDefault((Int16)ruleType);
                rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(defaultRule.id);
            }
            else
            {
                rulesUserStages = db.GetVWUT_RulesUserStageByRulesId(rulesUser.rulesId.Value);
            }
        }
        private IEnumerable<ConfirmationCalculatorHandler> Handle(EnumUT_RulesType ruleType)
        {
            RuleFinder(ruleType);
            var list = new List<ConfirmationCalculatorHandler>();
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
                    var isUsedBefore = list.Where(x => x.userId == assingUser).OrderByDescending(x => x.ruleOrder);
                    if (isUsedBefore.Count() > 0)
                    {
                        list.Remove(isUsedBefore.FirstOrDefault());
                    }
                    list.Add(new ConfirmationCalculatorHandler
                    {
                        created = DateTime.Now,
                        createdBy = this.UserId,
                        MainId = this.MainId,
                        ruleType = rulesStage.type,
                        ruleOrder = rulesStage.order,
                        ruleUserId = rulesStage.userId,
                        ruleRoleId = rulesStage.roleId,
                        ruleTitle = rulesStage.title,
                        userId = assingUser
                    });
                }

            }

            return list;

        }
    }

}

