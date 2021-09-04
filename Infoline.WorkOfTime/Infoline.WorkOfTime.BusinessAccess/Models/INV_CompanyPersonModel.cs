using Infoline.WorkOfTime.BusinessData;
using System;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class INV_CompanyPersonModel
    {

        public VWSH_User VWSH_User { get; set; }
        public VWSH_PersonInformation VWSH_PersonInformation { get; set; }
        public VWINV_CompanyPerson VWINV_CompanyPerson { get; set; }
        public VWINV_CompanyPersonDepartments VWINV_CompanyPersonDepartments { get; set; }
        public VWINV_CompanyPersonDepartments[] VWINV_CompanyPersonProjectDepartments { get; set; }
        public VWPRJ_Project[] VWPRJ_Project { get; set; }
        public VWINV_CompanyPersonDepartments[] VWINV_CompanyPersonDepartmentsInUser { get; set; }

        public INV_CompanyPersonModel(Guid PersonId)
        {
            var db = new WorkOfTimeDatabase();

            this.VWINV_CompanyPerson = db.GetVWINV_CompanyPersonById(PersonId) ?? new VWINV_CompanyPerson();
            this.VWSH_User = db.GetVWSH_UserById(VWINV_CompanyPerson.IdUser != null ? VWINV_CompanyPerson.IdUser.Value : PersonId) ?? new VWSH_User();
            this.VWSH_PersonInformation = db.GetVWSH_PersonInformationByUserId(VWINV_CompanyPerson.IdUser != null ? VWINV_CompanyPerson.IdUser.Value : PersonId)
                ?? new VWSH_PersonInformation();
            this.VWINV_CompanyPersonDepartments = db.GetVWINV_CompanyPersonDepartmentsByUserIdAndEndDateNull(PersonId) ?? new VWINV_CompanyPersonDepartments();
            this.VWINV_CompanyPersonProjectDepartments = db.GetVWINV_CompanyPersonDepartmentsProjects(PersonId);
            this.VWINV_CompanyPersonDepartmentsInUser = db.GetVWINV_CompanyPersonDepartmentsByIdUserManager(PersonId);
        }

    }
}