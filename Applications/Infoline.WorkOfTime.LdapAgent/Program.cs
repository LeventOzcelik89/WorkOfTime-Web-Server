using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.LdapAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var Url = ConfigurationManager.AppSettings.Get("Url");
            var Username = ConfigurationManager.AppSettings.Get("Username");
            var Password = ConfigurationManager.AppSettings.Get("Password");
            var DistinguishedName = ConfigurationManager.AppSettings.Get("DistinguishedName");
            var Filter = ConfigurationManager.AppSettings.Get("Filter");
            var Type = ConfigurationManager.AppSettings.Get("Type");


            if (Type == "2")
            {
                PrincipalContext context = new PrincipalContext(ContextType.Domain, Url, null, ContextOptions.SecureSocketLayer | ContextOptions.Negotiate, Username, Password);
                try
                {
                    using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                    {

                        foreach (var result in searcher.FindAll())
                        {
                            var ccc = result.GetUnderlyingObject();
                            DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;

                            Console.WriteLine(JsonConvert.SerializeObject(de));
                            Console.WriteLine("Bağlandım");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Bağlanamadım Mesaj:" + ex.Message);
                }
            }



            if (Type == "1")
            {
                using (var con = new LdapConnection(Url))
                {
                    con.AuthType = AuthType.Basic;
                    con.SessionOptions.ProtocolVersion = 3;
                    con.SessionOptions.SecureSocketLayer = true;

                    try
                    {
                        con.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback((conect, cer) => true);
                        con.Bind(new NetworkCredential(Username, Password, "fbu.edu.tr"));
                        //var request = new SearchRequest(DistinguishedName, "(&(objectCategory=Person)(cn=*))", System.DirectoryServices.Protocols.SearchScope.Subtree);
                        //var response = (SearchResponse)con.SendRequest(request);
                        //Console.WriteLine(JsonConvert.SerializeObject(response));
                        Console.WriteLine("Bağlandım");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Bağlanamadım Mesaj:" + ex.Message);
                    }
                }
            }



            if (Type == "0")
            {

                DirectoryEntry ldapConnection = new DirectoryEntry();
                ldapConnection.Path = "LDAP://" + Url + DistinguishedName;
                ldapConnection.Username = Username;
                ldapConnection.Password = Password;
                DirectorySearcher search = new DirectorySearcher(ldapConnection);
                if (Filter != null)
                {
                    search.Filter = "(&(objectCategory=Person)(cn=*))";
                }
                SearchResultCollection src = search.FindAll();
                var list = new List<Dictionary<string, object>>();
                foreach (SearchResult sr in src)
                {
                    try
                    {
                        using (DirectoryEntry entry = new DirectoryEntry(sr.Path))
                        {
                            var user = new Dictionary<string, object>();
                            if (user != null && entry.Properties != null)
                            {
                                foreach (string prop in entry.Properties.PropertyNames)
                                {
                                    try
                                    {
                                        user.Add(prop, entry.Properties[prop][0]);
                                    }
                                    catch { }
                                }
                            }
                            list.Add(user);
                        }
                    }
                    catch { }
                }
                Console.WriteLine(JsonConvert.SerializeObject(list));

            }

            Console.ReadLine();
        }
    }
}
