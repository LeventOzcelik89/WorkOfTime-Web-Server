using Infoline.Framework.Database;
using Infoline.Framework.Database.Mssql;
using NetTopologySuite.Features;
using NetTopologySuite.IO;
using ProjNet;
using System;
using System.IO;
using System.Linq;

namespace Infoline.Framework
{
    public class ShapeFileHelper
    {
        public static void SaveShape(FeatureCollectionExt data, string path, string name, bool zip = true)
        {
            SaveShape(data, data.TableInfo, name, path, zip);
        }

        public static void SaveShape(FeatureCollection collection, TableInfo info, string filename = null, string outpath = null, bool zip = true)
        {
            string _filename = "";

            if (!string.IsNullOrEmpty(outpath))
            {
                _filename = Path.Combine(outpath, string.Format("{0}.shp", filename));

            }
            var fields = info.Columns.Select(a => new DbaseFieldDescriptor
            {
                Name = (a.ColumnName.Length > 11 ? a.ColumnName.Substring(0, 11) : a.ColumnName),
                DbaseType = ToDbaseType(a.Type),
                Length = a.Length ?? 0
            }).ToArray();

            var _shpWriter = new ShapefileDataWriter(_filename, NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory());
            _shpWriter.Header = ShapefileDataWriter.GetHeader(fields, collection.Count);
            _shpWriter.Write(collection.Features);

            var defaultCRS = SRIDReader.GetCSbyID(4326);
            File.WriteAllText(_filename.Replace(".shp", ".prj"), defaultCRS.WKT);
            File.WriteAllText(_filename.Replace(".shp", ".cpg"), "UTF-8\r\n");

            var _zip = Path.Combine(outpath, string.Format("{0}.zip", filename));
            if (zip)
            {
                Framework.Helper.Zip.Created(_zip, new string[]
                {
                    _filename,
                    _filename.Replace(".shp", ".prj"),
                    _filename.Replace(".shp", ".cpg"),
                    _filename.Replace(".shp", ".shx"),
                    _filename.Replace(".shp", ".dbf"),
                }, zip);
            }
        }

        static char ToDbaseType(Type type)
        {
            if (type == typeof(double) || type == typeof(float) ||
                type == typeof(short) || type == typeof(ushort) ||
                type == typeof(int) || type == typeof(uint) ||
                type == typeof(long) || type == typeof(ulong) ||
                type == typeof(byte))
                return 'N';
            else if (type == typeof(string) || type == typeof(Guid))
                return 'C';
            else if (type == typeof(bool))
                return 'L';
            else if (type == typeof(DateTime))
                return 'D';
            else throw new ArgumentException("Type " + type.Name + " not supported");
        }
    }
}
