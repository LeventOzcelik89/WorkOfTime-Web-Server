using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Infoline.Framework.Database
{
    public static class PropertyAccessGenerator
    {
        public static object Cnv(object a, Type t)
        {
            if (a is string && t == typeof(string))
                return a;
            if (a is DBNull || a == null) return null;
            if (a is Int16 || a is Int32 || a is Int64 || a is UInt16 || a is UInt32 || a is UInt64 || a is Double)
            {
                Type nt = Nullable.GetUnderlyingType(t);
                if (nt != null) t = nt;
                return Convert.ChangeType(a, t);
            }

            Type nt1 = Nullable.GetUnderlyingType(t);
            if (nt1 != null) t = nt1;

            if (t == typeof(Guid))
                return Guid.Parse(a.ToString());


            return a;
        }
        static Cache<string, Action<object, object>> _setCache = new Cache<string, Action<object, object>>(100);

        static Cache<string, Func<object, object>> _getCache = new Cache<string, Func<object, object>>(100);

        public static Action<object, object> SetDelegate(Type type, string name)
        {
            Action<object, object> fnc;
            var key = type.FullName + name;

            if (!_setCache.TryGet(key, out fnc))
            {
                try
                {
                    PropertyInfo pi = type.GetProperty(name);
                    if (pi == null)
                        return null;
                    Expression<Func<object, Type, object>> convert = (a, t) => Cnv(a, t);

                    ParameterExpression p = Expression.Parameter(typeof(object));
                    ParameterExpression p1 = Expression.Parameter(typeof(object));
                    LabelTarget rt = Expression.Label();

                    LambdaExpression l = Expression.Lambda(typeof(Action<object, object>),
                        Expression.Block(
                           Expression.Assign(
                                Expression.Property(Expression.Convert(p, type), pi),
                                Expression.Convert(
                                    Expression.Invoke(convert, p1, Expression.Constant(pi.PropertyType)), pi.PropertyType
                                )
                           ),
                           Expression.Label(rt)
                        )
                     , p, p1);
                    fnc = (Action<object, object>)l.Compile();
                    _setCache.Add(key, fnc);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return fnc;
        }

        public static Func<object, object> GetDelegate(Type type, string name)
        {
            Func<object, object> fnc;
            var key = type.Name + name;
            if (!_getCache.TryGet(key, out fnc))
            {
                PropertyInfo pi = type.GetProperty(name);
                if (pi == null)
                    return null;
                System.Linq.Expressions.ParameterExpression p = System.Linq.Expressions.Expression.Parameter(typeof(object));

                LambdaExpression l = Expression.Lambda(typeof(Func<object, object>),
                    Expression.Convert(
                        Expression.Property(
                            Expression.Convert(p, type), pi
                        ), typeof(object)
                     ), p);
                fnc = (Func<object, object>)l.Compile();
                _getCache.Add(key, fnc);
            }
            return fnc;
        }
    }

    public class Cache<TKey, TValue>
    {

        Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
        public IEnumerable<TValue> Values { get { return dic.Values; } }
        public IEnumerable<TKey> Keys { get { return dic.Keys; } }
        public int Size { get; private set; }
        public Cache(int size)
        {
            Size = size;
        }
        public bool TryGet(TKey key, out TValue value)
        {
            return dic.TryGetValue(key, out value);
        }
        public void Remove(TKey key)
        {
            if (dic.ContainsKey(key))
                dic.Remove(key);
        }
        public void Add(TKey key, TValue value)
        {
            try
            {


                if (!dic.ContainsKey(key))
                {
                    dic.Add(key, value);

                    int size = dic.Count - Size;
                    dic.Keys.Take(size).ToList().ForEach(a => dic.Remove(a));
                }
                else
                    dic[key] = value;
            }
            catch (Exception)
            {


            }
        }
        public TValue Get(TKey key, Func<TValue> create)
        {
            TValue val;
            if (!dic.TryGetValue(key, out val))
            {
                val = create();
                Add(key, val);
            }
            return val;
        }


    }
}
