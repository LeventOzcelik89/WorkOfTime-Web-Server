using Infoline.WorkOfTime.BusinessAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infoline.WorkOfTime.ApplicationUpdater
{
    public class Program
    {
        public static DateTime LastRunTime { get; set; }
        public static string ServicesURL { get; set; }
        public static string RootPath { get; set; }
        public static string RootBackupPath { get; set; }
        public static string SiteUrl { get; set; }

        static void Main(string[] args)
        {
            Log.Success("WorkOfTime Update Uygulaması başladı.");
            ServicesURL = System.Configuration.ConfigurationManager.AppSettings["ServicesURL"];
            RootPath = System.Configuration.ConfigurationManager.AppSettings["RootPath"];
            RootBackupPath = System.Configuration.ConfigurationManager.AppSettings["BackupPath"];
            SiteUrl = System.Configuration.ConfigurationManager.AppSettings["SiteUrl"];

            Log.Info("Servis URL: " + ServicesURL);
            Log.Info("Uygulamalar Ana Dizin: " + RootPath);
            Log.Info("Uygulamalar Yedekleme Dizini: " + RootBackupPath);

            if (ServicesCheck())
            {
                //  StopApplication();
                FileUpdater();
                //  ExConfigUpdate();
                //  StartApplication();

                UpdateVersionCode();

            }
            Console.ReadLine();
        }

        public static void UpdateVersionCode()
        {

            try
            {
                var fileDir = RootPath + "/" + "web.config";
                if (File.Exists(fileDir))
                {
                    var fileText = File.ReadAllText(fileDir);

                    var regx = new Regex("(<add key=\"VersionCode\" value=\"(.*)\" \\/>)");
                    var rs = regx.Match(fileText);

                    if (rs.Success)
                    {

                        var newValue = Guid.NewGuid().ToString().Substring(0, 8);
                        fileText = fileText.Replace(rs.Value, "<add key=\"VersionCode\" value=\"" + newValue + "\" />");

                        File.WriteAllText(fileDir, fileText);

                    }

                    Log.Success("web.config Güncellendi !");

                }
            }
            catch (Exception ex)
            {
                Log.Error("web.config Güncellenemedi :" + ex.Message);
            }

        }

        public static bool ServicesCheck()
        {

            using (var client = new WebClient())
            {
                try
                {
                    var str = client.DownloadString(ServicesURL + "/General/GetTime");
                    if (!string.IsNullOrEmpty(str))
                    {
                        Log.Success(ServicesURL + " Servisle iletişim başarılı.");
                        return true;
                    }
                    else
                    {
                        Log.Error(ServicesURL + " Servisle iletişim başarısız.Lütfen sistem yöneticinizle görüşünüz.");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ServicesURL + " Servisle iletişim başarısız.Lütfen sistem yöneticinizle görüşünüz.Mesaj : " + ex);
                    return false;
                }
            }
        }

        public static void FileUpdater()
        {

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                var serverDate = DateTime.Now;
                try
                {
                    serverDate = Infoline.Helper.Json.Deserialize<DateTime>(client.DownloadString(ServicesURL + "/General/GetTime"));
                }
                catch { }


                if (DateTime.Now.ToString("dd.MM.yyy HH:mm") != serverDate.ToString("dd.MM.yyy HH:mm"))
                {
                    Log.Info("Sunucu saati Uzak sunucu saatinde farklı.");
                    var rs = WinTime.SetLocalTime(serverDate);
                    if (rs)
                    {
                        Log.Success("Sunucu saati Uzak sunucu saati göre ayarlandı.");
                    }
                    else
                    {
                        Log.Error("Sunucu saati güncelleme işlemi başarısız.");
                    }
                }
                var BackupPath = Path.Combine(RootPath, "Files", "Backups");
                if (!Directory.Exists(BackupPath))
                {
                    Directory.CreateDirectory(BackupPath);
                }

                var UserTransactionsPath = Path.Combine(RootPath, "Files", "UserTransactions");
                if (!Directory.Exists(UserTransactionsPath))
                {
                    Directory.CreateDirectory(UserTransactionsPath);
                }


                var backupPath = Path.Combine(RootBackupPath, DateTime.Now.ToString("dd-MM-yyyy-HH-mm"));
                var serverFiles = Infoline.Helper.Json.Deserialize<Dictionary<string, DateTime>>(client.DownloadString(ServicesURL + "/General/GetFiles"));
                var i = 0;


                foreach (var serverfile in serverFiles)
                {

                    var filePath = Path.Combine(RootPath, serverfile.Key);
                    var extension = Path.GetExtension(filePath);
                    var directory = Path.GetDirectoryName(filePath);
                    var fileName = Path.GetFileName(filePath);
                    var downloadURL = ServicesURL + "/General/GetFile?path=" + serverfile.Key;

                    try
                    {
                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        if (!File.Exists(filePath))
                        {
                            client.DownloadFile(downloadURL, filePath);
                            Log.Success(serverfile.Key + " Dosyası İndirildi");
                            i++;
                        }
                        else
                        {
                            var info = new FileInfo(filePath);
                            if (info.LastWriteTime < serverfile.Value && extension != ".config")
                            {

                                var fileBackupPath = Path.Combine(backupPath, serverfile.Key);
                                var directoryBackupPath = Path.GetDirectoryName(fileBackupPath);
                                if (!Directory.Exists(directoryBackupPath))
                                {
                                    Directory.CreateDirectory(directoryBackupPath);
                                }
                                File.Copy(filePath, fileBackupPath);
                                Log.Success(serverfile.Key + " Dosyası yedeklemesi yapıldı.");
                                client.DownloadFile(downloadURL, filePath);
                                Log.Success(serverfile.Key + " Dosyası güncellendi.");
                                i++;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(serverfile.Key + " Hata Oluştu.Mesaj:" + ex.Message);
                    }

                }

                if (i > 0)
                {
                    Log.Warning("Sunucu üzerinde (" + i + ") adet dosya bulundu ve değişiklikler indirildi.");

                    try
                    {
                        var clientVersion = Infoline.Helper.Json.Deserialize<Versions>(client.DownloadString(SiteUrl + "/General/GetInformation"));
                        var clientSaveVersion = client.DownloadString(ServicesURL + "/General/GetSaveVersion?SiteUrl=" + clientVersion.SiteUrl + "&Version=" + clientVersion.Version + "&Root=" + RootBackupPath);
                        Log.Warning("Versiyon güncelleme işlemi başarılı");
                    }
                    catch (Exception ex)
                    {
                        Log.Warning(ex.Message);
                        throw;
                    }
                }
                else
                {
                    Log.Warning("Sunucu üzerinde herhangi bir değişiklik bulunamadı.");
                }

                try
                {
                    client.DownloadString("http://localhost/Account/Execute");
                }
                catch { }
            }
        }
    }

    public class Versions
    {
        public string Version { get; set; }
        public string SiteUrl { get; set; }
    }

    public static class WinTime
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public ushort wYear;
            public ushort wMonth;
            public ushort wDayOfWeek;
            public ushort wDay;
            public ushort wHour;
            public ushort wMinute;
            public ushort wSecond;
            public ushort wMilliseconds;
        }

        [DllImport("kernel32.dll", EntryPoint = "SetLocalTime", SetLastError = true)]
        private extern static bool Win32SetLocalTime(ref SYSTEMTIME lpSystemTime);
        public static bool SetLocalTime(DateTime sysTime)
        {
            SYSTEMTIME systime = new SYSTEMTIME();
            systime.wYear = (ushort)sysTime.Year;
            systime.wMonth = (ushort)sysTime.Month;
            systime.wDayOfWeek = (ushort)sysTime.DayOfWeek;
            systime.wDay = (ushort)sysTime.Day;
            systime.wHour = (ushort)sysTime.Hour;
            systime.wMinute = (ushort)sysTime.Minute;
            systime.wSecond = (ushort)sysTime.Second;
            systime.wMilliseconds = (ushort)sysTime.Millisecond;

            return Win32SetLocalTime(ref systime);
        }
    }

}
