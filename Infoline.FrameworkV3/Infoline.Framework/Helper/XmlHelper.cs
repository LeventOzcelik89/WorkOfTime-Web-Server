using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;


namespace Infoline.Framework.Helper
{

    public class XmlHelper
    {
        public static string Serialize<T>(T instance, bool dontGetVersion = false)
        {
            using (var ms = new MemoryStream())
            {
                //XmlWriterSettings settings = new XmlWriterSettings();
                //if(dontGetVersion)
                //    settings.OmitXmlDeclaration = true;

                //using (XmlWriter writer = XmlWriter.Create(ms, settings))
                //{
                    var ns = new XmlSerializerNamespaces();
                    ns.Add("", "");

                    new XmlSerializer(instance != null ? instance.GetType() : typeof(string)).Serialize(ms, instance, ns);
                    //new XmlSerializer(instance != null ? instance.GetType() : typeof(string)).Serialize(writer, instance, ns);
                    ms.Position = 0;
                    var sr = new StreamReader(ms);
                    var result = sr.ReadToEnd();
                    return result;
                //}
            }
        }
        
        public static T Deserialize<T>(string serialized)
        {
            using (var ms = new MemoryStream())
            {
                using (var writer = new StreamWriter(ms))
                {
                    writer.Write(serialized);
                    writer.Flush();

                    ms.Position = 0;

                    XmlSerializer deserializer = new XmlSerializer(typeof(T));
                    return (T)deserializer.Deserialize(ms);
                }

            }
        }

        public static string SerializeObject<T>(T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                var xmlserializer = new XmlSerializer(typeof(T));
                var stringWriter = new StringWriter();
                using (var writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred", ex);
            }
        }

    }


}
