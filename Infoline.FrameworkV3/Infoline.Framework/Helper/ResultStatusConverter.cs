using Infoline.Framework.Database;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Helper
{
    public class ResultStatusConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (typeof(ResultStatus).IsAssignableFrom(objectType));
            //return (typeof(ResultStatus).IsAssignableFrom(objectType)) || typeof(DataLoggerData) == objectType || typeof(DataLoggerDataRow) == objectType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //// Need to use reflection here because Elements is private
            //PropertyInfo prop = typeof(Composite).GetProperty("Elements", BindingFlags.NonPublic | BindingFlags.Instance);
            //List<Element> children = (List<Element>)prop.GetValue(composite);

            //JArray array = new JArray();
            //foreach (Element e in children)
            //{
            //    array.Add(JToken.FromObject(e, serializer));
            //}

            //JObject obj = new JObject();
            //obj.Add(composite.Name, array);
            //obj.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            if (reader.TokenType == JsonToken.StartObject && (typeof(ResultStatus).IsAssignableFrom(objectType)))
            {
                var result = Activator.CreateInstance(objectType);
                reader.Read();
                while (reader.TokenType == JsonToken.PropertyName)
                {
                    var name = reader.Value.ToString();
                    reader.Read();
                    if (name != "objects")
                    {
                        object val = reader.Value;
                        reader.Read();
                        result.GetType().GetProperty(name).SetValue(result, val);
                    }
                    else
                    {
                        var val = serializer.Deserialize(reader, objectType.GenericTypeArguments[0]);
                        reader.Read();

                        var prop = result.GetType().GetProperties().Where(a => a.Name == "objects" && a.DeclaringType != typeof(object)).FirstOrDefault();
                        prop.SetValue(result, val);
                    }
                }
                return result;
            }
            return serializer.Deserialize(reader);
            //throw new NotImplementedException();
        }
    }
}
