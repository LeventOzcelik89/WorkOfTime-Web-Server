using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zkemkeeper;
using System.Configuration;
using Infoline.PdksEntegrationApp.Utils;
using Infoline.PdksEntegrationApp.Models;
using Infoline.WorkOfTime.BusinessData;
using Infoline.WorkOfTime.BusinessAccess;
using System.Threading;
using System.IO;
using Infoline.Framework.Database;

namespace Infoline.PdksEntegrationApp.ZKTEcoSF300Entegration
{
    public class ZKTecoSF300 : SH_ShiftTrackingDevice, IDeviceManipulator, IDisposable
    {
        private static WorkOfTimeDatabase db { get; set; }
        public CZKEM device = new CZKEM();

        public ZKTecoSF300()
        {
            var tenantCode = ConfigurationManager.AppSettings["DefaultTenant"].ToString();
            var tenant = TenantConfig.GetTenants().Where(a => a.TenantCode == Convert.ToInt32(tenantCode)).FirstOrDefault();
            db = tenant.GetDatabase();
        }

        public void Run()
        {
            var saveLogTimeInterval = int.Parse(ConfigurationManager.AppSettings["insertLogsTimeMinute"]);
            var syncUsersTimeInterval = int.Parse(ConfigurationManager.AppSettings["syncUsersMinute"]);
            var lastSaveLogTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
            var lastUserSync = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

            bool isFirstRun = true;
            while (true)
            {
                var isConn = false;
                if (this.Connect())
                {
                    Log.Success(this.DeviceSerialNo + " seri numaralı cihaz ile bağlantı kuruldu");
                    isConn = true;

                    if (isFirstRun)
                    {
                        saveUsers();
                        lastUserSync = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

                        insertLogsToDB();
                        lastSaveLogTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);

                        isFirstRun = false;
                    }
                }
                else
                {
                    Log.Error(this.DeviceSerialNo + " seri numaralı cihaz ile bağlantı kurulamıyor");
                    isConn = false;
                }
                try
                {
                    while (isConn)
                    {                      
                        var _lastExecute = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        if(lastSaveLogTime.AddMinutes(saveLogTimeInterval) <= _lastExecute)
                        {
                            insertLogsToDB();
                            lastSaveLogTime = _lastExecute;
                        }
                        if (lastUserSync.AddMinutes(syncUsersTimeInterval) <= _lastExecute)
                        {
                            saveUsers();
                            lastUserSync = _lastExecute;
                        }
                        Thread.Sleep(new TimeSpan(0, 1, 0));
                        isConn = isConnected();
                    }
                }
                catch (Exception ex)
                {
                    Log.Info(this.DeviceSerialNo + " Nolu cihazdaki ile ilgili sorunlar oluştu. Mesaj : " + ex.Message);
                }

                Thread.Sleep(new TimeSpan(0, 0, 10));
            }
        }

       
        private void saveUsers()
        {
            int count = 0;
            var deviceUsers = this.GetUsers().OrderBy((a => a.UserDeviceId));
            foreach (var user in deviceUsers)
            {
                var tmp = ZKTecoSF300.db.GetSH_ShiftTrackingDeviceUsersByDeviceIdAndDeviceUserId(this.id,user.UserDeviceId);
                if (tmp == null)
                {
                    var rs = ZKTecoSF300.db.InsertSH_ShiftTrackingDeviceUsers(new SH_ShiftTrackingDeviceUsers { 
                        id = Guid.NewGuid(),
                        deviceId = this.id,
                        deviceUserId = user.UserDeviceId
                    });

                    if (rs.result)
                    {
                        count++;
                    }
                    else
                    {
                        Log.Error(this.id + "Id'li cihaz için " + user.UserDeviceId + "cihaz Id'li kullanıcı sisteme eklenemedi");
                    }
                }
            }

            Log.Info(count + " kullanıcı sisteme kaydedildi");

        }

