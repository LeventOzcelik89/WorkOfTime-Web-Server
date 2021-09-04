using GeoAPI.Geometries;
using Infoline.Framework.Database;
using Microsoft.SqlServer.Types;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;

namespace Infoline.Helper
{
    public static class Json
    {
        public static string Serialize<T>(T instance, params JsonConverter[] convs)
        {
            JsonConverter[] converters = new JsonConverter[]
            {
                new FeatureCollectionJsonConverter(),
                new FeatureJsonConverter(),
                new GeometryJsonConverter(),
                new EnvelopeJsonConverter(),
                new ConditionJsonConverter(),
            };

            converters = converters.Union(convs).Reverse().ToArray();

            return JsonConvert.SerializeObject(instance, converters);
        }

        public static object DeserializeObject(string value, Type type, params JsonConverter[] convs)
        {

            JsonConverter[] converters = new JsonConverter[]
            {
                new FeatureCollectionJsonConverter(),
                new FeatureJsonConverter(),
                new GeometryJsonConverter(),
                new EnvelopeJsonConverter(),
                new ConditionJsonConverter(),
            };

            converters = converters.Union(convs).Reverse().ToArray();

            return JsonConvert.DeserializeObject(value, type, converters);
        }

        public static void Populate(string str, object obj)
        {
            Newtonsoft.Json.JsonConvert.PopulateObject(str, obj);
        }

        public static T Deserialize<T>(string serialized, params JsonConverter[] convs)
        {
            return (T)DeserializeObject(serialized, typeof(T), convs);
        }

        public static T Deserialize2<T>(string serialized)
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

        public static object Deserialize<T>(object model)
        {
            throw new NotImplementedException();
        }
    }





