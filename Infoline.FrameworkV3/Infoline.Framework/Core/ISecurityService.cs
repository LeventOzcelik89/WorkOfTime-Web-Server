using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Runtime.Serialization;

namespace Infoline
{
    //[ServicePath("Security")]
    [ServiceContract]
    public interface ISecurityService : IService
    {
        [OperationContract]
        [WebGet]
        bool ChangePassword(string userid, string password, string newpassword);


        [OperationContract]
        [WebGet]
        LoginResult Login(string userid, string password, Guid? deviceId, string IPAddress);

        [OperationContract]
        [WebInvoke]
        bool IsInRole(string userid, string role);


        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "Ticket")]
        void SaveTicket(CallContext ctx);

        [OperationContract]
        [WebGet(UriTemplate = "Ticket?id={id}")]
        CallContext LoadTicket(Guid id, string path = "");

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "Ticket?id={id}")]
        void DeleteTicket(Guid id);

        int TicketLife { get; set; }
    }

    public enum LoginResult
    {
        OK,
        InvalidUser,
        InvalidPassword,
        AccountDisabled,
        RequiresPasswordChage,
        UnknownDevice,
        UserExpired
    }



}