        private bool insertLogsToDB()
        {
            var rs = new ResultStatus { result = true };
            var trans = db.BeginTransaction();

            var firstLogTimeResulStatus = getLastInsertTimeFromFile();
            var lastLogTime = DateTime.Now;

            if (!firstLogTimeResulStatus.result)
            {
                return false;
            }

            DateTime firstLogTime = DateTime.Parse(firstLogTimeResulStatus.objects.ToString());
            var logs = this.GetLogDataBetweenDates(firstLogTime, lastLogTime);
            foreach (var log in logs)
            {
                short shiftStatus = specifyStatus(log.DateTimeRecord);
                var ShiftTrackingDeviceUser = ZKTecoSF300.db.GetSH_ShiftTrackingDeviceUsersByDeviceIdAndDeviceUserId(this.id, log.UserDeviceId.ToString());
                if (ShiftTrackingDeviceUser != null && ShiftTrackingDeviceUser.userId.HasValue)
                {
                    rs &= ZKTecoSF300.db.InsertSH_ShiftTracking(new SH_ShiftTracking
                    {
                        id = Guid.NewGuid(),
                        userId = ShiftTrackingDeviceUser.userId.Value,
                        shiftTrackingDeviceId = this.id,
                        passType = log.logType == "Parmak İzi" ? 1 : 2,
                        deviceUserId = log.UserDeviceId.ToString(),
                        timestamp = log.DateTimeRecord,
                        shiftTrackingStatus = shiftStatus
                    }, trans);
                }
                else
                {
                    rs &= ZKTecoSF300.db.InsertSH_ShiftTracking(new SH_ShiftTracking
                    {
                        id = Guid.NewGuid(),
                        shiftTrackingDeviceId = this.id,
                        passType = log.logType == "Parmak İzi" ? 1 : 2,
                        deviceUserId = log.UserDeviceId.ToString(),
                        timestamp = log.DateTimeRecord,
                        shiftTrackingStatus = shiftStatus
                    }, trans);
                }

            }

            if (rs.result)
            {
                trans.Commit();
                Log.Success(this.DeviceSerialNo + " Seri Numaralı Cihazın " + firstLogTime + " - " + lastLogTime + " Tarihi Arası Kayıtları (" + logs.Count() + " adet)  Kaydedilmiştir..");
                setLastInsertTimeToFile(lastLogTime);
            }
            else
            {
                trans.Rollback();
                Log.Error(this.DeviceSerialNo + " Seri Numaralı Cihazın " + firstLogTime + " - " + lastLogTime + " Tarihi Arası Kayıtları (" + logs.Count() + " adet) Kaydedilememiştir..");
            }

            return rs.result;

        }

        private short specifyStatus(DateTime dateTimeRecord)
        {
            return (dateTimeRecord.Hour >= 4 && dateTimeRecord.Hour <= 12) == true ? (short)0 : (short)1;
        }

        private ResultStatus setLastInsertTimeToFile(DateTime dateTime)
        {
            var rs = new ResultStatus { result = false, objects = null };
            String line;
            List<String> lines = new List<String>();
            try
            {
                bool tmp = false;
                StreamReader sr = new StreamReader("../../notes.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    if (line.StartsWith(this.id.ToString()))
                    {
                        if (line.Split('|')[1] == "lastInsertTime")
                        {
                            line = this.id + "|" + "lastInsertTime|" + dateTime.ToString();
                            tmp = true;
                        }
                    }
                    lines.Add(line);
                    line = sr.ReadLine();
                }
                if (!tmp)
                {
                    lines.Add(this.id + "|" + "lastInsertTime|" + dateTime.ToString());
                }

                sr.Close();
                File.WriteAllLines("../../notes.txt", lines);
                
            }
            catch (Exception e)
            {
                rs.message = "Dosyaya son database'e kayıt zamanı yazma başarırız... Exception: " + e.Message;
                Log.Error(rs.message);
                return rs;
            }

            rs.result = true;
            return rs;
        }

