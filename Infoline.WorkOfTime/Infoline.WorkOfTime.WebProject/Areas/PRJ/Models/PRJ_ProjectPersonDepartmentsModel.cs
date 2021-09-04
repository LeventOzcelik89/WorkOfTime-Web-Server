using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebProject.Areas.PRJ.Models
{
    public class PRJ_ProjectPersonDepartmentsModel
    {
        public VWINV_CompanyPersonDepartments[] GetMyProject(Guid? IdUser)
        {
            var db = new WorkOfTimeDatabase();
            var userStatus = (PageSecurity)HttpContext.Current.Session["userStatus"];
            var list = new List<ProjectPersonDepartments>();
            var data = db.GetVWINV_CompanyPersonDepartmentsByIdUser(IdUser.Value).Where(c => c.OrganizationType == (Int32)EnumINV_CompanyDepartmentsType.Matrix);
            foreach (var item in data)
            {
                var project = db.GetVWPRJ_ProjectById(item.ProjectId.Value);

                var datas = new ProjectPersonDepartments
                {
                    id = project.id,
                    IdUser = IdUser.Value,
                    ProjectType_Title = project.ProjectType_Title,
                    ProjectCode = project.ProjectCode,
                    ProjectName = project.ProjectName,
                    ProjectScope = project.ProjectScope,
                    ProjectStartDate = project.ProjectStartDate.Value,
                    ProjectEndDate = project.ProjectEndDate.Value,
                    ProjectPersonCount = project.ProjectPersonCount.Value,
                    ProjectTechnicalType = project.ProjectTechnicalType
                };

                list.Add(datas);
            }

            return list.ToArray();

        }
    }
    public class ProjectPersonDepartments : VWINV_CompanyPersonDepartments
    {
        public string ProjectType_Title { get; set; }
        public string ProjectCode { get; set; }
        public string ProjectName { get; set; }
        public string ProjectScope { get; set; }
        public DateTime ProjectStartDate { get; set; }
        public DateTime ProjectEndDate { get; set; }
        public int ProjectPersonCount { get; set; }
        public string ProjectTechnicalType { get; set; }


    }

}



