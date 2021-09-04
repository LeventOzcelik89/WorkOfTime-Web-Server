using Infoline.WorkOfTime.BusinessData;
using System;

namespace Infoline.WorkOfTime.BusinessAccess
{

    public class PageSecurity
    {
        public Guid ticketid { get; set; }
        public VWSH_User user { get; set; }
        public VWSH_PersonInformation UserInformation { get; set; }
        public string[] UnAuthorizedPage { get; set; }
        public Guid[] AuthorizedRoles { get; set; }
        public SH_FilesRole[] FilesRole { get; set; }
        public VWINV_CompanyPersonDepartments[] ChildPersons { get; set; }
    }

    public class LoginStatus
    {
        public LoginResult LoginResult { get; set; }
        public Guid ticketid { get; set; }
    }
}
