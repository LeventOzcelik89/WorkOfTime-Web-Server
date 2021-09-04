namespace Infoline.GIS
{
    interface IGeometryFile
    {
       GeometryObject ReadData();

       bool WriteData(GeometryObject geometryObject);
    }
}