        private ResultStatus getLastInsertTimeFromFile()
        {
            var rs = new ResultStatus { result = true };
            String line;
            var lastInsertDateTime = new DateTime(1990, 1, 1);

            try
            {
                StreamReader sr = new StreamReader("../../notes.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    if (line.StartsWith(this.id.ToString()))
                    {
                        if (line.Split('|')[1] == "lastInsertTime")
                        {
                            lastInsertDateTime = DateTime.Parse(line.Split('|')[2]);
                            break;
                        }
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                rs.message = "Dosyadan son database'e kayıt zamanı okuma başarırız... Veritabanına kayıt aktarılmadı... Exception: " + e.Message;
                Log.Error(rs.message);
                rs.result = false;
            }

            rs.objects = lastInsertDateTime.ToString();
            return rs;
        }

        public bool AddUserToDevice(string userName, int password, int id, int privelage = 0)
        {
            if (checkUniqueId(id))
            {
                return device.SSR_SetUserInfo(this.MachineNumber.Value, id.ToString(), userName, password.ToString(), privelage, true);
            }
            return false; //aynı idli kayıt var

        }

        private bool checkUniqueId(int id)
        {
            string checkId = id.ToString();
            string UserId = string.Empty, sName = string.Empty, sPassword = string.Empty, sTmpData = string.Empty;
            int iPrivilege = 0;
            bool bEnabled = false;


            device.ReadAllUserID(this.MachineNumber.Value);
            device.ReadAllTemplate(this.MachineNumber.Value);

            while (device.SSR_GetAllUserInfo(this.MachineNumber.Value, out UserId, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                if (checkId == UserId) return false;
            }

            return true;
        }

        public bool Beep(int DelayMs)
        {
            return device.Beep(DelayMs);
        }

        public bool ClearAllLog()
        {
            return device.ClearGLog(this.MachineNumber.Value);
        }

        public bool ClearAttendanceRecord()
        {
            int iDataFlag = 1;

            if (device.ClearData(this.MachineNumber.Value, iDataFlag))
                return device.RefreshData(this.MachineNumber.Value);
            else
            {
                return false;
            }
        }

        public bool ClearFingerprintTemplate()
        {
            int iDataFlag = 2;

            if (device.ClearData(this.MachineNumber.Value, iDataFlag))
                return device.RefreshData(this.MachineNumber.Value);
            else
            {
                return false;
            }
        }

        public bool ClearUsers()
        {
            int iDataFlag = 5;

            if (device.ClearData(this.MachineNumber.Value, iDataFlag))
                return device.RefreshData(this.MachineNumber.Value);
            else
            {
                return false;
            }
        }

        public bool Connect()
        {
            if (!UniversalStatic.ValidateIP(this.IPAddress))
            {
                return false;
            }

            if (device.Connect_Net(this.IPAddress, this.Port.Value))
            {

                //65535, 32767
                if (device.RegEvent(1, 32767))
                {
                    // [ Register your events here ]
                    // [ Go through the _IZKEMEvents_Event class for a complete list of events
                    device.OnConnected += OnConnected_Event;
                    device.OnDisConnected += OnDisConnected_Event;
                    device.OnEnrollFinger += OnEnrollFinger_Event;
                    device.OnFinger += OnFinger_Event;
                    device.OnAttTransactionEx += new _IZKEMEvents_OnAttTransactionExEventHandler(OnAttTransactionEx_Event);
                }
                setDate(DateTime.Now);
                return true;
            }
            return false;
        }

        private void OnEnrollFinger_Event(int userId, int FingerIndex, int ActionResult, int TemplateLength)
        {
        }

        private void OnDisConnected_Event()
        {
        }

        private void OnConnected_Event()
        {
        }

        private void OnFinger_Event()
        {
        }
        private void OnAttTransactionEx_Event(string userId, int IsInValid, int AttState, int VerifyMethod, int Year, int Month, int Day, int Hour, int Minute, int Second, int WorkCode)
        {
        }

        public void Disconnect()
        {
            device.OnConnected -= OnConnected_Event;
            device.OnDisConnected -= OnDisConnected_Event;
            device.OnEnrollFinger -= OnEnrollFinger_Event;
            device.OnFinger -= OnFinger_Event;
            device.OnAttTransactionEx -= new _IZKEMEvents_OnAttTransactionExEventHandler(OnAttTransactionEx_Event);
            device.Disconnect();
        }

        public string GetDeviceInfo()
        {
            StringBuilder sb = new StringBuilder();

            string returnValue = string.Empty;


            device.GetFirmwareVersion(this.MachineNumber.Value, ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("Firmware V: ");
                sb.Append(returnValue);
                sb.Append(",");
            }


            returnValue = string.Empty;
            device.GetVendor(ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("\nVendor: ");
                sb.Append(returnValue);
                sb.Append(",");
            }

            string sWiegandFmt = string.Empty;
            device.GetWiegandFmt(this.MachineNumber.Value, ref sWiegandFmt);

            returnValue = string.Empty;
            device.GetSDKVersion(ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("\nSDK V: ");
                sb.Append(returnValue);
                sb.Append(",");
            }

            returnValue = string.Empty;
            device.GetSerialNumber(this.MachineNumber.Value, out returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("\nSerial No: ");
                sb.Append(returnValue);
                sb.Append(",");
            }

            returnValue = string.Empty;
            device.GetDeviceMAC(this.MachineNumber.Value, ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("\nDevice MAC: ");
                sb.Append(returnValue);
            }

            return sb.ToString();
        }

        public ICollection<LogInfo> GetLogData()
        {
            string dwUserId = "";
            int dwVerifyMode = 0;
            int dwInOutMode = 0;
            int dwYear = 0;
            int dwMonth = 0;
            int dwDay = 0;
            int dwHour = 0;
            int dwMinute = 0;
            int dwSecond = 0;
            int dwWorkCode = 0;

            ICollection<LogInfo> lstEnrollData = new List<LogInfo>();

            device.ReadAllGLogData(this.MachineNumber.Value);

            while (device.SSR_GetGeneralLogData(this.MachineNumber.Value, out dwUserId, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
            {

                LogInfo objInfo = new LogInfo();
                objInfo.MachineNumber = this.MachineNumber.Value;
                objInfo.logType = dwVerifyMode == (int)LogType.Parmakİzi ? "Parmak İzi" : "Şifre";
                objInfo.UserDeviceId = int.Parse(dwUserId);
                objInfo.DateTimeRecord = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);

                lstEnrollData.Add(objInfo);
            }

            return lstEnrollData;
        }

        public ICollection<LogInfo> GetLogDataBetweenDates(DateTime firstDate, DateTime endDate)
        {
            string dwUserId = "";
            int dwVerifyMode = 0;
            int dwInOutMode = 0;
            int dwYear = 0;
            int dwMonth = 0;
            int dwDay = 0;
            int dwHour = 0;
            int dwMinute = 0;
            int dwSecond = 0;
            int dwWorkCode = 0;

            ICollection<LogInfo> lstEnrollData = new List<LogInfo>();

            device.EnableDevice(this.MachineNumber.Value, false);
            var b = device.ReadAllGLogData(this.MachineNumber.Value);

            while (device.SSR_GetGeneralLogData(this.MachineNumber.Value, out dwUserId, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
            {
                var date = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
                if (date >= firstDate && date <= endDate)
                {
                    LogInfo objInfo = new LogInfo();
                    objInfo.MachineNumber = this.MachineNumber.Value;
                    objInfo.logType = dwVerifyMode == (int)LogType.Parmakİzi ? "Parmak İzi" : "Şifre";
                    objInfo.UserDeviceId = int.Parse(dwUserId);
                    objInfo.DateTimeRecord = date;

                    lstEnrollData.Add(objInfo);
                }

            }
            device.EnableDevice(this.MachineNumber.Value, true);


            return lstEnrollData;
        }

        public ICollection<LogInfo> GetLogDataBetweenDatesAndUserId(DateTime firstDate, DateTime endDate, int userId)
        {
            string dwUserId = "";
            int dwVerifyMode = 0;
            int dwInOutMode = 0;
            int dwYear = 0;
            int dwMonth = 0;
            int dwDay = 0;
            int dwHour = 0;
            int dwMinute = 0;
            int dwSecond = 0;
            int dwWorkCode = 0;

            ICollection<LogInfo> lstEnrollData = new List<LogInfo>();

            device.ReadAllGLogData(this.MachineNumber.Value);

            while (device.SSR_GetGeneralLogData(this.MachineNumber.Value, out dwUserId, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
            {
                var date = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
                if (userId == int.Parse(dwUserId) && date >= firstDate && date <= endDate)
                {
                    LogInfo objInfo = new LogInfo();
                    objInfo.MachineNumber = this.MachineNumber.Value;
                    objInfo.logType = dwVerifyMode == (int)LogType.Parmakİzi ? "Parmak İzi" : "Şifre";
                    objInfo.UserDeviceId = int.Parse(dwUserId);
                    objInfo.DateTimeRecord = date;

                    lstEnrollData.Add(objInfo);
                }

            }

            return lstEnrollData;
        }

        public ICollection<LogInfo> GetLogDataByUserId(int userId)
        {
            string dwUserId = "";
            int dwVerifyMode = 0;
            int dwInOutMode = 0;
            int dwYear = 0;
            int dwMonth = 0;
            int dwDay = 0;
            int dwHour = 0;
            int dwMinute = 0;
            int dwSecond = 0;
            int dwWorkCode = 0;

            ICollection<LogInfo> lstEnrollData = new List<LogInfo>();

            device.ReadAllGLogData(this.MachineNumber.Value);

            while (device.SSR_GetGeneralLogData(this.MachineNumber.Value, out dwUserId, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
            {
                var date = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
                if (userId == int.Parse(dwUserId))
                {
                    LogInfo objInfo = new LogInfo();
                    objInfo.MachineNumber = this.MachineNumber.Value;
                    objInfo.UserDeviceId = int.Parse(dwUserId);
                    objInfo.DateTimeRecord = date;

                    lstEnrollData.Add(objInfo);
                }

            }

            return lstEnrollData;
        }

        public UserInfo GetUserById(int userId)
        {
            string UserId = string.Empty, sName = string.Empty, sPassword = string.Empty, sTmpData = string.Empty;
            int iPrivilege = 0, iFlag = 0;
            bool bEnabled = false;


            device.ReadAllUserID(this.MachineNumber.Value);
            device.ReadAllTemplate(this.MachineNumber.Value);

            while (device.SSR_GetAllUserInfo(this.MachineNumber.Value, out UserId, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                if (userId == int.Parse(UserId))
                {
                    UserInfo fpInfo = new UserInfo();
                    fpInfo.MachineNumber = this.MachineNumber.Value;
                    fpInfo.UserDeviceId = UserId;
                    fpInfo.Name = sName;
                    fpInfo.TmpData = sTmpData;
                    fpInfo.Privelage = iPrivilege;
                    fpInfo.Password = sPassword;
                    fpInfo.Enabled = bEnabled;
                    fpInfo.iFlag = iFlag.ToString();

                    return fpInfo;
                }
            }
            return null;
        }
        public List<int> GetUserIds()
        {
            List<int> userIds = new List<int>();
            string UserId = string.Empty, sName = string.Empty, sPassword = string.Empty, sTmpData = string.Empty;
            int iPrivilege = 0;
            bool bEnabled = false;


            device.ReadAllUserID(this.MachineNumber.Value);
            while (device.SSR_GetAllUserInfo(this.MachineNumber.Value, out UserId, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                userIds.Add(int.Parse(UserId));
            }

            return userIds;
        }

        public bool DeleteFingerRecordsById(int userId)
        {
            return device.DeleteEnrollData(this.MachineNumber.Value, userId, this.MachineNumber.Value, 11);
        }
        public int getUserFingerTemplateRecordsCount(int userId)
        {
            string UserId = string.Empty, sName = string.Empty, sPassword = string.Empty, sTmpData = string.Empty;
            int iPrivilege = 0, iTmpLength = 0, iFlag = 0, idwFingerIndex = 0;
            bool bEnabled = false;

            int count = 0;

            ICollection<UserInfo> lstFPTemplates = new List<UserInfo>();

            device.ReadAllUserID(this.MachineNumber.Value);
            device.ReadAllTemplate(this.MachineNumber.Value);

            while (device.SSR_GetAllUserInfo(this.MachineNumber.Value, out UserId, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                {
                    if (device.GetUserTmpExStr(this.MachineNumber.Value, UserId, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength) && UserId == userId.ToString())
                    {
                        count++;
                    }
                }
            }

            return count;
        }
        public ICollection<UserInfo> GetUsers()
        {
            string UserId = string.Empty, sName = string.Empty, sPassword = string.Empty, sTmpData = string.Empty;
            int iPrivilege = 0;
            bool bEnabled = false;

            ICollection<UserInfo> lstFPTemplates = new List<UserInfo>();

            device.ReadAllUserID(this.MachineNumber.Value);

            while (device.SSR_GetAllUserInfo(this.MachineNumber.Value, out UserId, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                UserInfo fpInfo = new UserInfo();
                fpInfo.MachineNumber = this.MachineNumber.Value;
                fpInfo.UserDeviceId = UserId;
                fpInfo.Name = sName;
                fpInfo.FingerIndex = 0;
                fpInfo.TmpData = sTmpData;
                fpInfo.Privelage = iPrivilege;
                fpInfo.Password = sPassword;
                fpInfo.Enabled = bEnabled;

                lstFPTemplates.Add(fpInfo);

            }
            return lstFPTemplates;
        }

        public bool isConnected()
        {
            return Connect();
        }

        public bool RestartDevice()
        {
            return device.RestartDevice(this.MachineNumber.Value);
        }

        public bool pingDevice()
        {
            return UniversalStatic.PingTheDevice(this.IPAddress);
        }

        public DateTime GetDate()
        {
            int dwYear = 0, dwMonth = 0, dwDay = 0, dwHour = 0, dwMinute = 0, dwSecond = 0;
            device.GetDeviceTime(this.MachineNumber.Value, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
            return new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
        }

        public bool setDate(DateTime date)
        {
            return device.SetDeviceTime2(this.MachineNumber.Value, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

        public string GetLastError()
        {
            int errorCod = 0;
            device.GetLastError(ref errorCod);
            switch (errorCod)
            {
                case 1: return "BAŞARILI";
                case 4: return "GEÇERSİZ PARAMETRE";
                case 0: return "ERROR";
                case -1: return "ERROR_IO ";
                case -2: return "ERROR_SIZE";
                case -3: return "ERROR";
                case -4: return "YETERSİZ HAFIZA";
                case -100: return "ERROR";
                default: return "HATA";

            }
        }

        public bool RemoveUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public bool lockDevice()
        {
            return device.EnableDevice(this.MachineNumber.Value, false);
        }

        public bool unlockDevice()
        {
            return device.EnableDevice(this.MachineNumber.Value, true);
        }

        public int getMachineNumber()
        {
            return this.MachineNumber.Value;
        }

        public void setMachineNumber(int MachineNumber)
        {
            this.MachineNumber = MachineNumber;
        }

        public bool setUserPassword(int userId, int password)
        {
            var user = GetUserById(userId);
            if (user == null) return false;
            return device.SSR_SetUserInfo(this.MachineNumber.Value, userId.ToString(), user.Name, password.ToString(), user.Privelage, user.Enabled);
        }

        public bool setUserName(int userId, string username)
        {
            var user = GetUserById(userId);
            if (user == null) return false;

            return device.SSR_SetUserInfo(this.MachineNumber.Value, userId.ToString(), username, user.Password, user.Privelage, user.Enabled);
        }

        public int getPort()
        {
            return this.Port.Value;
        }

        public void setPort(int Port)
        {
            this.Port = Port;
        }

        public string getIpAddress()
        {
            string ip = "";
            device.GetDeviceIP(this.MachineNumber.Value, ref ip);
            return ip;
        }

        public bool setIpAddress(string IPAdd)
        {
            return device.SetDeviceIP(this.MachineNumber.Value, IPAddress);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
