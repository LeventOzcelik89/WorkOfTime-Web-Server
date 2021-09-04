using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infoline.ProjectManagement
{
    public class ServiceModel
    {
        public static PageSecurity GetPageSecurity()
        {
            var userId = CallContext.Current.UserId;
            var db = new WorkOfTimeDatabase();
            var user = db.GetVWSH_UserById(userId);
            return new PageSecurity
            {
                user = user,
                AuthorizedRoles = user.RoleIds.Split(',').Select(a => new Guid(a)).ToArray()
            };

        }

    }
}