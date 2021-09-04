using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class VMHDM_IssueSuggestionModel
    {
        public VWHDM_Issue[] Issues { get; set; }
        public VWHDM_Suggestion[] Suggestions { get; set; }
    }

    public class VMHDM_IssueModel : VWHDM_Issue
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }
        public VWHDM_IssueUser[] Users { get; set; }
        public Guid[] UserIds { get; set; }
        public string path { get; set; }
        public HDM_Issue[] AllIssuesDB { get; set; }

        public VMHDM_IssueModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var issue = db.GetVWHDM_IssueById(this.id);

            if (issue != null)
            {
                this.B_EntityDataCopyForMaterial(issue, true);

                if (this.mainId.HasValue)
                {
                    this.Users = this.db.GetVWHDM_IssueUserByIssueId(this.mainId.Value);
                    this.UserIds = this.Users.Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToArray();
                }
                else
                {
                    this.Users = this.db.GetVWHDM_IssueUserByIssueId(this.id);
                    this.UserIds = this.Users.Where(a => a.userId.HasValue).Select(a => a.userId.Value).ToArray();
                }
            }
            else
            {
                if (this.pid.HasValue)
                {
                    var parentIssue = db.GetHDM_IssueById(this.pid.Value);
                    this.mainId = parentIssue.mainId.HasValue ? parentIssue.mainId.Value : parentIssue.id;
                }
            }

            return this;
        }

        public ResultStatus Save(Guid userid, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            this.trans = trans ?? db.BeginTransaction();
            var issue = db.GetVWHDM_IssueById(this.id);

            var res = IsExistIssueProperty();

            if (res.result == false)
            {
                return res;
            }

            if (issue == null)
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
            }

            if (trans == null)
            {
                if (res.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return res;
        }

        public ResultStatus IsExistIssueProperty()
        {
            db = db ?? new WorkOfTimeDatabase();
            var rs = new ResultStatus { result = true };

            if (!String.IsNullOrEmpty(this.title))
            {
                var isExistIssueByTitle = db.GetHDM_IssueByTitle(this.title);

                if (isExistIssueByTitle != null && isExistIssueByTitle.id != this.id)
                {
                    rs.message = "Bu konu daha önce kaydedilmiştir. Lütfen kontrol ediniz.";
                    rs.result = false;
                    return rs;
                }
            }

            return rs;
        }

        private ResultStatus Insert(DbTransaction trans)
        {
            var rs = this.db.InsertHDM_Issue(new HDM_Issue().B_EntityDataCopyForMaterial(this, true), trans);

            if (this.UserIds != null && this.UserIds.Count() > 0)
            {
                var roles = GetManagerRoles();
                rs &= this.db.BulkInsertSH_UserRole(roles, trans);

                if (!this.mainId.HasValue)
                {
                    var newUsers = GetUsers();
                    rs &= this.db.BulkInsertHDM_IssueUser(newUsers, trans);
                }
            }

            return rs;
        }

        private ResultStatus Update(DbTransaction trans)
        {
            var rs = this.db.UpdateHDM_Issue(new HDM_Issue().B_EntityDataCopyForMaterial(this, true), false, trans);

            if (this.UserIds != null && this.UserIds.Count() > 0)
            {
                var roles = GetManagerRoles();
                rs &= this.db.BulkInsertSH_UserRole(roles, trans);

                if (!this.mainId.HasValue)
                {
                    var oldUser = this.db.GetHDM_IssueUserByIssueId(this.id);
                    rs &= this.db.BulkDeleteHDM_IssueUser(oldUser);
                    var newUsers = GetUsers();
                    rs &= this.db.BulkInsertHDM_IssueUser(newUsers, trans);
                }
            }

            return rs;
        }

        private HDM_IssueUser[] GetUsers()
        {
            return this.UserIds.Select(a => new HDM_IssueUser
            {
                issueId = this.id,
                userId = a,
                id = Guid.NewGuid(),
                created = DateTime.Now,
                createdby = this.createdby,
            }).ToArray();
        }

        private List<SH_UserRole> GetManagerRoles()
        {
            var roles = this.db.GetSH_UserRoleByUserIds(this.UserIds);
            var managerRole = new Guid(SHRoles.YardimMasaYonetim);

            var newManagerRoles = new List<SH_UserRole>();
            foreach (var userId in this.UserIds)
            {
                var userRoles = roles.Where(a => a.userid == userId && a.roleid == managerRole).ToArray();

                if (userRoles.Count() == 0)
                {
                    newManagerRoles.Add(new SH_UserRole
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = this.createdby,
                        roleid = managerRole,
                        userid = userId
                    });
                }
            }

            return newManagerRoles;
        }

        public ResultStatus Delete(Guid[] authorizedRoles, DbTransaction trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = trans ?? this.db.BeginTransaction();

            var issue = db.GetHDM_IssueById(this.id);
            if (issue == null)
            {
                return new ResultStatus { message = "Konu/Soru zaten silinmiş durumda.", result = false };
            }

            this.AllIssuesDB = db.GetHDM_Issue();

            var listIssues = new List<HDM_Issue>();
            listIssues.Add(issue);

            var subIssues = GetSubIssues(this.id, listIssues);
            var ticketsByIssueIds = this.db.GetHDM_TicketByIssueIds(subIssues.Select(a => a.id).ToArray());
            var suggestionByIssues = this.db.GetHDM_SuggestionByIssueIds(subIssues.Select(a => a.id).ToArray());
            var ticketsBySuggestions = this.db.GetHDM_TicketBySuggestionIds(suggestionByIssues.Select(a => a.id).ToArray());

            if (ticketsByIssueIds.Count() > 0)
            {
                return new ResultStatus { message = "Bu konu veya alt konular ile ilgili açılmış talep bulunmaktadır.", result = false };
            }

            if (ticketsBySuggestions.Count() > 0)
            {
                return new ResultStatus { message = "Bu konu veya alt konular ile ilgili çözüm önerilerinden biriyle açılmış talep bulunmaktadır.", result = false };
            }

            var issueUser = this.db.GetHDM_IssueUserByIssueIds(subIssues.Select(x => x.id).ToArray());

            var dbresult = this.db.BulkDeleteHDM_Suggestion(suggestionByIssues, this.trans);
            dbresult &= this.db.BulkDeleteHDM_IssueUser(issueUser, this.trans);
            dbresult &= this.db.BulkDeleteHDM_Issue(subIssues, this.trans);

            if (trans == null)
            {
                if (dbresult.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return dbresult;
        }

        public HDM_Issue[] GetSubIssues(Guid issueId, List<HDM_Issue> SubIssues)
        {
            var issues = this.AllIssuesDB.Where(a => a.pid == issueId).ToArray();
            SubIssues.AddRange(issues);

            foreach (var issue in issues)
            {
                GetSubIssues(issue.id, SubIssues);
            }

            return SubIssues.ToArray();
        }

        public string GetPath(Guid issueId)
        {
            var issue = db.GetHDM_IssueById(issueId);

            if (String.IsNullOrEmpty(this.path))
            {
                this.path = "<a href='/HDM/VWHDM_Issue/Detail?id=" + issue.id + "'>" + issue.title + "</a>";
            }
            else
            {
                this.path = "<a href='/HDM/VWHDM_Issue/Detail?id=" + issue.id + "'>" + issue.title + "</a>" + " / " + this.path;
            }

            if (issue.pid.HasValue)
            {
                return GetPath(issue.pid.Value);
            }

            return "<a href='/HDM/VWHDM_Issue/Index'>Ana Konular </a> / " + this.path;
        }
    }
}
