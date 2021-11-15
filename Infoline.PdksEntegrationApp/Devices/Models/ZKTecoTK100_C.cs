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
using Infoline.PdksEntegrationApp.Devices;

namespace Infoline.PdksEntegrationApp.Devices.Models
{
    public class ZKTecoTK100_C : PdksDevice, IDisposable
    {
    
        public override void saveUsers()
        {
            int count = 0;
            var deviceUsers = this.GetUsers().OrderBy((a => a.UserDeviceId));
            foreach (var user in deviceUsers)
            {
                var tmp = ZKTecoTK100_C.db.GetSH_ShiftTrackingDeviceUsersByDeviceIdAndDeviceUserId(this.id,user.UserDeviceId);
                if (tmp == null)
                {
                    var rs = ZKTecoTK100_C.db.InsertSH_ShiftTrackingDeviceUsers(new SH_ShiftTrackingDeviceUsers { 
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

        public override bool insertLogsToDB()
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
                short shiftStatus = specifyStatus(log.inOutMode);
                var ShiftTrackingDeviceUser = ZKTecoTK100_C.db.GetSH_ShiftTrackingDeviceUsersByDeviceIdAndDeviceUserId(this.id, log.UserDeviceId.ToString());
                if (ShiftTrackingDeviceUser != null && ShiftTrackingDeviceUser.userId.HasValue)
                {
                    rs &= ZKTecoTK100_C.db.InsertSH_ShiftTracking(new SH_ShiftTracking
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
                    rs &= ZKTecoTK100_C.db.InsertSH_ShiftTracking(new SH_ShiftTracking
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

        private short specifyStatus(int inOrOut)
        {
            switch (inOrOut)
            {
                case 0:
                    return (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi;
                case 1:
                    return (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBitti;
                case 4:
                    return (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaVerildi;
                case 5:
                    return (short)EnumSH_ShiftTrackingShiftTrackingStatus.MolaBitti;
            }
            return (short)EnumSH_ShiftTrackingShiftTrackingStatus.MesaiBaslandi;
        }

        private ResultStatus setLastInsertTimeToFile(DateTime dateTime)
        {
            var rs = new ResultStatus { result = false, objects = null };
            String line;
            List<String> lines = new List<String>();
            try
            {
                bool tmp = false;
                StreamReader sr = new StreamReader("notes.txt");
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
                File.WriteAllLines("notes.txt", lines);
                
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
                StreamReader sr = new StreamReader("notes.txt");
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

        public override bool AddUserToDevice(string userName, int password, int id, int privelage = 0)
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

        public override bool Beep(int DelayMs)
        {
            return device.Beep(DelayMs);
        }

        public override bool ClearAllLog()
        {
            return device.ClearGLog(this.MachineNumber.Value);
        }

        public override bool ClearAttendanceRecord()
        {
            int iDataFlag = 1;

            if (device.ClearData(this.MachineNumber.Value, iDataFlag))
                return device.RefreshData(this.MachineNumber.Value);
            else
            {
                return false;
            }
        }

        public override bool ClearFingerprintTemplate()
        {
            int iDataFlag = 2;

            if (device.ClearData(this.MachineNumber.Value, iDataFlag))
                return device.RefreshData(this.MachineNumber.Value);
            else
            {
                return false;
            }
        }

        public override bool ClearUsers()
        {
            int iDataFlag = 5;

            if (device.ClearData(this.MachineNumber.Value, iDataFlag))
                return device.RefreshData(this.MachineNumber.Value);
            else
            {
                return false;
            }
        }

        public override bool Connect()
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

        public override void Disconnect()
        {
            device.OnConnected -= OnConnected_Event;
            device.OnDisConnected -= OnDisConnected_Event;
            device.OnEnrollFinger -= OnEnrollFinger_Event;
            device.OnFinger -= OnFinger_Event;
            device.OnAttTransactionEx -= new _IZKEMEvents_OnAttTransactionExEventHandler(OnAttTransactionEx_Event);
            device.Disconnect();
        }

        public override string GetDeviceInfo()
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

        public override ICollection<LogInfo> GetLogData()
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
                objInfo.inOutMode = dwInOutMode;

                lstEnrollData.Add(objInfo);
            }

            return lstEnrollData;
        }

        public override ICollection<LogInfo> GetLogDataBetweenDates(DateTime firstDate, DateTime endDate)
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
                    objInfo.inOutMode = dwInOutMode;

                    lstEnrollData.Add(objInfo);
                }

            }
            device.EnableDevice(this.MachineNumber.Value, true);


            return lstEnrollData;
        }

        public override ICollection<LogInfo> GetLogDataBetweenDatesAndUserId(DateTime firstDate, DateTime endDate, int userId)
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
                    objInfo.inOutMode = dwInOutMode;

                    lstEnrollData.Add(objInfo);
                }

            }

            return lstEnrollData;
        }

        public override ICollection<LogInfo> GetLogDataByUserId(int userId)
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
                    objInfo.logType = dwVerifyMode == (int)LogType.Parmakİzi ? "Parmak İzi" : "Şifre";
                    objInfo.MachineNumber = this.MachineNumber.Value;
                    objInfo.UserDeviceId = int.Parse(dwUserId);
                    objInfo.DateTimeRecord = date;
                    objInfo.inOutMode = dwInOutMode;

                    lstEnrollData.Add(objInfo);
                }

            }

            return lstEnrollData;
        }

        public override UserInfo GetUserById(int userId)
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
        public override List<int> GetUserIds()
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

        public override bool DeleteFingerRecordsById(int userId)
        {
            return device.DeleteEnrollData(this.MachineNumber.Value, userId, this.MachineNumber.Value, 11);
        }
        public override int getUserFingerTemplateRecordsCount(int userId)
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
        public override ICollection<UserInfo> GetUsers()
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

        public override bool isConnected()
        {
            return Connect();
        }

        public override bool RestartDevice()
        {
            return device.RestartDevice(this.MachineNumber.Value);
        }

        public override bool pingDevice()
        {
            return UniversalStatic.PingTheDevice(this.IPAddress);
        }

        public override DateTime GetDate()
        {
            int dwYear = 0, dwMonth = 0, dwDay = 0, dwHour = 0, dwMinute = 0, dwSecond = 0;
            device.GetDeviceTime(this.MachineNumber.Value, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
            return new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
        }

        public override bool setDate(DateTime date)
        {
            return device.SetDeviceTime2(this.MachineNumber.Value, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

        public override string GetLastError()
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

        public override bool RemoveUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public override bool lockDevice()
        {
            return device.EnableDevice(this.MachineNumber.Value, false);
        }

        public override bool unlockDevice()
        {
            return device.EnableDevice(this.MachineNumber.Value, true);
        }

        public override int getMachineNumber()
        {
            return this.MachineNumber.Value;
        }

        public override void setMachineNumber(int MachineNumber)
        {
            this.MachineNumber = MachineNumber;
        }

        public override bool setUserPassword(int userId, int password)
        {
            var user = GetUserById(userId);
            if (user == null) return false;
            return device.SSR_SetUserInfo(this.MachineNumber.Value, userId.ToString(), user.Name, password.ToString(), user.Privelage, user.Enabled);
        }

        public override bool setUserName(int userId, string username)
        {
            var user = GetUserById(userId);
            if (user == null) return false;

            return device.SSR_SetUserInfo(this.MachineNumber.Value, userId.ToString(), username, user.Password, user.Privelage, user.Enabled);
        }

        public override int getPort()
        {
            return this.Port.Value;
        }

        public override void setPort(int Port)
        {
            this.Port = Port;
        }

        public override string getIpAddress()
        {
            string ip = "";
            device.GetDeviceIP(this.MachineNumber.Value, ref ip);
            return ip;
        }

        public override bool setIpAddress(string IPAdd)
        {
            return device.SetDeviceIP(this.MachineNumber.Value, IPAddress);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
