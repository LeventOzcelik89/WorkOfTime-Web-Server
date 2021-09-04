using System;
using System.Linq;
using System.Threading;
using System.Collections;
using System.Threading.Tasks;

using System.Runtime.Serialization;
using System.Security.Principal;
using Infoline.Helper;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
namespace Infoline
{
    [Serializable]
    [KnownType(typeof(string[]))]
    [KnownType(typeof(Guid))]
    [KnownType(typeof(PropertyBag))]
    public class CallContext : ILogicalThreadAffinative, ISerializable
    {

        #region Private Fields
        Guid? _deviceId;
        Guid _ticketid;
        Guid _userid;
        DateTime _createTime, _lifeTime;

        PropertyBag _properties;


        string _username = string.Empty;
        string _userFullname;
        int _userLevel = 5;
        string[] _roles;
        PropertyBag _userData;

        #endregion

        #region Properties

        public Guid TicketId { get { return _ticketid; } }

        public Guid UserId { get { return _userid; } }

        public Guid? DeviceId { get { return _deviceId; } }

        public string UserName { get { return _username; } }

        public string UserFullName { get { return _userFullname; } set { _userFullname = value; } }

        public string[] Roles { get { return _roles != null ? _roles : new string[0]; } }

        public int UserLevel { get { return _userLevel; } }

        public DateTime CreateTime { get { return _createTime; } }
        public DateTime LifeTime { get { return _lifeTime; } set { _lifeTime = value; } }

        public TimeSpan Duration { get { return ((TimeSpan)(DateTime.Now - _createTime)); } }

        public bool IsAuthenticated
        {
            get
            {
                return true;
                //return Application != null && Application.SecurityService.IsValidTicket(_ticket);
            }
        }
        public bool IsAdmin { get { return IsInRole(AdminRole); } }

        public PropertyBag UserData { get { return _userData; } }

        public PropertyBag Properties { get { return _properties; } }


        #endregion

        #region Constructors

        public CallContext(Guid ticketid, Guid userid, Guid? deviceId, PropertyBag properties,
            string username, string fullname, string[] roles, PropertyBag uservalues, DateTime lifetime, int userlevel = 5)
        {
            _ticketid = ticketid;
            _deviceId = deviceId;
            _userLevel = userlevel;
            _userid = userid;
            _roles = roles;
            _properties = properties ?? new PropertyBag();
            _username = username;
            _userFullname = string.IsNullOrEmpty(fullname) ? username : fullname;
            _createTime = DateTime.Now;
            _lifeTime = lifetime;
            _properties = properties ?? new PropertyBag();
            _userData = uservalues ?? new PropertyBag();
        }
        #endregion

        #region Operations
        /// <summary>
        /// Activates current Call Context
        /// </summary>
        public void Activate()
        {
            if (IsReady)
            {
                if (Current.TicketId != this.TicketId)
                    Current.Deactivate();
                else
                    return; //Already activated
            }
            Current = this;
            System.Threading.Thread.CurrentPrincipal = new MiraPrincipal(this);
        }


        public void Save()
        {
            Application.Current.SecurityService.SaveTicket(this);
        }
        public void Deactivate()
        {
            //  System.Threading.Thread.CurrentPrincipal = null;
            //System.Threading.Tasks.Task.Factory.StartNew(Save);
            Save();
            Current = null;
            if (Deactivated != null)
                Deactivated(this);
        }
        public void Logout()
        {
            Application.Current.SecurityService.DeleteTicket(_ticketid);
            Current = null;
        }

        #endregion

        #region RoleCheck
        public void CheckRole(string role)
        {
            CheckRole(role, role.ToString());
        }
        public void CheckRole(string role, string format, params string[] args)
        {
            if (!IsInRole(role))
                throw new System.Security.SecurityException(string.Format(format, args));
        }
        //public bool IsInRole(int role)
        //{
        //    return RoleManager.GetRoles(role).Any(this.IsInRole);
        //}
        public const string AdminRole = "Admin";
        public bool IsInRole(string role)
        {
            if (role == AdminRole)
                return _roles != null ? _roles.Any(a => a == AdminRole) : true;
            else
                return _roles != null ? _roles.Any(a => a == AdminRole || a == role) : true;

        }
        #endregion

        #region STATIC

        public static event Action<CallContext> Deactivated;

        public static bool IsReady
        {
            get
            {
                return Application.Current.ContextProvider.IsReady;
            }
        }


        //static CallContext _debug = new CallContext("debug", "", "", Guid.Empty, false, new Role[] { Role.AdminRole }, Guid.Empty, new PropertyBag());
        public static CallContext Current
        {
            get
            {
                return Application.Current.ContextProvider.Context;
            }
            private set
            {
                Application.Current.ContextProvider.Context = value;
            }
        }
        #endregion

        #region Task
        static public Task CreateTaskWithContext(Action task)
        {
            return CreateTaskWithContext(task, System.Threading.CancellationToken.None);
        }

        static public Task CreateTaskWithContext(Action task, CancellationToken ctoken)
        {
            var cc = CallContext.IsReady ? CallContext.Current : null;

            return new Task(delegate
            {

                if (cc != null)
                    cc.Activate();
                try
                {
                    task();
                }
                finally
                {
                    if (cc != null)
                        CallContext.Current = null;
                }

            }, ctoken, TaskCreationOptions.LongRunning)/*.ReportError()*/;
        }



