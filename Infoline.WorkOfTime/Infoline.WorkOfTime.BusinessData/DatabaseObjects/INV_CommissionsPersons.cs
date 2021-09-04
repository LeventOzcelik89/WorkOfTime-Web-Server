using System;
using GeoAPI.Geometries;

namespace Infoline.WorkOfTime.BusinessData
{
    public partial class INV_CommissionsPersons : InfolineTable
    {
        public bool? IsOwner { get; set;}
        public Guid? IdCommisions { get; set;}
        public Guid? IdUser { get; set;}
    }
}
