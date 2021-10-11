using Infoline.PdksEntegrationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.PdksEntegrationApp.Utils
{
    public interface IDeviceManipulator
    {
        bool Connect();
        void Disconnect();
        bool isConnected();
        bool pingDevice();
        string getIpAddress();
        bool setIpAddress(string IPAdd);
        int getPort();
        void setPort(int Port);
        bool lockDevice();
        bool unlockDevice();
        int getMachineNumber();
        void setMachineNumber(int machineNumber);
        string GetDeviceInfo();
        bool RestartDevice();
        bool Beep(int DelayMs);


        DateTime GetDate();
        bool setDate(DateTime date);
        string GetLastError();


        ICollection<LogInfo> GetLogData();
        ICollection<LogInfo> GetLogDataByUserId(int userId);
        ICollection<LogInfo> GetLogDataBetweenDates(DateTime firstDate, DateTime endDate);
        ICollection<LogInfo> GetLogDataBetweenDatesAndUserId(DateTime firstDate, DateTime endDate, int userId);
        bool ClearAllLog();
        bool ClearAttendanceRecord();


        ICollection<UserInfo> GetUsers();
        UserInfo GetUserById(int userId);
        List<int> GetUserIds();
        bool DeleteFingerRecordsById(int userId);
        int getUserFingerTemplateRecordsCount(int userId);
        bool AddUserToDevice(string userName, int password, int id, int privelage);
        bool ClearUsers();
        bool ClearFingerprintTemplate();
        bool RemoveUserById(int userId);
        bool setUserPassword(int userId, int password);
        bool setUserName(int userId, string username);

    }
}
