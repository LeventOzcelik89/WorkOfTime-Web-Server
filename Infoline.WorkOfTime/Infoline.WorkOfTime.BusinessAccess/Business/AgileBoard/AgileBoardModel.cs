using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.BusinessAccess.Business.AgileBoard
{
    public class AgileBoardModel<T>
    {

        public List<VWSH_AgileBoards> MyAgileBoards { get; set; }
        public VWSH_AgileBoards AgileBoard { get; set; }

        private WorkOfTimeDatabase db { get; set; }
        private PageSecurity userStatus { get; set; }

        public T Model { get; set; }

        public AgileBoardModel(PageSecurity _userStatus, Guid agileBoardId)
        {

            this.userStatus = _userStatus;

            this.db = this.db ?? new WorkOfTimeDatabase();
            this.MyAgileBoards = db.GetVWSH_AgileBoardsByUserId(this.userStatus.user.id).ToList();
            this.AgileBoard = this.MyAgileBoards.Where(a => a.id == agileBoardId).FirstOrDefault();

        }

        public void Modell()
        {

            this.Model.GetType().GetProperties();

        }

    }
}
