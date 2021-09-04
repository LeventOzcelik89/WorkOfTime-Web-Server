using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Infoline.Helper;
using System.Reflection;

namespace Infoline
{
    public class Application : ICallContextProvider, ILogListener
    {
        static public event Action<Application> ApplicationStarted;
        //static public event Action<Exception> Error;

        static Application _current;

        public static Application Current
        {
            get
            {
                if (_current == null)
                {

                    _current = new Application();
                    _current.ContextProvider = _current as ICallContextProvider;
                    ApplicationLog.LogInfo("Application starting!!");
                    _current.Init();
                    ApplicationLog.LogInfo("Application strated!!");

                }
                return _current;
            }
        }

        void Init()
        {
            try
            {
                var cat = new AggregateCatalog();
                string root = ".";

                if (AppDomain.CurrentDomain.SetupInformation.ConfigurationFile.EndsWith("web.config"))//Asp.net
                {
                    root = "bin";
                }
                else
                    cat.Catalogs.Add(new AssemblyCatalog(Assembly.GetEntryAssembly()));

                var d = new DirectoryCatalog(root);
                d.Refresh();
                cat.Catalogs.Add(d);

                var s = System.Diagnostics.Stopwatch.StartNew();
                s.Start();
                
                new CompositionContainer(cat).ComposeParts(this);
                
                s.Stop();
                System.Diagnostics.Debug.WriteLine("Compose:{0}", s.ElapsedMilliseconds);
                s.Start();
                foreach (var item in services)
                {
                    _services[item.Metadata.ServiceType] = item.Value;
                }

                _modules.Do(a => a.Init());
                s.Stop();


                System.Diagnostics.Debug.WriteLine("Before start:{0}", s.ElapsedMilliseconds);
                if (ApplicationStarted != null)
                    ApplicationStarted(this);
            }
            catch (ReflectionTypeLoadException ex2)
            {
                throw new Exception(string.Join("\r\n", ex2.LoaderExceptions.Select(a => a.Message)));
            }
            catch (Exception ex2)
            {
                throw ex2.InnerException;
            }
        }

        Application()
        {

        }

        public ICallContextProvider ContextProvider { get; set; }

        ISecurityService _securityService;
        public ISecurityService SecurityService
        {
            get
            {

                if (_securityService == null)
                {
                    _securityService = GetService<ISecurityService>();
                    if (_securityService == null)
                        throw new Exception("ISecurityService not found!!");
                }
                return _securityService;
            }
        }


        public IEnumerable<Type> Services
        {
            get
            {
                return _services.Keys;
            }
        }
        [ImportMany(typeof(IService))]
        IEnumerable<Lazy<IService, IServiceMeta>> services = null;

        [ImportMany(typeof(ILogListener))]
        IEnumerable<ILogListener> errorListeners = null;


        [ImportMany(typeof(IApplicationModule))]
        IEnumerable<IApplicationModule> _modules = null;
        public IEnumerable<IApplicationModule> Modules { get { return _modules; } }


        Dictionary<Type, object> _services = new Dictionary<Type, object>();
        public object GetService(Type servicetype)
        {
            return _services[servicetype];
        }


        public T GetService<T>() where T : class,IService
        {
            object s;
            if (_services.TryGetValue(typeof(T), out s))
                return s as T;
            throw new Exception(string.Format("Service {0} not found!!", typeof(T)));

        }

        public void AddService<T>(object service)
        {
            AddService(typeof(T), service);
        }
        public void AddService(Type type, object service)
        {
            //System.Diagnostics.Debug.Assert(service != null && service.GetType().IsClass);
            _services[type] = service;
        }
        public void RemoveService(Type type)
        {
            _services.Remove(type);
        }

        Dictionary<string, object> dic = new Dictionary<string, object>();
        public object this[string key]
        {
            get { return dic.ContainsKey(key) ? dic[key] : null; }
            set { dic[key] = value; }
        }

        #region ICallContextProvider
        [ThreadStatic]
        CallContext _context;
        CallContext ICallContextProvider.Context { get { return _context; } set { _context = value; } }
        bool ICallContextProvider.IsReady { get { return _context != null; } }
        #endregion
        #region ILogListener
        bool ILogListener.Enabled
        {
            get { return true; }
        }

        void ILogListener.LogMessage(LogLevel level, string message)
        {
            if (errorListeners != null)
            {
                errorListeners.Where(a => a.Enabled)
                       .Do(a => a.LogMessage(level, message));
            }
        }

        void ILogListener.LogError(Exception ex)
        {
            if (errorListeners != null)
            {
                errorListeners.Where(a => a.Enabled)
                       .Do(a => a.LogError(ex));
            }
        }
        #endregion


    }
    public interface IService
    {

    }
    public interface IServiceHost : IService
    {
        void RegisterService(Type servicetype, object service, string relativeurl);
        void UnregisterService(string relariveurl);
        string BaseUrl { get; }
    }
    public interface IRemoteServiceRegisterer : IService
    {
        void RegisterService<TClass>();
        string BaseUri { get; set; }
    }
    public interface IApplicationModule
    {
        void Init();
    }

    public interface IServiceMeta
    {
        Type ServiceType { get; }
    }
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class RegisterServiceAttribute : Attribute
    {
        public RegisterServiceAttribute()
        {

        }
    }
    [AttributeUsage(AttributeTargets.Interface, Inherited = false, AllowMultiple = false)]
    public sealed class ServicePathAttribute : Attribute
    {
        public string Path { get; private set; }
        public ServicePathAttribute(string path)
        {
            Path = path;
        }
    }
    public class DependencyImporter<TClass>
    {
        [ImportMany]
        IEnumerable<TClass> settings = null;
        public IEnumerable<TClass> Imports
        {
            get
            {
                return settings;
            }
        }
        public DependencyImporter()
        {
            var cat = new AssemblyCatalog(System.Reflection.Assembly.GetCallingAssembly());
            new CompositionContainer(cat).ComposeParts(this);
        }

        public DependencyImporter(string directory)
        {
            var cat = new DirectoryCatalog(directory);
            cat.Refresh();
            new CompositionContainer(cat).ComposeParts(this);
        }
    }
}
