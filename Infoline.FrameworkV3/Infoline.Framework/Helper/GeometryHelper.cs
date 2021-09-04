using GeoAPI.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infoline.Framework.Helper
{
    public class GeometryHelper
    {
        public static IGeometry GeomFromWKT(string wkt)
        {
            var reader = new NetTopologySuite.IO.WKTReader();
            return reader.Read(wkt);
        }

        public static IGeometry GeomFromWKB(byte[] wkb)
        {
            var reader = new NetTopologySuite.IO.WKBReader();
            return reader.Read(wkb);
        }


        public static bool IsValid(IGeometry geometry)
        {
            var validator = GeometryValidatorFactory.GetValidator(geometry);
            return validator.IsValid(geometry);
        }

        public static IGeometry MakeValid(IGeometry geometry)
        {
            var validator = GeometryValidatorFactory.GetValidator(geometry);
            return validator.MakeValid(geometry);
        }

    }

    public interface IGeometryValidator
    {
        bool IsValid(IGeometry geometry);
        IGeometry MakeValid(IGeometry geometry);
    }
    public class GeometryValidatorFactory
    {
        public static IGeometryValidator GetValidator(IGeometry geometry)
        {
            if (geometry is IPolygon)
                return new PolygonValidator();

            return null;
        }
    }

    public class PointValidator : IGeometryValidator
    {
        public bool IsValid(IGeometry geometry)
        {
            throw new NotImplementedException();
        }
        public IGeometry MakeValid(IGeometry geometry)
        {
            throw new NotImplementedException();
        }
    }
    public class LineStringValidator : IGeometryValidator
    {
        public bool IsValid(IGeometry geometry)
        {
            throw new NotImplementedException();
        }
        public IGeometry MakeValid(IGeometry geometry)
        {
            throw new NotImplementedException();
        }
    }
    public class PolygonValidator : IGeometryValidator
    {
        public bool IsValid(IGeometry geometry)
        {
            throw new NotImplementedException();
        }
        public IGeometry MakeValid(IGeometry geometry)
        {
            throw new NotImplementedException();
        }
    }


}
