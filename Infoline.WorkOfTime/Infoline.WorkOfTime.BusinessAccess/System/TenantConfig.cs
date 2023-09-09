using Infoline.Framework.Database;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess
{
    public class TenantConfig
    {
        private static string _domain { get; set; }
        private static List<TEN_Tenant> _tenants { get; set; } = new List<TEN_Tenant>();
        public static TEN_Tenant Tenant
        {
            get
            {
                return GetTenantByTenantDomain(_domain);
            }
        }
        public TenantConfig(string domain = null)
        {
            _domain = domain.ToLowerInvariant();

        }
        public static List<TEN_Tenant> GetTenants()
        {
            if (_tenants.Count() == 0)
            {
                var remoteCon = ConfigurationManager.AppSettings["RemoteConnection"];
                var connection = new Infoline.Framework.Helper.CryptographyHelper().Decrypt(remoteCon);
                using (var db = new InfolineDatabase(connection, DatabaseType.Mssql))
                {
                    _tenants = db.ExecuteReader<TEN_Tenant>("select * from TEN_Tenant with(nolock)").Where(x => x.TenantCode != 1997 && x.TenantCode != 1998).OrderBy(a => a.TenantCode).ToList();
                }
            }
            return _tenants;
        }
      

        public static TEN_Tenant GetTenantByTenantDomain(string domain = null)
        {
            var tenantlar = GetTenants();
            var _tenant = tenantlar.Where(a =>
            (a.ServiceDomain != null && a.ServiceDomain.ToLowerInvariant().Split(',').Contains(domain)) ||
            (a.WebDomain != null && a.WebDomain.ToLowerInvariant().Split(',').Contains(domain))).FirstOrDefault();

            if (_tenant == null)
            {
                var _defaultTenant = ConfigurationManager.AppSettings["DefaultTenant"];
                _tenant = GetTenantByTenantCode(Convert.ToInt32(_defaultTenant));
            }
            _tenant.Config = new Config(_tenant.TenantCode);
            return _tenant;
        }
        public static TEN_Tenant GetTenantByTenantCode(int? tenantCode = null)
        {

            var tenantlar = GetTenants();
            var ten = tenantlar.Where(a => a.TenantCode == tenantCode).FirstOrDefault();

            if (ten == null)
            {
                var _defaultTenant = ConfigurationManager.AppSettings["DefaultTenant"];
                ten = tenantlar.Where(a => a.TenantCode == Convert.ToInt32(_defaultTenant)).FirstOrDefault();
            }
            ten.Config = new Config(ten.TenantCode);
            return ten;
        }
    }

    public partial class TEN_Tenant : InfolineTable
    {
        public int? DBType { get; set; }
        public string DBIp { get; set; }
        public int? DBPort { get; set; }
        public string DBCatalog { get; set; }
        public string DBUser { get; set; }
        public string DBPassword { get; set; }
        public string WebDomain { get; set; }
        public string ServiceDomain { get; set; }
        public string TenantName { get; set; }
        public int? TenantCode { get; set; }
        public DateTime? TenantStartDate { get; set; }
        public DateTime? TenantEndDate { get; set; }
        public string TenantConfig { get; set; }
        public bool? TenantIsPOC { get; set; }
        public string MailHost { get; set; }
        public int? MailPort { get; set; }
        public string MailPassword { get; set; }
        public string MailUser { get; set; }
        public bool? MailSSL { get; set; }
        public Config Config { get; set; }
        public string Logo { get; set; }
        public string LogoOther { get; set; }
        public string Favicon { get; set; }
        public string outlookUserName { get; set; }
        public string outlookPassword { get; set; }
        public bool? hasUser { get; set; }

        public string GetConnectionString()
        {
            var conn = "Data Source=" + this.DBIp + "," + this.DBPort + ";Initial Catalog=" + this.DBCatalog + ";User ID=" + this.DBUser + ";Password=" + this.DBPassword + ";Max Pool Size=10000;";
            return conn;
        }

        public DatabaseType GetDataBaseType()
        {
            return (DatabaseType)DBType;
        }

        public string GetWebUrl()
        {
            if (string.IsNullOrEmpty(this.WebDomain))
            {
                return "http://developer.workoftime.com";
            }

            return (this.WebDomain ?? "").Split(',').FirstOrDefault();
        }

        public string GetWebServiceUrl()
        {
            if (string.IsNullOrEmpty(this.ServiceDomain))
            {
                return "http://developerservice.workoftime.com";
            }
            return (this.ServiceDomain ?? "").Split(',').FirstOrDefault();
        }

        public Email GetEmailClass()
        {
            return new Email(this.TenantCode);
        }

        public WorkOfTimeDatabase GetDatabase()
        {
            return new WorkOfTimeDatabase(this.GetConnectionString(), this.GetDataBaseType());
        }


        public void ExecuteRoles()
        {
            PageInfo.Execute(this.Config.Roles);
        }


    }

    public class IysInformation
    {
        public string userName { get; set; }
        public string password { get; set; }
        public int code { get; set; }
        public int brandCode { get; set; }
        public int retailerCode { get; set; }
    }

    public class Config
    {
        public string Logo { get; set; }
        public string Favicon { get; set; }
        public string Version { get; set; }
        public string[] MailingUsers { get; set; }
        public string[] LdapUrls { get; set; } = new string[] { };
        public int LanguageCode { get; set; } = 1100;
        public string outlookUserName { get; set; }
        public string outlookPassword { get; set; }
        public Dictionary<string, string[]> CustomProperty { get; set; } = new Dictionary<string, string[]>();
        public List<Enum_Modules> AllowModules { get; set; } = new List<Enum_Modules>() {
            Enum_Modules.MODULE_COMMISSIONS,
            Enum_Modules.MODULE_DASHBOARD,
            Enum_Modules.MODULE_PERMISSIONS,
            Enum_Modules.MODULE_TASKS,
            Enum_Modules.MODULE_CRM,
            Enum_Modules.MODULE_TRANSACTION,
            Enum_Modules.MODULE_ORDER,
            Enum_Modules.MODULE_SHIFT,
            Enum_Modules.MODULE_ADVANCE,
            Enum_Modules.MODULE_INVOICE,
            Enum_Modules.MODULE_STOCK,
            Enum_Modules.MODULE_HELPDESK
        };
        public List<string> Roles { get; set; } = new List<string>() {
            SHRoles.CRMYonetici,
            SHRoles.DepoSorumlusu,
            SHRoles.IdariPersonelYonetici,
            SHRoles.IdariPersonel,
            SHRoles.IKYonetici,
            SHRoles.IsGorusmesiMulakat,
            SHRoles.MuhasebeSatis,
            SHRoles.MuhasebeAlis,
            SHRoles.OnMuhasebe,
            SHRoles.Personel,
            SHRoles.PersonelTalebi,
            SHRoles.ProjePersonel,
            SHRoles.ProjeYonetici,
            SHRoles.SahaGorevOperator,
            SHRoles.SahaGorevPersonel,
            SHRoles.SahaGorevYonetici,
            SHRoles.SatinAlmaOnaylayici,
            SHRoles.SatinAlmaPersonel,
            SHRoles.SatinAlmaTalebi,
            SHRoles.SatisOnaylayici,
            SHRoles.SatisPersoneli,
            SHRoles.MusteriSatisSorumlusu,
            SHRoles.SatisFatura,
            SHRoles.StokYoneticisi,
            SHRoles.SahaGorevMusteri,
            SHRoles.SistemYonetici,
            SHRoles.Cevirmen,
            SHRoles.CRMBayiPersoneli,
            SHRoles.BayiGorevPersoneli,
            SHRoles.CagriMerkezi,
            SHRoles.DokumanYonetimRolu,
            SHRoles.YukleniciPersoneli,
            SHRoles.UretimYonetici,
            SHRoles.UretimPersonel,
            SHRoles.YardimMasaPersonel,
            SHRoles.YardimMasaTalep,
            SHRoles.YardimMasaYonetim,
            SHRoles.YardimMasaMusteri,
            SHRoles.SatinAlmaOnaylayiciGorev,
            SHRoles.TeknikServisBayiRolu,
            SHRoles.TeknikServisYoneticiRolu,
            SHRoles.HakEdisBayiPersoneli,
            SHRoles.SatisBayiPersoneli,
            SHRoles.PrimHakedisPersoneli,
            SHRoles.BayiOnaylayici,
            SHRoles.ISGSorumlusu,
            SHRoles.SayimPersoneli,
            SHRoles.SayimYoneticisi
        };
        public IysInformation IysInformations { get; set; }

        public Dictionary<DayOfWeek, WorkingTime> WorkingTimes = new Dictionary<DayOfWeek, WorkingTime>
        {
            {DayOfWeek.Sunday,new WorkingTime { allowTimes =new List<TimeInterval> {
                new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) },
                new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 00, 0) }
            },isWorking = false }},
            {DayOfWeek.Monday, new WorkingTime { allowTimes =new List<TimeInterval> {
                new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 00, 0) },
                new TimeInterval { Start = new TimeSpan(13, 00, 0), End = new TimeSpan(18, 00, 0) }
            },isWorking = true}},{DayOfWeek.Tuesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 00, 0) },
                new TimeInterval { Start = new TimeSpan(13, 00, 0), End = new TimeSpan(18, 00, 0) }
            },isWorking = true}},{DayOfWeek.Wednesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 00, 0) },
                new TimeInterval { Start = new TimeSpan(13, 00, 0), End = new TimeSpan(18, 00, 0) }
            },isWorking = true}},{DayOfWeek.Thursday,new WorkingTime { allowTimes =new List<TimeInterval> {
                new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 00, 0) },
                new TimeInterval { Start = new TimeSpan(13, 00, 0), End = new TimeSpan(18, 00, 0) }
            },isWorking = true}},{DayOfWeek.Friday,new WorkingTime { allowTimes =new List<TimeInterval> {
                new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 00, 0) },
                new TimeInterval { Start = new TimeSpan(13, 00, 0), End = new TimeSpan(18, 00, 0) }
            },isWorking = true}},{DayOfWeek.Saturday,new WorkingTime { allowTimes =new List<TimeInterval> {
                new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) },
                new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) }
            },isWorking = false}},
        };
        public bool shiftQrCodeRequired { get; set; } = false;
        public Config(int? TenantCode)
        {
            System.Version current = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            this.Version = string.Format("{0}.{1}.{2}.{3}", current.Major, current.Minor, current.Revision, current.Build);
            this.Logo = "/Content/Customers/" + TenantCode + "/images/logo.png";
            this.Favicon = "/Content/Customers/1100/images/favicon.ico";
            this.MailingUsers = "leventozcelik89@gmail.com";

            switch (TenantCode)
            {
                case 1100:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    this.AllowModules = new List<Enum_Modules>() {
                        Enum_Modules.MODULE_DASHBOARD,
                        Enum_Modules.MODULE_TASKS,
                        Enum_Modules.MODULE_SHIFT,
                        Enum_Modules.MODULE_PERMISSIONS,
                        Enum_Modules.MODULE_CRM,
                        Enum_Modules.MODULE_TRANSACTION,
                        Enum_Modules.MODULE_COMMISSIONS,
                        Enum_Modules.MODULE_ORDER,
                        Enum_Modules.MODULE_WASTAGE,
                        Enum_Modules.MODULE_ADVANCE,
                        Enum_Modules.MODULE_INVOICE,
                        Enum_Modules.MODULE_STOCK,
                        Enum_Modules.MODULE_HELPDESK
                    };
                    this.shiftQrCodeRequired = true;

                    break;
                case 1101:
                    this.WorkingTimes = new Dictionary<DayOfWeek, WorkingTime>
                    {
                        {DayOfWeek.Monday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Tuesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Wednesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Thursday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Friday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Saturday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(14, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Sunday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) },
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) }
                        },isWorking = false }},
                    };
                    this.LanguageCode = 1101;
                    break;
                case 1103:
                    break;
                case 1108:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1114:
                    this.WorkingTimes = new Dictionary<DayOfWeek, WorkingTime>
                    {
                        {DayOfWeek.Monday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Tuesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Wednesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Thursday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Friday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Saturday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 30, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(14, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Sunday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) },
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) }
                        },isWorking = false }},
                    };
                    break;
                case 1115:
                    break;
                case 1127:
                    break;
                case 1129:
                    break;
                case 1130:
                    this.WorkingTimes = new Dictionary<DayOfWeek, WorkingTime>
                    {
                        {DayOfWeek.Monday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(13, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(14, 00, 0), End = new TimeSpan(16, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Tuesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(13, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(14, 00, 0), End = new TimeSpan(16, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Wednesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(13, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(14, 00, 0), End = new TimeSpan(16, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Thursday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(13, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(14, 00, 0), End = new TimeSpan(16, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Friday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(13, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(14, 00, 0), End = new TimeSpan(16, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Saturday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(13, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(14, 00, 0), End = new TimeSpan(16, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Sunday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) },
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) }
                        },isWorking = false }},
                    };
                    break;
                case 1131:
                    break;
                case 1132:
                    this.WorkingTimes = new Dictionary<DayOfWeek, WorkingTime>
                    {
                        {DayOfWeek.Monday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 00, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Tuesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 00, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Wednesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 00, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Thursday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 00, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Friday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 00, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Saturday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 00, 0), End = new TimeSpan(18, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Sunday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) },
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) }
                        },isWorking = false }},
                    };
                    break;
                case 1133:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.SahaGorevMusteri,
                    });
                    break;
                case 1135:
                    this.AllowModules = new List<Enum_Modules>() {
                        Enum_Modules.MODULE_COMMISSIONS,
                        Enum_Modules.MODULE_DASHBOARD,
                        Enum_Modules.MODULE_PERMISSIONS,
                        Enum_Modules.MODULE_TASKS,
                        Enum_Modules.MODULE_CRM,
                        Enum_Modules.MODULE_TRANSACTION,
                        Enum_Modules.MODULE_ORDER,
                        Enum_Modules.MODULE_SHIFT,
                        Enum_Modules.MODULE_ADVANCE,
                        Enum_Modules.MODULE_INVOICE
                    };
                    break;
                case 1136:
                    break;
                case 1137:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1138:
                    break;
                case 1139:
                    this.Favicon = "/Content/Customers/" + TenantCode + "/images/favicon.ico";
                    this.LdapUrls = new string[] { "sv-dc-01.callay.com.tr:636" };
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    this.WorkingTimes = new Dictionary<DayOfWeek, WorkingTime>
                    {
                        {DayOfWeek.Monday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 30, 0), End = new TimeSpan(19, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Tuesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 30, 0),  End = new TimeSpan(19, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Wednesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 30, 0),  End = new TimeSpan(19, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Thursday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 30, 0), End = new TimeSpan(19, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Friday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 30, 0), End = new TimeSpan(19, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Saturday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 30, 0), End = new TimeSpan(19, 30, 0) }
                        },isWorking = true}},{DayOfWeek.Sunday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 30, 0), End = new TimeSpan(19, 30, 0) }
                        },isWorking = true }},
                    };
                    break;

                case 1140:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1142:
                    break;
                case 1143:
                    break;
                case 1144:
                    break;
                case 1145:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1146:
                    break;
                case 1147:
                    break;
                case 1148:
                    this.Roles = new List<string>() {
                        SHRoles.DepoSorumlusu,
                        SHRoles.IdariPersonelYonetici,
                        SHRoles.IKYonetici,
                        SHRoles.MuhasebeSatis,
                        SHRoles.MuhasebeAlis,
                        SHRoles.OnMuhasebe,
                        SHRoles.Personel,
                        SHRoles.SahaGorevOperator,
                        SHRoles.SahaGorevPersonel,
                        SHRoles.SahaGorevYonetici,
                        SHRoles.SatinAlmaOnaylayici,
                        SHRoles.SatinAlmaPersonel,
                        SHRoles.SatinAlmaTalebi,
                        SHRoles.SatisFatura,
                        SHRoles.StokYoneticisi,
                        SHRoles.SistemYonetici,
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep
                    };

                    break;
                case 1149:
                    break;
                case 1150:
                    break;
                case 1151:
                    this.Roles = new List<string>() {
                    SHRoles.CRMYonetici,
                    SHRoles.DepoSorumlusu,
                    SHRoles.IdariPersonelYonetici,
                    SHRoles.IKYonetici,
                    SHRoles.IsGorusmesiMulakat,
                    SHRoles.MuhasebeSatis,
                    SHRoles.Personel,
                    SHRoles.SahaGorevOperator,
                    SHRoles.SahaGorevPersonel,
                    SHRoles.SahaGorevYonetici,
                    SHRoles.SatisPersoneli,
                    SHRoles.SatisFatura,
                    SHRoles.StokYoneticisi,
                    SHRoles.SistemYonetici,
                    };
                    break;
                case 1152:
                    break;
                case 1153:
                    break;
                case 1154:
                    break;
                case 1155:
                    break;
                case 1156:
                    break;
                case 1157:
                    break;
                case 1158:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1159:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1160:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1161:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1162:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1163:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1164:
                    break;
                case 1165:
                    break;
                case 1166:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1167:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1168:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1169:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1170:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1171:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1172:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;

                case 1173:
                    break;

                case 1174:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    this.AllowModules = new List<Enum_Modules>() {
                        Enum_Modules.MODULE_DASHBOARD,
                        Enum_Modules.MODULE_TASKS,
                        Enum_Modules.MODULE_SHIFT,
                        Enum_Modules.MODULE_PERMISSIONS,
                        Enum_Modules.MODULE_CRM,
                        Enum_Modules.MODULE_COMMISSIONS,
                        Enum_Modules.MODULE_ORDER,
                        Enum_Modules.MODULE_WASTAGE,
                        Enum_Modules.MODULE_STOCK
                    };
                    this.shiftQrCodeRequired = true;
                    break;

                case 1175:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1176:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1177:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1178:
                    break;
                case 1179:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1180:
                    break;
                case 1181:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1182:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1183:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1184:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1185:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                        SHRoles.YardimMasaMusteri,
                    });
                    break;
                case 1186:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1187:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    this.WorkingTimes = new Dictionary<DayOfWeek, WorkingTime>
                    {
                        {DayOfWeek.Monday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(17, 00, 0) }
                        },isWorking = true}},{DayOfWeek.Tuesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(17, 00, 0) }
                        },isWorking = true}},{DayOfWeek.Wednesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(17, 00, 0) }
                        },isWorking = true}},{DayOfWeek.Thursday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(17, 00, 0) }
                        },isWorking = true}},{DayOfWeek.Friday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(17, 00, 0) }
                        },isWorking = true}},{DayOfWeek.Saturday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(8, 00, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(17, 00, 0) }
                        },isWorking = true}},{DayOfWeek.Sunday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) },
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) }
                        },isWorking = false }},
                    };
                    break;
                case 1189:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1191:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    this.Roles.Remove(SHRoles.SahaGorevYonetici);
                    this.Roles.Remove(SHRoles.SahaGorevPersonel);
                    this.Roles.Remove(SHRoles.SahaGorevOperator);
                    this.Roles.Remove(SHRoles.CRMYonetici);
                    this.Roles.Remove(SHRoles.SatisFatura);
                    this.Roles.Remove(SHRoles.SatisOnaylayici);
                    this.Roles.Remove(SHRoles.SatisPersoneli);
                    this.Roles.Remove(SHRoles.MuhasebeSatis);
                    this.Roles.Remove(SHRoles.SahaGorevMusteri);
                    this.Roles.Remove(SHRoles.OnMuhasebe);
                    this.shiftQrCodeRequired = true;
                    this.LanguageCode = 1191;
                    break;
                case 1192:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    this.WorkingTimes = new Dictionary<DayOfWeek, WorkingTime>
                   {
                        {DayOfWeek.Monday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 00, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 00, 0) }
                        },isWorking = true}},{DayOfWeek.Tuesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 00, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 00, 0) }
                        },isWorking = true}},{DayOfWeek.Wednesday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 00, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 00, 0) }
                        },isWorking = true}},{DayOfWeek.Thursday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 00, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 00, 0) }
                        },isWorking = true}},{DayOfWeek.Friday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(9, 00, 0), End = new TimeSpan(12, 30, 0) },
                            new TimeInterval { Start = new TimeSpan(13, 30, 0), End = new TimeSpan(18, 00, 0) }
                        },isWorking = true}},{DayOfWeek.Saturday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(0, 00, 0), End = new TimeSpan(0, 00, 0) },
                            new TimeInterval { Start = new TimeSpan(0, 00, 0), End = new TimeSpan(0, 00, 0) }
                        },isWorking = false}},{DayOfWeek.Sunday,new WorkingTime { allowTimes =new List<TimeInterval> {
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) },
                            new TimeInterval { Start = new TimeSpan(0, 0, 0), End = new TimeSpan(0, 0, 0) }
                        },isWorking = false }},
                    };
                    this.AllowModules.Remove(Enum_Modules.MODULE_SHIFT);
                    break;
                case 1194:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1197:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1199:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1994:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1200:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1203:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                    });
                    break;
                case 1999:
                    this.Roles.AddRange(new List<string> {
                        SHRoles.YardimMasaYonetim,
                        SHRoles.YardimMasaPersonel,
                        SHRoles.YardimMasaTalep,
                        });
                    break;
                default:
                    break;
            }
        }
    }

    public enum Enum_Modules
    {
        MODULE_DASHBOARD = 0,
        MODULE_PERMISSIONS = 1,
        MODULE_COMMISSIONS = 2,
        MODULE_TASKS = 3,
        MODULE_FILES = 4,
        MODULE_DOCUMENTS = 5,
        MODULE_CRM = 6,
        MODULE_SHIFT = 7,
        MODULE_TRANSACTION = 8,
        MODULE_ORDER = 9,
        MODULE_WASTAGE = 10,
        MODULE_ADVANCE = 11,
        MODULE_INVOICE = 12,
        MODULE_STOCK = 13,
        MODULE_HELPDESK = 14
    }

    public class WorkingTime
    {
        public List<TimeInterval> allowTimes { get; set; } = new List<TimeInterval>();
        public bool isWorking { get; set; } = true;
    }

    public class TimeInterval
    {
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
}
