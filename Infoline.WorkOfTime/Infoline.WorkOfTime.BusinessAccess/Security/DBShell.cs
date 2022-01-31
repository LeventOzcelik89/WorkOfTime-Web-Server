using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public partial class WorkOfTimeDatabase
    {
        public SH_User GetSH_UserInfoByEmail(string email)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(a => a.email == email).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }

        public SH_User[] GetSH_UserInfosByEmail(string email)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(a => a.email == email).OrderByDesc(a => a.created).Execute().ToArray();
            }
        }
        public SH_User GetSH_UserByLoginName(string loginname)
        {
            using (var db = GetDB())
            {
                return db.Table<SH_User>().Where(a => a.loginname == loginname).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }
        public VWSH_User GetVWSH_UserByLoginName(string loginname)
        {
            using (var db = GetDB())
            {
                return db.Table<VWSH_User>().Where(a => a.loginname == loginname || a.IdentificationNumber == loginname || a.email == loginname || a.email.Contains(loginname + "@") || a.IdentificationNumber == loginname).OrderByDesc(a => a.created).Execute().FirstOrDefault();
            }
        }
        public string GetMd5Hash(string input)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
        public LoginStatus Login(string userName, string password, string[] ldapUrls)
        {
            var passwordMd5 = GetMd5Hash(GetMd5Hash(password));
            var user = GetVWSH_UserByLoginName(userName);
            var res = new LoginStatus();

            if (user == null)
            {
                return new LoginStatus { LoginResult = LoginResult.InvalidUser };
            }

            if (user.status == null || user.status == false)
            {
                return new LoginStatus { LoginResult = LoginResult.AccountDisabled };
            }

            if (!(!string.IsNullOrEmpty(password) && (password == user.password || GetMd5Hash(password) == user.password || GetMd5Hash(GetMd5Hash(password)) == user.password)))
            {

                if (ldapUrls.Length > 0)
                {
                    var loginname = (user.email ?? "").Split('@').FirstOrDefault();
                    var rs = new ResultStatus { result = false };
                    foreach (var ldapUrl in ldapUrls)
                    {
                        if (rs.result == true)
                        {
                            continue;
                        }
                        else
                        {
                            rs = LoginLdap(loginname, password, ldapUrl);
                        }
                    }

                    if (rs.result == false)
                    {
                        return new LoginStatus { LoginResult = LoginResult.InvalidPassword };
                    }

                }
                else
                {
                    return new LoginStatus { LoginResult = LoginResult.InvalidPassword };
                }
            }

            using (var db = GetDB())
            {
                var ticket = new SH_Ticket
                {
                    id = Guid.NewGuid(),
                    userid = user.id,
                    createtime = DateTime.Now,
                    endtime = DateTime.Now.AddMinutes(30),
                    IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                };
                var result = db.ExecuteInsert(ticket);
                res.LoginResult = LoginResult.OK;
                res.ticketid = ticket.id;
            }

            return res;

        }
        public LoginStatus Login(string userName, string password)
        {

            var passwordMd5 = GetMd5Hash(GetMd5Hash(password));
            var user = GetVWSH_UserByLoginName(userName);
            var res = new LoginStatus();

            if (user == null)
            {
                return new LoginStatus { LoginResult = LoginResult.InvalidUser };
            }

            if (user.status.HasValue == false || (user.status.HasValue == true && user.status.Value == false))
            {
                return new LoginStatus { LoginResult = LoginResult.AccountDisabled };
            }


            if (!(!string.IsNullOrEmpty(password) && (password == user.password || GetMd5Hash(password) == user.password || GetMd5Hash(GetMd5Hash(password)) == user.password)))
            {
                return new LoginStatus { LoginResult = LoginResult.InvalidPassword };
            }


            using (var db = GetDB())
            {

                var ticket = new SH_Ticket
                {
                    id = Guid.NewGuid(),
                    userid = user.id,
                    createtime = DateTime.Now,
                    endtime = DateTime.Now.AddMinutes(30),
                    IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]
                };

                var result = db.ExecuteInsert(ticket);
                res.LoginResult = LoginResult.OK;
                res.ticketid = ticket.id;
            }

            return res;

        }
        public PageSecurity GetUserPageSecurityByticketid(Guid ticketid)
        {
            if (ticketid == null)
            {
                return null;
            }
            using (var db = GetDB())
            {
                var ticket = db.Table<SH_Ticket>().Where(a => a.id == ticketid).OrderByDesc(a => a.createtime).Take(1).Execute().FirstOrDefault();
                return GetUserPageSecurityByUserid(ticket.userid, ticket.id);
            }
        }
        public PageSecurity GetUserPageSecurityByUserid(Guid? userId, Guid ticketId)
        {
            var pages = PageInfo.GetPages();

            if (pages.Count() == 0)
            {
                pages = GetSH_Pages().ToList();
            }



            var pageSecurity = new PageSecurity();
            using (var db = GetDB())
            {
                var user = db.Table<VWSH_User>().Where(a => a.id == userId).Execute().FirstOrDefault();
                var userRoles = db.Table<SH_UserRole>().Where(a => a.userid == userId).OrderByDesc(a => a.created).Execute().ToList();
                var roleIds = userRoles.Where(a => a.roleid.HasValue).Select(a => a.roleid.Value).ToList();


                if (userRoles.Count() > 0)
                {
                    var rolePages = db.Table<SH_PagesRole>().Where(a => a.roleid.In(roleIds.ToArray())).Execute().Select(a => a.action).Distinct().ToList();
                    pageSecurity.UnAuthorizedPage = pages.Where(a => a.AllowEveryone == false && !rolePages.Contains(a.Action)).Select(a => a.Action).ToArray();
                }
                else
                {
                    pageSecurity.UnAuthorizedPage = pages.Where(a => a.AllowEveryone == false).Select(a => a.Action).ToArray();
                }

                pageSecurity.user = user;
                pageSecurity.ticketid = ticketId;
                pageSecurity.AuthorizedRoles = roleIds.ToArray();
                pageSecurity.FilesRole = db.Table<SH_FilesRole>().Where(w => w.roleid.In(roleIds.ToArray())).Execute().ToArray();
                pageSecurity.ChildPersons = new ManagersCalculator().GetAllChilds(userId.Value);
                pageSecurity.UserInformation = db.Table<VWSH_PersonInformation>().Where(a => a.UserId == user.id).Execute().FirstOrDefault() ?? new VWSH_PersonInformation();
                return pageSecurity;
            }
        }

        public PageSecurityForMobile GetUserPageSecurityByUseridForMobile(Guid? userId, Guid ticketId)
        {
            var pages = PageInfo.GetPages();

            if (pages.Count() == 0)
            {
                pages = GetSH_Pages().ToList();
            }

            var pageSecurity = new PageSecurityForMobile();
            using (var db = GetDB())
            {
                var user = db.Table<VWSH_User>().Where(a => a.id == userId).Execute().FirstOrDefault();
                var userRoles = db.Table<SH_UserRole>().Where(a => a.userid == userId).OrderByDesc(a => a.created).Execute().ToList();
                var roleIds = userRoles.Where(a => a.roleid.HasValue).Select(a => a.roleid.Value).ToList();

                if (userRoles.Count() > 0)
                {
                    var rolePages = db.Table<SH_PagesRole>().Where(a => a.roleid.In(roleIds.ToArray())).Execute().Select(a => a.action).Distinct().ToList();
                    pageSecurity.UnAuthorizedPage = pages.Where(a => a.AllowEveryone == false && !rolePages.Contains(a.Action)).Select(a => a.Action).ToArray();
                }
                else
                {
                    pageSecurity.UnAuthorizedPage = pages.Where(a => a.AllowEveryone == false).Select(a => a.Action).ToArray();
                }

                pageSecurity.user = user;
                pageSecurity.AuthorizedRoles = roleIds.ToArray();
                return pageSecurity;
            }
        }

        public ResultStatus LoginLdap(string userName, string password, string Url)
        {
            var result = new ResultStatus { result = false };
            try
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain, Url, null, ContextOptions.SecureSocketLayer | ContextOptions.Negotiate, userName, password);
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    result.result = true;
                    result.message = "Bağlantı kuruldu.";
                }
            }
            catch (Exception ex)
            {
                result.result = false;
                result.message = "Bağlantı kurulamadı.Mesaj:" + ex.Message;
            }
            return result;
        }
    }
}
