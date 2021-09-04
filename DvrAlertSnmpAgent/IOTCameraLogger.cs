using Infoline.Helper;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using SnmpSharpNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DvrAlertSnmpAgent
{
    internal class IOTCameraLogger
    {
        private bool start { get; set; } = false;
        public TEN_Tenant _tenant { get; set; }
        public WorkOfTimeDatabase _db { get; set; }
        public string _url { get; set; }
        public Email _email { get; set; }
        public static Dictionary<Oid, AsnType> walkData { get; set; }
        public static IOT_CameraLog CameraLog { get; set; }
        public string CameraIpAddress { get; set; }
        public IOTCameraLogger(TEN_Tenant tenant)
        {
            _tenant = tenant;
            _db = _tenant.GetDatabase();
            _url = _tenant.GetWebUrl();
            _email = _tenant.GetEmailClass();
            Log.Success("Ajan Başlatıldı. ({0} {1})", _tenant.TenantName, _tenant.TenantCode);
            start = true;
            ProcessLoop();
        }

        public void ProcessLoop()
        {
            while (true)
            {
                try
                {
                    CameraIpAddress = System.Configuration.ConfigurationManager.AppSettings["CameraIpAddress"];
                    var snmpVerb = new SimpleSnmp(CameraIpAddress, 161, "public");
                    walkData = snmpVerb.Walk(SnmpVersion.Ver1, ".1.3.6.1.4.1.36849.1.1");
                    CameraLog = new IOT_CameraLog
                    {
                        id = Guid.NewGuid(),
                        created = DateTime.Now,
                    };

                    CameraLog.modelName = GetModelName();

                    CameraLog.videoType = GetVideoType();
                    CameraLog.audioType = GetAudioType();
                    CameraLog.version = GetVersion();
                    CameraLog.readtime = GetDate();
                    CameraLog.macAddress = GetMacAdress();
                    CameraLog.ipAddress = GetIpAdress();
                    CameraLog.subnetMask = GetMask();
                    CameraLog.dnsAddress = GetDNS();

                    CameraLog.videoStatusChannel1 = GetChannelVideoStatus("1");
                    CameraLog.videoStatusChannel2 = GetChannelVideoStatus("2");
                    CameraLog.videoStatusChannel3 = GetChannelVideoStatus("3");
                    CameraLog.videoStatusChannel4 = GetChannelVideoStatus("4");

                    CameraLog.alarmInputStatus1 = GetAlarmInputStatus("1");
                    CameraLog.alarmInputStatus2 = GetAlarmInputStatus("2");
                    CameraLog.alarmInputStatus3 = GetAlarmInputStatus("3");
                    CameraLog.alarmInputStatus4 = GetAlarmInputStatus("4");

                    CameraLog.alarmOutputStatus1 = GetAlarmOutputStatus("1");
                    CameraLog.alarmOutputStatus2 = GetAlarmOutputStatus("2");
                    CameraLog.alarmOutputStatus3 = GetAlarmOutputStatus("3");
                    CameraLog.alarmOutputStatus4 = GetAlarmOutputStatus("4");

                    CameraLog.motionDetectionStatus1 = GetMotionDetectionStatus("1");
                    CameraLog.motionDetectionStatus2 = GetMotionDetectionStatus("2");
                    CameraLog.motionDetectionStatus3 = GetMotionDetectionStatus("3");
                    CameraLog.motionDetectionStatus4 = GetMotionDetectionStatus("4");

                    CameraLog.tamperingDetectionStatus1 = GetTamperingDetectionStatus("1");
                    CameraLog.tamperingDetectionStatus2 = GetTamperingDetectionStatus("2");
                    CameraLog.tamperingDetectionStatus3 = GetTamperingDetectionStatus("3");
                    CameraLog.tamperingDetectionStatus4 = GetTamperingDetectionStatus("4");

                    CameraLog.recordingStatus1 = GetRecordingStatus("1");
                    CameraLog.recordingStatus2 = GetRecordingStatus("2");
                    CameraLog.recordingStatus3 = GetRecordingStatus("3");
                    CameraLog.recordingStatus4 = GetRecordingStatus("4");

                    var rs = _db.InsertIOT_CameraLog(CameraLog);

                    if (rs.result)
                    {
                        Console.WriteLine("Kamera verileri doğru bir şekilde kaydedildi.");
                    }
                    else
                    {
                        Console.WriteLine("Kamera verileri kaydedilirken bir hata oluştu.");
                    }

                    WriteInfo();

                }
                catch
                {
                    //Log.Info("Personel bilgileri çekilirken sorunlar oluştu. Mesaj : {2} ({0} {1})", _tenant.TenantName, _tenant.TenantCode, ex.Message);
                }

                Thread.Sleep(new TimeSpan(0, 1, 0));
            }

        }

        private static string GetModelName()
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.1.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetVideoType()
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.2.2.1.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetAudioType()
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.2.2.2.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetVersion()
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.2.1.1.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetDate()
        {
            try
            {
                var data = walkData[new Oid(".1.3.6.1.4.1.36849.1.1.2.1.2.0")].ToString();
                return data;
            }
            catch
            {
                return null;
            }
        }
        private static string GetMacAdress()
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.2.3.1.1.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetIpAdress()
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.2.3.1.2.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetMask()
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.2.3.1.4.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetDNS()
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.2.3.1.5.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetChannelVideoStatus(string channelNumber)
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.3.2." + channelNumber + ".0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetAlarmInputStatus(string channelNumber)
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.4.1." + channelNumber + ".1.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetAlarmOutputStatus(string channelNumber)
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.4.2." + channelNumber + ".1.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetMotionDetectionStatus(string channelNumber)
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.4.3." + channelNumber + ".1.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetTamperingDetectionStatus(string channelNumber)
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.4.4." + channelNumber + ".1.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static string GetRecordingStatus(string channelNumber)
        {
            try
            {
                return walkData[new Oid(".1.3.6.1.4.1.36849.1.1.6." + channelNumber + ".2.0")].ToString();
            }
            catch
            {
                return null;
            }
        }
        private static void WriteInfo()
        {
            Console.WriteLine(" ---------------------------------------------- ");
            Console.WriteLine(" ---------------------------------------------- ");
            Console.WriteLine(" ---------------------------------------------- ");
            Console.WriteLine("Cihaz Modeli : " + CameraLog.modelName);
            Console.WriteLine("Cihaz Versiyonu : " + CameraLog.version);
            Console.WriteLine("Cihaz Mac Adresi : " + CameraLog.macAddress);
            Console.WriteLine("Cihaz IP Adresi : " + CameraLog.ipAddress);
            Console.WriteLine("Cihaz Mask Adresi : " + CameraLog.subnetMask);
            Console.WriteLine("Cihaz DNS Adresi : " + CameraLog.dnsAddress);

            Console.WriteLine("Cihaz Video Tipi : " + CameraLog.videoType);
            Console.WriteLine("Cihaz Ses Tipi : " + CameraLog.audioType);
            Console.WriteLine(" ---------------------------------------------- ");
            Console.WriteLine("Tarih : " + CameraLog.readtime);
            Console.WriteLine(" ---------------------------------------------- ");

            Console.WriteLine("Kamera 1 Durum : " + GetChannelStatusString(CameraLog.videoStatusChannel1));
            Console.WriteLine("Kamera 2 Durum : " + GetChannelStatusString(CameraLog.videoStatusChannel2));
            Console.WriteLine("Kamera 3 Durum : " + GetChannelStatusString(CameraLog.videoStatusChannel3));
            Console.WriteLine("Kamera 4 Durum : " + GetChannelStatusString(CameraLog.videoStatusChannel4));

            Console.WriteLine(" ---------------------------------------------- ");
            Console.WriteLine("Kamera 1 Alarm Giriş : " + GetEventStatusString(CameraLog.alarmInputStatus1));
            Console.WriteLine("Kamera 2 Alarm Giriş : " + GetEventStatusString(CameraLog.alarmInputStatus2));
            Console.WriteLine("Kamera 3 Alarm Giriş : " + GetEventStatusString(CameraLog.alarmInputStatus3));
            Console.WriteLine("Kamera 4 Alarm Giriş : " + GetEventStatusString(CameraLog.alarmInputStatus4));

            Console.WriteLine(" ---------------------------------------------- ");
            Console.WriteLine("Kamera 1 Alarm Çıkış : " + GetEventStatusString(CameraLog.alarmOutputStatus1));
            Console.WriteLine("Kamera 2 Alarm Çıkış : " + GetEventStatusString(CameraLog.alarmOutputStatus2));
            Console.WriteLine("Kamera 3 Alarm Çıkış : " + GetEventStatusString(CameraLog.alarmOutputStatus3));
            Console.WriteLine("Kamera 4 Alarm Çıkış : " + GetEventStatusString(CameraLog.alarmOutputStatus4));

            Console.WriteLine(" ---------------------------------------------- ");
            Console.WriteLine("Kamera 1 Hareket Alarmı : " + GetEventStatusString(CameraLog.motionDetectionStatus1));
            Console.WriteLine("Kamera 2 Hareket Alarmı : " + GetEventStatusString(CameraLog.motionDetectionStatus2));
            Console.WriteLine("Kamera 3 Hareket Alarmı : " + GetEventStatusString(CameraLog.motionDetectionStatus3));
            Console.WriteLine("Kamera 4 Hareket Alarmı : " + GetEventStatusString(CameraLog.motionDetectionStatus4));

            Console.WriteLine(" ---------------------------------------------- ");
            Console.WriteLine("Kamera 1 Kurcalama Alarmı : " + GetEventStatusString(CameraLog.tamperingDetectionStatus1));
            Console.WriteLine("Kamera 2 Kurcalama Alarmı : " + GetEventStatusString(CameraLog.tamperingDetectionStatus2));
            Console.WriteLine("Kamera 3 Kurcalama Alarmı : " + GetEventStatusString(CameraLog.tamperingDetectionStatus3));
            Console.WriteLine("Kamera 4 Kurcalama Alarmı : " + GetEventStatusString(CameraLog.tamperingDetectionStatus4));

            Console.WriteLine(" ---------------------------------------------- ");
            Console.WriteLine("Kamera 1 Kayıt Durumu : " + GetRecordingStatusString(CameraLog.recordingStatus1));
            Console.WriteLine("Kamera 2 Kayıt Durumu : " + GetRecordingStatusString(CameraLog.recordingStatus2));
            Console.WriteLine("Kamera 3 Kayıt Durumu : " + GetRecordingStatusString(CameraLog.recordingStatus3));
            Console.WriteLine("Kamera 4 Kayıt Durumu : " + GetRecordingStatusString(CameraLog.recordingStatus4));
            Console.WriteLine(" ---------------------------------------------- ");
            Console.WriteLine(" ---------------------------------------------- ");
        }
        private static string GetChannelStatusString(string status)
        {
            if (status == "ON")
            {
                return "Çalışıyor";
            }
            else if (status == "VLOSS")
            {
                return "Görüntü Kaybı";
            }
            else if (status == "DISABLE")
            {
                return "Devre Dışı";
            }
            else if (status == "NOTSUPPORTED")
            {
                return "Desteklenmiyor";
            }
            else
            {
                return "Bilinmiyor";
            }
        }
        private static string GetEventStatusString(string status)
        {
            if (status == "ON")
            {
                return "Açık";
            }
            else if (status == "CLOSE")
            {
                return "Kapalı";
            }
            else if (status == "DISABLE")
            {
                return "Devre Dışı";
            }
            else if (status == "NOTSUPPORTED")
            {
                return "Desteklenmiyor";
            }
            else
            {
                return "Bilinmiyor";
            }
        }
        private static string GetRecordingStatusString(string status)
        {
            if (status == "RECORDING OFF")
            {
                return "KAYITTA DEĞİL";
            }
            else if (status == "RECORDING ON")
            {
                return "KAYITTA";
            }
            else
            {
                return "Bilinmiyor";
            }
        }
    }
}
