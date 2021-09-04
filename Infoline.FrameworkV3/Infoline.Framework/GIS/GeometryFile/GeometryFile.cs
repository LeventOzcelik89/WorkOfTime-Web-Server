namespace Infoline.GIS
{
    public class GeometryFile : GeometryFilePath
    {
        private GeometryObject _geometryObject { get; set; }
        private GeometryFileType _geometryFileType { get; set; }

        public GeometryFile(string path, string fileName, GeometryObject geometryObject, GeometryFileType geometryFileType)
        {
            _path = path;
            _geometryFileType = geometryFileType;
            _geometryObject = geometryObject;
            _fileName = fileName;
        }

        public bool WriteGeometryFile()
        {
            _geometryObject.CheckGeometryObject();

            if (_geometryFileType == GeometryFileType.GEOJSON)
            {
                var geoJsonFile = new GeoJsonFileLib(_path, _fileName);
                return (geoJsonFile.WriteData(_geometryObject));
            }
            else if (_geometryFileType == GeometryFileType.KML)
            {
                var kmlFile = new KMLFileLib(_path, _fileName);
                return kmlFile.WriteData(_geometryObject);
            }
            else if (_geometryFileType == GeometryFileType.MSSQL)
            {
                return false;
            }
            else if (_geometryFileType == GeometryFileType.SHAPE)
            {
                var shapeFile = new ShapeFileLibrary(_path, _fileName);
                return (shapeFile.WriteData(_geometryObject));
            }
            else if (_geometryFileType == GeometryFileType.TOPOJSON)
            {
                return false;
            }
            else
            {
                return false;
            }
        }

        public string DownloadFileTile(GeometryFileType exportType)
        {
            if (exportType == GeometryFileType.GEOJSON)
                return "geojson";
            else if (exportType == GeometryFileType.SHAPE)
                return "zip";
            else if (exportType == GeometryFileType.KML)
                return "kml";
            else if (exportType == GeometryFileType.TOPOJSON)
                return "topojson";
            else
                return "";
        }
    }
}
