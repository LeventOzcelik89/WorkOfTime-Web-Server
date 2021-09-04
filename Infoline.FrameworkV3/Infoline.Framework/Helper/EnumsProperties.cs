using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using Infoline.Framework.Database;
using System.Runtime.Serialization;

namespace Infoline.Helper
{


    public struct EnumsProperties
    {
        public static T GetAttribute<T>(Enum enumValue) where T : Attribute
        {
            T attribute;

            MemberInfo memberInfo = enumValue.GetType().GetMember(enumValue.ToString())
                                            .FirstOrDefault();

            if (memberInfo != null)
            {
                attribute = (T)memberInfo.GetCustomAttributes(typeof(T), false).FirstOrDefault();
                return attribute;
            }
            return null;
        }

        public static string GetDescriptionFromEnumValue(Enum value)
        {
            DescriptionAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? value.ToString() : attribute.Description;
        }

        public static Dictionary<string, string> GetGenericFromEnumValue(Enum value)
        {
            var res = new Dictionary<string, string>();
            GenericAttribute generic = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(GenericAttribute), false)
                .SingleOrDefault() as GenericAttribute;
            if (generic != null)
            {
                foreach (var item in generic.Data)
                {
                    res.Add(item.Key, item.Value);
                }
            }


            var keyValue = value.GetType()
                .GetField(value.ToString())
               .GetCustomAttributes<KeyValueAttribute>()
               .ToArray();

            foreach (var item in keyValue)
            {
                res.Add(item.Key, item.Value);
            }



            return res;
        }

        public static T GetEnumValueFromDescription<T>(string description)
        {
            var type = typeof(T);
            if (!type.IsEnum)
                throw new ArgumentException();
            FieldInfo[] fields = type.GetFields();
            var field = fields
                            .SelectMany(f => f.GetCustomAttributes(
                                typeof(DescriptionAttribute), false), (
                                    f, a) => new { Field = f, Att = a })
                            .Where(a => ((DescriptionAttribute)a.Att)
                                .Description == description).SingleOrDefault();
            return field == null ? default(T) : (T)field.Field.GetRawConstantValue();
        }

        public static IEnumerable<Item> EnumToArrayAqiRangeInformation<T>()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var deger = (int)item;

                yield return new Item { Key = deger.ToString(), Value = GetDescriptionFromEnumValue((Enum)item), EnumKey = item.ToString(), Information = ((Enum)item).GetAttributeOfType<Information>() };
            };
        }

        public static IEnumerable<Item> EnumToArray<T>()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {

                var deger = item.ToString().Replace("_", "");
                yield return new Item { Key = deger.ToString(), Value = GetDescriptionFromEnumValue((Enum)item) };

            }
        }

        public static IEnumerable<Item> EnumToArrayGeneric<T>()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var deger = (int)item;
                yield return new Item
                {
                    Key = deger.ToString(),
                    Value = GetDescriptionFromEnumValue((Enum)item),
                    EnumKey = item.ToString(),
                    Generic = GetGenericFromEnumValue((Enum)item)
                };

            }
        }

        public static IEnumerable<Item> EnumToArrayValues<T>() where T : struct, IConvertible
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var deger = (int)item;
                yield return new Item { Key = deger.ToString(), Value = GetDescriptionFromEnumValue((Enum)item), EnumKey = item.ToString() };
            }
        }

        public static IEnumerable<AttriubteItem> EnumToArrayGenericValues<T>()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var deger = (int)item;
                yield return new AttriubteItem { Key = deger.ToString(), Value = GetStringGenericValue((Enum)item) };
            }
        }

        public static IEnumerable<AttriubteItem> EnumToArrayBirimValues<T>()
        {
            var dictionary = new Dictionary<string, string>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {

                var deger = (int)item;

                yield return new AttriubteItem { Key = deger.ToString(), BirimValue = GetStringBirimValue((Enum)item) };


            }
        }

        public static BreadCrump_AuthorityAction GetStringGenericValue(Enum value)
        {
            BreadCrump_AuthorityAction attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(BreadCrump_AuthorityAction), false)
                .SingleOrDefault() as BreadCrump_AuthorityAction;
            return attribute;
        }

        public static Birim GetStringBirimValue(Enum value)
        {
            Birim attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(Birim), false)
                .SingleOrDefault() as Birim;
            return attribute;
        }

        public static string EnumKeyToDescription<T>(dynamic key) where T : struct, IConvertible
        {
            if (key == null)
                return string.Empty;

            var result = EnumsProperties.EnumToArrayValues<T>().Where(p => p.Key.Equals(key.ToString())).FirstOrDefault();
            return result != null ? result.Value : string.Empty;
        }

        private static string EnumKeyToTitValue<T>(dynamic key)
        {
            var result = EnumsProperties.EnumToArrayGenericValues<T>().Where(p => p.Key.Equals(key.ToString())).FirstOrDefault();
            return result != null ? result.Value.AuthorityActionText : string.Empty;
        }
        public static string EnumKeyToBirimValue<T>(dynamic key)
        {
            var result = EnumsProperties.EnumToArrayBirimValues<T>().Where(p => p.Key.Equals(key.ToString())).FirstOrDefault();
            return result != null ? result.BirimValue.birim : string.Empty;
        }

    }


}



