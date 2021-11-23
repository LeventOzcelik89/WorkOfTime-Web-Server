using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infoline.ProjectManagement.WebService.Models
{
    public class GuhemWeb
    {
        public string SchoolName { get; set; }
        public string ResponsibleName { get; set; }
        public string ResponsibleLastName { get; set; }
        public string ResponsiblePhone { get; set; }
        public string ResponsibleEmail { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int ParticipantCount { get; set; }
        public string RangeOfAge { get; set; }

    }
}