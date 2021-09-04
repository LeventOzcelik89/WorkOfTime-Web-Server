using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.GIS
{
    public class GeoJsonFileLib : GeometryFilePath, IGeometryFile
    {
      
        public GeoJsonFileLib(string path, string fileName)
        {
            this._path = path;
            this._fileName = fileName;
        }

        public GeometryObject ReadData()
        {
            throw new NotImplementedException();
        }

        public bool WriteData(GeometryObject geometryObject)
        {
            try
            {
                var geoJsonObject = new GeoJsonObject();
                geoJsonObject.type = GeoJSONObjectType.FeatureCollection.ToString();
                geoJsonObject.crs = GetCrs();
                geoJsonObject.features = GetFeatureListByGeometryObject(geometryObject);

                using (StreamWriter writer = new StreamWriter(_path + "\\" + _fileName + "\\" + _fileName + ".geojson"))
                {
                    writer.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(geoJsonObject));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private Crs GetCrs()
        {
            var crsDictionary = new Dictionary<string, object>();
            crsDictionary.Add("name", "urn:ogc:def:crs:OGC:1.3:CRS84");
            var crs = new Crs();
            crs.type = CRSType.name.ToString();
            crs.properties = crsDictionary;
            return crs;
        }

        private Features[] GetFeatureListByGeometryObject(GeometryObject geometryObject)
        {
            var featuresList = new List<Features>();
            foreach (var item in geometryObject.TableRows)
            {
                var featuresPropertiesDictionary = new Dictionary<string, object>();
                foreach (var tableColoumb in item.TableColoumbs)
                {
                    if (tableColoumb.FieldName != "PolygonField")
                        featuresPropertiesDictionary.Add(tableColoumb.FieldName, tableColoumb.Value);
                }

                var listCoordinates = new List<WGS84>();
                var coordinatesWGS84 = SqlGeographyConverter.SQLGeographyToWGS84(item.Polygon);

                if (item.GeometryType == GeometryType.MultiPoint ||
                   item.GeometryType == GeometryType.MultiPointZ ||
                   item.GeometryType == GeometryType.PointM ||
                   item.GeometryType == GeometryType.PointZ ||
                   item.GeometryType == GeometryType.Point)
                {
                    var geometry = new GeometryPoint();
                    var coordinate = new List<double>();
                    coordinate.Add(coordinatesWGS84.FirstOrDefault().Longitude);
                    coordinate.Add(coordinatesWGS84.FirstOrDefault().Latitude);
                    coordinate.Add(coordinatesWGS84.FirstOrDefault().Altitude);
                    
                    var geometryType = ConvertToGeometryTypeToGeoJsonType(item.GeometryType);
                    geometry.type = geometryType.ToString();
                    geometry.coordinates = coordinate.ToArray();
                    featuresList.Add(new Features { geometry = geometry, properties = featuresPropertiesDictionary, type = GeoJSONObjectType.Feature.ToString() });
                }


                else
                {
                    var geometry = new GeometryLineString();
                    var listCoordinate = new List<double[]>();

                    foreach (var coordinateWGS84 in coordinatesWGS84)
                    {
                        var coordinate = new List<double>();
                        coordinate.Add(coordinateWGS84.Longitude);
                        coordinate.Add(coordinateWGS84.Latitude);
                        coordinate.Add(coordinateWGS84.Altitude);
                        listCoordinate.Add(coordinate.ToArray());
                    }

                    var geometryType = ConvertToGeometryTypeToGeoJsonType(item.GeometryType);
                    geometry.type = geometryType.ToString();
                    geometry.coordinates = listCoordinate;
                    featuresList.Add(new Features { geometry = geometry, properties = featuresPropertiesDictionary, type = GeoJSONObjectType.Feature.ToString() });
                }

            }
            return featuresList.ToArray();
        }

        private GeoJSONObjectType ConvertToGeometryTypeToGeoJsonType(GeometryType geometryType)
        {
            if (geometryType == GeometryType.MultiPoint)
                return GeoJSONObjectType.MultiPoint;

            else if (geometryType == GeometryType.MultiPointZ)
                return GeoJSONObjectType.MultiPoint;

            else if (geometryType == GeometryType.Point)
                return GeoJSONObjectType.Point;

            else if (geometryType == GeometryType.PointM)
                return GeoJSONObjectType.Point;

            else if (geometryType == GeometryType.PointZ)
                return GeoJSONObjectType.Point;

            else if (geometryType == GeometryType.Polygon)
                return GeoJSONObjectType.Polygon;

            else if (geometryType == GeometryType.PolygonZ)
                return GeoJSONObjectType.Polygon;

            else if (geometryType == GeometryType.PolyLine)
                return GeoJSONObjectType.LineString;

            else if (geometryType == GeometryType.PolyLineM)
                return GeoJSONObjectType.LineString;

            else if (geometryType == GeometryType.PolyLineZ)
                return GeoJSONObjectType.LineString;
            else
                return GeoJSONObjectType.LineString;
        }
    }

    public class GeoJsonObject
    {
        public string type { get; set; }
        public Crs crs { get; set; }
        public Features[] features { get; set; }
    }

    public class Features
    {
        public string type { get; set; }
        public Dictionary<string, object> properties { get; set; }
        public object geometry { get; set; }
    }

    public class GeometryLineString
    {
        public string type { get; set; }
        public List<double[]> coordinates { get; set; }
    }

    public class GeometryPoint
    {
        public string type { get; set; }
        public double[] coordinates { get; set; }
    }

    public class Crs
    {
        public string type { get; set; }
        public Dictionary<string, object> properties { get; set; }
    }

    public enum CRSType
    {
        unspecified,
        name,
        link

    }

    public enum GeoJSONObjectType
    {
        Point,
        MultiPoint,
        LineString,
        MultiLineString,
        Polygon,
        MultiPolygon,
        GeometryCollection,
        Feature,
        FeatureCollection
    }
}
