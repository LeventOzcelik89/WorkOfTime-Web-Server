using Infoline.Helper;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infoline.WorkOfTime.BusinessAccess.Mobile
{
    public class Defaults
    {

        public Summaries GetSummaries(Guid userId)
        {

            var res = new Summaries
            {
                PermitSummaries = new PermitSummaries { },
                CommissionSummaries = new CommissionSummary { },
                TransactionSummaries = new TransactionSummary { },
                AdvanceSummaries = new AdvanceSummary { },
                OrderSummaries = new OrderSummary { },
                TaskSummaries = new VMFTM_TaskModel().GetTaskSummary(userId),
                TaskSummariesNew = new VMFTM_TaskServiceModel().GetTaskSummary(userId),
                SummaryWaitingTransactions = new SummaryWaitingTransactions { },
                SummaryWaitingAdvances = new SummaryWaitingAdvances { },
                SummaryWaitingTickets = new SummaryWaitingTickets { },
                SummaryWaitingManagerTickets = new SummaryWaitingManagerTickets { },
            };

            var db = new WorkOfTimeDatabase();

            var user = db.GetVWSH_UserById(userId);
            var permits = new Permits().MyPermits(userId);
            var permitsOther = new Permits().MyPermitsAbout(userId);
            var commissions = new Commissions().MBMyCommissions(userId);
            var transaction = new Transaction().MyTransaction(userId);
            var advance = new Advance().MyAdvance(userId);
            var order = new Order().MyOrder(userId);
            var commissionsOther = new Commissions().MyCommissionsAbout(userId);
            var mySumTransaction = new Transaction().MySummaryTransaction(userId);
            var mySumAdvances = new Advance().MySummaryTransaction(userId);
            var mySumTickets = new VMHDM_TicketModel().MySummaryTickets(userId);
            var mySumManagerTickets = new VMHDM_TicketModel().MySummaryManagerTickets(userId);

            if (user == null)
            {
                return res;
            }

            var hesap = user.GetRemainingExcuse();
            var mnt = hesap % 1;
            var minute = Math.Floor((60 * mnt));
            var saat = Math.Floor(hesap);
            var txt = "";
            txt += saat != 0 ? saat + " saat " : "";
            txt += minute != 0 ? minute + " dakika" : "";


            res.PermitSummaries = new PermitSummaries
            {
                MyPermits = new SummaryPermits
                {
                    Deserved = user.PermitYearlyDeserved,
                    Used = user.PermitYearlyUsed,
                    Approved = permits.objects.Approved.Count(),
                    Declined = permits.objects.Declined.Count(),
                    Waiting = permits.objects.Waiting.Count(),
                },
                OtherPermits = new SummaryOther
                {
                    Approved = permitsOther.objects.Approved.Count(),
                    Declined = permitsOther.objects.Declined.Count(),
                    Waiting = permitsOther.objects.Waiting.Count(),
                },
                MyPermitsHourly = new SummaryHourlyPermits
                {
                    Deserved = user.GetRemainingExcuse() + (user.PermitExcuseUsed),
                    Used = user.PermitExcuseUsed,
                    Remaining = user.GetRemainingExcuse(),
                    RemainingText = txt
                }
            };

            res.CommissionSummaries = new CommissionSummary
            {
                MyCommissions = new SummaryCommissions
                {
                    Used = commissions.objects.Approved.Where(a => a.EndDate <= DateTime.Now).Count(),
                    Approved = commissions.objects.Approved.Count(),
                    Declined = commissions.objects.Declined.Count(),
                    Waiting = commissions.objects.Waiting.Count()
                },
                OtherCommissions = new SummaryOther
                {
                    Waiting = commissionsOther.objects.Waiting.Count(),
                    Declined = commissionsOther.objects.Declined.Count(),
                    Approved = commissionsOther.objects.Approved.Count()
                }
            };

            res.TransactionSummaries = new TransactionSummary
            {
                MyTransaction = new SummaryTransaction
                {
                    All = transaction.objects.All.Count(),
                    Waiting = transaction.objects.Waiting.Count(),
                    Approved = transaction.objects.Approved.Count(),
                    Declined = transaction.objects.Declined.Count(),
                }
            };

            res.AdvanceSummaries = new AdvanceSummary
            {
                MyAdvance = new SummaryAdvance
                {
                    All = advance.objects.All.Count(),
                    Waiting = advance.objects.Waiting.Count(),
                    Approved = advance.objects.Approved.Count(),
                    Declined = advance.objects.Declined.Count(),
                }
            };

            res.OrderSummaries = new OrderSummary
            {
                MyOrder = new SummaryOrder
                {
                    All = order.objects.All.Count(),
                    Waiting = order.objects.Waiting.Count(),
                    Approved = order.objects.Approved.Count(),
                    Invoiced = order.objects.Invoiced.Count(),
                    Declined = order.objects.Declined.Count()
                }
            };

            res.SummaryWaitingTransactions = new SummaryWaitingTransactions
            {
                MyTransactionCount = new OrderWaitingTransactions
                {
                    Waiting = mySumTransaction.objects.Waiting.Count()
                }
            };

            res.SummaryWaitingAdvances = new SummaryWaitingAdvances
            {
                MyAdvancesCount = new OrderWaitingAdvances
                {
                    Waiting = mySumAdvances.objects.Waiting.Count()
                }
            };


            res.SummaryWaitingTickets = new SummaryWaitingTickets
            {
                MyTicketsCount = new OrderWaitingTickets
                {
                    Waiting = mySumTickets.objects.Waiting.Count()
                }
            };

            res.SummaryWaitingManagerTickets = new SummaryWaitingManagerTickets
            {
                MyTicketsCount = new OrderWaitingManagerTickets
                {
                    Waiting = mySumManagerTickets.objects.Waiting.Count()
                }
            };





            res.Notifications = GetNotifications(user, res.TaskSummaries);

            res.Others = new OtherSummaries
            {
                TotalWorkedProject = db.GetVWINV_CompanyPersonDepartmentsByIdUser(user.id).Where(a => a.OrganizationType == (int)EnumINV_CompanyDepartmentsType.Matrix).Count()
            };

            return res;

        }

        public PendingRequest[] GetNotifications(VWSH_User _user, SummaryHeadersTask data)
        {

            var db = new WorkOfTimeDatabase();
            var res = new List<PendingRequest>();
            var taskmodel = new VMFTM_TaskModel();
            //var taskSummary = taskmodel.GetTaskSummary(_user.id);

            if (_user != null && _user.IsWorking == true)
            {
                //adedin geleceği yere _ koyulması gerekiyor.

                if (data.AssignableTask > 0)
                {
                    res.Add(new PendingRequest
                    {
                        name = "FTMTaskAssignable",
                        description = "Üstlenebileceğiniz _saha görevleri bulunmaktadır.",
                        url = "/FTM/VWFTM_Task/Index",
                        iconUrl = "/Content/Custom/img/maintenance.png",
                        count = data.AssignableTask
                    });
                }

                res.Add(new PendingRequest
                {
                    name = "PermitCountByWaiting",
                    description = "Onayınızı bekleyen_izin talebi bulunmaktadır.",
                    url = "/INV/VWINV_Permit/MyAboutIndex",
                    iconUrl = "/Content/Custom/img/izinIcon24.png",
                    count = db.GetVWINV_PermitPending(_user.id).ToArray().Count()
                });


                res.Add(new PendingRequest
                {
                    name = "PermitSineCountByWaiting",
                    description = "Islak imzalı dosya yüklemeniz beklenen_izin talebiniz bulunmaktadır.",
                    url = "/INV/VWINV_Permit/MyIndex",
                    iconUrl = "/Content/Custom/img/imzaIcon24.png",
                    count = db.GetVWINV_PermitSinePending(_user.id).ToArray().Count()
                });

                res.Add(new PendingRequest
                {
                    name = "CommissionsCountByWaiting",
                    description = "Onayınızı bekleyen_görevlendirme talebi bulunmaktadır.",
                    url = "/INV/VWINV_Commissions/MyAboutIndex",
                    iconUrl = "/Content/Custom/img/gorevlendirmeIcon24.png",
                    count = db.GetVWINV_CommissionsPending(_user.id).ToArray().Count()
                });

                res.Add(new PendingRequest
                {
                    name = "CommissionsSineCountByWaiting",
                    description = "Islak imzalı dosya yüklemeniz beklenen_görev talebiniz bulunmaktadır.",
                    url = "/INV/VWINV_Commissions/MyIndex",
                    iconUrl = "/Content/Custom/img/imzaIcon24.png",
                    count = db.GetVWINV_CommissionsSinePending(_user.id).Count()
                });

            }

            return res.ToArray();

        }

        public NotificationRequest[] GetNotificationsWithMobileNavigation(VWSH_User _user)
        {

            var db = new WorkOfTimeDatabase();
            var res = new List<NotificationRequest>();

            //adedin geleceği yere _ koyulması gerekiyor.
            if (_user != null && _user.IsWorking == true)
            {
                var permitList = db.GetVWINV_PermitPending(_user.id);
                for (int i = 0; i < permitList.Count(); i++)
                {
                    res.Add(new NotificationRequest
                    {
                        description = "<font style=\"color: blue\">Onayınızı</font> bekleyen <font style=\"color: red\">izin talebi</font> bulunmaktadır.",
                        url = TenantConfig.Tenant.GetWebUrl() + "/INV/VWINV_Permit/DetailConfirmation?id=" + permitList[i].id,
                        iconUrl = "/Content/Custom/img/izinIcon24.png",
                    });
                }


                var sendingPermitList = db.GetVWINV_PermitSinePending(_user.id);
                for (int i = 0; i < sendingPermitList.Count(); i++)
                {
                    res.Add(new NotificationRequest
                    {
                        description = "<font style=\"color: blue\">Islak imzalı dosya</font> yüklemeniz beklenen <font style=\"color: red\">izin talebi</font> bulunmaktadır.",
                        url = TenantConfig.Tenant.GetWebUrl() + "/INV/VWINV_Permit/Detail?id=" + sendingPermitList[i].id.ToString(),
                        iconUrl = "/Content/Custom/img/imzaIcon24.png",
                    });
                }


                var pendingConfirmationList = db.GetVWINV_CommissionsPending(_user.id);
                for (int i = 0; i < pendingConfirmationList.Count(); i++)
                {
                    res.Add(new NotificationRequest
                    {
                        description = "<font style=\"color: blue\">Onayınızı</font> bekleyen <font style=\"color: red\">görevlendirme talebi</font> bulunmaktadır.",
                        url = TenantConfig.Tenant.GetWebUrl() + "/INV/VWINV_Commissions/DetailConfirmation?id=" + pendingConfirmationList[i].id.ToString(),
                        iconUrl = "/Content/Custom/img/gorevlendirmeIcon24.png",
                    });
                }


                var sineConfirmationList = db.GetVWINV_CommissionsSinePending(_user.id);
                for (int i = 0; i < sineConfirmationList.Count(); i++)
                {
                    res.Add(new NotificationRequest
                    {
                        description = "<font style=\"color: blue\">Islak imzalı dosya</font> yüklemeniz beklenen <font style=\"color: red\">görevlendirme talebi</font> bulunmaktadır.",
                        url = TenantConfig.Tenant.GetWebUrl() + "/INV/VWINV_Commissions/Detail?id=" + sineConfirmationList[i].id.ToString(),
                        iconUrl = "/Content/Custom/img/imzaIcon24.png",
                    });
                }


                var taskList = db.GetVWFTM_TaskAssignableByUserId(_user.id);
                for (int i = 0; i < taskList.Count(); i++)
                {

                    res.Add(new NotificationRequest
                    {
                        description = "<font style=\"color: blue\">Üstlenebileceğiniz</font> <font style=\"color: red\">saha görevi</font> bulunmaktadır.",
                        url = TenantConfig.Tenant.GetWebUrl() + "/FTM/VWFTM_Task/Detail?id=" + taskList[i].id.ToString(),
                        iconUrl = "/Content/Custom/img/maintenance.png",
                    });
                }
            }

            return res.ToArray();
        }

        public MobileLoginInformations GetMobileLoginInformations(VWSH_User user)
        {
            var db = new WorkOfTimeDatabase();
            var res = new MobileLoginInformations();

            res.GetUserInfo = db.GetUserPageSecurityByUserid(user.id, CallContext.Current.TicketId);
            res.GetConfigs = GetConfigs();
            res.GetDataSources = GetDataSources(user.id);
            res.MBUT_LocationConfig = new Locations().LConfigGetByUserId(user.id);

            return res;
        }

        public DataSources GetDataSources(Guid userId)
        {

            var db = new WorkOfTimeDatabase();

            var res = new DataSources();

            res.Permit = new DataSourcePermit
            {
                PermitOffical = db.GetVWINV_PermitOffical(),
                PermitType = db.INV_PermitTypeNameIdCreated().Where(x => x.RequestStaff == true).OrderBy(x => x.created).ToArray()
            };
            res.Commission = new DataSourceCommission
            {

                Projects = db.GetPRJ_Project().ToDictionary(a => a.id, a => a.ProjectName),

                DemandForAdvance = db.GetSYS_EnumsByWhere(a => a.tableName == "INV_Commissions" && a.fieldName == "DemandForAdvance")
                        .OrderBy(a => a.enumDescription).ToDictionary(a => a.enumKey, a => a.enumDescription),
                Information = db.GetSYS_EnumsByWhere(a => a.tableName == "INV_Commissions" && a.fieldName == "Information")
                        .OrderBy(a => a.enumDescription).ToDictionary(a => a.enumKey, a => a.enumDescription),
                RequestForAccommodation = db.GetSYS_EnumsByWhere(a => a.tableName == "INV_Commissions" && a.fieldName == "RequestForAccommodation")
                        .OrderBy(a => a.enumDescription).ToDictionary(a => a.enumKey, a => a.enumDescription),
                TravelInformation = db.GetSYS_EnumsByWhere(a => a.tableName == "INV_Commissions" && a.fieldName == "TravelInformation")
                        .OrderBy(a => a.enumDescription).ToDictionary(a => a.enumKey, a => a.enumDescription),
                CompanyCars = db.GetVWCMP_CompanyCars().OrderBy(a => a.plate).ToDictionary(a => a.id, a => a.plate),

                Persons = db.GetVWINV_CompanyPersonDepartments()
                    .Where(a =>
                        a.IsBasePosition == true &&
                        a.OrganizationType == (Int32)EnumINV_CompanyDepartmentsType.Organization &&
                        a.StartDate < DateTime.Now &&
                        (a.EndDate == null || a.EndDate > DateTime.Now) &&
                        (a.IdUser == userId || a.Manager1 == userId || a.Manager2 == userId) &&
                        a.Person_Title != null
                    ).GroupBy(a => a.IdUser).ToDictionary(a => a.Key, a => a.Select(c => c.Person_Title).FirstOrDefault())

            };
            res.ShiftsNew = TenantConfig.Tenant.Config.WorkingTimes;
            res.ShiftTimeSpace = 15;
            res.FTMTasks = new DataSourceFTMTask
            {
                TaskOperationStatus = EnumsProperties.EnumToArrayGeneric<EnumFTM_TaskOperationStatus>().ToDictionary(a => Convert.ToInt32(a.Key), b => new TaskOperationStatus { Value = b.Value, Color = b.Generic["color"] }),
                TaskType = db.GetSYS_EnumsByWhere(a => a.tableName == "FTM_Task" && a.fieldName == "type").OrderBy(a => a.enumDescription).ToDictionary(a => a.enumKey, a => a.enumDescription),
            };


            return res;

        }

        public ConfigDatas GetConfigs()
        {
            var config = new Config(TenantConfig.Tenant.TenantCode.Value);
            var data = new ConfigDatas
            {
                Logo = config.Logo,
                TenantLogo = TenantConfig.Tenant.Logo,
                TenantCode = TenantConfig.Tenant.TenantCode.Value,
                AllowModules = config.AllowModules.OrderBy(x => x).ToList(),
                TenantSetting = new TenantSetting { Barcode = true, nfc = true },
                TenantName = TenantConfig.Tenant.TenantName,
                WebApplicationURL = TenantConfig.Tenant.GetWebUrl(),
                shiftQRCodeRequired = config.shiftQrCodeRequired
            };

            return data;
        }


    }



    public class Summaries
    {
        public PermitSummaries PermitSummaries { get; set; }            //  İzinler için özet sayılar
        public CommissionSummary CommissionSummaries { get; set; }      //  Görevlendirmeler için özet sayılar
        public TransactionSummary TransactionSummaries { get; set; }      //  Masraf Talepleri için özet sayılar
        public AdvanceSummary AdvanceSummaries { get; set; }      //  Avans Talepleri için özet sayılar

        public OrderSummary OrderSummaries { get; set; }      //  Satış Siparişleri için özet sayılar
        public SummaryHeadersTask TaskSummaries { get; set; }      //  Görevlendirmeler için özet sayılar
        public PendingRequest[] Notifications { get; set; }             //  Bildirimler
        public OtherSummaries Others { get; set; }                      //  Diğer Bilgiler
        public SummaryWaitingTransactions SummaryWaitingTransactions { get; set; }
        public SummaryWaitingAdvances SummaryWaitingAdvances { get; set; }
        public SummaryWaitingTickets SummaryWaitingTickets { get; set; }
        public SummaryWaitingManagerTickets SummaryWaitingManagerTickets { get; set; }
        public SummaryHeadersTaskNew TaskSummariesNew { get; set; }

    }
    public class PermitSummaries
    {
        public SummaryPermits MyPermits { get; set; }                       //  Benim Yıllık İzinlerim
        public SummaryHourlyPermits MyPermitsHourly { get; set; }           //  Benim Mazeret İzinlerim
        public SummaryOther OtherPermits { get; set; }                      //  Bana Yapılan İzin Talepleri
    }
    public class SummaryPermits
    {
        public int? Deserved { get; set; }      //  Hak Edilen Yıllık İzin
        public int? Used { get; set; }          //  Kullanılan Yıllık İzin
        public int Approved { get; set; }       //  Onaylanan Tüm İzinler Sayısı
        public int Declined { get; set; }       //  Reddedilen Tüm İzinler Sayısı
        public int Waiting { get; set; }        //  Bekleyen Tüm İzinlerin Sayısı
    }
    public class SummaryHourlyPermits
    {
        public double? Deserved { get; set; }      //  Hakedilen
        public double? Used { get; set; }          //  Kullanılan
        public double Remaining { get; set; }      //  Kalan
        public string RemainingText { get; set; }  //  Kalan string
    }
    public class CommissionSummary
    {
        public SummaryCommissions MyCommissions { get; set; }           //  Görevlendirme Taleplerim
        public SummaryOther OtherCommissions { get; set; }              //  Bana Yapılan Görevlendirme Talepleri
    }
    public class SummaryOther
    {
        public int Approved { get; set; }       //  Kabul Edilen
        public int Declined { get; set; }       //  Red Edilen
        public int Waiting { get; set; }        //  Bekleyen
    }
    public class SummaryCommissions
    {
        public int? Used { get; set; }          //  Gerçekleşen Görevlendirmelerim
        public int Approved { get; set; }       //  Onaylanan Edilen
        public int Declined { get; set; }       //  Reddedilen
        public int Waiting { get; set; }        //  Bekleyen
    }

    public class TransactionSummary
    {
        public SummaryTransaction MyTransaction { get; set; }           //  Masraf Taleplerim
    }

    public class AdvanceSummary
    {
        public SummaryAdvance MyAdvance { get; set; }           //  Avans Taleplerim
    }

    public class OrderSummary
    {
        public SummaryOrder MyOrder { get; set; }           //  Masraf Taleplerim
    }

    public class ConfigTenantUrl
    {
        public string WebURL { get; set; }
        public string WebServiceURL { get; set; }
    }
    public class SummaryTransaction
    {
        public int All { get; set; }          //  Tümü
        public int Waiting { get; set; }        //  Bekleyen
        public int Approved { get; set; }       //  Onaylanan
        public int Declined { get; set; }       //  Reddedilen
    }

    public class SummaryWaitingTransactions
    {
        public OrderWaitingTransactions MyTransactionCount { get; set; }           //  Masraf Taleplerim
    }

    public class SummaryWaitingAdvances
    {
        public OrderWaitingAdvances MyAdvancesCount { get; set; }           //  Avans Taleplerim
    }

    public class SummaryWaitingTickets
    {
        public OrderWaitingTickets MyTicketsCount { get; set; }           //  Avans Taleplerim
    }

    public class SummaryWaitingManagerTickets
    {
        public OrderWaitingManagerTickets MyTicketsCount { get; set; }           //  Avans Taleplerim
    }


    public class SummaryAdvance
    {
        public int All { get; set; }          //  Tümü
        public int Waiting { get; set; }        //  Bekleyen
        public int Approved { get; set; }       //  Onaylanan
        public int Declined { get; set; }       //  Reddedilen
    }

    public class SummaryOrder
    {
        public int All { get; set; }          //  Gerçekleşen Görevlendirmelerim
        public int Waiting { get; set; }        //  Bekleyen
        public int Approved { get; set; }       //  Onaylanan
        public int Invoiced { get; set; }       //  Fatura Kesilen
        public int Declined { get; set; }       //  Reddedilen
    }

    public class OrderWaitingTransactions
    {
        public int Waiting { get; set; }        //  Bekleyen
    }

    public class OrderWaitingAdvances
    {
        public int Waiting { get; set; }        //  Bekleyen
    }

    public class OrderWaitingTickets
    {
        public int Waiting { get; set; }        //  Bekleyen
    }

    public class OrderWaitingManagerTickets // Yönetici Bekleyen Görevler
    {
        public int Waiting { get; set; }        //  Bekleyen
    }

    public class PendingRequest
    {
        public string name { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public int count { get; set; }
        public string iconUrl { get; set; }
    }

    public class NotificationRequest
    {
        public string description { get; set; }
        public string url { get; set; }
        public string iconUrl { get; set; }
    }

    public class MobileLoginInformations
    {
        public PageSecurity GetUserInfo { get; set; }
        public ConfigDatas GetConfigs { get; set; }
        public DataSources GetDataSources { get; set; }
        public List<VMUT_LocationConfig> MBUT_LocationConfig { get; set; }
    }

    public class OtherSummaries
    {
        public int TotalWorkedProject { get; set; }     //  Görev alınan toplam proje sayısı
    }

    public class DataSources
    {
        public DataSourceCommission Commission { get; set; }                //  Görevlendirme Verileri
        public DataSourcePermit Permit { get; set; }                        //  İzin Verileri
        public Dictionary<DayOfWeek, WorkingTime> ShiftsNew { get; set; }
        public DataSourceFTMTask FTMTasks { get; set; }
        public int ShiftTimeSpace { get; set; }
    }
    public class DataSourceCommission
    {
        public Dictionary<int?, string> DemandForAdvance { get; set; }            //  Avans Talebi        
        public Dictionary<int?, string> TravelInformation { get; set; }           //  Seyahat Bilgileri   
        public Dictionary<int?, string> Information { get; set; }                 //  Görevlendirme Tipi  
        public Dictionary<int?, string> RequestForAccommodation { get; set; }     //  Konaklama Talebi   
        public Dictionary<Guid, string> Projects { get; set; }                      //  Projeler            
        public Dictionary<Guid?, string> Persons { get; set; }                      //  Personeller 
        public Dictionary<Guid, string> CompanyCars { get; set; }                   //  Araçlar 
    }
    public class DataSourcePermit
    {
        public INV_PermitType[] PermitType { get; set; }            //  İzin Tipleri
        public VWINV_PermitOffical[] PermitOffical { get; set; }            //  Resmi Tatiller
    }

    public class DataSourceFTMTask
    {
        public Dictionary<int, TaskOperationStatus> TaskOperationStatus { get; set; }
        public Dictionary<int?, string> TaskType { get; set; }
    }

    public class TaskOperationStatus
    {
        public string Value { get; set; }
        public string Color { get; set; }
    }



    public class Splitted<T>
    {
        public T[] All { get; set; }                //  Tümü
        public T[] Approved { get; set; }           //  Onaylanan
        public T[] Waiting { get; set; }            //  Bekleyen
        public T[] Invoiced { get; set; }           //  Faturası Kesilen
        public T[] Declined { get; set; }           //  Reddedilen
    }

    public class ConfigDatas
    {
        public int TenantCode { get; set; }
        public string TenantName { get; set; }
        public TenantSetting TenantSetting { get; set; }
        public List<Enum_Modules> AllowModules { get; set; }
        public string Logo { get; set; }
        public string TenantLogo { get; set; }
        public string WebApplicationURL { get; set; }
        public bool shiftQRCodeRequired { get; set; }
    }

    public class TenantSetting
    {
        public bool Barcode { get; set; }
        public bool nfc { get; set; }
    }

    public class SummaryHeaders
    {
        public int waiting { get; set; }
        public int approved { get; set; }
        public int rejected { get; set; }
    }
}