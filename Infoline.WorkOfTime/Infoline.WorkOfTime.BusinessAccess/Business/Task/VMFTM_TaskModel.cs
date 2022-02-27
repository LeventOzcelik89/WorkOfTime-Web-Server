using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Infoline.Helper;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web;
namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMFTM_TaskModel : VWFTM_Task
    {
        public WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public List<VWFTM_TaskOperation> taskOperations { get; set; }
        public List<VWFTM_TaskUser> taskUsers { get; set; }
        public List<Guid> assignableUsers { get; set; }
        public List<VWFTM_TaskUserHelper> taskUsersHelper { get; set; }
        public List<FTM_TaskFollowUpUser> taskFollowUpUsers { get; set; } = new List<FTM_TaskFollowUpUser>();
        public List<Guid> helperUsers { get; set; }
        public List<Guid> followUpUsers { get; set; }
        public VWCMP_Company company { get; set; } = new VWCMP_Company();
        public VWCMP_Company customer { get; set; } = new VWCMP_Company();
        public VWCMP_Storage customerStorage { get; set; } = new VWCMP_Storage();
        public bool sendMail { get; set; }
        public List<Guid?> personUserIds { get; set; }
        public DateTime? taskStartDate { get; set; }
        public DateTime? taskEndDate { get; set; }
        public Guid? companyStorageId { get; set; }
        public CMP_CompanyCars companyCar { get; set; }
        public List<VWPA_Transaction> transactions { get; set; } = new List<VWPA_Transaction>();
        public string group_Title { get; set; }
        public string userMails { get; set; }
        public string[] files { get; set; }
        public static TimeSpan VerifyCodeDueDate = new TimeSpan(999, 30, 0);
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //FTN_TaskProject
        public List<Guid> corporationIds { get; set; }
        public List<Guid> taskIds { get; set; }
        public List<Guid> projectIds { get; set; }
        public Guid[] FTM_TaskSubjectTypeIds { get; set; }
        public short? isSendDocuments { get; set; }
        public bool isTaskRule { get; set; }
        public string operationStartTime { get; set; }
        public string TypeTitle(short? key)
        {
            var enumTypeArray = EnumsProperties.EnumToArrayGeneric<EnumFTM_TaskType>().ToArray();
            var type = enumTypeArray.Where(a => a.Key == key.ToString()).FirstOrDefault();
            var type_Title = type != null ? type.Value : "";
            return type_Title;
        }
        public VMFTM_TaskModel Load(Guid? userId = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var task = db.GetVWFTM_TaskById(this.id);
            if (task != null)
            {
                this.B_EntityDataCopyForMaterial(task, true);
                taskUsers = db.GetVWFTM_TaskUserByTaskId(this.id).ToList();
                taskUsersHelper = db.GetVWFTM_TaskUserHelperByTaskId(this.id).ToList();
                taskFollowUpUsers = db.GetFTM_TaskFollowUpUserByTaskId(this.id).ToList();
                taskOperations = db.GetVWFTM_TaskOperationByTaskId(this.id).OrderByDescending(a => a.created).ToList();
                assignableUsers = taskUsers.Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToList();
                helperUsers = taskUsersHelper.Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToList();
                followUpUsers = taskFollowUpUsers.Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToList();

                if (task.assignUserId.HasValue)
                {
                    var rulesUser = db.GetVWUT_RulesUserByUserIdAndType(task.assignUserId.Value, (Int16)EnumUT_RulesType.Task);

                    if (rulesUser != null && this.lastOperationStatus >= (int)EnumFTM_TaskOperationStatus.GorevBaslandi)
                    {
                        isTaskRule = true;
                    }
                }

                if (task.assignUserId.HasValue)
                {
                    var group = db.GetVWSH_GroupUserByUserId(task.assignUserId.Value);

                    this.group_Title = "-";
                    if (group != null)
                    {
                        this.group_Title = group.groupId_Title;
                    }
                }

                this.transactions = db.GetVWPA_TransactionByDataId(task.id).ToList();

                if (this.companyCarId.HasValue)
                {
                    var companyCar = db.GetCMP_CompanyCarsById(this.companyCarId.Value);
                    if (companyCar != null)
                    {
                        this.companyStorageId = companyCar.companyStorageId;
                        this.companyCar = companyCar;
                    }
                }
                if (taskOperations.Count() > 0)
                {
                    var firstOperation = taskOperations.Where(a => a.status == (Int32)EnumFTM_TaskOperationStatus.GorevBaslandi && a.created.HasValue && a.status >= (int)EnumFTM_TaskOperationStatus.GorevBaslandi).FirstOrDefault();
                    var lastOperation = taskOperations.Where(a => a.status == (Int32)EnumFTM_TaskOperationStatus.CozumBildirildi && a.created.HasValue && a.status >= (int)EnumFTM_TaskOperationStatus.GorevBaslandi).OrderBy(x => x.created).LastOrDefault();
                    if (firstOperation != null && firstOperation.created.HasValue)
                    {
                        taskStartDate = firstOperation.created.Value;
                    }
                    if (lastOperation != null && lastOperation.created.HasValue)
                    {
                        taskEndDate = lastOperation.created.Value;
                    }
                    //else
                    //{
                    //                   taskEndDate = taskStartDate.AddHours(1);
                    //}
                    files = db.GetSYS_FilesInDataId(taskOperations.Select(x => x.id).ToArray()).Select(x => x.FilePath).ToArray();
                }
                this.FTM_TaskSubjectTypeIds = db.GetFTM_TaskSubjectTypeByTaskIdTypesIds(this.id);
            }
            else
            {
                taskUsers = new List<VWFTM_TaskUser>();
                taskUsersHelper = new List<VWFTM_TaskUserHelper>();
                taskOperations = new List<VWFTM_TaskOperation>();
            }
            this.code = this.code ?? BusinessExtensions.B_GetIdCode();
            this.hasVerifyCode = this.hasVerifyCode ?? false;
            this.type = this.type ?? (int)EnumFTM_TaskType.Ariza;
            this.priority = this.priority ?? (int)EnumFTM_TaskPriority.Orta;
            this.dueDate = this.dueDate ?? DateTime.Now.AddDays(1);
            this.created = this.created ?? DateTime.Now;
            this.createdby = this.createdby ?? userId;
            this.assignableUsers = this.assignableUsers ?? new List<Guid>();
            this.helperUsers = this.helperUsers ?? new List<Guid>();
            this.followUpUsers = this.followUpUsers ?? new List<Guid>();
            this.personUserIds = db.GetSH_UserByRoleIdList(new Guid(SHRoles.SahaGorevPersonel));
            this.personUserIds.AddRange(db.GetSH_UserByRoleIdList(new Guid(SHRoles.YukleniciPersoneli)));
            this.planLater = this.planLater.HasValue ? this.planLater.Value : (short)EnumFTM_TaskPlanLater.Hayir;
            this.isSendDocuments = this.isSendDocuments.HasValue ? this.isSendDocuments.Value : (short)EnumFTM_TaskPersonIsSendDocuments.Hayır;
            if (this.personUserIds.Count() > 0)
            {
                this.personUserIds.AddRange(db.GetSH_UserByRoleIdList(new Guid(SHRoles.BayiGorevPersoneli)));
            }
            if (this.companyId.HasValue)
            {
                company = db.GetVWCMP_CompanyById(this.companyId.Value);
            }
            if (this.customerId.HasValue)
            {
                customer = db.GetVWCMP_CompanyById(this.customerId.Value);
            }
            if (this.customerStorageId.HasValue)
            {
                customerStorage = db.GetVWCMP_StorageById(this.customerStorageId.Value);
            }
            this.location = this.location ?? this.customerStorage?.location ?? this.customer?.location;
            return this;
        }
        public ResultStatus InsertAll(Guid? userId, HttpRequestBase requestFiles = null, DbTransaction _trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans;
            this.Load(userId);
            var users = new VWSH_User[0];
            var now = DateTime.Now;
            var task = db.GetFTM_TaskById(this.id);
            if (task != null)
            {
                return new ResultStatus { result = false, message = "Görev zaten oluşturulmuş." };
            }
            if (this.location == null && (this.type == (Int32)EnumFTM_TaskType.Kesif || this.type == (Int32)EnumFTM_TaskType.GelAL || this.type == (Int32)EnumFTM_TaskType.Diger))
            {
                return new ResultStatus { result = false, message = "Bu görev tipinde konum seçmek zorunludur." };
            }
            if (!this.customerId.HasValue)
            {
                return new ResultStatus { result = false, message = "Müşteri işletme zorunludur." };
            }
            var inventory = new VWPRD_Inventory();
            if (this.fixtureId != null)
            {
                this.SetFixtureInfo(this.fixtureId.Value);
            }
            if (this.assignableUsers.Count() > 0)
            {
                users = db.GetVWSH_UserByIds(this.assignableUsers.ToArray());
            }
            var createOperation = new VWFTM_TaskOperation
            {
                createdby = userId,
                created = new DateTime(now.Ticks),
                location = this.lastOperationLocation,
                taskId = this.id,
                status = (int)EnumFTM_TaskOperationStatus.GorevOlusturuldu,
                description = this.description,
            };
            taskOperations.Add(createOperation);
            if (this.assignableUsers.Count() > 0)
            {
                taskOperations.Add(new VWFTM_TaskOperation
                {
                    createdby = userId,
                    created = new DateTime(now.Ticks).AddSeconds(1),
                    location = this.lastOperationLocation,
                    taskId = this.id,
                    status = (int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi,
                    description = string.Join(",", users.Select(a => a.FullName)) + " kullanıcılarına atama yapıldı."
                });
                taskUsers = assignableUsers.Select(a => new VWFTM_TaskUser
                {
                    createdby = userId,
                    created = (DateTime.Now),
                    userId = a,
                    taskId = this.id,
                    verifyCode = this.hasVerifyCode == true ? RandomString(8) : null,
                    status = false,
                }).ToList();
            }
            if (this.helperUsers.Count() > 0)
            {
                taskUsersHelper = helperUsers.Select(a => new VWFTM_TaskUserHelper
                {
                    createdby = userId,
                    created = (DateTime.Now),
                    userId = a,
                    taskId = this.id,
                }).ToList();
            }
            if (this.hasVerifyCode == true)
            {
                taskOperations.Add(new VWFTM_TaskOperation
                {
                    createdby = userId,
                    created = new DateTime(now.Ticks).AddSeconds(2),
                    location = this.lastOperationLocation,
                    taskId = this.id,
                    status = (int)EnumFTM_TaskOperationStatus.DogrulamaKoduGonderildi,
                    description = string.Join(",", users.Select(a => a.FullName)) + " kullanıcılarına doğrulama kodu gönderildi."
                });
            }
            if (this.followUpUsers.Count() > 0)
            {
                taskFollowUpUsers = this.followUpUsers.Select(x => new FTM_TaskFollowUpUser
                {
                    created = DateTime.Now,
                    createdby = userId,
                    userId = x,
                    taskId = this.id
                }).ToList();
            }
            var rs = new ResultStatus { result = true };
            rs = db.InsertFTM_Task(new FTM_Task().B_EntityDataCopyForMaterial(this), this.trans);
            rs &= db.BulkInsertFTM_TaskOperation(taskOperations.Select(a => new FTM_TaskOperation().B_EntityDataCopyForMaterial(a, true)), this.trans);
            rs &= db.BulkInsertFTM_TaskUser(taskUsers.Select(a => new FTM_TaskUser().B_EntityDataCopyForMaterial(a, true)), this.trans);
            rs &= db.BulkInsertFTM_TaskUserHelper(taskUsersHelper.Select(a => new FTM_TaskUserHelper().B_EntityDataCopyForMaterial(a, true)), this.trans);
            rs &= db.BulkInsertFTM_TaskFollowUpUser(taskFollowUpUsers, this.trans);
            if (this.FTM_TaskSubjectTypeIds != null && this.FTM_TaskSubjectTypeIds.Count() > 0)
            {
                var taskTypeKeyList = this.FTM_TaskSubjectTypeIds.Select(x => new FTM_TaskSubjectType
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    subjectId = x,
                    taskId = this.id,
                }).ToList();
                rs &= db.BulkInsertFTM_TaskSubjectType(taskTypeKeyList, trans);
            }
            if (rs.result)
            {
                foreach (var assignableUser in assignableUsers)
                {
                    new Notification().NotificationSend(assignableUser, this.createdby, "Görev Ataması", "Tarafınıza görev ataması yapıldı.", $"/FTM/VWFTM_Task/Detail?id={this.id}");
                }
                if (requestFiles != null)
                {
                    new FileUploadSave(requestFiles, createOperation.id).SaveAs();
                }
                rs.message = "Görev başarıyla oluşturuldu.";
            }
            else
            {
                rs.message = "Görev oluşturulamadı.";
            }
            var type_Title = TypeTitle(this.type);
            var notification = new Notification();
            if ((this.sendMail == true || this.hasVerifyCode == true) && rs.result == true)
            {
                foreach (var item in taskUsers)
                {
                    var user = users.Where(a => a.id == item.userId).FirstOrDefault();
                    var text = "<h3>Sayın " + user.FullName + "</h3>";
                    text += "<div>Tarafınıza <strong>" + this.code + "</strong> kodlu görev oluşturulmuştur.</div>";
                    var notify = string.Format("Sayın " + user.FullName + ", tarafınıza " + this.code + " kodlu görev oluşturulumuştur.");
                    if (!string.IsNullOrEmpty(type_Title))
                    {
                        text += "<div>Görev Tipi : <strong>" + type_Title + "</strong></div>";
                    }
                    text += "<div>Müşteri İşletme : <strong>" + customer.name + "</strong></div>";
                    if (this.fixtureId.HasValue)
                    {
                        text += "<div>Envanter : <strong>" + this.fixture_Title + "</strong></div>";
                    }
                    if (this.description != null)
                    {
                        text += "<div>Açıklama : <strong>" + this.description + "</strong></div>";
                    }
                    text += "<div>Görev Oluşturulma Tarihi : <strong>" + this.created + "</strong></div>";
                    text += (this.hasVerifyCode == true ? "<div>Görev doğrulama Kodu : <strong>" + item.verifyCode + "</strong></div>" : "");
                    text += "<div>Görev detaylarını görüntülemek için <a href='" + TenantConfig.Tenant.GetWebUrl() + "/FTM/VWFTM_Task/Detail/" + this.id + "'>tıklayınız.</a> </div>";
                    text += "<div>Bilgilerinize.</div>";
                    new Email().Template("Template1", "gorevMailFoto.jpg", TenantConfig.Tenant.TenantName + " | Görev Yönetimi", text).Send((Int16)EmailSendTypes.Operasyon, user.email, "Saha Görevi Oluşturuldu ", true);
                    notification.NotificationSend(user.id, this.createdby, "Görev Yönetimi", notify, $"/FTM/VWFTM_Task/Detail?id={this.id}");
                }
            }
            if (rs.result)
            {
                CustomerTaskMailSend();
                if (taskFollowUpUsers.Count() > 0)
                {
                    var followshusers = db.GetVWSH_UserByIds(taskFollowUpUsers.Select(x => x.userId.Value).ToArray());
                    foreach (var followUpUser in taskFollowUpUsers)
                    {
                        var user = followshusers.Where(a => a.id == followUpUser.userId).FirstOrDefault();
                        var notify = string.Format("Sayın " + user.FullName + ", tarafınıza " + this.code + " kodlu görev oluşturulumuştur.");
                        var text = "<h3>Sayın " + user.FullName + "</h3>";
                        text += "<div><strong>" + this.code + "</strong> kodlu görev oluşturulmuştur.</div>";
                        if (!string.IsNullOrEmpty(type_Title))
                        {
                            text += "<div>Görev Tipi : <strong>" + type_Title + "</strong></div>";
                        }
                        text += "<div>Müşteri İşletme : <strong>" + customer.name + "</strong></div>";
                        if (this.fixtureId.HasValue)
                        {
                            text += "<div>Envanter : <strong>" + this.fixture_Title + "</strong></div>";
                        }
                        if (this.description != null)
                        {
                            text += "<div>Açıklama : <strong>" + this.description + "</strong></div>";
                        }
                        text += "<div>Görev Oluşturulma Tarihi : <strong>" + this.created + "</strong></div>";
                        text += "<div>Görev detaylarını görüntülemek için <a href='" + TenantConfig.Tenant.GetWebUrl() + "/FTM/VWFTM_Task/Detail/" + this.id + "'>tıklayınız.</a> </div>";
                        text += "<div>Bilgilerinize.</div>";
                        new Email().Template("Template1", "gorevMailFoto.jpg", TenantConfig.Tenant.TenantName + " | Görev Yönetimi", text).Send((Int16)EmailSendTypes.Operasyon, user.email, "Saha Görevi Oluşturuldu ", true);
                        notification.NotificationSend(user.id, this.createdby, "Görev Yönetimi", notify, $"/FTM/VWFTM_Task/Detail?id={this.id}");
                    }
                }
            }
            return rs;
        }
        public ResultStatus InsertMy(Guid? userId, HttpRequestBase requestFiles = null, DbTransaction _trans = null)
        {
            this.Load(userId);
            this.trans = _trans ?? this.db.BeginTransaction();
            var now = DateTime.Now;
            var user = db.GetVWSH_UserById(userId.Value);
            var _task = db.GetFTM_TaskById(this.id);
            if (_task != null)
                return new ResultStatus { result = false, message = "Görev zaten oluşturulmuş." };
            if (this.location == null && (this.type == (Int32)EnumFTM_TaskType.Kesif || this.type == (Int32)EnumFTM_TaskType.GelAL || this.type == (Int32)EnumFTM_TaskType.Diger))
                return new ResultStatus { result = false, message = "Bu görev tipinde konum seçmek zorunludur." };
            if (this.fixtureId != null)
            {
                this.SetFixtureInfo(this.fixtureId.Value);
            }
            var createOperation = new VWFTM_TaskOperation
            {
                createdby = userId,
                created = new DateTime(now.Ticks),
                location = this.lastOperationLocation,
                taskId = this.id,
                status = (int)EnumFTM_TaskOperationStatus.GorevOlusturuldu,
                description = this.description,
            };
            taskOperations.Add(createOperation);
            taskOperations.Add(new VWFTM_TaskOperation
            {
                createdby = userId,
                created = new DateTime(now.Ticks).AddSeconds(2),
                location = this.lastOperationLocation,
                taskId = this.id,
                status = (int)EnumFTM_TaskOperationStatus.GorevUstlenildi,
                description = user.FullName + " görevi üstlendi."
            });
            taskUsers.Add(new VWFTM_TaskUser
            {
                createdby = userId,
                created = (DateTime.Now),
                userId = userId,
                taskId = this.id,
                status = true,
                verifyCode = null,
            });
            if (this.helperUsers.Count() > 0)
            {
                taskUsersHelper = helperUsers.Select(a => new VWFTM_TaskUserHelper
                {
                    createdby = userId,
                    created = (DateTime.Now),
                    userId = a,
                    taskId = this.id,
                }).ToList();
            }

            if (!this.companyId.HasValue)
            {
                this.companyId = user.CompanyId;
                this.company_Title = user.Company_Title;
            }


            var rs = new ResultStatus { result = true };
            rs &= db.InsertFTM_Task(new FTM_Task().B_EntityDataCopyForMaterial(this), this.trans);
            rs &= db.BulkInsertFTM_TaskOperation(taskOperations.Select(a => new FTM_TaskOperation().B_EntityDataCopyForMaterial(a, true)), this.trans);
            rs &= db.BulkInsertFTM_TaskUser(taskUsers.Select(a => new FTM_TaskUser().B_EntityDataCopyForMaterial(a, true)), this.trans);
            rs &= db.BulkInsertFTM_TaskUserHelper(taskUsersHelper.Select(a => new FTM_TaskUserHelper().B_EntityDataCopyForMaterial(a, true)), this.trans);
            if (this.FTM_TaskSubjectTypeIds != null && this.FTM_TaskSubjectTypeIds.Count() > 0)
            {
                var taskTypeKeyList = this.FTM_TaskSubjectTypeIds.Select(x => new FTM_TaskSubjectType
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    subjectId = x,
                    taskId = this.id,
                }).ToList();
                rs &= db.BulkInsertFTM_TaskSubjectType(taskTypeKeyList, trans);
            }
            if (rs.result)
            {
                if (_trans == null) trans.Commit();
                if (requestFiles != null)
                {
                    new FileUploadSave(requestFiles, createOperation.id).SaveAs();
                }
                CustomerTaskMailSend();
                rs.message = "Görev başarıyla oluşturuldu.";
            }
            else
            {
                if (_trans == null) trans.Rollback();
                rs.message = "Görev oluşturulamadı.";
            }
            return rs;
        }
        public ResultStatus InsertCustomer(Guid? userId, HttpRequestBase requestFiles = null, DbTransaction _trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? this.db.BeginTransaction();
            this.Load(userId);
            var now = DateTime.Now;
            var _task = db.GetFTM_TaskById(this.id);
            if (_task != null)
                return new ResultStatus { result = false, message = "Görev zaten oluşturulmuş." };
            if (this.location == null && (this.type == (Int32)EnumFTM_TaskType.Kesif || this.type == (Int32)EnumFTM_TaskType.GelAL || this.type == (Int32)EnumFTM_TaskType.Diger))
                return new ResultStatus { result = false, message = "Bu görev tipinde konum seçmek zorunludur." };
            VWSH_User user = null;
            if (userId.HasValue)
            {
                user = db.GetVWSH_UserById(userId.Value);
                this.customerId = user?.CompanyId;
                this.customer_Title = user?.Company_Title;
            }
            if (!this.customerId.HasValue)
            {
                return new ResultStatus { result = false, message = "Herhangi bir firmada çalışmadığınızdan görev oluşturamazsınız." };
            }
            if (this.fixtureId != null)
            {
                this.SetFixtureInfo(this.fixtureId.Value);
            }
            var taskOperationCreate = new VWFTM_TaskOperation
            {
                createdby = userId,
                created = new DateTime(now.Ticks),
                location = this.lastOperationLocation,
                taskId = this.id,
                status = (int)EnumFTM_TaskOperationStatus.GorevOlusturulduMusteri,
                description = this.description,
            };
            taskOperations.Add(taskOperationCreate);
            this.notificationDate = DateTime.Now;
            var rs = db.InsertFTM_Task(new FTM_Task().B_EntityDataCopyForMaterial(this), this.trans);
            rs &= db.BulkInsertFTM_TaskOperation(taskOperations.Select(a => new FTM_TaskOperation().B_EntityDataCopyForMaterial(a, true)), this.trans);
            if (this.FTM_TaskSubjectTypeIds != null && this.FTM_TaskSubjectTypeIds.Count() > 0)
            {
                var taskTypeKeyList = this.FTM_TaskSubjectTypeIds.Select(x => new FTM_TaskSubjectType
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    subjectId = x,
                    taskId = this.id,
                }).ToList();
                rs &= db.BulkInsertFTM_TaskSubjectType(taskTypeKeyList, trans);
            }
            if (rs.result)
            {
                if (_trans == null) trans.Commit();
                rs.message = "Arıza kaydı başarıyla oluşturuldu.";
            }
            else
            {
                if (_trans == null) trans.Rollback();
                rs.message = "Arıza kaydı oluşturulamadı.";
            }
            if (rs.result == true)
            {
                if (requestFiles != null && rs.result == true)
                {
                    new FileUploadSave(requestFiles, taskOperationCreate.id).SaveAs();
                }
                if (files != null && files.Length > 0 && rs.result == true)
                {
                    var webPath = System.Configuration.ConfigurationManager.AppSettings["FilesPath"];
                    var path = string.Format("{0}/{1}/{2}/", webPath, "FTM_TaskOperation", taskOperationCreate.id);
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    var sysList = new List<SYS_Files>();
                    foreach (var item in files)
                    {
                        try
                        {
                            var fileName = Guid.NewGuid().ToString() + ".jpeg";
                            var filePath = Path.Combine(path, fileName);
                            var bytes = Convert.FromBase64String(item);
                            File.WriteAllBytes(filePath, bytes);
                            var newPath = path.Substring(path.IndexOf("Files", StringComparison.Ordinal));
                            var sysfiles = new SYS_Files
                            {
                                id = Guid.NewGuid(),
                                created = DateTime.Now,
                                createdby = userId,
                                DataId = taskOperationCreate.id,
                                FilePath = "/" + newPath + fileName,
                                DataTable = "FTM_TaskOperation",
                                FileGroup = "Görev Dosyası",
                                FileExtension = "jpeg",
                            };
                            sysList.Add(sysfiles);
                        }
                        catch
                        {
                        }
                    }
                    if (sysList.Count() > 0)
                    {
                        rs &= db.BulkInsertSYS_Files(sysList);
                    }
                }
                var operatorUserIds = db.GetSH_UserByRoleId(new Guid(SHRoles.SahaGorevOperator));
                if (operatorUserIds.Count() == 0)
                {
                    operatorUserIds = db.GetSH_UserByRoleId(new Guid(SHRoles.SahaGorevYonetici));
                }
                var task = db.GetVWFTM_TaskById(this.id);
                var opereators = new List<VWSH_User>();
                if (operatorUserIds.Count() > 0)
                {
                    opereators = db.GetVWSH_UserByIds(operatorUserIds).ToList();
                }
                var type_Title = TypeTitle(this.type);
                var notification = new Notification();
                foreach (var operatorm in opereators)
                {
                    var text = "<h3>Sayın " + operatorm.FullName + "</h3>";
                    text += "<div>Müşteri İşletme  <strong>" + task.customer_Title + "</strong> adına <strong>" + task.createdby_Title + "</strong> tarafından " + this.fixture_Title + " envanterine arıza kaydı oluşturulmuştur.</div>";
                    if (!string.IsNullOrEmpty(type_Title))
                    {
                        text += "<div>Görev Tipi : <strong>" + type_Title + "</strong></div>";
                    }
                    text += "<div>Arıza Açıklaması : " + this.description + "</div>";
                    text += "<div>Arıza Oluşturulma Tarihi : <strong>" + this.created + "</strong></div>";
                    text += "<div>Görev detaylarını görüntülemek ve personel ataması yapmak için <a href='" + TenantConfig.Tenant.GetWebUrl() + "/FTM/VWFTM_Task/Detail/" + this.id + "'>tıklayınız.</a> </div>";
                    text += "<div>Bilgilerinize.</div>";
                    var notify = "Sayın " + operatorm.FullName + " müşteri işletme " + task.customer_Title + " adına " + task.createdby_Title + " tarafından " + this.fixture_Title + "envaterine arıza kaydı oluşturulumuştur.";
                    new Email().Template("Template1", "gorevMailFoto.jpg", TenantConfig.Tenant.TenantName + " | Görev Yönetimi", text).Send((Int16)EmailSendTypes.Operasyon, operatorm.email, "Saha Görevi Oluşturuldu ", true);
                    notification.NotificationSend(operatorm.id, this.createdby, "Görev Yönetimi", notify, $"/FTM/VWFTM_Task/Detail?id={this.id}");
                }
                if (user != null)
                {
                    //Todo Oğuz müşteriye mail At.
                    var msj = "<h3>Sayın " + user.FullName + "</h3>";
                    msj += "<p>Arıza/Bakım kaydınız oluşturulmuştur.En kısa sürede ekiplerimiz tarafından çözümlenecektir.</p>";
                    var notify = "Sayın " + user.FullName + " Arıza/Bakım kaydınız oluşturulmuştur.En kısa sürede ekiplerimiz tarafından çözümlenecektir.";
                    msj += "<div>Tarafınıza <strong>" + this.code + "</strong> kodlu görev oluşturulmuştur.</div>";
                    if (!string.IsNullOrEmpty(type_Title))
                    {
                        msj += "<div>Görev Tipi : <strong>" + type_Title + "</strong></div>";
                    }
                    msj += "<div>Müşteri İşletme : <strong>" + customer.name + "</strong></div>";
                    if (this.fixtureId.HasValue)
                    {
                        msj += "<div>Envanter : <strong>" + this.fixture_Title + "</strong></div>";
                    }
                    if (this.description != null)
                    {
                        msj += "<div>Açıklama : <strong>" + this.description + "</strong></div>";
                    }
                    msj += "<div>Arıza/Bakım Oluşturulma Tarihi : <strong>" + this.created + "</strong></div>";
                    msj += "<p>Bilgilerinize.</p>";
                    msj += "<p>Saygılarımızla.</p>";
                    new Email().Template("Template1", "bos.png", TenantConfig.Tenant.TenantName + " | Görev Yönetimi", msj).Send((Int16)EmailSendTypes.Operasyon, user.email, "Arıza/Bakım Kaydı Oluşturuldu ", true);
                    notification.NotificationSend(user.id, this.createdby, "Görev Yönetimi", notify, $"/FTM/VWFTM_Task/Detail?id={this.id}");
                }
            }
            return rs;
        }
        public ResultStatus Update(Guid? userId, DbTransaction _trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? this.db.BeginTransaction();
            this.assignableUsers = this.assignableUsers ?? new List<Guid>();
            this.helperUsers = this.helperUsers ?? new List<Guid>();
            this.followUpUsers = this.followUpUsers ?? new List<Guid>();
            var task = db.GetVWFTM_TaskById(this.id);
            task.priority = this.priority;
            task.dueDate = this.dueDate;
            task.changed = DateTime.Now;
            task.changedby = userId;
            task.companyId = this.companyId;
            task.customerId = this.customerId;
            task.customerStorageId = this.customerStorageId;
            task.planStartDate = this.planStartDate;
            task.dueDate = this.dueDate;
            task.notificationDate = this.notificationDate;
            task.companyCarId = this.companyCarId;
            task.planLater = (short)EnumFTM_TaskPlanLater.Hayir;
            if (task.customerId.HasValue)
            {
                this.customer = db.GetVWCMP_CompanyById(task.customerId.Value);
            }
            if (task.customerStorageId.HasValue)
            {
                this.customerStorage = db.GetVWCMP_StorageById(task.customerStorageId.Value);
            }
            task.location = this.customerStorage?.location ?? this.customer?.location;
            var rs = new ResultStatus { result = true };
            if (this.fixtureId != null)
            {
                this.SetFixtureInfo(this.fixtureId.Value);
            }
            if (task.description != this.description)
            {
                var operations = db.GetFTM_TaskOperationByTaskId(this.id);
                var updateOperations = operations.Where(a => a.status == (int)EnumFTM_TaskOperationStatus.GorevOlusturuldu || a.status == (int)EnumFTM_TaskOperationStatus.GorevOlusturulduMusteri || a.status == (int)EnumFTM_TaskOperationStatus.GorevOlusturulduSistem).ToList();
                foreach (var item in updateOperations)
                {
                    item.changed = DateTime.Now;
                    item.changedby = userId;
                    item.description = this.description;
                }
                rs &= db.BulkUpdateFTM_TaskOperation(updateOperations, true, trans);
            }
            var insertUsers = new List<FTM_TaskUser>();
            var users = new VWSH_User[0];
            var helpers = new VWSH_User[0];
            var follows = new VWSH_User[0];
            if (task.assignUserId == null)
            {
                var mevcutkullanicilar = db.GetFTM_TaskUserByTaskId(this.id);
                var silinecekKullanicilar = mevcutkullanicilar.Where(a => !this.assignableUsers.Contains(a.userId.Value));
                var eklenecekKullanicilar = this.assignableUsers.Where(a => !mevcutkullanicilar.Select(c => c.userId).Contains(a)).ToArray();
                if (eklenecekKullanicilar.Count() > 0)
                {
                    users = db.GetVWSH_UserByIds(eklenecekKullanicilar);
                    var currentUsers = db.GetFTM_TaskUserByTaskId(this.id);
                    rs &= db.BulkDeleteFTM_TaskUser(currentUsers, trans);
                }
                var taskOperations = new List<FTM_TaskOperation>();
                if (users.Count() > 0)
                {
                    var now = DateTime.Now;
                    taskOperations.Add(new FTM_TaskOperation
                    {
                        createdby = userId,
                        created = new DateTime(now.Ticks),
                        location = this.lastOperationLocation,
                        taskId = this.id,
                        status = (int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi,
                        description = string.Join(",", users.Select(a => a.FullName)) + " kullanıcılarına atama yapıldı."
                    });
                    if (task.hasVerifyCode == true)
                    {
                        taskOperations.Add(new FTM_TaskOperation
                        {
                            createdby = userId,
                            created = new DateTime(now.Ticks).AddSeconds(1),
                            location = this.lastOperationLocation,
                            taskId = this.id,
                            status = (int)EnumFTM_TaskOperationStatus.DogrulamaKoduGonderildi,
                            description = string.Join(",", users.Select(a => a.FullName)) + " kullanıcılarına doğrulama kodu gönderildi."
                        });
                    }
                    insertUsers = assignableUsers.Select(a => new FTM_TaskUser
                    {
                        createdby = userId,
                        created = (DateTime.Now),
                        userId = a,
                        taskId = this.id,
                        verifyCode = this.hasVerifyCode == true ? RandomString(8) : null,
                        status = false,
                    }).ToList();
                }
                rs &= db.BulkInsertFTM_TaskOperation(taskOperations, trans);
                rs &= db.BulkDeleteFTM_TaskUser(silinecekKullanicilar, trans);
                rs &= db.BulkInsertFTM_TaskUser(insertUsers, trans);
            }
            var deletehelperUsers = db.GetVWFTM_TaskUserHelperByTaskId(this.id);
            if (deletehelperUsers.Count() > 0)
            {
                rs &= db.BulkDeleteFTM_TaskUserHelper(deletehelperUsers.Select(a => new FTM_TaskUserHelper().B_EntityDataCopyForMaterial(a, true)), trans);
            }
            var deletefollows = db.GetFTM_TaskFollowUpUserByTaskId(this.id);
            if (deletefollows.Count() > 0)
            {
                rs &= db.BulkDeleteFTM_TaskFollowUpUser(deletefollows, this.trans);
            }
            if (this.followUpUsers.Count() > 0)
            {
                rs &= db.BulkInsertFTM_TaskFollowUpUser(this.followUpUsers.Select(a => new FTM_TaskFollowUpUser
                {
                    userId = a,
                    taskId = this.id,
                    created = DateTime.Now,
                    createdby = this.changedby
                }), this.trans);
            }
            if (this.helperUsers.Count() > 0)
            {
                rs &= db.BulkInsertFTM_TaskUserHelper(this.helperUsers.Select(a => new FTM_TaskUserHelper
                {
                    userId = a,
                    taskId = this.id,
                    created = DateTime.Now,
                    createdby = this.changedby
                }), trans);
            }
            var data = db.GetFTM_TaskSubjectTypeByTaskId(this.id).ToList();
            rs &= db.BulkDeleteFTM_TaskSubjectType(data, trans);
            if (this.FTM_TaskSubjectTypeIds != null && this.FTM_TaskSubjectTypeIds.Count() > 0)
            {
                var taskTypeKeyList = this.FTM_TaskSubjectTypeIds.Select(x => new FTM_TaskSubjectType
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    subjectId = x,
                    taskId = this.id,
                }).ToList();
                rs &= db.BulkInsertFTM_TaskSubjectType(taskTypeKeyList, trans);
            }
            rs &= db.UpdateFTM_Task(new FTM_Task().B_EntityDataCopyForMaterial(task, true), false, trans);
            if (rs.result)
            {
                if (_trans == null) trans.Commit();
                rs.message = "Görev başarıyla güncellendi.";
            }
            else
            {
                if (_trans == null) trans.Rollback();
                rs.message = "Görev güncellenemedi.";
            }
            var type_Title = TypeTitle(this.type);
            var notification = new Notification();
            if ((this.sendMail == true || this.hasVerifyCode == true) && rs.result == true)
            {
                foreach (var item in insertUsers)
                {
                    var user = users.Where(a => a.id == item.userId).FirstOrDefault();
                    if (user == null)
                    {
                        continue;
                    }
                    var notify = "Sayın " + user.FullName + ", tarafınıza <strong>" + this.code + "</strong> kodlu saha görevi oluşturulmuştur.";
                    var text = "<h3>Sayın " + user.FullName + "</h3>";
                    text += "<div>Tarafınıza <strong>" + this.code + "</strong> kodlu saha görevi oluşturulmuştur.</div>";
                    text += (this.hasVerifyCode == true ? "<div>Görev doğrulama Kodu : <strong>" + item.verifyCode + "</strong></div>" : "");
                    text += "<div>Görev detaylarını görüntülemek için <a href='" + TenantConfig.Tenant.GetWebUrl() + "/FTM/VWFTM_Task/Detail/" + this.id + "'>tıklayınız.</a> </div>";
                    text += "<div>Bilgilerinize.</div>";
                    new Email().Template("Template1", "gorevMailFoto.jpg", TenantConfig.Tenant.TenantName + " | Görev Yönetimi", text).Send((Int16)EmailSendTypes.Operasyon, user.email, "Saha Görevi Oluşturuldu ", true);
                    notification.NotificationSend(user.id, this.createdby, "Görev Yönetimi", notify, $"/FTM/VWFTM_Task/Detail?id={this.id}");
                }
            }
            if (rs.result)
            {
                CustomerTaskMailSend();
            }
            return rs;
        }
        public ResultStatus UpdateStaff(Guid? userId, DbTransaction _trans = null)
        {
            var rs = new ResultStatus { result = true };
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? this.db.BeginTransaction();

			if (this.assignableUsers != null && this.assignableUsers.Count() == 0)
			{
                rs = new ResultStatus { result = false, message = "Lütfen Üstlenebilecek Personel Seçiniz" };
			}

            this.assignableUsers = this.assignableUsers ?? new List<Guid>();
            var task = db.GetFTM_TaskById(this.id);
            if (task == null)
            {
                return new ResultStatus { result = false, message = "Görev bulunamadı." };
            }
            task.changed = DateTime.Now;
            task.changedby = userId;
            var insertUsers = new List<FTM_TaskUser>();
            var users = new VWSH_User[0];
            var mevcutkullanicilar = db.GetFTM_TaskUserByTaskId(this.id);
            var silinecekKullanicilar = mevcutkullanicilar.Where(a => !this.assignableUsers.Contains(a.userId.Value));
            var eklenecekKullanicilar = this.assignableUsers.Where(a => !mevcutkullanicilar.Select(c => c.userId).Contains(a)).ToArray();
            if (eklenecekKullanicilar.Count() > 0)
            {
                users = db.GetVWSH_UserByIds(eklenecekKullanicilar);
                var currentUsers = db.GetFTM_TaskUserByTaskId(this.id);
                rs &= db.BulkDeleteFTM_TaskUser(currentUsers, trans);
            }
            var taskOperations = new List<FTM_TaskOperation>();
            if (users.Count() > 0)
            {
                var now = DateTime.Now;
                taskOperations.Add(new FTM_TaskOperation
                {
                    createdby = userId,
                    created = new DateTime(now.Ticks),
                    location = this.lastOperationLocation,
                    taskId = this.id,
                    status = (int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi,
                    description = string.Join(",", users.Select(a => a.FullName)) + " kullanıcılarına atama yapıldı."
                });
                insertUsers = assignableUsers.Select(a => new FTM_TaskUser
                {
                    createdby = userId,
                    created = (DateTime.Now),
                    userId = a,
                    taskId = this.id,
                    verifyCode = null,
                    status = false,
                }).ToList();
            }
            rs &= db.BulkInsertFTM_TaskOperation(taskOperations, trans);
            rs &= db.BulkDeleteFTM_TaskUser(silinecekKullanicilar, trans);
            rs &= db.BulkInsertFTM_TaskUser(insertUsers, trans);
            rs &= db.UpdateFTM_Task(new FTM_Task().B_EntityDataCopyForMaterial(task, true), false, this.trans);
            if (rs.result)
            {
                this.trans.Commit();
            }
            else
            {
                this.trans.Rollback();
            }
            return rs;
        }
        public ResultStatus Delete(Guid? userId, DbTransaction _trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? this.db.BeginTransaction();
            var task = db.GetFTM_TaskById(this.id);
            if (task == null)
            {
                return new ResultStatus
                {
                    message = "Görev zaten silinmiş durumda.",
                    result = false,
                };
            }
            var _taskOperation = db.GetFTM_TaskOperationByTaskId(task.id);
            var _taskFormResult = db.GetFTM_TaskFormResultByTaskId(task.id);
            var _taskSubjects = db.GetFTM_TaskSubjectTypeByTaskId(task.id);
            var _taskUser = db.GetFTM_TaskUserByTaskId(task.id);
            var _taskUserHelper = db.GetFTM_TaskUserHelperByTaskId(task.id);
            if (_taskOperation.Count(a => a.status == (int)EnumFTM_TaskOperationStatus.CozumOnaylandi) > 0)
            {
                return new ResultStatus
                {
                    message = task.code + " kodlu görev tamamlanmış silinemez.",
                    result = false,
                };
            }
            var managerUserIds = db.GetSH_UserByRoleId(new Guid(SHRoles.SahaGorevYonetici));
            var operatorUserIds = db.GetSH_UserByRoleId(new Guid(SHRoles.SahaGorevOperator));
            if (userId.HasValue && ((managerUserIds.Contains(userId.Value) || operatorUserIds.Contains(userId.Value)) == false))
            {
                if (task.createdby != userId)
                {
                    return new ResultStatus
                    {
                        message = task.code + " kodlu görevi siz oluşturmadığınız için silemessiniz.",
                        result = false,
                    };
                }
            }
            var rs = db.BulkDeleteFTM_TaskFormResult(_taskFormResult, trans);
            rs &= db.BulkDeleteFTM_TaskSubjectType(_taskSubjects, trans);
            rs &= db.BulkDeleteFTM_TaskUser(_taskUser, trans);
            rs &= db.BulkDeleteFTM_TaskOperation(_taskOperation, trans);
            rs &= db.BulkDeleteFTM_TaskUserHelper(_taskUserHelper, trans);
            rs &= db.DeleteFTM_Task(task, trans);
            if (rs.result)
            {
                if (_trans == null) trans.Commit();
            }
            else
            {
                if (_trans == null) trans.Rollback();
            }
            return rs;
        }
        public string getPassingTime(DateTime? time)
        {
            if (this.created.HasValue && time.HasValue)
            {
                var fark = (time - this.created);
                var result = (fark.Value.Days != 0 ? fark.Value.Days + " Gün " : "");
                result += (fark.Value.Hours != 0 ? fark.Value.Hours + " Saat " : "");
                result += (fark.Value.Minutes != 0 ? fark.Value.Minutes + " Dakika " : "");
                result += (fark.Value.Seconds != 0 ? fark.Value.Seconds + " Saniye " : "");
                return result;
            }
            else
            {
                return "-";
            }
        }
        public void SetFixtureInfo(Guid inventoryId)
        {
            var inventory = db.GetVWPRD_InventoryById(inventoryId);
            this.location = inventory.location;
            this.fixture_Title = inventory.fullName;
            switch ((EnumPRD_InventoryActionType)inventory.lastActionType)
            {
                case EnumPRD_InventoryActionType.Depoda:
                    var storage = db.GetVWCMP_StorageById(inventory.lastActionDataId.Value);
                    if (storage != null)
                    {
                        this.customerId = storage.companyId;
                        this.customer_Title = storage.name + "(" + storage.code + ")";
                    }
                    break;
                case EnumPRD_InventoryActionType.Personelde:
                    var person = db.GetVWSH_UserById(inventory.lastActionDataId.Value);
                    this.customerId = person.CompanyId;
                    this.customer_Title = person.Company_Title;
                    break;
                case EnumPRD_InventoryActionType.KirayaVerildi:
                case EnumPRD_InventoryActionType.CikisYapildi:
                    this.customerId = inventory.lastActionDataCompanyId;
                    this.customer_Title = inventory.lastActionDataCompanyId_Title;
                    break;
            }
        }
        public void CustomerTaskMailSend()
        {
            db = db ?? new WorkOfTimeDatabase();
            var wantedDocuments = db.GetVWCMP_CompanyFileSelectorByCustomerId(this.customerId.Value);
            var documentList = new List<string>();
            var emailSender = this.userMails?.Split(',').ToList();
            if (emailSender != null)
            {
                if (this.customerStorage.email != null)
                {
                    emailSender.Remove(this.customerStorage.email);
                }
                if (this.isSendDocuments != null && this.isSendDocuments == (int)EnumFTM_TaskPersonIsSendDocuments.Evet)
                {
                    var persons = new List<Guid>();
                    persons.AddRange(this.assignableUsers);
                    persons.AddRange(this.helperUsers);
                    var personGroup = persons.GroupBy(x => x).ToList();
                    var personDocuments = db.GetSYS_FilesInDataId(personGroup.Select(x => x.Key).ToArray());
                    var webUrl = TenantConfig.Tenant.GetWebUrl();
                    foreach (var person in personGroup)
                    {
                        foreach (var documents in wantedDocuments)
                        {
                            var doc = personDocuments.Where(x => x.DataId == person.Key && x.FileGroup == documents.fileGroupName).FirstOrDefault();
                            if (doc != null)
                            {
                                documentList.Add(webUrl + "" + doc.FilePath);
                            }

                        }
                    }
                }
                foreach (var item in emailSender)
                {
                    var notification = new Notification();
                    var text = "Sayın ";
                    var mail = item;
                    var createdbyEmail = db.GetVWSH_UserById(this.createdby.Value);
                    var bccMail = new List<string>();
                    if (createdbyEmail != null)
                    {
                        bccMail.Add(createdbyEmail.email);
                    }
                    text += "Sayın Yetkili";
                    text += "<div>Tarafınıza <strong>" + this.code + "</strong> kodlu görev oluşturulmuştur.</div>";
                    if (this.type.HasValue)
                    {
                        var typeTitle = ((EnumFTM_TaskType)this.type).B_ToDescription();
                        text += "<div>Görev Tipi:<strong> : " + typeTitle + "</strong></div>";
                    }
                    text += "<div>Görev Planlanmış Başlangıç Tarihi <strong> : " + this.planStartDate + "</strong></div>";
                    text += "<div>Görev Planlanmış Bitiş Tarihi <strong> : " + this.dueDate + "</strong></div>";
                    text += "<br>";
                    if (this.companyCarId.HasValue)
                    {
                        var car = db.GetVWCMP_CompanyCarsById(companyCarId.Value);
                        if (car != null)
                        {
                            text += !string.IsNullOrEmpty(car.brand) ? "<div>Görevli Araç Marka : <strong> " + car.brand + "</strong></div>" : "";
                            text += !string.IsNullOrEmpty(car.model) ? "<div>Görevli Araç Model : <strong> " + car.model + "</strong></div>" : "";
                            text += !string.IsNullOrEmpty(car.plate) ? "<div>Görevli Araç Plaka : <strong> " + car.plate + "</strong></div>" : "";
                            text += !string.IsNullOrEmpty(car.color) ? "<div>Görevli Araç Renk : <strong> " + car.color + "</strong></div>" : "";
                        }
                    }
                    text += "<br>";
                    if (this.assignableUsers.Count() > 0)
                    {
                        var personalData = db.GetVWSH_UserByIds(this.assignableUsers.ToArray());
                        if (personalData.Count() > 0)
                        {
                            text += "<div>Görevli Personeller : <strong>" + string.Join(",", personalData.Select(x => x.FullName)) + "</strong></div>";
                        }
                    }
                    text += "<br>";
                    if (this.helperUsers.Count() > 0)
                    {
                        var helperUsersData = db.GetVWSH_UserByIds(this.helperUsers.ToArray());
                        if (helperUsersData.Count() > 0)
                        {
                            text += "<div>Yardımcı Personeller : <strong>" + string.Join(",", helperUsersData.Select(x => x.FullName)) + "</strong></div>";
                        }
                    }
                    text += "<br>";
                    if (this.description != null)
                    {
                        text += "<div>Görev Açıklaması : <strong>" + this.description + "</strong></div>";
                    }
                    text += "<br>";
                    text += "<div>Bilgilerinize.</div>";
                    new Email().Template("Template1", "gorevMailFoto.jpg", TenantConfig.Tenant.TenantName + " | Görev Bildirimi", text).Send((Int16)EmailSendTypes.Operasyon, mail, "İş Planı Hakkında ", true, null, bccMail.ToArray(), documentList.ToArray(), true);
                }
                if (this.customerStorage != null && !String.IsNullOrEmpty(this.customerStorage.email) && (this.lastOperationStatus == null || this.lastOperationStatus == (int)EnumFTM_TaskOperationStatus.PersonelAtamaYapildi))
                {
                    var notification = new Notification();
                    var text = "Sayın ";
                    var mail = this.customerStorage.email;
                    var createdbyEmail = db.GetVWSH_UserById(this.createdby.Value);
                    var bccMail = new List<string>();
                    if (createdbyEmail != null)
                    {
                        bccMail.Add(createdbyEmail.email);
                    }
                    var fullName = "";
                    var customerId = new Guid();
                    text += "<strong>" + this.customer.name + "</strong> Yetkilileri";
                    text += "<div>Tarafınıza <strong>" + this.code + "</strong> kodlu görev oluşturulmuştur.</div>";
                    if (this.type.HasValue)
                    {
                        var typeTitle = ((EnumFTM_TaskType)this.type).B_ToDescription();
                        text += "<div>Görev Tipi:<strong> : " + typeTitle + "</strong></div>";
                    }
                    text += "<div>Görev Planlanmış Başlangıç Tarihi <strong> : " + this.planStartDate + "</strong></div>";
                    text += "<div>Görev Planlanmış Bitiş Tarihi <strong> : " + this.dueDate + "</strong></div>";
                    text += "<br>";
                    if (this.companyCarId.HasValue)
                    {
                        var car = db.GetVWCMP_CompanyCarsById(companyCarId.Value);
                        if (car != null)
                        {
                            text += !string.IsNullOrEmpty(car.brand) ? "<div>Görevli Araç Marka : <strong> " + car.brand + "</strong></div>" : "";
                            text += !string.IsNullOrEmpty(car.model) ? "<div>Görevli Araç Model : <strong> " + car.model + "</strong></div>" : "";
                            text += !string.IsNullOrEmpty(car.plate) ? "<div>Görevli Araç Plaka : <strong> " + car.plate + "</strong></div>" : "";
                            text += !string.IsNullOrEmpty(car.color) ? "<div>Görevli Araç Renk : <strong> " + car.color + "</strong></div>" : "";
                        }
                    }
                    text += "<br>";
                    if (this.assignableUsers.Count() > 0)
                    {
                        var personalData = db.GetVWSH_UserByIds(this.assignableUsers.ToArray());
                        if (personalData.Count() > 0)
                        {
                            text += "<div>Görevli Personeller : <strong>" + string.Join(",", personalData.Select(x => x.FullName)) + "</strong></div>";
                        }
                    }
                    text += "<br>";
                    if (this.helperUsers.Count() > 0)
                    {
                        var helperUsersData = db.GetVWSH_UserByIds(this.helperUsers.ToArray());
                        if (helperUsersData.Count() > 0)
                        {
                            text += "<div>Yardımcı Personeller : <strong>" + string.Join(",", helperUsersData.Select(x => x.FullName)) + "</strong></div>";
                        }
                    }
                    text += "<br>";
                    if (this.description != null)
                    {
                        text += "<div>Görev Açıklaması : <strong>" + this.description + "</strong></div>";
                    }
                    text += "<br>";
                    text += "<div>Bilgilerinize.</div>";
                    var notify = string.Format("Sayın " + fullName + ", tarafınıza " + this.code + " kodlu görev oluşturulumuştur.");
                    new Email().Template("Template1", "gorevMailFoto.jpg", TenantConfig.Tenant.TenantName + " | Görev Bildirimi", text).Send((Int16)EmailSendTypes.Operasyon, mail, "İş Planı Hakkında ", true, null, bccMail.ToArray(), documentList.ToArray(), true);
                    notification.NotificationSend(customerId, this.createdby, "Görev Bildirimi", notify, $"/FTM/VWFTM_Task/Detail?id={this.id}");
                }
            }

        }
        public SummaryHeadersTask GetTaskSummary(Guid userId)
        {
            this.Load(userId);
            var userRoles = db.GetSH_UserRoleByUserId(userId);
            if (userRoles.Count() > 0)
            {
                if (userRoles.Count(x => x.roleid == new Guid(SHRoles.SahaGorevYonetici)) > 0 || userRoles.Count(x => x.roleid == new Guid(SHRoles.SahaGorevOperator)) > 0)
                {
                    return db.GetVWFTM_TaskByCounts(userId);
                }
            }
            return db.GetVWFTM_TaskByUserIdCounts(userId);
        }
        public SimpleQuery UpdateQuery(SimpleQuery query, PageSecurity userStatus, short comingType)
        {
            //comingType 1 ise task
            //comingType 2 ise company
            //comintType 3 ise bakım envanteri için
            //comingType 4 ise form relation için texte bakılacak
            //comingType 5 ise companyId
            BEXP filter = null;
            if (userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevYonetici)) || userStatus.AuthorizedRoles.Contains(new Guid(SHRoles.SahaGorevOperator)))
            {
                var operandColumn = "";
                if (comingType == 1)
                {
                    operandColumn = "customerId";
                }
                else if (comingType == 2)
                {
                    operandColumn = "id";
                }
                else if (comingType == 3)
                {
                    operandColumn = "firstActionDataCompanyId";
                }
                else if (comingType == 4)
                {
                    operandColumn = "companyId_Title";
                }
                else if (comingType == 5)
                {
                    operandColumn = "companyId";
                }
                this.db = this.db ?? new WorkOfTimeDatabase();
                var authoritys = db.GetVWFTM_TaskAuthorityByUserId(userStatus.user.id);
                if (comingType == 4)
                {
                    foreach (var authority in authoritys)
                    {
                        filter |= new BEXP
                        {
                            Operand1 = (COL)operandColumn,
                            Operator = BinaryOperator.Equal,
                            Operand2 = (VAL)authority.customerId_Title
                        };
                    }
                }
                else
                {
                    foreach (var authority in authoritys)
                    {
                        filter |= new BEXP
                        {
                            Operand1 = (COL)operandColumn,
                            Operator = BinaryOperator.Equal,
                            Operand2 = (VAL)authority.customerId
                        };
                    }
                }
                //Projeler  ?? 
                query.Filter &= filter;
                return query;
            }
            else
            {
                return query;
            }
        }
        public static SimpleQuery ActivityTrackingUpdateQuery(SimpleQuery query)
        {
            BEXP filter = null;
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1).Date;
            var endOfMonth = startOfMonth.AddMonths(1).Date;
            filter = new BEXP
            {
                Operand1 = new BEXP
                {
                    Operand1 = (COL)"created",
                    Operator = BinaryOperator.GreaterThanOrEqual,
                    Operand2 = (VAL)startOfMonth
                },
                Operator = BinaryOperator.And,
                Operand2 = new BEXP
                {
                    Operand1 = (COL)"created",
                    Operator = BinaryOperator.LessThanOrEqual,
                    Operand2 = (VAL)endOfMonth
                }
            };
            query.Filter &= filter;
            return query;
        }
        public static SimpleQuery DashboardUpdateQuery(SimpleQuery query, Guid companyId)
        {
            BEXP filter = null;
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1).Date;
            var endOfMonth = startOfMonth.AddMonths(1).Date;
            filter = new BEXP
            {
                Operand1 = new BEXP
                {
                    Operand1 = new BEXP
                    {
                        Operand1 = (COL)"created",
                        Operator = BinaryOperator.GreaterThanOrEqual,
                        Operand2 = (VAL)startOfMonth
                    },
                    Operator = BinaryOperator.And,
                    Operand2 = new BEXP
                    {
                        Operand1 = (COL)"created",
                        Operator = BinaryOperator.LessThanOrEqual,
                        Operand2 = (VAL)endOfMonth
                    }
                },
                Operand2 = new BEXP
                {
                    Operand1 = (COL)"customerId",
                    Operator = BinaryOperator.Equal,
                    Operand2 = (VAL)companyId
                },
                Operator = BinaryOperator.And
            };
            query.Filter &= filter;
            return query;
        }
        public class MonthlyTypeLineChartDataModel
        {
            public string taskType { get; set; }
            public string Reso { get; set; }
            public string Resp { get; set; }
            public int Month { get; set; }
            public string taskIds { get; set; }
        }
        public class MonthlyPersonData
        {
            public string text { get; set; }
            public int year { get; set; }
            public string personelName { get; set; }
            public double resolutionTime { get; set; }
            public double responseTime { get; set; }
            public int count { get; set; }
        }
        public class MonthTaskReportModel
        {
            public Guid taskId { get; set; }
            public string personnelName { get; set; }
            public int year { get; set; }
            public int month { get; set; }
            public int responseTime { get; set; }
            public int resolutionTime { get; set; }
            public string subjectTitle { get; set; }
            public string taskType_Title { get; set; }
            public Guid assignUserId { get; set; }
            public string groupId_Title { get; set; }
            public Guid customerId { get; set; }
            public Guid customerStorageId { get; set; }
        }
        public class DailyPersonalReportModel
        {
            public int id { get; set; }
            public DateTime start { get; set; }
            public DateTime end { get; set; }
            public string title { get; set; }
            public string customer { get; set; }
            public Guid attendees { get; set; }
            public Guid taskId { get; set; }
            public string taskCode { get; set; }
            public string customerStorage_Title { get; set; }
            public string taskType_Title { get; set; }
            public string taskDescription { get; set; }
            public string lastOperationStatus_Title { get; set; }
            public string color { get; set; }
            public string taskStatus_Title { get; set; }
        }
        public class DailyPersonalReportPersonalData
        {
            public string text { get; set; }
            public Guid value { get; set; }
            public string color { get; set; }
            public List<DailyPersonalReportModel> dataSource { get; set; }
        }
        public class VWModelActivityResult
        {
            public Guid Id { get; set; }
            public string FullName { get; set; }
            public string Photo { get; set; }
            public int CompleteCount { get; set; }
            public int WorkingNow { get; set; }
            public int taskCount { get; set; }
            public string totalWorkingHours { get; set; }
        }
        public class VWModelOperatorActivityResult
        {
            public Guid Id { get; set; }
            public string FullName { get; set; }
            public int OpenedTask { get; set; }
            public int ApprovedTask { get; set; }
            public int MyAppointmentTask { get; set; }
        }
        public class VWINV_PermitAndTaskEndCount
        {
            public VWINV_Permit[] permits { get; set; }
            public int taskCount { get; set; }
            public string userName { get; set; }
        }
        public class VWModelStaffReportResult
        {
            public Guid Id { get; set; }
            public string FullName { get; set; }
            public string type_Title { get; set; }
            public short? taskType { get; set; }
            public string taskSubject_Title { get; set; }
            public DateTime? startDate { get; set; }
            public DateTime? endDate { get; set; }
            public string customerName { get; set; }
            public string customerLocation { get; set; }
            public string taskStatus_Title { get; set; }
            public string totalWorkingHours { get; set; }
        }
        public class VWModelTaskCustomerReportResult
        {
            public Guid Id { get; set; }
            public string FullName { get; set; }
            public int allTaskCount { get; set; }
            public int finishedTask { get; set; }
            public Guid[] finishedTaskIds { get; set; }
        }
        public partial class VWINV_Fixture : InfolineTable
        {
            public string UniversalCode { get; set; }
            public string Name { get; set; }
            public Guid? HardwareType { get; set; }
            public Guid? Brand { get; set; }
            public Guid? Model { get; set; }
            public int? LifeTime { get; set; }
            public int? OSType { get; set; }
            public bool? IsSocketable { get; set; }
            public bool? IsTraceable { get; set; }
            public double? Width { get; set; }
            public double? Height { get; set; }
            public double? Weight { get; set; }
            public double? Area { get; set; }
            public string SerialNo { get; set; }
            public string IMEINo { get; set; }
            public string BarcodeNo { get; set; }
            public Guid? Materialid { get; set; }
            public Guid? MaterialStockid { get; set; }
            public string FixtureCode { get; set; }
            public string QRCode { get; set; }
            public string AccesableIPAddress { get; set; }
            public string AccesableUserName { get; set; }
            public string AccesablePassword { get; set; }
            public Guid? PID { get; set; }
            public string DetailDescription { get; set; }
            public int? MaintancePeriod { get; set; }
            public Guid? fixtureId { get; set; }
            public DateTime? lastTaskDate { get; set; }
            public int? ActionType { get; set; }
            public string ActionType_Title { get; set; }
            public DateTime? ActionDate { get; set; }
            public Guid? ActionDataId { get; set; }
            public string ActionDataId_Title { get; set; }
            public IGeometry Location { get; set; }
            public string createdby_Title { get; set; }
            public string changedby_Title { get; set; }
            public string HardwareType_Title { get; set; }
            public string Brand_Title { get; set; }
            public string Model_Title { get; set; }
            public string FullName { get; set; }
            public string OSType_Title { get; set; }
            public string IsTraceable_Title { get; set; }
            public string IsSocketable_Title { get; set; }
            public string PoligonToString { get; set; }
            public string Material_Title { get; set; }
            public double? FixturePrice { get; set; }
            public Guid? InvoiceId { get; set; }
            public string InvoiceId_Title { get; set; }
            public int? FixtureTypeNo { get; set; }
            public string Confirmation { get; set; }
            public string SearchText { get; set; }
            public DateTime? MaintanceDatePast { get; set; }
            public DateTime? MaintanceDateFuture { get; set; }
            public int? MaintenanceRemainingDay { get; set; }
            public Guid? IdUser { get; set; }
            public Guid? IdCompany { get; set; }
        }
        public class FTM_TaskCustom
        {
            public string NameSurName { get; set; }
            public string Phone { get; set; }
            public string Adress { get; set; }
            public string Description { get; set; }
            public IGeometry Location { get; set; }
            public string[] Files { get; set; }
        }
        public class VModelFTM_Task : VWFTM_Task
        {
            public VModelFTM_Task()
            {
                if (!this.planStartDate.HasValue)
                {
                    this.planStartDate = new DateTime(2000, 1, 1);
                }
                if (!this.dueDate.HasValue && !this.lastOperationDate.HasValue)
                {
                    this.dueDate = new DateTime(2000, 1, 1);
                }
            }
            public DateTime start
            {
                get
                {
                    return this.planStartDate.HasValue
                        ? this.planStartDate.Value
                        : new DateTime(2000, 1, 1);
                }
            }
            public DateTime end
            {
                get
                {
                    return this.lastOperationDate.HasValue
                        ? this.lastOperationDate.Value
                        : this.dueDate.HasValue
                            ? this.dueDate.Value
                            : new DateTime(2000, 1, 1);
                }
            }
            public string title
            {
                get
                {
                    return this.customer_Title;
                }
            }
            public Guid _id
            {
                get
                {
                    return this.id;
                }
            }
        }
    }
}
