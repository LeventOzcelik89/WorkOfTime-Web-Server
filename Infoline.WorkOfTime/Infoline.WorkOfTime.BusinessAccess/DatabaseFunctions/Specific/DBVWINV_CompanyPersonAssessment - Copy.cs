using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infoline.Framework.Database;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Data.Common;
using Infoline.ProjectManagement.BusinessData;

namespace Infoline.ProjectManagement.BusinessAccess
{
    partial class ProjectManagementDatabase
    {
        //değerlendirecek olduğum formlar
        public VWINV_CompanyPersonAssessment[] GetVWINV_ExpectedMyAssessment(Guid IdUser, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonAssessment>().Where(a =>
                (a.Manager1Approval == IdUser && a.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.Yonetici1Degerlendirme))
                    .Execute().ToArray();
            }
        }
        //onayımın beklendiği değerlendirmeler
        public VWINV_CompanyPersonAssessment[] GetVWINV_ExpectedMyApprovalForAssessment(Guid IdUser, DbTransaction tran = null)
        {
            using (var db = GetDB(tran))
            {
                return db.Table<VWINV_CompanyPersonAssessment>().Where(a =>

                (a.Manager2Approval == IdUser && a.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.Yonetici2Degerlendirme) ||
                (a.GeneralManagerApproval == IdUser && a.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.GenelMudurDegerlendirme) ||
                (a.IkApproval == IdUser && a.ApproveStatus == (Int32)EnumINV_CompanyPersonAssessmentApproveStatus.IkDegerlendirme)
                
                ).Execute().ToArray();
            }
        }

    }
}
