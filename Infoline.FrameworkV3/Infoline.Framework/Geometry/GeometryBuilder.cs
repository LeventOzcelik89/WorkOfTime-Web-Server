using GeoAPI.Geometries;
using NetTopologySuite.IO;
using ProjNet.Converters.WellKnownText;
using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework
{
    public class GeometryBuilder
    {
        static WKTReader wktReader = new WKTReader();
        static WKBReader wkbReader = new WKBReader();


        public static IGeometry FromText(string wkt)
        {
            var geometry = wktReader.Read(wkt);
            geometry = GeometryValidator.MakeValid(geometry);
            return geometry;
        }

        public static IGeometry FromWKB(byte[] wkb)
        {
            var geometry = wkbReader.Read(wkb);
            geometry = GeometryValidator.MakeValid(geometry);
            return geometry;
        }


        public IGeometry ChangeProjection(IGeometry geometry, string sourceWkt, string targetWkt)
        {
            var sourceCrs = sourceWkt != null ?
                                (CoordinateSystemWktReader.Parse(sourceWkt) as ICoordinateSystem) :
                                (ProjNet.SRIDReader.GetCSbyID(4326) as ICoordinateSystem);


            var targetCrs = targetWkt != null ?
                                (CoordinateSystemWktReader.Parse(targetWkt) as ICoordinateSystem) :
                                (ProjNet.SRIDReader.GetCSbyID(4326) as ICoordinateSystem);

            if (sourceCrs.EqualParams(targetCrs))
                return geometry;

            var gtFac = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            var transformFunction = gtFac.CreateFromCoordinateSystems(sourceCrs, targetCrs);

            var points = geometry.Coordinates.Select(a => transformFunction.MathTransform.Transform(new[] { a.X, a.Y, a.Z })).ToArray();
            var len = geometry.Coordinates.Length;
            var i = 0;
            foreach (var coordinate in geometry.Coordinates)
            {
                var point = points[i];

                coordinate.X = point[0];
                coordinate.Y = point[1];
                coordinate.Z = point[2];
                i++;
            }

            return geometry;

        }

    }
}
