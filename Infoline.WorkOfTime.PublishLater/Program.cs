using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Configuration;
using System.Linq;
using System.Net;

namespace Infoline.WorkOfTime.PublishLater
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Başladı.");
            var remoteCon = ConfigurationManager.AppSettings["RemoteConnection"];
            var connection = new Infoline.Framework.Helper.CryptographyHelper().Decrypt(remoteCon);
            var executes = TenantConfig.GetTenants();
            var refreshView = "";

            using (var _db = new InfolineDatabase(connection.Replace("IntranetManagement","WorkOfTime"), DatabaseType.Mssql))
            {
                var views = _db.ExecuteReader<sysobjects>("select name from sys.objects  AS so WHERE so.type = 'V'").ToList();
                refreshView = string.Join(" ", views.Select(x => "exec sp_refreshview '" + x.name + "';"));
            }

            foreach (var execute in executes)
            {
                if (!execute.TenantCode.HasValue)
                {
                    continue;
                }

                try
                {
                    using (var _db = new InfolineDatabase(connection.Replace("IntranetManagement", "WorkOfTime" + (execute.TenantCode == 1100 ? "" : execute.TenantCode.ToString())), DatabaseType.Mssql))
                    {
                        var res = _db.ExecuteNonQuery(refreshView);
                        if (res.result)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                        }
                        Console.WriteLine(res.result ? execute.TenantCode + " Refresh View Başarılı" : execute.TenantCode + " Refresh View Başarısız");

                    }
                }

                catch (System.Exception )
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(execute.TenantCode + " Refresh View Başarısız");
                }

                try
                {
                    using (var client = new WebClient())
                    {
                        var uri = execute.WebDomain.Split(',').FirstOrDefault() + "/Account/Execute";
                        string JsonData = client.DownloadString(new Uri(uri));
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(execute.TenantCode + " Execute Başarılı");
                    }

                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(execute.TenantCode + " Execute Başarısız");
                }

            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Bitti.");
            Console.ReadLine();

        }
        public class sysobjects
        {
            public string name { get; set; }
            public string object_id { get; set; }
            public string principal_id { get; set; }
            public string schema_id { get; set; }
            public string parent_object_id { get; set; }
            public string type { get; set; }
            public string type_desc { get; set; }
            public string create_date { get; set; }
            public string modify_date { get; set; }
            public string is_ms_shipped { get; set; }
            public string is_published { get; set; }
            public string is_schema_published { get; set; }

        }
    }
}
