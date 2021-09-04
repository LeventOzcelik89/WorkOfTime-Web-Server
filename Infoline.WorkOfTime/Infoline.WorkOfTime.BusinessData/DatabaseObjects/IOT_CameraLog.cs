using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class IOT_CameraLog : InfolineTable
    {
        public string modelName { get; set;}
        public string version { get; set;}
        public string readtime { get; set;}
        public string macAddress { get; set;}
        public string ipAddress { get; set;}
        public string subnetMask { get; set;}
        public string dnsAddress { get; set;}
        public string videoType { get; set;}
        public string audioType { get; set;}
        public string videoStatusChannel1 { get; set;}
        public string videoStatusChannel2 { get; set;}
        public string videoStatusChannel3 { get; set;}
        public string videoStatusChannel4 { get; set;}
        public string alarmInputStatus1 { get; set;}
        public string alarmInputStatus2 { get; set;}
        public string alarmInputStatus3 { get; set;}
        public string alarmInputStatus4 { get; set;}
        public string alarmOutputStatus1 { get; set;}
        public string alarmOutputStatus2 { get; set;}
        public string alarmOutputStatus3 { get; set;}
        public string alarmOutputStatus4 { get; set;}
        public string motionDetectionStatus1 { get; set;}
        public string motionDetectionStatus2 { get; set;}
        public string motionDetectionStatus3 { get; set;}
        public string motionDetectionStatus4 { get; set;}
        public string tamperingDetectionStatus1 { get; set;}
        public string tamperingDetectionStatus2 { get; set;}
        public string tamperingDetectionStatus3 { get; set;}
        public string tamperingDetectionStatus4 { get; set;}
        public string recordingStatus1 { get; set;}
        public string recordingStatus2 { get; set;}
        public string recordingStatus3 { get; set;}
        public string recordingStatus4 { get; set;}
    }
}
