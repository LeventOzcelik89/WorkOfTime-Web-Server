using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class Intranet1000 : InfolineTable
    {
        public string COLUMN_NAME { get; set;}
        public string IS_NULLABLE { get; set;}
        public string DATA_TYPE { get; set;}
        public int? CHARACTER_MAXIMUM_LENGTH { get; set;}
        public byte? NUMERIC_PRECISION { get; set;}
        public int? NUMERIC_SCALE { get; set;}
    }
}
