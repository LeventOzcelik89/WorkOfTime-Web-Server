using Infoline.PdksEntegrationApp.Models;
using Infoline.PdksEntegrationApp.Utils;
using Infoline.WorkOfTime.BusinessAccess;
using Infoline.WorkOfTime.BusinessData;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using zkemkeeper;

namespace Infoline.PdksEntegrationApp.Devices
{
    public abstract class PdksDevice : SH_ShiftTrackingDevice, IDeviceManipulator
    {
        protected static WorkOfTimeDatabase db { get; set; }
        public CZKEM device = new CZKEM();

        public PdksDevice()
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
                    Log.Success(this.DeviceSerialNo + " seri numaralı cihaz ile bağlantı kuruldu. (" + this.DeviceBrand + this.DeviceModel + ")");
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
                    Log.Error(this.DeviceSerialNo + " seri numaralı cihaz ile bağlantı kurulamıyor. (" + this.DeviceBrand + this.DeviceModel + ")");
                    isConn = false;
                }
                try
                {
                    while (isConn)
                    {
                        var _lastExecute = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, 0);
                        if (lastSaveLogTime.AddMinutes(saveLogTimeInterval) <= _lastExecute)
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

        public abstract bool insertLogsToDB();
        public abstract void saveUsers();
        public abstract bool AddUserToDevice(string userName, int password, int id, int privelage);
        public abstract bool Beep(int DelayMs);
        public abstract bool ClearAllLog();
        public abstract bool ClearAttendanceRecord();
        public abstract bool ClearFingerprintTemplate();
        public abstract bool ClearUsers();
        public abstract bool Connect();
        public abstract bool DeleteFingerRecordsById(int userId);
        public abstract void Disconnect();
        public abstract DateTime GetDate();
        public abstract string GetDeviceInfo();
        public abstract string getIpAddress();
        public abstract string GetLastError();
        public abstract ICollection<LogInfo> GetLogData();
        public abstract ICollection<LogInfo> GetLogDataBetweenDates(DateTime firstDate, DateTime endDate);
        public abstract ICollection<LogInfo> GetLogDataBetweenDatesAndUserId(DateTime firstDate, DateTime endDate, int userId);
        public abstract ICollection<LogInfo> GetLogDataByUserId(int userId);
        public abstract int getMachineNumber();
        public abstract int getPort();
        public abstract UserInfo GetUserById(int userId);
        public abstract int getUserFingerTemplateRecordsCount(int userId);
        public abstract List<int> GetUserIds();
        public abstract ICollection<UserInfo> GetUsers();
        public abstract bool isConnected();
        public abstract bool lockDevice();
        public abstract bool pingDevice();
        public abstract bool RemoveUserById(int userId);
        public abstract bool RestartDevice();
        public abstract bool setDate(DateTime date);
        public abstract bool setIpAddress(string IPAdd);
        public abstract void setMachineNumber(int machineNumber);
        public abstract void setPort(int Port);
        public abstract bool setUserName(int userId, string username);
        public abstract bool setUserPassword(int userId, int password);
        public abstract bool unlockDevice();
    }
}
