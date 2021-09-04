using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMTaskTemplateModel : VWFTM_TaskTemplate
    {

        public VMTaskTemplateModel()
        {
            this.sendMail = this.sendMail ?? true;
        }

        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }

        public List<FTM_TaskTemplateUser> taskTemplateUsers { get; set; } = new List<FTM_TaskTemplateUser>();
        public List<FTM_TaskTemplateUserHelper> taskTemplateUsersHelper { get; set; } = new List<FTM_TaskTemplateUserHelper>();
        public List<FTM_TaskTemplateInventories> taskTemplateInventories { get; set; } = new List<FTM_TaskTemplateInventories>();
        public List<FTM_TaskTemplateSubjectType> taskTemplateSubjectType { get; set; } = new List<FTM_TaskTemplateSubjectType>();
        public List<Guid> assignableUsers { get; set; } = new List<Guid>();
        public List<Guid> helperUsers { get; set; } = new List<Guid>();
        public VWCMP_Company company { get; set; } = new VWCMP_Company();
        public VWCMP_Company customer { get; set; } = new VWCMP_Company();
        public VWCMP_Storage customerStorage { get; set; } = new VWCMP_Storage();
        public Guid[] FTM_TaskTemplateSubjectTypeIds { get; set; } = new Guid[0];
        public List<Guid?> personUserIds { get; set; } = new List<Guid?>();
        public Guid[] PRD_Inventory_Ids { get; set; } = new Guid[0];

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public void Load(Guid? userId = null)
        {

            this.db = this.db ?? new WorkOfTimeDatabase();
            var template = db.GetVWFTM_TaskTemplateById(this.id);

            if (template != null)
            {

                this.B_EntityDataCopyForMaterial(template, true);
                this.taskTemplateUsers = db.GetFTM_TaskTemplateUserByTaskTemplateId(this.id).ToList();
                this.taskTemplateUsersHelper = db.GetFTM_TaskTemplateUserHelperByTaskTemplateId(this.id).ToList();
                this.assignableUsers = taskTemplateUsers.Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToList();
                this.helperUsers = taskTemplateUsersHelper.Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToList();

                this.taskTemplateInventories = db.GetFTM_TaskTemplateInventoriesByTaskTemplateId(this.id).ToList();
                this.taskTemplateSubjectType = db.GetFTM_TaskTemplateSubjectTypeByTaskTemplateId(this.id).ToList();

                this.PRD_Inventory_Ids = this.taskTemplateInventories.Where(a => a.inventoryId.HasValue).Select(a => a.inventoryId.Value).ToArray();
                this.FTM_TaskTemplateSubjectTypeIds = this.taskTemplateSubjectType.Where(a => a.subjectId.HasValue).Select(a => a.subjectId.Value).ToArray();

            }
            else
            {

            }

            this.code = this.code ?? BusinessExtensions.B_GetIdCode();
            this.hasVerifyCode = this.hasVerifyCode ?? false;
            this.type = this.type ?? (int)EnumFTM_TaskType.Ariza;
            this.priority = this.priority ?? (int)EnumFTM_TaskPriority.Orta;
            this.created = this.created ?? DateTime.Now;
            this.createdby = this.createdby ?? userId;
            this.assignableUsers = this.assignableUsers ?? new List<Guid>();
            this.helperUsers = this.helperUsers ?? new List<Guid>();
            this.personUserIds = db.GetSH_UserByRoleIdList(new Guid(SHRoles.SahaGorevPersonel));

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

        }

        public ResultStatus Update(Guid userId, DbTransaction _trans = null)
        {

            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? this.db.BeginTransaction();

            var now = DateTime.Now;
            var inventories = new List<VWPRD_Inventory>();

            if (this.location == null && (this.type == (Int32)EnumFTM_TaskType.Kesif || this.type == (Int32)EnumFTM_TaskType.GelAL || this.type == (Int32)EnumFTM_TaskType.Diger))
            {
                return new ResultStatus { result = false, message = "Bu görev tipinde konum seçmek zorunludur." };
            }
            if (!this.customerId.HasValue)
            {
                return new ResultStatus { result = false, message = "Müşteri işletme zorunludur." };
            }
            if (this.assignableUsers.Count() > 0)
            {
                taskTemplateUsers = assignableUsers.Select(a => new FTM_TaskTemplateUser
                {
                    createdby = userId,
                    created = (DateTime.Now),
                    userId = a,
                    taskTemplateId = this.id,
                    verifyCode = this.hasVerifyCode == true ? RandomString(8) : null,
                    status = false,
                }).ToList();
            }
            if (this.helperUsers.Count() > 0)
            {
                taskTemplateUsersHelper = helperUsers.Select(a => new FTM_TaskTemplateUserHelper
                {
                    createdby = userId,
                    created = (DateTime.Now),
                    userId = a,
                    taskTemplateId = this.id,
                }).ToList();
            }

            if (this.PRD_Inventory_Ids.Count() > 0)
            {
                taskTemplateInventories = this.PRD_Inventory_Ids.Select(a => new FTM_TaskTemplateInventories
                {
                    createdby = userId,
                    created = DateTime.Now,
                    inventoryId = a,
                    taskTemplateId = this.id
                }).ToList();
            }

            var rs = new ResultStatus { result = true };

            rs &= db.UpdateFTM_TaskTemplate(this.B_ConvertType<FTM_TaskTemplate>(), false, trans);
            rs &= db.BulkDeleteFTM_TaskTemplateUser(db.GetFTM_TaskTemplateUserByTaskTemplateId(this.id));
            rs &= db.BulkDeleteFTM_TaskTemplateUserHelper(db.GetFTM_TaskTemplateUserHelperByTaskTemplateId(this.id));
            rs &= db.BulkDeleteFTM_TaskTemplateInventories(db.GetFTM_TaskTemplateInventoriesByTaskTemplateId(this.id));

            rs &= db.BulkInsertFTM_TaskTemplateUser(taskTemplateUsers, trans);
            rs &= db.BulkInsertFTM_TaskTemplateUserHelper(taskTemplateUsersHelper, trans);
            rs &= db.BulkInsertFTM_TaskTemplateInventories(taskTemplateInventories, trans);

            if (this.FTM_TaskTemplateSubjectTypeIds != null && this.FTM_TaskTemplateSubjectTypeIds.Count() > 0)
            {
                var taskTypeKeyList = this.FTM_TaskTemplateSubjectTypeIds.Select(x => new FTM_TaskTemplateSubjectType
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    subjectId = x,
                    taskTemplateId = this.id,
                }).ToList();

                rs &= db.BulkDeleteFTM_TaskTemplateSubjectType(db.GetFTM_TaskTemplateSubjectTypeByTaskTemplateId(this.id));
                rs &= db.BulkInsertFTM_TaskTemplateSubjectType(taskTypeKeyList, trans);

            }

            if (rs.result)
            {
                if (_trans == null) trans.Commit();
                rs.message = "Görev Şablonu başarıyla güncellendi.";
            }
            else
            {
                if (_trans == null) trans.Rollback();
                rs.message = "Görev Şablonu oluşturulamadı.";
            }

            return rs;

        }

        public ResultStatus Insert(Guid userId, DbTransaction _trans = null)
        {

            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? this.db.BeginTransaction();

            this.Load(userId);

            var now = DateTime.Now;
            var inventories = new List<VWPRD_Inventory>();

            if (this.location == null && (this.type == (Int32)EnumFTM_TaskType.Kesif || this.type == (Int32)EnumFTM_TaskType.GelAL || this.type == (Int32)EnumFTM_TaskType.Diger))
            {
                return new ResultStatus { result = false, message = "Bu görev tipinde konum seçmek zorunludur." };
            }
            if (!this.customerId.HasValue)
            {
                return new ResultStatus { result = false, message = "Müşteri işletme zorunludur." };
            }
            if (this.assignableUsers.Count() > 0)
            {
                taskTemplateUsers = assignableUsers.Select(a => new FTM_TaskTemplateUser
                {
                    createdby = userId,
                    created = (DateTime.Now),
                    userId = a,
                    taskTemplateId = this.id,
                    verifyCode = this.hasVerifyCode == true ? RandomString(8) : null,
                    status = false,
                }).ToList();
            }
            if (this.helperUsers.Count() > 0)
            {
                taskTemplateUsersHelper = helperUsers.Select(a => new FTM_TaskTemplateUserHelper
                {
                    createdby = userId,
                    created = (DateTime.Now),
                    userId = a,
                    taskTemplateId = this.id,
                }).ToList();
            }

            if (this.PRD_Inventory_Ids.Count() > 0)
            {
                taskTemplateInventories = this.PRD_Inventory_Ids.Select(a => new FTM_TaskTemplateInventories
                {
                    createdby = userId,
                    created = DateTime.Now,
                    inventoryId = a,
                    taskTemplateId = this.id
                }).ToList();
            }

            var rs = new ResultStatus { result = true };


            rs &= db.InsertFTM_TaskTemplate(this.B_ConvertType<FTM_TaskTemplate>(), trans);
            rs &= db.BulkInsertFTM_TaskTemplateUser(taskTemplateUsers, trans);
            rs &= db.BulkInsertFTM_TaskTemplateUserHelper(taskTemplateUsersHelper, trans);
            rs &= db.BulkInsertFTM_TaskTemplateInventories(taskTemplateInventories, trans);

            if (this.FTM_TaskTemplateSubjectTypeIds != null && this.FTM_TaskTemplateSubjectTypeIds.Count() > 0)
            {
                var taskTypeKeyList = this.FTM_TaskTemplateSubjectTypeIds.Select(x => new FTM_TaskTemplateSubjectType
                {
                    created = DateTime.Now,
                    createdby = this.createdby,
                    subjectId = x,
                    taskTemplateId = this.id,
                }).ToList();

                rs &= db.BulkInsertFTM_TaskTemplateSubjectType(taskTypeKeyList, trans);

            }

            if (rs.result)
            {
                if (_trans == null) trans.Commit();
                rs.message = "Görev Şablonu başarıyla oluşturuldu.";
            }
            else
            {
                if (_trans == null) trans.Rollback();
                rs.message = "Görev Şablonu oluşturulamadı.";
            }

            return rs;

        }

        public ResultStatus Delete(DbTransaction _trans = null)
        {

            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? this.db.BeginTransaction();

            this.Load();

            var rs = db.BulkDeleteFTM_TaskTemplateUser(this.taskTemplateUsers, trans);
            rs &= db.BulkDeleteFTM_TaskTemplateUserHelper(this.taskTemplateUsersHelper, trans);
            rs &= db.BulkDeleteFTM_TaskTemplateInventories(this.taskTemplateInventories, trans);
            rs &= db.BulkDeleteFTM_TaskTemplateSubjectType(this.taskTemplateSubjectType, trans);
            rs &= db.DeleteFTM_TaskTemplate(this.B_ConvertType<FTM_TaskTemplate>(), trans);

            if (rs.result)
            {
                if (_trans == null) trans.Commit();
                rs.message = "Görev Şablonu başarıyla silindi.";
            }
            else
            {
                if (_trans == null) trans.Rollback();
                rs.message = "Görev Şablonu silinemedi.";
            }

            return rs;

        }

    }
}
