using GeoAPI.Geometries;
using Infoline.GIS;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.GIS
{
    public class TableColoumbs
    {

        public int FieldIndex { get; set; }
        public string FieldName { get; set; }
        public int Lenght { get; set; }
        public Type SharpType { get; set; }
        public object Value { get; set; }
    }

    public class TableRow
    {
        // TODO: CHECK [FM]
        public Guid idx { get; set; }
        public TableColoumbs[] TableColoumbs { get; set; }
        public GeometryType GeometryType { get; set; }
        public IGeometry Polygon { get; set; }
        public string PolygonString { get; set; }
    }

    public class GeometryObject
    {
        public string TableName { get; set; }
        public TableRow[] TableRows { get; set; }
        public string[] FieldNames { get; set; }
        public GeometryType GeometryType { get; set; }

        public void CheckGeometryObject()
        {

            if (!string.IsNullOrEmpty(TableName))
            {
                TableName = TableName.Trim().Replace(".", "_");
            }

            foreach (var TableRow in this.TableRows)
            {
                foreach (var TableColoumb in TableRow.TableColoumbs)
                {
                    if (TableColoumb.SharpType == null)
                    {
                        if (TableColoumb.Value == null)
                        {
                            TableColoumb.SharpType = typeof(string);
                        }
                        else
                        {
                            TableColoumb.SharpType = TableColoumb.Value.GetType();
                        }

                        if (TableColoumb.SharpType.Name == "Boolean") {
                            TableColoumb.Value = (Boolean)TableColoumb.Value ? 0 : 1;
                        }

                    }
                }

                if (TableRow.Polygon == null)
                {
                    if (TableRow.PolygonString != null)
                    {
                        TableRow.Polygon = SqlGeographyConverter.StringToSqlGeography(TableRow.PolygonString);
                    }
                }

                if (TableRow.PolygonString == null)
                {
                    if (TableRow.Polygon != null)
                    {
                        TableRow.PolygonString = TableRow.Polygon.ToString();
                    }
                }
            }
        }
    }
}
