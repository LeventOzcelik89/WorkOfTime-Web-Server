using EGIS.ShapeFileLib;
using Infoline.GIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using System.IO;

namespace Infoline.GIS
{
    public class ShapeRead
    {
        public string _path;
        public ShapeRead(string path)
        {
            this._path = path;
        }

        public GeometryObject ReadDataFromShapeFile()
        {
            var dbfReader = new DbfReader(_path);
            var fieldDescs = dbfReader.DbfRecordHeader.GetFieldDescriptions();
            ShapeFile sf = new ShapeFile(_path);
            var polygons = ReadLinesFromShapeFile();
            var tableRows = new List<TableRow>();

            

            for (int record = 0; record < dbfReader.DbfRecordHeader.RecordCount; record++)
            {
                try
                {
                    var TableColoumbsList = new List<TableColoumbs>();
                    for (int i = 0; i < fieldDescs.Count(); i++)
                    {
                        TableColoumbsList.Add(
                            new TableColoumbs
                            {
                                FieldIndex = i,
                                Lenght = fieldDescs[i].FieldLength,
                                FieldName = fieldDescs[i].FieldName,
                                SharpType = ShapeConverter.ConvertShapeTypeToCSharpType(fieldDescs[i].FieldType),
                                Value = ShapeCheck.CheckDbfFieldTypeValue(dbfReader.GetFields(record)[i], fieldDescs[i].FieldType),
                            });
                    }
                    tableRows.Add(new TableRow
                    {
                        GeometryType = ShapeConverter.ConvertShapeTypeToGeometryType(sf.ShapeType),
                        idx = Guid.NewGuid(),
                        TableColoumbs = TableColoumbsList.ToArray(),
                        PolygonString = polygons[record].LineGeometry.ToString(),
                        Polygon = polygons[record].LineGeometry
                    });
                }
                catch
                {
                    continue;
                }
            }

            var generalObject = new GeometryObject();
            generalObject.TableName = Path.GetFileName(_path).Replace(".shp", "");
            generalObject.GeometryType = ShapeConverter.ConvertShapeTypeToGeometryType(sf.ShapeType);
            generalObject.TableName = Path.GetFileName(_path).Replace(".shp", "");
            generalObject.FieldNames = dbfReader.GetFieldNames();
            generalObject.TableRows = tableRows.ToArray();

            return generalObject;
        }

        public List<DbfFieldDesc> ReadDbfFieldDesc(GeometryObject geometryObject)
        {
            var fieldDescs = new List<DbfFieldDesc>();

            foreach (var item in geometryObject.TableRows.FirstOrDefault().TableColoumbs)
            {
                if (item.FieldName != "PolygonField")
                {
                    var dbfFieldProperties = ShapeConverter.ConvertSharpTypeToDbfFieldType(item.SharpType, item.Lenght);
                    int recordOffset = 1;
                    fieldDescs.Add(new DbfFieldDesc
                    {
                        FieldName = item.FieldName,
                        FieldType = dbfFieldProperties.FieldType,
                        FieldLength = dbfFieldProperties.FieldLength,
                        RecordOffset = recordOffset,
                        DecimalCount = dbfFieldProperties.DecimalCount
                    });
                    recordOffset += dbfFieldProperties.FieldLength;
                }
            }

            return fieldDescs;
        }

        private GeometryOfShapeFile[] ReadLinesFromShapeFile()
        {
            var path = _path.Replace("dbf", "shp");
            EGIS.ShapeFileLib.ShapeFile sf = new EGIS.ShapeFileLib.ShapeFile(path);
            EGIS.ShapeFileLib.ShapeFileEnumerator sfEnum = sf.GetShapeFileEnumerator();
            int recordIndex = 0;
            var lineList = new List<GeometryOfShapeFile>();
            
            while (sfEnum.MoveNext())
            {
                try
                {
                    var line = new GeometryOfShapeFile();
                    System.Collections.ObjectModel.ReadOnlyCollection<PointD[]> pointRecords = sfEnum.Current;
                   

                    foreach (PointD[] pts in pointRecords)
                    {
                        line.LineString = "";
                        for (int n = 0; n < pts.Length; ++n)
                        {
                            if (n > 0)
                                line.LineString += ",";
                            line.LineString += pts[n].X.ToString().Replace(",", ".") + " " + pts[n].Y.ToString().Replace(",", ".");
                        }
                        line.pointList = pts;
                        var lineStringCheck = ShapeCheck.CheckPolygonString(line.LineString, sf.ShapeType);
                        line.LineGeometry = SqlGeographyConverter.StringToSqlGeography(lineStringCheck); //, sf.ShapeType

                    }

                    line.Record = recordIndex;
                    line.shapeType = sf.ShapeType;
                    lineList.Add(line);
                    ++recordIndex;
                }

                catch
                {
                    continue;
                }
            }

            sfEnum.Dispose();
            sf.Close();
            sf.Dispose();
            return lineList.ToArray();
        }


        public List<string[]> ReadFieldData(GeometryObject geometryObject)
        {
            var strArrayList = new List<string[]>();
            foreach (var tableRow in geometryObject.TableRows)
            {
                var strList = new List<string>();
                foreach (var tableColoumb in tableRow.TableColoumbs)
                {
                    if (tableColoumb.FieldName != "PolygonField")
                    {
                        strList.Add(tableColoumb.Value.ToString());
                    }
                }
                strArrayList.Add(strList.ToArray());
            }
            return strArrayList;
        }
    }
    
}
