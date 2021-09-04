using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Linq;
using System.Linq.Expressions;
using System.Reflection;

using System.Data.SqlClient;
using Infoline.Helper;
namespace Infoline.Data
{

    public class DBContext : QueryProvider, IDisposable, IObjectTracker
    {
        IDBProvider dbprovider = new SQL2008Provider();
        IDbConnection connection;
        public bool UseEntityProxy { get; set; }
        //Dictionary<Type,string> _queryCa
        public DBContext(string connectionstring)
        {
            UseEntityProxy = false;
            this.connection = dbprovider.CreateConnection(connectionstring);
        }

        #region Execute
        public IEnumerable<TClass> Execute<TClass>(string query, params object[] parameters)
        {
            return ExecuteReader(dbprovider.CreateCommand(query, parameters), typeof(TClass)).Cast<TClass>();
        }
        IDbCommand PreExecute(IDbCommand cmd)
        {
            cmd.Connection = connection;
            return new CmdWrapper(cmd);
        }
        public TReturn ExecuteScalar<TReturn>(string query, params object[] parameters)
        {
            var cmd = PreExecute(dbprovider.CreateCommand(query, parameters));
            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            try
            {
                var ret = cmd.ExecuteScalar();
                return (TReturn)(ret == DBNull.Value ? null : ret);
            }
            finally
            {
                if (cmd.Transaction == null)
                    cmd.Connection.Close();
            }
        }
        public int ExecuteNonQuery(string query, params object[] parameters)
        {
            var cmd = PreExecute(dbprovider.CreateCommand(query, parameters));

            if (cmd.Connection.State == ConnectionState.Closed)
                cmd.Connection.Open();
            try
            {
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if (cmd.Transaction == null)
                    cmd.Connection.Close();
            }
        }

        public bool Delete<TClass>(Guid id) where TClass : Entity
        {
            return ExecuteNonQuery(string.Format("delete from {0} where id = {{0}}", typeof(TClass).Name), id) > 0;
            // return Delete<TClass>(a => a.id == id);
        }


        public bool Delete<TClass>(Expression<Func<TClass, bool>> predicate)
        {
            return PreExecute(dbprovider.DeleteCommand(predicate)).ExecuteNonQuery() > 0;

        }

        #region Linq
        protected override S Execute<S>(Expression expression)
        {
            return ExecuteReader(dbprovider.CreateCommand(expression), typeof(S))
                .Cast<S>().FirstOrDefault();

        }
        protected override object Execute(System.Linq.Expressions.Expression expression)
        {
            return ExecuteReader(dbprovider.CreateCommand(expression), expression.Type.GetGenericArguments()[0]);
        }
        #endregion
        IEnumerable ExecuteReader(IDbCommand cmd, Type type)
        {
            cmd = PreExecute(cmd);
            cmd.Connection = connection;
            if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                throw new Exception("Connection already open!!");
            cmd.Connection.Open();
            var reader = cmd.ExecuteReader();

            try
            {
                using (reader)
                {
                    Action<object, object>[] setters = new Action<object, object>[reader.FieldCount];

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        setters[i] = PropertyAccessGenerator.SetDelegate(type, reader.GetName(i));

                    }

                    while (reader.Read())
                    {
                        var ret = UseEntityProxy ? ProperyChangeProxy.Create(type) : Activator.CreateInstance(type);
                        var e = ret as Entity;
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (setters[i] != null)
                                setters[i](ret, reader.GetValue(i));

                        }
                        if (e != null)
                        {
                            e.State = EntityState.UnModified;
                            e.Context = new WeakReference(this);
                        }
                        yield return ret;
                    }
                }
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
        #endregion

        public TClass New<TClass>() where TClass : Entity, new()
        {
            var ret = ProperyChangeProxy.Create<TClass>();
            _trackedItems.Add(ret);
            ret.id = Guid.NewGuid();
            return ret;
        }

        static Cache<Type, Func<IDataReader, object>> _readerCache = new Cache<Type, Func<System.Data.IDataReader, object>>(50);

        #region Query
        Dictionary<Type, object> _tables = new Dictionary<Type, object>();

        public Query<TClass> Query<TClass>() where TClass : Entity, new()
        {
            object ret;
            if (!_tables.TryGetValue(typeof(TClass), out ret))
            {
                _tables.Add(typeof(TClass), ret = new Query<TClass>(this));
            }
            return (Query<TClass>)ret;
        }
        public TClass Get<TClass>(Expression<Func<TClass, bool>> predicate) where TClass : Entity, new()
        {
            return Query<TClass>().FirstOrDefault(predicate);
        }
        public TClass Get<TClass>(Guid id) where TClass : Entity, new()
        {
            return Execute<TClass>(string.Format("select * from {0} where id = {{0}}", typeof(TClass).Name), id).FirstOrDefault(); ;
        }
        #endregion

        #region Commit
        List<Entity> _trackedItems = new List<Entity>();
        void IObjectTracker.TrackObject(Entity obj)
        {

            //Remove entity
            if (obj.Context != null && obj.Context.IsAlive && obj.Context.Target == this)
            {
                var before = _trackedItems.FirstOrDefault(a => a.id == obj.id);
                if (before != null)
                    _trackedItems.Remove(before);
                obj.Context = null;
                _trackedItems.Add(obj);
            }
        }

        static Cache<Type, IUpdateCommands> _adapterCache = new Cache<Type, IUpdateCommands>(50);

