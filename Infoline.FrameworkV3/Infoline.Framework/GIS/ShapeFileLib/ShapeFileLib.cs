using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Types;
using EGIS.ShapeFileLib;
using System.IO.Compression;
using GeoAPI.Geometries;

namespace Infoline.GIS
{
    public class ShapeFileLibrary : IGeometryFile
    {
        private string _path { get; set; }

        private string _fileName { get; set; }

        public ShapeFileLibrary(string path, string fileName)
        {
            this._path = path;
            this._fileName = fileName;
        }

        public GeometryObject ReadData()
        {
            var shapeRead = new ShapeRead(_path);
            return shapeRead.ReadDataFromShapeFile();
        }

        public bool WriteData(GeometryObject geometryObject)
        {
            var writeShape = new ShapeWrite(this._path, this._fileName);
            return writeShape.WriteDataShapeFile(geometryObject);
        }
       
    }

    public class DbfFieldProperties
    {
        public int DecimalCount { get; set; }
        public int FieldLength { get; set; }
        public DbfFieldType FieldType { get; set; }
    }

    public class GeometryOfShapeFile
    {
        // TODO: CHECK[FM]
        public PointD[] pointList { get; set; }
        public ShapeType shapeType { get; set; }
        public string LineString { get; set; }
        public IGeometry LineGeometry { get; set; }
        public int Record { get; set; }
    }
}
