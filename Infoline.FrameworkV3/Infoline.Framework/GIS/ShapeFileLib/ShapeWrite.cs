using EGIS.ShapeFileLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.GIS
{
    public class ShapeWrite
    {
        private string _path { get; set; }

        private string _fileName { get; set; }

        public ShapeWrite(string path, string fileName)
        {
            this._path = path;
            this._fileName = fileName;
        }

        public bool WriteDataShapeFile(GeometryObject geometryObject)
        {
            var shapeRead = new ShapeRead(_path);
            var fieldDescs = shapeRead.ReadDbfFieldDesc(geometryObject);
            var pointList = ShapeConverter.GetPointDList(geometryObject);
            var fieldData = shapeRead.ReadFieldData(geometryObject);
            return CreateShapeFiles(ShapeConverter.ConvertGeometryTypeToShapeType(geometryObject.GeometryType), pointList, fieldDescs.ToArray(), fieldData);
        }

        private bool CreateShapeFiles(ShapeType shapeType, List<EGIS.ShapeFileLib.PointD[]> arrpt, DbfFieldDesc[] fieldDescs, List<string[]> fieldNames)
        {
            try
            {
                var newPath = _path + "\\" + _fileName;
                if (!Directory.Exists(newPath))
                    Directory.CreateDirectory(newPath);

                ShapeFileWriter sfw;
                sfw = ShapeFileWriter.CreateWriter(newPath, _fileName, shapeType, fieldDescs.ToArray());
                int i = 0;
                foreach (var item in arrpt)
                {
                    sfw.AddRecord(item, item.Length, fieldNames[i]);
                    i++;
                }
                sfw.Close();
                var destinationFolder = "C:\\windows\\temp" + "\\" + _fileName + ".zip";
                
                if (!File.Exists(destinationFolder))
                    ZipFile.CreateFromDirectory(newPath, destinationFolder);
                if (!File.Exists(newPath + "\\" + _fileName + ".zip"))
                    Directory.Move(destinationFolder, newPath + "\\" + _fileName + ".zip");
                return true;
            }
            catch (Exception ex)
            {
                var k = ex;
                return false;
            }
        }
    }
}