        public void Update(IEnumerable<Entity> entities)
        {
            if (connection.State == ConnectionState.Closed)
                connection.Open();
            using (var trx = new System.Transactions.TransactionScope())
            {


                //    var trx = connection.BeginTransaction();

                try
                {
                    foreach (var item in entities)
                    {
                        IDbCommand cmd = null;
                        var type = item.GetType();

                        if (type.Assembly.IsDynamic)
                            type = type.BaseType;

                        IUpdateCommands adapter;
                        if (!_adapterCache.TryGet(type, out adapter))
                            _adapterCache.Add(type, adapter = dbprovider.CreateUpdateCommands(type, connection));

                        switch (item.State)
                        {
                            case EntityState.New:
                                cmd = adapter.InsertCommand;
                                break;
                            case EntityState.UnModified:
                                break;
                            case EntityState.Modified:

                                cmd = adapter.UpdateCommand(item.changedProperties);
                                break;
                            case EntityState.Deleted:
                                cmd = adapter.DeleteCommand;
                                break;
                            default:
                                break;
                        }
                        if (cmd == null)
                            continue;

                        foreach (var p in cmd.Parameters.Cast<IDbDataParameter>())
                        {
                            p.Value = PropertyAccessGenerator.GetDelegate(item.GetType(), p.SourceColumn)
                                (p.SourceVersion == DataRowVersion.Original ? item.Orginal : item);
                            if (p.Value == null && p.IsNullable)
                                p.Value = DBNull.Value;
                        }
                        cmd.Connection = connection;

                        cmd = PreExecute(cmd);
                        //  cmd.Transaction = trx;
                        if (cmd.ExecuteNonQuery() < 1)
                        {
                            throw new DBConcurrencyException();
                        }


                        //TODO : Update local version
                    }
                    //   trx.Commit();
                    trx.Complete();
                    foreach (var item in entities)
                    {
                        item.State = EntityState.UnModified;
                        item.Context = new WeakReference(this);
                    }

                }
                finally
                {

                    connection.Close();
                    //  trx.Dispose();
                }
            }

        }
        public void Upsert<TClass>(TClass value) where TClass : Entity, new()
        {
            var o = this.Get<TClass>(value.id) ?? New<TClass>();
            BaseEditableObject<TClass>.Copy(value, o);
            SubmitChanges();

        }

        public void SubmitChanges()
        {
            Update(_trackedItems);

            _trackedItems.Clear();
        }
        #endregion

        //protected override string GetQueryText(System.Linq.Expressions.Expression expression)
        //{
        //    return dbprovider.GetQueryText(expression);
        //}
        bool disposed = false;
        public void Dispose()
        {
            if (!disposed)
                connection.Dispose();
            disposed = false;
        }
        
    }

    public interface IDBProvider
    {
        IDbCommand CreateCommand(Expression expression);
        IDbCommand DeleteCommand(Expression expression);

        IUpdateCommands CreateUpdateCommands(Type type, IDbConnection connection);

        IDbCommand CreateCommand(string query, params object[] parameters);

        string GetQueryText(System.Linq.Expressions.Expression expression);

        IDbConnection CreateConnection(string connection);

        string Name { get; }
    }

    public interface IUpdateCommands
    {
        IDbCommand UpdateCommand(IEnumerable<string> changedprops);
        IDbCommand DeleteCommand { get; }
        IDbCommand InsertCommand { get; }
    }

    class CmdWrapper : IDbCommand
    {
        IDbCommand sink;
        public CmdWrapper(IDbCommand sink)
        {
            this.sink = sink;
        }


        public string CommandText { get { return sink.CommandText; } set { sink.CommandText = value; } }


        public int CommandTimeout { get { return sink.CommandTimeout; } set { sink.CommandTimeout = value; } }
        public CommandType CommandType { get { return sink.CommandType; } set { sink.CommandType = value; } }

        public IDbConnection Connection { get { return sink.Connection; } set { sink.Connection = value; } }
        public IDbTransaction Transaction { get { return sink.Transaction; } set { sink.Transaction = value; } }
        public UpdateRowSource UpdatedRowSource { get { return sink.UpdatedRowSource; } set { sink.UpdatedRowSource = value; } }

        public void Cancel()
        {
            sink.Cancel();
        }
        public IDbDataParameter CreateParameter()
        {
            return sink.CreateParameter();
        }
        void Log(string action)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("Action:{0}\r\n", action);
            sb.AppendLine(sink.CommandText);
            foreach (IDbDataParameter p in sink.Parameters)
            {
                sb.AppendFormat("{0}={1}", p.ParameterName, p.Value);
            }
            ApplicationLog.LogMessage(LogLevel.Info, sb.ToString());
        }
        public int ExecuteNonQuery()
        {
            Log("ExecuteNonQuery");
            return sink.ExecuteNonQuery();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            Log("ExecuteReader");
            return sink.ExecuteReader(behavior);
        }

        public IDataReader ExecuteReader()
        {
            Log("ExecuteReader");
            return sink.ExecuteReader();
        }

        public object ExecuteScalar()
        {
            Log("ExecuteScalar ");
            return sink.ExecuteScalar();
        }

        public IDataParameterCollection Parameters { get { return sink.Parameters; } }

        public void Prepare()
        {
            sink.Prepare();
        }


        public void Dispose()
        {
            sink.Dispose();
        }
    }
}