public class Item
{
    public string Key { get; set; }
    public string Value { get; set; }
    public string EnumKey { get; set; }
    public Dictionary<string, string> Generic { get; set; }
    public Information Information { get; set; }
}

public class AttriubteItem
{
    public string Key { get; set; }
    public BreadCrump_AuthorityAction Value { get; set; }
    public Birim BirimValue { get; set; }

}

public class Birim : Attribute
{
    public Birim(string nameParam)
    {
        _birim = nameParam;
    }
    public string birim { get { return _birim; } }
    private string _birim;

}

[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public sealed class Information : Attribute
{
    public Information(int min, int max, string color, string colorDescription)
    {
        this.Color = color;
        this.Description = colorDescription;
        this.Min = min;
        this.Max = max;
    }

    public Information(string color)
    {
        this.Color = color;

    }
    public string Color { get; }
    public string Description { get; }
    public int Min { get; }
    public int Max { get; }
}


public class GenericAttribute : Attribute
{
    public Dictionary<string, string> Data { get; set; } = new Dictionary<string, string>();
    /// <summary>
    /// Değerler ilk parametresi key değeri ikinci parametresi value değeri almaktadır.
    /// </summary>
    /// <param name="values"></param>
    public GenericAttribute(params string[] values)
    {
        for (int i = 0; i < values.Length - 1; i++)
        {
            if (i % 2 == 0)
            {
                Data[values[i]] = values[i + 1];
            }
        }
    }
}


[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public class KeyValueAttribute : Attribute
{
    public string Key { get; set; }

    public string Value { get; set; }
    public KeyValueAttribute(string key, string value)
    {
        Key = key;
        Value = value;
    }
}


public class BreadCrump_AuthorityAction : Attribute
{
    private string _authorityActionText;
    private string _breadCrumpText;
    private int _parentActionCode;
    public bool _showOnAuthorityActions { get; set; }

    public BreadCrump_AuthorityAction(
        string breadCrumpText,
        string authorityActionText,
        int parentActionCode = 0,
        bool showOnAuthorityActions = false)
    {
        _authorityActionText = authorityActionText;
        _breadCrumpText = breadCrumpText;
        _parentActionCode = parentActionCode;
        _showOnAuthorityActions = showOnAuthorityActions;
    }

    public string BreadCrumpText { get { return _breadCrumpText; } }
    public string AuthorityActionText { get { return _authorityActionText; } }
    public int ParentActionCode { get { return _parentActionCode; } }
    public bool ShowOnAuthorityActions { get { return _showOnAuthorityActions; } }

}