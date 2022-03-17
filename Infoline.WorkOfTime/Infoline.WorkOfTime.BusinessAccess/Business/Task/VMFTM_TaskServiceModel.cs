using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public class FTM_MYTask
    {
        public VWFTM_Task[] AssignedTask { get; set; }
        public VWFTM_Task[] AssignableTask { get; set; }
        public VWFTM_Task[] CompletedTask { get; set; }
    }

    public class INV_Product
    {
        public Guid id { get; set; }
        public Guid? pid { get; set; }
        public string name { get; set; }
        public string productId_Image { get; set; }
        public string productId_Title { get; set; }
        public string qrCode { get; set; }
    }

    public class FTM_TaskRead
    {
        public VWFTM_Task[] AssignedTask { get; set; }
        public VWFTM_Task[] AssignableTask { get; set; }
        public VMFTM_TaskAndCompanyInfo NewTask { get; set; }
    }

    public class FTM_TaskReadCustomer
    {
        public VWFTM_Task[] CompleteTask { get; set; }
        public VWFTM_Task[] PendingTask { get; set; }
        public VWFTM_Task NewTask { get; set; }
    }

    public class VMFTM_Task : VWFTM_Task
    {
        public VMFTM_TaskUserInfo assignUserInfo { get; set; }
        public VMFTM_TaskUserInfo[] assignableUsersInfo { get; set; }
        public VMFTM_TaskUserInfo[] helperUsersInfo { get; set; }
        public List<VWFTM_TaskOperationOperation> taskOperation { get; set; }
        public CMP_Storage TaskStorage { get; set; }
        public VWPRD_Inventory fixtureModel { get; set; }
        public List<VWFTM_TaskSubjectModel> FTM_TaskSubjects { get; set; }
        public Guid[] FTM_TaskSubjectTypeIds { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string adress { get; set; }
        public string companyDescription { get; set; }
        public Guid companyCarStorageId { get; set; }
        public string companyCarStorage_Title { get; set; }
        public string project_Title { get; set; }
        public bool isTaskRule { get; set; }
        public VWCMP_Request Request { get; set; }
        public VWCMP_InvoiceItemReport[] CMP_InvoiceItemReports { get; set; }
        public VMFTM_TaskUserInfo[] taskFollowUpUsers { get; set; }
		public VWFTM_Task[] subTasks { get; set; }

	}


    public class VWFTM_TaskOperationOperation : VWFTM_TaskOperation
    {
        public List<VWCMP_InvoiceItemReport> invoiceItems { get; set; }
    }

    public class VMFTM_TaskUserInfo
    {
        public Guid id { get; set; }
        public string photo { get; set; }
        public string name { get; set; }
    }
    public class VWFTMTaskAndFiles : VWFTM_Task
    {
        public string[] files { get; set; }
        public string[] taskFiles { get; set; }

    }
    public class VWFTM_TaskSubjectModel
    {
        public string name { get; set; }
        public Guid id { get; set; }
    }

    public class VMFTM_TaskServiceModel
    {
        public WorkOfTimeDatabase _db = new WorkOfTimeDatabase();
        private FTM_Task _task { get; set; }
        private List<FTM_TaskOperation> _taskOperation { get; set; } = new List<FTM_TaskOperation>();
        private List<FTM_TaskFormResult> _taskFormResult { get; set; } = new List<FTM_TaskFormResult>();
        public PRJ_Project project { get; set; }

        public VMFTM_TaskServiceModel()
        {

        }
        public VMFTM_TaskServiceModel(Guid taskId)
        {
            _task = _db.GetFTM_TaskById(taskId);
            _taskOperation = _db.GetFTM_TaskOperationByTaskId(taskId).ToList();
            _taskFormResult = _db.GetFTM_TaskFormResultByTaskId(taskId).ToList();
        }
        public VMFTM_Task Get(Guid taskId)
        {
            var data = _db.GetVWFTM_TaskById(taskId);
            var operations = _db.GetVWFTM_TaskOperationByTaskId(taskId).ToList();
            var userDetail = new VWSH_User();
            if (data.assignUserId.HasValue)
            {
                userDetail = _db.GetVWSH_UserById(data.assignUserId.Value);
            }
            var car = new VWCMP_CompanyCars();
            var companyCarStorage = new VWCMP_Storage();
            if (data.companyCarId.HasValue)
            {
                car = _db.GetVWCMP_CompanyCarsById(data.companyCarId.Value);
            }

            var invoiceItems = _db.GetVWCMP_InvoiceItemReportByInvoiceIds(operations.Where(a => a.dataId.HasValue).Select(a => a.dataId.Value).ToArray());

            var model = new VMFTM_Task
            {
            }.B_EntityDataCopyForMaterial(data);


            model.taskOperation = operations.Select(a => new VWFTM_TaskOperationOperation
            {
                invoiceItems = invoiceItems.Where(b => b.invoiceId == a.dataId).ToList(),
                dataId = a.dataId,
                battery = a.battery,
                changed = a.changed,
                changedby = a.changedby,
                changedby_Title = a.changedby_Title,
                created = a.created,
                createdby = a.createdby,
                createdby_Title = a.createdby_Title,
                description = a.description,
                distance = a.distance,
                fixtureId = a.fixtureId,
                fixture_Title = a.fixture_Title,
                formId = a.formId,
                formResultId = a.formResultId,
                id = a.id,
                user_Title = a.user_Title,
                jsonResult = a.jsonResult,
                location = a.location,
                passingTime = a.passingTime,
                status = a.status,
                status_Title = a.status_Title,
                subject = a.subject,
                subject_Title = a.subject_Title,
                taskId = a.taskId,
                task_Name = a.task_Name,
                userId = a.userId,
                createdPhoto = a.createdPhoto
            }).OrderByDescending(a => a.created).ThenByDescending(a => a.status).ToList();

            if (model.customerStorageId.HasValue)
            {
                var storage = _db.GetCMP_StorageById(model.customerStorageId.Value);

                model.TaskStorage = storage;
            }

            model.subTasks = _db.GetVWFTM_TaskByPid(data.id);

            model.FTM_TaskSubjectTypeIds = _db.GetFTM_TaskSubjectTypeByTaskIdTypesIds(taskId);

            var FTM_TaskSubjectsData = _db.GetVWFTM_TaskSubjectByTaskId(taskId).ToList();

            if (FTM_TaskSubjectsData != null && FTM_TaskSubjectsData.Count() > 0)
            {
                model.FTM_TaskSubjects = FTM_TaskSubjectsData.Select(x => new VWFTM_TaskSubjectModel
                {
                    id = x.subjectId.Value,
                    name = x.subjectId_Title
                }).ToList();
            }
            if (car != null && car.plate != null && car.companyStorageId.HasValue)
            {
                model.companyCarStorageId = car.companyStorageId.Value;
                model.companyCarStorage_Title = car.plate;
            }

            if (data.assignableUserIds != null)
            {
                var users = _db.GetVWSH_UserByIds(data.assignableUserIds.Split(',').Select(a => Guid.Parse(a)).ToArray());

                model.assignUserInfo = users.Where(a => a.id == model.assignUserId).Select(a => new VMFTM_TaskUserInfo
                {
                    id = a.id,
                    name = a.FullName,
                    photo = a.ProfilePhoto
                }).FirstOrDefault();

                model.assignableUsersInfo = users.Select(a => new VMFTM_TaskUserInfo
                {
                    id = a.id,
                    name = a.FullName,
                    photo = a.ProfilePhoto
                }).ToArray();
            }
            else
            {
                model.assignableUsersInfo = new VMFTM_TaskUserInfo[0];
            }

            if (!string.IsNullOrEmpty(data.helperUserIds))
            {
                var users = _db.GetVWSH_UserByIds(data.helperUserIds.Split(',').Select(a => Guid.Parse(a)).ToArray());
                model.helperUsersInfo = users.Select(a => new VMFTM_TaskUserInfo
                {
                    id = a.id,
                    name = a.FullName,
                    photo = a.ProfilePhoto
                }).ToArray();
            }
            else
            {
                model.helperUsersInfo = new VMFTM_TaskUserInfo[0];
            }

            if (model.companyId.HasValue)
            {
                var project = _db.GetPRJ_ProjectByCompanyIdIsActive(model.companyId.Value);

                if (project != null)
                {
                    model.projectId = project.id;
                    model.project_Title = project.ProjectName;
                }
            }


            model.Request = _db.GetVWCMP_RequestByTaskId(model.id);
            model.CMP_InvoiceItemReports = _db.GetVWCMP_InvoiceItemReportByTaskId(model.id);

            if (model.assignUserId.HasValue)
            {
                var rulesUser = _db.GetVWUT_RulesUserByUserIdAndType(model.assignUserId.Value, (Int16)EnumUT_RulesType.Task);

                if (rulesUser != null && model.lastOperationStatus >= (int)EnumFTM_TaskOperationStatus.GorevBaslandi)
                {
                    model.isTaskRule = true;
                }
            }


            if (model.fixtureId.HasValue)
            {
                model.fixtureModel = _db.GetVWPRD_InventoryById(model.fixtureId.Value);
            }

            if (model.customerId.HasValue)
            {
                var cmp = GetCMP_CompanyInformation(model.customerId.Value);
                if (cmp != null)
                {
                    model.adress = cmp.openAddress;
                    model.phone = cmp.phone;
                    model.email = cmp.email;
                    model.companyDescription = cmp.description;
                }
            }

            if (data.assignUserId.HasValue)
            {
                model.company_Title = userDetail.Company_Title ?? null;
                model.companyId = userDetail.CompanyId ?? null;
            }

            var followUpUsers = _db.GetVWFTM_TaskFollowUpUserByTaskId(taskId);
            model.taskFollowUpUsers = followUpUsers.Select(x => new VMFTM_TaskUserInfo
            {
                id = x.userId.Value,
                name = x.userId_Title
            }).ToArray();

            return model;
        }
        public INV_Product[] GetProducts()
        {
            var list = new List<INV_Product>();
            if (_task.fixtureId.HasValue)
            {
                var product = _db.GetVWPRD_InventoryById(_task.fixtureId.Value);

                if (product != null)
                {
                    list.Add(new INV_Product
                    {
                        id = product.id,
                        pid = null,
                        productId_Title = product.productId_Title,
                        productId_Image = product.productId_Image,
                        name = product.fullName,
                        qrCode = product.code,
                    });
                }
                else
                {
                    list = GetProductByCustomerId(_task.customerId.Value).ToList();
                }
            }
            return list.ToArray();
        }
        public INV_Product[] GetProductByCustomerId(Guid customerId)
        {
            var list = _db.GetVWPRD_InventoryByCompanyId(customerId)
                .Select(inventory => new INV_Product
                {
                    id = inventory.id,
                    pid = null,
                    productId_Title = inventory.productId_Title,
                    productId_Image = inventory.productId_Image,
                    name = inventory.fullName,
                    qrCode = inventory.code,
                }).ToList();
            return list.ToArray();
        }
        public FTM_MYTask GetMyTask(Guid userId)
        {
            var userDetail = _db.GetVWSH_UserById(userId);
            var tasks = _db.GetVWFTM_TaskByUserId(userId);
            var result = new FTM_MYTask
            {
                AssignedTask = tasks.Where(a => a.assignUserId == userId && a.isComplete == false).OrderByDescending(a => a.created).ToArray(),
                AssignableTask = tasks.Where(a => a.assignUserId == null && a.isComplete == false && (a.assignableUserIds != null && a.assignableUserIds.Contains(userId.ToString().ToUpper()))).OrderByDescending(a => a.created).ToArray(),
                CompletedTask = tasks.Where(a => a.assignUserId == userId && a.isComplete == true).OrderByDescending(a => a.created).ToArray(),
            };
            return result;
        }

        public FTM_TaskRead GetFTM_TaskRead(string QRCode, Guid userId)
        {
            var user = _db.GetVWSH_UserById(userId);
            var task = new VMFTM_TaskAndCompanyInfo
            {
                code = BusinessExtensions.B_GetIdCode(),
                createdby = userId,
                created = DateTime.Now,
                hasVerifyCode = false,
                companyId = user?.CompanyId,
                company_Title = user?.Company_Title
            };

            var otherTasks = new List<VWFTM_Task>();

            if (!string.IsNullOrEmpty(QRCode))
            {
                var inventory = _db.GetVWPRD_InventoryByCode(QRCode);
                if (inventory != null)
                {
                    task.fixtureId = inventory.id;
                    task.fixture_Title = inventory.fullName;
                    task.location = inventory.location;
                    ///TODO:Şahin Burada işletme setleme İlerleyen safalarda düzeltilecek
                    switch ((EnumPRD_InventoryActionType)inventory.lastActionType)
                    {
                        case EnumPRD_InventoryActionType.Depoda:

                            if (inventory.firstActionDataId.HasValue && TenantConfig.Tenant.TenantCode == 1169)
                            {

                                var cmpany = _db.GetVWCMP_CompanyById(inventory.firstActionDataId.Value);
                                if (cmpany != null)
                                {
                                    var cc = GetCMP_CompanyInformation(cmpany.id);
                                    if (cc != null)
                                    {
                                        task.customerId = cc.id;
                                        task.customer_Title = cc.fullName;
                                        task.adress = cc.openAddress;
                                        task.phone = cc.phone;
                                        task.email = cc.email;
                                    }
                                }
                            }
                            else if (inventory.lastActionDataId.HasValue)
                            {
                                var storage = _db.GetVWCMP_StorageById(inventory.lastActionDataId.Value);
                                if (storage != null)
                                {
                                    task.customerId = storage.companyId;
                                    task.customerStorageId = storage.id;
                                    task.customerStorage_Title = storage.fullName;
                                    task.customer_Title = storage.companyId_Title;
                                }
                            }
                            break;
                        case EnumPRD_InventoryActionType.Personelde:
                            var person = _db.GetVWSH_UserById(inventory.lastActionDataId.Value);
                            var pp = _db.GetVWCMP_CompanyById(person.CompanyId.Value);
                            if (pp != null)
                            {
                                task.customerId = pp.id;
                                task.customer_Title = pp.fullName;
                                task.adress = pp.openAddress;
                                task.phone = pp.phone;
                                task.email = pp.email;
                            }
                            break;
                        case EnumPRD_InventoryActionType.KirayaVerildi:
                        case EnumPRD_InventoryActionType.CikisYapildi:
                            var ss = _db.GetVWCMP_CompanyById(inventory.lastActionDataId.Value);
                            if (ss != null)
                            {
                                task.customerId = ss.id;
                                task.customer_Title = ss.fullName;
                                task.adress = ss.openAddress;
                                task.phone = ss.phone;
                                task.email = ss.email;
                            }
                            break;
                    }
                    otherTasks = _db.GetVWFTM_TaskByFixtureId(inventory.id, userId).ToList();
                }
                else
                {
                    var storage = _db.GetVWCMP_StorageByCode(QRCode);
                    if (storage != null)
                    {
                        task.customerId = storage.companyId;
                        task.customer_Title = storage.companyId_Title;
                        task.customerStorageId = storage.id;
                        task.customerStorage_Title = storage.fullName;
                        task.adress = storage.address;
                        task.phone = storage.phone;
                        otherTasks = _db.GetVWFTM_TaskByCustomerStorageId(storage.id, userId).ToList();
                    }
                    else
                    {
                        var customer = _db.GetVWCMP_CompanyByCode(QRCode);
                        if (customer != null)
                        {
                            task.customerId = customer.id;
                            task.customer_Title = customer.fullName;
                            task.email = customer.email;
                            task.adress = customer.openAddress;
                            task.phone = customer.phone;
                            otherTasks = _db.GetVWFTM_TaskByCustomerId(customer.id, userId).ToList();
                        }
                    }

                }
            }

            return new FTM_TaskRead
            {
                NewTask = task,
                AssignedTask = otherTasks.Where(a => a.isComplete == false && a.assignUserId == userId).OrderByDescending(a => a.created).ToArray(),
                AssignableTask = otherTasks.Where(a => a.isComplete == false && a.assignUserId == null && ((a.assignableUserIds == null && a.companyId == user.CompanyId) || (a.assignableUserIds != null && a.assignableUserIds.Contains(userId.ToString().ToUpper())))).OrderByDescending(a => a.created).ToArray()
            };
        }
        public List<VWFTMTaskAndFiles> GetFTM_TaskReadCustomer(string QRCode, Guid userId)
        {
            var user = _db.GetVWSH_UserById(userId);
            var tasks = new List<VWFTM_Task>();
            if (user != null)
            {
                if (user.CompanyId.HasValue)
                {
                    tasks = _db.GetVWFTM_TaskByCustomerId(user.CompanyId.Value).ToList();
                }
            }
            if (!string.IsNullOrEmpty(QRCode))
            {
                var inventory = _db.GetPRD_InventoryByCode(QRCode) ?? new PRD_Inventory();
                tasks = tasks.Where(a => a.fixtureId == inventory.id).ToList();
            }

            var res = new List<VWFTMTaskAndFiles>();
            foreach (var task in tasks)
            {
                var operations = _db.GetFTM_TaskOperationByTaskId(task.id).OrderBy(x => x.created).ToArray();
                var files = new string[] { };
                var taskFiles = new string[] { };

                if (operations.Count() > 0)
                {
                    var operationFirst = operations.FirstOrDefault();
                    if (operationFirst != null)
                    {
                        var sysFiles = _db.GetSYS_FilesByDataIdAll(operationFirst.id);
                        if (sysFiles.Count() > 0)
                        {
                            files = sysFiles.Select(x => x.FilePath).ToArray();
                        }
                    }

                    taskFiles = _db.GetSYS_FilesInDataId(operations.Select(x => x.id).ToArray()).Select(x => x.FilePath).ToArray();
                }

                var model = new VWFTMTaskAndFiles
                {
                    files = files,
                    taskFiles = taskFiles
                }.B_EntityDataCopyForMaterial(task);

                res.Add(model);
            }

            return res.OrderByDescending(x => x.lastOperationDate).ToList();
        }

        public VWCMP_Company GetCMP_CompanyInformation(Guid? id)
        {
            if (!id.HasValue)
            {
                return new VWCMP_Company { };
            }

            var data = _db.GetVWCMP_CompanyById(id.Value);

            if (data == null)
            {
                return new VWCMP_Company { };
            }

            return data;
        }

        public SummaryHeadersTaskNew GetTaskSummary(Guid userId, bool showHeaderFilter = true)
        {

            var roles = _db.GetSH_UserRoleByUserId(userId);

            var userRoles = roles.Where(a => a.roleid == new Guid(SHRoles.BayiGorevPersoneli)).ToArray();
            if (userRoles.Count() > 0)
            {
                return _db.GetVWPA_TaskMyRequestsCountFilter(userId, showHeaderFilter);
            }


            userRoles = roles.Where(a => a.roleid == new Guid(SHRoles.SahaGorevOperator) || a.roleid == new Guid(SHRoles.SahaGorevYonetici) || a.roleid == new Guid(SHRoles.SistemYonetici)).ToArray();
            if (userRoles.Count() > 0)
            {
                return _db.GetVWPA_TaskAllRequestsCountFilter(userId, showHeaderFilter);
            }

            var user = _db.GetVWSH_UserById(userId);
            userRoles = roles.Where(a => a.roleid == new Guid(SHRoles.SahaGorevMusteri)).ToArray();
            if (userRoles.Count() > 0)
            {
                if (user.CompanyId == null)
                {
                    return new SummaryHeadersTaskNew();
                }

                return _db.GetVWPA_TaskCustomerRequestsCountFilter(new Guid(user.CompanyId.ToString()), showHeaderFilter);
            }

            userRoles = roles.Where(a => a.roleid == new Guid(SHRoles.YukleniciPersoneli)).ToArray();
            if (userRoles.Count() > 0)
            {
                return _db.GetVWPA_TaskYukleniciRequestsCountFilter(userId, new Guid(user.CompanyId.ToString()), showHeaderFilter);
            }

            return _db.GetVWPA_TaskMyRequestsCountFilter(userId, showHeaderFilter);
        }
    }

    public class VMFTM_TaskAndCompanyInfo : VWFTM_Task
    {
        public string adress { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
    }
}
