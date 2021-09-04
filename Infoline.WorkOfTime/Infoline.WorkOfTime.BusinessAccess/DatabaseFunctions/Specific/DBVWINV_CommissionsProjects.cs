using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public partial class WorkOfTimeDatabase
    {

        public VWINV_CommissionsProjects[] GetVWINV_CommissionsProjectsIds(Guid commissionsIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CommissionsProjects>().Where(a => a.IdCommissions == commissionsIds).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }
        public VWINV_CommissionsProjects[] GetVWINV_CommissionsProjectsByIdCommissions(Guid[] commissionsIds, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CommissionsProjects>().Where(a => a.IdCommissions.In(commissionsIds)).OrderByDesc(x => x.created).Execute().ToArray();
            }
        }

        public VWINV_CommissionsPersons[] GetVWINV_CommissionsPersonsByCalendar(DateTime start, DateTime end)
        {
            using (var db = GetDB())
            {
                return db.Table<VWINV_CommissionsPersons>().Where(c => c.StartDate >= start && c.EndDate <= end && (c.ApproveStatus == (int)EnumINV_CommissionsApproveStatus.Onaylandi || c.ApproveStatus == (int)EnumINV_CommissionsApproveStatus.IslakImzaYuklendi)).Execute().ToArray();
            }
        }

        public VWINV_CommissionsProjects[] GetVWINV_CommissionsProjectsByCommissionsId(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWINV_CommissionsProjects>().Where(a => a.IdCommissions == id).Execute<VWINV_CommissionsProjects>().Where(x => x.IdProject.HasValue).Select(x => new VWINV_CommissionsProjects
                {
                    id = x.IdProject.Value,
                    Project_Title = x.Project_Title,
                    IdProject = x.IdProject
                }).ToArray();
            }
        }

        public VWINV_CommissionsPersons[] GetVWINV_CommissionsPersonsByIdAll(Guid id, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))

            {
                return db.Table<VWINV_CommissionsPersons>().Where(a => a.id == id).Execute<VWINV_CommissionsPersons>().Where(x => x.IdUser.HasValue).Select(x => new VWINV_CommissionsPersons
                {
                    id = x.id,
                    Person_Title = x.Person_Title,
                    IsOwner = x.IsOwner,
                    CommissionsPersonsId = x.CommissionsPersonsId,
                    IdUser = x.IdUser.Value,
                    Files = x.Files
                }).ToArray();
            }
        }



    }
}
