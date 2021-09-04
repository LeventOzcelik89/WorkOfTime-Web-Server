using GeoAPI.Geometries;
using Microsoft.SqlServer.Types;
using NetTopologySuite.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework
{
    public class GeometryValidator
    {
        static IGeometryFactory factory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory();


        public static bool IsValid(IGeometry geometry)
        {
            var sqlGeometry = SqlGeography.STGeomFromText(new System.Data.SqlTypes.SqlChars(geometry.AsText().ToArray()), 4326);
            return sqlGeometry.STIsValid().IsTrue;
        }

        public static IGeometry MakeValid(IGeometry geometry)
        {
            return MakeValid(geometry, true);
        }

        public static IGeometry ReorientObject(IGeometry geometry)
        {
            if (geometry is ILinearRing)
                return ReorientObject(geometry as ILinearRing);
            else if (geometry is IPolygon)
                return ReorientObject(geometry as IPolygon);
            else if (geometry is IMultiPolygon)
                return ReorientObject(geometry as IMultiPolygon);
            return geometry;
        }


        private static IGeometry MakeValid(IGeometry geometry, bool forceXY)
        {
            geometry = ReorientObject(geometry);
            var sqlGeometry = ToSqlGeography(geometry);
            if (!sqlGeometry.STIsValid())
            {
                sqlGeometry = sqlGeometry.MakeValid(); // z ve m eksenleri gidiyor.
                var newGeometry = ToIGeometry(sqlGeometry);
                newGeometry = GetOldCoordinates(newGeometry, geometry, forceXY);

                sqlGeometry = ToSqlGeography(newGeometry);
                var isStilValid = sqlGeometry.STIsValid();
                if (isStilValid)
                {
                    if (newGeometry is ILineString)
                    {
                        if (sqlGeometry.EnvelopeAngle() > 90)
                        {
                            sqlGeometry = sqlGeometry.ReorientObject();
                            newGeometry = ToIGeometry(sqlGeometry);
                            newGeometry = GetOldCoordinates(newGeometry, geometry, forceXY);
                        }
                    }
                    return newGeometry;
                }
                else
                {
                    return MakeValid(geometry, false);
                }
            }

            return geometry;
        }


        private static IGeometry GetOldCoordinates(IGeometry newGeometry, IGeometry oldGeometry, bool forceXY = true)
        {
            foreach (var coordinate in newGeometry.Coordinates)
            {
                GeoAPI.Geometries.Coordinate oldCoordinate = null;
                foreach (var oldCrds in oldGeometry.Coordinates)
                {
                    if (oldCrds.Equals2D(coordinate, 0.0000001))
                    {
                        oldCoordinate = oldCrds;
                        break;
                    }
                }

                if (oldCoordinate != null)
                {
                    if (forceXY == true)
                    {
                        coordinate.X = oldCoordinate.X;
                        coordinate.Y = oldCoordinate.Y;
                    }
                    coordinate.Z = oldCoordinate.Z;
                    coordinate.M = oldCoordinate.M;
                }
            }

            return newGeometry;
        }
        private static SqlGeography ToSqlGeography(IGeometry geometry)
        {
            var sqlGeometry = SqlGeography.STGeomFromText(new System.Data.SqlTypes.SqlChars(geometry.AsText().ToArray()), 4326);
            return sqlGeometry;
        }
        private static IGeometry ToIGeometry(SqlGeography sqlGeography)
        {
            return new WKTReader().Read(sqlGeography.MakeValid().ToString());
        }

        private static ILinearRing ReorientObject(ILinearRing ring)
        {
           
            if (((NetTopologySuite.Geometries.LineString)ring).IsEmpty == false) 
                if (!ring.IsCCW) // 2017-05-12 my boş olunca hata veriyordu. IsEmpty ekledim.
                    return ring.Reverse() as ILinearRing;
            return ring;
        }
        private static IPolygon ReorientObject(IPolygon polygon)
        {
            var shell = ReorientObject((IGeometry)polygon.Shell) as ILinearRing;
            var holes = polygon.Holes.Select(a => ReorientObject(a) as ILinearRing).ToArray();
            return factory.CreatePolygon(shell, holes);
        }
        private static IMultiPolygon ReorientObject(IMultiPolygon multiPolygon)
        {
            List<IPolygon> polygons = new List<IPolygon>();
            for (var i = 0; i < multiPolygon.Count; i++)
                polygons.Add(ReorientObject(multiPolygon[i]) as IPolygon);
            //var polygons = multiPolygon.Select((a,i) => ).ToArray();
            return factory.CreateMultiPolygon(polygons.ToArray());
        }

    }
}
