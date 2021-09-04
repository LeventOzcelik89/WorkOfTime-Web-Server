using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class VWINV_CompanyDepartmentsPageReport 
    {
        public int ToplamDepartman { get; set;}
        public int ToplamPozisyon { get; set;}
        public string SonEklenenDepartment { get; set;}
        public string DepartmanEnFazlaPozisyon { get; set;}
    }
}
