using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.WorkOfTime.BusinessData;
using System.ComponentModel;

namespace Infoline.WorkOfTime.BusinessAccess
{

    [EnumInfo(typeof(INV_CompanyDepartments), "Type")]
    public enum EnumINV_CompanyDepartmentsType
    {
        [Description("Genel Organizasyon Şeması")]
        Organization = 0,
        [Description("Proje Matrix Şema")]
        Matrix = 1,
        //[Description("Şirket İçi Şema")]
        //Company = 2,
    }
    
    public partial class WorkOfTimeDatabase
    {


        public INV_CompanyDepartments GetINV_CompanyDepartmentByType(EnumINV_CompanyDepartmentsType type, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CompanyDepartments>().Where(a => a.Type == (int)type && a.PID == null).OrderBy(a => a.created).Execute().FirstOrDefault();
            }
        }


        public INV_CompanyDepartments[] GetINV_CompanyDepartmentByProjectId(Guid projectId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CompanyDepartments>().Where(a => a.ProjectId == projectId && a.Type == (int)EnumINV_CompanyDepartmentsType.Matrix).OrderBy(a => a.created).Execute().ToArray();
            }
        }

        public INV_CompanyDepartments GetINV_CompanyDepartmentsByProjectId(Guid projectId, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<INV_CompanyDepartments>().Where(a => a.ProjectId == projectId && a.Type == (int)EnumINV_CompanyDepartmentsType.Matrix).OrderBy(a => a.created).Execute().FirstOrDefault();
            }
        }

    }

}
