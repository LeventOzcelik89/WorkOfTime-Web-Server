using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Infoline.GIS
{
    public class KMLFileLib : GeometryFilePath, IGeometryFile
    {
        public KMLFileLib(string path, string fileName)
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
            return WriteToKmlFile(geometryObject);
        }

        private bool WriteToKmlFile(GeometryObject geometryObject)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(_path + "\\" + _fileName + "\\" + _fileName + ".kml"))
                {
                    writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                    writer.WriteLine("<kml xmlns = \"http://www.opengis.net/kml/2.2\">");
                    writer.WriteLine("<Document id=\"root_doc\">");
                    writer.WriteLine(WriteSchema(geometryObject));
                    writer.WriteLine(WriteFolder(geometryObject));
                    writer.WriteLine("</Document>");
                    writer.WriteLine("</kml>");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private object WriteFolder(GeometryObject geometryObject)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<Folder>");
            stringBuilder.AppendLine("\t<name>" + geometryObject.TableName + "</name>");

            foreach (var item in geometryObject.TableRows)
            {
                try
                {
                    var itemGeometryString = SqlGeographyConverter.WGS84ListToString(SqlGeographyConverter.SQLGeographyToWGS84(item.Polygon));
                    stringBuilder.AppendLine("\t<Placemark>");
                    stringBuilder.AppendLine("\t\t<ExtendedData>");
                    stringBuilder.AppendLine("\t\t\t<SchemaData schemaUrl =\"#" + geometryObject.TableName + "\">");
                    if (item.TableColoumbs != null)
                    {
                        foreach (var item1 in item.TableColoumbs)
                        {
                            stringBuilder.AppendLine("\t\t\t\t<SimpleData name=\"" + item1.FieldName + "\">" + item1.Value + "</SimpleData>");
                        }
                    }
                    stringBuilder.AppendLine("\t\t\t</SchemaData>");
                    stringBuilder.AppendLine("\t\t</ExtendedData>");

                    if (item.GeometryType == GeometryType.PointM ||
                       item.GeometryType == GeometryType.PointZ ||
                       item.GeometryType == GeometryType.Point)
                    {
                        stringBuilder.AppendLine(WritePoint(itemGeometryString).ToString());
                    }

                    else if (item.GeometryType == GeometryType.Polygon ||
                            item.GeometryType == GeometryType.PolygonZ)
                    {
                        stringBuilder.AppendLine(WritePolygon(itemGeometryString).ToString());

                    }

                    else if (item.GeometryType == GeometryType.PolyLine || item.GeometryType == GeometryType.PolyLineM || item.GeometryType == GeometryType.PolyLineZ || item.GeometryType == GeometryType.MultiPoint || item.GeometryType == GeometryType.MultiPointZ || item.GeometryType == GeometryType.MultiPointM)
                    {
                        stringBuilder.AppendLine(WritePolyLine(itemGeometryString).ToString());
                    }

                    stringBuilder.AppendLine("\t</Placemark>");
                }
                catch
                {
                    continue;
                }
            }
            stringBuilder.AppendLine("</Folder>");
            return stringBuilder;
        }

        private object WritePoint(string itemGeometryString)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("\t\t<Point>");
            stringBuilder.AppendLine("\t\t\t<altitudeMode>relativeToGround</altitudeMode>");
            stringBuilder.AppendLine("\t\t\t<coordinates>" + itemGeometryString + "</coordinates>");
            stringBuilder.AppendLine("\t\t</Point>");
            return stringBuilder;
        }

        private object WritePolygon(string itemGeometryString)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("\t\t<Polygon>");
            stringBuilder.AppendLine("\t\t\t<altitudeMode>relativeToGround</altitudeMode>");
            stringBuilder.AppendLine("\t\t\t<outerBoundaryIs>");
            stringBuilder.AppendLine("\t\t\t\t<LinearRing>");
            stringBuilder.AppendLine("\t\t\t\t\t<altitudeMode>relativeToGround</altitudeMode>");
            stringBuilder.AppendLine("\t\t\t\t\t<coordinates>" + itemGeometryString + "</coordinates>");
            stringBuilder.AppendLine("\t\t\t\t</LinearRing>");
            stringBuilder.AppendLine("\t\t\t</outerBoundaryIs>");
            stringBuilder.AppendLine("\t\t</Polygon>");
            return stringBuilder;
        }

        private object WritePolyLine(string itemGeometryString)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("\t\t<MultiGeometry>");
            stringBuilder.AppendLine("\t\t\t<LineString>");
            stringBuilder.AppendLine("\t\t\t\t<altitudeMode>relativeToGround</altitudeMode>");
            stringBuilder.AppendLine("\t\t\t\t<coordinates>" + itemGeometryString + "</coordinates>");
            stringBuilder.AppendLine("\t\t\t</LineString>");
            stringBuilder.AppendLine("\t\t</MultiGeometry>");
            return stringBuilder;
        }

        private StringBuilder WriteSchema(GeometryObject geometryObject)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("<Schema name= \"" + geometryObject.TableName + "\" id=\"" + geometryObject.TableName + "\">");
            if (geometryObject.TableRows.FirstOrDefault().TableColoumbs != null)
            {
                foreach (var item in geometryObject.TableRows.FirstOrDefault().TableColoumbs)
                {
                    stringBuilder.AppendLine("\t<SimpleField name=\"" + item.FieldName + "\" type=\"" + item.SharpType.Name + "\"></SimpleField>");
                }
            }
            stringBuilder.AppendLine("</Schema>");
            return stringBuilder;
        }
    }
    public enum AltitudeMode
    {
        ClampToGround = 0,
        RelativeToGround = 1,
        Absolute = 2
    }
}