        #endregion

        CallContext(SerializationInfo info, StreamingContext context)
        {
            _ticketid = new Guid(info.GetString("ticketid"));
            _userid = new Guid(info.GetString("userid"));
            _createTime = info.GetDateTime("createtime");
            _lifeTime = info.GetDateTime("lifetime");
            _deviceId = new Guid(info.GetString("deviceId"));
            _roles = (string[])info.GetValue("roles", typeof(string[]));
            _properties = (PropertyBag)info.GetValue("props", typeof(PropertyBag));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ticketid", _ticketid, typeof(Guid));
            info.AddValue("userid", _userid, typeof(Guid));
            info.AddValue("deviceId", _deviceId, typeof(Guid));
            info.AddValue("createtime", _createTime);
            info.AddValue("lifetime", _lifeTime);
            info.AddValue("roles", new string[] { "test" }, typeof(string[]));
            info.AddValue("props", _properties);
        }

    }
    public interface ICallContextProvider
    {
        CallContext Context { get; set; }
        bool IsReady { get; }
    }

    [Serializable]
    public class PropertyBag : ISerializable
    {
        public bool HasChanged { get; set; }
        Hashtable Properties;


        public PropertyBag(Hashtable ht)
        {
            Properties = ht;

        }

        //public T Get<T>(string name)
        //{
        //    object t = Properties[name];
        //    if (t == null || !(t is T))
        //    {
        //        Set(name, t);
        //    }
        //    return (T)t;
        //}

        public T Get<T>(string name, T def)
        {
            object t = Properties[name];
            if (t == null || !(t is T))
            {
                t = def;
                Set(name, t);
            }
            return (T)t;
        }
        public void Set(string name, object value)
        {
            if (Properties[name] != value)
            {
                Properties[name] = value;
                HasChanged = true;
            }
        }

        #region ISerializable Members
        public PropertyBag()
        {
            Properties = new Hashtable();
        }
        PropertyBag(SerializationInfo info, StreamingContext context)
        {
            Properties = new Hashtable();
            foreach (var item in info)
            {
                Properties[item.Name] = item.Value;
            }
            //Properties = info.GetValue(em"hash", typeof(Hashtable)) as Hashtable;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (string item in Properties.Keys)
            {
                var v = Properties[item];
                if (v != null)
                    info.AddValue(item, v, v.GetType());
            }
        }

        #endregion
    }

    #region Principal & Identity
    class MiraPrincipal : IPrincipal, ISerializable
    {
        CallContext ctx;
        public MiraPrincipal(CallContext ctx)
        {
            this.ctx = ctx;
            _identity = new MiraIdentity(ctx);
        }
        public MiraPrincipal(SerializationInfo info, StreamingContext context)
        {

            var t = GetType();


            var serializableMembers = FormatterServices.GetSerializableMembers(t);

            FormatterServices.PopulateObjectMembers(this, serializableMembers,
                serializableMembers.Select(a =>
                    {
                        return info.GetValue(a.Name, a is FieldInfo ? (a as FieldInfo).FieldType : (a as PropertyInfo).PropertyType);
                    }).ToArray());
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            object generic = this;
            if (context.State == StreamingContextStates.CrossAppDomain)
            {
                generic = new GenericPrincipal(new MiraIdentity(ctx), ctx.Roles);

            }
            info.SetType(generic.GetType());

            var serializableMembers = FormatterServices.GetSerializableMembers(generic.GetType());

            var serializableValues = FormatterServices.GetObjectData(generic, serializableMembers);

            for (int i = 0; i < serializableMembers.Length; i++)

                info.AddValue(serializableMembers[i].Name, serializableValues[i]);


        }

        private IIdentity _identity;
        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            return ctx.IsInRole(role);
        }
    }
    class MiraIdentity : IIdentity, ISerializable
    {

        CallContext ctx;
        public MiraIdentity(CallContext ctx)
        {
            this.ctx = ctx;
        }
        public MiraIdentity(SerializationInfo info, StreamingContext context)
        {

            var t = GetType();


            var serializableMembers = FormatterServices.GetSerializableMembers(t);

            FormatterServices.PopulateObjectMembers(this, serializableMembers,
                serializableMembers.Select(a =>
                {
                    return info.GetValue(a.Name, a is FieldInfo ? (a as FieldInfo).FieldType : (a as PropertyInfo).PropertyType);
                }).ToArray());
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {

            // State will be CrossAppDomain for serialization induced by WebDev.WebServer
            object generic = this;
            if (context.State == StreamingContextStates.CrossAppDomain)
            {
                generic = new GenericIdentity(Name, AuthenticationType);

            }
            info.SetType(generic.GetType());

            var serializableMembers = FormatterServices.GetSerializableMembers(generic.GetType());

            var serializableValues = FormatterServices.GetObjectData(generic, serializableMembers);

            for (int i = 0; i < serializableMembers.Length; i++)

                info.AddValue(serializableMembers[i].Name, serializableValues[i]);


        }

        public string AuthenticationType
        {
            get { return "Mira"; }
        }

        public bool IsAuthenticated
        {
            get { return ctx.IsAuthenticated; }
        }

        public string Name
        {
            get { return ctx.UserName; }
        }
    }
    #endregion
}
