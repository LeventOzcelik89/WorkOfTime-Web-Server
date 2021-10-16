using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public class SpesificSuggestModal
    {
        public string title { get; set; }
        public string content { get; set; }
    }

    public class VMHDM_SuggestionModel : VWHDM_Suggestion
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }

        public VMHDM_SuggestionModel Load()
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var suggestion = db.GetVWHDM_SuggestionById(this.id);

            if (suggestion != null)
            {
                this.B_EntityDataCopyForMaterial(suggestion, true);
            }

            return this;
        }

        public ResultStatus GetByKeyword(string keyword)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            var suggestion = db.GetVWHDM_SuggestionByTextJustTitleAndContent(keyword);
            foreach (var item in suggestion)
            {
                item.content = Regex.Replace(item.content, "<p>", "½$#");
                item.content = Regex.Replace(item.content, "<.*?>", String.Empty);
                item.content = Regex.Replace(item.content, "\r", String.Empty);
                item.content = Regex.Replace(item.content, "\n", String.Empty);
            }
            return new ResultStatus { objects = suggestion, result = true, message = "" };
        }

        public ResultStatus Save(Guid userid, HttpRequestBase request = null, DbTransaction trans = null)
        {
            db = db ?? new WorkOfTimeDatabase();
            this.trans = trans ?? db.BeginTransaction();
            var suggestion = db.GetVWHDM_SuggestionById(this.id);

            var res = new ResultStatus { result = true };

            if (suggestion == null)
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

        private ResultStatus Insert(DbTransaction trans)
        {
            var rs = this.db.InsertHDM_Suggestion(new HDM_Suggestion().B_EntityDataCopyForMaterial(this, true), trans);
            rs &= SetAssignUserRoles(trans);
            return rs;
        }

        private ResultStatus Update(DbTransaction trans)
        {
            var rs = this.db.UpdateHDM_Suggestion(new HDM_Suggestion().B_EntityDataCopyForMaterial(this, true), false, trans);
            rs &= SetAssignUserRoles(trans);
            return rs;
        }

        private ResultStatus SetAssignUserRoles(DbTransaction trans)
        {
            var rs = new ResultStatus { result = true };

            if (this.assignUserId.HasValue)
            {
                var roles = this.db.GetSH_UserRoleByUserId(this.assignUserId.Value);
                var personelRole = new Guid(SHRoles.YardimMasaPersonel);
                var customerRole = new Guid(SHRoles.YardimMasaMusteri);
                var newManagerRoles = new List<SH_UserRole>();
                var userRoles = roles.Where(a => a.userid == this.assignUserId.Value && (a.roleid == personelRole || a.roleid == customerRole)).ToArray();

                if (userRoles.Count() == 0)
                {
                    var role = new SH_UserRole
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                        createdby = this.createdby,
                        roleid = personelRole,
                        userid = this.assignUserId
                    };

                    rs &= this.db.InsertSH_UserRole(role, trans);
                }
            }

            return rs;
        }

        public ResultStatus Delete(DbTransaction trans = null)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();
            this.trans = trans ?? this.db.BeginTransaction();
            var suggestion = db.GetHDM_SuggestionById(this.id);

            if (suggestion == null)
            {
                return new ResultStatus { message = "Çözüm önerisi zaten silinmiş durumda.", result = false };
            }

            var isUsedTicket = this.db.GetHDM_TicketBySuggestionId(this.id);

            if (isUsedTicket.Count() > 0)
            {
                return new ResultStatus { message = "Bu çözüm önerisiyle yardım talebi oluşturulmuş.", result = false };
            }

            var dbresult = this.db.DeleteHDM_Suggestion(suggestion, this.trans);

            if (trans == null)
            {
                if (dbresult.result)
                    this.trans.Commit();
                else
                    this.trans.Rollback();
            }

            return dbresult;
        }
    }


}
