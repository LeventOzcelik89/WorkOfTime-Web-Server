using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMPDS_FormPermitTaskModel : VWPDS_FormPermitTask
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public int filterType { get; set; }
        public VWPDS_FormPermitTaskUser[] TaskUsers { get; set; }
        public Guid[] evaluatorIds { get; set; }
        public Guid[] evaluatedUserIds { get; set; }

        public VMPDS_FormPermitTaskModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var task = db.GetVWPDS_FormPermitTaskById(this.id);

            if (task != null)
            {
                this.B_EntityDataCopyForMaterial(task, true);
                this.TaskUsers = db.GetVWPDS_FormPermitTaskUserByTaskId(this.id).ToArray();
                this.evaluatorIds = this.TaskUsers.Where(a => a.evaluatorId.HasValue).Select(a => a.evaluatorId.Value).ToArray();

                if (this.TaskUsers.Where(a => a.evaluatedUserId == a.evaluatorId).Count() == this.TaskUsers.Count())
                {
                    this.filterType = (int)EnumPDS_FormPermitTaskFilterType.Own;
                }
                else
                {
                    this.filterType = (int)EnumPDS_FormPermitTaskFilterType.Personel;
                    this.evaluatedUserIds = this.TaskUsers.Where(a => a.evaluatedUserId.HasValue).Select(a => a.evaluatedUserId.Value).ToArray();
                }
            }
            else
            {
                this.status = true;
            }

            return this;
        }

        public ResultStatus Save(Guid userId, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var task = db.GetPDS_FormPermitTaskById(this.id);
            var rs = new ResultStatus { result = true };

            if (this.evaluatorIds == null || this.evaluatorIds.Length == 0)
            {
                return new ResultStatus { result = false, message = "Lütfen değerlendirici personel seçimi yapınız." };
            }

            if (this.filterType == (int)EnumPDS_FormPermitTaskFilterType.Personel && (this.evaluatedUserIds == null || this.evaluatedUserIds.Length == 0))
            {
                return new ResultStatus { result = false, message = "Lütfen değerlendirilen personel seçimi yapınız." };
            }

            if (task == null)
            {
                this.createdby = userId;
                this.created = DateTime.Now;
                rs = Insert(trans);
            }
            else
            {
                this.changedby = userId;
                this.changed = DateTime.Now;
                rs = Update(trans);
            }

            return rs;
        }

        private ResultStatus Insert(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var task = new PDS_FormPermitTask().B_EntityDataCopyForMaterial(this, true);

            var insertList = GetTaskUsers();

            var rs = db.InsertPDS_FormPermitTask(task, transaction);
            rs &= db.BulkInsertPDS_FormPermitTaskUser(insertList, transaction);

            if (rs.result == true)
            {
                if (trans == null)
                    transaction.Commit();
                return new ResultStatus { result = true, message = "Form Değerlendirme Aralığı Oluşturma işlemi başarılı." };
            }
            else
            {
                if (trans == null)
                    transaction.Rollback();
                return new ResultStatus { result = false, message = "Form Değerlendirme Aralığı Oluşturma işlemi başarısız." };
            }
        }

        private ResultStatus Update(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var transaction = trans ?? db.BeginTransaction();
            var task = new PDS_FormPermitTask().B_EntityDataCopyForMaterial(this, true);

            var taskUsers = db.GetPDS_FormPermitTaskUserByTaskId(this.id);
            var insertList = GetTaskUsers();

            var rs = db.UpdatePDS_FormPermitTask(task, false, transaction);
            rs &= db.BulkDeletePDS_FormPermitTaskUser(taskUsers, transaction);
            rs &= db.BulkInsertPDS_FormPermitTaskUser(insertList, transaction);

            if (rs.result == true)
            {
                if (trans == null)
                    transaction.Commit();
                return new ResultStatus { result = true, message = "Form Değerlendirme Aralığı Güncelleme işlemi başarılı." };
            }
            else
            {
                if (trans == null)
                    transaction.Rollback();
                return new ResultStatus { result = false, message = "Form Değerlendirme Aralığı Güncelleme işlemi başarısız." };
            }
        }

        public List<PDS_FormPermitTaskUser> GetTaskUsers()
        {
            var insertList = new List<PDS_FormPermitTaskUser>();

            if (this.filterType == (int)EnumPDS_FormPermitTaskFilterType.Personel)
            {
                foreach (var user in this.evaluatorIds)
                {
                    foreach (var eval in this.evaluatedUserIds)
                    {
                        var s = new PDS_FormPermitTaskUser
                        {
                            id = Guid.NewGuid(),
                            formPermitTaskId = this.id,
                            evaluatedUserId = eval,
                            evaluatorId = user,
                            created = DateTime.Now,
                            createdby = this.createdby
                        };

                        insertList.Add(s);
                    }
                }
            }

            if (this.filterType == (int)EnumPDS_FormPermitTaskFilterType.Own)
            {
                foreach (var eval in this.evaluatorIds)
                {
                    var s = new PDS_FormPermitTaskUser
                    {
                        id = Guid.NewGuid(),
                        formPermitTaskId = this.id,
                        evaluatedUserId = eval,
                        evaluatorId = eval,
                        created = DateTime.Now,
                        createdby = this.createdby
                    };

                    insertList.Add(s);
                }
            }

            return insertList;
        }

        public ResultStatus Delete(DbTransaction trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = trans ?? db.BeginTransaction();

            if (this.evaluateFormId.HasValue)
            {
                var formResults = this.db.GetVWPDS_FormResultByFormId(this.evaluateFormId.Value);

                if (formResults.Count() > 0)
                {
                    return new ResultStatus { result = false, message = "Bu form ile değerlendirme yapıldığı için silme işlemi gerçekleştirilemez." };
                }
            }

            var rs = new ResultStatus { result = true };
            var taskUsers = db.GetPDS_FormPermitTaskUserByTaskId(this.id);
            rs &= this.db.BulkDeletePDS_FormPermitTaskUser(taskUsers, this.trans);
            rs &= this.db.DeletePDS_FormPermitTask(new PDS_FormPermitTask().B_EntityDataCopyForMaterial(this), this.trans);

            if (trans == null)
            {
                if (rs.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return rs;
        }
    }
}
