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

    public class VWAgileBoardDashboardModel
    {
        private WorkOfTimeDatabase db { get; set; }
        private DbTransaction trans { get; set; }

        public Guid? id { get; set; }

        public VWSH_AgileBoards AgileBoard { get; set; } = new VWSH_AgileBoards();
        public List<VWSH_AgileBoards> MyAgileBoards { get; set; } = new List<VWSH_AgileBoards>();
        public List<VWCRM_ManagerStage> Stages { get; set; } = new List<VWCRM_ManagerStage>();
        public List<Guid> SalesPersons { get; set; } = new List<Guid> { };

        public VWAgileBoardDashboardModel Load(Guid userId)
        {
            this.db = this.db ?? new WorkOfTimeDatabase();

            this.Stages = db.GetVWCRM_ManagerStage().ToList();
            this.MyAgileBoards = db.GetVWSH_AgileBoardsByUserId(userId).OrderByDescending(a => a.lastUsedDate).ToList();

            var satisPersonelleri = db.GetUserByRoleId(SHRoles.SatisPersoneli).ToList();
            this.SalesPersons = db.GetVWSH_UserMyPersonIsWorkingIDS(satisPersonelleri).Select(a => a.id).ToList();
            this.SalesPersons.Add(userId);

            if (this.id.HasValue)
            {
                this.AgileBoard = this.MyAgileBoards.Where(a => a.id == this.id).FirstOrDefault();
            }
            else
            {
                this.AgileBoard = this.MyAgileBoards.OrderByDescending(a => a.lastUsedDate).FirstOrDefault();
            }

            return this;
        }

    }
}