    public class ConditionJsonConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Condition).IsAssignableFrom(objectType);
        }


        private BEXP Casting(dynamic data)
        {
            if (data == null) return null;
            var operatorText = (data["Operator"] ?? data["operator"]).ToString();
            BinaryOperator operatorm;
            var isParse = Enum.TryParse<BinaryOperator>(operatorText, out operatorm);
            if (!isParse) return null;

            if (operatorm == BinaryOperator.And || operatorm == BinaryOperator.Or)
            {
                return new BEXP
                {
                    Operand1 = Casting(data.Operand1 ?? data.operand1),
                    Operator = operatorm,
                    Operand2 = Casting(data.Operand2 ?? data.operand2)
                };
            }
            else
            {
                var operand2 = (data.Operand2 ?? data.operand2);
                if (operand2.Type == Newtonsoft.Json.Linq.JTokenType.Array)
                {
                    var arr = (string[])Json.Deserialize<string[]>(operand2.ToString());
                    return new BEXP
                    {
                        Operand1 = new COL((data.Operand1 ?? data.operand1).ToString()),
                        Operator = operatorm,
                        Operand2 = new ARR { Values = arr.Select(a => (VAL)a).ToArray() }
                    };
                }
                else
                {
                    return new BEXP
                    {
                        Operand1 = new COL((data.Operand1 ?? data.operand1).ToString()),
                        Operator = operatorm,
                        Operand2 = new VAL((operand2).ToString())
                    };

                }
            }
        }





        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Condition condition = new ConditionEx();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var property = reader.Value.ToString().ToLower();
                    reader.Read();
                    if (property.EndsWith("filter"))
                    {

                        if (reader.TokenType == JsonToken.StartObject)
                        {
                            var filter = serializer.Deserialize(reader);
                            condition = new ConditionNew
                            {
                                StartIndex = condition.StartIndex,
                                Count = condition.Count,
                                Sort = condition.Sort,
                                Fields = condition.Fields,
                                Filter = Casting(filter)
                            };
                        }
                        else if (reader.TokenType == JsonToken.StartArray)
                        {
                            ((ConditionEx)condition).Filter = serializer.Deserialize<QueryCondition[]>(reader);
                        }
                        else
                        {
                            ((ConditionEx)condition).Filter = null;
                        }

                    }
                    else if (property.EndsWith("sort"))
                    {
                        condition.Sort = serializer.Deserialize<QuerySort>(reader);
                    }
                    else if (property.EndsWith("ndex"))
                    {
                        condition.StartIndex = serializer.Deserialize<int?>(reader);
                    }
                    else if (property.EndsWith("count"))
                    {
                        condition.Count = serializer.Deserialize<int?>(reader);
                    }
                    else if (property.EndsWith("fields"))
                    {
                        condition.Fields = serializer.Deserialize<string[]>(reader);
                    }
                    else
                    {
                        //serializer.Deserialize(reader);
                    }
                }
                else if (reader.TokenType == JsonToken.EndObject)
                    break;
            }

            return condition;
        }



        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

        }
    }


    public class JsonSqlGeometryConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(SqlGeography).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;
            var result = SqlGeography.Parse(reader.Value.ToString());
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
                serializer.Serialize(writer, value.ToString());
            else serializer.Serialize(writer, value);
        }
    }

    public class EnvelopeJsonConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(Envelope).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;
            var wktReader = new WKTReader();
            var result = wktReader.Read(reader.Value.ToString()).EnvelopeInternal;
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
                serializer.Serialize(writer, GetGeometry((Envelope)value).AsText());
            else serializer.Serialize(writer, value);
        }

        public static IGeometry GetGeometry(Envelope env)
        {
            IGeometryFactory factory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory();
            return factory.CreatePolygon(new Coordinate[]
            {
                new Coordinate { X = env.MinX, Y = env.MaxY },
                new Coordinate { X = env.MinX, Y = env.MinY },
                new Coordinate { X = env.MaxX, Y = env.MinY },
                new Coordinate { X = env.MaxX, Y = env.MaxY },
                new Coordinate { X = env.MinX, Y = env.MaxY },
            });
        }
    }

    public class GeometryJsonConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IGeometry).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) return null;
            var wktReader = new WKTReader();
            var result = wktReader.Read(reader.Value.ToString());
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
                serializer.Serialize(writer, ((IGeometry)value).AsText());
            else serializer.Serialize(writer, value);
        }
    }

    public class FeatureJsonConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(IFeature).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var feature = new Feature();
            feature.Attributes = new AttributesTable();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    var propertyName = reader.Value.ToString();
                    if (propertyName == "shape")
                    {
                        reader.Read();
                        var shape = serializer.Deserialize<IGeometry>(reader);
                        feature.Geometry = shape;
                    }
                    else
                    {
                        reader.Read();
                        var value = serializer.Deserialize(reader);
                        feature.Attributes.AddAttribute(propertyName, value);
                    }
                }
                else if (reader.TokenType == JsonToken.EndObject)
                    break;
            }
            return feature;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                var feature = (IFeature)value;

                var geoJsonWriter = new GeoJsonWriter();
                var json = geoJsonWriter.Write(feature);
                writer.WriteRawValue(json);
            }
            else serializer.Serialize(writer, value);
        }
    }

    public class FeatureCollectionJsonConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(FeatureCollection).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            FeatureCollection collection = new FeatureCollection();
            collection.BoundingBox = new Envelope();
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    if (reader.Path == "features")
                    {
                        reader.Read();
                        if (reader.TokenType == JsonToken.StartArray)
                        {
                            while (reader.Read())
                            {
                                if (reader.TokenType == JsonToken.EndArray)
                                    break;
                                var feature = serializer.Deserialize<Feature>(reader);
                                collection.Add(feature);
                                if (feature != null && feature.Geometry != null)
                                    collection.BoundingBox = collection.BoundingBox.ExpandedBy(feature.Geometry.EnvelopeInternal);
                            }
                        }
                    }
                }
            }
            return collection;

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                var collection = (FeatureCollection)value;
                var geoJsonWriter = new GeoJsonWriter();
                var json = geoJsonWriter.Write(collection);
                writer.WriteRawValue(json);
            }
            else serializer.Serialize(writer, value);
        }
    }

    public class DateTime_TR_UTC_JsonConverter : Newtonsoft.Json.JsonConverter
    {


        public override bool CanConvert(Type objectType)
        {
            return typeof(DateTime).IsAssignableFrom(objectType) || typeof(DateTime?).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }
            else
            {
                var cc = new CultureInfo("tr-TR", false);

                var value = ((DateTime)reader.Value);
                if (value.Kind == DateTimeKind.Utc)
                {
                    return value.ToLocalTime();
                }
                else
                {
                    return value;
                }
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {

                var cc = new DateTime(((DateTime)value).Ticks, DateTimeKind.Local);

                var deger = JsonConvert.SerializeObject(cc, new JsonSerializerSettings
                {
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                }).Replace("\"", "");
                serializer.Serialize(writer, deger);
            }
            else
            {
                serializer.Serialize(writer, value);
            }
        }


    }



    public class JsonHelper
    {
        /// <summary>
        /// JSON Serialization
        /// </summary>
        public static string JsonSerializer<T>(T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }
        /// <summary>
        /// JSON Deserialization
        /// </summary>
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
    }
}