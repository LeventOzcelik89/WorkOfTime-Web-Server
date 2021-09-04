using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMSH_GroupUsersModel : VWSH_GroupUsers
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }

        public Guid[] groupUserIds { get; set; }

        public VMSH_GroupUsersModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var groupUsers = db.GetVWSH_GroupUsersById(this.id);

            if (groupUsers != null)
            {
                this.B_EntityDataCopyForMaterial(groupUsers, true);
            }

            return this;
        }

        public ResultStatus Save(Guid userid, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var groupUsers = db.GetVWSH_GroupUsersById(this.id);
            var res = new ResultStatus { result = true };


            if (groupUsers == null)
            {
                this.createdby = userid;
                this.created = DateTime.Now;
                res = Insert(trans);
            }
            else
            {
                this.changedby = userid;
                this.changed = DateTime.Now;
                res = Update(trans);
            }
            if (request != null && res.result)
            {
                new FileUploadSave(request, this.id).SaveAs();
                new TableAdditionalPropertySave(request, this.id, "CMP_Company").SaveAs(this.changedby ?? this.createdby);
            }
            return res;
        }

        private ResultStatus Insert(DbTransaction trans = null)
        {
            var transaction = trans ?? db.BeginTransaction();

            if (this.groupUserIds == null || this.groupUserIds.Count() <= 0)
            {
                return new ResultStatus
                {
                    result = false,
                    message = "Lütfen personel seçiniz.."
                };
            }
           

            var dbresult = db.BulkInsertSH_GroupUsers(this.groupUserIds.Select(x => new SH_GroupUsers
            {
                groupId = this.groupId,
                userId = x,
                createdby = this.createdby
            }));

            if (!dbresult.result)
            {
                if (trans == null) transaction.Rollback();

                return new ResultStatus
                {
                    result = false,
                    message = "Kaydetme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "Kaydetme işlemi başarılı bir şekilde gerçekleştirildi."
                };
            }
        }
        private ResultStatus Update(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();

            var groupUser = new SH_GroupUsers
            {
                id = this.id,
                created = this.created,
                changed = this.changed,
                changedby = this.changedby,
                createdby = this.createdby,
                groupId = this.groupId,
                userId = this.userId
            };


            var dbresult = db.UpdateSH_GroupUsers(groupUser, true, this.trans);


            if (!dbresult.result)
            {

                return new ResultStatus
                {
                    result = false,
                    message = "İşletme güncelleme işlemi başarısız oldu."
                };
            }
            else
            {
                return new ResultStatus
                {
                    result = true,
                    message = "İşletme güncelleme işlemi başarılı şekilde gerçekleştirildi."
                };
            }
        }

        public ResultStatus Delete(DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            var _trans = trans ?? db.BeginTransaction();
            var user = db.GetSH_GroupUsersById(this.id);

            var dbresult = db.DeleteSH_GroupUsers(user);

            if (trans == null)
            {
                if (dbresult.result)
                {
                    _trans.Commit();
                    dbresult.message = "Silme İşlemi Başarılı";
                }
                else
                {
                    _trans.Rollback();
                    dbresult.message = "Silme İşlemi Başarısız";

                }
            }

            return dbresult;
        }
    }
}
