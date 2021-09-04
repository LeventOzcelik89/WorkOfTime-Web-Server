using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class ActivityTypeModel
    {
        public string Value { get; set; }
        public string Icon { get; set; }
    }

    public class ActivityCalendarModel
    {
        public Guid id { get; set; }
        public Guid createdby { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public int? type { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string users { get; set; }
        public string color { get; set; }
        public string icon { get; set; }
    }

    public class VMAPM_ActivityModel : VWAPM_Activity
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public APM_ActivityAction[] Actions { get; set; }
        public APM_ActivityRelation[] Relations { get; set; }
        public APM_ActivityUser[] Users { get; set; }
        public Guid[] UserIds { get; set; }
        public Guid[] RelationIds { get; set; }

        public VMAPM_ActivityModel Load()
        {
            db = db ?? new WorkOfTimeDatabase();
            var activity = db.GetVWAPM_ActivityById(this.id);

            if (activity != null)
            {
                this.B_EntityDataCopyForMaterial(activity, true);
                this.Actions = db.GetAPM_ActivityActionByActivityId(this.id);
                this.Relations = db.GetAPM_ActivityRelationByActivityId(this.id);
                this.Users = db.GetAPM_ActivityUserByActivityId(this.id);
                this.UserIds = this.Users.Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToArray();
                this.RelationIds = this.Relations.Where(a => a.dataId.HasValue).Select(a => a.dataId.Value).ToArray();
            }
            else
            {
                this.id = Guid.NewGuid();

            }

            return this;
        }

        public ResultStatus Save(Guid? userId, HttpRequestBase request = null, DbTransaction _trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            this.trans = _trans ?? db.BeginTransaction();

            var rs = new ResultStatus { result = true };
            var activity = db.GetAPM_ActivityById(this.id);

            if (activity == null)
            {
                this.created = DateTime.Now;
                this.createdby = userId;
                rs = this.Insert();
            }
            else
            {
                this.changed = DateTime.Now;
                this.changedby = userId;
                rs = this.Update();
            }

            if (rs.result)
            {
                new FileUploadSave(request, this.id).SaveAs();
            }

            if (_trans == null)
            {
                if (rs.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return rs;
        }

        private ResultStatus Insert()
        {

            var newRelations = GetRelations();
            var newUsers = GetUsers();

            var action = new APM_ActivityAction
            {
                activityId = this.id,
                created = DateTime.Now,
                createdby = this.createdby,
                id = Guid.NewGuid(),
                description = "Aktivite oluşturuldu",
                type = (int)EnumAPM_ActivityActionType.YeniAktivite,
            };

            var rs = db.InsertAPM_Activity(new APM_Activity().B_EntityDataCopyForMaterial(this, true), this.trans);
            rs &= db.BulkInsertAPM_ActivityRelation(newRelations, this.trans);
            rs &= db.BulkInsertAPM_ActivityUser(newUsers, this.trans);
            rs &= db.InsertAPM_ActivityAction(action, this.trans);

            return rs;
        }

        private ResultStatus Update()
        {
            var newRelations = GetRelations();
            var newUsers = GetUsers();

            var oldRelations = db.GetAPM_ActivityRelationByActivityId(this.id);
            var oldUsers = db.GetAPM_ActivityUserByActivityId(this.id);

            var action = new APM_ActivityAction
            {
                activityId = this.id,
                created = DateTime.Now,
                createdby = this.createdby,
                id = Guid.NewGuid(),
                description = "Aktivite güncellendi",
                type = (int)EnumAPM_ActivityActionType.AktiviteDuzenleme,
            };

            var rs = db.UpdateAPM_Activity(new APM_Activity().B_EntityDataCopyForMaterial(this, true), false, this.trans);

            rs &= db.BulkDeleteAPM_ActivityRelation(oldRelations, this.trans);
            rs &= db.BulkDeleteAPM_ActivityUser(oldUsers, this.trans);

            rs &= db.BulkInsertAPM_ActivityRelation(newRelations, this.trans);
            rs &= db.BulkInsertAPM_ActivityUser(newUsers, this.trans);
            rs &= db.InsertAPM_ActivityAction(action, this.trans);

            return rs;
        }

        public ResultStatus Delete(DbTransaction _trans = null)
        {
            this.trans = _trans ?? db.BeginTransaction();

            var relations = db.GetAPM_ActivityRelationByActivityId(this.id);
            var users = db.GetAPM_ActivityUserByActivityId(this.id);
            var actions = db.GetAPM_ActivityActionByActivityId(this.id);

            var rs = db.BulkDeleteAPM_ActivityRelation(relations, this.trans);
            rs &= db.BulkDeleteAPM_ActivityUser(users, this.trans);
            rs &= db.BulkDeleteAPM_ActivityAction(actions, this.trans);
            rs &= db.DeleteAPM_Activity(this.id, this.trans);

            if (_trans == null)
            {
                if (rs.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return rs;
        }

        private APM_ActivityUser[] GetUsers()
        {
            this.UserIds = this.UserIds ?? new Guid[] { };
            return this.UserIds.Select(a => new APM_ActivityUser
            {
                activityId = this.id,
                userId = a,
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.createdby,
            }).ToArray();
        }

        private APM_ActivityRelation[] GetRelations()
        {
            if (this.RelationIds != null && this.RelationIds.Length > 0)
            {
                var relations = db.GetVWAPM_ActivityRelationTablesByIds(this.RelationIds);
                return relations.Select(a => new APM_ActivityRelation
                {
                    activityId = this.id,
                    id = Guid.NewGuid(),
                    created = DateTime.Now,
                    createdby = this.createdby,
                    dataId = a.id,
                    dataTable = a.dataTable
                }).ToArray();
            }

            return new List<APM_ActivityRelation>().ToArray();
        }
    }
}
