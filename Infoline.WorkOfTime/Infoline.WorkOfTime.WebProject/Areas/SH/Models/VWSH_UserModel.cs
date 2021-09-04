using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessAccess.Models;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.WebProject.Areas.INV.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Infoline.WorkOfTime.WebProject.Areas.SH.Models
{
    public class VWSH_UserModel
    {
        public VWSH_User VWSH_User { get; set; }
        public VWSH_PersonInformation VWSH_PersonInformation { get; set; }
        public VWINV_CompanyPersonAvailability[] VWINV_CompanyPersonAvailabilities { get; set; }

        public VWSH_UserModel(Guid UserId)
        {
            var db = new WorkOfTimeDatabase();
            this.VWSH_User = db.GetVWSH_UserById(UserId) ?? new VWSH_User();
            this.VWSH_PersonInformation = db.GetVWSH_PersonInformationByUserId(UserId);
            this.VWINV_CompanyPersonAvailabilities = new INV_CompanyPersonAvailabilityModel(UserId, new VWINV_CompanyPersonAvailability[] { }).GetDailySchemaByPerson();

         

        }

    
    }



    public class WMSH_User : SH_User
    {
        public SH_PersonInformation SH_PersonInformation { get; set; }
        public INV_CompanyPerson INV_CompanyPerson { get; set; }
        public INV_CompanyPersonSalary INV_CompanyPersonSalary { get; set; }
        public INV_CompanyPersonDepartments INV_CompanyPersonDepartments { get; set; }


        public WMSH_User()
        {
            this.INV_CompanyPersonDepartments = new INV_CompanyPersonDepartments();
        }

    }





}