using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infoline.WorkOfTime.WebService.Models
{
    public class Login
    {
        public string username { get; set; }
        public string password { get; set; }
        public MB_MobileDevice device { get; set; }
    }

    public class AToken
    {
        public Guid? TicketId { get; set; }
        public Guid? DeviceId { get; set; }
    }
}