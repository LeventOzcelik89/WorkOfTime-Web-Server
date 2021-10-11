using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zkemkeeper;
using System.Configuration;
using Infoline.PdksEntegrationApp.Utils;
using Infoline.PdksEntegrationApp.Models;

namespace Infoline.PdksEntegrationApp.ZKTEcoSF300Entegration
{
    public class ZKTecoSF300 : IDeviceManipulator
    {
        public CZKEM device = new CZKEM();
        int machineNumber { get; set; }
        string IPAdd { get; set; }
        int port { get; set; }
        bool isConn { get; set; }

        public ZKTecoSF300(string ip, int machineNumber, int port = 4370)
        {
            this.machineNumber = machineNumber;
            this.IPAdd = ip;
            this.port = port;
        }

        public bool AddUserToDevice(string userName, int password, int id, int privelage = 0)
        {
            if (checkUniqueId(id))
            {
                return device.SSR_SetUserInfo(machineNumber, id.ToString(), userName, password.ToString(), privelage, true);
            }
            return false; //aynı idli kayıt var

        }

        private bool checkUniqueId(int id)
        {
            string checkId = id.ToString();
            string UserId = string.Empty, sName = string.Empty, sPassword = string.Empty, sTmpData = string.Empty;
            int iPrivilege = 0;
            bool bEnabled = false;


            device.ReadAllUserID(machineNumber);
            device.ReadAllTemplate(machineNumber);

            while (device.SSR_GetAllUserInfo(machineNumber, out UserId, out sName, out sPassword, out iPrivilege, out bEnabled))
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
            return device.ClearGLog(machineNumber);
        }

        public bool ClearAttendanceRecord()
        {
            int iDataFlag = 1;

            if (device.ClearData(machineNumber, iDataFlag))
                return device.RefreshData(machineNumber);
            else
            {
                return false;
            }
        }

        public bool ClearFingerprintTemplate()
        {
            int iDataFlag = 2;

            if (device.ClearData(machineNumber, iDataFlag))
                return device.RefreshData(machineNumber);
            else
            {
                return false;
            }
        }

        public bool ClearUsers()
        {
            int iDataFlag = 5;

            if (device.ClearData(machineNumber, iDataFlag))
                return device.RefreshData(machineNumber);
            else
            {
                return false;
            }
        }

        public bool Connect()
        {
            if (!UniversalStatic.ValidateIP(IPAdd))
            {
                return false;
            }

            if (device.Connect_Net(IPAdd, port))
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
                isConn = true;
            }
            return isConn;
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
            isConn = false;
        }

