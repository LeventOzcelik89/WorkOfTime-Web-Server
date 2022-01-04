using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWPA_TransactionConfirmation : InfolineTable
    {
        public Guid? transactionId { get; set;}
        public Guid? userId { get; set;}
        public short? status { get; set;}
        public string description { get; set;}
        public short? ruleOrder { get; set;}
        public short? ruleType { get; set;}
        public Guid? ruleUserId { get; set;}
        public Guid? ruleRoleId { get; set;}
        public string ruleTitle { get; set;}
        public string createdby_Title { get; set;}
        public string changedby_Title { get; set;}
        public string Person_Title { get; set;}
        public string ruleUserId_Title { get; set;}
        public string confirmationUserPhoto { get; set;}
        public string ruleType_Title { get; set;}
        public string confirmationUserIds { get; set;}
        public string confirmationUserIds_Titles { get; set;}
    }
}
