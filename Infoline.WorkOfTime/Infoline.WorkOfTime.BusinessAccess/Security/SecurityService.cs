using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading;

namespace Infoline.WorkOfTime.Business.Security
{
    [Export(typeof(IService))]
    [ExportMetadata("ServiceType", typeof(ISecurityService))]
    public class SecurityService : ISecurityService
    {
        Timer _tickettimer;
        int _deleteTimeSpan = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;
        public SecurityService()
        {
            _tickettimer = new Timer(DeleteTickets, null, _deleteTimeSpan, Timeout.Infinite);
        }
        void DeleteTickets(object state)
        {
            _tickettimer.Change(Timeout.Infinite, Timeout.Infinite);
            try
            {
                using (var db = new WorkOfTimeDatabase().GetDB())
                {
                    var aa = db.Table<SH_Ticket>().Where(a => a.endtime < DateTime.Now).Execute().ToArray();

                    db.Table<SH_Ticket>().Where(a => a.endtime < DateTime.Now);
                    //db.ExecuteNonQuery("delete  from SH_Ticket  where endtime < {0}", DateTime.Now);
                }
            }
            catch
            {
            }
            _tickettimer.Change(_deleteTimeSpan, Timeout.Infinite);
        }
        public bool ChangePassword(string userid, string password, string newpassword)
        {
            throw new NotImplementedException();
        }
        public LoginResult Login(string tckimlikNo, string password, Guid? deviceId, string IPAddress)
        {
            var db = new WorkOfTimeDatabase();
            var login = db.Login(tckimlikNo, password, TenantConfig.Tenant.Config.LdapUrls);

            if (login.LoginResult == LoginResult.OK)
            {
                var ticketObject = db.GetSH_TicketById(login.ticketid);
                var user = db.GetSH_UserById(ticketObject.userid.Value);
                var ctx = new CallContext(ticketObject.id, user.id, deviceId, new PropertyBag(), tckimlikNo, string.Concat(user.firstname, " ", user.lastname), new string[0], new PropertyBag(), DateTime.Now.AddDays(1));
                ctx.Activate();
                if (ticketObject.endtime < DateTime.Now) { 
                    using (var d = db.GetDB())
                    {
                        d.ExecuteInsert(new SH_Ticket
                        {
                            id = ctx.TicketId,
                            userid = ctx.UserId,
                            createtime = DateTime.Now,
                            endtime = DateTime.Now.AddMinutes(TicketLife),
                            DeviceId = deviceId,
                            IP = IPAddress
                        });
                    }
                }
                return LoginResult.OK;
            }
            else
            {
                return login.LoginResult;
            }
        }
        public bool IsInRole(string userid, string role)
        {
            return true;
        }
        public void SaveTicket(CallContext ctx)
        {
            using (var db = new WorkOfTimeDatabase().GetDB())
            {
                db.ExecuteInsert(new SH_Ticket { id = ctx.TicketId, userid = ctx.UserId, endtime = DateTime.Now.AddMinutes(TicketLife) });
            }
        }
        public CallContext LoadTicket(Guid id, string path = "")
        {
            var db = new WorkOfTimeDatabase();
            using (var d = db.GetDB())
            {
                var ctx = d.Table<SH_Ticket>().Where(a => a.id == id && a.endtime > DateTime.Now).Execute().FirstOrDefault();
                if (ctx != null)
                {
                    var user = db.GetSH_UserById(ctx.userid.Value);
                    if (user != null)
                    {
                        return new CallContext(
                            id,
                            ctx.userid.Value,
                            ctx.DeviceId,
                            new PropertyBag(), user.loginname, user.firstname, new string[0], new PropertyBag(), Convert.ToDateTime(ctx.endtime));
                    }
                }
            }
            return null;
        }
        public void DeleteTicket(Guid id)
        {
            using (var db = new WorkOfTimeDatabase().GetDB())
            {
                db.Table<SH_Ticket>().Delete(a => a.id == id);
            }
        }
        public int TicketLife { get; set; }
    }
}