        public string GetDeviceInfo()
        {
            StringBuilder sb = new StringBuilder();

            string returnValue = string.Empty;


            device.GetFirmwareVersion(machineNumber, ref returnValue);
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
            device.GetWiegandFmt(machineNumber, ref sWiegandFmt);

            returnValue = string.Empty;
            device.GetSDKVersion(ref returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("\nSDK V: ");
                sb.Append(returnValue);
                sb.Append(",");
            }

            returnValue = string.Empty;
            device.GetSerialNumber(machineNumber, out returnValue);
            if (returnValue.Trim() != string.Empty)
            {
                sb.Append("\nSerial No: ");
                sb.Append(returnValue);
                sb.Append(",");
            }

            returnValue = string.Empty;
            device.GetDeviceMAC(machineNumber, ref returnValue);
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

            device.ReadAllGLogData(machineNumber);

            while (device.SSR_GetGeneralLogData(machineNumber, out dwUserId, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
            {

                LogInfo objInfo = new LogInfo();
                objInfo.MachineNumber = machineNumber;
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

            device.ReadAllGLogData(machineNumber);

            while (device.SSR_GetGeneralLogData(machineNumber, out dwUserId, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
            {
                var date = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
                if (date >= firstDate && date <= endDate)
                {
                    LogInfo objInfo = new LogInfo();
                    objInfo.MachineNumber = machineNumber;
                    objInfo.logType = dwVerifyMode == (int)LogType.Parmakİzi ? "Parmak İzi" : "Şifre";
                    objInfo.UserDeviceId = int.Parse(dwUserId);
                    objInfo.DateTimeRecord = date;

                    lstEnrollData.Add(objInfo);
                }

            }

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

            device.ReadAllGLogData(machineNumber);

            while (device.SSR_GetGeneralLogData(machineNumber, out dwUserId, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
            {
                var date = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
                if (userId == int.Parse(dwUserId) && date >= firstDate && date <= endDate)
                {
                    LogInfo objInfo = new LogInfo();
                    objInfo.MachineNumber = machineNumber;
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

            device.ReadAllGLogData(machineNumber);

            while (device.SSR_GetGeneralLogData(machineNumber, out dwUserId, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
            {
                var date = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
                if (userId == int.Parse(dwUserId))
                {
                    LogInfo objInfo = new LogInfo();
                    objInfo.MachineNumber = machineNumber;
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


            device.ReadAllUserID(machineNumber);
            device.ReadAllTemplate(machineNumber);

            while (device.SSR_GetAllUserInfo(machineNumber, out UserId, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                if (userId == int.Parse(UserId))
                {
                    UserInfo fpInfo = new UserInfo();
                    fpInfo.MachineNumber = machineNumber;
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


            device.ReadAllUserID(machineNumber);
            while (device.SSR_GetAllUserInfo(machineNumber, out UserId, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                userIds.Add(int.Parse(UserId));
            }

            return userIds;
        }

        public bool DeleteFingerRecordsById(int userId)
        {
            return device.DeleteEnrollData(machineNumber, userId, machineNumber, 11);
        }
        public int getUserFingerTemplateRecordsCount(int userId)
        {
            string UserId = string.Empty, sName = string.Empty, sPassword = string.Empty, sTmpData = string.Empty;
            int iPrivilege = 0, iTmpLength = 0, iFlag = 0, idwFingerIndex = 0;
            bool bEnabled = false;

            int count = 0;

            ICollection<UserInfo> lstFPTemplates = new List<UserInfo>();

            device.ReadAllUserID(machineNumber);
            device.ReadAllTemplate(machineNumber);

            while (device.SSR_GetAllUserInfo(machineNumber, out UserId, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                for (idwFingerIndex = 0; idwFingerIndex < 10; idwFingerIndex++)
                {
                    if (device.GetUserTmpExStr(machineNumber, UserId, idwFingerIndex, out iFlag, out sTmpData, out iTmpLength) && UserId == userId.ToString())
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

            device.ReadAllUserID(machineNumber);

            while (device.SSR_GetAllUserInfo(machineNumber, out UserId, out sName, out sPassword, out iPrivilege, out bEnabled))
            {
                UserInfo fpInfo = new UserInfo();
                fpInfo.MachineNumber = machineNumber;
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
            return isConn;
        }

        public bool RestartDevice()
        {
            return device.RestartDevice(machineNumber);
        }

        public bool pingDevice()
        {
            return UniversalStatic.PingTheDevice(IPAdd);
        }

        public DateTime GetDate()
        {
            int dwYear = 0, dwMonth = 0, dwDay = 0, dwHour = 0, dwMinute = 0, dwSecond = 0;
            device.GetDeviceTime(machineNumber, ref dwYear, ref dwMonth, ref dwDay, ref dwHour, ref dwMinute, ref dwSecond);
            return new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);
        }

        public bool setDate(DateTime date)
        {
            return device.SetDeviceTime2(machineNumber, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }

        public string GetLastError()
        {
            int errorCod = 0;
            device.GetLastError(ref errorCod);
            switch (errorCod)
            {
                case 1: return "BAŞARILI";
                case 4: return "GEÇERSİZ PARAMETRE";
                case 0: return "VERİ BULUNAMADI";
                case -1: return "ERROR_IO ";
                case -2: return "ERROR_SIZE";
                case -3: return "VERİ BULUNAMADI";
                case -4: return "YETERSİZ HAFIZA";
                case -100: return "VERİ BULUNAMADI";
                default: return "HATA";

            }
        }

        public bool RemoveUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public bool lockDevice()
        {
            return device.EnableDevice(machineNumber, false);
        }

        public bool unlockDevice()
        {
            return device.EnableDevice(machineNumber, true);
        }


        public string getIpAddress()
        {
            string ip = "";
            device.GetDeviceIP(machineNumber, ref ip);
            return ip;
        }

        public bool setIpAddress(string IpAdd)
        {
            return device.SetDeviceIP(machineNumber, IpAdd);
        }

        public int getMachineNumber()
        {
            return this.machineNumber;
        }

        public void setMachineNumber(int machineNumber)
        {
            this.machineNumber = machineNumber;
        }

        public bool setUserPassword(int userId, int password)
        {
            var user = GetUserById(userId);
            if (user == null) return false;
            return device.SSR_SetUserInfo(machineNumber, userId.ToString(), user.Name, password.ToString(), user.Privelage, user.Enabled);
        }

        public bool setUserName(int userId, string username)
        {
            var user = GetUserById(userId);
            if (user == null) return false;

            return device.SSR_SetUserInfo(machineNumber, userId.ToString(), username, user.Password, user.Privelage, user.Enabled);
        }

        public int getPort()
        {
            return this.port;
        }

        public void setPort(int Port)
        {
            this.port = Port;
        }
    }
}
