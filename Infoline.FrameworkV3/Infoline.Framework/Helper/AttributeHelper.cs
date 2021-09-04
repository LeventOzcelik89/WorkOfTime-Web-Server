using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Infoline.Helper
{
    public static class AttributeHelper
    {
        public static void PropertySet(object p, string propName, object value)
        {
            Type t = p.GetType();
            PropertyInfo info = t.GetProperty(propName);
            if (info == null)
                return;
            if (!info.CanWrite)
                return;
            info.SetValue(p, value);
        }

        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static T[] GetAttributesOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? attributes.Where(a => a is T).Select(a => (T)a).ToArray() : null;
        }



        public static T GetAttributeOfType<T>(this PropertyInfo propInfo) where T : System.Attribute
        {
            var attributes = propInfo.GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }

        public static T[] GetAttributesOfType<T>(this PropertyInfo propInfo) where T : System.Attribute
        {
            var attributes = propInfo.GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? attributes.Where(a => a is T).Select(a => (T)a).ToArray() : null;
        }

    }
}
